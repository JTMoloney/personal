using System;
using System.Collections.Generic;

namespace JTM {

    public static class d12 {

        // The '%' operator doesn't work as we want it to for negative numbers, so we need to implement our own
        // modulus operator. In this case, if the resultant n % m is negative, add m again to make it positive.
        public static int actMod (int n, int m) {
            return ((n %= m) >= 0 ? n : n + m);
        }

        public static void Main() {

            // Read the input as an array of strings, and initialise variables for part 1.
            string[] input = System.IO.File.ReadAllLines(@"..\input");

            int posX = 0, posY = 0, dir = 90, p1Ans = 0;

            // This Dictionary holds information of how to use the F instruction's value. Each int has the
            // multiplier for [x,y] (i.e if our angle is 0 (north), our value will be added to posY. If it is
            // 180 (south), it will be subtracted from posY).
            Dictionary<int, int[]> dirChange = new Dictionary<int, int[]>();
            dirChange.Add(0,    new int[] {0,1});
            dirChange.Add(90,   new int[] {1,0});
            dirChange.Add(180,  new int[] {0,-1});
            dirChange.Add(270,  new int[] {-1,0});

            // Loop through each instruction.
            foreach (string inp in input){

                // Seperate the type of instruction from its value.
                char inst = inp[0];
                int value = Int32.Parse(inp.Substring(1));

                switch (inst) {
                    
                    // The four cardinal directions are simple changes to their respective axis.
                    case 'N':
                        posY += value;
                        break;

                    case 'E':
                        posX += value;
                        break;

                    case 'S':
                        posY -= value;
                        break;

                    case 'W':
                        posX -= value;
                        break;

                    // For turning we add or subtract the value and then get the modulo 360.
                    case 'L':
                        dir = actMod((dir - value), 360);
                        break;

                    case 'R':
                        dir = actMod((dir + value), 360);
                        break;

                    // For F we look in the Dictionary to see how we change the X and Y positions according to
                    // the direction we're facing.
                    case 'F':
                        int[] change = dirChange[dir];
                        posX += change[0] * value;
                        posY += change[1] * value;
                        break;
                }
            }

            p1Ans = Math.Abs(posX) + Math.Abs(posY);

            // Part 2.

            // Reset the plane position and initialise variables for part 2.
            posX = 0;
            posY = 0;
            int wpX = 10, wpY = 1, p2Ans = 0;

            // A dictionary for keeping a track of how to rotate the waypoint around clockwise. Each rotate
            // value has two int arrays which say how to determine the new [x,y] coordinates according to
            // the current [x,y] coordinates. (i.e. to turn the coords 90 clockwise, new x will become the 
            // old y and the new y will become negative old x).
            // Anti-clockwise rotation can also use this dictionary by looking up 360-value, since a 90 degree
            // clockwise turn is the same as a 270 degree anti-clockwise turn.
            Dictionary<int, int[][]> CTurn = new Dictionary<int, int[][]>();
            CTurn.Add(90,   new int[][] {   new int[] { 0,  1 },
                                            new int[] { -1,  0 } });
            CTurn.Add(180,  new int[][] {   new int[] { -1, 0 },
                                            new int[] { 0,  -1 } });
            CTurn.Add(270,  new int[][] {   new int[] { 0,  -1 },
                                            new int[] { 1, 0 } });
            
            foreach (string inp in input){

                char inst = inp[0];
                int value = Int32.Parse(inp.Substring(1));
                
                // Initialise variables that will be used for rotations
                int tmpX = 0, tmpY = 0;
                int[][] rotate = new int[2][];

                switch (inst) {

                    // Cardinal directions are the same, just applied to the waypoint.
                    case 'N':
                        wpY += value;
                        break;

                    case 'E':
                        wpX += value;
                        break;

                    case 'S':
                        wpY -= value;
                        break;

                    case 'W':
                        wpX -= value;
                        break;

                    // Get the rotate values from the dictionary and calculate the new waypoint coords using
                    // the old ones.
                    case 'L':
                        rotate = CTurn[360 - value];
                        tmpX = wpX;
                        tmpY = wpY;
                        wpX = rotate[0][0] * tmpX + rotate[0][1] * tmpY;
                        wpY = rotate[1][0] * tmpX + rotate[1][1] * tmpY;
                        break;

                    case 'R':
                        rotate = CTurn[value];
                        tmpX = wpX; 
                        tmpY = wpY;
                        wpX = rotate[0][0] * tmpX + rotate[0][1] * tmpY;
                        wpY = rotate[1][0] * tmpX + rotate[1][1] * tmpY;
                        break;

                    // Add the waypoint coords multiplied by the value.
                    case 'F':
                        posX += wpX * value;
                        posY += wpY * value;
                        break;
                }
            }

            p2Ans = Math.Abs(posX) + Math.Abs(posY);

            Console.WriteLine("The Manhatten Distance between the starting point and end point is {0} for part 1, {1} for part 2.", p1Ans, p2Ans);

        }
    }
}