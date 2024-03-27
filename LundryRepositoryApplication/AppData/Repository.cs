using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LundryRepositoryApplication.AppData
{
    internal class Repository
    {
        string conString = $"server = (localdb)\\mssqllocaldb; AttachDbFilename = {Application.StartupPath}\\AppData\\ProjectDB.mdf; Trusted_Connection = true;";


        public List<CustomerTable> GetCustomers()
        {


            List<CustomerTable> customers = new List<CustomerTable>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                cmd.CommandText = "select * from Customer";

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                con.Open();
                sda.Fill(ds);


                if (ds.Tables.Count > 0)
                {



                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CustomerTable customer = new CustomerTable();
                        customer.CustomerID = Convert.ToInt32(dr["CustomerID"]);
                        customer.Name = dr["Name"].ToString();
                        customer.Phone = dr["Phone"].ToString();
                        customer.Address = dr["Address"]?.ToString();
                        customers.Add(customer);
                    }

                }

            }

            return customers;
        }
        public CustomerTable GetCustomer(int id)
        {


            CustomerTable customer = new CustomerTable();

            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                cmd.CommandText = $"select * from Customer where CustomerID = {id}; select * from item where CustomerID = {id} ";

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                con.Open();
                sda.Fill(ds);


                if (ds.Tables.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    customer.CustomerID = Convert.ToInt32(row["CustomerID"]);
                    customer.Name = row["Name"].ToString();
                    customer.Phone = row["Phone"].ToString();
                    customer.Address = row["Address"]?.ToString();

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        ItemTable item = new ItemTable();
                       
                        item.ItemName = dr["ItemName"].ToString();
                        item.Price =Convert.ToDecimal(dr["Price"]);
                        item.Qty =Convert.ToUInt32(dr["Qty"]);


                        customer.ItemList.Add(item);
                    }

                }

            }

            return customer;
        }
        public int SaveCustomer(CustomerTable customer)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;



                try
                {


                    cmd.CommandText = "select isnull(max(CustomerID), 0) + 1 as CustomerID from Customer";


                    string CustomerID = cmd.ExecuteScalar()?.ToString();



                    cmd.CommandText = $"INSERT INTO [dbo].[Customer]([CustomerID],[Name],[Address],[Phone]) VALUES (  {CustomerID}, '{customer.Name}', '{customer.Address}', '{customer.Phone}'   )";


                    rowNo = cmd.ExecuteNonQuery();


                    if (rowNo > 0)
                    {

                        foreach (var item in customer.ItemList)
                        {
                            cmd.CommandText = $"INSERT INTO [dbo].[Item] ([CustomerID] ,[ItemName] ,[Price] ,[Qty])  VALUES ({CustomerID} ,'{item.ItemName}' , '{item.Price}' , '{item.Qty}')";


                            int r1 = cmd.ExecuteNonQuery();
                        }

                    }

                    tran.Commit();
                }
                catch (SqlException e)
                {

                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }


        public int UpdateCustomer(CustomerTable customer)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;



                try
                {




                    cmd.CommandText = $"UPDATE [dbo].[Customer]   SET [Name] = '{customer.Name}',[Address] = '{customer.Address}',[Phone] = '{customer.Phone}' where CustomerID = {customer.CustomerID}";

                    rowNo = cmd.ExecuteNonQuery();


                    if (rowNo > 0)
                    {
                        cmd.CommandText = $"delete from [dbo].[Item] where CustomerID = {customer.CustomerID}";


                        if (cmd.ExecuteNonQuery() >= 0)
                        {
                            foreach (var item in customer.ItemList)
                            {
                                cmd.CommandText = $"INSERT INTO [dbo].[Item] ([CustomerID] ,[ItemName] ,[Price] ,[Qty])  VALUES ({customer.CustomerID} ,'{item.ItemName}' , '{item.Price}' , '{item.Qty}')";


                                cmd.ExecuteNonQuery();
                            }
                        }



                    }

                    tran.Commit();
                }
                catch (SqlException e)
                {

                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }

        public int DeleteCustomer(string CustomerID)
        {
            int rowNo = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cmd = con.CreateCommand();

                cmd.Transaction = tran;




                try
                {
                    cmd.CommandText = $"delete from [dbo].[Customer]   where CustomerID = {CustomerID}";

                    rowNo = cmd.ExecuteNonQuery();

                    tran.Commit();

                }
                catch (SqlException e)
                {
                    tran.Rollback();
                    MessageBox.Show(e.Message);
                    return 0;
                }
            }
            return rowNo;
        }

        internal List<VwCusItemTable> GetReportData()
        {
            List<VwCusItemTable> items = new List<VwCusItemTable>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = con.CreateCommand();


                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from vwCusItem";



                DataTable dt = new DataTable();
                con.Open();



                dt.Load(cmd.ExecuteReader());




                foreach (DataRow dr in dt.Rows)
                {
                    VwCusItemTable vwCusItem = new VwCusItemTable();
                    vwCusItem.CustomerID = Convert.ToInt32(dr["CustomerID"]);
                    vwCusItem.Name = dr["Name"].ToString();
                    vwCusItem.Phone = dr["Phone"].ToString();
                    vwCusItem.Address = dr["Address"]?.ToString();
                    vwCusItem.ItemName = dr["ItemName"]?.ToString();
                    vwCusItem.Price = Convert.ToDecimal(dr["Price"]);
                    vwCusItem.Qty = Convert.ToUInt32(dr["Qty"]);
                    vwCusItem.Total = Convert.ToDecimal(dr["Total"]);






                    items.Add(vwCusItem);
                }



            }

            return items;
        }


    }
}
