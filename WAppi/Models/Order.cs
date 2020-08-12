using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace WAppi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ProductName {get;set;}
        public int Amount { get; set; }
        public int OrderNumber { get; set; }
        public string ClientEmail { get; set; }
        public string ClientName { get; set; }
        public string ClientLastName { get; set; }
        public decimal SummaryPrice { get; set; } = 0;
        public OrderStatuses Status { get; set; } = OrderStatuses.New;

        public void SetAsPaid()
        {
            Status = OrderStatuses.Paid;
        }
        public enum OrderStatuses
        {
            New = 0,
            Paid = 1
        }
    }
}
