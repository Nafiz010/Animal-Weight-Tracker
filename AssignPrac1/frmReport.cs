using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssignPrac1
{
    public partial class frmReport : Form
    {
        AnimalWeightTrackerEntities3 db = new AnimalWeightTrackerEntities3();
        bool mouseDown;
        private Point offset;
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            
            var ani = db.Animals.ToList();
            cboAnimal.DataSource = ani;
            cboAnimal.DisplayMember = "Name";
            cboAnimal.ValueMember = "Animal_Id";

            chart1.Titles.Add("Daily Recorded Weight");
            var data = db.Daily_Measurement.Select(d => new {d.DOM, d.Weight }).ToList();
            chart1.DataSource = data;

            chart1.Series["Series1"].XValueMember = "DOM";
            chart1.Series["Series1"].YValueMembers = "Weight";

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            int id = Int32.Parse(cboAnimal.SelectedValue.ToString());

            ////Total Weight Based on Animal
            var msum = db.Daily_Measurement.Where(t => t.Daily_Log.Course.Animal.Animal_Id == id).Sum(d => d.Weight);
            //Count for Average
            var count = db.Daily_Measurement.Count(t => t.Daily_Log.Course.Animal.Animal_Id == id);
            //Average Weight
            var avg = msum / count;

            //First day Weight
            var fw = db.Animals.Where(t => t.Animal_Id == id).FirstOrDefault();
            Double weight = Double.Parse(fw.Weight.ToString());

            //Weight Loss Gain
            var wlg = Convert.ToDouble(avg) - weight;

            lblLossGain.Text = wlg.ToString();

            try
            {
                var mWeight = db.Daily_Measurement.Where(d => d.Shift == "Morning").Sum(d => d.Weight);
                var eWeight = db.Daily_Measurement.Where(d => d.Shift == "Evening").Sum(d => d.Weight);
                var avgWeight = (mWeight + eWeight) / 2;
                lblAVGW.Text = avgWeight.ToString();
            }
            catch (Exception)
            {


            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Hide();
            frmMainMenu MM = new frmMainMenu();
            MM.ShowDialog();
        }

        private void MouseDown_Event(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void MouseUp_Event(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void MouseMove_Event(object sender, EventArgs e)
        {
            mouseDown = false;
        }
    }
}
