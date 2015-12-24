using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Core
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            DateCreated = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Note> Notes { get; set; }

        public bool IsTemporary()
        {
            return Id == 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTemporary() && !other.IsTemporary() && Id == other.Id)
            {
                var type = GetType();
                var otherType = other.GetType();
                return type.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(type);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (IsTemporary())
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity lhs, BaseEntity rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(BaseEntity lhs, BaseEntity rhs)
        {
            return !(lhs == rhs);
        }
    }
}
