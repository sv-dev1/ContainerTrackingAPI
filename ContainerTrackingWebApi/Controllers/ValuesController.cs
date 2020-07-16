using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ContainerTrackingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContainerTrackingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        string connection = Startup.GetConnectionString();
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
    }
}
