namespace CatSideDownApi.Contracts
{
    public class LogInRequest
    {
        public User User { get; set; }
        public bool RememberMe { get; set; }
    }
}
