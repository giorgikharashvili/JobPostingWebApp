using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;



namespace DevSpot.Models
{
    public class JobPosting
    {
        [Key] // manually setting the Id variable as the primary key
        public int Id { get; set; } // entity framework automatically registers this property here as the primary key
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Location { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public bool IsApproved { get; set; }
        [Required]
        public string UserId { get; set; } // depending on this UserId we are able to get an access to the identity user of this job posting 
                                           // vgulisxmob mtlian ROW-s databaseshi
        [ForeignKey(nameof(UserId))] // what is a ForeignKey
        public IdentityUser User { get; set; }
    }
}
