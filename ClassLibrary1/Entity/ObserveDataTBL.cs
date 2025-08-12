using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Entity
{
    public class ObserveDataTBL
    {
        public int ID { get; set; }
        public int IDParameter{ get; set; }
        public string Date{  get; set; }
        public float Value { get; set; }
        public ParameterTBL Parameter { get; set; }
    }
}
