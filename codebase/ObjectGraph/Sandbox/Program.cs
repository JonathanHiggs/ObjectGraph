using ObjectGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var graph = new Graph())
            {
                var env = new Environment(graph);

                Console.WriteLine(env);

                graph.Diddle(() =>
                {
                    env.Source = Source.Close;

                    Console.WriteLine(env);

                    graph.Diddle(() =>
                    {
                        env.Location = Location.LDN;

                        Console.WriteLine(env);
                    });
                    
                    Console.WriteLine(env);
                });

                Console.WriteLine(env);
            }
        }
    }
}
