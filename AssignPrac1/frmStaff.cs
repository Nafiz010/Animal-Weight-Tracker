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
    public partial class frmStaff : Form
    {
        AnimalWeightTrackerEntities3 db;
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmStaff()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Hide();
            frmMainMenu MM = new frmMainMenu();
            MM.ShowDialog();
        }

        private void frmStaff_Load(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            var dgv = db.Staffs.Select(d => new { Id = d.Staff_Id, Name = d.Name, Age = d.Age, Designation = d.Designation, Organization_Name = d.Organization.Name }).ToList();
            dgvStaff.DataSource = dgv;
            var data = db.Organizations.ToList();
            cboOrg.DataSource = data;
            cboOrg.DisplayMember = "Name";
            cboOrg.ValueMember = "Organization_Id";

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

            //Designation Validation
            if (cboDeg.Text == "")
            {
                errors.AppendLine("Designation cannot be blank");
            }


            //Show error message
            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Staff st = new Staff();
            st.Name = txtName.Text;
            st.Age = txtAge.Text;
            st.Designation = cboDeg.Text;
            st.Org_Id = Int32.Parse(cboOrg.SelectedValue.ToString());

            try
            {
                db.Staffs.Add(st);
                db.SaveChanges();
                txtID.Text = st.Staff_Id.ToString();
                var dgv = db.Staffs.Select(d => new { Id = d.Staff_Id, Name = d.Name, Age = d.Age, Designation = d.Designation, Organization_Name = d.Organization.Name }).ToList();
                dgvStaff.DataSource = dgv;
                MessageBox.Show("Data Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Staffs.Where(d => d.Staff_Id == id).FirstOrDefault();

            if (data != null)
            {
                txtName.Text = data.Name;
                txtAge.Text = data.Age;
                cboDeg.Text = data.Designation;
                cboOrg.SelectedValue = data.Org_Id;

            }
            else
            {
                txtName.Text = "";
                txtAge.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Staffs.Where(d => d.Staff_Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Name = txtName.Text;
                data.Age = txtAge.Text;
                data.Designation = cboDeg.Text;
                data.Org_Id = Int32.Parse(cboOrg.SelectedValue.ToString());


                db.SaveChanges();
                var dgv = db.Staffs.Select(d => new { Id = d.Staff_Id, Name = d.Name, Age = d.Age, Designation = d.Designation, Organization_Name = d.Organization.Name }).ToList();
                dgvStaff.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                txtName.Text = "";
                txtAge.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Staffs.Where(d => d.Staff_Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Staffs.Remove(data);

                    db.SaveChanges();

                    txtName.Text = "";
                    txtAge.Text = "";
                    
                    var dgv = db.Staffs.Select(d => new { Id = d.Staff_Id, Name = d.Name, Age = d.Age, Designation = d.Designation, Organization_Name = d.Organization.Name }).ToList();
                    dgvStaff.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtName.Text = "";
                txtAge.Text = "";
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
