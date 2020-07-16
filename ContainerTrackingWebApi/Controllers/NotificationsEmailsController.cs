using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ContainerTrackingWebApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContainerTrackingWebApi.Controllers
{   
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class NotificationsEmailsController : ControllerBase
    {
        string connection = Startup.GetConnectionString();

        [HttpGet]
        [Route("getNotificationsEmails")]
        public async Task<IActionResult> getNotificationsEmails(string userid)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            NotificationsEmails notifications = new NotificationsEmails();
            string user_id = "";
            user_id = userid.Trim('"');
            user_id = user_id.Trim('/');            
            SqlCommand cmd1 = new SqlCommand("select * from notifications_emails where user_id='" + user_id + "' ", conn);
           
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            MyAdapter.SelectCommand = cmd1;
            DataTable dTable1 = new DataTable();
            MyAdapter.Fill(dTable1);
            if (dTable1 != null && dTable1.Rows.Count > 0)
            {
                foreach (DataRow item in dTable1.Rows)
                {
                    notifications.ConAddEmails = item["con_add_emails"].ToString();
                    notifications.DepChangeEmails = item["dep_change_emails"].ToString();
                    notifications.ArrChangeEmails = item["arr_change_emails"].ToString();
                    notifications.ConDelEmails = item["con_del_emails"].ToString();
                    notifications.ConTimeoutEmails = item["con_timeout_emails"].ToString();
                    notifications.UntilArrivalEmails = item["until_arrival_emails"].ToString();
                }
            }
            conn.Close();
            return Ok(JsonConvert.SerializeObject(notifications));
        }


        [HttpPost]
        [Route("updateNotificationsEmails")]
        public async Task<IActionResult> updateNotificationsEmails(NotificationsEmails notificationsemails)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                string useid = "";
                string userid = Convert.ToString(notificationsemails.UserId);
                useid = userid.Trim('"');
                useid = useid.Trim('/');
                SqlCommand cmd1 = new SqlCommand("select * from notifications_emails where user_id='" + useid + "' ", conn);
                SqlDataAdapter MyAdapter = new SqlDataAdapter();
                MyAdapter.SelectCommand = cmd1;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);

                if (dTable.Rows.Count == 0)
                {
                    SqlCommand cmd2 = new SqlCommand("insert into notifications_emails (con_add_emails,dep_change_emails,arr_change_emails,con_del_emails,con_timeout_emails,created_at,user_id,updated_at,until_arrival_emails) values (@con_add_emails,@dep_change_emails,@arr_change_emails,@con_del_emails,@con_timeout_emails,@created_at,@user_id,@updated_at,@until_arrival_emails);", conn);
                    cmd2.Parameters.AddWithValue("@user_id", useid);
                    cmd2.Parameters.AddWithValue("@con_add_emails", notificationsemails.ConAddEmails);
                    cmd2.Parameters.AddWithValue("@dep_change_emails", notificationsemails.DepChangeEmails);
                    cmd2.Parameters.AddWithValue("@arr_change_emails", notificationsemails.ArrChangeEmails);
                    cmd2.Parameters.AddWithValue("@con_del_emails", notificationsemails.ConDelEmails);
                    cmd2.Parameters.AddWithValue("@con_timeout_emails", notificationsemails.ConTimeoutEmails);
                    cmd2.Parameters.AddWithValue("@until_arrival_emails", notificationsemails.UntilArrivalEmails);
                    cmd2.Parameters.AddWithValue("@created_at", DateTime.Now);
                    cmd2.Parameters.AddWithValue("@updated_at", DateTime.Now);                  
                    SqlDataReader MyReader3;
                    MyReader3 = cmd2.ExecuteReader();
                }
                else
                {
                    SqlCommand cmd2 = new SqlCommand("update notifications_emails set con_add_emails=@con_add_emails,dep_change_emails=@dep_change_emails,arr_change_emails=@arr_change_emails,con_del_emails=@con_del_emails,con_timeout_emails=@con_timeout_emails,updated_at=@updated_at,until_arrival_emails=@until_arrival_emails where user_id=@user_id", conn);
                    cmd2.Parameters.AddWithValue("@user_id", useid);
                    cmd2.Parameters.AddWithValue("@con_add_emails", notificationsemails.ConAddEmails);
                    cmd2.Parameters.AddWithValue("@dep_change_emails", notificationsemails.DepChangeEmails);
                    cmd2.Parameters.AddWithValue("@arr_change_emails", notificationsemails.ArrChangeEmails);
                    cmd2.Parameters.AddWithValue("@con_del_emails", notificationsemails.ConDelEmails);
                    cmd2.Parameters.AddWithValue("@con_timeout_emails", notificationsemails.ConTimeoutEmails);
                    cmd2.Parameters.AddWithValue("@until_arrival_emails", notificationsemails.UntilArrivalEmails);
                    cmd2.Parameters.AddWithValue("@updated_at", DateTime.Now);
                    cmd2.ExecuteNonQuery();
                }

                conn.Close();
                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
            
        }
    }
}