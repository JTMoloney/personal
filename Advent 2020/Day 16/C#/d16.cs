using System;
using System.Collections;
using System.Collections.Generic;

namespace JTM {
    
    public static class d16 {

        public static void Main() {

            // Read the input as a string array, and initialise a Dictionary for the field ranges and a list of
            // arrays to store the composite list of all ranges (see below for more explanation).
            string[] input = System.IO.File.ReadAllLines(@"..\input");

            Dictionary<int, int[][]> fieldRanges = new Dictionary<int, int[][]>();
            List<int[]> compositeRanges = new List<int[]>();

            // Reading all of the fields and their bounds.
            for (int i = 0; i < 20; i++){
                
                string inp = input[i];

                int colonPos = inp.IndexOf(':');
                int firstDashPos = inp.IndexOf('-');
                int orPos = inp.IndexOf(" or ");
                int secondDashPos = inp.LastIndexOf('-');

                // Ended up not using the field name to simplify accessing the dictionary.
                //string field = inp.Substring(0, colonPos);
                int firstLow = Int32.Parse(inp.Substring(colonPos+2, firstDashPos - colonPos - 2));
                int firstHigh = Int32.Parse(inp.Substring(firstDashPos+1, orPos - firstDashPos - 1));
                int secondLow = Int32.Parse(inp.Substring(orPos+4, secondDashPos - orPos - 4));
                int secondHigh = Int32.Parse(inp.Substring(secondDashPos+1)); 

                int[][] ranges = new int[][]{ new int[]{firstLow, firstHigh}, 
                                            new int[]{secondLow, secondHigh}};

                // Add the field number and the read ranges to the dictionary.
                fieldRanges.Add(i, ranges);

                // So this whole system is complete overkill for what was actually needed for the task.
                // (Looking at the input you can see that there's just going to be invalid numbers above
                // the max highest range and below the min lowest range), but I wanted to challenge myself
                // to make a generalised system that could keep track of multiple seperate ranges and merge
                // them together and make new ones when needed.
                if(i == 0){
                    // The first ranges need to be added to the list.
                    compositeRanges.Add(new int[] {firstLow, firstHigh});
                    compositeRanges.Add(new int[] {secondLow, secondHigh});
                }
                else{

                    foreach (int[] range in ranges){

                        // Create a new list which will replace the current one.
                        List<int[]> newComp = new List<int[]>();

                        // Looking at the current composite ranges.                        
                        for (int j = 0; j < compositeRanges.Count; j++){

                            int[] compRange = compositeRanges[j];

                            // If the low is lower than the composite high, then we know our range will be 
                            // changing, contained in or preceding the composite range.
                            if(range[0] <= compRange[1]){ 
                                
                                // If the high is higher than the composite low, then we know that our range will
                                // be changing or contained in the composite range.
                                if (range[1] >= compRange[0]){

                                    // Get the minimum value between the lows, which will be the new low. 
                                    int min = Math.Min(range[0], compRange[0]);

                                    // Go through the next composite ranges until the high isn't as big as the next 
                                    // composite low, or we reach the end. By doing this we combine all contained 
                                    // compRanges together when we add a new range to the list.
                                    //
                                    // e.g. compositeRanges = [[1, 3], [6, 10]...] and range = [3, 6].
                                    // in this case we want to merge the two composite ranges together to [1,10].
                                    // so to begin with we get min = Math.Min(1,3) = 1, and we keep this for later.
                                    // Starting at compRange = [1,3] and range = [3,7], we compare that range[1] (6)
                                    // >= compositeRanges[j+1][0] (6), which it is, so we change compRange to [6,10].
                                    // From here we set max = Math.Max(6, 10) = 10, and so we add [min, max] ([1,10])
                                    // to our newComp list.
                                    //
                                    while (j < compositeRanges.Count - 1 && range[1] >= compositeRanges[j+1][0] - 1)
                                        compRange = compositeRanges[++j];

                                    // Get the maximum value between the highs, which will be the new high added.
                                    int max = Math.Max(range[1], compRange[1]);

                                    newComp.Add(new int[]{min, max});
                                }
                                
                                // If the high is lower than the composite low, we can just add the range and all
                                // future compRanges to the List.
                                //
                                // e.g. compositeRanges = [[5,10], [20,30]...] and range = [1,4].
                                // We obviously just want to add this new range to the beginning of compositeRanges.
                                //
                                else{
                                    newComp.Add(range);
                                    while (j < compositeRanges.Count)
                                        newComp.Add(compositeRanges[j++]);
                                }
                            }

                            // If the low is higher than the comp high, we can just add the compRange to the List.
                            //
                            // e.g. compositeRanges = [[1,3], [6,10]...] and range = [5,8].
                            // In this case we can't make any changes to [1,3] since it has no overlap with range,
                            // so we add it to the list and move on with the loop, to the next compRange ([6,10]).
                            // 
                            else
                                newComp.Add(compRange);
                            
                        }
                        // And now replace compositeRanges with our new list.
                        compositeRanges = newComp;
                    }
                }
            }

            // Get all of the int values from the input string array, and initialise errorRate for part 1.
            int[] myTicket = Array.ConvertAll(input[22].Split(','), Int32.Parse);
            int errorRate = 0;

            // Keep a track of all of the valid tickets for part 2.
            List<int[]> validTickets = new List<int[]>();
            validTickets.Add(myTicket);

            // I could make a more general system for finding where to start looking for the tickets, but
            // after thinking over that generalised range system earlier I thought I should keep it simple...
            for (int i = 25; i < input.Length; i++){

                // Convert the ticket into its values, and check that they all fit into the generalised rules
                // we've previously found. If so, add it to validTickets.
                int[] ticket = Array.ConvertAll(input[i].Split(','), Int32.Parse);
                bool valid = true;

                foreach (int val in ticket){
                    foreach (int[] range in compositeRanges){
                        if (val < range[0] || val > range[1]){
                            errorRate += val;
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid)
                    validTickets.Add(ticket);
            }

            // Let's go for maximum efficiency! BitArrays have only 1 bit per value. A little overkill, but
            // isn't this whole day's coding? It's also one-dimensional, but I'm accessing its values through 
            // [x*ticket.Length + y] to make it psuedo-2D.
            BitArray potentialFields = new BitArray(400, true);
            Dictionary<int, int> foundFields = new Dictionary<int, int>();
            long p2Ans = 1;
            int tickLen = myTicket.Length;

            for (int i = 0; foundFields.Count < 20; i++){

                // Only looping through them once doesn't get all of the answers, so we'll have to loop through
                // until we find all of our answers.
                i %= validTickets.Count;
                int[] ticket = validTickets[i];

                // Loop through the ticket's values, seeing which ranges they are valid for.
                for (int x = 0; x < tickLen; x++){

                    // If we've already found the field's true identity, don't bother looking again.
                    if (!foundFields.ContainsKey(x)){
                        int value = ticket[x];
                        int possibleFields = 0;

                        // looping through the identities.
                        for (int y = 0; y < tickLen; y++){

                            // If this field could potentially have this identity.
                            if (potentialFields[x*tickLen + y]){

                                // Get the relevent ranges from the dictionary.
                                int[][] ranges = fieldRanges[y];

                                // If this value is in the ranges for this identity, keep going, else mark it
                                // as being a non-potential pairing.
                                if ((value >= ranges[0][0] && value <= ranges[0][1]) || (value >= ranges[1][0] && value <= ranges[1][1]))
                                    possibleFields++;
                                else
                                    potentialFields[x*tickLen + y] = false;
                            }
                        }

                        // If there's only one possible pairing, then put them in the dictionary.
                        if (possibleFields == 1){
                            for (int y = 0; y < tickLen; y++){
                                if (potentialFields[x*tickLen + y]){
                                    foundFields[x] = y;

                                    // Remove the potential for this y to be paired with any other x.
                                    for(int z = 0; z < tickLen; z++){
                                        if (z != x)
                                            potentialFields[z*tickLen + y] = false;
                                    }
                                    // If it's one of the first six identities, save the value for part 2.
                                    if (y < 6)
                                        p2Ans *= myTicket[x];
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("The Ticket Scanning Error Rate is {0}. The answer to part 2 is {1}.", errorRate, p2Ans);

        }
    }
}