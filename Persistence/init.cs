using ClassLibrary1.Entity;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class MyDbContextInitializer : SqliteDropCreateDatabaseAlways<DBContext>
    {
        public MyDbContextInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder) { }

        protected override void Seed(DBContext context)
        {
            //context.Set<StationTBL>().Add(new StationTBL());
            //context.Set<ParameterTBL>().Add(new ParameterTBL());
            //context.Set<ObserveDataTBL>().Add(new ObserveDataTBL());
        }
    }
}
