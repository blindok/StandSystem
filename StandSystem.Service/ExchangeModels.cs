using System;
using System.Collections.Generic;
using System.Text;

namespace StandSystem.Service
{
    public class StandInfoModel
    {
        public string Data { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
    }

    public class ClientInfoModel
    {
        public string Data { get; set; }
        public bool IsProgrammNeed { get; set; }
        public string Path { get; set; }
    }
}