using Dapper;
using EventManagementsystem.Models;
using EventManagementSystem.IRepository;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace EventManagementSystem.Repository
{
    public class LoginRepository: ILoginRepository
    {
        private IConfiguration configuration;

        public LoginRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public DbConnection GetDbConnection()
        {
            string connectionstringAppSetting = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionstringAppSetting))
            {
                return new SqlConnection(connectionstringAppSetting);
            }
            return new SqlConnection(connectionstringAppSetting);
        }

        public UserDetails GetLoginDetailByEmail(string email, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email, DbType.String, ParameterDirection.Input);
            parameters.Add("@password", password, DbType.String, ParameterDirection.Input);
            using (IDbConnection connection = GetDbConnection())
            {
                return (UserDetails)SqlMapper.Query<UserDetails>(connection, "get_login_detail_by_email ", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Boolean GetEmailValidate(string email)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@email", email, DbType.String, ParameterDirection.Input);
            using (IDbConnection connection = GetDbConnection())
            {
                return SqlMapper.Query<Boolean>(connection, "get_email_validate ", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public void SaveUserDetails(UserDetails userDetails)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@firstname", userDetails.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@lastname", userDetails.LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("@email", userDetails.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@password", userDetails.Password, DbType.String, ParameterDirection.Input);           
            parameters.Add("@gender", userDetails.Gender, DbType.String, ParameterDirection.Input);
            parameters.Add("@mobile", userDetails.Mobile, DbType.String, ParameterDirection.Input);
            using (IDbConnection connection = GetDbConnection())
            {
                SqlMapper.Query<dynamic>(connection, "save_userdetails ", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
