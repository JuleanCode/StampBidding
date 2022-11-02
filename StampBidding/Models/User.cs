using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class User
    {
        public User()
        {
            Auctions = new HashSet<Auction>();
            Biddings = new HashSet<Bidding>();
            ItemBuyers = new HashSet<Item>();
            ItemSellers = new HashSet<Item>();
            Receipts = new HashSet<Receipt>();
        }

        public string Uuid { get; set; } = null!;
        public int RoleId { get; set; }
        public int MemberId { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Provence { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Street { get; set; } = null!;
        public int Housenumber { get; set; }
        public string AddressSuffix { get; set; } = null!;
        public string PhoneNumber { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Auction> Auctions { get; set; }
        public virtual ICollection<Bidding> Biddings { get; set; }
        public virtual ICollection<Item> ItemBuyers { get; set; }
        public virtual ICollection<Item> ItemSellers { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
