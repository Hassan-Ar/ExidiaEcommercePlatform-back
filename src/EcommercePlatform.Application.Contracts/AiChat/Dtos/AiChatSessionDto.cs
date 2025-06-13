using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace EcommercePlatform.AiChat.Dtos;

public class AiChatSessionDto : AuditedEntityDto<Guid>
{
    public string SessionName { get; set; }
    public Guid UserId { get; set; }
    public List<AiChatMessageDto> Messages { get; set; }
} 