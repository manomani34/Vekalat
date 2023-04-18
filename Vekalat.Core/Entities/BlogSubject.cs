using Vekalat.Core.Common;
using System.Collections.Generic;

namespace Vekalat.Core.Entities
{
    public class BlogSubject : Entity
    {
        public string Title { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}

