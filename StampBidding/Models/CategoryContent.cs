using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class CategoryContent
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Text { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
    }
}
