﻿using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Operationlogs
    {
        public long Logid { get; set; }
        public long Userid { get; set; }
        public int Operationid { get; set; }
        public DateTime Timestamp { get; set; }
        public int Statusid { get; set; }
        public string Text { get; set; }

        public Operations Operation { get; set; }
        public Operationsstatus Status { get; set; }
        public Users User { get; set; }
    }
}
