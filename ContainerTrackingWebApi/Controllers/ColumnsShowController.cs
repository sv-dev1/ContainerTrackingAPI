using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ContainerTrackingWebApi.Models;
using ContainerTrackingWebApi.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContainerTrackingWebApi.Controllers
{
    [Route("api/AccountAPI")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class ColumnsShowController : ControllerBase
    {
        string connection = Startup.GetConnectionString();

        #region Settings
        [HttpGet]
        [Route("getSettingsDetails")]
        public async Task<IActionResult> getSettingsDetails(string userid)
        {
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            //ColumnsShow settings = new ColumnsShow();
            SettingsVM settings = new SettingsVM();
            string user_id = "";
            user_id = userid.Trim('"');
            user_id = user_id.Trim('/');

            SqlCommand cmd = new SqlCommand("select * from columns_show where user_id='" + user_id + "' ", conn);
            SqlDataAdapter MyAdapter = new SqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            if (dTable != null && dTable.Rows.Count > 0)
            {
                foreach (DataRow item in dTable.Rows)
                {
                    settings.shipment_ref = Convert.ToInt16(item["shipment_ref"].ToString());
                    settings.origin = Convert.ToInt16(item["origin"].ToString());
                    settings.container_type = Convert.ToInt16(item["container_type"].ToString());
                    settings.destination = Convert.ToInt16(item["destination"].ToString());
                    settings.container_no = Convert.ToInt16(item["container_no"].ToString());
                    settings.departure = Convert.ToInt16(item["departure"].ToString());
                    settings.arrival = Convert.ToInt16(item["arrival"].ToString());
                    settings.first_arrival = Convert.ToInt16(item["first_arrival"].ToString());
                    settings.shipping_line = Convert.ToInt16(item["shipping_line"].ToString());
                    settings.status = Convert.ToInt16(item["status"].ToString());
                    settings.early_delay = Convert.ToInt16(item["early_delay"].ToString());
                    settings.from_country = Convert.ToInt16(item["from_country"].ToString());
                    settings.to_country = Convert.ToInt16(item["to_country"].ToString());
                    settings.transit_time = Convert.ToInt16(item["transit_time"].ToString());
                    settings.first_eta = Convert.ToInt16(item["first_eta"].ToString());
                    settings.bl_reference_no = Convert.ToInt16(item["bl_reference_no"].ToString());
                    settings.transit_ports = Convert.ToInt16(item["transit_ports"].ToString());
                    settings.getout_date = Convert.ToInt16(item["getout_date"].ToString());
                    settings.empty_return_date = Convert.ToInt16(item["empty_return_date"].ToString());
                    settings.shipment_by = Convert.ToInt16(item["shipment_by"].ToString());
                    settings.days_before_arrival = Convert.ToInt16(item["days_before_arrival"].ToString());
                    settings.vessel_name = Convert.ToInt16(item["vesselname"].ToString());
                }
            }
            conn.Close();
            return Ok(JsonConvert.SerializeObject(settings));
        }

        [HttpPost]
        [Route("updateSettingsdetails")]
        public async Task<IActionResult> updateSettingsdetails(SettingsVM settings)
        {
            try
            {
                string command = "";
                string userid = Convert.ToString(settings.userid);
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                int i = 0;
                string useid = "";
                useid = userid.Trim('"');
                useid = useid.Trim('/');
                SqlCommand cmd1 = new SqlCommand("select * from columns_show where user_id='" + useid + "' ", conn);
                SqlDataAdapter MyAdapter = new SqlDataAdapter();
                MyAdapter.SelectCommand = cmd1;
                SqlCommand cmd = new SqlCommand();
                DataTable dTable = new DataTable();
                MyAdapter.Fill(dTable);
                if (dTable == null || dTable.Rows.Count == 0)
                {
                    i = 1;
                    command = "insert into columns_show (shipment_ref,origin,container_type,destination,container_no,departure,arrival,first_arrival,shipping_line,status,early_delay,from_country,to_country,transit_time,first_eta,bl_reference_no,transit_ports,getout_date,empty_return_date, shipment_by,user_id,days_before_arrival,vesselname) values(@shipment_ref,@origin,@container_type,@destination,@container_no,@departure,@arrival,@first_arrival,@shipping_line,@status,@early_delay,@from_country,@to_country,@transit_time,@first_eta,@bl_reference_no,@transit_ports,@getout_date,@empty_return_date, @shipment_by,@id,@days_before_arrival,@vesselname) ";
                }
                else
                {
                    command = "update columns_show set shipment_ref=@shipment_ref,origin=@origin,container_type=@container_type," +
                     "destination=@destination,container_no=@container_no,departure=@departure,arrival=@arrival,first_arrival=@first_arrival," +
                     "shipping_line=@shipping_line,status=@status,early_delay=@early_delay,from_country=@from_country,to_country=@to_country," +
                     "transit_time=@transit_time,first_eta=@first_eta,bl_reference_no=@bl_reference_no,transit_ports=@transit_ports," +
                     "getout_date=@getout_date,empty_return_date=@empty_return_date, shipment_by=@shipment_by,days_before_arrival=@days_before_arrival,vesselname=@vesselname  where user_id=@id";
                }
                cmd.CommandText = command;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@id", useid);
                cmd.Parameters.AddWithValue("@shipment_ref", settings.shipment_ref);
                cmd.Parameters.AddWithValue("@origin", settings.origin);
                cmd.Parameters.AddWithValue("@container_type", settings.container_type);
                cmd.Parameters.AddWithValue("@destination", settings.destination);
                cmd.Parameters.AddWithValue("@container_no", settings.container_no);
                cmd.Parameters.AddWithValue("@departure", settings.departure);
                cmd.Parameters.AddWithValue("@arrival", settings.arrival);
                cmd.Parameters.AddWithValue("@shipping_line", settings.shipping_line);
                cmd.Parameters.AddWithValue("@status", settings.status);
                cmd.Parameters.AddWithValue("@early_delay", settings.early_delay);
                cmd.Parameters.AddWithValue("@from_country", settings.from_country);
                cmd.Parameters.AddWithValue("@to_country", settings.to_country);
                cmd.Parameters.AddWithValue("@transit_time", settings.transit_time);
                cmd.Parameters.AddWithValue("@first_eta", settings.first_eta);
                cmd.Parameters.AddWithValue("@bl_reference_no", settings.bl_reference_no);
                cmd.Parameters.AddWithValue("@transit_ports", settings.transit_ports);
                cmd.Parameters.AddWithValue("@getout_date", settings.getout_date);
                cmd.Parameters.AddWithValue("@empty_return_date", settings.empty_return_date);
                cmd.Parameters.AddWithValue("@shipment_by", settings.shipment_by);
                cmd.Parameters.AddWithValue("@first_arrival", settings.first_arrival);
                cmd.Parameters.AddWithValue("@days_before_arrival", settings.days_before_arrival);
                cmd.Parameters.AddWithValue("@vesselname", settings.vessel_name);
                if (i == 0)
                {
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    SqlDataReader MyReader2;
                    MyReader2 = cmd.ExecuteReader();
                }
                conn.Close();
                return Ok(JsonConvert.SerializeObject(1));
            }
            catch (Exception)
            {
                return Ok(JsonConvert.SerializeObject(0));
            }
        }

        #endregion

    }
}