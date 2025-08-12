using ClassLibrary1.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class StationTBLConfiguration : EntityTypeConfiguration<StationTBL>
    {
        public StationTBLConfiguration()
        {
            ToTable("StationTBL", "Test");

            HasKey(s => s.ID);

        }
    }
}
