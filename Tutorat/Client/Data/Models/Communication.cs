using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class Communication
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public int OrderId { get; set; }
        [Required]
        [StringLength(450)]
        public string FromUser { get; set; }
        [Required]
        [StringLength(450)]
        public string SendTo { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Text { get; set; }

        [ForeignKey(nameof(FromUser))]
        [InverseProperty(nameof(AspNetUsers.CommunicationFromUserNavigation))]
        public virtual AspNetUsers FromUserNavigation { get; set; }
        [ForeignKey(nameof(SendTo))]
        [InverseProperty(nameof(AspNetUsers.CommunicationSendToNavigation))]
        public virtual AspNetUsers SendToNavigation { get; set; }
    }
}
