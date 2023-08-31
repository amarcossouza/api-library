using DbExtensions;
using System;

namespace LIB.entity
{
    [Serializable]
    public abstract class BaseEntity<T>
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public T id { get; set; }

        public bool isNew()
        {
            return id == null;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + id.GetHashCode();
            //hash = hash * 23 + isNew().GetHashCode();
            return hash;
        }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int atualize { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int insert { set; get; }

        [Column]
        public DateTime lastEntry { protected set; get; } = DateTime.Now;
    }


   

    [Serializable]
    public abstract class BaseEntity
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id { get; set; }

        public bool isNew()
        {
            return id <= 0;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + id.GetHashCode();
            //hash = hash * 23 + isNew().GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseEntity))
                return false;
            return this.GetHashCode() == obj.GetHashCode();
        }

      //  [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int atualize { set; get; }

       // [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int insert { set; get; }

        [Column]
        public DateTime lastEntry { protected set; get; } = DateTime.Now;
    }

    [Serializable]
    public abstract class BaseDelectableEntity : BaseEntity
    {
        [Column]
        public DateTime? deleted { protected get; set; } = null;

        public bool isDeleted => !deleted.HasValue;

        public void Delete() => deleted = DateTime.Now; 
    }
}
