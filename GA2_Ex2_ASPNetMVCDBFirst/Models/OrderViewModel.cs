using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GA2_Ex2_ASPNetMVCDBFirst.Models
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Agent")]
        public int? AgentID { get; set; }

        [Display(Name = "User")]
        public int? UserID { get; set; } // Added to match Order model

        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
    }

    public class OrderDetailViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Item")]
        public int ItemID { get; set; }

        public string ItemName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Amount must be greater than 0")]
        public decimal UnitAmount { get; set; }
    }
}