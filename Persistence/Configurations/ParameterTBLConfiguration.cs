using ClassLibrary1.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Persistence.Configurations
{
    public class ParameterTBLConfiguration : EntityTypeConfiguration<ParameterTBL>
    {
        public ParameterTBLConfiguration()
        {
            ToTable("ParameterTBL","Test");

            HasKey(p => p.ID);

            HasRequired(p => p.StationTBL)
                .WithMany(s => s.ParameterTBLs)
                .HasForeignKey(p => p.IDStation)
                .WillCascadeOnDelete(false);
        }
    }
}
