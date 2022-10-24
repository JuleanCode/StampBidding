using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class Receipt
    {
        public Receipt()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string BuyerId { get; set; } = null!;

        public virtual User Buyer { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; }
    }
}
