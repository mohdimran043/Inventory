using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModels
{
    public class Devices
    {
        public int deviceid { get; set; }
        public string devicenumber { get; set; }

        public int ahwalid { get; set; }

        public string model { get; set; }
        public int devicetypeid { get; set; }

        public int defective { get; set; }

        public int rental { get; set; }

        public string barcode { get; set; }
    }
}
