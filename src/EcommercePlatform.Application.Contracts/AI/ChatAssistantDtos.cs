namespace EcommercePlatform.ChatAssistant.Dtos;

using System;
using System.Collections.Generic;

public class ChatAssistantRequestDto
{
    /// <summary>
    /// Optional existing chat session ID. If null, a new session will be created.
    /// </summary>
    public Guid? SessionId { get; set; }

    /// <summary>
    /// The ID of the user sending the message (optional for anonymous).
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// The natural-language message typed by the user.
    /// </summary>
    public string Message { get; set; }
}

public class ProductBriefDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string ProductPageUrl { get; set; }
}

public class ChatAssistantResponseDto
{
    /// <summary>
    /// The chat session ID that this response belongs to.
    /// </summary>
    public Guid SessionId { get; set; }

    /// <summary>
    /// Echo of the user message.
    /// </summary>
    public string UserMessage { get; set; }

    /// <summary>
    /// The assistant's reply generated via Ollama.
    /// </summary>
    public string AssistantMessage { get; set; }

    /// <summary>
    /// List of product suggestions.
    /// </summary>
    public List<ProductBriefDto> Products { get; set; } = new();
}

public class SimpleChatRequestDto
{
    public string Message { get; set; }
} 