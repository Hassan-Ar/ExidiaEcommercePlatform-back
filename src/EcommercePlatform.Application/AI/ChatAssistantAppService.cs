using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePlatform.ChatAssistant.Dtos;
using EcommercePlatform.Products.Dtos;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Application.Services;
using EcommercePlatform.AiChat;
using Volo.Abp.Domain.Repositories;
using System.Text.Json;

namespace EcommercePlatform.ChatAssistant;

public class ChatAssistantAppService : ApplicationService, IChatAssistantAppService
{
    private readonly IProductSearchService _productSearchService;
    private readonly OllamaLLMClient _llmClient;
    private readonly IConfiguration _configuration;
    private readonly IRepository<AiChatSession, Guid> _sessionRepository;
    private readonly IRepository<AiChatMessage, Guid> _messageRepository;

    public ChatAssistantAppService(
        IProductSearchService productSearchService,
        OllamaLLMClient llmClient,
        IConfiguration configuration,
        IRepository<AiChatSession, Guid> sessionRepository,
        IRepository<AiChatMessage, Guid> messageRepository)
    {
        _productSearchService = productSearchService;
        _llmClient = llmClient;
        _configuration = configuration;
        _sessionRepository = sessionRepository;
        _messageRepository = messageRepository;
    }

    public async Task<ChatAssistantResponseDto> ProcessUserMessageAsync(ChatAssistantRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Message))
        {
            throw new ArgumentException("Message cannot be empty", nameof(input.Message));
        }

        // Determine or create session
        Guid sessionId;
        AiChatSession sessionEntity;
        if (input.SessionId.HasValue)
        {
            sessionId = input.SessionId.Value;
            sessionEntity = await _sessionRepository.GetAsync(sessionId);
        }
        else
        {
            sessionEntity = new AiChatSession(
                GuidGenerator.Create(),
                input.UserId ?? Guid.Empty,
                Guid.NewGuid().ToString("N"),
                context: string.Empty);
            sessionEntity.Status = "Active";
            sessionEntity.StartTime = DateTime.UtcNow;
            await _sessionRepository.InsertAsync(sessionEntity, autoSave: true);
            sessionId = sessionEntity.Id;
        }

        // Save user message
        var userMessage = new AiChatMessage(
            GuidGenerator.Create(),
            sessionId,
            role: "user",
            content: input.Message,
            messageType: "text",
            metadata: "");
        await _messageRepository.InsertAsync(userMessage, autoSave: true);

        // Retrieve conversation history (last 10 messages)
        var previousMessages = (await _messageRepository.GetQueryableAsync())
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.Timestamp).ToList();
        previousMessages = previousMessages
            .TakeLast(10)
            .ToList();

        // Build conversational prompt
        var conversationPromptBuilder = new System.Text.StringBuilder();
        conversationPromptBuilder.AppendLine("You are a helpful AI shopping assistant.");
        foreach (var msg in previousMessages)
        {
            var prefix = msg.Role == "user" ? "User:" : "Assistant:";
            conversationPromptBuilder.AppendLine($"{prefix} {msg.Content}");
        }

        // Try to leverage previous assistant suggestion list for follow-up queries
        List<ProductBriefDto> previousSuggestions = null;
        var lastAssistantMsg = previousMessages.LastOrDefault(m => m.Role == "assistant" && !string.IsNullOrWhiteSpace(m.Metadata));
        if (lastAssistantMsg != null)
        {
            try
            {
                previousSuggestions = JsonSerializer.Deserialize<List<ProductBriefDto>>(lastAssistantMsg.Metadata);
            }
            catch { /* ignore bad json */ }
        }

        // Default search across catalogue
        var products = await _productSearchService.SearchProductsAsync(input.Message, 5);

        // If user likely referring to earlier suggestion, narrow to that list
        if (previousSuggestions != null && previousSuggestions.Count > 0)
        {
            var tokens = input.Message.Split(new[] {' ', ',', '.', ';', '\\', '/', '-', '_'}, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(t => t.Trim().ToLowerInvariant()).ToArray();

            var matchedPrev = previousSuggestions.Where(ps => tokens.All(t => ps.Name.ToLowerInvariant().Contains(t))).ToList();

            if (matchedPrev.Count == 0 && tokens.Any(t => t is "it" or "its" or "price" or "cost"))
            {
                // Pronoun-based follow-up, assume first suggestion
                matchedPrev = new List<ProductBriefDto> { previousSuggestions.First() };
            }

            if (matchedPrev.Count > 0)
            {
                // Override product search results with matched previous suggestion(s)
                products = matchedPrev.Select(mp => new ProductDto
                {
                    Id = mp.Id,
                    Name = mp.Name,
                    Description = mp.Description,
                    Price = mp.Price,
                    ImageUrl = mp.ImageUrl,
                    CategoryId = Guid.Empty,
                    ShopId = Guid.Empty,
                    StockQuantity = 0,
                    SKU = string.Empty,
                    IsActive = true
                }).ToList();
            }
        }

        var baseUrl = _configuration["App:ClientUrl"];

        // Build product summaries for prompt & DTO
        var productBriefs = products.Select(p => new ProductBriefDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            ProductPageUrl = $"{baseUrl}/store/product/{p.Id}"
        }).ToList();

        string assistantReply;
        if (productBriefs.Any())
        {
            var productListForPrompt = string.Join("\n", productBriefs.Select(pb => $"- {pb.Name}: {pb.Description} (Price: {pb.Price:C}) Link: {pb.ProductPageUrl}"));

            conversationPromptBuilder.AppendLine("---");
            conversationPromptBuilder.AppendLine("Relevant products:");
            conversationPromptBuilder.AppendLine(productListForPrompt);

            var prompt = conversationPromptBuilder.ToString();

            try
            {
                assistantReply = await _llmClient.GenerateResponseAsync(prompt);
            }
            catch
            {
                // Even if no products matched, still ask the model to respond contextually
                var promptNoProducts = conversationPromptBuilder.ToString();
                try
                {
                    assistantReply = await _llmClient.GenerateResponseAsync(promptNoProducts);
                }
                catch
                {
                    assistantReply = "Sorry, we couldn't find any matching products at the moment.";
                }
            }
        }
        else
        {
            assistantReply = "Sorry, we couldn't find any matching products at the moment.";
        }

        // Save AI response
        var aiMessage = new AiChatMessage(
            GuidGenerator.Create(),
            sessionId,
            role: "assistant",
            content: assistantReply,
            messageType: "text",
            metadata: System.Text.Json.JsonSerializer.Serialize(productBriefs));
        await _messageRepository.InsertAsync(aiMessage, autoSave: true);

        return new ChatAssistantResponseDto
        {
            SessionId = sessionId,
            UserMessage = input.Message,
            AssistantMessage = assistantReply,
            Products = productBriefs
        };
    }
} 