using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomAwaiter
{
    internal class DelayAwaiter : INotifyCompletion
    {
        public int Time { get; set; }
        public void OnCompleted(Action continuation)
        {
            Thread.Sleep(Time);
            continuation.Invoke();
        }
        public bool IsCompleted { get; private set; }
        public void GetResult() { }
    }
}
