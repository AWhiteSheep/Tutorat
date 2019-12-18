using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data.Models
{
    public partial class Demandes
    {
        [Key]
        public string IdentifiantUtilisateur { get; set; }
        [Key]
        public int IdentifiantHoraire { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateExpired { get; set; }
        public bool? Notified { get; set; }
        public int? NotificationId { get; set; }

        [ForeignKey(nameof(IdentifiantHoraire))]
        [InverseProperty(nameof(Horraire.Demandes))]
        public virtual Horraire IdentifiantHoraireNavigation { get; set; }
        [ForeignKey(nameof(IdentifiantUtilisateur))]
        [InverseProperty(nameof(AspNetUsers.Demandes))]
        public virtual AspNetUsers IdentifiantUtilisateurNavigation { get; set; }
        [ForeignKey(nameof(NotificationId))]
        [InverseProperty(nameof(Notifications.Demandes))]
        public virtual Notifications Notification { get; set; }
    }
}
