using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomModels
{
    public class Shifts
    {
        public int shiftID { get; set; }
        public string name { get; set; }
        public int startingHour { get; set; }
        public int numberOfHours { get; set; }
    }
}
