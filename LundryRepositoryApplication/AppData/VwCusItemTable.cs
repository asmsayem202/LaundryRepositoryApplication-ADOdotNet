using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LundryRepositoryApplication.AppData
{
    internal class VwCusItemTable
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public uint Qty { get; set; }
        public decimal Total { get; set; }
    }
}
