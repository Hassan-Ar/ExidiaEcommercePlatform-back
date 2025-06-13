using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePlatform.AiChat.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace EcommercePlatform.AiChat;

public class AiChatAppService :
    CrudAppService<
        AiChatSession,
        AiChatSessionDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAiChatSessionDto>,
    IAiChatAppService
{
    private readonly IRepository<AiChatSession, Guid> _sessionRepository;
    private readonly IRepository<AiChatMessage, Guid> _messageRepository;

    public AiChatAppService(
        IRepository<AiChatSession, Guid> sessionRepository,
        IRepository<AiChatMessage, Guid> messageRepository)
        : base(sessionRepository)
    {
        _sessionRepository = sessionRepository;
        _messageRepository = messageRepository;
    }

    public async Task<List<AiChatMessageDto>> GetMessagesBySessionAsync(Guid sessionId)
    {
        var messages = (await _messageRepository.GetQueryableAsync())
            .Where(m => m.SessionId == sessionId)
            .ToList();
        return ObjectMapper.Map<List<AiChatMessage>, List<AiChatMessageDto>>(messages);
    }

    public async Task<AiChatMessageDto> AddMessageToSessionAsync(Guid sessionId, CreateAiChatMessageDto input)
    {
        var session = await _sessionRepository.GetAsync(sessionId);

        var message = new AiChatMessage
        {
            SessionId = sessionId,
            Role = input.Role,
            Content = input.Content,
            Timestamp = DateTime.UtcNow
        };

        await _messageRepository.InsertAsync(message);

        return ObjectMapper.Map<AiChatMessage, AiChatMessageDto>(message);
    }
} 