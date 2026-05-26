using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Lab1.BLL.Events
{
    public class DeviceErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; }

        public DeviceErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
