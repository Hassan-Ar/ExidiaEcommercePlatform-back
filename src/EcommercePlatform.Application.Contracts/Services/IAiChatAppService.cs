using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EcommercePlatform.Application.Contracts.Services
{
    /// <summary>
    /// Interface for the AI chat application service.
    /// </summary>
    public interface IAiChatAppService : IApplicationService
    {
        /// <summary>
        /// Gets a chat session by ID.
        /// </summary>
        Task<AiChatSessionDto> GetSessionAsync(Guid id);

        /// <summary>
        /// Gets a list of chat sessions for a user.
        /// </summary>
        Task<List<AiChatSessionDto>> GetSessionsByUserAsync(Guid userId);

        /// <summary>
        /// Gets a paged list of chat sessions.
        /// </summary>
        Task<PagedResultDto<AiChatSessionDto>> GetPagedSessionsAsync(PagedAndSortedResultRequestDto input);

        /// <summary>
        /// Gets a list of messages for a chat session.
        /// </summary>
        Task<List<AiChatMessageDto>> GetSessionMessagesAsync(Guid sessionId);

        /// <summary>
        /// Creates a new chat session.
        /// </summary>
        Task<AiChatSessionDto> CreateSessionAsync(CreateAiChatSessionDto input);

        /// <summary>
        /// Adds a user message to a chat session.
        /// </summary>
        Task<AiChatMessageDto> AddUserMessageAsync(Guid sessionId, AddAiChatMessageDto input);

        /// <summary>
        /// Adds an AI response to a chat session.
        /// </summary>
        Task<AiChatMessageDto> AddAiResponseAsync(Guid sessionId, AddAiChatResponseDto input);

        /// <summary>
        /// Deactivates a chat session.
        /// </summary>
        Task<AiChatSessionDto> DeactivateSessionAsync(Guid id);

        /// <summary>
        /// Reactivates a chat session.
        /// </summary>
        Task<AiChatSessionDto> ReactivateSessionAsync(Guid id);

        /// <summary>
        /// Updates the metadata for a chat session.
        /// </summary>
        Task<AiChatSessionDto> UpdateSessionMetadataAsync(Guid id, UpdateAiChatSessionMetadataDto input);
    }

    /// <summary>
    /// DTO for AI chat session data.
    /// </summary>
    public class AiChatSessionDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the user who initiated the chat session, if any.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// A unique identifier for the session.
        /// </summary>
        public string SessionIdentifier { get; set; }

        /// <summary>
        /// Indicates whether the session is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The time of the last activity in the session.
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// Any additional metadata associated with this session.
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// The creation time of the session.
        /// </summary>
        public DateTime CreationTime { get; set; }
    }

    /// <summary>
    /// DTO for AI chat message data.
    /// </summary>
    public class AiChatMessageDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the chat session this message belongs to.
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Indicates whether the message is from the user (true) or AI (false).
        /// </summary>
        public bool IsFromUser { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Any additional metadata associated with the message.
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// The creation time of the message.
        /// </summary>
        public DateTime CreationTime { get; set; }
    }

    /// <summary>
    /// DTO for creating an AI chat session.
    /// </summary>
    public class CreateAiChatSessionDto
    {
        /// <summary>
        /// The ID of the user who initiated the chat session, if any.
        /// </summary>
        public Guid? UserId { get; set; }
    }

    /// <summary>
    /// DTO for adding an AI chat message.
    /// </summary>
    public class AddAiChatMessageDto
    {
        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// DTO for adding an AI chat response.
    /// </summary>
    public class AddAiChatResponseDto
    {
        /// <summary>
        /// The content of the AI response.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Any additional metadata associated with the AI response.
        /// </summary>
        public string Metadata { get; set; }
    }

    /// <summary>
    /// DTO for updating AI chat session metadata.
    /// </summary>
    public class UpdateAiChatSessionMetadataDto
    {
        /// <summary>
        /// The new metadata for the session.
        /// </summary>
        public string Metadata { get; set; }
    }
} 