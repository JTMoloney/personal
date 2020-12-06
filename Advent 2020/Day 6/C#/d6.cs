using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JTM {

    public static class d6 {

        public static void Main() {

            // Read the input file as a string, and initiate the sum variable.
            string inp = System.IO.File.ReadAllText(@"..\input");
            int p1Sum = 0, p2Sum = 0;

            // Split the string by detecting \r\n\r\n, which shows an empty line, and then removing empty entries
            // This results in each groups' answers being in a seperate string.
            string[] groups = inp.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);


            foreach (string group in groups) {

                // Initialise two HashSets to keep track of both answer logics.
                // p1Hash is an OR situation, so we just need to put the character into the HashSet if any person answers it.
                // p2Hash is an AND situation, so we start with all characters in the HashSet, andn the loop through all sets 
                // of answers, keeping only characters that appeared in each member's answers.
                HashSet<char> p1Hash = new HashSet<char>();
                HashSet<char> p2Hash = new HashSet<char>();

                // Fill the 2nd HashSet with all alphabet characters.
                char[] alphas = new char[]{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
                foreach (char alpha in alphas)
                    p2Hash.Add(alpha);
                
                // Split the groups' answers in each person's individual answers.
                string[] indivAnswers = group.Split("\n", StringSplitOptions.RemoveEmptyEntries);

                foreach (string answers in indivAnswers){
                    // And then split each person's answers into single answers in an array of chars. Also initialise the
                    // HashSet for p2.
                    char[] charAnswers = answers.ToCharArray();
                    HashSet<char> ansHash = new HashSet<char>();

                    foreach (char answer in charAnswers){
                        //If we find an answer, add it to p1...
                        p1Hash.Add(answer);
                        //And check if all previous group members also answered it for p2.
                        if (p2Hash.Contains(answer))
                            ansHash.Add(answer);
                        
                    }
                    //Update the p2Hash for the latest calculated version.
                    p2Hash = new HashSet<char>(ansHash);
                }

                p1Sum += p1Hash.Count;
                p2Sum += p2Hash.Count;
            }

                Console.WriteLine("There are a total of {0} unique positive answers in all groups.\nThere are a total of {1} common positive answers in all groups.", p1Sum, p2Sum);
        }

    }

}