using System;
using System.Collections.Generic;

namespace JTM {

    public static class d13 {

        public static void Main() {

            // Read the input as an array of strings. The first line is only used for Part 1.
            string[] input = System.IO.File.ReadAllLines(@"..\input");
            int aimTime = Int32.Parse(input[0]), minTime = Int32.MaxValue, minBus = 0;

            // Split the second line into the individual bus IDs.
            String[] busTimes = input[1].Split(",");

            // Loop through the busTimes, finding when they arrive after aimTime and choosing the smallest wait. 
            for (int i = 0; i < busTimes.Length; i++){
                string bus = busTimes[i];
                int busID = 0;

                if (Int32.TryParse(bus, out busID)){
                    // The strange ((x + y - 1) / y) ensures that we round up the division of x.
                    int firstPoss = busID * ((aimTime + busID - 1)/busID);
                    // Keep a track of the smallest arrival time after the aimTime.
                    if (firstPoss < minTime){
                        minBus = busID;
                        minTime = firstPoss;
                    }
                }
            }

            // Part 2

            // Create a long array for the bus times.
            long[] busTimesL = new long[busTimes.Length];

            // Parse the numbers into the long array.
            for (int i = 0; i < busTimes.Length; i++){
                
                if(busTimes[i] == "x")
                    busTimesL[i] = 1;
                else
                    busTimesL[i] = Int64.Parse(busTimes[i]);
            }

            long time = 0, inc = busTimesL[0];

            // I had no idea how to approach this one apart from through brute force, but thanks to:
            // (https://www.reddit.com/r/adventofcode/comments/kc4njx/2020_day_13_solutions/gfokhzh/)
            // I managed to get a solution. 
            // Take it slowly and find the fastest time for the first two busses to sync up how we want them to,
            // using the first bus' time as the increment. When the synced time is found, use the sum of the
            // bus's times as the new increment for the next bus. Continue this until all buses are now aligned.
            for (int i = 1; i < busTimesL.Length; i++){

                long nextBus = busTimesL[i];
                
                if (nextBus != 1){
                    while(true){
                        time += inc;
                        if ((time + i) % nextBus == 0){
                            inc *= nextBus;
                            break;
                        }
                    }
                }
            }                

            Console.WriteLine("The answer to Part 1 is {0}. The answer to Part 2 is {1}.", minBus * (minTime - aimTime), time);
        }
    }
}