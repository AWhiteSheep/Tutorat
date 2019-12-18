using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class Horraire
    {
        public Horraire()
        {
            Demandes = new HashSet<Demandes>();
        }

        [Key]
        public int IdentityKey { get; set; }
        public int ServiceId { get; set; }
        public int Jour { get; set; }        
        public TimeSpan HeureDebut { get; set; }
        public int NbHeure { get; set; }
        public int NbMinute { get; set; }
        public int EleveMaxInscription { get; set; }

        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Services.Horraire))]
        public virtual Services Service { get; set; }
        [InverseProperty("IdentifiantHoraireNavigation")]
        public virtual ICollection<Demandes> Demandes { get; set; }
    }
}
