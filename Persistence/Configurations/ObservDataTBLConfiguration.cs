using ClassLibrary1.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class ObserveDataTBLConfiguration : EntityTypeConfiguration<ObserveDataTBL>
    {
        public ObserveDataTBLConfiguration()
        {
            ToTable("ObserveDataTBL", "Test");

            HasKey(s => s.ID);


            HasRequired(o => o.Parameter)
                .WithMany(p => p.ObserveDataTBLs)
                .HasForeignKey(o => o.IDParameter)
                .WillCascadeOnDelete(false);
        }
    }
}
