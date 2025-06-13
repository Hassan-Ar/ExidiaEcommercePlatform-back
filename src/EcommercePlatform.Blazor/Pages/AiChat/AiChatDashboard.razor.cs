using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Application.Services;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;

namespace EcommercePlatform.Blazor.Pages.AiChat
{
    public partial class AiChatDashboard : AbpComponentBase
    {
        [Inject]
        protected IAiChatAppService AiChatAppService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        private List<AiChatSessionDto> ActiveSessions { get; set; } = new List<AiChatSessionDto>();
        private Guid? CurrentSessionId { get; set; }
        private List<AiChatMessageDto> CurrentSessionMessages { get; set; } = new List<AiChatMessageDto>();
        private string NewMessage { get; set; }
        private bool IsProcessing { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadActiveSessionsAsync();
            
            // Create a new session if none exists
            if (ActiveSessions.Count == 0)
            {
                await CreateNewSessionAsync();
            }
            else
            {
                // Select the most recent session
                CurrentSessionId = ActiveSessions[0].Id;
                await LoadSessionMessagesAsync();
            }
        }

        private async Task LoadActiveSessionsAsync()
        {
            var sessions = await AiChatAppService.GetPagedSessionsAsync(new Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto
            {
                MaxResultCount = 10,
                SkipCount = 0,
                Sorting = "LastActivityTime DESC"
            });
            
            ActiveSessions = sessions.Items;
        }

        private async Task CreateNewSessionAsync()
        {
            var session = await AiChatAppService.CreateSessionAsync(new CreateAiChatSessionDto
            {
                UserId = CurrentUser.Id
            });
            
            CurrentSessionId = session.Id;
            await LoadActiveSessionsAsync();
            CurrentSessionMessages.Clear();
            
            // Add welcome message
            await AddAiWelcomeMessageAsync();
        }

        private async Task LoadSessionMessagesAsync()
        {
            if (CurrentSessionId.HasValue)
            {
                CurrentSessionMessages = await AiChatAppService.GetSessionMessagesAsync(CurrentSessionId.Value);
            }
        }

        private async Task SelectSessionAsync(Guid sessionId)
        {
            CurrentSessionId = sessionId;
            await LoadSessionMessagesAsync();
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessage) || !CurrentSessionId.HasValue || IsProcessing)
            {
                return;
            }

            IsProcessing = true;
            
            try
            {
                // Add user message
                var userMessage = await AiChatAppService.AddUserMessageAsync(CurrentSessionId.Value, new AddAiChatMessageDto
                {
                    Content = NewMessage
                });
                
                CurrentSessionMessages.Add(userMessage);
                NewMessage = string.Empty;
                
                // Simulate AI processing
                await Task.Delay(1000);
                
                // Generate AI response
                await GenerateAiResponseAsync(userMessage.Content);
                
                // Refresh sessions list to update last activity time
                await LoadActiveSessionsAsync();
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private async Task GenerateAiResponseAsync(string userMessage)
        {
            // In a real implementation, this would call an AI service
            // For now, we'll simulate a response
            string aiResponse = GenerateSimulatedAiResponse(userMessage);
            
            var response = await AiChatAppService.AddAiResponseAsync(CurrentSessionId.Value, new AddAiChatResponseDto
            {
                Content = aiResponse,
                Metadata = "{\"confidence\": 0.95, \"processing_time\": 0.5}"
            });
            
            CurrentSessionMessages.Add(response);
        }

        private string GenerateSimulatedAiResponse(string userMessage)
        {
            // Simple response generation based on user message content
            userMessage = userMessage.ToLower();
            
            if (userMessage.Contains("hello") || userMessage.Contains("hi"))
            {
                return "Hello! How can I assist you with your shopping today?";
            }
            else if (userMessage.Contains("product") && (userMessage.Contains("recommend") || userMessage.Contains("suggestion")))
            {
                return "Based on your shopping history and preferences, I'd recommend checking out our new premium collection. Would you like me to show you some specific items?";
            }
            else if (userMessage.Contains("price") || userMessage.Contains("discount") || userMessage.Contains("deal"))
            {
                return "We currently have several promotions running! Our summer collection is 20% off, and there's a buy-one-get-one deal on selected items. Would you like me to show you the discounted products?";
            }
            else if (userMessage.Contains("order") && (userMessage.Contains("track") || userMessage.Contains("status")))
            {
                return "I'd be happy to help you track your order. Could you please provide your order number?";
            }
            else if (userMessage.Contains("return") || userMessage.Contains("refund"))
            {
                return "Our return policy allows returns within 30 days of purchase. Would you like me to guide you through the return process?";
            }
            else if (userMessage.Contains("thank"))
            {
                return "You're welcome! Is there anything else I can help you with today?";
            }
            else
            {
                return "I understand you're interested in learning more. Could you please provide more details about what you're looking for, and I'll do my best to assist you?";
            }
        }

        private async Task AddAiWelcomeMessageAsync()
        {
            var response = await AiChatAppService.AddAiResponseAsync(CurrentSessionId.Value, new AddAiChatResponseDto
            {
                Content = "Hello! I'm your AI shopping assistant. How can I help you today? I can recommend products, answer questions about your orders, or help you find what you're looking for.",
                Metadata = "{\"type\": \"welcome\"}"
            });
            
            CurrentSessionMessages.Add(response);
        }

        private async Task EndSessionAsync()
        {
            if (CurrentSessionId.HasValue)
            {
                await AiChatAppService.DeactivateSessionAsync(CurrentSessionId.Value);
                await LoadActiveSessionsAsync();
                
                if (ActiveSessions.Count > 0)
                {
                    CurrentSessionId = ActiveSessions[0].Id;
                    await LoadSessionMessagesAsync();
                }
                else
                {
                    await CreateNewSessionAsync();
                }
            }
        }

        private string GetMessageClass(bool isFromUser)
        {
            return isFromUser ? "user-message" : "ai-message";
        }

        private string GetFormattedTime(DateTime time)
        {
            return time.ToString("HH:mm");
        }
    }
}
