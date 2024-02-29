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
    public partial class frmAnimals : Form
    {
        AnimalWeightTrackerEntities3 db;
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmAnimals()
        {
            InitializeComponent();
        }

        private void frmAnimals_Load(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            var dgv = db.Animals.Select(d => new { Id = d.Animal_Id, Name = d.Name, Age = d.Age, Gender = d.Gender, Weight = d.Weight, Height = d.Height, Species = d.Species.Name, Organization_Name = d.Organization.Name }).ToList();
            dgvAnimal.DataSource = dgv;
            var data = db.Organizations.ToList();
            var spe = db.Species.ToList();
            cboSpecies.DataSource = spe;
            cboSpecies.DisplayMember = "Name";
            cboSpecies.ValueMember = "Species_Id";
            cboOrg.DataSource = data;
            cboOrg.DisplayMember = "Name";
            cboOrg.ValueMember = "Organization_Id";
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
            //Age Validation
            if (txtAge.Text == "")
            {
                errors.AppendLine("Age cannot be blank");
            }

            //Gender Validation
            if (cboGend.Text == "")
            {
                errors.AppendLine("Gender cannot be blank");
            }

            if (txtWeight.Text == "")
            {
                errors.AppendLine("Weight cannot be blank");
            }

            if (txtHeight.Text == "")
            {
                errors.AppendLine("Height cannot be blank");
            }

            //Show error message
            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Animal an = new Animal();
            an.Name = txtName.Text;
            an.Age = txtAge.Text;
            an.Gender = cboGend.Text;
            an.Weight = Int32.Parse(txtWeight.Text.ToString());
            an.Height = Int32.Parse(txtHeight.Text.ToString());
            an.Species_Id = Int32.Parse(cboSpecies.SelectedValue.ToString());
            an.Organization_Id = Int32.Parse(cboOrg.SelectedValue.ToString());

            try
            {
                db.Animals.Add(an);
                db.SaveChanges();
                txtID.Text = an.Animal_Id.ToString();
                var dgv = db.Animals.Select(d => new { Id = d.Animal_Id, Name = d.Name, Age = d.Age, Gender = d.Gender, Weight = d.Weight, Height = d.Height, Species = d.Species.Name, Organization_Name = d.Organization.Name }).ToList();
                dgvAnimal.DataSource = dgv;
                MessageBox.Show("Animal Added Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Animals.Where(d => d.Animal_Id == id).FirstOrDefault();

            if (data != null)
            {
                txtName.Text = data.Name;
                txtAge.Text = data.Age;
                cboGend.Text = data.Gender;
                cboSpecies.SelectedValue = data.Species_Id;
                txtWeight.Text = data.Weight.ToString();
                txtHeight.Text = data.Height.ToString();
                cboOrg.SelectedValue = data.Organization_Id;
                
            }
            else
            {
                txtName.Text = "";
                txtAge.Text = "";
                txtWeight.Text = "";
                txtHeight.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Animals.Where(d => d.Animal_Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Name = txtName.Text;
                data.Age = txtAge.Text;
                data.Gender = cboGend.Text;
                data.Weight = Int32.Parse(txtWeight.Text.ToString());
                data.Height = Int32.Parse(txtHeight.Text.ToString());
                data.Species_Id = Int32.Parse(cboSpecies.SelectedValue.ToString());
                data.Organization_Id = Int32.Parse(cboOrg.SelectedValue.ToString());
                

                db.SaveChanges();
                var dgv = db.Animals.Select(d => new { Id = d.Animal_Id, Name = d.Name, Age = d.Age, Gender = d.Gender, Weight = d.Weight, Height = d.Height, Species = d.Species.Name, Organization_Name = d.Organization.Name }).ToList();
                dgvAnimal.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                txtName.Text = "";
                txtAge.Text = "";
                cboGend.Text = "";
                cboSpecies.Text = "";
                txtWeight.Text = "";
                txtHeight.Text = "";
                cboOrg.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Animals.Where(d => d.Animal_Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    
                    db.Animals.Remove(data);

                    db.SaveChanges();

                    txtName.Text = "";
                    txtAge.Text = "";
                    txtWeight.Text = "";
                    txtHeight.Text = "";
                    var dgv = db.Animals.Select(d => new { Id = d.Animal_Id, Name = d.Name, Age = d.Age, Gender = d.Gender, Weight = d.Weight, Height = d.Height, Species = d.Species.Name, Organization_Name = d.Organization.Name }).ToList();
                    dgvAnimal.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtName.Text = "";
                txtAge.Text = "";
                txtWeight.Text = "";
                txtHeight.Text = "";
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
