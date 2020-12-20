using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BuyHighSellLow.DataAccess.Models.Identity
{
    public class BHSLUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [Required]
        public decimal Balance { get; set; }
    }
}