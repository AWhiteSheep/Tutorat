using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class ServiceCategorie
    {
        [Key]
        public int ServiceId { get; set; }
        [Key]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(Categories.ServiceCategorie))]
        public virtual Categories Category { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Services.ServiceCategorie))]
        public virtual Services Service { get; set; }
    }
}
