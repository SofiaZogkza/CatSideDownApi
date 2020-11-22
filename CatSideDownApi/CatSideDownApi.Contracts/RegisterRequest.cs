namespace CatSideDownApi.Contracts
{
    public class RegisterRequest
    {
        public User user { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
