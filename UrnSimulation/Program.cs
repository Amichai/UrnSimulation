using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrnSimulation {
    class Program {
        static void Main(string[] args) {
            var c = 10000;
            for (int i = 0; i < 100; i++) {
                var r = runTrial(c);
                Console.WriteLine(r);
            }

            Console.ReadKey();
        }

        private static double runTrial(int drawCount) {
            var urn = new Urn();
            for (int i = 0; i < drawCount; i++) {
                urn.Draw();
            }
            return urn.Draw();
        }
    }
}
