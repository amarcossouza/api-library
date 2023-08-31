using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.entity.user
{
    [Table(Name = "role_access")]
    public class RoleAccess : BaseEntity
    {
        public RoleAccess(Role role, string area)
        {
            this.role = role;
            this.area = area;
        }

        public RoleAccess() { }

        [Column]
        public string area { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool read { set; get; } = false;

        [Column(ConvertTo = typeof(sbyte))]
        public bool write { set; get; } = false; 


        [Column(ConvertTo = typeof(sbyte))]
        public bool canInspecaoFabric { set; get; } = false;

        [Column(ConvertTo = typeof(sbyte))]
        public bool isGlobalUser { set; get; } = false;

        [Column(ConvertTo = typeof(sbyte))]
        public bool execute { set; get; } = false;

        
        [Column]
        public int idRole { set; get; }

        private Role _role;
        [Association(ThisKey = nameof(idRole))]
        public Role role
        {
            set
            {
                _role = value;
                if (_role != null)
                    idRole = _role.id;
            }
            get { return _role; }
        }
    }
}
