﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class SendMail { 
    
        private static SendMail instance = null;
        private static readonly object instanceLock = new object();
        private SendMail() { }
        public static SendMail Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SendMail();
                    }
                    return instance;
                }
            }
        }
        //=================================================================================================
        public void Send(String ToMail, String input, String title)
        {
            try
            {
                var frommail = new MailAddress("sstore1403@gmail.com");
                var tomail = new MailAddress(ToMail);
                String pass = "rwepsohznkzwkfzr";
                //String subject = "SStore Forgot Password";
                String body = input;

                var smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(frommail.Address, pass);
                smtp.Timeout = 2000000;

                var mess = new MailMessage(frommail, tomail);

                mess.Subject = title;
                mess.Body = body;
                smtp.Send(mess);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        //-------------------------------------------------------------------------------
        public String GetOTP()
        {

            string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string LowerCase = "qwertyuiopasdfghjklzxcvbnm";
            string Digits = "1234567890";
            string allCharacters = UpperCase + LowerCase + Digits;
            //Random will give random charactors for given length  
            Random r = new Random();
            String password = "";
            for (int i = 0; i < 8; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    password += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
                }
                else
                {
                    password += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
                }
            }
            return password;
        }
    }
}
