namespace BuyHighSellLow.Logic.Models.Requests
{
    public class UserDetailsRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}