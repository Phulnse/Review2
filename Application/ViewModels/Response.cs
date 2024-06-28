using System.Runtime.InteropServices;

namespace Application.ViewModels
{
    public class Response
    {
        public int statusCode { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        public bool isSuccess { get; set; }

        public Response(int statusCode, [Optional] object data, [Optional] string message)
        {
            this.statusCode = statusCode;
            this.isSuccess = true;
            this.data = data;
            this.message = message;
        }

        public Response(int statusCode, string message)
        {
            this.statusCode = statusCode;
            this.message = message;
            this.isSuccess = false;
        }
    }
}
