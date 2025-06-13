using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.AiChat.Dtos;

public class CreateAiChatMessageDto
{
    [Required]
    public Guid SessionId { get; set; }

    [Required]
    [StringLength(64)]
    public string Role { get; set; }

    [Required]
    [StringLength(4000)]
    public string Content { get; set; }
} 