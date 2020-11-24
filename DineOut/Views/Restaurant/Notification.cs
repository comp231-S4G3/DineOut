using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DineOut.Models
{
    public class Notification
    {
        [Key]
        [Column("notify_id")]
        public int NotifyId { get; set; }
        [Column("send_time")]
        public DateTime SendTime { get; set; }
        [Column("subject")]
        public string Subject { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("failed_info")]
        public string FailedInfo { get; set; }
    }
}