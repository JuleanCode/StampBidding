using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class Status
    {
        public Status()
        {
            Auctions = new HashSet<Auction>();
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string StatusText { get; set; } = null!;

        public virtual ICollection<Auction> Auctions { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
