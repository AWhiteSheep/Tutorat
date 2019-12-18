using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data.Models
{
    public partial class Categories
    {
        public Categories()
        {
            ServiceCategorie = new HashSet<ServiceCategorie>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(450)]
        public string Name { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<ServiceCategorie> ServiceCategorie { get; set; }
    }
}
