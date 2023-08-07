using Dapper;
using EventManagementSystem.IRepository;
using EventManagementSystem.Models;
using System.Data;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace EventManagementSystem.Repository
{
    public class EventRepository : IEventRepository
    {
        private IConfiguration configuration;

        public EventRepository(IConfiguration _configuration)
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

        public List<EventDetails> GetEventDetailsList(int isPublic, int type, int userId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@IsPublic", isPublic, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@type", type, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@UserId", userId==null? 0:userId, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = GetDbConnection())
            {
                List<EventDetails> events = SqlMapper.Query<EventDetails>(connection, "get_public_event_detail", parameter, commandType: CommandType.StoredProcedure).ToList();
                return events;
            }
        }

        public void SaveEventDetails(EventDetails eventDetails)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Title", eventDetails.Title, DbType.String, ParameterDirection.Input);
            parameter.Add("@StartDate", eventDetails.StartDate, DbType.Date, ParameterDirection.Input);
            parameter.Add("@EndDate", eventDetails.StartDate, DbType.Date, ParameterDirection.Input);
            parameter.Add("@Duration", eventDetails.Duration, DbType.String, ParameterDirection.Input);
            parameter.Add("@Description", eventDetails.Description, DbType.String, ParameterDirection.Input);
            parameter.Add("@Location", eventDetails.Location==null?"": eventDetails.Location, DbType.String, ParameterDirection.Input);
            parameter.Add("@UserId", eventDetails.UserId, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@IsPublic", eventDetails.IsPublic, DbType.Boolean, ParameterDirection.Input);
            parameter.Add("@EventId", eventDetails.EventId, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = GetDbConnection())
            {
                SqlMapper.Query(connection, "set_event_detail", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Comments> GetEventComments(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@eventId", Convert.ToInt32(id), DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = GetDbConnection())
            {
                List<Comments> commentList =SqlMapper.Query<Comments>(connection, "get_event_comments", parameter, commandType: CommandType.StoredProcedure).ToList();
                return commentList;
            }
        }

        public void AddEventComments(string id, string comment, int userId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@eventId", Convert.ToInt32(id), DbType.Int32, ParameterDirection.Input);
            parameter.Add("@comment", comment, DbType.String, ParameterDirection.Input);
            parameter.Add("@userId", userId, DbType.String, ParameterDirection.Input);          

            using (IDbConnection connection = GetDbConnection())
            {
                SqlMapper.Query<dynamic>(connection, "save_new_comment", parameter, commandType: CommandType.StoredProcedure);                
            }
        }

        public void DeleteEvent(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@eventId", Convert.ToInt32(id), DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = GetDbConnection())
            {
                SqlMapper.Query<dynamic>(connection, "delete_event_details", parameter, commandType: CommandType.StoredProcedure);
            }
        }
        public EventDetails GetEventDetailById(int eventId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@EventId", eventId, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = GetDbConnection())
            {
                EventDetails events = SqlMapper.Query<EventDetails>(connection, "get_event_detail_by_id", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return events;
            }
        }
    }
}
