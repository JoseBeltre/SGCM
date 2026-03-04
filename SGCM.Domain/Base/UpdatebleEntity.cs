using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCM.Domain.Base
{
    public abstract class UpdatableEntity : BaseEntity
    {
        public abstract DateTime? UpdatedAt { get; set; }
        public abstract DateTime CreatedAt { get; set; }
    }
}
