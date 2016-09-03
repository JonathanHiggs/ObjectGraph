using ObjectGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class Environment : GraphObject
    {
        public static readonly StaticProperty<Environment, Source> SourceProperty
            = RegisterProperty<Environment, Source>(e => e.Source, Source.RealTime);

        public static readonly StaticProperty<Environment, Location> LocationProperty
            = RegisterProperty<Environment, Location>(e => e.Location, Location.NYC);

        //public static readonly StaticProperty<Environment, DateTime> PricingDateProperty
        //    = RegisterProperty<Environment, DateTime>(e => e.PricingDate, )

        public Environment(Graph graph)
            : base(graph)
        { }


        public Source Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }


        public Location Location
        {
            get { return GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }


        //public DateTime PricingDate
        //{
        //    get { return GetValue(PricingDateProperty); }
        //    set { SetValue(PricingDateProperty, value); }
        //}


        public override string ToString()
        {
            return $"<Environment ({Source}|{Location})>";
        }
    }
}
