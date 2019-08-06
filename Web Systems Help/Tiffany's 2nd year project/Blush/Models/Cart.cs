namespace Blush.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        public int Id { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }

        public DateTime? Date { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
