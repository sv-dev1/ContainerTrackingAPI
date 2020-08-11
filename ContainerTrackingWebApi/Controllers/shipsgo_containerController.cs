using ContainerTrackingWebApi.Models;
using ContainerTrackingWebApi.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Users = ContainerTrackingWebApi.Models.Users;

namespace ContainerTrackingWebApi.Controllers
{
    [Route("api/AccountAPI")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class shipsgo_containerController : ControllerBase
    {
        string connection = Startup.GetConnectionString();
        ShippingLineController objShippingLine = new ShippingLineController();
        //ContainerApidataController containerApidata = new ContainerApidataController();

        [HttpPost]
        [Route("deleteContainer")]
        public async Task<IActionResult> deleteContainer(string userid, string containerno)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from shipsgo_container where id = (SELECT TOP 1 id FROM shipsgo_container where user_id=@id and container_no=@container_no)", conn);
                cmd.Parameters.AddWithValue("@id", userid);
                cmd.Parameters.AddWithValue("@container_no", containerno);
                cmd.ExecuteNonQuery();
                SqlCommand cmd1 = new SqlCommand("delete from container_apidata where userid=@id and container_no=@container_no", conn);
                cmd1.Parameters.AddWithValue("@id", userid);
                cmd1.Parameters.AddWithValue("@container_no", containerno);
                cmd1.ExecuteNonQuery();
                conn.Close();
                string getEmail = "select a.f_name,a.l_name,b.con_del_emails, c.con_del_sts from users a inner join notifications_emails b on a.id = b.user_id inner join user_notifications c on b.user_id = c.user_id where b.user_id = " + userid;
                DataTable dTable1 = new DataTable();
                SqlCommand sqlcmd = new SqlCommand(getEmail, conn);
                SqlDataAdapter MyAdapter1 = new SqlDataAdapter(sqlcmd);
                UserNotifications notifications = new UserNotifications();
                NotificationsEmails emailnotification = new NotificationsEmails();
                Users users = new Users();
                MyAdapter1.Fill(dTable1);
                if (dTable1 != null && dTable1.Rows.Count > 0)
                {
                    foreach (DataRow item in dTable1.Rows)
                    {
                        users.FName = item["f_name"].ToString();
                        users.LName = item["l_name"].ToString();
                        notifications.ConDelSts = Convert.ToInt16(item["con_del_sts"].ToString());
                        emailnotification.ConDelEmails = item["con_del_emails"].ToString();
                    }
                }
                EmailController obj = new EmailController();
                if (notifications.ConDelSts == 1)
                {
                    obj.deleteContainerMail(emailnotification.ConDelEmails, containerno, users.FName + " " + users.LName);
                }

                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }


        /// <summary>
        /// method inserts container information in the DB
        /// </summary>
        /// <param name="shipsgo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insertShipgocontainer")]
        public async Task<IActionResult> insertShipgocontainer(ShipsgoContainerVM shipsgovm)
        {
            try
            {
                ShipsgoContainer shipsgo = new ShipsgoContainer
                {
                    UserId = Convert.ToInt32(shipsgovm.user_id),
                    PoNo = shipsgovm.po_no,
                    Origin = shipsgovm.origin,
                    ContainerType = shipsgovm.container_type,
                    Destination = shipsgovm.destination,
                    ContainerNo = shipsgovm.container_no,
                    Departure = shipsgovm.departure,
                    Arrival = shipsgovm.arrival,
                    FirstArrival = shipsgovm.first_arrival,
                    ShippingLine = shipsgovm.shipping_line,
                    Status = shipsgovm.status,
                    ShipsgoId = Convert.ToString(shipsgovm.shipsgo_id),
                    EarlyDelay = shipsgovm.early_delay,
                    UpdatedAt = shipsgovm.updated_at,
                    Eta = shipsgovm.eta,
                    FromCountry = shipsgovm.from_country,
                    ToCountry = shipsgovm.to_country,
                    TransitTime = shipsgovm.transit_time,
                    FirstEta = shipsgovm.first_eta,
                    BlReferenceNo = shipsgovm.bl_reference_no,
                    TransitPorts = shipsgovm.transit_ports,
                    GetoutDate = shipsgovm.getout_date,
                    EmptyReturnDate = shipsgovm.empty_return_date,
                    ShipmentBy = Convert.ToString(shipsgovm.shipment_by)
                };
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into shipsgo_container (user_id,po_no,origin,container_type,destination,container_no,departure,arrival,first_arrival,shipping_line,is_deleted,status,shipsgo_id,early_delay,created_at,updated_at,eta,from_country,to_country,transit_time,first_eta,bl_reference_no,transit_ports,getout_date,empty_return_date,shipment_by) values(@user_id,@po_no,@origin,@container_type,@destination,@container_no,@departure,@arrival,@first_arrival,@shipping_line,@is_deleted,@status,@shipsgo_id,@early_delay,@created_at,@updated_at,@eta,@from_country,@to_country,@transit_time,@first_eta,@bl_reference_no,@transit_ports,@getout_date,@empty_return_date,@shipment_by)", conn);

                cmd.Parameters.AddWithValue("@user_id", shipsgo.UserId);
                cmd.Parameters.AddWithValue("@po_no", shipsgo.PoNo);
                cmd.Parameters.AddWithValue("@origin", shipsgo.Origin);
                cmd.Parameters.AddWithValue("@container_type", shipsgo.ContainerType);
                cmd.Parameters.AddWithValue("@destination", shipsgo.Destination);
                cmd.Parameters.AddWithValue("@container_no", shipsgo.ContainerNo);
                cmd.Parameters.AddWithValue("@departure", shipsgo.Departure);
                cmd.Parameters.AddWithValue("@arrival", shipsgo.Arrival);
                cmd.Parameters.AddWithValue("@first_arrival", shipsgo.FirstArrival);
                cmd.Parameters.AddWithValue("@shipping_line", shipsgo.ShippingLine);
                cmd.Parameters.AddWithValue("@is_deleted", 0);
                cmd.Parameters.AddWithValue("@status", shipsgo.Status);
                cmd.Parameters.AddWithValue("@shipsgo_id", shipsgo.ShipsgoId);
                cmd.Parameters.AddWithValue("@early_delay", shipsgo.EarlyDelay);
                cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                cmd.Parameters.AddWithValue("@updated_at", shipsgo.UpdatedAt);
                cmd.Parameters.AddWithValue("@eta", shipsgo.Eta);
                cmd.Parameters.AddWithValue("@from_country", shipsgo.FromCountry);
                cmd.Parameters.AddWithValue("@to_country", shipsgo.ToCountry);
                cmd.Parameters.AddWithValue("@transit_time", shipsgo.TransitTime);
                cmd.Parameters.AddWithValue("@first_eta", shipsgo.FirstEta);
                cmd.Parameters.AddWithValue("@bl_reference_no", shipsgo.BlReferenceNo);
                cmd.Parameters.AddWithValue("@transit_ports", shipsgo.TransitPorts);
                cmd.Parameters.AddWithValue("@getout_date", shipsgo.GetoutDate);
                cmd.Parameters.AddWithValue("@empty_return_date", shipsgo.EmptyReturnDate);
                cmd.Parameters.AddWithValue("@shipment_by", shipsgo.ShipmentBy);

                SqlDataReader MyReader2;
                MyReader2 = cmd.ExecuteReader();

                MyReader2.Close();
                string getEmail = "select a.f_name,a.l_name,b.con_add_emails, c.con_add_sts from users a inner join notifications_emails b on a.id = b.user_id inner join user_notifications c on b.user_id = c.user_id where b.user_id = " + shipsgo.UserId;
                DataTable dTable1 = new DataTable();
                SqlCommand sqlcmd = new SqlCommand(getEmail, conn);
                SqlDataAdapter MyAdapter1 = new SqlDataAdapter(sqlcmd);
                UserNotifications notifications = new UserNotifications();
                NotificationsEmails emailnotification = new NotificationsEmails();
                Users users = new Users();
                MyAdapter1.Fill(dTable1);
                if (dTable1 != null && dTable1.Rows.Count > 0)
                {
                    foreach (DataRow item in dTable1.Rows)
                    {
                        users.FName = item["f_name"].ToString();
                        users.LName = item["l_name"].ToString();
                        notifications.ConAddSts = Convert.ToInt16(item["con_add_sts"].ToString());
                        emailnotification.ConAddEmails = item["con_add_emails"].ToString();
                    }
                }
                EmailController obj = new EmailController();
                if (notifications.ConAddSts == 1)
                {
                    obj.sendNotifyMail(emailnotification.ConAddEmails, users.FName, users.LName, shipsgo.ContainerNo, shipsgo.ShippingLine, shipsgo.PoNo);
                }

                conn.Close();
                getDataFromApi(shipsgo.UserId.ToString(), shipsgo.ContainerNo, shipsgo.ShippingLine, out string responseText);
                return Ok(JsonConvert.SerializeObject(1));
            }

            catch (Exception ex)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        [HttpGet]
        [Route("getShipmentDetails")]
        public async Task<IActionResult> getShipmentDetails(string id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                string user_id = "";
                user_id = id.Trim('"');
                user_id = user_id.Trim('/');
                List<ShipsgoContainer> contanList = new List<ShipsgoContainer>();
                SqlCommand cmd = new SqlCommand("SELECT shipsgo_container.*,users.* FROM shipsgo_container INNER JOIN users ON shipsgo_container.user_id = users.id AND (users.id = '" + user_id + "' OR users.invited_id = '" + user_id + "') WHERE shipsgo_container.is_deleted != -1 GROUP BY shipsgo_container.id", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        ShipsgoContainer container = new ShipsgoContainer();
                        container.Id = Convert.ToInt32(reader["id"].ToString());
                        container.UserId = Convert.ToInt32(reader["user_id"].ToString());
                        container.PoNo = reader["po_no"].ToString();
                        container.Origin = reader["origin"].ToString();
                        container.ContainerType = reader["container_type"].ToString();
                        container.Destination = reader["destination"].ToString();
                        container.ContainerNo = reader["container_no"].ToString();

                        //container.Departure = Convert.ToDateTime(reader["departure"].ToString()).ToString("dd MMM yyyy");
                        //container.Arrival = Convert.ToDateTime(reader["arrival"].ToString()).ToString("dd MMM yyyy");
                        //container.FirstArrival = Convert.ToDateTime(reader["first_arrival"]).ToString("dd MMM yyyy");

                        container.Departure = DateTime.ParseExact(reader["departure"].ToString(), "dd MMM yyyy", provider);
                        container.Arrival = DateTime.ParseExact(reader["arrival"].ToString(), "dd MMM yyyy", provider);
                        container.FirstArrival = DateTime.ParseExact(reader["first_arrival"].ToString(), "dd MMM yyyy", provider);


                        container.ShippingLine = reader["shipping_line"].ToString();
                        container.Status = reader["status"].ToString();
                        container.ShipsgoId = reader["shipsgo_id"].ToString();
                        container.EarlyDelay = reader["early_delay"].ToString() == "-" ? "on time" : reader["early_delay"].ToString();
                        container.Eta = reader["eta"].ToString();
                        container.FromCountry = reader["from_country"].ToString();
                        container.ToCountry = reader["to_country"].ToString();
                        container.TransitTime = reader["transit_time"].ToString();
                        container.FirstEta = reader.GetDateTime(reader.GetOrdinal("first_eta"));
                        container.BlReferenceNo = reader["bl_reference_no"].ToString();
                        container.TransitPorts = reader["transit_ports"].ToString();
                        container.GetoutDate = reader.GetDateTime(reader.GetOrdinal("getout_date"));
                        container.ShipmentBy = reader["shipment_by"].ToString();
                        container.CompanyName = reader["company_name"].ToString();
                        contanList.Add(container);

                    }
                }

                return Ok(JsonConvert.SerializeObject(contanList));
            }
            catch (Exception)
            {

                return Ok(JsonConvert.SerializeObject("0"));
            }

        }

