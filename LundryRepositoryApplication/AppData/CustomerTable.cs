using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LundryRepositoryApplication.AppData
{
    internal class CustomerTable
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<ItemTable> ItemList { get; set; } = new List<ItemTable>();
    }
}
