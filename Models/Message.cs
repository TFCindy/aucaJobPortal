using System;
using System.Collections.Generic;

namespace AucaJobPortalDotNet.Models;

public partial class Message
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public int? ApplicationId { get; set; }

    public string? Subject { get; set; }

    public string Content { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime? SentAt { get; set; }

    public virtual Application? Application { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
