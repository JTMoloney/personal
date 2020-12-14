using System;
using System.Collections.Generic;
using System.Linq;

namespace JTM {
    
    public static class d14 {

        public static void Main() {

            // Read the input as an array of strings.
            string[] input = System.IO.File.ReadAllLines(@"..\input");

            // Initialise the dictionary containing the input values, the mask as the first input, and the
            // counter variable for part 1.
            // A dictionary allows for easy access and easy overwriting in a sparse dataset like this.
            Dictionary<long, long> p1Mem = new Dictionary<long, long>();
            char[] mask = input[0].Substring(7).ToCharArray();
            long p1Ans = 0;

            for (int i = 1; i < input.Length; i++){

                // If the second character is "a", then the input is "mask", which means we need to change the
                // mask to the given value.
                if (input[i].Substring(1,1).Equals("a"))
                    mask = input[i].Substring(7).ToCharArray();
                else {
                    // Read the location and value from the mem line, converting the value into binary.
                    // The numbers are 36-bit, so I need to use long numbers rather than int.
                    long memLoc = Int64.Parse(input[i].Substring(4, input[i].IndexOf(']') - 4));
                    long memVal = Int64.Parse(input[i].Substring(input[i].IndexOf('=') + 1));
                    char[] valBin = Convert.ToString(memVal, 2).PadLeft(36, '0').ToCharArray();

                    for (int j = 0; j < valBin.Length; j++){
                        // If the mask bit is X, ignore it, else change the bit of the value.
                        if (mask[j] == 'X')
                            continue;
                        else
                            valBin[j] = mask[j];
                    }

                    memVal = Convert.ToInt64(new string(valBin), 2);

                    // Add the value to the memory location.
                    p1Mem[memLoc] = memVal;
                }                

            }

            // Sum all of the values together.
            p1Ans = p1Mem.Sum(x => x.Value);

            // Part 2

            // Reset the mask and make another Dictionary for part 2's memory. Also initialise part 2's counter.
            mask = input[0].Substring(7).ToCharArray();
            Dictionary<long, long> p2Mem = new Dictionary<long, long>();

            long p2Ans = 0;

            for (int i = 1; i < input.Length; i++){

                // Same process as last time, except we convert the location long into binary instead of the value.
                if (input[i].Substring(1,1).Equals("a")){
                    mask = input[i].Substring(7).ToCharArray();
                }
                else {
                    long memLoc = Int64.Parse(input[i].Substring(4, input[i].IndexOf(']') - 4));
                    long memVal = Int64.Parse(input[i].Substring(input[i].IndexOf('=') + 1));
                    char[] locBin = Convert.ToString(memLoc, 2).PadLeft(36, '0').ToCharArray();
                    
                    // A list which keeps track of which locations to write the value to.
                    List<char[]> locBins = new List<char[]>();

                    // First we need to apply the '1' rule of our mask, overwriting the binary's value at that
                    // position.
                    for (int j = 0; j < mask.Length; j++){
                        if (mask[j] == '1')
                            locBin[j] = '1';
                    }

                    // Add the changed binary as the first value in the list of binaries.
                    locBins.Add(locBin);

                    // Now loop through the mask again applying the 'X' rule.
                    // Whenever there's an 'X', loop through the list of binaries and create new binaries that
                    // differ by just the bit in the 'X' position. Then add these new binaries to the list.
                    // By the end, the list will contain all location binaries we need to assign the value to.
                    for (int j = 0; j < mask.Length; j++){
                        if (mask[j] == 'X'){
                            List<char[]> newBins = new List<char[]>();

                            foreach(char[] bin in locBins){
                                char[] newBin = new char[bin.Length]; 
                                bin.CopyTo(newBin,0);
                                
                                // If the bit at the jth position is 1 make it 0, else if it's 0 make it 1.
                                newBin[j] = (newBin[j] == '1' ? '0' : '1');

                                newBins.Add(newBin);
                            }
                            locBins.AddRange(newBins);
                        }
                    }

                    // Assign the value to all locations in the list.
                    foreach (char[] bin in locBins){
                        memLoc = Convert.ToInt64(new string(bin), 2);
                        p2Mem[memLoc] = memVal;
                    }
                }                
            }
            
            p2Ans = p2Mem.Sum(x => x.Value);

            Console.WriteLine("The sum of all the Dictionary values is {0} for part 1, {1} for part 2.", p1Ans, p2Ans);
        }
    }
}