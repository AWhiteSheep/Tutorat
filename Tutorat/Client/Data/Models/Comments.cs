using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class Comments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(450)]
        public string PosterId { get; set; }
        public int ServiceId { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string CommentText { get; set; }
        [Required]
        public DateTime PostedDateTime { get; set; }

        [ForeignKey(nameof(PosterId))]
        [InverseProperty(nameof(AspNetUsers.Comments))]
        public virtual AspNetUsers Poster { get; set; }
        [ForeignKey(nameof(ServiceId))]
        [InverseProperty(nameof(Services.Comments))]
        public virtual Services Service { get; set; }
    }
}
