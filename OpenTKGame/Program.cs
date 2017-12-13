using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKGame.Components;

namespace OpenTKGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Kör 'spelet' med en frekvens på 60 bilder per sekund
            new MainWindow().Run(60);
        }
    }
}
