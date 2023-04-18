using Vekalat.Core.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class Order : Entity
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User UserFk { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderTypeid { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }
        public double SubTotal { get; set; }
        public double? Discount { get; set; }
        public double Total { get; set; }
        public int? SendCost { get; set; }
        public int? SendType { get; set; }
        [MaxLength(50)]
        public string Authority { get; set; }
        [MaxLength(100)]
        public string FishNo { get; set; }
        public long FishNoCost { get; set; }
        [MaxLength(500)]
        public string FishNotes { get; set; }
        public DateTime FishDate { get; set; }
        public int? AccountNameId { get; set; }
        [ForeignKey("AccountNameId")]
        public AccountName AccountName { get; set; }

        [MaxLength(50)]
        public string DiscuntCode { get; set; }
    }
}
