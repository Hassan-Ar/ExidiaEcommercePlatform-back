using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.AiChat.Dtos;

public class CreateUpdateAiChatSessionDto
{
    [Required]
    [StringLength(256)]
    public string SessionName { get; set; }

    [Required]
    public Guid UserId { get; set; }
} 