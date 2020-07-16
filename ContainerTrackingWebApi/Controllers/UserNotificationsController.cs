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
    [Route("api/AccountAPI")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class UserNotificationsController : ControllerBase
    {
        string connection = Startup.GetConnectionString();
        [HttpPost]
        [Route("updateNotificationsdetails")]        
        public async Task<IActionResult> updateNotificationsdetails(UserNotifications notifications)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                string useid = "";
                string userid = Convert.ToString(notifications.UserId);
                useid = userid.Trim('"');
                useid = useid.Trim('/');
                SqlCommand cmd1 = new SqlCommand("select * from user_notifications where user_id='" + useid + "' ", conn);
                SqlDataAdapter MyAdapter = new SqlDataAdapter();
                MyAdapter.SelectCommand = cmd1;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                if (dTable.Rows.Count == 0)
                {
                    SqlCommand cmd = new SqlCommand("insert into user_notifications (con_add_sts,con_add_time,dep_change_sts,dep_change_time,arr_change_sts,arr_change_time,con_del_sts,con_del_time,con_timeout_sts,con_timeout_time,created_at,user_id,updated_at,con_untilarrival_by_email,con_untilarrival_days) values (@con_add_sts,@con_add_time,@dep_change_sts,@dep_change_time,@arr_change_sts,@arr_change_time,@con_del_sts,@con_del_time,@con_timeout_sts,@con_timeout_time,@created_at,@user_id,@updated_at,@con_untilarrival_by_email,@con_untilarrival_days);", conn);
                    cmd.Parameters.AddWithValue("@user_id", useid);
                    cmd.Parameters.AddWithValue("@con_add_sts", notifications.ConAddSts);
                    cmd.Parameters.AddWithValue("@con_add_time", notifications.ConAddTime);
                    cmd.Parameters.AddWithValue("@dep_change_sts", notifications.DepChangeSts);
                    cmd.Parameters.AddWithValue("@dep_change_time", notifications.DepChangeTime);
                    cmd.Parameters.AddWithValue("@arr_change_sts", notifications.ArrChangeSts);
                    cmd.Parameters.AddWithValue("@arr_change_time", notifications.ArrChangeTime);
                    cmd.Parameters.AddWithValue("@con_del_sts", notifications.ConDelSts);
                    cmd.Parameters.AddWithValue("@con_del_time", notifications.ConDelTime);
                    cmd.Parameters.AddWithValue("@con_timeout_sts", notifications.ConTimeoutSts);
                    cmd.Parameters.AddWithValue("@con_timeout_time", notifications.ConTimeoutTime);
                    cmd.Parameters.AddWithValue("@con_untilarrival_by_email", notifications.ConUntilarrivalByEmail);
                    cmd.Parameters.AddWithValue("@con_untilarrival_days", notifications.ConUntilarrivalDays);
                    cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                    cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                    SqlDataReader MyReader2;
                    MyReader2 = cmd.ExecuteReader();                   
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("update user_notifications set con_add_sts=@con_add_sts,con_add_time=@con_add_time,dep_change_sts=@dep_change_sts," +
                    "dep_change_time=@dep_change_time,arr_change_sts=@arr_change_sts,arr_change_time=@arr_change_time,con_del_sts=@con_del_sts,con_del_time=@con_del_time," +
                    "updated_at=@updated_at,con_timeout_sts=@con_timeout_sts,con_timeout_time=@con_timeout_time,con_untilarrival_by_email=@con_untilarrival_by_email,con_untilarrival_days=@con_untilarrival_days where user_id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", useid);
                    cmd.Parameters.AddWithValue("@con_add_sts", notifications.ConAddSts);
                    cmd.Parameters.AddWithValue("@con_add_time", notifications.ConAddTime);
                    cmd.Parameters.AddWithValue("@dep_change_sts", notifications.DepChangeSts);
                    cmd.Parameters.AddWithValue("@dep_change_time", notifications.DepChangeTime);
                    cmd.Parameters.AddWithValue("@arr_change_sts", notifications.ArrChangeSts);
                    cmd.Parameters.AddWithValue("@arr_change_time", notifications.ArrChangeTime);
                    cmd.Parameters.AddWithValue("@con_del_sts", notifications.ConDelSts);
                    cmd.Parameters.AddWithValue("@con_del_time", notifications.ConDelTime);
                    cmd.Parameters.AddWithValue("@con_timeout_sts", notifications.ConTimeoutSts);
                    cmd.Parameters.AddWithValue("@con_timeout_time", notifications.ConTimeoutTime);
                    cmd.Parameters.AddWithValue("@con_untilarrival_by_email", notifications.ConUntilarrivalByEmail);
                    cmd.Parameters.AddWithValue("@con_untilarrival_days", notifications.ConUntilarrivalDays);
                    cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                    cmd.ExecuteNonQuery();                    
                }
                conn.Close();
                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        [HttpGet]
        [Route("getNotificationsDetails")]
        public async Task<IActionResult> getNotificationsDetails(string userid)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            UserNotifications notifications = new UserNotifications();
            string user_id = "";
            user_id = userid.Trim('"');
            user_id = user_id.Trim('/');
            SqlCommand cmd = new SqlCommand("select * from user_notifications where user_id='" + user_id + "' ", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow item in dTable.Rows)
                {
                    notifications.ConAddSts = Convert.ToInt16(item["con_add_sts"].ToString());
                    notifications.ConAddTime = Convert.ToInt16(item["con_add_time"].ToString());
                    notifications.DepChangeSts = Convert.ToInt16(item["dep_change_sts"].ToString());
                    notifications.DepChangeTime = Convert.ToInt16(item["dep_change_time"].ToString());
                    notifications.ArrChangeSts = Convert.ToInt16(item["arr_change_sts"].ToString());
                    notifications.ArrChangeTime = Convert.ToInt16(item["arr_change_time"].ToString());
                    notifications.ConTimeoutSts = Convert.ToInt16(item["con_timeout_sts"].ToString());
                    notifications.ConTimeoutTime = Convert.ToInt16(item["con_timeout_time"].ToString());
                    notifications.ConDelSts = Convert.ToInt16(item["con_del_sts"].ToString());
                    notifications.ConDelTime = Convert.ToInt16(item["con_del_time"].ToString());
                    notifications.CreatedAt = Convert.ToDateTime(item["created_at"].ToString());
                    notifications.UpdatedAt = Convert.ToDateTime(item["updated_at"].ToString());
                    notifications.ConUntilarrivalByEmail = Convert.ToInt16(item["con_untilarrival_by_email"].ToString());
                    notifications.ConUntilarrivalDays = Convert.ToInt16(item["con_untilarrival_days"].ToString());
                }
            }

            conn.Close();
            return Ok(JsonConvert.SerializeObject(notifications));
        }
    }
}