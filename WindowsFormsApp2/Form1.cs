using ClassLibrary1.Entity;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public List<string> strs = new List<string>();
        private DBContext db;
        private List<StationTBL> stations;
        private List<ParameterTBL> parameters;
        public Form1()
        {
            InitializeComponent();
            db = new DBContext();

            dataGridView1.CellValueChanged += DgvStations_CellValueChanged;
            dataGridView1.UserDeletingRow += dataGridView1_UserDeletingRow;


            dataGridView2.CellValueChanged += DgvParam_CellValueChanged;
            dataGridView2.UserDeletingRow += dataGridView2_UserDeletingRow;


            dataGridView3.CellValueChanged += DgvValue_CellValueChanged;
            dataGridView3.UserDeletingRow += dataGridView3_UserDeletingRow;

            dataGridView5.CellValueChanged += DgvValue2_CellValueChanged;
            dataGridView5.UserDeletingRow += dataGridView5_UserDeletingRow;
            

            LoadData();
            LoadCombos();
            comboBoxStations_SelectedIndexChanged(dataGridView3, EventArgs.Empty);
            comboBoxParam_SelectedIndexChanged(dataGridView5,EventArgs.Empty);


        }

        private void LoadCombos()
        {
            paramStations.DataSource = stations;
            paramStations.DisplayMember = "NameStation";
            paramStations.ValueMember = "ID";
            paramStations.SelectedIndexChanged += comboBoxStations_SelectedIndexChanged;

            paramStations2.DataSource = stations;
            paramStations2.DisplayMember = "NameStation";
            paramStations2.ValueMember = "ID";
            paramStations2.SelectedIndexChanged += comboBoxStations_SelectedIndexChanged;

            paramCombo.DataSource = parameters;
            paramCombo.SelectedIndexChanged += comboBoxParam_SelectedIndexChanged;
            paramCombo.DisplayMember = "ParameterName";
            paramCombo.ValueMember = "ID";

        }
        private async void LoadData()
        {

            //stations = await db.Stations.AsNoTracking().ToListAsync();
            

            this.parameters = (await db.Parameters.ToListAsync()).GroupBy(p => p.ParameterName).Select(g => g.First()).ToList();
            //dataGridView1.DataSource = stations;

            stations= await db.Stations.AsNoTracking().ToListAsync();
            
            var bindingList = new BindingList<ClassLibrary1.Entity.StationTBL>(stations);
            dataGridView1.DataSource = bindingList;


            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["ParameterTBLs"].Visible = false;


            var parameters = await db.Parameters.AsNoTracking().ToListAsync();
            dataGridView2.DataSource = parameters;
            comboBoxStations_SelectedIndexChanged(dataGridView1, EventArgs.Empty);

            var observeData = await db.ObserveDatas.AsNoTracking().ToListAsync();
            dataGridView3.DataSource = observeData;
            dataGridView5.DataSource = observeData;


        }
        private async void DgvStations_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var editedRow = dataGridView1.Rows[e.RowIndex];
            var newValue = editedRow.Cells[e.ColumnIndex].Value;
            if (string.IsNullOrEmpty(newValue.ToString()))
            {
                return;
            }
            var id = editedRow.Cells["ID"].Value;
            if ((int)id == 0)
            {
                if (db.Stations.Any(r => r.NameStation == newValue.ToString()))
                {
                    MessageBox.Show("نام ایستگاه نباید تکراری باشد");
                    editedRow.Cells[e.ColumnIndex].Value = "";
                    return;
                }
                var station = new StationTBL
                {
                    NameStation = newValue.ToString()
                };
                db.Stations.AddOrUpdate(station);
                db.SaveChanges();
                LoadData();
                LoadCombos();
            }
            else
            {
                var station = await db.Stations.FindAsync(id);
                station.NameStation = newValue.ToString();
                db.Stations.AddOrUpdate(station);
                await db.SaveChangesAsync();
            }
        }
        private async void DgvParam_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var editedRow = dataGridView2.Rows[e.RowIndex];
            var newValue = editedRow.Cells[e.ColumnIndex].Value;
            if (string.IsNullOrEmpty(newValue.ToString()))
            {
                return;
            }
            var id = editedRow.Cells["ID"].Value;
            if ((int)id == 0)
            {
                var station = new ParameterTBL
                {
                    ParameterName = newValue.ToString(),
                    IDStation = (int)paramStations.SelectedValue,
                };
                if(db.Parameters.Any(r=>r.ParameterName==station.ParameterName && r.IDStation == station.IDStation))
                {
                    MessageBox.Show($"این پارامتر برای ایستگاه {station.ID} وجود دارد");
                    editedRow.Cells[e.ColumnIndex].Value = "";

                    return;
                }
                db.Parameters.AddOrUpdate(station);
                db.SaveChanges();
                LoadData();
                LoadCombos();
            }
            else
            {
                var station = await db.Parameters.FindAsync(id);
                station.ParameterName = newValue.ToString();
                db.Parameters.AddOrUpdate(station);
                await db.SaveChangesAsync();
            }
        }
        private async void DgvValue_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var editedRow = dataGridView3.Rows[e.RowIndex];


            var newValue = editedRow.Cells[e.ColumnIndex].Value.ToString();
            if (string.IsNullOrEmpty(newValue.ToString()))
            {
                return;
            }
            var idd = editedRow.Cells["ID"].Value.ToString();
            var date = editedRow.Cells["Date"].Value.ToString();
            if (string.IsNullOrEmpty(date) || !Regex.IsMatch(date, @"^\d{2}-\d{1,2}-\d{1,2}$"))
            {

                PersianCalendar pc = new PersianCalendar();

                editedRow.Cells["Date"].Value = pc.GetYear(DateTime.Now).ToString().Substring(2) + "-" +
                    (pc.GetMonth(DateTime.Now) < 10 ? "0" + pc.GetMonth(DateTime.Now) : pc.GetMonth(DateTime.Now).ToString()) +
                    "-" + (pc.GetDayOfMonth(DateTime.Now) < 10 ? "0" + pc.GetDayOfMonth(DateTime.Now) : pc.GetDayOfMonth(DateTime.Now) + "");
                date = editedRow.Cells["Date"].Value.ToString();


            }


            var paramName = dataGridView3.Columns[e.ColumnIndex].HeaderText;
            var value = editedRow.Cells[paramName];

            int id = 0;


            if (!int.TryParse(idd, out id) && (int)id == 0)
            {
                float val = 0;
                float.TryParse(newValue, out val);
                if (paramName == "Date")
                    return;
                var station = new ObserveDataTBL
                {
                    Value = val,
                    IDParameter = db.Parameters
                    .Where(r => r.ParameterName == paramName && r.IDStation == (int)paramStations.SelectedValue)
                    .FirstOrDefault().ID,
                    Date = date,
                };
                if(db.ObserveDatas.Any(r=>r.Date==station.Date && r.IDParameter == station.IDParameter))
                {
                    MessageBox.Show("در تاریخ انتخابی برای پارامتر مورد نظر یک رکورد از پیش ثبت شده است");
                    return;
                }
                db.ObserveDatas.AddOrUpdate(station);
                db.SaveChanges();
                LoadData();
            }
            else
            {
                var station = await db.ObserveDatas.FindAsync(id);
                station.Date = date;
                if (paramName != "Date")
                {
                    try
                    {
                        station.Value = (float.Parse(editedRow.Cells[paramName].Value.ToString()));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"خطای مقدار پارامتر {paramName}. باید Float باشد");
                    }
                }
                db.ObserveDatas.AddOrUpdate(station);
                await db.SaveChangesAsync();
            }
        }
       
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید این سطر را حذف کنید؟",
                "تایید حذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                int id = Convert.ToInt32(e.Row.Cells["ID"].Value);
                var dels = db.ObserveDatas.Where(r => r.Parameter.IDStation == id);
                db.ObserveDatas.RemoveRange(dels);
                db.SaveChanges();
                var dels2 = db.Parameters.Where(r => r.IDStation == id);
                db.Parameters.RemoveRange(dels2);
                db.SaveChanges();
                db.Stations.Remove(db.Stations.Find(id));
                db.SaveChanges();

            }
        }
        private void dataGridView2_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید این سطر را حذف کنید؟",
                "تایید حذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                int id = Convert.ToInt32(e.Row.Cells["ID"].Value);
                var dels = db.ObserveDatas.Where(r => r.IDParameter == id);
                db.ObserveDatas.RemoveRange(dels);
                db.SaveChanges();
                var dels2 = db.Parameters.Where(r => r.ID == id);
                db.Parameters.RemoveRange(dels2);
                db.SaveChanges();

            }
        }
        private void dataGridView3_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید این سطر را حذف کنید؟",
                "تایید حذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                int idStation = (int)paramStations2.SelectedValue;

                string date = e.Row.Cells["Date"].Value.ToString();
                var dels = db.ObserveDatas.Where(r => r.Parameter.IDStation == idStation && r.Date== date);
                db.ObserveDatas.RemoveRange(dels);
                db.SaveChanges();

            }
        }
        private void dataGridView5_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(
                "آیا مطمئن هستید که می‌خواهید این سطر را حذف کنید؟",
                "تایید حذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                string date = e.Row.Cells["Date"].Value.ToString();
                string paramname = ((ParameterTBL)paramCombo.SelectedItem).ParameterName;
                List<int> idparams = db.Parameters.Where(r => r.ParameterName == paramname).Select(r => r.ID).ToList();

                var dels = db.ObserveDatas.Where(r => idparams.Any(r1=>r1==r.ID) && r.Date==date);
                db.ObserveDatas.RemoveRange(dels);
                db.SaveChanges();

            }
        }


        private async void DgvValue2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var editedRow = dataGridView5.Rows[e.RowIndex];


            var newValue = editedRow.Cells[e.ColumnIndex].Value.ToString();
            if (string.IsNullOrEmpty(newValue.ToString()))
            {
                return;
            }
            var idd = editedRow.Cells["ID"].Value.ToString();
            var date = editedRow.Cells["Date"].Value.ToString();
            if (string.IsNullOrEmpty(date) || !Regex.IsMatch(date, @"^\d{2}-\d{1,2}-\d{1,2}$"))
            {

                PersianCalendar pc = new PersianCalendar();

                editedRow.Cells["Date"].Value = pc.GetYear(DateTime.Now).ToString().Substring(2) + "-" +
                    (pc.GetMonth(DateTime.Now) < 10 ? "0" + pc.GetMonth(DateTime.Now) : pc.GetMonth(DateTime.Now).ToString()) +
                    "-" + (pc.GetDayOfMonth(DateTime.Now) < 10 ? "0" + pc.GetDayOfMonth(DateTime.Now) : pc.GetDayOfMonth(DateTime.Now) + "");
                date = editedRow.Cells["Date"].Value.ToString();


            }
            var paramid = (int)paramCombo.SelectedValue;


            var stationName = dataGridView5.Columns[e.ColumnIndex].HeaderText;
            if(!db.Parameters.Any(r => r.StationTBL.NameStation == stationName && r.ParameterName== ((ParameterTBL)paramCombo.SelectedItem).ParameterName))
            {
                if (stationName == "Date")
                    return;
                db.Parameters.Add(new ParameterTBL
                {
                    IDStation=stations.Where(r=>r.NameStation==stationName).FirstOrDefault().ID,
                    ParameterName= ((ParameterTBL)paramCombo.SelectedItem).ParameterName
                });
                await db.SaveChangesAsync();
            }
            paramid = db.Parameters.Where(r => r.StationTBL.NameStation == stationName && r.ParameterName== ((ParameterTBL)paramCombo.SelectedItem).ParameterName).First().ID;


            var value = editedRow.Cells[stationName];

            int id = 0;


            if (!db.ObserveDatas.Any(r=>r.IDParameter== paramid))
            {
                float val = 0;
                float.TryParse(newValue, out val);
                if (stationName == "Date")
                    return;
                var station = new ObserveDataTBL
                {
                    Value = val,
                    IDParameter = paramid,
                    Date = date,
                };
                db.ObserveDatas.AddOrUpdate(station);
                db.SaveChanges();
                LoadData();

            }
            else
            {
                var station = await db.ObserveDatas.Where(r=>r.IDParameter==paramid).FirstOrDefaultAsync();
                station.Date = date;
                if (stationName != "Date")
                {
                    try
                    {
                        station.Value = (float.Parse(editedRow.Cells[stationName].Value.ToString()));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"خطای مقدار پارامتر {stationName}. باید Float باشد");
                    }
                }
                db.ObserveDatas.AddOrUpdate(station);
                await db.SaveChangesAsync();
            }
        }
        private async void comboBoxStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (paramStations.SelectedItem is StationTBL selectedStation)
            {
                dataGridView3.Columns.Clear();
                var list = await db.Parameters.Where(r => r.IDStation == selectedStation.ID).ToListAsync();
         
                var bindingList = new BindingList<ClassLibrary1.Entity.ParameterTBL>(list);

                dataGridView2.DataSource = bindingList;

                dataGridView2.Columns["ObserveDataTBLs"].Visible = false;
                dataGridView2.Columns["StationTBL"].Visible = false;
                dataGridView2.Columns["IDStation"].Visible = false;
                dataGridView2.Columns["ID"].Visible = false;


                var parameters = list.Distinct().ToList();

                if (!parameters.Any(r => r.ID != 0))
                {
                    dataGridView3.Visible = false;
                    return;
                }
                else
                {
                    dataGridView3.Visible = true;

                }
                var list2 = await db.ObserveDatas.Where(r => r.Parameter.IDStation == selectedStation.ID)
                        .Include(r => r.Parameter)
                        .Select(r => new
                        {
                            r.Date,
                            ParameterName = r.Parameter.ParameterName,
                            IDParam = r.Parameter.ID,
                            r.Value,
                            r.ID
                        })
                        .ToListAsync();

                var table = new DataTable();
                table.Columns.Add("Date", typeof(string));
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("IDParam", typeof(int));
                table.Columns["IDParam"].ReadOnly = true;
                table.Columns["ID"].ReadOnly = true;

                foreach (var param in parameters)
                    table.Columns.Add(param.ParameterName, typeof(decimal));



                var grouped = list2.GroupBy(d => d.Date);
                foreach (var g in grouped)
                {
                    var row = table.NewRow();
                    row["Date"] = g.Key;
                    row["ID"] = g.FirstOrDefault().ID;
                    row["IDParam"] = g.FirstOrDefault().IDParam;

                    foreach (var item in g)
                        row[item.ParameterName] = item.Value;

                    table.Rows.Add(row);
                }

                dataGridView3.DataSource = table;

                dataGridView3.Columns["IDParam"].Visible = false;

            }
        }

        private async void comboBoxParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (paramCombo.SelectedItem is ParameterTBL selectedCombo)
            {
                var parameters = stations.Distinct().ToList();

                if (!parameters.Any(r => r.ID != 0))
                {
                    dataGridView3.Visible = false;
                    return;
                }
                else
                {
                    dataGridView3.Visible = true;

                }

                var list2 = await db.ObserveDatas.Where(r => r.Parameter.ParameterName.ToLower()==selectedCombo.ParameterName.ToLower())
                .Include(r => r.Parameter)
                .Include(r=>r.Parameter.StationTBL)
                .Select(r => new
                {
                    r.Date,
                    ParameterName = r.Parameter.ParameterName,
                    IDParam = r.Parameter.ID,
                    IDStation=r.Parameter.StationTBL.ID,
                    StationName=r.Parameter.StationTBL.NameStation,
                    r.Value,

                    r.ID
                })
                .ToListAsync();

                var table = new DataTable();
                table.Columns.Add("Date", typeof(string));
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("IDParam", typeof(int));
                table.Columns.Add("IDStation", typeof(int));
                table.Columns.Add("StationName", typeof(string));
                table.Columns["IDParam"].ReadOnly = true;
                table.Columns["ID"].ReadOnly = true;
                table.Columns["IDStation"].ReadOnly = true;
                table.Columns["StationName"].ReadOnly = true;
                table.Columns["StationName"].ReadOnly = true;

                foreach (var param in parameters)
                    table.Columns.Add(param.NameStation, typeof(decimal));



                var grouped = list2.GroupBy(d => d.Date);
                foreach (var g in grouped)
                {
                    var row = table.NewRow();
                    foreach (var child in g.ToList())
                    {
                        row["Date"] = child.Date;
                        row["ID"] = child.ID;
                        row["IDParam"] = child.IDParam;
                        row["IDStation"] = child.IDStation;
                        row["StationName"] = child.StationName;
                        row[child.StationName] = child.Value;
                    }
                    table.Rows.Add(row);

                }

                dataGridView5.DataSource = table;

                dataGridView5.Columns["IDParam"].Visible = false;
                dataGridView5.Columns["StationName"].Visible = false;
                dataGridView5.Columns["IDStation"].Visible = false;

            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'test_DBDataSet.STATIONS' table. You can move, or remove it, as needed.

        }
    }
}
