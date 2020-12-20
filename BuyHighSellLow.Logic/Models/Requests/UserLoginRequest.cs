namespace BuyHighSellLow.Logic.Models.Requests
{
    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}