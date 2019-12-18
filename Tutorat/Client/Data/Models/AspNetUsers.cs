using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.Data
{
    public partial class AspNetUsers : IdentityUser
    {
        public AspNetUsers()
        {
            Comments = new HashSet<Comments>();
            Demandes = new HashSet<Demandes>();
            Inscriptions = new HashSet<Inscriptions>();
            NotificationsIdentifiantUtilisateurReceiverNavigation = new HashSet<Notifications>();
            NotificationsIdentifiantUtilisateurRelatedNavigation = new HashSet<Notifications>();
            Services = new HashSet<Services>();
        }

        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        [InverseProperty("Poster")]
        public virtual ICollection<Comments> Comments { get; set; }
        [InverseProperty("IdentifiantUtilisateurNavigation")]
        public virtual ICollection<Demandes> Demandes { get; set; }
        [InverseProperty("IdentifiantDemandeurNavigation")]
        public virtual ICollection<Inscriptions> Inscriptions { get; set; }
        [InverseProperty(nameof(Notifications.IdentifiantUtilisateurReceiverNavigation))]
        public virtual ICollection<Notifications> NotificationsIdentifiantUtilisateurReceiverNavigation { get; set; }
        [InverseProperty(nameof(Notifications.IdentifiantUtilisateurRelatedNavigation))]
        public virtual ICollection<Notifications> NotificationsIdentifiantUtilisateurRelatedNavigation { get; set; }
        [InverseProperty("Tuteur")]
        public virtual ICollection<Services> Services { get; set; }
    }
}
