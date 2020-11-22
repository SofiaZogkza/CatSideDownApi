namespace CatSideDownApi.Contracts
{
    public class LogInResponse
    {
        public User User { get; set; }
        public Response Response { get; set; }

        public string JwtToken { get; set; }
    }
}
