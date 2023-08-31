using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.tag
{
    public class TagCollection
    {
        private Dictionary<string, Tag> tags;
        public int maxRegister { get; } = Int32.MinValue;
        public int minRegister { get; } = Int32.MaxValue;

        public int registerRange { get { return maxRegister - minRegister; } }

        public TagCollection(Tag[] tags)
        {
            this.tags = new Dictionary<string, Tag>(tags.Length);
            foreach (var tag in tags)
            {
                this.tags.put(tag.name, tag);
                if (tag.register > maxRegister)
                    maxRegister = tag.register;
                if (tag.register < minRegister)
                    minRegister = tag.register;
            }
        }

        public int Count => tags.Count;

        public IEnumerable<Tag> values => tags.Values;

        public IEnumerable<int> registers => tags.Values.Select(t => t.register);

        public Tag getBy(string name)
        {
            if (!tags.ContainsKey(name))
                throw new ArgumentException($"Tag não encontrada. Nenhuma tag com nome {name} foi encontrada.");
            return tags[name];
        }

        public int getRegisterBy(string name)
        {
            if (!tags.ContainsKey(name))
                throw new ArgumentException($"Registro de Tag não encontrado. Nenhuma tag com nome {name} foi encontrada.");
            return tags[name].register;
        }

        public int getRegisterBy(int sequencial, string name)
        {
            return tags.Where(t => t.Key == name && t.Value.sequencial == sequencial).Select(t => t.Value.register).FirstOrDefault();
        }

    }
}
