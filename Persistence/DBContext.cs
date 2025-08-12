using ClassLibrary1.Entity;
using Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Persistence
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=MyConnectionString") { }

        public DbSet<ParameterTBL> Parameters { get; set; }
        public DbSet<StationTBL> Stations { get; set; }
        public DbSet<ObserveDataTBL> ObserveDatas { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            var model = modelBuilder.Build(Database.Connection);


            ISqlGenerator sqlGenerator = new SqliteSqlGenerator();
            string sql = sqlGenerator.Generate(model.StoreModel);

            Debug.WriteLine(sql);

            IDatabaseCreator sqliteDatabaseCreator = new SqliteDatabaseCreator();
            //sqliteDatabaseCreator.Create(Database, model);


            modelBuilder.Configurations.Add(new ParameterTBLConfiguration());
            modelBuilder.Configurations.Add(new StationTBLConfiguration());
            modelBuilder.Configurations.Add(new ObserveDataTBLConfiguration());

            base.OnModelCreating(modelBuilder);
        }


    
    }
}
