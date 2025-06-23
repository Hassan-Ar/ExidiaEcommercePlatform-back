using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Entities;
using EcommercePlatform.Enums;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// Domain service for managing AI chat sessions and messages.
    /// </summary>
    public class AiChatManager : DomainService, IAiChatManager
    {
        private readonly IRepository<AiChatSession, Guid> _chatSessionRepository;
        private readonly IRepository<AiChatMessage, Guid> _chatMessageRepository;

        /// <summary>
        /// Creates a new instance of AiChatManager.
        /// </summary>
        /// <param name="chatSessionRepository">The chat session repository.</param>
        /// <param name="chatMessageRepository">The chat message repository.</param>
        public AiChatManager(
            IRepository<AiChatSession, Guid> chatSessionRepository,
            IRepository<AiChatMessage, Guid> chatMessageRepository)
        {
            _chatSessionRepository = chatSessionRepository;
            _chatMessageRepository = chatMessageRepository;
        }

        /// <summary>
        /// Creates a new AI chat session.
        /// </summary>
        /// <param name="userId">The ID of the user who initiated the chat session, if any.</param>
        /// <returns>The newly created chat session.</returns>
        public async Task<AiChatSession> CreateSessionAsync(Guid? userId = null)
        {
            // Generate a unique session identifier
            var sessionIdentifier = Guid.NewGuid().ToString("N");

            // Create chat session
            var chatSession = new AiChatSession(
                GuidGenerator.Create(),
                userId,
                sessionIdentifier);

            return await _chatSessionRepository.InsertAsync(chatSession);
        }

        /// <summary>
        /// Adds a user message to a chat session.
        /// </summary>
        /// <param name="sessionId">The ID of the chat session.</param>
        /// <param name="message">The message content.</param>
        /// <returns>The newly created chat message.</returns>
        public async Task<AiChatMessage> AddUserMessageAsync(Guid sessionId, string message)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty.", nameof(message));
            }

            // Ensure session exists and is active
            var session = await _chatSessionRepository.GetAsync(sessionId);
            if (!session.IsActive)
            {
                throw new InvalidOperationException("Cannot add message to inactive chat session.");
            }

            // Create chat message
            var chatMessage = new AiChatMessage(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                sessionId,
                true, // isFromUser = true
                message);

            // Update session last activity time
            session.UpdateLastActivityTime();
            await _chatSessionRepository.UpdateAsync(session);

            return await _chatMessageRepository.InsertAsync(chatMessage);
        }

        /// <summary>
        /// Adds an AI response to a chat session.
        /// </summary>
        /// <param name="sessionId">The ID of the chat session.</param>
        /// <param name="message">The message content.</param>
        /// <param name="metadata">Any additional metadata associated with this message.</param>
        /// <returns>The newly created chat message.</returns>
        public async Task<AiChatMessage> AddAiResponseAsync(Guid sessionId, string message, string metadata = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty.", nameof(message));
            }

            // Ensure session exists and is active
            var session = await _chatSessionRepository.GetAsync(sessionId);
            if (!session.IsActive)
            {
                throw new InvalidOperationException("Cannot add message to inactive chat session.");
            }

            // Create chat message
            var chatMessage = new AiChatMessage(
                GuidGenerator.Create(),
                CurrentTenant.Id,
                sessionId,
                false, // isFromUser = false
                message);

            if (!string.IsNullOrWhiteSpace(metadata))
            {
                chatMessage.UpdateMetadata(metadata);
            }

            // Update session last activity time
            session.UpdateLastActivityTime();
            await _chatSessionRepository.UpdateAsync(session);

            return await _chatMessageRepository.InsertAsync(chatMessage);
        }

        /// <summary>
        /// Deactivates a chat session.
        /// </summary>
        /// <param name="sessionId">The ID of the chat session to deactivate.</param>
        /// <returns>The deactivated chat session.</returns>
        public async Task<AiChatSession> DeactivateSessionAsync(Guid sessionId)
        {
            var session = await _chatSessionRepository.GetAsync(sessionId);
            session.Deactivate();
            return await _chatSessionRepository.UpdateAsync(session);
        }

        /// <summary>
        /// Reactivates a chat session.
        /// </summary>
        /// <param name="sessionId">The ID of the chat session to reactivate.</param>
        /// <returns>The reactivated chat session.</returns>
        public async Task<AiChatSession> ReactivateSessionAsync(Guid sessionId)
        {
            var session = await _chatSessionRepository.GetAsync(sessionId);
            session.Reactivate();
            return await _chatSessionRepository.UpdateAsync(session);
        }

        /// <summary>
        /// Updates the metadata for a chat session.
        /// </summary>
        /// <param name="sessionId">The ID of the chat session.</param>
        /// <param name="metadata">The new metadata.</param>
        /// <returns>The updated chat session.</returns>
        public async Task<AiChatSession> UpdateSessionMetadataAsync(Guid sessionId, string metadata)
        {
            var session = await _chatSessionRepository.GetAsync(sessionId);
            session.UpdateMetadata(metadata);
            return await _chatSessionRepository.UpdateAsync(session);
        }
    }

    /// <summary>
    /// Interface for the AI chat manager domain service.
    /// </summary>
    public interface IAiChatManager : IDomainService
    {
        /// <summary>
        /// Creates a new AI chat session.
        /// </summary>
        Task<AiChatSession> CreateSessionAsync(Guid? userId = null);

        /// <summary>
        /// Adds a user message to a chat session.
        /// </summary>
        Task<AiChatMessage> AddUserMessageAsync(Guid sessionId, string message);

        /// <summary>
        /// Adds an AI response to a chat session.
        /// </summary>
        Task<AiChatMessage> AddAiResponseAsync(Guid sessionId, string message, string metadata = null);

        /// <summary>
        /// Deactivates a chat session.
        /// </summary>
        Task<AiChatSession> DeactivateSessionAsync(Guid sessionId);

        /// <summary>
        /// Reactivates a chat session.
        /// </summary>
        Task<AiChatSession> ReactivateSessionAsync(Guid sessionId);

        /// <summary>
        /// Updates the metadata for a chat session.
        /// </summary>
        Task<AiChatSession> UpdateSessionMetadataAsync(Guid sessionId, string metadata);
    }
}
