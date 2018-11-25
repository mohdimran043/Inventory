using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModels
{
    public class OperationLog
    {
        public int userID { get; set; }
        public int operationID { get; set; }
        public int statusID { get; set; }
        public string text { get; set; }

       
    }
}
