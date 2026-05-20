using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Lab1.BLL.Components
{
    public class Memory
    {
        public string Type { get; set; }
        public int CapacityGB { get; set; }

        public int UsedGB { get; private set; } = 0;

        public Memory(string type, int capacity)
        {
            Type = type;
            CapacityGB = capacity;
        }

        public bool Allocate(int gb)
        {
            if (UsedGB + gb <= CapacityGB)
            {
                UsedGB += gb;
                return true; 
            }
            return false; 
        }
    }
}