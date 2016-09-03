using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGraph
{
    public class DerrivedProperty<TObj, TVal> : Property<TObj, TVal> where TObj : GraphObject
    {
        private Expression<Func<TObj, TVal>> property;
        private Func<TObj, TVal> defaultDelegate;
        private IEnumerable<Property> dependentProperties;


        internal DerrivedProperty(Expression<Func<TObj, TVal>> property, Func<TObj, TVal> defaultDelegate, IEnumerable<Property> dependentProperties)
        {
            if (null == property)
                throw new ArgumentNullException(nameof(property));

            this.property = property;
            this.defaultDelegate = defaultDelegate;
            this.dependentProperties = dependentProperties;
        }


        public TVal Default(TObj obj)
        {
            return defaultDelegate(obj);
        }


        public Expression<Func<TObj, TVal>> Property
        { get { return property; } }


        public IEnumerable<Property> DependentProperties
        { get { return dependentProperties; } }
    }
}
