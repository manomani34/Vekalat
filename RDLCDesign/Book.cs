using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDLCDesign
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string DateOfKetabat { get; set; }
        public string VolumeName { get; set; }
        public Int16 StartPage { get; set; }
        public Int16 EndPage { get; set; }
        public string BookListTitle { get; set; }
        public string AccessCode { get; set; }
    }
}
