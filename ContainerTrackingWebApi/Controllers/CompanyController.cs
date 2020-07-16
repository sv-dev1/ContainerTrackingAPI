using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ContainerTrackingWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContainerTrackingWebApi.Controllers
{

    [Route("api/AccountAPI")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        string connection = Startup.GetConnectionString();
        #region Company
        /// <summary>
        /// method updates company information in the DB
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="companyname"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateCompanyDetails")]
        public async Task<IActionResult> updateCompanyDetails(string userid, string firstname, string lastname, string companyname)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update users set f_name=@f_name,l_name=@l_name,company_name=@company_name where id=@id", conn);
                cmd.Parameters.AddWithValue("@id", userid);
                cmd.Parameters.AddWithValue("@f_name", firstname);
                cmd.Parameters.AddWithValue("@l_name", lastname);
                cmd.Parameters.AddWithValue("@company_name", companyname);
                cmd.ExecuteNonQuery();
                conn.Close();
                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }


        /// <summary>
        /// method returns company details by name
        /// </summary>
        /// <param name="company_name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCompanyDetailsByName")]
        public async Task<IActionResult> getCompanyDetailsByName(string company_name)
        {
            try
            {

                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                Users users = new Users();                
                SqlCommand cmd = new SqlCommand("select us.id,us.f_name,us.l_name,us.profile_pic,us.email,us.password,us.company_name,us.zip_code,us.city_name,us.country_name,us.cvr_no,us.country_code,us.phone_no,us.address,us.total_container,us.expiry_date from users us join company_alias ca on ca.userid=us.id where ca.name='" + company_name + "'", conn);
                SqlDataAdapter MyAdapter = new SqlDataAdapter();
                MyAdapter.SelectCommand = cmd;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                users = setUserDetails(dTable);
                conn.Close();
                return Ok(JsonConvert.SerializeObject(users));
            }
            catch (Exception ex)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        private Users setUserDetails(DataTable dTable)
        {
            Users users = new Users();
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
        /// <summary>
        /// method returns company details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCompanyDetails")]
        public async Task<IActionResult> getCompanyDetailsById(string id)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            List<string> lst = new List<string>();
            SqlCommand cmd = new SqlCommand("SELECT ca.userid,ca.name from company_alias ca join users us on ca.userid=us.id WHERE (us.id = '" + id + "' or us.invited_id = '" + id + "') and ca.isDeleted=0", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            lst.Add("All Containers");
            DataRow newRow = dTable.NewRow();
            newRow[0] = "0";
            newRow[1] = "All Containers";
            dTable.Rows.InsertAt(newRow, 0);
            conn.Close();
            return Ok(JsonConvert.SerializeObject(dTable));
        }
        #endregion
    }
}