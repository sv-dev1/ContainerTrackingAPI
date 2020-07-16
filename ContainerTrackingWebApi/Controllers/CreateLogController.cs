using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContainerTrackingWebApi.Controllers
{
    [Route("api/AccountAPI")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class CreateLogController : ControllerBase
    {
        string connection = Startup.GetConnectionString();

        //this method inserts information about when user logined, logout etc 
        [NonAction]
        public void insertlogData(string Username, string result,string userAgent)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into create_log (email,login_time,logout_time,login_result,user_agent,source_ip) values(@email,@login_time,@logout_time,@login_result,@user_agent,@source_ip)", conn);
            cmd.Parameters.AddWithValue("@email", Username);
            cmd.Parameters.AddWithValue("@login_time", DateTime.Now);
            cmd.Parameters.AddWithValue("@logout_time", DateTime.Now);
            cmd.Parameters.AddWithValue("@login_result", result);
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //var userAgent = Request.Headers["User-Agent"].ToString();
            cmd.Parameters.AddWithValue("@user_agent", userAgent);
            cmd.Parameters.AddWithValue("@source_ip", myIP);
            SqlDataReader MyReader2;
            MyReader2 = cmd.ExecuteReader();
            conn.Close();
        }
        //this method updates user logout time 
        [HttpPost]
        [Route("updateUserlog")]
        public async Task<IActionResult> updateUserlog(string email)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update create_log set logout_time=@logout_time where email=@email order by id desc limit 1", conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@logout_time", DateTime.Now);
                cmd.ExecuteNonQuery();
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