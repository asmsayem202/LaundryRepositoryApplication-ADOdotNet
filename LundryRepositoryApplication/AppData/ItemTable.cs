using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LundryRepositoryApplication.AppData
{
    internal class ItemTable
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public uint Qty { get; set; }
        public decimal Total => Price * Qty;
    }
}
