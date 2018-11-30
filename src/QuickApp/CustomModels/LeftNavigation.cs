using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOI.Patrol.CustomModels
{
    public class LeftNavigation
    {
        public string label { get; set; }
        public string route { get; set; }
        public string iconClasses { get; set; }
        public List<LeftNavigation> children { get; set; }
        public int order { get; set; }
    }
}
