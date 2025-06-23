using System.Threading.Tasks;
using EcommercePlatform.ChatAssistant;
using EcommercePlatform.ChatAssistant.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePlatform.Controllers;

[Route("api/app/chat-assistant")]
[AllowAnonymous]
public class ChatAssistantController : EcommercePlatformController
{
    private readonly IChatAssistantAppService _chatAssistantAppService;

    public ChatAssistantController(IChatAssistantAppService chatAssistantAppService)
    {
        _chatAssistantAppService = chatAssistantAppService;
    }

    [HttpPost("process")]
    public Task<ChatAssistantResponseDto> ProcessAsync([FromBody] ChatAssistantRequestDto input)
    {
        return _chatAssistantAppService.ProcessUserMessageAsync(input);
    }

    [HttpPost("process-simple")]
    public Task<ChatAssistantResponseDto> ProcessSimpleAsync([FromBody] EcommercePlatform.ChatAssistant.Dtos.SimpleChatRequestDto request)
    {
        var dto = new EcommercePlatform.ChatAssistant.Dtos.ChatAssistantRequestDto
        {
            Message = request.Message
        };
        return _chatAssistantAppService.ProcessUserMessageAsync(dto);
    }
} 