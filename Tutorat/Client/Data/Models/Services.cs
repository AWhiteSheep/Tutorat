using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class Services
    {
        public Services()
        {
            Horraire = new HashSet<Horraire>();
            ServiceCategorie = new HashSet<ServiceCategorie>();
        }

        [Key]
        public int IdentityKey { get; set; }
        [Required]
        [StringLength(450)]
        public string TuteurId { get; set; }
        [Required]
        public string Titre { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Description { get; set; }

        [ForeignKey(nameof(TuteurId))]
        [InverseProperty(nameof(AspNetUsers.Services))]
        public virtual AspNetUsers Tuteur { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<Horraire> Horraire { get; set; }
        [InverseProperty("Service")]
        public virtual ICollection<ServiceCategorie> ServiceCategorie { get; set; }
    }
}
