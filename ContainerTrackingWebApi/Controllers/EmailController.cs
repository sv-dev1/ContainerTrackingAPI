using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContainerTrackingWebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        SmtpClient smtp = new SmtpClient();
        [NonAction]
        public void smtpDetails()
        {
            smtp.Port = 587;
            smtp.Host = "smtp.unoeuro.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("no_reply@containertracking.dk", "Container22");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        }
        /// <summary>
        /// this method send inquiry email to jesper@containertracking.dk
        /// </summary>
        /// <param name="clientmessage"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("sendEmail")]
        public async Task<IActionResult> sendEmail(string clientmessage)
        {
            try
            {
                ViewModel.Email arr = JsonConvert.DeserializeObject<ViewModel.Email>(clientmessage);
                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress("jesper@containertracking.dk"));
                message.Subject = "Contact Us Inquiries on " + DateTime.Now.ToString("dd-MMM-yyyy");
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlTemplate(arr);
                smtpDetails();
                smtp.Send(message);
                sendClientMail(arr);
                return Ok("1");
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        /// <summary>
        ///Method sends booking demo email
        /// </summary>
        /// <param name="emailto"></param>
        /// <param name="f_name"></param>
        /// <param name="l_name"></param>
        /// <param name="phone_number"></param>
        /// <param name="website"></param>
        /// <param name="cmp_name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SendBookingDemoEmail")]
        public async Task<IActionResult> SendBookingDemoEmail(string emailto, string f_name, string l_name, string phone_number, string website, string cmp_name)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress("jesper@containertracking.dk"));
                message.IsBodyHtml = true; //to make message body as html 
                message.Subject = "Booking demo request received on " + DateTime.Now.ToString("dd-MMM-yyyy");
                message.Body = htmlTemplatedemo(emailto, f_name, l_name, phone_number, website, cmp_name);
                smtpDetails();
                smtp.Send(message);
                sendClientDemoMail(emailto, f_name, l_name, phone_number, website, cmp_name);
                return Ok("1");
            }
            catch (Exception)
            {
                return Ok("0");
            }
        }
        /// <summary>
        /// method creates html template of the inquiry recieved
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        [NonAction]
        public string htmlTemplate(ViewModel.Email arr)
        {
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            string html = "";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'>Hi <b>Jesper Johansen</b>,</td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 40px 50px'>Following are the details :</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Name :  </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + arr.name + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Email : </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + arr.email + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Subject : </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + arr.subject + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Message :  </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + arr.message + "</td></tr><br/><br/>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        /// <summary>
        /// method create demo email template
        /// </summary>
        /// <param name="emailto"></param>
        /// <param name="f_name"></param>
        /// <param name="l_name"></param>
        /// <param name="phone_number"></param>
        /// <param name="website"></param>
        /// <param name="cmp_name"></param>
        /// <returns></returns>
        [NonAction]
        public string htmlTemplatedemo(string emailto, string f_name, string l_name, string phone_number, string website, string cmp_name)
        {
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            string html = "";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'>Hi <b>Jesper Johansen</b>,</td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 40px 50px'>Request for demo details :</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Name :  </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + f_name + " " + l_name + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Email : </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + emailto + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Phone Number : </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + phone_number + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Company Name : </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + cmp_name + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Website :  </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + website + "</td></tr><br/><br/>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        /// <summary>
        /// method sends inquiry email to client
        /// </summary>
        /// <param name="arr"></param>
        private void sendClientMail(ViewModel.Email arr)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("jesper@containertracking.dk");
            message.To.Add(new MailAddress(arr.email));
            message.Subject = "Inquiry on " + DateTime.Now.ToString("dd-MMM-yyyy") + " Regarding " + arr.subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = clientHtmlTemplate(arr);
            smtpDetails();
            smtp.Send(message);
        }
        private void sendClientDemoMail(string emailto, string f_name, string l_name, string phone_number, string website, string cmp_name)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("jesper@containertracking.dk");
            message.To.Add(new MailAddress(emailto));
            message.Subject = "Requested Demo on " + DateTime.Now.ToString("dd-MMM-yyyy");
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = htmlNotifyTemplateDemo(emailto, f_name, l_name);
            smtpDetails();
            smtp.Send(message);
        }
        /// <summary>
        /// method defines the email body of client inquiry
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        [NonAction]
        public string clientHtmlTemplate(ViewModel.Email arr)
        {

            var path = "https://containertracking.dk/assets/images/emailheader.png";
            string html = "";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'>  Hi <b>" + arr.name + ",</b></td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>Thank You For Contacting Us. <br>One of our representatives will be in contact with you shortly regarding your inquiry.</td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'><br><br><span style = 'float:left;'> Jesper Johansen<br>Founder Of Container Tracking </span><br/><br/><br/></td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [NonAction]
        public void sendNotifyMail(string emailto, string f_name, string l_name, string containerNo, string shippingline, string shipmentRefNo)
        {
            try
            {

                //Models.Email arr = JsonConvert.DeserializeObject<Models.Email>(clientmessage);
                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress(emailto));
                message.IsBodyHtml = true; //to make message body as html  
                message.Subject = "Tracking request for container number " + containerNo + " having shipment ref. " + shipmentRefNo;
                message.Body = htmlNotifyTemplate(emailto, f_name, l_name, containerNo, shippingline, shipmentRefNo);
                smtpDetails();
                smtp.Send(message);
                //sendClientMail(arr);
            }
            catch (Exception ex)
            {
                
            }
        }
        [NonAction]
        public void sendTimeoutNotifyMail(string email, string f_name, string l_name, string container_no, string origin, string destination)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress(email));
                message.IsBodyHtml = true; //to make message body as html  
                message.Subject = "GATE OUT Container number " + container_no;
                message.Body = htmlNotifyTemplateGateout(f_name, l_name, container_no, origin, destination);
                smtpDetails();
                smtp.Send(message);
            }
            catch (Exception ex)
            {
               
            }
        }
        [NonAction]
        public void sendSealineNoInfo(string container_no)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress("jesper@containertracking.dk"));
                message.IsBodyHtml = true; //to make message body as html  
                message.Subject = "SEALINE_HASNT_PROVIDE_INFO FOR " + container_no;
                message.Body = htmlSealineNoInfo(container_no);
                smtpDetails();
                smtp.Send(message);
            }
            catch (Exception ex)
            {

            }
        }
        [NonAction]
        public string htmlSealineNoInfo(string container_no)
        {
            string html = "";
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            //html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'><b>" + f_name + " " + l_name + ",</b></td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>SEALINE HASNT PROVIDE INFO for Container number " + container_no + " </td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [NonAction]
        public void sendDaysBeforeArrivalMail(string email, string f_name, string l_name, string container_no, string shipmentRef, string sealine, string final_arrival)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress(email));
                message.IsBodyHtml = true; //to make message body as html  
                message.Subject = "Arrival Notice for the container " + container_no;
                message.Body = htmldaysbeforeTemplate(email, f_name, l_name, container_no, sealine, shipmentRef, final_arrival);
                smtpDetails();
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [NonAction]
        public string htmlNotifyTemplate(string emailto, string f_name, string l_name, string containerNo, string shippingline, string shipmentRefNo)
        {
            string html = "";
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            string pathOverview = "https://containertracking.dk/assets/images/shipment-btn.png";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            //html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'><b>" + f_name + " " + l_name + ",</b></td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>Congratulations: Your container will now be monitored and you will be informed automatically if there is a change in the departure or arrival date.</td></tr>";
            html += "<tr><td colspan='2' style='padding:15px 10px 5px 50px;margin:10px 0px 0px 0px;'><b>Container Information</b></td></tr>";
            html += "<tr><td  style='width:35%; padding:50px 10px 5px 50px;margin:30px 0px 0px 0px;font-weight:600; vertical-align:top;'><b>Shipment ref. </b></td><td style='width:65%; vertical-align:top; padding:50px 50px 5px 50px;margin:30px 0px 0px 0px;'>" + shipmentRefNo + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Container No. </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + containerNo + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Shipping Line </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + shippingline + "</td></tr><br/><br/>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 10px 15px;'><a href='https://containertracking.dk/Shipment'><img width='250' src='" + pathOverview + "'></a></td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [NonAction]
        public string htmldaysbeforeTemplate(string emailto, string f_name, string l_name, string containerNo, string shippingline, string shipmentRefNo, string finaldate)
        {
            string html = "";
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            string pathOverview = "https://containertracking.dk/assets/images/shipment-btn.png";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            //if (f_name != "" ||f_name!=null)
            //{
            //    html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'><b>" + f_name + " " + l_name + ",</b></td></tr>";
            //}
            html += "<tr><td colspan='2' style='padding:15px 10px 5px 50px;margin:10px 0px 0px 0px;'><b>Container Information</b></td></tr>";
            html += "<tr><td  style='width:35%; padding:50px 10px 5px 50px;margin:30px 0px 0px 0px;font-weight:600; vertical-align:top;'><b>Expected Arrival Destination </b></td><td style='width:65%; vertical-align:top; padding:50px 50px 5px 50px;margin:30px 0px 0px 0px;'>" + finaldate + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Shipment ref. </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + shipmentRefNo + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Container No. </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + containerNo + "</td></tr>";
            html += "<tr><td  style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Shipping Line </b></td><td style='width:65%; vertical-align:top; padding:5px 50px 5px 50px;'>" + shippingline + "</td></tr><br/><br/>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 10px 15px;'><a href='https://containertracking.dk/Shipment'><img width='250' src='" + pathOverview + "'></a></td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [NonAction]
        public string htmlNotifyTemplateGateout(string f_name, string l_name, string container_no, string origin, string destination)
        {
            string html = "";
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            //html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'><b>" + f_name + " " + l_name + ",</b></td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>Container number " + container_no + " from " + origin + " has arrived in " + destination + " and we have registered that the container is now Gate Out</td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [NonAction]
        public string htmlNotifyTemplateDemo(string emailto, string f_name, string l_name)
        {
            string html = "";
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            //html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'> Dear <b>" + f_name + " " + l_name + ",</b></td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>We have received your request for a Demo and you will be contacted as soon as possible.</ td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [HttpGet]
        [Route("sendnewUserAddedEmail")]
        public async Task<IActionResult> sendnewUserAddedEmail(string emailto, string company_name)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress(emailto));
                message.IsBodyHtml = true; //to make message body as html  
                message.Subject = "Access to Container Tracking List for user " + company_name;
                message.Body = htmlnewUserAdded(emailto, company_name);
                smtpDetails();
                smtp.Send(message);
                return Ok("1");
            }
            catch (Exception ex)
            {
                return Ok("0");
            }
        }
        [NonAction]
        public void deleteContainerMail(string emailto, string container_no, string name)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress(emailto));
                message.IsBodyHtml = true; //to make message body as html  
                message.Subject = "Container " + container_no + " Deletion Alert";
                message.Body = htmldeleteContainerMail(container_no, name);
                smtpDetails();
                smtp.Send(message);

            }
            catch (Exception)
            {
            }
        }
        [NonAction]
        public string htmlnewUserAdded(string emailto, string company_name)
        {
            string html = "";
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            //html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'> <b>" + company_name + ",</b></td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>Your account has been created with</ td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'><b>Username - " + emailto + "</b></ td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'><b>Password - 12345</b></ td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>If you already have an account, you can log in with your normal password, and if you do not have a profile at www.containertracking.dk, you can create your profile at this link: <b><a href='https://www.containertracking.dk/register'>www.containertracking.dk/register</a></b></ td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [NonAction]
        public string htmldeleteContainerMail(string container_no, string name)
        {
            string html = "";
            var path = "https://containertracking.dk/assets/images/emailheader.png";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table  cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; font-size: 12pt; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            //html += "<tr><td colspan='2' style='padding:40px 10px 5px 50px'><span style='float:left;'> <b>" + name + ",</b></td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>Your Container <b>" + container_no + "</b> and has been deleted. It will no longer be available for notifications.</ td></tr>";
            html += "<tr><td colspan='2' style='padding:5px 10px 5px 50px'>Kindly login at <b><a href='http://www.containertracking.dk'>www.containertracking.dk</a></b> for more information.</ td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }
        [NonAction]
        public void sendTrackingupdateMail(string emailto, string f_name, string l_name, string shipmentref, string origin, string containertype, string destination, string containerNo, string departure, string arrival, string sealine, double NoOfdays, string status)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress("jesper@containertracking.dk");
                message.To.Add(new MailAddress(emailto));
                message.Subject = "Container Tracking Information of " + containerNo;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlTrackingupdateTemplate(emailto, f_name, l_name, shipmentref, origin, containertype, destination, containerNo, departure, arrival, sealine, NoOfdays, status);
                smtpDetails();
                smtp.Send(message);
                //sendClientMail(arr);
            }
            catch (Exception ex)
            {
                
            }
        }
        [NonAction]
        public string htmlTrackingupdateTemplate(string emailto, string f_name, string l_name, string shipmentref, string origin, string containertype, string destination, string containerNo, string departure, string arrival, string sealine, double NoOfdays, string status)
        {
            string html = "";

            string path = "https://containertracking.dk/assets/images/emailheader.png";
            string pathOverview = "https://containertracking.dk/assets/images/shipment-btn.png";
            //html += "";
            html += "<html xmlns = 'http://www.w3.org/1999/xhtml'><head><title></title></head><body style='font-family: Barlow, regular; background='#f2f2f2'>";
            html += "<div align='center' width='640' style='width:640px; margin:0 auto; background:#fff;'><table width='640' cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:640px; border:2px #D3D3D3 solid;  background:#fff; font-family:Barlow, regular;'>";
            html += "<tr><td colspan='2'><img width='640' style='width:100%;height:auto;' src = '" + path + "'/></td></tr>";
            html += "<tr><td style='width:35%; padding:40px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Shipment ref.  </b></td><td style='width:65%; padding:40px 50px 5px 50px; vertical-align:top;'>" + shipmentref + "</td></tr>";
            html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Origin  </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + origin + "</td></tr>";
            html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Destination  </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + destination + "</td></tr>";
            html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Container Number  </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + containerNo + "</td></tr>";
            html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Departure </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + Convert.ToDateTime(departure).ToString("dd-MMM-yyyy") + "</td></tr>";
            html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Arrival  </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + Convert.ToDateTime(arrival).ToString("dd-MMM-yyyy") + "</td></tr>";
            html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Shipping Line </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + sealine + "</td></tr>";
            //html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Status </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + status + "</td></tr>";           
            //html += "<tr><td style='width:35%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Container type  </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top;'>" + containertype + "</td></tr>";                  
            if (NoOfdays == 0)
            {
                html += "<tr><td style='width:30%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Note </b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top; font-weight:300;'>Please note that your container is on time.</td></tr>";
            }
            else if (NoOfdays < 0)
            {
                html += "<tr><td style='width:30%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Note</b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top; font-weight:300;'>Please note that your container is " + NoOfdays + " days delay.</td></tr>";
            }
            else
            {
                html += "<tr><td style='width:30%; padding:5px 10px 5px 50px; font-weight:600; vertical-align:top;'><b>Note</b></td><td style='width:65%; padding:5px 50px 5px 50px; vertical-align:top; font-weight:300;'>Please note that your container is " + NoOfdays + " days early.</td></tr>";
            }
            html += "<tr><td colspan='2' align='center' style='padding:50px 10px 15px;'><a href='https://containertracking.dk/Shipment'><img width='250' src='" + pathOverview + "'></a></td></tr>";
            html += "<tr><td colspan='2' align='center' style='padding:50px 0'>&copy; " + DateTime.Now.Year.ToString() + " Container Tracking. All Rights Reserved.</td></tr></table></div>";
            html += "</body></html>";
            return html;
        }




    }
}