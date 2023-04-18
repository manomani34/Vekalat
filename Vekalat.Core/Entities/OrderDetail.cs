using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vekalat.Core.Entities;
using Vekalat.Core.Common;

namespace Vekalat.Core.Entities
{
    public  class OrderDetail : Entity
    {
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
        public int EquipmentId { get; set; }
        [ForeignKey("EquipmentId")] 
        public Equipment Equipment{ get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public int Radif { get; set; }

    }
}
