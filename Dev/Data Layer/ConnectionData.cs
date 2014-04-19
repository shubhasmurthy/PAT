using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using Platform_Allocation_Tool.Business_Layer;
namespace Platform_Allocation_Tool.Data_Layer
{
    public static class ConnectionData
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["PlatformAllocation_preDbConnection"].ToString();

        #region Alert/Error Messages

        private const String msgUserNotFound = "User cannot be found.";
        private const String msgUnauthorizedAccess = "You are not authorized to access this site.  Please contact your supervisor to be added to the appropriate Active Directory group.";

        #endregion

        public static DataTable ListAllDemands(User u)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            DataTable resultTable = new DataTable();

            SqlCommand sqlDemandsList = new SqlCommand();
            sqlDemandsList.Connection = dbConnect;
            sqlDemandsList.CommandType = CommandType.StoredProcedure;
            sqlDemandsList.CommandText = "ListActiveDemandsPerUser";
            sqlDemandsList.Parameters.Add("@id", SqlDbType.VarChar);
            sqlDemandsList.Parameters["@id"].Value = u.ID;

            SqlDataAdapter sqlAdapter;
            try
            {
                sqlAdapter = new SqlDataAdapter(sqlDemandsList);
                sqlAdapter.Fill(resultTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
            return resultTable;
        }

        public static DataTable ListApprovedDemands(User u)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            DataTable resultTable = new DataTable();

            SqlCommand sqlDemandsList = new SqlCommand();
            sqlDemandsList.Connection = dbConnect;
            sqlDemandsList.CommandType = CommandType.StoredProcedure;
            sqlDemandsList.CommandText = "ListApprovedDemandsPerUser";
            sqlDemandsList.Parameters.Add("@id", SqlDbType.VarChar);
            sqlDemandsList.Parameters["@id"].Value = u.ID;

            SqlDataAdapter sqlAdapter;
            try
            {
                sqlAdapter = new SqlDataAdapter(sqlDemandsList);
                sqlAdapter.Fill(resultTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
            return resultTable;
        }

        public static DataTable ListSavedDemands(User u)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            DataTable resultTable = new DataTable();

            SqlCommand sqlDemandsList = new SqlCommand();
            sqlDemandsList.Connection = dbConnect;
            sqlDemandsList.CommandType = CommandType.StoredProcedure;
            sqlDemandsList.CommandText = "ListSavedDemandsPerUser";
            sqlDemandsList.Parameters.Add("@id", SqlDbType.VarChar);
            sqlDemandsList.Parameters["@id"].Value = u.ID;

            SqlDataAdapter sqlAdapter;
            try
            {
                sqlAdapter = new SqlDataAdapter(sqlDemandsList);
                sqlAdapter.Fill(resultTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
            return resultTable;
        }

        public static DataTable ListClosedDemands(User u)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            DataTable resultTable = new DataTable();

            SqlCommand sqlDemandsList = new SqlCommand();
            sqlDemandsList.Connection = dbConnect;
            sqlDemandsList.CommandType = CommandType.StoredProcedure;
            sqlDemandsList.CommandText = "ListClosedDemandsPerUser";
            sqlDemandsList.Parameters.Add("@id", SqlDbType.VarChar);
            sqlDemandsList.Parameters["@id"].Value = u.ID;

            SqlDataAdapter sqlAdapter;
            try
            {
                sqlAdapter = new SqlDataAdapter(sqlDemandsList);
                sqlAdapter.Fill(resultTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
            return resultTable;
        }

        public static DataTable ListDeclinedDemands(User u)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            DataTable resultTable = new DataTable();

            SqlCommand sqlDemandsList = new SqlCommand();
            sqlDemandsList.Connection = dbConnect;
            sqlDemandsList.CommandType = CommandType.StoredProcedure;
            sqlDemandsList.CommandText = "ListDeclinedDemandsPerUser";
            sqlDemandsList.Parameters.Add("@id", SqlDbType.VarChar);
            sqlDemandsList.Parameters["@id"].Value = u.ID;

            SqlDataAdapter sqlAdapter;
            try
            {
                sqlAdapter = new SqlDataAdapter(sqlDemandsList);
                sqlAdapter.Fill(resultTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
            return resultTable;
        }

        public static DataTable ListOrderedDemands(User u)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            DataTable resultTable = new DataTable();

            SqlCommand sqlDemandsList = new SqlCommand();
            sqlDemandsList.Connection = dbConnect;
            sqlDemandsList.CommandType = CommandType.StoredProcedure;
            sqlDemandsList.CommandText = "ListOrderedDemandsPerUser";
            sqlDemandsList.Parameters.Add("@id", SqlDbType.VarChar);
            sqlDemandsList.Parameters["@id"].Value = u.ID;

            SqlDataAdapter sqlAdapter;
            try
            {
                sqlAdapter = new SqlDataAdapter(sqlDemandsList);
                sqlAdapter.Fill(resultTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
            return resultTable;
        }

        public static void AddNewProgram(string programName)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            try
            {
                SqlCommand sqlAddNewProgram = new SqlCommand();
                sqlAddNewProgram.Connection = dbConnect;
                sqlAddNewProgram.CommandType = CommandType.StoredProcedure;
                sqlAddNewProgram.CommandText = "InsertNewProgram";
                sqlAddNewProgram.Parameters.Add("@ProgramName", SqlDbType.VarChar);
                sqlAddNewProgram.Parameters["@ProgramName"].Value = programName;

                sqlAddNewProgram.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static bool AuthenticateUser(string username, string password)
        {  
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            SqlCommand sqlValidateUser = new SqlCommand();
            sqlValidateUser.Connection = dbConnect;
            sqlValidateUser.CommandType = CommandType.StoredProcedure;
            sqlValidateUser.CommandText = "ValidateUser";
            sqlValidateUser.Parameters.Add("@UserName", SqlDbType.VarChar);
            sqlValidateUser.Parameters.Add("@Password", SqlDbType.VarChar);
            sqlValidateUser.Parameters["@UserName"].Value = username;
            sqlValidateUser.Parameters["@Password"].Value = password;
            int ReturnCode = (int)sqlValidateUser.ExecuteScalar(); 
                return ReturnCode == 1;
        }
        public static void GetDemandByID(Demand demand)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            SqlCommand sqlSelectUser = new SqlCommand();
            sqlSelectUser.Connection = dbConnect;
            sqlSelectUser.CommandType = CommandType.StoredProcedure;
            sqlSelectUser.CommandText = "SelectDemand";
            sqlSelectUser.Parameters.Add("@id", SqlDbType.SmallInt);
            sqlSelectUser.Parameters["@id"].Value = demand.DemandId;
            SqlDataReader dbReader = sqlSelectUser.ExecuteReader();
            try
            {
                if (dbReader.Read())
                {
                    if (dbReader["DemandName"] != DBNull.Value)
                        demand.DemandName = dbReader["DemandName"].ToString();

                    if (dbReader["PlatformName"] != DBNull.Value)
                        demand.PlatformName = dbReader["PlatformName"].ToString();

                    if (dbReader["ProgramName"] != DBNull.Value)
                        demand.ProgramName = dbReader["ProgramName"].ToString();

                    if (dbReader["OpenDate"] != DBNull.Value)
                        demand.CreatedDate = (DateTime)dbReader["OpenDate"];

                    if (dbReader["CloseDate"] != DBNull.Value)
                        demand.CloseDate = (DateTime)dbReader["CloseDate"];

                    if (dbReader["Name"] != DBNull.Value)
                        demand.Status = dbReader["Name"].ToString();

                    if (dbReader["DeclineReason"] != DBNull.Value)
                        demand.DeclineReason = dbReader["DeclineReason"].ToString();
                    else
                        demand.DeclineReason = "";

                    dbReader.Close();
                }
                else
                {
                    dbReader.Close();
                    throw new UnauthorizedAccessException();
                }

            }
            catch (Exception ex)
            {
                dbReader.Close();
                throw ex;
            }

            SqlCommand sqlSelectTeamBoard = new SqlCommand();
            sqlSelectTeamBoard.Connection = dbConnect;
            sqlSelectTeamBoard.CommandType = CommandType.StoredProcedure;
            sqlSelectTeamBoard.CommandText = "SelectTeamBoards";
            sqlSelectTeamBoard.Parameters.Add("@id", SqlDbType.SmallInt);
            sqlSelectTeamBoard.Parameters["@id"].Value = demand.DemandId;

            dbReader = sqlSelectTeamBoard.ExecuteReader();
            try
            {
                demand.Boards = new List<TeamBoard>();
                while (dbReader.Read())
                {
                    demand.Boards.Add(new TeamBoard(dbReader["BoardSKU"].ToString(), dbReader["TypeName"].ToString(), dbReader["TeamName"].ToString(), Convert.ToInt16(dbReader["NumberOfBoards"])));
                }

                dbReader.Close();
            }
            catch (Exception ex)
            {
                try 
                { 
                    dbReader.Close(); 
                }
                catch { }

                throw ex;
            }

            SqlCommand sqlSelectTechDoc = new SqlCommand();
            sqlSelectTechDoc.Connection = dbConnect;
            sqlSelectTechDoc.CommandType = CommandType.StoredProcedure;
            sqlSelectTechDoc.CommandText = "SelectTechDoc";
            sqlSelectTechDoc.Parameters.Add("@id", SqlDbType.SmallInt);
            sqlSelectTechDoc.Parameters["@id"].Value = demand.DemandId;

            dbReader = sqlSelectTechDoc.ExecuteReader();
            try
            {
                demand.TechnicalDocumentation = new List<TechnicalDocumentation>();
                while (dbReader.Read())
                {   
                    demand.TechnicalDocumentation.Add(new TechnicalDocumentation(dbReader["TDocName"].ToString(), dbReader["Url"].ToString()));
                }

                dbReader.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    dbReader.Close();
                }
                catch { }

                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }

        }

        public static User GetAdmin()
        {
            User user = new User();
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            SqlCommand sqlSelectUser = new SqlCommand();
            sqlSelectUser.Connection = dbConnect;
            sqlSelectUser.CommandType = CommandType.StoredProcedure;
            sqlSelectUser.CommandText = "GetAdmin";

            SqlDataReader dbReader = sqlSelectUser.ExecuteReader();
            try
            {
                if (dbReader.Read())
                {
                    if (dbReader["ID"] != DBNull.Value)
                        user.ID = dbReader["ID"].ToString();

                    if (dbReader["FirstName"] != DBNull.Value)
                        user.FirstName = dbReader["FirstName"].ToString();

                    if (dbReader["LastName"] != DBNull.Value)
                        user.LastName = dbReader["LastName"].ToString();

                    if (dbReader["WWID"] != DBNull.Value)
                        user.WWID = dbReader["WWID"].ToString();

                    if (dbReader["eAddress"] != DBNull.Value)
                        user.Email = dbReader["eAddress"].ToString();

                    if (dbReader["Active"] != DBNull.Value)
                        user.Active = Convert.ToBoolean(dbReader["Active"].ToString());

                    dbReader.Close();
                }
                else
                {
                    dbReader.Close();
                    throw new UnauthorizedAccessException();
                }

            }
            catch (Exception ex)
            {
                dbReader.Close();
                throw ex;
            }
            return user;
        }
        public static void GetSelectedUser(User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            SqlCommand sqlSelectUser = new SqlCommand();
            sqlSelectUser.Connection = dbConnect;
            sqlSelectUser.CommandType = CommandType.StoredProcedure;
            sqlSelectUser.CommandText = "SelectUser";
            sqlSelectUser.Parameters.Add("@id", SqlDbType.VarChar);
            sqlSelectUser.Parameters["@id"].Value = user.ID;

            SqlDataReader dbReader = sqlSelectUser.ExecuteReader();
            try
            {
                if (dbReader.Read())
                {
                    if (dbReader["FirstName"] != DBNull.Value)
                        user.FirstName = dbReader["FirstName"].ToString();

                    if (dbReader["LastName"] != DBNull.Value)
                        user.LastName = dbReader["LastName"].ToString();

                    if (dbReader["WWID"] != DBNull.Value)
                        user.WWID = dbReader["WWID"].ToString();

                    if (dbReader["eAddress"] != DBNull.Value)
                        user.Email = dbReader["eAddress"].ToString();

                    if (dbReader["Active"] != DBNull.Value)
                        user.Active = Convert.ToBoolean(dbReader["Active"].ToString());

                    dbReader.Close();
                }
                else
                {
                    dbReader.Close();
                    throw new UnauthorizedAccessException();
                }

            }
            catch (Exception ex)
            {
                dbReader.Close();
                throw ex;
            }

            SqlCommand sqlSelectUserRoles = new SqlCommand();
            sqlSelectUserRoles.Connection = dbConnect;
            sqlSelectUserRoles.CommandType = CommandType.StoredProcedure;
            sqlSelectUserRoles.CommandText = "SelectUserRoles";
            sqlSelectUserRoles.Parameters.Add("@id", SqlDbType.VarChar);
            sqlSelectUserRoles.Parameters["@id"].Value = user.ID;

            dbReader = sqlSelectUserRoles.ExecuteReader();
            try
            {
                while (dbReader.Read())
                {
                    user.Role = dbReader["Name"].ToString();

                    if (user.Role.Equals("Administrator"))
                        user.IsAdmin = true;
                }

                dbReader.Close();
            }
            catch (Exception ex)
            {
                //close sqldatareader
                try { dbReader.Close(); }
                catch { }

                throw ex;
            }
             
            finally
            {
                dbConnect.Close();
            }
        }

        public static void GetSelectedTeam(Team team)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            SqlCommand sqlSelectTeam = new SqlCommand();
            sqlSelectTeam.Connection = dbConnect;
            sqlSelectTeam.CommandType = CommandType.StoredProcedure;
            sqlSelectTeam.CommandText = "SelectTeam";
            sqlSelectTeam.Parameters.Add("@name", SqlDbType.VarChar);
            sqlSelectTeam.Parameters.Add("@mgrID", SqlDbType.VarChar, 8).Direction = ParameterDirection.Output;
            sqlSelectTeam.Parameters.Add("@repID", SqlDbType.VarChar, 8).Direction = ParameterDirection.Output;
            sqlSelectTeam.Parameters.Add("@mgrEmailID", SqlDbType.VarChar, 60).Direction = ParameterDirection.Output;
            sqlSelectTeam.Parameters.Add("@repEmailID", SqlDbType.VarChar, 60).Direction = ParameterDirection.Output;

            sqlSelectTeam.Parameters["@name"].Value = team.Name;
            try
            {
                sqlSelectTeam.ExecuteNonQuery();
                team.MgrId = sqlSelectTeam.Parameters["@mgrID"].Value.ToString();
                team.RepId = sqlSelectTeam.Parameters["@repID"].Value.ToString();
                team.MgrEmailId = sqlSelectTeam.Parameters["@mgrEmailID"].Value.ToString();
                team.RepEmailId = sqlSelectTeam.Parameters["@repEmailID"].Value.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<String> GetTeams()
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            
            List<String> teamNames = new List<String>();
            SqlCommand sqlSelectTeams = new SqlCommand();
            sqlSelectTeams.Connection = dbConnect;
            sqlSelectTeams.CommandType = CommandType.StoredProcedure;
            sqlSelectTeams.CommandText = "SelectTeams";
            SqlDataReader dbReader = sqlSelectTeams.ExecuteReader();

            try
                {
                    while (dbReader.Read())
                    {
                        teamNames.Add(dbReader.GetString(0));
                    }
                }
                catch (Exception ex)
                {
                    try { dbReader.Close(); }
                    catch { }

                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
            return teamNames;

        }
        public static String GetSelectedUserName(String id)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            String userName = String.Empty;
            if (id.Length.Equals(0))
                userName = "Anonymous";
            else
            {
                SqlCommand sqlSelectUser = new SqlCommand();
                sqlSelectUser.Connection = dbConnect;
                sqlSelectUser.CommandType = CommandType.StoredProcedure;
                sqlSelectUser.CommandText = "SelectUser";
                sqlSelectUser.Parameters.Add("@id", SqlDbType.VarChar);
                sqlSelectUser.Parameters["@id"].Value = id;

                SqlDataReader dbReader = sqlSelectUser.ExecuteReader();
                try
                {
                    if (dbReader.Read())
                    {
                        userName = dbReader["FirstName"].ToString() + " " + dbReader["LastName"].ToString();
                        dbReader.Close();
                    }
                    else
                    {
                        userName = String.Empty;
                        dbReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    try { dbReader.Close(); }
                    catch { }

                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
            }
            return userName;
        }

        public static void WriteLog(SessionLog log, String type, String logMessage, String trace)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            log.SessionLength = Convert.ToInt32(DateTime.Now.Subtract(log.SessionStart).TotalSeconds);

            SqlCommand sqlInsertLogEntry = new SqlCommand();

            try
            {
                sqlInsertLogEntry.Connection = dbConnect;
                sqlInsertLogEntry.CommandType = CommandType.StoredProcedure;
                sqlInsertLogEntry.CommandText = "InsertLogs";

                sqlInsertLogEntry.Parameters.Add("@type", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@userName", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@serverName", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@platform", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@browser", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@stackTrace", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@message", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@assemblyVersion", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@timeoutMinutes", SqlDbType.VarChar);
                sqlInsertLogEntry.Parameters.Add("@sessionLength", SqlDbType.Int);
                sqlInsertLogEntry.Parameters.Add("@sessionID", SqlDbType.Char);
                sqlInsertLogEntry.Parameters.Add("@sessionStart", SqlDbType.DateTime);
                sqlInsertLogEntry.Parameters.Add("@entryID", SqlDbType.VarChar);

                sqlInsertLogEntry.Parameters["@type"].Value = type;
                sqlInsertLogEntry.Parameters["@userName"].Value = log.UserName;
                sqlInsertLogEntry.Parameters["@serverName"].Value = log.ServerName;
                sqlInsertLogEntry.Parameters["@platform"].Value = log.Platform;
                sqlInsertLogEntry.Parameters["@browser"].Value = log.Browser;
                sqlInsertLogEntry.Parameters["@stackTrace"].Value = trace;
                sqlInsertLogEntry.Parameters["@message"].Value = logMessage;
                sqlInsertLogEntry.Parameters["@assemblyVersion"].Value = log.AssemblyVersion;
                sqlInsertLogEntry.Parameters["@timeoutMinutes"].Value = log.TimeoutMinutes;
                sqlInsertLogEntry.Parameters["@sessionLength"].Value = log.SessionLength;
                sqlInsertLogEntry.Parameters["@sessionID"].Value = log.SessionID;
                sqlInsertLogEntry.Parameters["@sessionStart"].Value = log.SessionStart;
                sqlInsertLogEntry.Parameters["@entryID"].Value = log.EntryID;

                sqlInsertLogEntry.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               // throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }


        public static void EditDemand(Demand demand, User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            SqlCommand sqlInsertDemand = new SqlCommand();
            SqlCommand sqlInsertTeamBoard = new SqlCommand();
            SqlCommand sqlDeleteTeamBoard = new SqlCommand();
            SqlCommand sqlInsertTechDoc = new SqlCommand();
            SqlCommand sqlClearTechDoc = new SqlCommand();

            try
            {
                EditDemandDetails(demand, user);

                sqlDeleteTeamBoard.Connection = dbConnect;
                sqlDeleteTeamBoard.CommandType = CommandType.StoredProcedure;
                sqlDeleteTeamBoard.CommandText = "ClearTeamBoard";
                sqlDeleteTeamBoard.Parameters.Add("@demandId", SqlDbType.TinyInt);
                sqlDeleteTeamBoard.Parameters["@demandId"].Value = demand.DemandId;

                sqlDeleteTeamBoard.ExecuteNonQuery();

                sqlInsertTeamBoard.Connection = dbConnect;
                sqlInsertTeamBoard.CommandType = CommandType.StoredProcedure;
                sqlInsertTeamBoard.CommandText = "InsertTeamBoard";

                sqlInsertTeamBoard.Parameters.Add("@sku", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@team", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@numberOfBoards", SqlDbType.TinyInt);
                sqlInsertTeamBoard.Parameters.Add("@demandId", SqlDbType.TinyInt);
                sqlInsertTeamBoard.Parameters.Add("@user", SqlDbType.VarChar);

                foreach (TeamBoard tb in demand.Boards)
                {
                    sqlInsertTeamBoard.Parameters["@sku"].Value = tb.SKU;
                    sqlInsertTeamBoard.Parameters["@team"].Value = tb.TeamName;
                    sqlInsertTeamBoard.Parameters["@demandId"].Value = demand.DemandId;
                    sqlInsertTeamBoard.Parameters["@numberOfBoards"].Value = tb.NumberOfBoards;
                    sqlInsertTeamBoard.Parameters["@user"].Value = user.ID;

                    sqlInsertTeamBoard.ExecuteNonQuery();
                }


                sqlClearTechDoc.Connection = dbConnect;
                sqlClearTechDoc.CommandType = CommandType.StoredProcedure;
                sqlClearTechDoc.CommandText = "ClearTechDoc";
                sqlClearTechDoc.Parameters.Add("@demandID", SqlDbType.TinyInt);
                sqlClearTechDoc.Parameters["@demandId"].Value = demand.DemandId;

                sqlClearTechDoc.ExecuteNonQuery();

                if (demand.TechnicalDocumentation.Count != 0)
                {
                    sqlInsertTechDoc.Connection = dbConnect;
                    sqlInsertTechDoc.CommandType = CommandType.StoredProcedure;
                    sqlInsertTechDoc.CommandText = "InsertTechDoc";

                    sqlInsertTechDoc.Parameters.Add("@name", SqlDbType.VarChar);
                    sqlInsertTechDoc.Parameters.Add("@link", SqlDbType.VarChar);
                    sqlInsertTechDoc.Parameters.Add("@demandID", SqlDbType.TinyInt);
                    sqlInsertTechDoc.Parameters.Add("@user", SqlDbType.VarChar);

                    foreach (TechnicalDocumentation td in demand.TechnicalDocumentation)
                    {
                        sqlInsertTechDoc.Parameters["@name"].Value = td.TDocName;
                        sqlInsertTechDoc.Parameters["@link"].Value = td.TDocAddress;
                        sqlInsertTechDoc.Parameters["@demandId"].Value = demand.DemandId;
                        sqlInsertTechDoc.Parameters["@user"].Value = user.ID;

                        sqlInsertTechDoc.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void EditTeamBoard(Demand demand)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
           
            SqlCommand sqlInsertTeamBoard = new SqlCommand();

            try
            {
                sqlInsertTeamBoard.Connection = dbConnect;
                sqlInsertTeamBoard.CommandType = CommandType.StoredProcedure;
                sqlInsertTeamBoard.CommandText = "EditTeamBoard";

                sqlInsertTeamBoard.Parameters.Add("@sku", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@team", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@numberOfBoards", SqlDbType.TinyInt);
                sqlInsertTeamBoard.Parameters.Add("@demandId", SqlDbType.TinyInt);

                foreach (TeamBoard tb in demand.Boards)
                {
                    sqlInsertTeamBoard.Parameters["@sku"].Value = tb.SKU;
                    sqlInsertTeamBoard.Parameters["@team"].Value = tb.TeamName;
                    sqlInsertTeamBoard.Parameters["@demandId"].Value = demand.DemandId;
                    sqlInsertTeamBoard.Parameters["@numberOfBoards"].Value = tb.NumberOfBoards;
                    sqlInsertTeamBoard.ExecuteNonQuery();
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void EditDemandDetails(Demand demand, User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            SqlCommand sqlInsertDemand = new SqlCommand();

            try
            {
                sqlInsertDemand.Connection = dbConnect;
                sqlInsertDemand.CommandType = CommandType.StoredProcedure;
                sqlInsertDemand.CommandText = "EditDemand";

                sqlInsertDemand.Parameters.Add("@demandID", SqlDbType.TinyInt);
                sqlInsertDemand.Parameters.Add("@name", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@pfName", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@prgName", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@openDate", SqlDbType.DateTime);
                sqlInsertDemand.Parameters.Add("@closeDate", SqlDbType.DateTime);
                sqlInsertDemand.Parameters.Add("@user", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@state", SqlDbType.VarChar);

                sqlInsertDemand.Parameters["@demandID"].Value = demand.DemandId;
                sqlInsertDemand.Parameters["@name"].Value = demand.DemandName;
                sqlInsertDemand.Parameters["@pfName"].Value = demand.PlatformName;
                sqlInsertDemand.Parameters["@prgName"].Value = demand.ProgramName;
                sqlInsertDemand.Parameters["@openDate"].Value = demand.CreatedDate;
                sqlInsertDemand.Parameters["@closeDate"].Value = demand.CloseDate;
                sqlInsertDemand.Parameters["@user"].Value = user.ID;
                sqlInsertDemand.Parameters["@state"].Value = demand.Status;

                sqlInsertDemand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void UpdateDemandStatus(Demand demand, User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            SqlCommand sqlInsertDemand = new SqlCommand();

            try
            {
                sqlInsertDemand.Connection = dbConnect;
                sqlInsertDemand.CommandType = CommandType.StoredProcedure;
                sqlInsertDemand.CommandText = "UpdateDemandStatus";

                sqlInsertDemand.Parameters.Add("@demandID", SqlDbType.TinyInt);
                sqlInsertDemand.Parameters.Add("@state", SqlDbType.VarChar);

                sqlInsertDemand.Parameters["@demandID"].Value = demand.DemandId;
                sqlInsertDemand.Parameters["@state"].Value = demand.Status;

                sqlInsertDemand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void SaveDemand(Demand demand, User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            SqlCommand sqlInsertDemand = new SqlCommand();
            SqlCommand sqlInsertTeamBoard = new SqlCommand();

            try
            {
                UpdateDemandStatus(demand, user);

                sqlInsertTeamBoard.Connection = dbConnect;
                sqlInsertTeamBoard.CommandType = CommandType.StoredProcedure;
                sqlInsertTeamBoard.CommandText = "EditTeamBoard";

                sqlInsertTeamBoard.Parameters.Add("@sku", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@team", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@numberOfBoards", SqlDbType.TinyInt);
                sqlInsertTeamBoard.Parameters.Add("@demandId", SqlDbType.TinyInt);

                EditTeamBoard(demand);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void DeleteDemand(Demand demand)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            SqlCommand sqlDeleteDemand = new SqlCommand();
            SqlCommand sqlDeleteTeamBoard = new SqlCommand();
            SqlCommand sqlDeleteTechDoc = new SqlCommand();

            try
            {
                sqlDeleteTechDoc.Connection = dbConnect;
                sqlDeleteTechDoc.CommandType = CommandType.StoredProcedure;
                sqlDeleteTechDoc.CommandText = "ClearTechDoc";
                sqlDeleteTechDoc.Parameters.Add("@demandID", SqlDbType.TinyInt);
                sqlDeleteTechDoc.Parameters["@demandId"].Value = demand.DemandId;
                sqlDeleteTechDoc.ExecuteNonQuery();

                sqlDeleteTeamBoard.Connection = dbConnect;
                sqlDeleteTeamBoard.CommandType = CommandType.StoredProcedure;
                sqlDeleteTeamBoard.CommandText = "ClearTeamBoard";
                sqlDeleteTeamBoard.Parameters.Add("@demandId", SqlDbType.TinyInt);
                sqlDeleteTeamBoard.Parameters["@demandId"].Value = demand.DemandId;
                sqlDeleteTeamBoard.ExecuteNonQuery();


                sqlDeleteDemand.Connection = dbConnect;
                sqlDeleteDemand.CommandType = CommandType.StoredProcedure;
                sqlDeleteDemand.CommandText = "DeleteDemand";
                sqlDeleteDemand.Parameters.Add("@demandID", SqlDbType.TinyInt);
                sqlDeleteDemand.Parameters["@demandID"].Value = demand.DemandId;

                sqlDeleteDemand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static Int16 WriteNewDemand(Demand demand, User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();
            Int16 demandId;
            SqlCommand sqlInsertDemand = new SqlCommand();
            SqlCommand sqlInsertTeamBoard = new SqlCommand();
            SqlCommand sqlInsertTechDoc = new SqlCommand();

            try
            {
                sqlInsertDemand.Connection = dbConnect;
                sqlInsertDemand.CommandType = CommandType.StoredProcedure;
                sqlInsertDemand.CommandText = "InsertDemand";

                sqlInsertDemand.Parameters.Add("@DemandID", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                sqlInsertDemand.Parameters.Add("@name", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@pfName", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@prgName", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@openDate", SqlDbType.DateTime);
                sqlInsertDemand.Parameters.Add("@closeDate", SqlDbType.DateTime);
                sqlInsertDemand.Parameters.Add("@user", SqlDbType.VarChar);
                sqlInsertDemand.Parameters.Add("@state", SqlDbType.VarChar);

                sqlInsertDemand.Parameters["@name"].Value = demand.DemandName;
                sqlInsertDemand.Parameters["@pfName"].Value = demand.PlatformName;
                sqlInsertDemand.Parameters["@prgName"].Value = demand.ProgramName;
                sqlInsertDemand.Parameters["@openDate"].Value = demand.CreatedDate;
                sqlInsertDemand.Parameters["@closeDate"].Value = demand.CloseDate;
                sqlInsertDemand.Parameters["@user"].Value = user.ID;
                sqlInsertDemand.Parameters["@state"].Value = demand.Status;

                sqlInsertDemand.ExecuteNonQuery();
                demandId = Convert.ToInt16(sqlInsertDemand.Parameters["@DemandID"].Value);

                sqlInsertTeamBoard.Connection = dbConnect;
                sqlInsertTeamBoard.CommandType = CommandType.StoredProcedure;
                sqlInsertTeamBoard.CommandText = "InsertTeamBoard";

                sqlInsertTeamBoard.Parameters.Add("@sku", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@team", SqlDbType.VarChar);
                sqlInsertTeamBoard.Parameters.Add("@numberOfBoards", SqlDbType.TinyInt);
                sqlInsertTeamBoard.Parameters.Add("@demandId", SqlDbType.TinyInt);
                sqlInsertTeamBoard.Parameters.Add("@user", SqlDbType.VarChar);

                foreach(TeamBoard tb in demand.Boards)
                {
                    sqlInsertTeamBoard.Parameters["@sku"].Value = tb.SKU;
                    sqlInsertTeamBoard.Parameters["@team"].Value = tb.TeamName;
                    sqlInsertTeamBoard.Parameters["@demandId"].Value = demandId;
                    sqlInsertTeamBoard.Parameters["@numberOfBoards"].Value = tb.NumberOfBoards;
                    sqlInsertTeamBoard.Parameters["@user"].Value = user.ID;

                    sqlInsertTeamBoard.ExecuteNonQuery();
                }

                if(demand.TechnicalDocumentation.Count !=0)
                {
                    sqlInsertTechDoc.Connection = dbConnect;
                    sqlInsertTechDoc.CommandType = CommandType.StoredProcedure;
                    sqlInsertTechDoc.CommandText = "InsertTechDoc";

                    sqlInsertTechDoc.Parameters.Add("@name", SqlDbType.VarChar);
                    sqlInsertTechDoc.Parameters.Add("@link", SqlDbType.VarChar);
                    sqlInsertTechDoc.Parameters.Add("@demandID", SqlDbType.TinyInt);
                    sqlInsertTechDoc.Parameters.Add("@user", SqlDbType.VarChar);

                    foreach(TechnicalDocumentation td in demand.TechnicalDocumentation)
                    {
                        sqlInsertTechDoc.Parameters["@name"].Value = td.TDocName;
                        sqlInsertTechDoc.Parameters["@link"].Value = td.TDocAddress;
                        sqlInsertTechDoc.Parameters["@demandId"].Value = demandId;
                        sqlInsertTechDoc.Parameters["@user"].Value = user.ID;

                        sqlInsertTechDoc.ExecuteNonQuery();
                    }

                }

                return demandId;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void WriteNewBoard(TeamBoard tb, User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            SqlCommand sqlInsertBoardEntry = new SqlCommand();

            try
            {
                sqlInsertBoardEntry.Connection = dbConnect;
                sqlInsertBoardEntry.CommandType = CommandType.StoredProcedure;
                sqlInsertBoardEntry.CommandText = "InsertNewBoard";

                sqlInsertBoardEntry.Parameters.Add("@sku", SqlDbType.VarChar);
                sqlInsertBoardEntry.Parameters.Add("@typeName", SqlDbType.VarChar);
                sqlInsertBoardEntry.Parameters.Add("@user", SqlDbType.VarChar);

                sqlInsertBoardEntry.Parameters["@sku"].Value = tb.SKU;
                sqlInsertBoardEntry.Parameters["@typeName"].Value = tb.BoardType;
                sqlInsertBoardEntry.Parameters["@user"].Value = user.ID;


                sqlInsertBoardEntry.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void MonitorClosedDemands()
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            SqlCommand sqlUpdateClosedDemands = new SqlCommand();

            try
            {
                sqlUpdateClosedDemands.Connection = dbConnect;
                sqlUpdateClosedDemands.CommandType = CommandType.StoredProcedure;
                sqlUpdateClosedDemands.CommandText = "MonitorClosedDemands";
                sqlUpdateClosedDemands.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void WriteDeclineReason(Demand d)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            SqlCommand sqlWriteDeclineReason = new SqlCommand();

            try
            {
                sqlWriteDeclineReason.Connection = dbConnect;
                sqlWriteDeclineReason.CommandType = CommandType.StoredProcedure;
                sqlWriteDeclineReason.CommandText = "WriteDeclineReason";

                sqlWriteDeclineReason.Parameters.Add("@reason", SqlDbType.VarChar);
                sqlWriteDeclineReason.Parameters.Add("@demandId", SqlDbType.SmallInt);

                sqlWriteDeclineReason.Parameters["@reason"].Value = d.DeclineReason;
                sqlWriteDeclineReason.Parameters["@demandId"].Value = d.DemandId;


                sqlWriteDeclineReason.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }

        public static void WriteNewPlatform(String platformName, User user)
        {
            SqlConnection dbConnect = new SqlConnection(connectionString);
            dbConnect.Open();

            SqlCommand sqlInsertPlatform = new SqlCommand();

            try
            {
                sqlInsertPlatform.Connection = dbConnect;
                sqlInsertPlatform.CommandType = CommandType.StoredProcedure;
                sqlInsertPlatform.CommandText = "InsertPlatform";

                sqlInsertPlatform.Parameters.Add("@name", SqlDbType.VarChar);
                sqlInsertPlatform.Parameters.Add("@user", SqlDbType.VarChar);

                sqlInsertPlatform.Parameters["@name"].Value = platformName;
                sqlInsertPlatform.Parameters["@user"].Value = user.ID;


                sqlInsertPlatform.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dbConnect.Close();
            }
        }
    }

}