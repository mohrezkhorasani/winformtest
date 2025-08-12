using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Entity
{
    public class StationTBL
    {
        public int ID { get; set; }
        public string NameStation { get; set; }
        public ICollection<ParameterTBL> ParameterTBLs { get; set; }

    }
}
