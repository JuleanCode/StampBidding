using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class Auction
    {
        public Auction()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatorId { get; set; } = null!;
        public int StatusId { get; set; }

        public virtual User Creator { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; }
    }
}
