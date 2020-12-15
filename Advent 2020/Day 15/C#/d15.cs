using System;
using System.Collections.Generic;

namespace JTM {
    
    public static class d15 {

        public static void Main() {

            // Read the input as a string, and split it on the commas, so we only have the integers.
            // Also initialise a Dicionary to keep track of the number history and counter variables.
            string[] input = System.IO.File.ReadAllText(@"..\input").Split(',');
            Dictionary<int, int> history = new Dictionary<int, int>();
            int currNum = 0, nextNum = 0, p1Ans = 0, p2Ans = 0;

            // Add all but the last input numbers into the history.
            for (int i = 1; i < input.Length; i++)
                history.Add(Int32.Parse(input[i - 1]), i);
            
            // Set the last input number as the next number.
            nextNum = Int32.Parse(input[input.Length - 1]);

            // For the 30,000,000 turns required:
            for (int turn = input.Length; turn <= 30000000; turn++){

                // Get the next number, as defined from the last turn.
                currNum = nextNum;

                // If it's turn 2020 (part 1) or 30,000,000 (part 2), save the number.
                if (turn == 2020)
                    p1Ans = currNum;
                else if (turn == 30000000)
                    p2Ans = currNum;

                // If this is the first time the number has come up (as in it isn't in the history), add it to
                // the history and set the next number to be 0.
                if(!history.ContainsKey(currNum)){
                    history[currNum] = turn;
                    nextNum = 0;
                }
                // Else find the difference between the current turn and the last time the number appeared,
                // which is the next number, and then replace the current number's turn value in the Dictionary.
                else {
                    nextNum = turn - history[currNum];
                    history[currNum] = turn;
                }
            }

            Console.WriteLine("The number on turn 2020 is {0}, and on turn 30000000 it is {1}.", p1Ans, p2Ans);
        }
    }
}