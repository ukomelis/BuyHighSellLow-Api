using Microsoft.AspNetCore.Identity;

namespace BuyHighSellLow.DataAccess.Models.Identity
{
    public class BHSLUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
    }
}