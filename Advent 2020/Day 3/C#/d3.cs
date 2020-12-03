using System;

namespace JTM {

    public static class d3 {

        public static void Main() {

            // Read the input file as an array of strings, and initialise a 2d char array* to represent the map
            // *Technically a jagged array, but this is easier to handle and apparently more efficient to use.
            string[] inp = System.IO.File.ReadAllLines(@"..\input");
            char[][] map = new char[inp.Length][];

            // Add each string to the 2d char array.
            for (int i = 0; i < inp.Length; i++)
                map[i] = inp[i].ToCharArray();
            
            // Initialise the bumps counting array, the list of slopes to be used, and the bump multiplication count.
            int bumpMult = 1;
            
            int[] bumps = new int[] {0, 0, 0, 0, 0};

            int[][] slopes = new int[][] {
                new int[] {1,1}, 
                new int[] {3,1}, 
                new int[] {5,1}, 
                new int[] {7,1}, 
                new int[] {1,2}
            };

            for (int i = 0; i < slopes.Length; i++){

                int x = 0, y = 0, changeX = slopes[i][0], changeY = slopes[i][1], maxX = map[0].Length, maxY = inp.Length;
            
                while (y < maxY){

                    if(map[y][x] == '#')
                        bumps[i] = bumps[i] + 1;

                    x = (x + changeX) % maxX;
                    y = y + changeY;
                }
            
                Console.WriteLine("When moving at a slope of ({0}, {1}), you bump into {2} trees.", changeX, changeY, bumps[i]);

                bumpMult *= bumps[i];

            }

            Console.WriteLine("Which when multiplied together makes {0}.", bumpMult);

        }
    }
}