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
    public partial class frmDMeal : Form
    {
        AnimalWeightTrackerEntities3 db = new AnimalWeightTrackerEntities3();
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmDMeal()
        {
            InitializeComponent();
        }

        private void frmDMeal_Load(object sender, EventArgs e)
        {
            var dgv = db.Daily_Meal.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Meal = d.Meal.Name, Meal_Intake = d.Meal_Intake }).ToList();
            dgvDMeal.DataSource = dgv;
            var dl = db.Daily_Log.ToList();
            var ml = db.Meals.ToList();
            cboDLog.DataSource = dl;
            cboDLog.DisplayMember = "Log_Id";
            cboDLog.ValueMember = "Log_Id";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errors = new StringBuilder();

            if (cboDLog.Text == "")
            {
                errors.AppendLine("Daily Log cannot be blank");
            }

            if (cboMeal.Text == "")
            {
                errors.AppendLine("Meal cannot be blank");
            }

            if (txtMIntake.Text == "")
            {
                errors.AppendLine("Meal Intake cannot be blank");
            }


            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Daily_Meal dm = new Daily_Meal();
            dm.Log_Id = Int32.Parse(cboDLog.SelectedValue.ToString());
            dm.Meal_Id = Int32.Parse(cboMeal.SelectedValue.ToString());
            dm.Meal_Intake = Decimal.Parse(txtMIntake.Text);

            try
            {
                db.Daily_Meal.Add(dm);
                db.SaveChanges();
                txtID.Text = dm.Id.ToString();
                var dgv = db.Daily_Meal.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Meal = d.Meal.Name, Meal_Intake = d.Meal_Intake }).ToList();
                dgvDMeal.DataSource = dgv;
                MessageBox.Show("Daily Meal Saved Successfully !!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Meal.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                cboDLog.SelectedValue = data.Log_Id;
                cboMeal.SelectedValue = data.Meal_Id;
                txtMIntake.Text = data.Meal_Intake.ToString();

            }
            else
            {
                txtMIntake.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Meal.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            { 
                data.Log_Id = Int32.Parse(cboDLog.SelectedValue.ToString());
                data.Meal_Id = Int32.Parse(cboMeal.SelectedValue.ToString());
                data.Meal_Intake = Decimal.Parse(txtMIntake.Text);

                db.SaveChanges();
                var dgv = db.Daily_Meal.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Meal = d.Meal.Name, Meal_Intake = d.Meal_Intake }).ToList();
                dgvDMeal.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                txtMIntake.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Meal.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Daily_Meal.Remove(data);

                    db.SaveChanges();

                    cboDLog.SelectedValue = "";
                    cboMeal.SelectedValue = "";
                    txtMIntake.Text = "";
                    var dgv = db.Daily_Meal.Select(d => new { Id = d.Id, Log_ID = d.Log_Id, Meal = d.Meal.Name, Meal_Intake = d.Meal_Intake}).ToList();
                    dgvDMeal.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                cboDLog.SelectedValue = "";
                cboMeal.SelectedValue = "";
                txtMIntake.Text = "";
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Hide();
            frmMainMenu MM = new frmMainMenu();
            MM.ShowDialog();
        }
    }
}
