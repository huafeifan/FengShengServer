using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class ProtosListenerObject
    {
        public Action<object> Action { get; set; }
        public int RemainCount { get; set; }
    }
}
