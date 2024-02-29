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
    public partial class frmMainMenu : Form
    {
        bool mouseDown;
        private Point offset;
        public frmMainMenu()
        {
            InitializeComponent();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff st = new frmStaff();
            this.Hide();
            st.ShowDialog();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();            
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

        private void btnAnimal_Click(object sender, EventArgs e)
        {
            frmAnimals an = new frmAnimals();
            this.Hide();
            an.ShowDialog();
        }

        private void btnSpecies_Click(object sender, EventArgs e)
        {
            frmSpecies sp = new frmSpecies();
            this.Hide();
            sp.ShowDialog();
        }

        private void btnOrg_Click(object sender, EventArgs e)
        {
            frmOrganization org = new frmOrganization();
            this.Hide();
            org.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMeal ml = new frmMeal();
            this.Hide();
            ml.ShowDialog();
        }

        private void btnExer_Click(object sender, EventArgs e)
        {
            frmExercise ex = new frmExercise();
            this.Hide();
            ex.ShowDialog();
        }

        private void btnCourse_Click(object sender, EventArgs e)
        {
            FrmCourse cs = new FrmCourse();
            this.Hide();
            cs.ShowDialog();
        }

        private void btnDLog_Click(object sender, EventArgs e)
        {
            frmDLog DL = new frmDLog();
            this.Hide();
            DL.ShowDialog();
        }

        private void btnDExer_Click(object sender, EventArgs e)
        {
            frmDExer DEx = new frmDExer();
            this.Hide();
            DEx.ShowDialog();
        }

        private void btnDMeal_Click(object sender, EventArgs e)
        {
            frmDMeal DM = new frmDMeal();
            this.Hide();
            DM.ShowDialog();
        }

        private void btnDMeas_Click(object sender, EventArgs e)
        {
            frmDMeasure Dme = new frmDMeasure();
            this.Hide();
            Dme.ShowDialog();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            frmReport rp = new frmReport();
            this.Hide();
            rp.ShowDialog();
        }
    }
}
