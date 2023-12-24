using StandSystem.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace StandSystem.Service
{
    class FakeTcl : ITcl
    {
        private string oldValue = "";
        private string newValue = "";
        public string GetDataFromDeviceHex()
        {
            var rand = new Random();
            if (rand.Next(5) == 3)
            {
                oldValue = newValue;
            }
            return oldValue;
        }

        public void SetDataToDeviceHex(string value)
        {
            newValue = value;
        }

        public void Start() { }
        public void Stop() { }
    }
}