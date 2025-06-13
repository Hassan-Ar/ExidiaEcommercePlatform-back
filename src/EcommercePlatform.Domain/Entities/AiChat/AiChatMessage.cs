using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.AiChat;

public class AiChatMessage : FullAuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid SessionId { get; set; }
    public AiChatSession Session { get; set; }
    public string Role { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string MessageType { get; set; }
    public string Metadata { get; set; }

    public AiChatMessage()
    {
    }

    public AiChatMessage(
        Guid id,
        Guid sessionId,
        string role,
        string content,
        string messageType,
        string metadata,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        SessionId = sessionId;
        Role = role;
        Content = content;
        Timestamp = DateTime.UtcNow;
        MessageType = messageType;
        Metadata = metadata;
    }

    public void UpdateContent(string newContent)
    {
        Content = newContent;
    }

    public void UpdateMetadata(string newMetadata)
    {
        Metadata = newMetadata;
    }
} 