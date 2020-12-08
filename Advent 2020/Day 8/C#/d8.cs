using System;
using System.Collections.Generic;

namespace JTM{

    public static class d8 {

        public static void Main() {

            // Read the input file as an array of strings, and initialise answer variables for both parts.
            string[] input = System.IO.File.ReadAllLines(@"..\input");
            int p1Ans = 0, p2Ans = 0;

            // Initialise a Dictionary to hold all the strings by their line number, and a HashSet to hold all the
            // previously visited lines.
            Dictionary<int, string> codeString = new Dictionary<int, string>();
            HashSet<int> previousInst = new HashSet<int>();

            // Part 2 - Initialise a list of int arrays to hold all possible branches (when we encounter a nop or
            // a jmp in the unchanged input), and a list of the previously visited lines in that branch.
            List<int[]> branchPoints = new List<int[]>();
            List<HashSet<int>> branchPrevInst = new List<HashSet<int>>(); 

            // Add all the strings from input into the dictionary.
            for (int i = 0; i < input.Length; i++)
                codeString.Add(i, input[i]);

            // Integers for the accumulator and the line pointer.
            int accu = 0, pointer = 0;

            // Keeps going until it ends up at a line it's visited before. 
            while (!previousInst.Contains(pointer)){

                // Add the current pointer into the previousInst HashSet, and get the instruction type and value
                // data from the instruction list.
                previousInst.Add(pointer);
                string fullInst = codeString[pointer];
                string inst = fullInst.Substring(0, 3);
                string sign = fullInst.Substring(4, 1);
                int val = Int32.Parse(fullInst.Substring(5));

                switch (inst){

                    // If the instruction was nop, we just increment the pointer by one. For part 2, we also add
                    // the current pointer, accumulator and previousInst as a branch point.
                    case "nop":
                        branchPoints.Add(new int[]{pointer, accu});
                        branchPrevInst.Add(new HashSet<int>(previousInst));
                        pointer++;
                        break;

                    // If the instruction was acc, we increment the pointer by one and add the value to the accu.
                    case "acc":
                        pointer++;
                        if (sign == "+")
                            accu += val;
                        else
                            accu -= val;
                        break;

                    // If the instruction was jmp, we increment the pointer by the value, and add a branch point.
                    case "jmp":
                        branchPoints.Add(new int[]{pointer, accu});
                        branchPrevInst.Add(new HashSet<int>(previousInst));
                        if (sign == "+")
                            pointer += val;
                        else
                            pointer -= val;
                        break;
                }
            }

            // Save the accumulator as the answer.
            p1Ans = accu;

            // For Part 2, we go through all of the branch points we've found and run them from the changed
            // instruction. This should in theory be faster than running the same code with minor changes each time.

            // Initialise a variable for tracking whether we've found the end of the file.
            bool endFound = false;

            // Until it's either gone through all the branch points or found the end.
            for (int i = 0; i < branchPoints.Count && !endFound; i++){

                // Get all the values saved for the branch point, removing the current pointer from the previousInst
                // since we want to run it again but as a different instruction.
                pointer = branchPoints[i][0];
                accu = branchPoints[i][1];
                previousInst = new HashSet<int>(branchPrevInst[i]);
                previousInst.Remove(pointer);

                bool first = true;
                
                while (!previousInst.Contains(pointer) && !endFound){
                    previousInst.Add(pointer);
                    string fullInst = codeString[pointer];
                    string inst = fullInst.Substring(0, 3);
                    string sign = fullInst.Substring(4, 1);
                    int val = Int32.Parse(fullInst.Substring(5));

                    // If it's the first instruction, then it's our branch point and so we change the instruction type.
                    if (first) {
                        if(inst.Equals("nop"))
                            inst = "jmp";
                        else if(inst.Equals("jmp"))
                            inst = "nop";
                        first = false;
                    }

                    switch (inst){
                        case "nop":
                            pointer++;
                            break;

                        case "acc":
                            pointer++;
                            if (sign == "+")
                                accu += val;
                            else
                                accu -= val;
                            break;

                        case "jmp":
                            if (sign == "+")
                                pointer += val;
                            else
                                pointer -= val;
                            break;
                    }

                    // If the pointer gets to the end, then keep the accumulator's value as Part 2's answer and 
                    // change endFound to true so that both loops end.
                    if (pointer == codeString.Count) {
                        p2Ans = accu;
                        endFound = true;
                    }
                }
                
            }

            Console.WriteLine("Before the infinite loop, accu has the value {0}.\nAfter changing an erroneous instruction and reaching the end, accu has the value {1}", p1Ans, p2Ans);

        }
    }
}