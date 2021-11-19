using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMsAccess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'appData.Employees' table. You can move, or remove it, as needed.
            this.employeesTableAdapter.Fill(this.appData.Employees);
            employeesBindingSource.DataSource = this.appData.Employees;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                employeesBindingSource.EndEdit();
                employeesTableAdapter.Update(this.appData.Employees);
                panel1.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK,MessageBoxIcon.Error);
                employeesBindingSource.ResetBindings(false);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            employeesBindingSource.ResetBindings(false);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            TxtFullName.Focus();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = true;
                TxtFullName.Focus();
                this.appData.Employees.AddEmployeesRow(this.appData.Employees.NewEmployeesRow());
                employeesBindingSource.MoveLast();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                employeesBindingSource.ResetBindings(false);
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        PctView.Image = Image.FromFile(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {

                if (MessageBox.Show("Are you sure want to delete this record ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    employeesBindingSource.RemoveCurrent();
            }
        }

        private void TxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (string.IsNullOrEmpty(TxtSearch.Text))
                {
                    this.employeesTableAdapter.Fill(this.appData.Employees);
                    employeesBindingSource.DataSource = this.appData.Employees;
                    //DgvView.DataSource = employeesBindingSource;
                }
                else
                {
                    var query = from o in this.appData.Employees
                                where o.FullName.Contains(TxtSearch.Text) || o.PhoneNumber == TxtSearch.Text || o.Email == TxtSearch.Text || o.Address.Contains(TxtSearch.Text)
                                select o;
                    employeesBindingSource.DataSource = query.ToList();
                    //DgvView.DataSource = query.ToList();
                }
            }
        }
    }
}
