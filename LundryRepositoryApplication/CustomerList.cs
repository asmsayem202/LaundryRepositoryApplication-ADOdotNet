using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using LundryRepositoryApplication.AppData;

namespace LundryRepositoryApplication
{
    public partial class CustomerList : Form
    {
        Repository repository = new Repository();
        public CustomerList()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();


            if (int.Parse(id) > 0)
            {
                CustomerEntry form = new CustomerEntry();

                form.CustomerID = int.Parse(id);


                form.ShowDialog(this);
            }
        }


        void DataLoad()
        {

            this.customerTableBindingSource.DataSource = repository.GetCustomers();

        }

        private void btnData_Click(object sender, EventArgs e)
        {

        }

        private void CustomerList_Load(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {

            ReportDocument report = new ReportDocument();


            report.Load($"{Application.StartupPath}\\Reports\\CustomerReport.rpt");

            if (report.IsLoaded)
            {


                report.SetDataSource(repository.GetReportData());

            }



            ReportViewerForm form = new ReportViewerForm();

            form.crystalReportViewer1.ReportSource = report;



            form.ShowDialog(this);
        }
    }
}
