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
    
    public partial class FrmCourse : Form
    {
        AnimalWeightTrackerEntities3 db;
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public FrmCourse()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            errors = new StringBuilder();

            if (cboGoal.Text == "")
            {
                errors.AppendLine("Goal cannot be null !!");
            }
            

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Course cs = new Course();
            cs.Start_Date = DOBStart.Value;
            cs.End_Date = DOBEnd.Value;
            cs.Animal_Id = Int32.Parse(CBOAnimal.SelectedValue.ToString());
            cs.Staff_id = Int32.Parse(CBOStaff.SelectedValue.ToString());
            cs.Goal = cboGoal.Text;

            var data = db.Animals.Where(d => d.Animal_Id == cs.Animal_Id).FirstOrDefault();
            var w = data.Weight;
            var h = data.Height;
            txtBMI.Text = (w / (h * h)).ToString();
            cs.BMI = Decimal.Parse(txtBMI.Text);


            try
            {
                db.Courses.Add(cs);
                db.SaveChanges();
                txtID.Text = cs.Id.ToString();
                var dgv = db.Courses.Select(d => new { Id = d.Id, Start_Date = d.Start_Date, End_Date = d.End_Date, Animal = d.Animal.Name, Staff = d.Staff.Name, Goal = d.Goal, BMI = d.BMI }).ToList();
                dgvCour.DataSource = dgv;
                MessageBox.Show("Course Added Successfully !!!!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FrmCourse_Load(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            var dgv = db.Courses.Select(d => new { Id = d.Id, Start_Date = d.Start_Date, End_Date = d.End_Date, Animal = d.Animal.Name, Staff = d.Staff.Name, Goal = d.Goal, BMI = d.BMI }).ToList();
            dgvCour.DataSource = dgv;
            var ani = db.Animals.ToList();
            var sts = db.Staffs.ToList();
            CBOAnimal.DataSource = ani;
            CBOAnimal.DisplayMember = "Name";
            CBOAnimal.ValueMember = "Animal_Id";
            CBOStaff.DataSource = sts;
            CBOStaff.DisplayMember = "Name";
            CBOStaff.ValueMember = "Staff_Id";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Hide();
            frmMainMenu MM = new frmMainMenu();
            MM.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Courses.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                DOBStart.Value = data.Start_Date.Value;
                DOBEnd.Value = data.End_Date.Value;
                CBOAnimal.SelectedValue = data.Animal_Id;
                CBOStaff.SelectedValue = data.Staff_id;
                cboGoal.Text = data.Goal;
                txtBMI.Text = data.BMI.ToString();

            }
            else
            {
                
                txtBMI.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Courses.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                Course cs = new Course();
                data.Start_Date = DOBStart.Value;
                data.End_Date = DOBEnd.Value;
                data.Animal_Id = Int32.Parse(CBOAnimal.SelectedValue.ToString());
                data.Staff_id = Int32.Parse(CBOAnimal.SelectedValue.ToString());
                data.Goal = cboGoal.Text;
                data.BMI = Decimal.Parse(txtBMI.Text);

                db.SaveChanges();
                var dgv = db.Courses.Select(d => new { Id = d.Id, Start_Date = d.Start_Date, End_Date = d.End_Date, Animal = d.Animal.Name, Staff = d.Staff.Name, Goal = d.Goal, BMI = d.BMI }).ToList();
                dgvCour.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                txtBMI.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Courses.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Courses.Remove(data);

                    db.SaveChanges();

                    DOBStart.Text = "";
                    DOBEnd.Text = "";
                    CBOAnimal.Text = "";
                    CBOStaff.Text = "";
                    cboGoal.Text = "";
                    txtBMI.Text = "";
                    var dgv = db.Courses.Select(d => new { Id = d.Id, Start_Date = d.Start_Date, End_Date = d.End_Date, Animal = d.Animal.Name, Staff = d.Staff.Name, Goal = d.Goal, BMI = d.BMI }).ToList();
                    dgvCour.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                DOBStart.Text = "";
                DOBEnd.Text = "";
                CBOAnimal.Text = "";
                CBOStaff.Text = "";
                cboGoal.Text = "";
                txtBMI.Text = "";
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
