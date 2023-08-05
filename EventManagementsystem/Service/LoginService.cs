using EventManagementsystem.Models;
using EventManagementSystem.IRepository;
using EventManagementSystem.IService;

namespace EventManagementSystem.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginDetailRepository;

        public LoginService(ILoginRepository loginDetailRepository)
        {
            _loginDetailRepository = loginDetailRepository;
        }
        public UserDetails GetLoginDetailByEmail(string email, string password)
        {
            UserDetails userDetails = _loginDetailRepository.GetLoginDetailByEmail(email, password);
            if(userDetails != null) {
                return userDetails;
            }
            else
            {
                return null;
            }
        }
        public Boolean GetEmailValidate(string email)
        {
            return _loginDetailRepository.GetEmailValidate(email);
        }
        public void SaveUserDetails(UserDetails userDetails)
        {
            _loginDetailRepository.SaveUserDetails(userDetails);
        }
    }
}
