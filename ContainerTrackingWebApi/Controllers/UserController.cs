using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContainerTrackingWebApi.Models;
using ContainerTrackingWebApi.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using ContainerTrackingWebApi.ViewModel;

namespace ContainerTrackingWebApi.Controllers
{
    [Route("api/AccountAPI")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class UserController : ControllerBase
    {

        string connection = Startup.GetConnectionString();
        CreateLogController createLog = new CreateLogController();
        #region Encode/Decode
        /// <summary>
        /// Method encodes the data
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private static string Base64Encode(string plainText)
        {
            string base64Encoded;
            byte[] data = Encoding.ASCII.GetBytes(plainText);
            base64Encoded = System.Convert.ToBase64String(data);
            return base64Encoded;
        }
        private static string Base64Decode(string base64EncodedData)
        {
            string base64Decoded;
            byte[] data = Convert.FromBase64String(base64EncodedData);
            base64Decoded = ASCIIEncoding.ASCII.GetString(data);
            return base64Decoded;
        }
        #endregion

        #region User
        [HttpGet]
        [Route("checkLoginDetails")]
        //method is used to check user information if it exists or not on login
        public async Task<IActionResult> checkLogin(string Username, string Password)
        {
            try
            {
                string user_id = "";
                string user_role = "";
                using (SqlConnection conn = new SqlConnection(connection))
                {


                    conn.Open();
                    string pwd = "";
                    pwd = Base64Encode(Password);
                    SqlCommand cmd = new SqlCommand("select TOP 1 id,f_name,user_role,l_name,email from users where email='" + Username + "' and password='" + pwd + "' and status=1 order by id DESC", conn);
                    SqlDataAdapter MyAdapter = new SqlDataAdapter();
                    MyAdapter.SelectCommand = cmd;
                    DataTable dTable = new DataTable();
                    MyAdapter.Fill(dTable);
                    var userAgent = Request.Headers["User-Agent"].ToString();
                    if (dTable.Rows.Count > 0)
                    {
                        foreach (DataRow item in dTable.Rows)
                        {
                            user_id = item["id"].ToString();
                            user_role = item["user_role"].ToString();
                        }
                        createLog.insertlogData(Username, "SUCCESS", userAgent, conn);
                        conn.Close();
                        return Ok(JsonConvert.SerializeObject(user_id + "," + user_role));
                    }
                    else
                    {
                        createLog.insertlogData(Username, "FAILED", userAgent, conn);
                        conn.Close();
                        return Ok(JsonConvert.SerializeObject(0));
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost]
        [Route("updateUserDetails")]
        //public async Task<IActionResult> updateUserDetails(Users users)
        public async Task<IActionResult> updateUserDetails(ViewModel.UsersVM _users)
        {
            try
            {

                ViewModel.Users users = new ViewModel.Users
                {
                    Id = _users.id,
                    InvitedId = Convert.ToInt32(_users.invited_id),
                    Email = _users.email,
                    Password = _users.password,
                    Address = _users.address,
                    CityName = _users.city_name,
                    CompanyName = _users.company_name,
                    CountryCode = _users.country_code,
                    CountryName = _users.country_name,
                    CvrNo = _users.cvr_no,
                    ZipCode = _users.zip_code,
                    ExpiryDate = _users.expiry_date,
                    FName = _users.f_name,
                    LName = _users.l_name,
                    PhoneNo = _users.phone_no,
                    ProfilePic = _users.profile_pic,
                    SignupStatus = _users.signup_status,
                    TotalContainer = Convert.ToInt32(_users.total_container.ToString()),
                    UserRole = _users.user_role,
                    Status = _users.status,
                    PasswordNew = _users.passwordNew,
                    CreatedAt = _users.created_at,
                };

                string pwd = "";
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand("select id,f_name,l_name,profile_pic,email,password,company_name,zip_code,city_name,country_name,cvr_no,country_code,phone_no,address,total_container,expiry_date from users where (email='" + users.Email + "' or company_name ='" + users.CompanyName + "') and id!='" + users.Id + "' ", conn);
                    SqlDataAdapter MyAdapter = new SqlDataAdapter();
                    MyAdapter.SelectCommand = cmd1;
                    DataTable dTable = new DataTable();
                    MyAdapter.Fill(dTable);
                    if (dTable != null && dTable.Rows.Count > 0)
                    {
                        return Ok(JsonConvert.SerializeObject("Email or Company Name already exists!"));
                    }
                    if (!string.IsNullOrEmpty(users.PasswordNew))
                    {
                        pwd = Base64Encode(users.PasswordNew);
                    }
                    else
                    {
                        pwd = Base64Encode(users.Password);
                    }
                    string sqlqry = "update users set updated_at=@updated_at ,invited_id=@invited_id ,expiry_date=@expiry_date ,total_container=@total_container ";



                    SqlCommand cmd = new SqlCommand();

                    if (!string.IsNullOrEmpty(users.Email))
                    {
                        sqlqry += ",email=@email ";
                        cmd.Parameters.AddWithValue("@email", users.Email);
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        sqlqry += ",password=@pwd ";
                        cmd.Parameters.AddWithValue("@pwd", pwd);
                    }
                    if (!string.IsNullOrEmpty(users.LName))
                    {
                        sqlqry += ",l_name=@l_name ";
                        cmd.Parameters.AddWithValue("@l_name", users.LName);
                    }
                    if (!string.IsNullOrEmpty(users.FName))
                    {
                        sqlqry += ",f_name=@f_name ";
                        cmd.Parameters.AddWithValue("@f_name", users.FName);
                    }
                    if (!string.IsNullOrEmpty(users.Address))
                    {
                        sqlqry += ",address=@address ";
                        cmd.Parameters.AddWithValue("@address", users.Address);
                    }
                    if (!string.IsNullOrEmpty(users.ZipCode))
                    {
                        sqlqry += ",zip_code=@zip_code ";
                        cmd.Parameters.AddWithValue("@zip_code", users.ZipCode);
                    }
                    if (!string.IsNullOrEmpty(users.CityName))
                    {
                        sqlqry += ",city_name=@city_name ";
                        cmd.Parameters.AddWithValue("@city_name", users.CityName);
                    }

                    if (!string.IsNullOrEmpty(users.CompanyName))
                    {
                        sqlqry += ",company_name=@company_name ";
                        cmd.Parameters.AddWithValue("@company_name", users.CompanyName);
                    }
                    if (!string.IsNullOrEmpty(users.CountryCode))
                    {
                        sqlqry += ",country_code=@country_code ";
                        cmd.Parameters.AddWithValue("@country_code", users.CountryCode);
                    }
                    if (!string.IsNullOrEmpty(users.CountryName))
                    {
                        sqlqry += ",country_name=@country_name ";
                        cmd.Parameters.AddWithValue("@country_name", users.CountryName);
                    }
                    if (!string.IsNullOrEmpty(users.CvrNo))
                    {
                        sqlqry += ",cvr_no=@cvr_no ";
                        cmd.Parameters.AddWithValue("@cvr_no", users.CvrNo);
                    }

                    if (!string.IsNullOrEmpty(users.PhoneNo))
                    {
                        sqlqry += ",phone_no=@phone_no ";
                        cmd.Parameters.AddWithValue("@phone_no", users.PhoneNo);
                    }
                    sqlqry += " WHERE id = @id";
                    cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                    cmd.Parameters.AddWithValue("@expiry_date", users.ExpiryDate);
                    cmd.Parameters.AddWithValue("@total_container", users.TotalContainer);
                    cmd.Parameters.AddWithValue("@invited_id", users.InvitedId);
                    cmd.Parameters.AddWithValue("@id", users.Id);
                    cmd.Connection = conn;
                    cmd.CommandText = sqlqry;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return Ok(JsonConvert.SerializeObject(1));
                }
            }
            catch (Exception e)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        [HttpPost]
        [Route("updateUserStatus")]
        public async Task<IActionResult> updateUserStatus(string userid, string status)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update users set status=@status where id=@id", conn);
                cmd.Parameters.AddWithValue("@id", userid);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();
                conn.Close();
                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        [HttpPost]
        [Route("deleteUser")]
        public async Task<IActionResult> deleteUser(string userid)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();

                SqlCommand cmd = new SqlCommand("delete from columns_show where user_id=@id", conn);
                cmd.Parameters.AddWithValue("@id", userid);
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("delete from company_alias where userid=@id", conn);
                cmd1.Parameters.AddWithValue("@id", userid);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("delete from container_apidata where userid=@id", conn);
                cmd2.Parameters.AddWithValue("@id", userid);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand("delete from email_logs where user_id=@id", conn);
                cmd3.Parameters.AddWithValue("@id", userid);
                cmd3.ExecuteNonQuery();

                SqlCommand cmd4 = new SqlCommand("delete from notifications_emails where user_id=@id", conn);
                cmd4.Parameters.AddWithValue("@id", userid);
                cmd4.ExecuteNonQuery();

                SqlCommand cmd5 = new SqlCommand("delete from payment where user_id=@id", conn);
                cmd5.Parameters.AddWithValue("@id", userid);
                cmd5.ExecuteNonQuery();

                SqlCommand cmd6 = new SqlCommand("delete from shipsgo_container1 where user_id=@id", conn);
                cmd6.Parameters.AddWithValue("@id", userid);
                cmd6.ExecuteNonQuery();

                SqlCommand cmd7 = new SqlCommand("delete from user_notifications where user_id=@id", conn);
                cmd7.Parameters.AddWithValue("@id", userid);
                cmd7.ExecuteNonQuery();

                SqlCommand cmd8 = new SqlCommand("delete from users where id=@id", conn);
                cmd8.Parameters.AddWithValue("@id", userid);
                cmd8.ExecuteNonQuery();



                conn.Close();
                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        [HttpPost]
        [Route("InsertUserDetails")]
        public async Task<IActionResult> insertUserDetails(ViewModel.UsersVM _users)
        {
            try
            {
                Models.Users users = new Models.Users
                {
                    Id = _users.id,
                    InvitedId = Convert.ToInt32(_users.invited_id),
                    Email = _users.email,
                    Password = _users.password,
                    Address = _users.address,
                    CityName = _users.city_name,
                    CompanyName = _users.company_name,
                    CountryCode = _users.country_code,
                    CountryName = _users.country_name,
                    CvrNo = _users.cvr_no,
                    ZipCode = _users.zip_code,
                    ExpiryDate = _users.expiry_date,
                    FName = _users.f_name,
                    LName = _users.l_name,
                    PhoneNo = _users.phone_no,
                    ProfilePic = _users.profile_pic,
                    SignupStatus = _users.signup_status,
                    TotalContainer = Convert.ToInt32(_users.total_container.ToString()),
                    UserRole = _users.user_role,
                    Status = _users.status,
                };
                users.ExpiryDate = DateTime.Now.AddYears(1);
                users.CreatedAt = DateTime.Now;
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("select id,f_name,l_name,profile_pic,email,password,company_name,zip_code,city_name,country_name,cvr_no,country_code,phone_no,address,total_container,expiry_date from users where email='" + users.Email + "' or company_name ='" + users.CompanyName + "' ", conn);
                SqlDataAdapter MyAdapter = new SqlDataAdapter();
                MyAdapter.SelectCommand = cmd1;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    return Ok(JsonConvert.SerializeObject("Email or Company Name already exists!"));
                }

                if (users.Password != null)
                {
                    users.Password = Base64Encode(users.Password);
                }
                else
                {
                    users.Password = "";
                }

                SqlCommand cmd = new SqlCommand("insert into users (invited_id,email,password,l_name,f_name,address,total_container,updated_at,user_role,zip_code,city_name,company_name,country_code,country_name,cvr_no,phone_no,profile_pic,status,signup_status,expiry_date,created_at) values(@invited_id,@email,@password,@l_name,@f_name,@address,@total_container,@updated_at,@user_role,@zip_code,@city_name,@company_name,@country_code,@country_name,@cvr_no,@phone_no,@profile_pic,@status,@signup_status,@expiry_date,@created_at);SELECT SCOPE_IDENTITY () As id;", conn);
                cmd.Parameters.AddWithValue("@invited_id", users.InvitedId);

                cmd.Parameters.AddWithValue("@email", users.Email ?? string.Empty);
                cmd.Parameters.AddWithValue("@password", users.Password);
                cmd.Parameters.AddWithValue("@l_name", users.LName ?? string.Empty);
                cmd.Parameters.AddWithValue("@f_name", users.FName ?? string.Empty);
                cmd.Parameters.AddWithValue("@address", users.Address ?? string.Empty);
                cmd.Parameters.AddWithValue("@total_container", users.TotalContainer);
                cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                cmd.Parameters.AddWithValue("@user_role", users.UserRole);
                cmd.Parameters.AddWithValue("@zip_code", users.ZipCode ?? string.Empty);
                cmd.Parameters.AddWithValue("@city_name", users.CityName ?? string.Empty);
                cmd.Parameters.AddWithValue("@company_name", users.CompanyName ?? string.Empty);
                cmd.Parameters.AddWithValue("@country_code", users.CountryCode ?? string.Empty);
                cmd.Parameters.AddWithValue("@country_name", users.CountryName ?? string.Empty);
                cmd.Parameters.AddWithValue("@cvr_no", users.CvrNo ?? string.Empty);
                cmd.Parameters.AddWithValue("@phone_no", users.PhoneNo ?? string.Empty);
                cmd.Parameters.AddWithValue("@profile_pic", users.ProfilePic ?? string.Empty);

                cmd.Parameters.AddWithValue("@status", users.Status);
                cmd.Parameters.AddWithValue("@signup_status", users.SignupStatus);
                cmd.Parameters.AddWithValue("@expiry_date", users.ExpiryDate);
                cmd.Parameters.AddWithValue("@created_at", DateTime.Now);




                SqlDataReader MyReader2;
                MyReader2 = cmd.ExecuteReader();
                if (MyReader2.HasRows)
                {
                    MyReader2.Read();
                    int newRowID = Convert.ToInt32(MyReader2["id"]);
                    SettingsVM settings = new SettingsVM();
                    settings.user_id = newRowID;
                    settings.shipment_ref = 1;
                    settings.origin = 1;
                    settings.container_type = 1;
                    settings.destination = 1;
                    settings.container_no = 1;
                    settings.departure = 1;
                    settings.arrival = 1;
                    settings.shipping_line = 1;
                    settings.status = 1;
                    settings.early_delay = 1;
                    settings.from_country = 1;
                    settings.to_country = 1;
                    settings.transit_time = 1;
                    settings.first_eta = 1;
                    settings.bl_reference_no = 1;
                    settings.transit_ports = 1;
                    settings.getout_date = 1;
                    settings.empty_return_date = 1;
                    settings.shipment_by = 1;
                    settings.first_arrival = 1;
                    settings.days_before_arrival = 1;
                    settings.vessel_name = 1;
                    ColumnsShowController obj = new ColumnsShowController();
                    await obj.updateSettingsdetails(settings);

                    NotificationsVM notifications = new NotificationsVM();
                    NotificationsEmails notificationsemails = new NotificationsEmails();
                    notifications.user_id = Convert.ToString(newRowID);
                    notifications.ConAddSts = 1;
                    notifications.ConAddTime = 1;
                    notifications.DepChangeSts = 1;
                    notifications.DepChangeTime = 1;
                    notifications.ArrChangeSts = 1;
                    notifications.ArrChangeTime = 1;
                    notifications.ConDelSts = 1;
                    notifications.ConDelTime = 1;
                    notifications.CreatedAt = DateTime.Now;
                    notifications.UpdatedAt = DateTime.Now;

                    notificationsemails.UserId = newRowID;
                    notificationsemails.ConAddEmails = users.Email;
                    notificationsemails.DepChangeEmails = users.Email;
                    notificationsemails.ArrChangeEmails = users.Email;
                    notificationsemails.ConDelEmails = users.Email;
                    notificationsemails.ConTimeoutEmails = users.Email;
                    notificationsemails.UntilArrivalEmails = users.Email;

                    notifications.ConTimeoutSendsts = 1;
                    notifications.ConTimeoutSts = 1;
                    notifications.ConTimeoutTime = 1;
                    notifications.ConUntilarrivalByEmail = 1;
                    notifications.ConUntilarrivalDays = 4;

                    UserNotificationsController objUserNotifications = new UserNotificationsController();
                    NotificationsEmailsController objNotificationsEmails = new NotificationsEmailsController();

                    await objUserNotifications.updateNotificationsdetails(notifications);
                    await objNotificationsEmails.updateNotificationsEmails(notificationsemails);

                    CompanyAliasController objCompanyAlias = new CompanyAliasController();
                    await objCompanyAlias.insertCompanyAlias(newRowID.ToString(), users.CompanyName, users.InvitedId.ToString());
                    EmailController objEmail = new EmailController();
                    objEmail.sendnewUserAddedEmail(users.Email, users.CompanyName);
                }
                conn.Close();
                return Ok(JsonConvert.SerializeObject(1));
            }

            catch (Exception ex)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        public async Task<IActionResult> getUserdata(string user_id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from notifications_emails where user_id=" + user_id);

                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        [HttpGet]
        [Route("getuserlogDetails")]
        public async Task<IActionResult> getuserlogDetails()
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            List<CreateLog> lstuserlog = new List<CreateLog>();
            SqlCommand cmd = new SqlCommand("select * from create_log order by login_time desc", conn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    CreateLog userlog = new CreateLog();
                    userlog.Id = Convert.ToInt32(reader["id"].ToString());
                    userlog.Email = reader["email"].ToString();
                    // userlog.login_time = reader.GetSqlDateTime("login_time").IsValidDateTime ? (DateTime?)reader["login_time"] : null;
                    userlog.LoginTime = reader.GetDateTime(reader.GetOrdinal("login_time"));
                    userlog.LogoutTime = reader.GetDateTime(reader.GetOrdinal("logout_time"));
                    userlog.LoginResult = reader["login_result"].ToString();
                    userlog.UserAgent = reader["user_agent"].ToString();
                    userlog.SourceIp = reader["source_ip"].ToString();
                    lstuserlog.Add(userlog);
                }
            }

            conn.Close();
            return Ok(JsonConvert.SerializeObject(lstuserlog));
        }
        [HttpGet]
        [Route("getalluserDetails")]
        public async Task<IActionResult> getalluserDetails()
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            List<Models.Users> user = new List<Models.Users>();
            SqlCommand cmd = new SqlCommand("select * from users", conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Models.Users user1 = new Models.Users();
                    user1.Id = Convert.ToInt32(reader["id"].ToString());
                    user1.InvitedId = Convert.ToInt32(reader["invited_id"].ToString());
                    user1.Email = reader["email"].ToString();
                    user1.FName = reader["f_name"].ToString();
                    user1.LName = reader["l_name"].ToString();
                    user1.Password = Base64Decode(reader["password"].ToString());
                    user1.CompanyName = reader["company_name"].ToString();
                    user1.ZipCode = reader["zip_code"].ToString();
                    user1.CityName = reader["city_name"].ToString();
                    user1.CountryName = reader["country_name"].ToString();
                    user1.CvrNo = reader["cvr_no"].ToString();
                    user1.CountryCode = reader["country_code"].ToString();
                    user1.PhoneNo = reader["phone_no"].ToString();
                    user1.Address = reader["address"].ToString();
                    user1.ProfilePic = reader["profile_pic"].ToString();
                    user1.UserRole = Convert.ToInt32(reader["user_role"].ToString());
                    user1.Status = Convert.ToInt16(reader["status"].ToString());
                    user1.SignupStatus = Convert.ToInt16(reader["signup_status"].ToString());
                    user1.TotalContainer = Convert.ToInt32(reader["total_container"].ToString());
                    user1.ExpiryDate = reader.GetDateTime(reader.GetOrdinal("expiry_date"));
                    user1.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"));
                    user1.UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"));
                    user.Add(user1);
                }
            }

            conn.Close();
            return Ok(JsonConvert.SerializeObject(user));
        }
        [HttpPost]
        [Route("UpdateUserList")]
        public void UpdateUserList(string container_no, string name, string po_no, string user_id)
        {
            //string userid = "";
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            //SqlCommand cmdApi = new SqlCommand("SELECT TOP 1 * FROM users where company_name='" + name + "'", conn);
            //SqlDataAdapter MyAdapter1 = new SqlDataAdapter();
            //MyAdapter1.SelectCommand = cmdApi;
            //DataTable dTab = new DataTable();
            //MyAdapter1.Fill(dTab);
            //if (dTab != null && dTab.Rows.Count > 0)
            //{
            //    foreach (DataRow item1 in dTab.Rows)
            //    {
            //        userid = item1["id"].ToString();
            //    }
            //}
            SqlCommand cmd = new SqlCommand("update shipsgo_container1 set user_id='" + name + "' where po_no ='" + po_no + "' and container_no='" + container_no + "' and user_id='" + user_id + "'", conn);
            int i = cmd.ExecuteNonQuery();
            conn.Close();

        }
        [HttpGet]
        [Route("getUserDetails")]
        public async Task<IActionResult> getUserDetails(string id)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string user_id = "";
            user_id = id.Trim('"');
            user_id = user_id.Trim('/');
            ViewModel.UsersVM users = new ViewModel.UsersVM();
            SqlCommand cmd = new SqlCommand("select id,invited_id,created_at,created_at,f_name,l_name,status,profile_pic,email,password,company_name,zip_code,city_name,country_name,cvr_no,country_code,phone_no,address,total_container,expiry_date from users where id='" + user_id + "' ", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            users = SetUserVMDetails(dTable);
            conn.Close();
            return Ok(JsonConvert.SerializeObject(users));
        }

        private ViewModel.UsersVM SetUserVMDetails(DataTable dTable)
        {
            ViewModel.UsersVM users = new ViewModel.UsersVM();
            if (dTable != null && dTable.Rows.Count > 0)
            {

                foreach (DataRow item in dTable.Rows)
                {
                    users.id = Convert.ToInt32(item["id"].ToString());
                    users.invited_id = Convert.ToString(item["invited_id"]);
                    users.status = Convert.ToInt16(item["status"]);
                    users.l_name = item["l_name"].ToString();
                    users.f_name = item["f_name"].ToString();
                    users.address = item["address"].ToString();
                    users.total_container = Convert.ToInt32(item["total_container"].ToString());
                    users.zip_code = item["zip_code"].ToString();
                    users.city_name = item["city_name"].ToString();
                    users.company_name = item["company_name"].ToString();
                    users.country_code = item["country_code"].ToString();
                    users.country_name = item["country_name"].ToString();
                    users.email = item["email"].ToString();
                    users.password = Base64Decode(item["password"].ToString());
                    users.cvr_no = item["cvr_no"].ToString();
                    users.phone_no = item["phone_no"].ToString();
                    users.profile_pic = item["profile_pic"].ToString();
                    users.expiry_date = Convert.ToDateTime(item["expiry_date"].ToString());
                    users.created_at = Convert.ToDateTime(item["created_at"].ToString());
                }

            }
            return users;
        }


        private Models.Users setUserDetails(DataTable dTable)
        {
            Models.Users users = new Models.Users();
            if (dTable != null && dTable.Rows.Count > 0)
            {

                foreach (DataRow item in dTable.Rows)
                {
                    users.Id = Convert.ToInt32(item["id"].ToString());
                    users.LName = item["l_name"].ToString();
                    users.FName = item["f_name"].ToString();
                    users.Address = item["address"].ToString();
                    users.TotalContainer = Convert.ToInt32(item["total_container"].ToString());
                    users.ZipCode = item["zip_code"].ToString();
                    users.CityName = item["city_name"].ToString();
                    users.CompanyName = item["company_name"].ToString();
                    users.CountryCode = item["country_code"].ToString();
                    users.CountryName = item["country_name"].ToString();
                    users.Email = item["email"].ToString();
                    users.Password = item["password"].ToString();
                    users.CvrNo = item["cvr_no"].ToString();
                    users.PhoneNo = item["phone_no"].ToString();
                    users.ProfilePic = item["profile_pic"].ToString();
                    users.ExpiryDate = Convert.ToDateTime(item["expiry_date"].ToString());
                }

            }
            return users;
        }
        #endregion

        //[HttpGet]
        //[Route("getuserlogDetails1")]
        //public async Task<IActionResult> getuserlogDetails1()
        //{
        //    return Ok(JsonConvert.SerializeObject(1));
        //}
    }
}