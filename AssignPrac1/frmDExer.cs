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
    public partial class frmDExer : Form
    {
        AnimalWeightTrackerEntities3 db = new AnimalWeightTrackerEntities3();
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmDExer()
        {
            InitializeComponent();
        }

        private void frmDExer_Load(object sender, EventArgs e)
        {
            var dgv = db.Daily_Exercise.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Time = d.Time, Exercise_ID = d.Exercise_Id }).ToList();
            dgvDExer.DataSource = dgv;
            var dl = db.Daily_Log.ToList();
            var ex = db.Exercises.ToList();
            cboDLog.DataSource = dl;
            cboDLog.DisplayMember = "Log_Id";
            cboDLog.ValueMember = "Log_Id";
            cboExer.DataSource = ex;
            cboExer.DisplayMember = "Name";
            cboExer.ValueMember = "Id";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errors = new StringBuilder();

            if (cboDLog.Text == "")
            {
                errors.AppendLine("Daily Log cannot be null !!");
            }

            if (DOBTime.Text == "")
            {
                errors.AppendLine("Time cannot be null !!");
            }

            if (cboExer.Text == "")
            {
                errors.AppendLine("Exercise cannot be null !!");
            }

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Daily_Exercise de = new Daily_Exercise();
            de.Log_Id = Int32.Parse(cboDLog.SelectedValue.ToString());
            de.Time = DOBTime.Text;
            de.Exercise_Id = Int32.Parse(cboExer.SelectedValue.ToString());
                       
            
            try
            {
                db.Daily_Exercise.Add(de);
                db.SaveChanges();
                txtID.Text = de.Id.ToString();
                var dgv = db.Daily_Exercise.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Time = d.Time, Exercise_ID = d.Exercise_Id }).ToList();
                dgvDExer.DataSource = dgv;
                MessageBox.Show("Daily Exercise Added Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Exercise.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                cboDLog.SelectedValue = data.Log_Id;
                DOBTime.Text = data.Time;
                cboExer.SelectedValue = data.Exercise_Id;

            }
            else
            {
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Exercise.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Log_Id = Int32.Parse(cboDLog.SelectedValue.ToString());
                data.Time = DOBTime.Text;
                data.Exercise_Id = Int32.Parse(cboExer.SelectedValue.ToString());

                db.SaveChanges();
                var dgv = db.Daily_Exercise.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Time = d.Time, Exercise_ID = d.Exercise_Id }).ToList();
                dgvDExer.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {               
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Exercise.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Daily_Exercise.Remove(data);

                    db.SaveChanges();

                    var dgv = db.Daily_Exercise.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Time = d.Time, Exercise_ID = d.Exercise_Id}).ToList();
                    dgvDExer.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {               
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void MouseMove_Event(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void MouseUp_Event(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
