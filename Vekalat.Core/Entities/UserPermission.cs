using Vekalat.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Core.Entities
{
    public class UserPermission : Entity
    {
        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public Permission PermissionFk { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User UserFk { get; set; }


    }
}
