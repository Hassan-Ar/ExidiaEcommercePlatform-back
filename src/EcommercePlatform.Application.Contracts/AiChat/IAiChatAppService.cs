using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using EcommercePlatform.AiChat.Dtos;

namespace EcommercePlatform.AiChat;

public interface IAiChatAppService :
    ICrudAppService<
        AiChatSessionDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAiChatSessionDto>
{
    Task<List<AiChatMessageDto>> GetMessagesBySessionAsync(Guid sessionId);
    Task<AiChatMessageDto> AddMessageToSessionAsync(Guid sessionId, CreateAiChatMessageDto input);
} 