using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class Demandes
    {
        [Key]
        [Required]
        [StringLength(450)]
        public string IdentifiantUtilisateur { get; set; }
        [Key]
        public int IdentifiantHoraire { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateExpired { get; set; }
        public bool? Notified { get; set; }

        [ForeignKey(nameof(IdentifiantHoraire))]
        [InverseProperty(nameof(Horraire.Demandes))]
        public virtual Horraire IdentifiantHoraireNavigation { get; set; }
        [ForeignKey(nameof(IdentifiantUtilisateur))]
        [InverseProperty(nameof(AspNetUsers.Demandes))]
        public virtual AspNetUsers IdentifiantUtilisateurNavigation { get; set; }
    }
}
