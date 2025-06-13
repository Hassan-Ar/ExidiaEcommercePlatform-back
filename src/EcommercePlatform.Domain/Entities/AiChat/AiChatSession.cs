using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.AiChat;

public class AiChatSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public string SessionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; }
    public string Context { get; set; }
    public ICollection<AiChatMessage> Messages { get; set; }

    protected AiChatSession()
    {
        Messages = new List<AiChatMessage>();
    }

    public AiChatSession(
        Guid id,
        Guid customerId,
        string sessionId,
        string context,
        Guid? tenantId = null
    ) : base(id)
    {
        TenantId = tenantId;
        CustomerId = customerId;
        SessionId = sessionId;
        StartTime = DateTime.UtcNow;
        Status = "Active";
        Context = context;
        Messages = new List<AiChatMessage>();
    }

    public void AddMessage(AiChatMessage message)
    {
        Messages.Add(message);
    }

    public void EndSession()
    {
        if (Status == "Ended")
        {
            throw new InvalidOperationException("Session is already ended");
        }

        Status = "Ended";
        EndTime = DateTime.UtcNow;
    }

    public void UpdateContext(string newContext)
    {
        Context = newContext;
    }
} 