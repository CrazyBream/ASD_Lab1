using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Lab1.BLL.Events
{
    public class DeviceStateChangedEventArgs : EventArgs
    {
        public string Message { get; }

        public DeviceStateChangedEventArgs(string message)
        {
            Message = message;
        }
    }
}
