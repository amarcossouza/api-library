using DbExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity
{
    [Table(Name = "config")]
    public class GenericConfig<T>
    {
        // add last update, user, revision....

        [Column(IsPrimaryKey = true)]
        public string name { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool update { set; get; } = false;

        public T value { set; get; }

        [Column(Name = "value")]
        protected string _value
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = null;
                    return;
                }
                this.value = JsonConvert.DeserializeObject<T>(value);
            }
            get { return JsonConvert.SerializeObject(value); }
        }
    }
}
