using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Entity
{
    public class ParameterTBL
    {
        public int ID { get; set; }
        public int IDStation{ get; set; }
        public string ParameterName {  get; set; }
        public StationTBL StationTBL { get; set; }  
        public ICollection<ObserveDataTBL> ObserveDataTBLs { get; set; }
    }
}
