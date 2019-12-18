using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data.Models
{
    public partial class Inscriptions
    {
        [Key]
        public string IdentifiantDemandeur { get; set; }
        [Key]
        public int IdentifiantHoraire { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AcceptedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        public int? NotificationId { get; set; }

        [ForeignKey(nameof(IdentifiantDemandeur))]
        [InverseProperty(nameof(AspNetUsers.Inscriptions))]
        public virtual AspNetUsers IdentifiantDemandeurNavigation { get; set; }
        [ForeignKey(nameof(NotificationId))]
        [InverseProperty(nameof(Notifications.Inscriptions))]
        public virtual Notifications Notification { get; set; }
    }
}
