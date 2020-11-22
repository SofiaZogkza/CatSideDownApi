using CatSideDownApi.Contracts;
using System.Threading.Tasks;

namespace CatSideDownApi.Interfaces
{
    public interface IUserServices
    {
        LogInResponse LogIn(LogInRequest req);
        Response LogOut(LogOutRequest req);
        Task<Response> Register( RegisterRequest req);
        Task<byte[]> GetCatPictureFromCaatas();
        Task<User> GetUserInformation(string email);
    }
}
