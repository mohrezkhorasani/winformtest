using ClassLibrary1.Entity;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            using (var db = new DBContext())
            {


                if (!db.Stations.Any())
                {
                    db.Stations.Add(new StationTBL {ID=2, NameStation = "Tehran" });
                    db.Stations.Add(new StationTBL {ID=3, NameStation = "Rasht" });
                    db.Stations.Add(new StationTBL { ID = 5, NameStation = "Shiraz" });
                    db.Stations.Add(new StationTBL { ID = 6, NameStation = "Sari" });
                    db.Stations.Add(new StationTBL { ID = 7, NameStation = "Mashhad" });
                    db.SaveChanges();
                }
                if (!db.Parameters.Any())
                {
                    db.Parameters.Add(new ParameterTBL { IDStation = 2, ParameterName = "Rain", ID = 1 });
                    db.Parameters.Add(new ParameterTBL { IDStation = 2, ParameterName = "HR", ID = 3 });
                    db.Parameters.Add(new ParameterTBL { IDStation = 3, ParameterName = "Rain", ID = 4 });
                    db.Parameters.Add(new ParameterTBL { IDStation = 3, ParameterName = "Snow", ID = 5 });
                    db.Parameters.Add(new ParameterTBL { IDStation = 7, ParameterName = "Rain", ID =  6});
                    db.SaveChanges();
                }
                if (!db.ObserveDatas.Any())
                {
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 1, IDParameter = 1, Date = "99-04-01", Value = 51 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 3, IDParameter = 3, Date = "99-05-02", Value = 32 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 4, IDParameter = 4, Date = "99-05-02", Value = 3 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 5, IDParameter = 5, Date = "99-05-02", Value =17 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 7, IDParameter = 1, Date = "99-05-04", Value = 20 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 9, IDParameter = 3, Date = "99-05-04", Value = 10 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 10, IDParameter = 1, Date = "99-05-06", Value = 14 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 12, IDParameter = 3, Date = "99-05-06", Value = 2 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 13, IDParameter = 1, Date = "99-04-07", Value = 8 });
                    db.ObserveDatas.Add(new ObserveDataTBL { ID = 14, IDParameter = 3, Date = "99-04-07", Value = 5 });
                    db.SaveChanges();
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }
    }
}
