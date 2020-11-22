namespace CatSideDownApi.Contracts
{
    public class LogOutRequest
    {
        public string Email { get; set; }
        public string JwtToken { get; set; }
    }
}
