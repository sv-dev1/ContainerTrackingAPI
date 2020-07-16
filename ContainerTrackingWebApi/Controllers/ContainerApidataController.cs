//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ContainerTrackingWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [EnableCors("MyPolicy")]
//    [ApiController]
//    public class ContainerApidataController : ControllerBase
//    {
//        string connection = Startup.GetConnectionString();
//        ShippingLineController objShippingLine = new ShippingLineController();
//        shipsgo_containerController obj = new shipsgo_containerController();



//        /// <summary>
//        /// This method is used to get information about container's api data already stored in database
//        /// </summary>
//        /// <param name="container_no"></param>
//        /// <param name="flag"></param>
//        /// <param name="responseText"></param>
//        //public void getContainerApiData(string container_no, out int flag, out string responseText)
//        //{
//        //    responseText = "";
//        //    flag = 1;
//        //    try
//        //    {
//        //        SqlConnection conn = new SqlConnection(connection);
//        //        conn.Open();
//        //        SqlCommand cmdApi = new SqlCommand("SELECT * FROM container_apidata where container_no='" + container_no + "'", conn);
//        //        cmdApi.CommandTimeout = 30000;
//        //        SqlDataAdapter MyAdapter1 = new SqlDataAdapter();
//        //        MyAdapter1.SelectCommand = cmdApi;
//        //        DataTable dTab = new DataTable();
//        //        MyAdapter1.Fill(dTab);
//        //        if (dTab != null && dTab.Rows.Count > 0)
//        //        {
//        //            foreach (DataRow item1 in dTab.Rows)
//        //            {
//        //                string date1 = item1["day"].ToString();
//        //                string status = item1["status"].ToString();
//        //                string date2 = DateTime.Now.ToString("dd-MMM-yyyy");
//        //                if (status == "1")
//        //                {
//        //                    //if status = 1 i.e container hasn't reached yet its destination and call is made at start of day and after 12 hours
//        //                    if (Convert.ToDateTime(item1["day"].ToString()) == Convert.ToDateTime(date2))
//        //                    {
//        //                        if (Convert.ToDateTime(item1["time"].ToString()).AddHours(12).TimeOfDay > DateTime.Now.TimeOfDay)
//        //                        {
//        //                            flag = 0;
//        //                            responseText = item1["apiData"].ToString();
//        //                        }
//        //                    }
//        //                }
//        //                else if (status == "0")
//        //                {
//        //                    //if status = 0, means container data was not proper and call is made to get fresh data after 2 days
//        //                    if (Convert.ToDateTime(item1["day"].ToString()).AddDays(2) <= Convert.ToDateTime(date2))
//        //                    {
//        //                        flag = 1;
//        //                    }
//        //                    else
//        //                    {
//        //                        responseText = item1["apiData"].ToString();
//        //                        flag = 0;
//        //                    }
//        //                }
//        //                else if (status == "3")
//        //                {
//        //                    //if status = 0, means container has reached its destination and no call to Api is done and data is fetched from database
//        //                    responseText = item1["apiData"].ToString();
//        //                    flag = 0;
//        //                }
//        //            }
//        //        }
//        //        else
//        //        {
//        //            responseText = "DATA_NOT_FOUND";
//        //        }
//        //        conn.Close();
//        //    }
//        //    catch (Exception)
//        //    {

//        //    }
//        //}

//        //public void getDataFromApi(string user_id, string container_no, string shipping_line, out string responseText)
//        //{
//        //    try
//        //    {
//        //        string baseUrlAddress = "";
//        //        //if else checks shipping_line values and call to api is made accordingly
//        //        if (shipping_line == "Detect Automatically" || shipping_line == "")
//        //        {
//        //            baseUrlAddress = "https://sirius.searates.com/tracking/api?code=" + container_no + "&api_key=viytu6yterhgfioriufhjgvoruvi2vh";
//        //        }
//        //        else
//        //        {
//        //            string shipLine = shipping_line;

//        //            string Id = objShippingLine.getShippingline(shipping_line, 0).Split(',')[2];
//        //            baseUrlAddress = "https://sirius.searates.com/tracking/api?sealine=" + Id + "&code=" + container_no + "&api_key=viytu6yterhgfioriufhjgvoruvi2vh";
//        //        }
//        //        WebRequest request = HttpWebRequest.Create(baseUrlAddress);
//        //        WebResponse response = request.GetResponse();
//        //        StreamReader reader = new StreamReader(response.GetResponseStream());
//        //        responseText = reader.ReadToEnd();
//        //        //Update method is called to update database with new data got from searates api
//        //        updateContainerapiData(user_id, responseText, container_no);
//        //    }
//        //    catch (Exception)
//        //    {
//        //        obj.getContainerApiData(container_no, out int flag, out responseText);
//        //    }
//        //}

//        //private void updateContainerapiData(string user_id, string v, string container_no)
//        //{
//        //    SqlConnection conn = new SqlConnection(connection);
//        //    conn.Open();
//        //    string status;
//        //    if (v.Contains("DATA_NOT_FOUND") == true || v.Contains("ONLY_TEXT") == true || v.Contains("WRONG_CONTAINER_NUMBER") == true)
//        //    {
//        //        status = "0";
//        //    }
//        //    else
//        //    {
//        //        status = "1";
//        //    }
//        //    SqlCommand cmd1 = new SqlCommand("SELECT * from container_apidata where container_no='" + container_no + "'", conn);
//        //    SqlDataAdapter MyAdapter = new SqlDataAdapter();
//        //    MyAdapter.SelectCommand = cmd1;
//        //    DataTable dTable = new DataTable();
//        //    MyAdapter.Fill(dTable);
//        //    if (dTable.Rows.Count == 0)
//        //    {
//        //        SqlCommand cmd = new SqlCommand("insert into container_apidata (container_no,userid,day,time,apiData,status) values(@container_no,@userid,@day,@time,@apiData,@status)", conn);
//        //        cmd.Parameters.AddWithValue("@container_no", container_no);
//        //        cmd.Parameters.AddWithValue("@userid", user_id);
//        //        var dte = DateTime.Now.Date.Date.ToString();
//        //        var time = DateTime.Now.Date.ToLongTimeString();
//        //        cmd.Parameters.AddWithValue("@day", DateTime.Now.ToString("dd-MMM-yyyy"));
//        //        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss tt"));
//        //        cmd.Parameters.AddWithValue("@apiData", v);
//        //        cmd.Parameters.AddWithValue("@status", status);
//        //        SqlDataReader MyReader2;
//        //        MyReader2 = cmd.ExecuteReader();
//        //    }
//        //    else
//        //    {
//        //        SqlCommand cmd = new SqlCommand("update container_apidata set container_no=@container_no,day=@day,time=@time,apiData=@apiData,status=@status where container_no=@container_no", conn);
//        //        cmd.Parameters.AddWithValue("@container_no", container_no);
//        //        cmd.Parameters.AddWithValue("@userid", user_id);
//        //        cmd.Parameters.AddWithValue("@day", DateTime.Now.ToString("dd-MMM-yyyy"));
//        //        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss tt"));
//        //        cmd.Parameters.AddWithValue("@apiData", v);
//        //        cmd.Parameters.AddWithValue("@status", status);
//        //        cmd.ExecuteNonQuery();
//        //    }
//        //    conn.Close();
//        //}
//    }
//}