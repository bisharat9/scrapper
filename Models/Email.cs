using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace emailsender.Models
{
    public class Email
    {
       
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public HttpPostedFileBase file { get; set; }
        public HttpPostedFileBase EmailFile { get; set; }

    }
}