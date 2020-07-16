using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerTrackingWebApi.ViewModel
{
    public class UsersVM
    {
        public int id { get; set; }
        public string invited_id { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string passwordNew { get; set; }
        public string company_name { get; set; }
        public string zip_code { get; set; }
        public string city_name { get; set; }
        public string country_name { get; set; }
        public string cvr_no { get; set; }
        public string country_code { get; set; }
        public string phone_no { get; set; }
        public string address { get; set; }
        public string profile_pic { get; set; }
        public int user_role { get; set; }
        public short status { get; set; }
        public short signup_status { get; set; }
        public object total_container { get; set; }
        public DateTime expiry_date { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
