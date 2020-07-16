using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContainerTrackingWebApi.Controllers
{
    [Route("api/AccountAPI")]
    [ApiController]
    public class ShippingLineController : ControllerBase
    {
        string connection = Startup.GetConnectionString();
        /// <summary>
        /// method returns shipping link and name fetching from the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string getShippingline(string name, int id)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string link = "";
            if (id == 0 && name != "")
            {
                SqlCommand cmd = new SqlCommand("select * from sealine where Name='" + name + "'", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() != "")
                        {
                            link = reader["name"].ToString() + "," + reader["link"].ToString() + "," + reader["Id"].ToString();
                        }
                        else
                        {
                            link = "null";
                        }
                    }
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select * from sealine where Id='" + id + "'", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() != "")
                        {
                            link = reader["name"].ToString() + "," + reader["link"].ToString();
                        }
                        else
                        {
                            link = "null";
                        }
                    }
                }
            }

            conn.Close();
            if (link == "")
            {
                return "" + "," + "null";
            }
            else
            {
                return link;
            }
        }

        /// <summary>
        /// method returns all the shipping lines from the database
        /// </summary>
        [HttpGet]
        [Route("getallShippingLines")]
        public async Task<IActionResult> getallShippingLines()
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from sealine", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            conn.Close();
            return Ok(JsonConvert.SerializeObject(dTable));
        }
    }
}