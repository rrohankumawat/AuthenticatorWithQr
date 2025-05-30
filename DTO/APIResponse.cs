using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class APIResponse<T>
    {
        public T? Data { get; set; }

        public string? Message { get; set; }

        public HttpStatusCode Status { get; set; }
        public bool Success { get; set; }
        public CustomException? Error { get; set; }
    }


    public class CustomException : ApplicationException
    {
        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception? inner)
            : base(message, inner)
        {
        }

        //Overriding the HelpLink Property
        public override string HelpLink
        {
            get
            {
                return "Get More Information from here: https://sitelink.com/customPageForErrorDescription";
            }
        }
    }
}
