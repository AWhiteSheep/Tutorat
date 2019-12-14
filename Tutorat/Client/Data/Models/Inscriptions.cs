using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class Inscriptions
    {
        [Required]
        [StringLength(450)]
        public string IdentifiantDemandeur { get; set; }
        public int IdentifiantHoraire { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AcceptedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(IdentifiantDemandeur))]
        [InverseProperty(nameof(AspNetUsers.Inscriptions))]
        public virtual AspNetUsers IdentifiantDemandeurNavigation { get; set; }
    }
}
