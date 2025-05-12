using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GA2_Ex2_ASPNetMVCDBFirst.Models
{
    public class FilterViewModel
    {
        [Display(Name = "Filter Type")]
        public string FilterType { get; set; }

        [Display(Name = "Agent")]
        public int? AgentID { get; set; }

        public List<BestItem> BestItems { get; set; }
        public List<AgentItem> ItemsByAgent { get; set; }
        public List<AgentOrder> AgentPurchases { get; set; }
    }

    public class BestItem
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class AgentItem
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }

    public class AgentOrder
    {
        public int OrderID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public List<AgentItem> Items { get; set; }
    }
}