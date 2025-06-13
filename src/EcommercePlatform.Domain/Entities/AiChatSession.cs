using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents an AI chat session in the e-commerce platform.
    /// </summary>
    public class AiChatSession : FullAuditedEntity<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this chat session belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The ID of the user who initiated the chat session, if any.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// A unique identifier for the session, used for tracking and reference.
        /// </summary>
        public string SessionIdentifier { get; set; }

        /// <summary>
        /// Indicates whether the session is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The last activity timestamp for this session.
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// Any metadata associated with this session (in JSON format).
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Creates a new AiChatSession instance.
        /// </summary>
        protected AiChatSession()
        {
        }

        /// <summary>
        /// Creates a new AiChatSession instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the chat session.</param>
        /// <param name="tenantId">The tenant ID that this chat session belongs to.</param>
        /// <param name="userId">The ID of the user who initiated the chat session, if any.</param>
        /// <param name="sessionIdentifier">A unique identifier for the session.</param>
        public AiChatSession(
            Guid id,
            Guid? tenantId,
            Guid? userId,
            string sessionIdentifier)
            : base(id)
        {
            TenantId = tenantId;
            UserId = userId;
            SessionIdentifier = sessionIdentifier;
            IsActive = true;
            LastActivityTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the last activity time for this session.
        /// </summary>
        public void UpdateLastActivityTime()
        {
            LastActivityTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Deactivates this chat session.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactivates this chat session.
        /// </summary>
        public void Reactivate()
        {
            IsActive = true;
            LastActivityTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the metadata for this session.
        /// </summary>
        /// <param name="metadata">The new metadata.</param>
        public void UpdateMetadata(string metadata)
        {
            Metadata = metadata;
        }
    }
}
