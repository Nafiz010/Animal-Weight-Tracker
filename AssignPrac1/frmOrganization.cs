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
    public partial class frmOrganization : Form
    {
        AnimalWeightTrackerEntities3 db;
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmOrganization()
        {
            InitializeComponent();
        }

        private void frmOrganization_Load(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            var dgv = db.Organizations.Select(d => new { Id = d.Organization_Id, Name = d.Name, Address = d.Address, Organization_Type = d.OrganizationType.Type }).ToList();
            dgvOrg.DataSource = dgv;
            var data = db.OrganizationTypes.ToList();
            cboType.DataSource = data;
            cboType.DisplayMember = "Type";
            cboType.ValueMember = "Id";
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
            
            if (txtAdd.Text == "")
            {
                errors.AppendLine("Address cannot be blank");
            }

            if (cboType.Text == "")
            {
                errors.AppendLine("Organization Type cannot be blank");
            }

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Organization oz = new Organization();
            oz.Name = txtName.Text;
            oz.Address = txtAdd.Text;
            oz.Type_Id = Int32.Parse(cboType.SelectedValue.ToString());

            try
            {
                db.Organizations.Add(oz);
                db.SaveChanges();
                txtID.Text = oz.Organization_Id.ToString();
                var dgv = db.Organizations.Select(d => new { Id = d.Organization_Id, Name = d.Name, Address = d.Address, Organization_Type = d.OrganizationType.Type }).ToList();
                dgvOrg.DataSource = dgv;
                MessageBox.Show("Organization Added Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Organizations.Where(d => d.Organization_Id == id).FirstOrDefault();

            if (data != null)
            {
                txtName.Text = data.Name;
                txtAdd.Text = data.Address;
                cboType.SelectedValue = data.Type_Id;

            }
            else
            {
                txtName.Text = "";
                txtAdd.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Organizations.Where(d => d.Organization_Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Name = txtName.Text;
                data.Address = txtAdd.Text;
                data.Type_Id = Int32.Parse(cboType.SelectedValue.ToString());


                db.SaveChanges();
                var dgv = db.Organizations.Select(d => new { Id = d.Organization_Id, Name = d.Name, Address = d.Address, Organization_Type = d.OrganizationType.Type }).ToList();
                dgvOrg.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                txtName.Text = "";
                txtAdd.Text = "";
                cboType.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Organizations.Where(d => d.Organization_Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Organizations.Remove(data);

                    db.SaveChanges();

                    txtName.Text = "";
                    txtAdd.Text = "";
                    var dgv = db.Organizations.Select(d => new { Id = d.Organization_Id, Name = d.Name, Address = d.Address, Organization_Type = d.OrganizationType.Type }).ToList();
                    dgvOrg.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtName.Text = "";
                txtAdd.Text = "";
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
