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
    public partial class frmExercise : Form
    {
        AnimalWeightTrackerEntities3 db;
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmExercise()
        {
            InitializeComponent();
        }

        private void frmExercise_Load(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            var dgv = db.Exercises.Select(d => new { Id = d.Id, Name = d.Name, Time = d.Time, Calories_Burn = d.Calories_Burn, Type = d.Exercise_Type }).ToList();
            dgvExe.DataSource = dgv;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Hide();
            frmMainMenu MM = new frmMainMenu();
            MM.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            errors = new StringBuilder();

            //Check User Name required
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errors.AppendLine("Name cannot be blank");

            }

            if (txtCaloriesB.Text == "")
            {
                errors.AppendLine("Must enter how much Calories are burned !");
            }

            if (txtExerType.Text == "")
            {
                errors.AppendLine("Must enter Exercise Type !");
            }

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Exercise ex = new Exercise();
            ex.Name = txtName.Text;
            ex.Time = DOBTime.Text;
            ex.Calories_Burn = Convert.ToDouble(txtCaloriesB.Text);
            ex.Exercise_Type = txtExerType.Text;


            try
            {
                db.Exercises.Add(ex);
                db.SaveChanges();
                txtID.Text = ex.Id.ToString();
                var dgv = db.Exercises.Select(d => new { Id = d.Id, Name = d.Name, Time = d.Time, Calories_Burn = d.Calories_Burn, Type = d.Exercise_Type }).ToList();
                dgvExe.DataSource = dgv;
                MessageBox.Show("Exercise Added Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            int id = Int32.Parse(txtID.Text);
            var data = db.Exercises.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                txtName.Text = data.Name;
                DOBTime.Value = DateTime.Parse(data.Time);
                txtCaloriesB.Text = data.Calories_Burn.ToString();
                txtExerType.Text = data.Exercise_Type;

            }
            else
            {
                txtName.Text = "";
                txtCaloriesB.Text = "";
                txtExerType.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            int id = Int32.Parse(txtID.Text);
            var data = db.Exercises.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Name = txtName.Text;
                data.Time = DOBTime.Text;
                data.Calories_Burn = Convert.ToDouble(txtCaloriesB.Text);
                data.Exercise_Type = txtExerType.Text;

                db.SaveChanges();
                var dgv = db.Exercises.Select(d => new { Id = d.Id, Name = d.Name, Time = d.Time, Calories_Burn = d.Calories_Burn, Type = d.Exercise_Type }).ToList();
                dgvExe.DataSource = dgv;
                MessageBox.Show("Exercise Updated Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                txtName.Text = "";
                txtCaloriesB.Text = "";
                txtExerType.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Exercises.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Exercises.Remove(data);

                    db.SaveChanges();

                    txtName.Text = "";
                    DOBTime.Text = "";
                    txtCaloriesB.Text = "";
                    txtExerType.Text = "";
                    var dgv = db.Exercises.Select(d => new { Id = d.Id, Name = d.Name, Time = d.Time, Calories_Burn = d.Calories_Burn, Type = d.Exercise_Type }).ToList();
                    dgvExe.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtName.Text = "";
                DOBTime.Text = "";
                txtCaloriesB.Text = "";
                txtExerType.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
