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
    public partial class frmSpecies : Form
    {
        AnimalWeightTrackerEntities3 db = new AnimalWeightTrackerEntities3();
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmSpecies()
        {
            InitializeComponent();
        }

        private void frmSpecies_Load(object sender, EventArgs e)
        {
            db = new AnimalWeightTrackerEntities3();
            var dgv = db.Species.Select(d => new { Id = d.Species_Id, Name = d.Name, Description = d.Description }).ToList();
            dgvSpecies.DataSource = dgv;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errors = new StringBuilder();

            //Check User Name required
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errors.AppendLine("Name cannot be blank");

            }
            //Age Validation
            if (txtDesc.Text == "")
            {
                errors.AppendLine("Description cannot be blank");
            }

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Species sp = new Species();
            sp.Name = txtName.Text;
            sp.Description = txtDesc.Text;

            db.Species.Add(sp);
            db.SaveChanges();
            txtID.Text = sp.Species_Id.ToString();
            var dgv = db.Species.Select(d => new { Id = d.Species_Id, Name = d.Name, Description = d.Description }).ToList();
            dgvSpecies.DataSource = dgv;
            MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            var data = db.Species.Where(d => d.Species_Id == id).FirstOrDefault();

            if (data != null)
            {
                txtName.Text = data.Name;
                txtDesc.Text = data.Description;
            }
            else
            {
                txtName.Text = "";
                txtDesc.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Species.Where(d => d.Species_Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Name = txtName.Text;
                data.Description = txtDesc.Text;


                db.SaveChanges();
                var dgv = db.Species.Select(d => new { Id = d.Species_Id, Name = d.Name, Description = d.Description }).ToList();
                dgvSpecies.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                txtName.Text = "";
                txtDesc.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Species.Where(d => d.Species_Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Species.Remove(data);

                    db.SaveChanges();

                    txtName.Text = "";
                    txtDesc.Text = "";
                    var dgv = db.Species.Select(d => new { Id = d.Species_Id, Name = d.Name, Description = d.Description }).ToList();
                    dgvSpecies.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtName.Text = "";
                txtDesc.Text = "";
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
