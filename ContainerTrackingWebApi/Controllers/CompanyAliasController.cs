using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    public class CompanyAliasController : ControllerBase
    {
        string connection = Startup.GetConnectionString();

        /// <summary>
        /// inserts company alias details
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="name"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insertCompanyAlias")]
        public async Task<IActionResult> insertCompanyAlias(string userid, string name, string createdBy)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into company_alias (userid,name,isDeleted,createdBy,createdDate) values(@userid,@name,@isDeleted,@createdBy,@createdDate)", conn);

                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@isDeleted", 0);
                cmd.Parameters.AddWithValue("@createdBy", createdBy);
                cmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
                SqlDataReader MyReader2;
                MyReader2 = cmd.ExecuteReader();
                MyReader2.Close();
                return Ok(JsonConvert.SerializeObject("1"));
            }
            catch (Exception ex)
            {

                return Ok(JsonConvert.SerializeObject(ex.Message));
            }
        }

        /// <summary>
        /// update company alias details
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createdBy"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateCompanyAlias")]
        public async Task<IActionResult> updateCompanyAlias(string name, string createdBy, string userid)
        {
            try
            {

                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("select * from company_alias where name ='" + name + "' and createdBy!='" + createdBy + "' ", conn);
                SqlDataAdapter MyAdapter = new SqlDataAdapter();
                MyAdapter.SelectCommand = cmd1;
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    return Ok(JsonConvert.SerializeObject("Company Name already exists!"));
                }
                if (createdBy == userid)
                {
                    SqlCommand cmd = new SqlCommand("update company_alias set name=@name,modifiedDate=@modifiedDate where userid=@userid", conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("update company_alias set name=@name,modifiedDate=@modifiedDate where createdBy=@createdBy and userid=@userid", conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@createdBy", createdBy);
                    cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now);
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


        /// <summary>
        /// delete company alias details
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteCompanyAliasDetails")]
        public async Task<IActionResult> deleteCompanyAliasDetails(string userid, string createdBy)
        {
            try
            {
                if (createdBy == userid)
                {
                    createdBy = "0";
                }
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update company_alias set isDeleted=1 where userid=@userid and createdBy=@createdBy", conn);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@createdBy", createdBy);
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