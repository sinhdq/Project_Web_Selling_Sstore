using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLib
{
    public class Validate
    {
        private static Validate instance = null;
        private static readonly object instanceLock = new object();
        private Validate() { }
        public static Validate Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new Validate();
                    }
                    return instance;
                }
            }
        }

        public string DateNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        //=================================================================================================
        public bool Phone(string input)
        {
            string regex = @"(^[0]+[0-9]{9}$)";
            Regex re = new Regex(regex);

            if (re.IsMatch(input))
                return (true);
            else
                return (false);
        }
        //=================================================================================================
        public bool Mail(string input)
        {
            string regex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex re = new Regex(regex);

            if (re.IsMatch(input))
                return (true);
            else
                return (false);
        }
        //=================================================================================================
        public bool Name(string input)
        {
            string regex = @"\D+\z";
            Regex re = new Regex(regex);

            if (re.IsMatch(input))
                return (true);
            else
                return (false);
        }
    }
}
