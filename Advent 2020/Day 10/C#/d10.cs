using System;
using System.Collections.Generic;
using System.Linq;

namespace JTM {

    public static class d10 {

        public static void Main() {

            // Read the input file as an array of strings, and then instantly parse them as ints (thanks to linq)
            // Also initialise the HashSet of the ints for easy access, and initialise part 1's counting variables.
            int[] input = Array.ConvertAll(System.IO.File.ReadAllLines(@"..\input"), Int32.Parse);
            HashSet<int> jolts = new HashSet<int>(input);
            jolts.Add(jolts.Max() + 3);     // Add the phone's joltage to the list too.
            int diff1 = 0, diff3 = 0, currentJolt = 0;

            // Starting from the plug's joltage (0), see what the next biggest joltage's difference is.
            // If it's a difference of 1 or 3, increment the respective counter.
            while (jolts.Count > 0) {

                if (jolts.Contains(currentJolt + 1)) {
                    diff1++;
                    currentJolt += 1;
                }
                else if (jolts.Contains(currentJolt + 2)) {
                    currentJolt += 2;
                }
                else if (jolts.Contains(currentJolt + 3)) {
                    diff3++;
                    currentJolt += 3;
                }
                else
                    break;
                
                jolts.Remove(currentJolt);
            }

            // Part 2

            // Initialise a new List for the inputs (adding the 0), so it can be sorted in descending order.
            List<int> jolts2 = new List<int>(input);
            jolts2.Add(0);
            jolts2 = jolts2.OrderByDescending(i => i).ToList();

            // Initialise a Dictionary for keeping a track of the number of paths available to reach the end.
            Dictionary<int, long> possiblePaths = new Dictionary<int, long>();
            possiblePaths.Add(jolts2.Max() + 3, 1);
 
            // Starting from the highest joltage and working up to the initial value (0), add the number of possible
            // paths together to get how many paths are possible from the given value.
            foreach (int jolt in jolts2){

                long pathsFromHere = 0;

                if (possiblePaths.ContainsKey(jolt + 1)) {
                    pathsFromHere += possiblePaths[jolt + 1];
                }
                if (possiblePaths.ContainsKey(jolt + 2)) {
                    pathsFromHere += possiblePaths[jolt + 2];
                }
                if (possiblePaths.ContainsKey(jolt + 3)) {
                    pathsFromHere += possiblePaths[jolt + 3];
                }
                possiblePaths.Add(jolt, pathsFromHere);
            }

            Console.WriteLine("The number of 1-jolt and 3-jolt difference multiplied is {0}.\nThe number of possible configurations is {1}.", diff1*diff3, possiblePaths[0]);

        }
    }
}