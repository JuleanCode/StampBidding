using System;
using System.Collections.Generic;

namespace StampBidding.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryContents = new HashSet<CategoryContent>();
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<CategoryContent> CategoryContents { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
