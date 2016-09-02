using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectGraph
{
    public class PropertyRegister
    {
        private Dictionary<Type, HashSet<Property>> register = new Dictionary<Type, HashSet<Property>>();

        internal void RegisterProperty<T>(Property property) where T : GraphObject
        {
            var objType = typeof(T);

            if (!register.ContainsKey(objType))
                register.Add(objType, new HashSet<Property>());

            var propertySet = register[objType];

            if (!propertySet.Contains(property))
                propertySet.Add(property);
        }

        public HashSet<Property> GetProperties<T>() where T : GraphObject
        {
            var objType = typeof(T);
            if (!register.ContainsKey(objType))
                return new HashSet<Property>();

            return register[objType];
        }
    }
}
