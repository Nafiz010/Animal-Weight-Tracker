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
    public partial class frmMeal : Form
    {
        AnimalWeightTrackerEntities3 db;
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmMeal()
        {
            InitializeComponent();
        }

        private void frmMeal_Load(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            var dgv = db.Meals.Select(d => new { Id = d.Id, Name = d.Name, Units = d.Unit_In_Gram,Calories = d.Calories }).ToList();
            dgvMeal.DataSource = dgv;
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

            if (txtUnit.Text == "")
            {
                errors.AppendLine("Units cannot be blank");
            }

            if (txtCalories.Text == "")
            {
                errors.AppendLine("Calories cannot be blank");
            }

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Meal ml = new Meal();
            ml.Name = txtName.Text;
            ml.Unit_In_Gram = txtUnit.Text;
            ml.Calories = double.Parse(txtCalories.Text);

                       

            try
            {
                db.Meals.Add(ml);
                db.SaveChanges();
                txtID.Text = ml.Id.ToString();
                var dgv = db.Meals.Select(d => new { Id = d.Id, Name = d.Name, Units = d.Unit_In_Gram, Calories = d.Calories }).ToList();
                dgvMeal.DataSource = dgv;
                MessageBox.Show("Meal Added Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Meals.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                txtName.Text = data.Name;
                txtUnit.Text = data.Unit_In_Gram;
                txtCalories.Text = data.Calories.ToString();             

            }
            else
            {
                txtName.Text = "";
                txtUnit.Text = "";
                txtCalories.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Meals.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Name = txtName.Text;
                data.Unit_In_Gram = txtUnit.Text;
                data.Calories = double.Parse(txtCalories.Text.ToString());               


                db.SaveChanges();
                var dgv = db.Meals.Select(d => new { Id = d.Id, Name = d.Name, Units = d.Unit_In_Gram, Calories = d.Calories }).ToList();
                dgvMeal.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                txtName.Text = "";
                txtUnit.Text = "";
                txtCalories.Text = "";                
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Meals.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Meals.Remove(data);

                    db.SaveChanges();

                    txtName.Text = "";
                    txtUnit.Text = "";
                    txtCalories.Text = "";
                    var dgv = db.Meals.Select(d => new { Id = d.Id, Name = d.Name, Units = d.Unit_In_Gram, Calories = d.Calories }).ToList();
                    dgvMeal.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtName.Text = "";
                txtUnit.Text = "";
                txtCalories.Text = "";
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
