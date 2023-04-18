using Vekalat.Application.Common;
using Vekalat.Application.Enums;
using Vekalat.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vekalat.Core.Entities
{
    public class User : AuditedEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string Mobil { get; set; }
        //[Display(Name = "شهر")]
        //public int? cityid { get; set; }
        //[Display(Name = "استان")]
        //public int? ostanid { get; set; }
        //public string Title { get; set; }
        //public string TC { get; set; }
        //public string TaxNumber { get; set; }
        //public string Brand { get; set; }
        public bool IsActive { get; set; }
        //public string Logo { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public string Notes { get; set; }
        public List<EquipmentReservation> EquipmentReservations { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}

