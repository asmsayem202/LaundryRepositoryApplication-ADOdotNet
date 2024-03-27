using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LundryRepositoryApplication.AppData;

namespace LundryRepositoryApplication
{
    public partial class CustomerEntry : Form
    {
        Repository repository = new Repository();
        public int CustomerID { get; set; } = 0;
        public CustomerEntry()
        {
            InitializeComponent();
        }

        void ResetForm()
        {
            txtId.Text = null;
            txtName.Text = null;
            txtAddress.Text = null;
            txtPhone.Text = null;

            itemTableBindingSource.DataSource = null;
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                CustomerTable customer = new CustomerTable();

                if (txtId.Text.Length > 0)
                    customer.CustomerID = Convert.ToInt32(txtId.Text);

                customer.Phone = txtPhone.Text;
                customer.Name = txtName.Text;
                customer.Address = txtAddress.Text;





                foreach (DataGridViewRow item in gridItem.Rows)
                {

                    if (item.IsNewRow) continue;

                    ItemTable itemDetails = new ItemTable();

                    itemDetails.ItemName = item.Cells[0].Value.ToString();
                    itemDetails.Price = Convert.ToDecimal(item.Cells[1].Value);
                    itemDetails.Qty = Convert.ToUInt32(item.Cells[2].Value);
                    customer.ItemList.Add(itemDetails);
                }

                if (txtId.Text.Length > 0)
                {

                    int rw = repository.UpdateCustomer(customer);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data updated successfully");
                    }
                }
                else
                {
                    int rw = repository.SaveCustomer(customer);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data saved successfully");
                    }
                }


                ResetForm();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnDetele_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Length > 0)
            {

                var dialog = MessageBox.Show("Delete record", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dialog == DialogResult.OK)
                {
                    int rw = repository.DeleteCustomer(txtId.Text);


                    if (rw > 0)
                    {
                        MessageBox.Show("Data deleted successfully");
                        ResetForm();
                    }
                }
            }
        }

        private void CustomerEntry_Load(object sender, EventArgs e)
        {
            if(CustomerID > 0)
            {
                var customer = repository.GetCustomer(CustomerID);

                txtId.Text = customer.CustomerID.ToString();
                txtName.Text = customer.Name;
                txtAddress.Text = customer.Address;
                txtPhone.Text = customer.Phone;

                itemTableBindingSource.DataSource = customer.ItemList;


            }
        }
    }
}
