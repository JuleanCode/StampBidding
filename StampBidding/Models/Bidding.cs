using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class Bidding
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public DateTime Date { get; set; }
        public string Price { get; set; } = null!;
        public string UserId { get; set; } = null!;

        public virtual Item Item { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
