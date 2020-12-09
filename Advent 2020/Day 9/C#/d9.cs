using System;
using System.Collections.Generic;
using System.Linq;

namespace JTM {

    public static class d9 {

        public static void Main() {

            // Read the input file as an array of strings, and then instantly parse them as longs (thanks to linq)
            // Also initialise the size-25 long array for keeping track of the previous 25 values.
            long[] input = Array.ConvertAll(System.IO.File.ReadAllLines(@"..\input"), Int64.Parse);
            long[] recent = new long[25];

            long p1Ans = 0, p2Ans = 0;

            // Search through the input until you find the answer to part 1
            for(int i = 0; i < input.Length && p1Ans == 0; i++){

                long inp = input[i];

                // For the first 25 inputs, just add them to the recent array.
                if(i < 25)
                    recent[i] = inp;
                else {
                    
                    // Keep a HashSet for the recent longs that've been previously searched.
                    HashSet<long> prev = new HashSet<long>();
                    bool sumFound = false;

                    // Loop through the recent list, checking if the number required to add up to inp is in prev
                    // and if not, add the current long to prev.
                    for (int j = 0; j < 25 && !sumFound; j++){
                        
                        long rec = recent[j];

                        if (prev.Contains(inp-rec))
                            sumFound = true;
                        else
                            prev.Add(rec);
                    }

                    // If the sum is found, replace the oldest entry in recent with the current inp.
                    if (sumFound)
                        recent[i % 25] = inp;
                    else {
                        p1Ans = inp;

                        // Loop through all the previous inputs to check for part 2's answer.
                        for (int j = 0; j < i && p2Ans == 0; j++){

                            // Keep a sum variable and a List of all the summed longs.
                            long consSum = input[j];
                            List<long> consList = new List<long>();
                            consList.Add(input[j]);
                            
                            // Add consecutive values until it is either equal to or more than the answer we're looking for.
                            for (int k = j + 1; consSum < p1Ans; k++) {

                                consSum += input[k];
                                consList.Add(input[k]);

                                // If the sum is equal to the answer, then the answer to part 2 is the maximum value
                                // in the list added to the minimum value.
                                if (consSum == p1Ans)
                                    p2Ans = consList.Max() + consList.Min();
                            }
                        }
                    }
                }
            }

            Console.WriteLine("The first number which is not the sum of the previous 25 numbers is {0}.\nThe start and end point of the list of consecutive numbers which add up to {0} add together to {1}.", p1Ans, p2Ans);
        
        }

    }

}