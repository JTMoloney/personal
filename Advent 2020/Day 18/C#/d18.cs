using System;
using System.Collections.Generic;

namespace JTM {
    
    public static class d18 {

        // A method for parsing Reverse Polish Notation queues and returning the long value it represents.
        public static long RPNParse(Queue<char> queue){
            
            // Initialise the Stack for holding all of the values.
            Stack<long> intStack = new Stack<long>();
            long x = 0;

            // Loop through the characters in the queue.
            foreach (char token in queue){
                // If the token is a number, and so can be parsed, push the number onto the stack.
                if (Int64.TryParse(token.ToString(), out x))
                    intStack.Push(x);
                // Else either add or multiply the top two numbers together, pushing the result onto the stack.
                else{
                    
                    switch (token){

                        case '+':
                            intStack.Push(intStack.Pop() + intStack.Pop());
                            break;

                        case '*':
                            intStack.Push(intStack.Pop() * intStack.Pop());
                            break;
                        
                    }
                }
            }

            // Return the final variable.
            return intStack.Pop();
        }

        public static void Main() {

            // Read the input as a string array.
            string[] input = System.IO.File.ReadAllLines(@"..\input");

            // Intialise HashSets to quickly check whether a character is a number or a token.
            // We could just Int64.TryParse(new string(token)) to find whether the token is a valid num too, like in
            // RPNParse(), but I think that method looks a little ugly.
            // (Debatably, so does making a HashSet like this...)
            HashSet<char> ops = new HashSet<char>();
            ops.Add('+');
            ops.Add('*');

            HashSet<char> nums = new HashSet<char>();
            nums.Add('1');
            nums.Add('2');
            nums.Add('3');
            nums.Add('4');
            nums.Add('5');
            nums.Add('6');
            nums.Add('7');
            nums.Add('8');
            nums.Add('9');

            // Make a Dictionary for the precedence of the operators for Part 2. In this case addition is performed
            // before multiplication. Could compare the chars, but this makes it more easily extendable. 
            // Also initialise the counter variables for both parts.
            Dictionary<char, int> precedence = new Dictionary<char, int>();
            precedence['+'] = 2;
            precedence['*'] = 1;

            long p1Ans = 0, p2Ans = 0;

            // Loop through all of the input strings.
            foreach (string inp in input){
                
                // Convert the string into a character array that can be looped through.
                char[] eq = inp.ToCharArray();

                // Initialise a Stack and Queue for converting the equation in RPN.
                Stack<char> opStack = new Stack<char>();
                Queue<char> output = new Queue<char>();

                // If you don't know RPN, it's where an equation is represented as having its operators after its numbers
                // e.g. 5 * 7 => 5 7 *
                // e.g. 4 * 7 + 5 => 4 7 * 5 +
                // e.g. 4 * (7 + 5) => 4 7 5 + *
                // This allows for unambiguous representation of equations, and for a simple left-to-right parsing of
                // the equation. 
                //
                // To convert to RPN, we loop through each character...
                foreach (char token in eq){
                    
                    // If the token is a number, we add it to the queue.
                    if(nums.Contains(token))
                        output.Enqueue(token);
                    // If the token is an operator (+, *), we pop previous operators on the stack into the queue, and then 
                    // add the current token to the stack.
                    // (This part has no operation precedence, so there's no need to enforce multiplication before addition)
                    else if(ops.Contains(token)){
                        while (opStack.Count > 0 && ops.Contains(opStack.Peek()))
                            output.Enqueue(opStack.Pop());
                        
                        opStack.Push(token);
                    }
                    // If the token is an open bracket, we push the open bracket onto the operator stack. 
                    else if(token.Equals('('))
                        opStack.Push(token);
                    // When we find a close bracket, we pop the operator stack onto the queue until we find the open
                    // bracket, which we remove. This ensures all operators inside of brackets are performed before they
                    // are used outside of the brackets.
                    else if(token.Equals(')')){
                        while (!opStack.Peek().Equals('('))
                            output.Enqueue(opStack.Pop());
                        
                        opStack.Pop();
                    }
                }
                // Then empty all remaining operators in the stack into the queue.
                while (opStack.Count > 0)
                    output.Enqueue(opStack.Pop());
                
                // Parse the output queue and add its value to our part 1 counter.
                p1Ans += RPNParse(output);

                // Loop for Part 2.
                // This is the same, except we pay attention to our precedence.
                opStack = new Stack<char>();
                output = new Queue<char>();

                foreach (char token in eq){

                    if(nums.Contains(token))
                        output.Enqueue(token);
                    else if(ops.Contains(token)){
                        // Now we only pop the operator to the queue if it's a higher precedence than our token.
                        // This means that if our token needs to be performed first, it will be.
                        while (opStack.Count > 0 && ops.Contains(opStack.Peek()) 
                                && precedence[opStack.Peek()] > precedence[token])
                            output.Enqueue(opStack.Pop());
                        
                        opStack.Push(token);
                    }
                    else if(token.Equals('('))
                        opStack.Push(token);
                    else if(token.Equals(')')){
                        while (!opStack.Peek().Equals('('))
                            output.Enqueue(opStack.Pop());
                        
                        opStack.Pop();
                    }
                }
                while (opStack.Count > 0)
                    output.Enqueue(opStack.Pop());

                // Parse the output queue and add its value to our part 2 counter.
                p2Ans += RPNParse(output);
            }

            Console.WriteLine("The sum of all lines for Part 1 is {0}, and for Part 2 is {1}.", p1Ans, p2Ans);

        }
    }
}