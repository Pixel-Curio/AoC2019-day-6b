using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_6b
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, CelestialBody> bodies = new Dictionary<string, CelestialBody>();
            string[] orbits = File.ReadAllLines(@"day6a-input.txt");

            foreach (string orbit in orbits)
            {
                var names = orbit.Split(')');
                if (!bodies.ContainsKey(names[0])) bodies.Add(names[0], new CelestialBody { Name = names[0] });
                if (!bodies.ContainsKey(names[1])) bodies.Add(names[1], new CelestialBody { Name = names[1] });

                bodies[names[0]].OrbitedBy.Add(bodies[names[1]]);
                bodies[names[1]].Orbits.Add(bodies[names[0]]);
            }

            var paths = bodies.Where(x => x.Key == "SAN" || x.Key == "YOU")
                .Select(x => OrbitPath(x.Value).Where(x => x.Name != "YOU" && x.Name != "SAN").ToList())
                .ToArray();

            var commonOrbits = paths[0].Intersect(paths[1]);

            int shortestPath = -1;
            foreach (var body in commonOrbits)
            {
                int path = paths[0].IndexOf(body) + paths[1].IndexOf(body);
                if (shortestPath < 0 || path < shortestPath) shortestPath = path;
            }

            Console.WriteLine($"Shortest path is: {shortestPath}");
        }

        private static List<CelestialBody> OrbitPath(CelestialBody body)
        {
            return body.Orbits.Count == 0 ? new List<CelestialBody> { body } : OrbitPath(body.Orbits[0]).Prepend(body).ToList();
        }

        class CelestialBody
        {
            public string Name;
            public readonly List<CelestialBody> Orbits = new List<CelestialBody>();
            public readonly List<CelestialBody> OrbitedBy = new List<CelestialBody>();
        }
    }
}
