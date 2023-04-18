namespace RDLCDesign
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string FatherName { get; set; }

        public int gender { get; set; }

        [Required]
        [StringLength(15)]
        public string MeliCode { get; set; }

        [StringLength(20)]
        public string LicenseID { get; set; }

        [StringLength(50)]
        public string BirthDate { get; set; }

        [StringLength(20)]
        public string PostCode { get; set; }

        [StringLength(10)]
        public string PoBox { get; set; }

        [StringLength(20)]
        public string Tel { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public string Note { get; set; }

        public int? AuthorRefId { get; set; }

        [StringLength(20)]
        public string mobil { get; set; }

        [StringLength(20)]
        public string PDate { get; set; }

        [StringLength(100)]
        public string website { get; set; }

        [StringLength(100)]
        public string picname { get; set; }

        public string discription { get; set; }

        public string Rezome { get; set; }

        [StringLength(100)]
        public string MeliCodepic { get; set; }

        [StringLength(200)]
        public string WorkAddress { get; set; }

        public bool isconfirm { get; set; }

        [StringLength(20)]
        public string WorkTel { get; set; }

        [StringLength(100)]
        public string StudyField { get; set; }

        public bool IsTakmil { get; set; }

        [StringLength(30)]
        public string bimehNUmber { get; set; }

        public string AdminDes { get; set; }

        public bool IsKetabSal { get; set; }

        public int Status { get; set; }

        [StringLength(11)]
        public string mobil2 { get; set; }

        [StringLength(50)]
        public string PassWord { get; set; }

        public string KetabdaryNote { get; set; }

        [StringLength(20)]
        public string HonarCreditCode { get; set; }

        [StringLength(100)]
        public string disabilitypic { get; set; }

        [StringLength(10)]
        public string ConfirmDateMadarek { get; set; }

        [StringLength(10)]
        public string ConfirmDateKetabdary { get; set; }

        [StringLength(10)]
        public string LastEditDate { get; set; }

        public int KetabdaryType { get; set; }

        public int? StudyLevelId { get; set; }

        public int? CityId { get; set; }

        public int? BirthPlaceId { get; set; }

        public int? IssuePlaceId { get; set; }

        public int? BimehId { get; set; }

        public int? ReligionTypeId { get; set; }

        public int? MarriedTypeId { get; set; }

        public int? StudyLevelHozeId { get; set; }

        [StringLength(50)]
        public string BimehTakmily { get; set; }

        public int? BimehTakmilyId { get; set; }

        [StringLength(200)]
        public string AllData { get; set; }

        public int? DisabilityId { get; set; }

        [StringLength(100)]
        public string AbadiName { get; set; }

        [StringLength(10)]
        public string DeathDate { get; set; }

        [StringLength(100)]
        public string Honarpic { get; set; }

        public int? temp1 { get; set; }

        public string Shomare { get; set; }

    }
}
