using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Lab1.BLL.Components
{
    public class Processor
    {
        public string Model { get; set; }
        public int Cores { get; set; }
        public Processor(string model, int cores) { Model = model; Cores = cores; }
    }
}