        /// <summary>
        /// Calling api to get new data
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="container_no"></param>
        /// <param name="shipping_line"></param>
        /// <param name="responseText"></param>

        private void getDataFromApi(string user_id, string container_no, string shipping_line, out string responseText)
        {
            try
            {
                responseText = "";
                shipping_line = getShippinglinenew(shipping_line);
                string baseUrlAddress = "";
                //if else checks shipping_line values and call to api is made accordingly
                //if (shipping_line == "Detect Automatically" || shipping_line GetContainerTrackingalldetails== "")
                //{
                baseUrlAddress = "https://tracking.searates.com/container?number=" + container_no + "&sealine=" + shipping_line + "&api_key=CUZT-XV8N-Q57L-K630";
                //baseUrlAddress = "https://sirius.searates.com/tracking/api?code=" + container_no + "&api_key=CUZT-XV8N-Q57L-K630";
                //}
                //else
                //{
                //    string shipLine = shipping_line;
                //    string Id = getShippingline(shipping_line, 0).Split(',')[2];
                //    baseUrlAddress = "https://sirius.searates.com/tracking/api?sealine=" + Id + "&code=" + container_no + "&api_key=CUZT-XV8N-Q57L-K630";
                //}
                WebRequest request = HttpWebRequest.Create(baseUrlAddress);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseText = reader.ReadToEnd();
                //Update method is called to update database with new data got from searates api
                updateContainerapiData(user_id, responseText, container_no);
            }
            catch (Exception)
            {
                getContainerApiData(container_no, out int flag, out responseText);
            }
        }
        private string getShippinglinenew(string shipping_line1)
        {
            string shipping_line2 = "";
            shipping_line1 = shipping_line1.ToLower();
            switch (shipping_line1)
            {
                case "maersk":
                    shipping_line2 = "maeu";
                    break;
                case "yang ming":
                    shipping_line2 = "YMLU";
                    break;
                case "hapag-lloyd":
                    shipping_line2 = "HLCU";
                    break;
                case "msc":
                    shipping_line2 = "mscu";
                    break;
                case "cma cgm":
                    shipping_line2 = "CMDU";
                    break;
                case "safmarine":
                    shipping_line2 = "SAFM";
                    break;
                case "cosco":
                    shipping_line2 = "COSU";
                    break;
                case "apl":
                    shipping_line2 = "APLU";
                    break;
                case "zim":
                    shipping_line2 = "ZIMU";
                    break;
                case "oocl":
                    shipping_line2 = "OOLU";
                    break;
                case "sealand":
                    shipping_line2 = "SEJJ";
                    break;
                case "evergreen":
                    shipping_line2 = "EGLV";
                    break;
                case "one":
                    shipping_line2 = "ONEY";
                    break;
                default:
                    break;
            }
            return shipping_line2;
        }
        /// <summary>
        /// method to get all the containers of user with user_id or invited_id matches with user_id parameter
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable getContainerInfofromdatabase(string user_id)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT shipsgo_container.po_no,shipsgo_container.shipping_line,shipsgo_container.container_no,shipsgo_container.user_id,shipsgo_container.container_type,shipsgo_container.departure,shipsgo_container.arrival,shipsgo_container.first_arrival,users.id,users.invited_id,users.company_name FROM shipsgo_container INNER JOIN users ON shipsgo_container.user_id = users.id AND (users.id = '" + user_id + "' OR users.invited_id = '" + user_id + "') WHERE shipsgo_container.is_deleted != -1 order by shipsgo_container.id desc", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            cmd.CommandTimeout = 300000;
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            conn.Close();
            return dTable;

        }
        /// <summary>
        /// this method add columns to the data table
        /// </summary>
        /// <returns></returns>
        public DataTable fetchDataColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("container_no");
            dt.Columns.Add("user_id");
            dt.Columns.Add("company_name");
            dt.Columns.Add("container_type");
            dt.Columns.Add("po_no");
            dt.Columns.Add("latfinal");
            dt.Columns.Add("lngfinal");
            dt.Columns.Add("shipping_line");
            dt.Columns.Add("shipping_link");
            dt.Columns.Add("origin");
            dt.Columns.Add("destination");
            dt.Columns.Add("arrival");
            dt.Columns.Add("departure");
            dt.Columns.Add("daysbeforearrival");
            dt.Columns.Add("first_arrival");
            dt.Columns.Add("Name0");
            dt.Columns.Add("date00");
            dt.Columns.Add("description00");
            dt.Columns.Add("vessel00");
            dt.Columns.Add("type00");
            return dt;
        }
        /// <summary>
        /// method returns complete information about containers associated with user id
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetContainerTrackingalldetails")]
        public async Task<IActionResult> GetContainerTrackingalldetails(string user_id)
        {
            DataTable dt = fetchDataColumns();
            string responseText = "", container_no, origin = "", responseText1 = "", destination = "";
            string departure, arrival, first_arrival; int depLocation, arriLocation;
            int flag = 1; int gateout = 0;
            DataTable dTable = getContainerInfofromdatabase(user_id);//method is called to fetch all the containers of logined user, it also returns container of users where invited_id matches with user_id

            foreach (DataRow dataRow in dTable.Rows)//foreach checks information of each container 
            {
                try
                {
                    container_no = dataRow["container_no"].ToString();
                    //container_no = "MSKU1822065";

                    if (container_no != "")
                    {
                        getContainerApiData(container_no, out flag, out responseText);
                        if (flag == 1)
                        {
                            //flag=1 means that new data needs to be fetched 
                            getDataFromApi(user_id, container_no, dataRow["shipping_line"].ToString(), out responseText1);
                            if (responseText1 != "")
                            {
                                responseText = responseText1;
                            }
                        }
                        DataRow _ro = dt.NewRow();
                        DataColumnCollection columns = dt.Columns;

                        if ((responseText.Contains("success") == true))
                        {
                            if ((responseText.Contains("SEALINE_HASNT_PROVIDE_INFO") == false))
                            {
                                var root = JObject.Parse(responseText.ToString());
                                IList<JToken> hotels = root["data"]["locations"].Children().ToList();
                                IList<ContainerTrack> containerTracking1 = hotels.Select(result => JsonConvert
                                                   .DeserializeObject<ContainerTrack>(
                                                   result.ToString())).ToList();
                                //IEnumerable<ContainerTrack> containerTracking = containerTracking1.OrderByDescending(pet => pet.id);
                                IList<JToken> vessels = root["data"]["vessels"].Children().ToList();
                                IList<ContainerTrack> vesselData = vessels.Select(result => JsonConvert
                                                   .DeserializeObject<ContainerTrack>(
                                                   result.ToString())).ToList();
                                IList<JToken> events = root["data"]["container"]["events"].Children().ToList();
                                IList<events> eventData = events.Select(result => JsonConvert
                                                   .DeserializeObject<events>(
                                                   result.ToString())).ToList();

                                IEnumerable<ContainerTrack> containerTracking = containerTracking1.OrderBy(pet => eventData.Select(x => x.location).Distinct().ToList().IndexOf(pet.id));

                                IList<JToken> route = root["data"]["route"].Children().ToList();
                                //IList<routeEvents> prepoll = route.Select(result => JsonConvert
                                //                  .DeserializeObject<routeEvents>(
                                //                  result.ToString())).ToList();
                                if (Convert.ToDateTime(dataRow["departure"].ToString()).ToString("yyyy-MM-dd") == "1970-01-01" && root["data"]["route"]["prepol"]["date"].ToString() != "")
                                {
                                    departure = root["data"]["route"]["prepol"]["date"].ToString();
                                }
                                else
                                {
                                    departure = Convert.ToDateTime(dataRow["departure"].ToString()).ToString("yyyy-MM-dd");
                                    try
                                    {
                                        //if (Convert.ToInt32(root["data"]["route"]["pol"]["location"]) == containerTracking.Min(x => x.id))
                                        if (!string.IsNullOrEmpty(root["data"]["route"]["pol"]["date"].ToString()))
                                        {
                                            departure = Convert.ToDateTime(root["data"]["route"]["pol"]["date"].ToString()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if (Convert.ToDateTime(dataRow["arrival"].ToString()).ToString("yyyy-MM-dd") == "1970-01-01" && root["data"]["route"]["postpod"]["date"].ToString() != "")
                                {
                                    arrival = root["data"]["route"]["postpod"]["date"].ToString();
                                }
                                else
                                {
                                    arrival = Convert.ToDateTime(dataRow["arrival"].ToString()).ToString("yyyy-MM-dd");
                                    //arrival = "N/A";
                                }

                                //Discharged
                                List<EventList> _eventlist = new List<EventList>();
                                try
                                {
                                    _eventlist = JsonConvert.DeserializeObject<List<EventList>>(JsonConvert.SerializeObject(events));
                                }
                                catch (Exception)
                                {
                                }
                                if (string.IsNullOrEmpty(root["data"]["route"]["postpod"]["date"].ToString()) && Convert.ToDateTime(dataRow["arrival"].ToString()).ToString("yyyy-MM-dd") == "1970-01-01")
                                {
                                    try
                                    {
                                        DateTime? date = _eventlist.Where(x => x.Location == containerTracking.LastOrDefault()?.id && x.Description.ToLower() == "discharge").FirstOrDefault()?.Date;
                                        if (date.HasValue)
                                            arrival = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                                        else
                                        {
                                            date = _eventlist.Where(x => x.Description.ToLower() == "cargo received" || x.Description.ToLower().Contains("dischar")).FirstOrDefault()?.Date;
                                            if (date.HasValue)
                                                arrival = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }

                                if (Convert.ToDateTime(dataRow["first_arrival"].ToString()).ToString("yyyy-MM-dd") == "1970-01-01" && root["data"]["route"]["pod"]["date"].ToString() != "")
                                {
                                    first_arrival = root["data"]["route"]["pod"]["date"].ToString();
                                }
                                else
                                {
                                    first_arrival = Convert.ToDateTime(dataRow["first_arrival"].ToString()).ToString("yyyy-MM-dd");
                                }
                                string _val = Convert.ToString(root["data"]["route"]["prepol"]["location"]) == string.Empty ? null : Convert.ToString(root["data"]["route"]["prepol"]["location"]);
                                depLocation = Convert.ToInt32(_val);
                                if (root["data"]["route"]["postpod"]["location"].ToString() != "")
                                {
                                    arriLocation = Convert.ToInt32(root["data"]["route"]["postpod"]["location"].ToString());
                                }

                                if (first_arrival == "" || first_arrival == null)
                                {
                                    first_arrival = arrival;
                                }

                                //REACHED OR DISCHARGED
                                if (_eventlist.Count == 1)
                                {
                                    DateTime d = _eventlist.FirstOrDefault().Date;
                                    arrival = Convert.ToDateTime(d).ToString("yyyy-MM-dd");
                                    departure = Convert.ToDateTime(d).ToString("yyyy-MM-dd");
                                    if (Convert.ToDateTime(first_arrival).ToString("yyyy-MM-dd") == Convert.ToDateTime(dataRow["first_arrival"].ToString()).ToString("yyyy-MM-dd"))
                                        first_arrival = Convert.ToDateTime(d).ToString("yyyy-MM-dd");
                                }

                                else if (eventData.Count == 1)
                                {
                                    string d = eventData.FirstOrDefault().date;
                                    arrival = Convert.ToDateTime(d).ToString("yyyy-MM-dd");
                                    departure = Convert.ToDateTime(d).ToString("yyyy-MM-dd");
                                    first_arrival = Convert.ToDateTime(d).ToString("yyyy-MM-dd");
                                }
                                if (Convert.ToDateTime(first_arrival) > DateTime.UtcNow)
                                {
                                    arrival = Convert.ToDateTime(first_arrival).ToString("yyyy-MM-dd");
                                }

                                if (arrival == "1970-01-01")
                                {
                                    arrival = Convert.ToDateTime(first_arrival).AddDays(1).ToString("yyyy-MM-dd");
                                }
                                //IList<JToken> route1 = root["data"]["route"]["pol"].Children().ToList();
                                //IList<pol> poll = route.Select(result => JsonConvert
                                //                  .DeserializeObject<pol>(
                                //                  result.ToString())).ToList();
                                //IList<JToken> route2 = root["data"]["route"]["pod"].Children().ToList();
                                //IList<pod> pod1 = route.Select(result => JsonConvert
                                //                  .DeserializeObject<pod>(
                                //                  result.ToString())).ToList();
                                //IList<JToken> route3 = root["data"]["route"]["postpod"].Children().ToList();
                                //IList<postpod> postl = route.Select(result => JsonConvert
                                //                  .DeserializeObject<postpod>(
                                //                  result.ToString())).ToList();
                                int i = -1;

                                _ro["departure"] = departure;
                                _ro["first_arrival"] = first_arrival;
                                _ro["arrival"] = arrival;
                                _ro["latfinal"] = "";
                                _ro["lngfinal"] = "";
                                _ro["container_no"] = dataRow["container_no"].ToString();
                                _ro["user_id"] = dataRow["user_id"].ToString();
                                _ro["company_name"] = dataRow["company_name"].ToString();
                                _ro["container_type"] = dataRow["container_type"].ToString();
                                _ro["po_no"] = dataRow["po_no"].ToString();
                                _ro["shipping_line"] = dataRow["shipping_line"].ToString();
                                _ro["shipping_link"] = objShippingLine.getShippingline(dataRow["shipping_line"].ToString(), 0).Split(',')[1];
                                foreach (ContainerTrack result in containerTracking)// this foreach tracks each path container goes through and fills the required fields
                                {
                                    if (result.id == depLocation)
                                    {
                                        i = 0;
                                    }
                                    if (i != -1)
                                    {
                                        if (columns.Contains("Name" + i) == false)
                                        {
                                            dt.Columns.Add("Name" + i);
                                        }
                                        if (columns.Contains("Lat" + i) == false)
                                        {
                                            dt.Columns.Add("Lat" + i);
                                        }

                                        if (columns.Contains("Lng" + i) == false)
                                        {
                                            dt.Columns.Add("Lng" + i);
                                        }

                                        if (columns.Contains("Country" + i) == false)
                                        {
                                            dt.Columns.Add("Country" + i);
                                        }
                                        if (columns.Contains("Location" + i) == false)
                                        {
                                            dt.Columns.Add("Location" + i);
                                        }
                                        _ro["Name" + i] = result.name;
                                        _ro["Location" + i] = result.id;
                                        if (result.lat != null)
                                        {
                                            _ro["Lat" + i] = decimal.Parse(result.lat, CultureInfo.InvariantCulture).ToString().Replace(",", ".");
                                        }
                                        if (result.lng != null)
                                        {
                                            _ro["Lng" + i] = decimal.Parse(result.lng, CultureInfo.InvariantCulture).ToString().Replace(",", ".");
                                        }
                                        _ro["Country" + i] = result.country;
                                        if (i == 0)
                                        {
                                            //i=0 means 1st path is the origin
                                            //_ro["destination"] = destination = result.name;
                                            _ro["origin"] = origin = result.name;
                                        }

                                        if (i == containerTracking1.Count - 1)
                                        {
                                            //_ro["origin"] = origin = result.name;
                                            _ro["destination"] = destination = result.name;
                                        }
                                        int j = 0;
                                        foreach (var item in eventData)
                                        {
                                            if (item.location == result.id)
                                            {
                                                if (columns.Contains("date" + i + j) == false)
                                                {
                                                    dt.Columns.Add("date" + i + j);
                                                }

                                                if (columns.Contains("description" + i + j) == false)
                                                {
                                                    dt.Columns.Add("description" + i + j);
                                                }

                                                if (columns.Contains("type" + i + j) == false)
                                                {
                                                    dt.Columns.Add("type" + i + j);
                                                }
                                                if (columns.Contains("vessel" + i + j) == false)
                                                {
                                                    dt.Columns.Add("vessel" + i + j);
                                                }
                                                //_ro["date" + i + j] = item.date;
                                                _ro["date" + i + j] = string.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(item.date));
                                                // _ro["vessel" + i + j] = item.vessel;
                                                _ro["description" + i + j] = item.description;
                                                _ro["type" + i + j] = item.type;
                                                foreach (var vess in vesselData)
                                                {
                                                    if (vess.id == item.vessel)
                                                    {
                                                        _ro["vessel" + i + j] = vess.name;
                                                    }
                                                }
                                                if (item.description.Contains("Gate out") || item.description.Contains("Gate-out") || item.description.Contains("gate out"))
                                                {
                                                    string dtTimeout = Convert.ToDateTime(item.date).ToString("dd-MMM-yyyy");
                                                    if (dtTimeout == DateTime.Now.ToString("dd-MMM-yyyy"))
                                                    {
                                                        UpdateGateout(Convert.ToDateTime(dtTimeout), user_id, container_no);
                                                        gateout = 1;
                                                    }
                                                }
                                                j = j + 1;
                                            }
                                            else
                                            {
                                                j = 0;
                                            }
                                        }
                                        i = i + 1;
                                    }
                                }
                                //If destination is Null assign the last as destination.
                                if (_ro["destination"] == DBNull.Value)
                                {
                                    _ro["destination"] = containerTracking.LastOrDefault()?.name;
                                }
                                _ro["daysbeforearrival"] = 0;
                                DateTime dateTime = Convert.ToDateTime(_ro["arrival"]);
                                if (dateTime < DateTime.Now)//container has reached its destination
                                {
                                    Updatecontainersts(user_id, container_no);
                                }
                                if (_ro["arrival"].ToString() != "")
                                {
                                    TimeSpan difference = Convert.ToDateTime(_ro["arrival"]).Date - DateTime.Now.Date;
                                    int days = (int)difference.TotalDays;
                                    if (days < 0)
                                    {
                                        days = 0;
                                    }
                                    _ro["daysbeforearrival"] = days;
                                }

                                if (gateout == 1)
                                {
                                    //if the container has gateout, email is send to the user registered mail id
                                    sendGateoutemail(container_no, origin, destination, user_id);
                                }
                                sendArrivalEmail(user_id, container_no, dataRow["po_no"].ToString(), _ro["shipping_line"].ToString(), _ro["daysbeforearrival"].ToString(), _ro["arrival"].ToString());
                            }
                            else
                            {
                                _ro["origin"] = "DATA NOT FOUND";
                                _ro["departure"] = Convert.ToDateTime(dataRow["departure"]).ToString("dd-MMM-yyyy");
                                _ro["first_arrival"] = Convert.ToDateTime(dataRow["first_arrival"]).ToString("dd-MMM-yyyy");
                                _ro["arrival"] = Convert.ToDateTime(dataRow["arrival"]).ToString("dd-MMM-yyyy");
                                _ro["container_no"] = dataRow["container_no"].ToString();
                                _ro["user_id"] = dataRow["user_id"].ToString();
                                _ro["company_name"] = dataRow["company_name"].ToString();
                                _ro["container_type"] = dataRow["container_type"].ToString();
                                _ro["po_no"] = dataRow["po_no"].ToString();
                                _ro["shipping_line"] = dataRow["shipping_line"].ToString();
                                _ro["shipping_link"] = objShippingLine.getShippingline(dataRow["shipping_line"].ToString(), 0).Split(',')[1];
                                EmailController obj = new EmailController();
                                obj.sendSealineNoInfo(container_no);
                            }

                        }
                        else
                        {
                            _ro["origin"] = "DATA NOT FOUND";
                            _ro["departure"] = Convert.ToDateTime(dataRow["departure"]).ToString("dd-MMM-yyyy");
                            _ro["first_arrival"] = Convert.ToDateTime(dataRow["first_arrival"]).ToString("dd-MMM-yyyy");
                            _ro["arrival"] = Convert.ToDateTime(dataRow["arrival"]).ToString("dd-MMM-yyyy");
                            _ro["container_no"] = dataRow["container_no"].ToString();
                            _ro["user_id"] = dataRow["user_id"].ToString();
                            _ro["company_name"] = dataRow["company_name"].ToString();
                            _ro["container_type"] = dataRow["container_type"].ToString();
                            _ro["po_no"] = dataRow["po_no"].ToString();
                            _ro["shipping_line"] = dataRow["shipping_line"].ToString();
                            _ro["shipping_link"] = objShippingLine.getShippingline(dataRow["shipping_line"].ToString(), 0).Split(',')[1];
                        }
                        dt.Rows.Add(_ro);
                    }
                }
                catch (Exception ex)
                {
                    DataTable dataTable = dt;
                    return Ok(JsonConvert.SerializeObject(dt));
                }
            }
            return Ok(JsonConvert.SerializeObject(dt));

        }
        private void getContainerApiData(string container_no, out int flag, out string responseText)
        {
            responseText = "";
            flag = 1;
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmdApi = new SqlCommand("SELECT * FROM container_apidata where container_no='" + container_no + "'", conn);
                cmdApi.CommandTimeout = 30000;
                SqlDataAdapter MyAdapter1 = new SqlDataAdapter();
                MyAdapter1.SelectCommand = cmdApi;
                DataTable dTab = new DataTable();
                MyAdapter1.Fill(dTab);
                if (dTab != null && dTab.Rows.Count > 0)
                {
                    foreach (DataRow item1 in dTab.Rows)
                    {
                        string date1 = item1["day"].ToString();
                        string status = item1["status"].ToString();
                        string date2 = DateTime.Now.ToString("dd-MMM-yyyy");
                        if (status == "1")
                        {
                            //if status = 1 i.e container hasn't reached yet its destination and call is made at start of day and after 12 hours
                            if (Convert.ToDateTime(item1["day"].ToString()) <= Convert.ToDateTime(date2))
                            {
                                //if (Convert.ToDateTime(item1["time"].ToString()).AddHours(12).TimeOfDay > DateTime.Now.TimeOfDay)
                                //{
                                flag = 0;
                                responseText = item1["apiData"].ToString();
                                //}
                            }
                        }
                        else if (status == "0")
                        {
                            //if status = 0, means container data was not proper and call is made to get fresh data after 2 days
                            if (Convert.ToDateTime(item1["day"].ToString()).AddDays(1) <= Convert.ToDateTime(date2))
                            {
                                flag = 1;
                            }
                            else
                            {
                                responseText = item1["apiData"].ToString();
                                flag = 0;
                            }
                        }
                        else if (status == "3")
                        {
                            //if status = 0, means container has reached its destination and no call to Api is done and data is fetched from database
                            responseText = item1["apiData"].ToString();
                            flag = 0;
                        }
                    }
                }
                else
                {
                    responseText = "DATA_NOT_FOUND";
                }
                conn.Close();
            }
            catch (Exception)
            {

            }
        }


        private void sendArrivalEmail(string user_id, string container_no, string shipmentRef, string sealine, string days, string final_arrival)
        {
            try
            {
                string container_no1 = container_no;
                string shipmentRef1 = shipmentRef;
                string sealine1 = sealine;
                string days1 = days;
                if (days != "0")
                {
                    SqlConnection conn = new SqlConnection(connection);
                    conn.Open();
                    string getTrackingdatails = "select a.f_name,a.l_name, b.until_arrival_emails, c.con_untilarrival_by_email,c.con_untilarrival_days from users a inner join notifications_emails b on a.id = b.user_id inner join user_notifications c on b.user_id = c.user_id where b.user_id= " + user_id;
                    DataTable dTable1 = new DataTable();
                    SqlCommand sqlcmd = new SqlCommand(getTrackingdatails, conn);
                    SqlDataAdapter MyAdapter2 = new SqlDataAdapter(sqlcmd);
                    UserNotifications notifications = new UserNotifications();
                    NotificationsEmails notificationsEmails = new NotificationsEmails();
                    Users users = new Users();
                    MyAdapter2.Fill(dTable1);
                    if (dTable1 != null && dTable1.Rows.Count > 0)
                    {
                        foreach (DataRow item in dTable1.Rows)
                        {
                            users.FName = item["f_name"].ToString();
                            users.LName = item["l_name"].ToString();
                            notificationsEmails.UntilArrivalEmails = item["until_arrival_emails"].ToString();
                            notifications.ConUntilarrivalByEmail = Convert.ToInt16(item["con_untilarrival_by_email"].ToString());
                            notifications.ConUntilarrivalDays = Convert.ToInt16(item["con_untilarrival_days"].ToString());
                            if (notifications.ConUntilarrivalByEmail == 1 && notifications.ConUntilarrivalDays != 0 && notifications.ConUntilarrivalDays == Convert.ToInt32(days))
                            {
                                EmailController obj = new EmailController();
                                obj.sendDaysBeforeArrivalMail(notificationsEmails.UntilArrivalEmails, users.FName, users.LName, container_no, shipmentRef, sealine, final_arrival);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        /// <summary>
        /// method checks departure and first_arrival from the database, if exists returns values from db else returns data got from api
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itemDate"></param>
        /// <returns></returns>
        private string getDeptArrival(string date, string itemDate)
        {
            if (date != "1970-01-01")
            {
                return Convert.ToDateTime(date).ToString("dd-MMM-yyyy");
            }
            else
            {
                return formatDate(itemDate, 1);
            }

        }
        /// <summary>
        /// method converts timestamp to date format
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private string formatDate(string date, int format)
        {
            if (format == 1)
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(date)).ToString("dd-MMM-yyyy");
            }
            else
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(date)).ToString("dd-MMM-yyyy hh:mm:ss tt");
            }
        }
        /// <summary>
        /// Method sends email to user when the container associated with it passes gateout
        /// </summary>
        /// <param name="container_no"></param>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="user_id"></param>
        private void sendGateoutemail(string container_no, string origin, string destination, string user_id)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string getTrackingdatails = "select a.f_name,a.l_name, b.con_add_emails, b.dep_change_emails,b.arr_change_emails,b.con_timeout_emails,c.dep_change_sts,c.dep_change_time,c.arr_change_sts,c.arr_change_time,c.con_add_sts,c.con_timeout_sts,c.con_timeout_sendsts from users a inner join notifications_emails b on a.id = b.user_id inner join user_notifications c on b.user_id = c.user_id where b.user_id = " + user_id;
            DataTable dTable1 = new DataTable();
            SqlCommand sqlcmd = new SqlCommand(getTrackingdatails, conn);
            SqlDataAdapter MyAdapter2 = new SqlDataAdapter(sqlcmd);
            UserNotifications notifications = new UserNotifications();
            NotificationsEmails notificationsEmails = new NotificationsEmails();
            Users users = new Users();
            MyAdapter2.Fill(dTable1);
            if (dTable1 != null && dTable1.Rows.Count > 0)
            {
                foreach (DataRow item in dTable1.Rows)
                {
                    users.FName = item["f_name"].ToString();
                    users.LName = item["l_name"].ToString();
                    notificationsEmails.DepChangeEmails = item["dep_change_emails"].ToString();
                    notifications.DepChangeSts = Convert.ToInt16(item["dep_change_sts"].ToString());
                    notifications.ConAddSts = Convert.ToInt16(item["con_add_sts"].ToString());
                    notifications.ArrChangeSts = Convert.ToInt16(item["arr_change_sts"].ToString());
                    notificationsEmails.ArrChangeEmails = item["arr_change_emails"].ToString();
                    notifications.ConTimeoutSts = Convert.ToInt16(item["con_timeout_sts"].ToString());
                    notifications.ConTimeoutSendsts = Convert.ToInt16(item["con_timeout_sendsts"].ToString());
                    notificationsEmails.ConTimeoutEmails = item["con_timeout_emails"].ToString();
                    if (notifications.ConTimeoutSendsts == 0)
                    {
                        sendGateoutEmail(notificationsEmails.ConTimeoutEmails, users.FName, users.LName, container_no, origin, destination, user_id);
                    }
                }
            }
        }
        /// <summary>
        /// method updates status =3 means container has reached its destination 
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="container_no"></param>
        private void Updatecontainersts(string user_id, string container_no)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update container_apidata set status=3 where container_no='" + container_no + "' and userid='" + user_id + "'", conn);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
        }
        /// <summary>
        /// method updates origin and destination of container in DB
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="user_id"></param>
        /// <param name="container_no"></param>
        private void updateorigindest(string origin, string destination, string user_id, string container_no)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update shipsgo_container set origin=@origin,destination=@destination where container_no='" + container_no + "' and user_id='" + user_id + "'", conn);
            cmd.Parameters.AddWithValue("@origin", origin);
            cmd.Parameters.AddWithValue("@destination", destination);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
        }


        /// <summary>
        /// Method send gateout notification email to user registered mail id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="f_name"></param>
        /// <param name="l_name"></param>
        /// <param name="container_no"></param>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="userid"></param>
        private void sendGateoutEmail(string email, string f_name, string l_name, string container_no, string origin, string destination, string userid)
        {
            EmailController obj = new EmailController();
            obj.sendTimeoutNotifyMail(email, f_name, l_name, container_no, origin, destination);
            Updatecontainertimeoutstats(userid);
        }

        /// <summary>
        /// method updates shipment ref,container type and shipping line in the Db of the container
        /// </summary>
        /// <param name="container_no"></param>
        /// <param name="po_no"></param>
        /// <param name="container_type"></param>
        /// <param name="shipping_line"></param>
        /// <param name="user_id"></param>
        [HttpPost]
        [Route("UpdateReference")]
        public void UpdateReferenceNo(string container_no, string po_no, string container_type, string shipping_line, string user_id)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update shipsgo_container set po_no=@po_no,container_type=@container_type,shipping_line=@shipping_line where container_no='" + container_no + "' and user_id='" + user_id + "'", conn);
            cmd.Parameters.AddWithValue("@po_no", po_no);
            cmd.Parameters.AddWithValue("@container_type", container_type);
            cmd.Parameters.AddWithValue("@shipping_line", shipping_line);
            int i = cmd.ExecuteNonQuery();
            conn.Close();

        }
        /// <summary>
        /// method updates shipping line of the container
        /// </summary>
        /// <param name="container_no"></param>
        /// <param name="shipping_line"></param>
        /// <param name="user_id"></param>
        public void UpdateshippingLine(string container_no, string shipping_line, string user_id)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update shipsgo_container set shipping_line=@shipping_line where container_no='" + container_no + "' and user_id='" + user_id + "'", conn);
            cmd.Parameters.AddWithValue("@shipping_line", shipping_line);
            int i = cmd.ExecuteNonQuery();
            conn.Close();

        }
        public void UpdateGateout(DateTime gateout, string userid, string container_no)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update shipsgo_container set getout_date=@getout_date where container_no='" + container_no + "' and user_id='" + userid + "'", conn);
            cmd.Parameters.AddWithValue("@getout_date", gateout);
            int i = cmd.ExecuteNonQuery();
            conn.Close();

        }
        [HttpPost]
        [Route("UpdatecontainerDates")]
        public void UpdatecontainerDates(string container_no, string userid, string departure, string arrival, string firstarrival)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update shipsgo_container set departure='" + departure + "',arrival='" + arrival + "',first_arrival='" + firstarrival + "' where container_no='" + container_no + "' and user_id='" + userid + "'", conn);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            sendEmail(userid, container_no);

        }
        /// <summary>
        /// Send email regarding Container Tracking Information to the user email id
        /// </summary>
        /// <param name="user_id"></param>

        public void sendEmail(string user_id, string container_no)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT shipsgo_container.*,users.id,users.invited_id,users.company_name FROM shipsgo_container INNER JOIN users ON shipsgo_container.user_id = users.id AND (users.id = '" + user_id + "' OR users.invited_id = '" + user_id + "') WHERE shipsgo_container.is_deleted != -1 and  shipsgo_container.container_no='" + container_no + "'", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            cmd.CommandTimeout = 30000000;
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dTable.Rows)
                {
                    string getTrackingdatails = "select a.f_name,a.l_name, b.con_add_emails, b.dep_change_emails,b.arr_change_emails,b.con_timeout_emails,c.dep_change_sts,c.dep_change_time,c.arr_change_sts,c.arr_change_time,c.con_add_sts,c.con_timeout_sts,c.con_timeout_sendsts from users a inner join notifications_emails b on a.id = b.user_id inner join user_notifications c on b.user_id = c.user_id where b.user_id = " + user_id;
                    DataTable dTable1 = new DataTable();
                    SqlCommand sqlcmd = new SqlCommand(getTrackingdatails, conn);
                    SqlDataAdapter MyAdapter2 = new SqlDataAdapter(sqlcmd);
                    // notifications notifications = new notifications();
                    Users users = new Users();
                    MyAdapter2.Fill(dTable1);
                    if (dTable1 != null && dTable1.Rows.Count > 0)
                    {
                        foreach (DataRow item in dTable1.Rows)
                        {
                            var shipmentref = dataRow["po_no"].ToString();
                            var origin = dataRow["origin"].ToString();
                            var destination = dataRow["destination"].ToString();
                            var departure = dataRow["departure"].ToString();
                            var arrival = dataRow["arrival"].ToString();
                            var sealine = dataRow["shipping_line"].ToString();
                            var containertype = dataRow["container_type"].ToString();
                            var containerNo = dataRow["container_no"].ToString();
                            var status = "";
                            var timespan = Convert.ToDateTime(dataRow["first_arrival"].ToString()) - Convert.ToDateTime(dataRow["arrival"].ToString());
                            double NoOfdays = timespan.TotalDays;
                            EmailController obj = new EmailController();
                            if (Convert.ToInt32(item["dep_change_sts"]) == 1 && Convert.ToInt32(item["arr_change_sts"]) == 0)
                            {
                                obj.sendTrackingupdateMail(item["dep_change_emails"].ToString(), users.FName, users.LName, shipmentref, origin, containertype, destination, containerNo, departure, arrival, sealine, NoOfdays, status);
                            }
                            else if (Convert.ToInt32(item["arr_change_sts"]) == 1 && Convert.ToInt32(item["dep_change_sts"]) == 0)
                            {
                                obj.sendTrackingupdateMail(item["arr_change_emails"].ToString(), users.FName, users.LName, shipmentref, origin, containertype, destination, containerNo, departure, arrival, sealine, NoOfdays, status);
                            }
                            else if (Convert.ToInt32(item["arr_change_sts"]) == 1 && Convert.ToInt32(item["dep_change_sts"]) == 1)
                            {
                                if (item["dep_change_emails"].ToString() == item["arr_change_emails"].ToString())
                                {
                                    obj.sendTrackingupdateMail(item["arr_change_emails"].ToString(), users.FName, users.LName, shipmentref, origin, containertype, destination, containerNo, departure, arrival, sealine, NoOfdays, status);
                                }
                                else
                                {
                                    if (item["dep_change_emails"].ToString() != item["arr_change_emails"].ToString() && Convert.ToInt32(item["dep_change_sts"].ToString()) == 1 && Convert.ToInt32(item["arr_change_sts"].ToString()) == 0)
                                    {
                                        obj.sendTrackingupdateMail(item["dep_change_emails"].ToString(), users.FName, users.LName, shipmentref, origin, containertype, destination, containerNo, departure, arrival, sealine, NoOfdays, status);
                                    }
                                    else if (item["dep_change_emails"].ToString() != item["arr_change_emails"].ToString() && Convert.ToInt32(item["arr_change_sts"].ToString()) == 1 && Convert.ToInt32(item["dep_change_sts"].ToString()) == 0)
                                    {
                                        obj.sendTrackingupdateMail(item["arr_change_emails"].ToString(), users.FName, users.LName, shipmentref, origin, containertype, destination, containerNo, departure, arrival, sealine, NoOfdays, status);
                                    }
                                }

                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            conn.Close();
        }
        /// <summary>
        /// updates timeout stats of user notifications as 1
        /// </summary>
        /// <param name="userid"></param>
        public void Updatecontainertimeoutstats(string userid)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update user_notifications set con_timeout_sendsts='1' where user_id='" + userid + "'", conn);
            int i = cmd.ExecuteNonQuery();
            conn.Close();

        }
        private void updateContainerapiData(string user_id, string v, string container_no)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            string status;
            if (v.Contains("DATA_NOT_FOUND") == true || v.Contains("ONLY_TEXT") == true || v.Contains("WRONG_CONTAINER_NUMBER") == true || v.Contains("SEALINE_HASNT_PROVIDE_INFO") == true)
            {
                status = "0";
            }
            else
            {
                status = "1";
            }
            SqlCommand cmd1 = new SqlCommand("SELECT * from container_apidata where container_no='" + container_no + "'", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            MyAdapter.SelectCommand = cmd1;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            if (dTable.Rows.Count == 0)
            {
                SqlCommand cmd = new SqlCommand("insert into container_apidata (container_no,userid,day,time,apiData,status) values(@container_no,@userid,@day,@time,@apiData,@status)", conn);
                cmd.Parameters.AddWithValue("@container_no", container_no);
                cmd.Parameters.AddWithValue("@userid", user_id);
                var dte = DateTime.Now.Date.Date.ToString();
                var time = DateTime.Now.Date.ToLongTimeString();
                cmd.Parameters.AddWithValue("@day", DateTime.Now.ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss tt"));
                cmd.Parameters.AddWithValue("@apiData", v);
                cmd.Parameters.AddWithValue("@status", status);
                SqlDataReader MyReader2;
                MyReader2 = cmd.ExecuteReader();
            }
            else
            {
                if (status == "1")
                {
                    SqlCommand cmd = new SqlCommand("update container_apidata set container_no=@container_no,day=@day,time=@time,apiData=@apiData,status=@status where container_no=@container_no", conn);
                    cmd.Parameters.AddWithValue("@container_no", container_no);
                    cmd.Parameters.AddWithValue("@userid", user_id);
                    cmd.Parameters.AddWithValue("@day", DateTime.Now.ToString("dd-MMM-yyyy"));
                    cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss tt"));
                    cmd.Parameters.AddWithValue("@apiData", v);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
    }
}