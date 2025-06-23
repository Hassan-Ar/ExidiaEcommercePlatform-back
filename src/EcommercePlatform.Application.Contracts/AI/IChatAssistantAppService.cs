using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using EcommercePlatform.ChatAssistant.Dtos;

namespace EcommercePlatform.ChatAssistant;

public interface IChatAssistantAppService : IApplicationService
{
    /// <summary>
    /// Processes a user message and returns the assistant's reply along with product suggestions.
    /// </summary>
    Task<ChatAssistantResponseDto> ProcessUserMessageAsync(ChatAssistantRequestDto input);
} 