using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; } = default!;
        public bool IsRemoved { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? RemoveDate { get; set; }
    }

    public interface IBaseEntity<T>
    {
        T Id { get; set; }
        bool IsRemoved { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ModifiedDate { get; set; }
        DateTime RemoveDate { get; set; }
    }
}
