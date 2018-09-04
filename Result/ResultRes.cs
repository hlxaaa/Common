using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Result
{
    
    public class ResultRes
    {
        public int httpcode { get; set; }

        public string message { get; set; }
    }

    public class ResultForWeb
    {
        public int HttpCode { get; set; }
        public string data { get; set; }
        public string Message { get; set; }
    }

}
