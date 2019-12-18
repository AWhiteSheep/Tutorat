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
            CommunicationFromUserNavigation = new HashSet<Communication>();
            CommunicationSendToNavigation = new HashSet<Communication>();
            Demandes = new HashSet<Demandes>();
            Inscriptions = new HashSet<Inscriptions>();
            NotificationsIdentifiantUtilisateurReceiverNavigation = new HashSet<Notifications>();
            NotificationsIdentifiantUtilisateurRelatedNavigation = new HashSet<Notifications>();
            Services = new HashSet<Services>();
        }

        [InverseProperty("Poster")]
        public virtual ICollection<Comments> Comments { get; set; }
        [InverseProperty(nameof(Communication.FromUserNavigation))]
        public virtual ICollection<Communication> CommunicationFromUserNavigation { get; set; }
        [InverseProperty(nameof(Communication.SendToNavigation))]
        public virtual ICollection<Communication> CommunicationSendToNavigation { get; set; }
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
