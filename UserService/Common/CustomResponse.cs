using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Common
{
    // Custom response type
    public class CustomResponse<T>
    {
        public HttpStatusCode ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

}
