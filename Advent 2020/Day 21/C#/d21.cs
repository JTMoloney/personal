using System;
using System.Collections.Generic;

namespace JTM{

    public static class d21{

        public static void Main(){

            // Read the input as an array of strings.
            string[] input = System.IO.File.ReadAllLines(@"..\input");

            // Initialise a SortedDictionary and Dictionary for the keeping track of the possible allergen ingredients and 
            // also for keeping count of the number of times an ingredient has appeared.
            SortedDictionary<string, HashSet<string>> possAllergen = new SortedDictionary<string, HashSet<string>>();
            Dictionary<string, int> ingAppearances = new Dictionary<string, int>();

            // Loop through each line of input.
            foreach (string inp in input){
                
                // Split the line into string arrays of the ingredients and allergens.
                string[] split = inp.Split(" (contains ");
                string[] ings = split[0].Split(" ");
                string[] alls = split[1].Replace(")", "").Split(", "); 

                // Create a HashSet of all ingredients in this line.
                HashSet<string> ingredientsList = new HashSet<string>();

                // We need to increase the count for each ingredient.
                foreach (string ing in ings){

                    if (!ingAppearances.ContainsKey(ing))
                        ingAppearances[ing] = 0;
                    
                    ++ingAppearances[ing];
                    
                    ingredientsList.Add(ing);

                }
                
                // Loop through all of the allergens in this line.
                foreach (string all in alls){

                    // If this is the first time the allergen appears, add all ingredients to its possible list.
                    if(!possAllergen.ContainsKey(all)){
                        possAllergen[all] = new HashSet<string>();   

                        foreach (string ing in ings)
                            possAllergen[all].Add(ing);
                    }
                    // Else, we need to remove ingredients that don't appear on this line, as if the allergen is in this
                    // food and an ingredient isn't, that ingredient can't contain the allergen.
                    // (IntersectWith only keeps items in the first HashSet if they also appear in the second HashSet)
                    else
                        possAllergen[all].IntersectWith(ingredientsList);
                }
            }

            // Loop through the HashSets in the SortedDictionary until each only has one ingredient. If a HashSet has
            // only one ingredient, remove that ingredient from all other allergen HashSets.
            bool cont = true;
            while(cont){
                cont = false;
                foreach( string all in possAllergen.Keys){
                    if(possAllergen[all].Count != 1)
                        cont = true;
                    else{
                        foreach (string all2 in possAllergen.Keys){
                            if (!all.Equals(all2))
                                possAllergen[all2].ExceptWith(possAllergen[all]);
                        }    
                    }
                }
            }

            // Calculating Part 1's answer. Sum every non-allergen ingredient's number of times found.

            int p1Ans = 0;

            // Create a Union set of all ingredients that have allergens.
            HashSet<string> possibleUnion = new HashSet<string>();
            foreach (HashSet<string> v in possAllergen.Values)
                possibleUnion.UnionWith(v);

            // Create a HashSet from all ingredients, gotten from the list of Keys for the Dictionary, and
            // then remove the ingredients that have allergens.
            HashSet<string> impossIngredients = new HashSet<string>(ingAppearances.Keys);
            impossIngredients.ExceptWith(possibleUnion);

            foreach (string ing in impossIngredients)
                p1Ans += ingAppearances[ing];

            // Calculating Part 2's answer. Concatenate all of the allergen ingredients.

            string p2Ans = "";
            foreach(string all in possAllergen.Keys){
                foreach (string ing in possAllergen[all])
                    p2Ans += ing + ",";
            }
            p2Ans = p2Ans.TrimEnd(',');

            Console.WriteLine("Ingredients which can't be allergens appear {0} times.\nThe canonical dangerous ingredient list is {1}.", p1Ans, p2Ans); 
        }
    }
}