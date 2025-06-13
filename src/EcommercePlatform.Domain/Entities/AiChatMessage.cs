using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace EcommercePlatform.Entities
{
    /// <summary>
    /// Represents an AI chat message in the e-commerce platform.
    /// </summary>
    public class AiChatMessage : AuditedEntity<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID that this chat message belongs to.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// The ID of the chat session that this message belongs to.
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Indicates whether this message is from the user (true) or the AI (false).
        /// </summary>
        public bool IsFromUser { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The timestamp when this message was sent.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Any additional metadata associated with this message (in JSON format).
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Creates a new AiChatMessage instance.
        /// </summary>
        protected AiChatMessage()
        {
        }

        /// <summary>
        /// Creates a new AiChatMessage instance with the specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the chat message.</param>
        /// <param name="tenantId">The tenant ID that this chat message belongs to.</param>
        /// <param name="sessionId">The ID of the chat session that this message belongs to.</param>
        /// <param name="isFromUser">Indicates whether this message is from the user.</param>
        /// <param name="message">The content of the message.</param>
        public AiChatMessage(
            Guid id,
            Guid? tenantId,
            Guid sessionId,
            bool isFromUser,
            string message)
            : base(id)
        {
            TenantId = tenantId;
            SessionId = sessionId;
            IsFromUser = isFromUser;
            Message = message;
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the metadata for this message.
        /// </summary>
        /// <param name="metadata">The new metadata.</param>
        public void UpdateMetadata(string metadata)
        {
            Metadata = metadata;
        }
    }
}
