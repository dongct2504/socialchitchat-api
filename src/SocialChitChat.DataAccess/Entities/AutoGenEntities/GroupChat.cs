﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialChitChat.DataAccess.Entities.AutoGenEntities
{
    public partial class GroupChat
    {
        public Guid Id { get; set; }
        public bool IsGroupChat { get; set; }
        [StringLength(200)]
        public string? GroupName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
