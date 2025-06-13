using System;
using Volo.Abp.Application.Dtos;

namespace EcommercePlatform.AiChat.Dtos;

public class AiChatMessageDto : EntityDto<Guid>
{
    public Guid SessionId { get; set; }
    public string Role { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
} 