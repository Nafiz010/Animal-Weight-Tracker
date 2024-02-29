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
    public partial class frmDMeasure : Form
    {
        AnimalWeightTrackerEntities3 db = new AnimalWeightTrackerEntities3();
        StringBuilder errors;
        bool mouseDown;
        private Point offset;
        public frmDMeasure()
        {
            InitializeComponent();
        }

        private void frmDMeasure_Load(object sender, EventArgs e)
        {
            var dgv = db.Daily_Measurement.Select(d => new { Id = d.Id, Log_Id = d.Log_Id, Waist_Size = d.Waist_Size, Weight = d.Weight, Shift = d.Shift, Date = d.DOM }).ToList();
            dgvDExer.DataSource = dgv;
            var dl = db.Daily_Log.ToList();
            cboDLog.DataSource = dl;
            cboDLog.DisplayMember = "Log_Id";
            cboDLog.ValueMember = "Log_Id";
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
            errors = new StringBuilder();

            if (cboDLog.Text == "")
            {
                errors.AppendLine("Daily Log cannot be null !!");
            }

            if (cboShift.Text == "")
            {
                errors.AppendLine("Shift Log cannot be null !!");
            }

            if (txtWaistS.Text == "")
            {
                errors.AppendLine("Waist Size cannot be null !!");
            }

            if (txtWeight.Text == "")
            {
                errors.AppendLine("Weight cannot be null !!");
            }
            

            if (errors.ToString() != string.Empty)
            {
                MessageBox.Show(errors.ToString(), "Validation failed !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Daily_Measurement dlm = new Daily_Measurement();
            dlm.Log_Id = Int32.Parse(cboDLog.SelectedValue.ToString());
            dlm.Waist_Size = Int32.Parse(txtWaistS.Text.ToString());
            dlm.Weight = Decimal.Parse(txtWeight.Text);
            dlm.Shift = cboShift.Text;
            dlm.DOM = DateEx.Value;
            

            try
            {
                db.Daily_Measurement.Add(dlm);
                db.SaveChanges();
                txtID.Text = dlm.Id.ToString();
                var dgv = db.Daily_Measurement.Select(d => new { Id = d.Id, Log_Id = d.Log_Id, Waist_Size = d.Waist_Size, Weight = d.Weight, Shift = d.Shift, Date = d.DOM }).ToList();
                dgvDExer.DataSource = dgv;
                MessageBox.Show("Daily Measurement Added Successfully !!!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Data", "Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Measurement.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                cboDLog.SelectedValue = data.Log_Id;
                txtWaistS.Text = data.Waist_Size.ToString();
                txtWeight.Text = data.Weight.ToString();
                cboShift.Text = data.Shift;
                DateEx.Value = data.DOM.Value;

            }
            else
            {
                txtWaistS.Text = "";
                txtWeight.Text = "";
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Measurement.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                data.Log_Id = Int32.Parse(cboDLog.SelectedValue.ToString());
                data.Waist_Size = Int32.Parse(txtWaistS.Text.ToString());
                data.Weight = Decimal.Parse(txtWeight.Text);
                data.Shift = cboShift.Text;
                data.DOM = DateEx.Value;


                db.SaveChanges();
                var dgv = db.Daily_Measurement.Select(d => new { Id = d.Id, Log_Id = d.Log_Id, Waist_Size = d.Waist_Size, Weight = d.Weight, Shift = d.Shift, Date = d.DOM }).ToList();
                dgvDExer.DataSource = dgv;
                MessageBox.Show("Data updated successfully", "Updated !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {                
                txtWaistS.Text = "";
                txtWeight.Text = "";                
                MessageBox.Show("Data not found", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txtID.Text);
            var data = db.Daily_Measurement.Where(d => d.Id == id).FirstOrDefault();

            if (data != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    db.Daily_Measurement.Remove(data);

                    db.SaveChanges();

                    txtWaistS.Text = "";
                    txtWeight.Text = "";
                    var dgv = db.Daily_Measurement.Select(d => new { Id = d.Id, Log_Id = d.Log_Id, Waist_Size = d.Waist_Size, Weight = d.Weight, Shift = d.Shift, Date = d.DOM }).ToList();
                    dgvDExer.DataSource = dgv;
                    MessageBox.Show("Data deleted successfully", "Deleted !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtWaistS.Text = "";
                txtWeight.Text = "";
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
