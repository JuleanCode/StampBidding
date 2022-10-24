using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class Item
    {
        public Item()
        {
            Biddings = new HashSet<Bidding>();
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string SellerId { get; set; } = null!;
        public int AuctionId { get; set; }
        public int StatusId { get; set; }
        public string BuyerId { get; set; } = null!;
        public int ReceiptId { get; set; }
        public int MinPrice { get; set; }

        public virtual Auction Auction { get; set; } = null!;
        public virtual User Buyer { get; set; } = null!;
        public virtual Receipt Receipt { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Bidding> Biddings { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
