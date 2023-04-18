using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Vekalat.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Vekalat.Application.Common;

namespace Vekalat.Core.Entities
{
    public class Team : Entity
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Imagename { get; set; }
        public string Description { get; set; }
        public string Socials { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Job { get; set; }
        public bool IsVisible { get; set; }

        public List<TeamGallery> TeamGalleries { get; set; }
        public List<TeamLogo> TeamLogos  { get; set; }
    }
}
