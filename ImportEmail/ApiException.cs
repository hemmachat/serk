using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImportEmail
{
    public class ApiException : Exception
    {

        public ApiException(string message) : base(message, null)
        {
        }
    }
}