using emailsender.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using System.ServiceModel;

namespace emailsender.Controllers
{
    public class EmailController : Controller
    {


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Email email)
        {
            try
            {
                string sMail_Server_Name = "10.10.30.92";
                int iMail_Server_Port = 25;

                SmtpClient client = new SmtpClient();
                client.Host = sMail_Server_Name;
                client.Port = iMail_Server_Port;
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential();

                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("anonymous", "anonymous");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (email.EmailFile == null)
                {
                    MailMessage mail = new MailMessage();
                    mail.Subject = email.Subject;
                    mail.From = new MailAddress("NOreply@mtbc.com");
                    mail.To.Add(email.Receiver);
                    mail.Body = email.Message;
                    if (email.file != null)
                    {
                        string filename = Path.GetFileName(email.file.FileName);
                        mail.Attachments.Add(new Attachment(email.file.InputStream, filename));

                    }
                    mail.IsBodyHtml = true;
                    client.Send(mail);
                    ViewBag.Success = "Email Has been sent successfully.";
                    client.Dispose();
                }
                else
                {
                    var fileName = Path.GetFileName(email.EmailFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Document"), fileName);
                    var extension = Path.GetExtension(email.EmailFile.FileName);
                    email.EmailFile.SaveAs(filePath);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (extension.Trim() == ".xlsx")
                        {
                            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {

                                    while (reader.Read()) //Each row of the file
                                    {
                                        MailMessage mail = new MailMessage();
                                        mail.Subject = email.Subject;
                                        mail.From = new MailAddress("NOreply@mtbc.com");
                                        mail.To.Add(reader.GetValue(0).ToString());
                                        mail.Body = email.Message;
                                        if (email.file != null)
                                        {
                                            string filename = Path.GetFileName(email.file.FileName);
                                            mail.Attachments.Add(new Attachment(email.file.InputStream, filename));
                                        }
                                        mail.IsBodyHtml = true;
                                        client.Send(mail);
                                        ViewBag.Success = "Email Has been sent successfully.";

                                    }
                                }
                            }
                        }
                        else
                        {
                            using (StreamReader sr = new StreamReader(Path.Combine(Server.MapPath("~/Document"), fileName)))
                            {
                                while (sr.Peek() >= 0)
                                {
                                    MailMessage mail = new MailMessage();
                                    mail.Subject = email.Subject;
                                    mail.From = new MailAddress("NOreply@mtbc.com");
                                    mail.To.Add(sr.ReadLine());
                                    mail.Body = email.Message;
                                    if (email.file != null)
                                    {
                                        string filename = Path.GetFileName(email.file.FileName);
                                        mail.Attachments.Add(new Attachment(email.file.InputStream, filename));
                                    }
                                    mail.IsBodyHtml = true;
                                    client.Send(mail);
                                    ViewBag.Success = "Email Has been sent successfully.";

                                }
                            }
                        }
                        client.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

            }
            return View();
        }
        public ActionResult Crawler()
        {
            return View();
        }

    }
}