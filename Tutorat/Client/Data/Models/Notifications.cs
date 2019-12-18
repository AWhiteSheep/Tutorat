using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data.Models
{
    public partial class Notifications
    {
        public Notifications()
        {
            Demandes = new HashSet<Demandes>();
            Inscriptions = new HashSet<Inscriptions>();
        }

        [Key]
        public int IdentityKey { get; set; }
        [Required]
        [StringLength(450)]
        public string IdentifiantUtilisateurReceiver { get; set; }
        [StringLength(450)]
        public string IdentifiantUtilisateurRelated { get; set; }
        [Column("_IdentityKeyEntityRelated")]
        public string IdentityKeyEntityRelated { get; set; }
        [Column("_TypeEntity")]
        public string TypeEntity { get; set; }
        [Required]
        [StringLength(100)]
        public string NotificationName { get; set; }
        public bool Seen { get; set; }
        public TimeSpan? SeenAt { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Message { get; set; }

        [ForeignKey(nameof(IdentifiantUtilisateurReceiver))]
        [InverseProperty(nameof(AspNetUsers.NotificationsIdentifiantUtilisateurReceiverNavigation))]
        public virtual AspNetUsers IdentifiantUtilisateurReceiverNavigation { get; set; }
        [ForeignKey(nameof(IdentifiantUtilisateurRelated))]
        [InverseProperty(nameof(AspNetUsers.NotificationsIdentifiantUtilisateurRelatedNavigation))]
        public virtual AspNetUsers IdentifiantUtilisateurRelatedNavigation { get; set; }
        [InverseProperty("Notification")]
        public virtual ICollection<Demandes> Demandes { get; set; }
        [InverseProperty("Notification")]
        public virtual ICollection<Inscriptions> Inscriptions { get; set; }
    }
}
