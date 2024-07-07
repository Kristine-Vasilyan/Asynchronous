using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAwaiter
{
    internal class Delay
    {
        public int Time { get; set; }
        public Delay(int Time)
        {
            this.Time = Time;
        }
        public DelayAwaiter GetAwaiter()
        {
            DelayAwaiter delayAwaiter = new();
            delayAwaiter.Time = Time;   
            return delayAwaiter;
        }
    }
}
