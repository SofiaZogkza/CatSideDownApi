using CatSideDownApi.Contracts;
using CatSideDownApi.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CatSideDownApi.Services
{
    public class UserServices : IUserServices
    {
        private readonly CatSideDownDbContext _dbContext;
        public UserServices(CatSideDownDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LogInResponse LogIn(LogInRequest req)
        {
            var user = AuthenticateUser(req);
            return user;          
        }

        public Response LogOut(LogOutRequest req)
        {
            //if logged in then log out
            if (req.JwtToken != null)
            {
                req.JwtToken = null;
            }
            return BuildResponse(true, "You have successfully loggedout.");
        }

        public async Task<Response> Register(RegisterRequest req)
        {            
            var newUser = req.user;
            var response = new Response();
            var userExists = _dbContext.Users.Any(x => x.Email.ToLower() == req.user.Email.ToLower());

            if(userExists)
            {
                response = BuildResponse(false, "User already exists.");
                return response;
            }

            if (newUser.Password == req.ConfirmPassword)
            {
                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();
                response = BuildResponse(true, "Congratulations! You are Registered!");
            }
            else
            {
                response = BuildResponse(false, "The password and confirmation password do not match.");                    
            }
            return response;
        }

        public async Task<byte[]> GetCatPictureFromCaatas()
        {
            var baseAddress = new Uri("https://cataas.com/cat");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                using (var content = await httpClient.GetStreamAsync(baseAddress))
                {
                    var img = Image.FromStream(content);
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);

                    var _imageConverter = new ImageConverter();
                    var imgToByte = (byte[])_imageConverter.ConvertTo(img, typeof(byte[]));
                    return imgToByte;
                }
            }            
        }
        public async Task<User> GetUserInformation(string email)
        {
            var userInfo = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            return userInfo;
        }

        #region private Methods
        private LogInResponse AuthenticateUser(LogInRequest req)
        {
            var registeredUsers = _dbContext.Users;
            var response = new LogInResponse();

            foreach (var r in registeredUsers)
            {
                if ((req.User.Email == r.Email) && (req.User.Password == r.Password))
                {
                    response = BuildLogInResponse(req, true, "Logged In!");
                    return response;
                }
                else
                {
                    response = BuildLogInResponse(null, false, "Email or Password is incorrent.");
                }
            }
            return response;
        }
        private LogInResponse BuildLogInResponse(LogInRequest req, bool success, string message)
        {
            var response = new Response()
            {
                Success = success,
                Message = message
            };
            var loginResponse = new LogInResponse()
            {
                User = req?.User,
                Response = response
            };
            
            
            return loginResponse;
        }
        private Response BuildResponse(bool success, string message)
        {
            var response = new Response()
            {
                Success = success,
                Message = message
            };

            return response;
        }        
        #endregion
    }
}
