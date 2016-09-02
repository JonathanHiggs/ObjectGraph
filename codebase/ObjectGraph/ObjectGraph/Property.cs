using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGraph
{
    public class Property
    {
        private Type ownerType;

        internal Property(Type ownerType)
        {
            if (null == ownerType)
                throw new ArgumentNullException(nameof(ownerType));

            this.ownerType = ownerType;
        }


        public Type OwnerType
        { get { return ownerType; } }
    }


    public class Property<TObj, TVal> : Property where TObj : GraphObject
    {
        internal Property()
            : base(typeof(TObj))
        { }


        public Type ValueType
        { get { return typeof(TVal); } }
    }
}
