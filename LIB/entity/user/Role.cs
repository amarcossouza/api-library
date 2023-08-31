using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.entity.user
{
    [Table(Name = "role")]
    public class Role : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.MinLength(3)]
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string name { set; get; }

        [Association(OtherKey = nameof(RoleAccess.idRole))]
        public List<RoleAccess> roles { set; get; }

        public void AddAccess(RoleAccess access) {
            if (roles == null)
                roles = new List<RoleAccess>();
            roles.Add(access);
        }


        public override string ToString()
        {
            return $@"{name};
                  {lastEntry};";

        }


    }
}
