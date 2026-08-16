using System.ComponentModel.DataAnnotations.Schema;

namespace AucaJobPortalDotNet.Models;
[Table("messages")]
public class Message
{
    [Column("id")]
    public int Id{get;private set;}
    [Column("sender_id")]
    public int SenderId{get;set;}
    [Column("receiver_id")]
    public int ReceiverId{get;set;}
    [Column("Application_id")]
    public int? ApplicationId{get;set;}
    [Column("subject")]
    public string? Subject{get;set;}
    [Column("content")]
    public string Content{get;set;}=string.Empty;
    [Column("is_read")]
    public int IsRead{get;set;}
    [Column("sent_at")]
    public DateTime SentAt{get;set;}=DateTime.UtcNow;
    public virtual Application? Application{get;set;}

    [ForeignKey("SenderId")]
    public virtual User Sender{get;set;}=null!;
    [ForeignKey("ReceiverId")]
    public virtual User Receiver{get;set;}=null!;


}