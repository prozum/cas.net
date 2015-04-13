using System;

namespace Geometry
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            //Længde beregning vha. pythagoras
            Console.WriteLine ("Længde af hypotenuse: {0}", Geomet.Triangle.Pythagoras (2, 2, 0));

            Console.WriteLine ("A^b_C");
            Console.WriteLine ("Areal: {0}", Geomet.Triangle.Area (2, 2, 0, 2, 2, 90));

            double[] angles = Geomet.Triangle.Angles (3, 4, 5);
            Console.WriteLine ("Angles: {0}, {1}, {2}", angles[0], angles[1], angles[2]);
            Console.ReadLine ();

            //Console.WriteLine ("Side is: {0}", Triangle.SineRelation (2, 3, 0, 0, 36.9, 53.1, 0));
        }
    }
}
