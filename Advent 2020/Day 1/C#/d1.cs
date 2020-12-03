using System;

namespace JTM {

    public static class d1 {

        public static void Main() {

            // Read the input file as an array of strings, initialise an array of ints of same length
            string[] inp = System.IO.File.ReadAllLines(@"..\input");
            int[] expenses = new int[inp.Length];

            // bools for tracking if we've found the answers to each part of the question.
            bool p1 = false, p2 = false;

            // Convert strings to ints, check if any add to 2020 while you go.
            // Alternatively, int[] expenses = Array.ConvertAll(inp, int.Parse); and then compare afterwards...
            for (int i = 0; i < inp.Length; i++){
                try {
                    expenses[i] = Int32.Parse(inp[i]);

                    // Search all ints before i 
                    for (int j = 0; j < i; j++){
                        // If we haven't already found the answer to part 1, check for it.
                        if(!p1 && expenses[j] + expenses[i] == 2020){
                            Console.WriteLine("{0} and {1} make 2020, multiplied together they make {2}", expenses[j], expenses[i], expenses[j] * expenses[i]);
                            p1 = true;
                        }

                        // If we haven't already found the answer to part 2, check for it.
                        if(!p2){
                            for (int k = 0; k < j; k++){
                                if(expenses[k] + expenses[j] + expenses[i] == 2020){
                                    Console.WriteLine("{0}, {1} and {2} make 2020, multiplied together they make {3}", expenses[k], expenses[j], expenses[i], expenses[k] * expenses[j] * expenses[i]);
                                    p2 = true;
                                }
                            }
                        }
                    }

                    // If we've found both answers, no need to continue.
                    if (p1 && p2)
                        return;
                }
                catch {
                    Console.WriteLine("Couldn't convert input to integers...");
                    return;
                }
            }

            Console.WriteLine("Couldn't find anything else...");
            return;
        }
    }
}