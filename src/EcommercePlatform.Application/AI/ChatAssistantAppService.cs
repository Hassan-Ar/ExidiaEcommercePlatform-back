using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePlatform.ChatAssistant.Dtos;
using EcommercePlatform.Products.Dtos;
using Volo.Abp.Application.Services;

namespace EcommercePlatform.ChatAssistant;

public class ChatAssistantAppService : ApplicationService, IChatAssistantAppService
{
    private readonly IProductSearchService _productSearchService;
    private readonly OllamaLLMClient _llmClient;

    public ChatAssistantAppService(
        IProductSearchService productSearchService,
        OllamaLLMClient llmClient)
    {
        _productSearchService = productSearchService;
        _llmClient = llmClient;
    }

    public async Task<ChatAssistantResponseDto> ProcessUserMessageAsync(ChatAssistantRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Message))
        {
            throw new ArgumentException("Message cannot be empty", nameof(input.Message));
        }

        // For simplified testing we skip chat-session persistence and just generate response.
        Guid sessionId = Guid.NewGuid(); // dummy id for response

        // Find product matches
        var products = await _productSearchService.SearchProductsAsync(input.Message, 5);

        // Build product summaries for prompt & DTO
        var productBriefs = products.Select(p => new ProductBriefDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            ProductPageUrl = $"/product/{p.Id}"
        }).ToList();

        string assistantReply;
        if (productBriefs.Any())
        {
            var productListForPrompt = string.Join("\n", productBriefs.Select(pb => $"- {pb.Name}: {pb.Description} (Price: {pb.Price:C}) Link: {pb.ProductPageUrl}"));

            var prompt = $@"You are a helpful AI shopping assistant.
User query: ""{input.Message}""
Based on the following product list, recommend the best matches. Speak in a friendly marketing tone and include the product name and a short enticing description:
{productListForPrompt}\n";

            try
            {
                assistantReply = await _llmClient.GenerateResponseAsync(prompt);
            }
            catch
            {
                // Fallback if Ollama is not available
                assistantReply = "Here are some products you might like:" + "\n" + string.Join("\n", productBriefs.Select(pb => $"• {pb.Name}"));
            }
        }
        else
        {
            assistantReply = "Sorry, we couldn't find any matching products at the moment.";
        }

        // Skipping persistence for simple mode.

        return new ChatAssistantResponseDto
        {
            SessionId = sessionId,
            UserMessage = input.Message,
            AssistantMessage = assistantReply,
            Products = productBriefs
        };
    }
} 