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
    public partial class frmDLog : Form
    {
        AnimalWeightTrackerEntities3 db = new AnimalWeightTrackerEntities3();
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmDLog()
        {
            InitializeComponent();
        }

        private void frmDLog_Load(object sender, EventArgs e)
        {
            var dgv = db.Daily_Log.Select(d => new { Id = d.Log_Id, Course = d.Course.Goal, Activity = d.Activity.Name, Date = d.Date, Progress = d.Progress }).ToList();
            dgvDLog.DataSource = dgv;
            var cs = db.Courses.ToList();
            var ac = db.Activities.ToList();            
            cboCourse.DataSource = cs;
            cboCourse.DisplayMember = "Id";
            cboCourse.ValueMember = "Id";
            cboActivity.DataSource = ac;
            cboActivity.DisplayMember = "Name";
            cboActivity.ValueMember = "Activity_Id";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errors = new StringBuilder();

            if (cboCourse.Text == "")
            {
                errors.AppendLine("Course cannot be blank");
            }

            if (cboActivity.Text == "")
            {
                errors.AppendLine("Activity cannot be blank");
            }

            if (cboProg.Text == "")
            {
                errors.AppendLine("Progress cannot be blank");
            }

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Daily_Log dl = new Daily_Log();
            dl.Course_Id = Int32.Parse(cboCourse.SelectedValue.ToString());
            dl.Activity_Id = Int32.Parse(cboActivity.SelectedValue.ToString()); ;
            dl.Date = DateDL.Value;
            dl.Progress = cboProg.Text;

            try
            {
                db.Daily_Log.Add(dl);
                db.SaveChanges();
                txtID.Text = dl.Log_Id.ToString();
                var dgv = db.Daily_Log.Select(d => new { Id = d.Log_Id, Course = d.Course.Goal, Activity = d.Activity.Name, Date = d.Date, Progress = d.Progress }).ToList();
                dgvDLog.DataSource = dgv;
                MessageBox.Show("Daily Log Added Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Log.Where(d => d.Log_Id == id).FirstOrDefault();

            if (data != null)
            {
                cboCourse.SelectedValue = data.Course_Id;
                cboActivity.SelectedValue = data.Activity_Id;
                DateDL.Value = data.Date.Value;
                cboProg.Text = data.Progress;

            }
            else
            {
                cboProg.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Log.Where(d => d.Log_Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Course_Id = Int32.Parse(cboCourse.SelectedValue.ToString());
                data.Activity_Id = Int32.Parse(cboActivity.SelectedValue.ToString());
                data.Date = DateDL.Value;
                data.Progress = cboProg.Text;


                db.SaveChanges();
                var dgv = db.Daily_Log.Select(d => new { Id = d.Log_Id, Course = d.Course.Goal, Activity = d.Activity.Name, Date = d.Date, Progress = d.Progress }).ToList();
                dgvDLog.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                cboProg.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Log.Where(d => d.Log_Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Daily_Log.Remove(data);

                    db.SaveChanges();

                    cboCourse.SelectedValue = "";
                    cboActivity.SelectedValue = "";
                    cboProg.Text = "";
                    var dgv = db.Daily_Log.Select(d => new { Id = d.Log_Id, Course = d.Course.Goal, Activity = d.Activity.Name, Date = d.Date, Progress = d.Progress }).ToList();
                    dgvDLog.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                cboCourse.SelectedValue = "";
                cboActivity.SelectedValue = "";
                cboProg.Text = "";
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
