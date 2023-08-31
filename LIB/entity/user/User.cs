using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LIB.entity.user
{
    // TODO: Use deletableEntity for softdelete users
    [Table(Name = "user")]
    public class User : BaseDelectableEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(3)]
        [Column]
        public string name { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(3)]
        [Column]
        public string login { set; get; }


        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idCliente { set; get; }
               
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool active { set; get; } = false;
                       
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool dataAgreement { set; get; }

      //  public int atualize { set; get; }


        [System.ComponentModel.DataAnnotations.EmailAddress]
        [Column]
        public string email { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool notify { set; get; } = true;

        [Column(Name = "pass")]
        protected string _pass { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(4)]
        public string pass
        {
            set => _pass = passHash(value);
            get => _pass;
        }

        //[Column]
        //public DateTime lastEntry { set; get; }

        [Association(ThisKey = nameof(idRole), OtherKey = nameof(RoleAccess.idRole))]
        public List<RoleAccess> roles { set; get; }

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

        public bool passIsEquals(string pass)
        {
            if (string.IsNullOrEmpty(this.pass))
                return false;
            return _pass.Equals(passHash(pass));
        }

        private string passHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("", hash.Select(b => b.ToString("X2")).ToArray());
            }
        }


        // go to another class
        public bool canAccess(string path)
        {
            if (roles == null) return false;

            if (path.Contains("user") || path.Contains("role") || path.Contains("transaction"))
                path = "user";
            foreach (RoleAccess p in roles)
            {
                if (path.Contains("login")
                  || path == "/"
                  || path.Contains("menu")
                  || path.Contains("home")
                  || path.Contains("report"))
                    return true;

                if (path.Contains(p.area.ToLower()) && (p.read ))//|| p.write))
                    return true;
            }
            return false;
        }

        public bool canWrite(string path)
        {
            if (roles == null) return false;
            if (path.Contains("user") || path.Contains("role") || path.Contains("transaction"))
                path = "user";
            foreach (RoleAccess p in roles)
            {
                if (path.Contains(p.area.ToLower()) && (p.read && p.write))
                    return true;
            }
            return false;
        }



        public bool IsGlobalUser()
        {
            if (roles == null) return false;

            foreach (RoleAccess r in roles)
            {
                if (r.isGlobalUser)
                    return true;
            }
            return false;
        }

        public bool CanInspecaoFabric(string path)
        {
            if (roles == null) return false;

            foreach (RoleAccess r in roles)
            {
                if ((path.ToLower().Contains("relatorio") && r.area.Contains("relatorio"))
                    && r.canInspecaoFabric)
                    return true;
            }
            return false;
        }



        public override string ToString()
        {           
            return name;
        }

        public  string ToStringObject()
        { string getUserData = "";
            getUserData = name + ";" ;
            getUserData += login + ";";
            getUserData += email + ";";
            getUserData += pass + ";";
            getUserData += dataAgreement + ";";
          //  getUserData += cadastroPower + ";";
         //   getUserData += inspecaoFabricPower + ";";
            getUserData += active + ";";
            getUserData += idRole + ";";

            return getUserData;
        }
    }
}
