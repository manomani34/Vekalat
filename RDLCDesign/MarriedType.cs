using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace RDLCDesign
{
    public partial class MarriedType
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
