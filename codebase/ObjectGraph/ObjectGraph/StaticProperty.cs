using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGraph
{
    public class StaticProperty<TObj, TVal> : Property<TObj, TVal> where TObj : GraphObject
    {
        private Expression<Func<TObj, TVal>> property;
        private TVal defaultValue;


        internal StaticProperty(Expression<Func<TObj, TVal>> property, TVal defaultValue)
        {
            if (null == property)
                throw new ArgumentNullException(nameof(property));

            this.property = property;
            this.defaultValue = defaultValue;
        }


        public TVal Default
        { get { return defaultValue; } }


        public Expression<Func<TObj, TVal>> Property
        { get { return property; } }
    }
}
