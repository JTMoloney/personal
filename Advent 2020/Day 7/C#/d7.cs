using System;
using System.Collections.Generic;

namespace JTM{

    public static class d7{

        // A class for keeping track of each bag type, held in a dictionary.
        public class Bag {

            public List<string> parents = new List<string>();
            public Dictionary<string, int> children;

            public void addParent(string name){
                parents.Add(name);
            }
        }

        public static void Main(){

            // Read the input file as an array of strings, initiate a Dictionary to track all bags, and a counter
            // for part 2.
            string[] input = System.IO.File.ReadAllLines(@"..\input");
            Dictionary<string, Bag> bags = new Dictionary<string, Bag>();
            int p2Count = 0;

            // Go through the input and add all bag types to the Dictionary.
            foreach (string bagString in input){
                string bagName = bagString.Substring(0, bagString.IndexOf(" bags"));
                bags.Add(bagName, new Bag());
            }

            // Now that all of the bag types are in the Dictionary, we can add the parent-child relationships.
            for (int i = 0; i < input.Length; i++){
                string bagString = input[i];
                //This gets the index of the space before the word "bags".
                int strPos = bagString.IndexOf(" bags");

                // Get the type of the bag from the string, then remove it from the string so only text about the
                // children remain.
                string bagName = bagString.Substring(0, strPos);
                bagString = bagString.Substring(strPos + 14);

                Dictionary<string, int> children = new Dictionary<string, int>();

                // If it's an empty bag (leaf node), then just have an empty Dictionary.                
                if(bagString.Equals("no other bags."))
                        bags[bagName].children = children;
                else {
                    while (!bagString.Equals(".")){
                        strPos = bagString.IndexOf(" bag");

                        // Get the number of child bags and their name.
                        int childNum = Int32.Parse(bagString.Substring(0,1));
                        string childName = bagString.Substring(2, strPos-2);

                        // Add the child bag to the children variable in the parent bag, and add the parent bag to
                        // the child bag's parent variable
                        children.Add(childName, childNum);
                        bags[childName].addParent(bagName);

                        // If there's only one child bag, you need to remove one less character ("bag" vs. "bags")
                        if (childNum == 1)
                            bagString = bagString.Substring(strPos + 4);
                        else
                            bagString = bagString.Substring(strPos + 5);

                        // If there's just a full-stop, we've reached the end and so can break free from the loop.
                        if (bagString.Equals("."))
                            break;
                        else
                            bagString = bagString.Substring(2);
                    }

                    bags[bagName].children = children;
                }
            }

            string myBag = "shiny gold";

            // Part 1 - starting from the shiny gold bag, loop through the parent bags.
            // To prevent a type of bag being counted twice, keep a HashSet of bags found.
            List<string> parentBags = bags[myBag].parents;
            HashSet<string> previousParents = new HashSet<string>();

            while (parentBags.Count > 0){
                string parent = parentBags[parentBags.Count-1];
                parentBags.RemoveAt(parentBags.Count-1);
                
                // If the bag hasn't been found before, add its children to the parentBags list for looping.
                if (!previousParents.Contains(parent)){
                    parentBags.AddRange(bags[parent].parents);
                    previousParents.Add(parent);
                }
            }

            Dictionary<string, int> bagLayers = bags[myBag].children;

            // Part 2 - Starting from the shiny bag, expand all children until you reach all leaf nodes.
            while(bagLayers.Count > 0){
                // foreach is the easiest way to go through the values of a Dictionary, but then we can't
                // add to the Dictionary's values. So instead, create a copy of the Dictionary, clear the original
                // and loop through the copy, adding any new child nodes to the original list.
                Dictionary<string, int> loopThrough = new Dictionary<string,int>(bagLayers);
                bagLayers.Clear();

                // For each parent, add how many bags of the corresponding type there are to the total, and then
                // get a Dictionary of its children.
                foreach (string bagType in loopThrough.Keys){
                    int bagCount = loopThrough[bagType];
                    p2Count += bagCount;
                    Dictionary<string, int> bagChildren = bags[bagType].children;

                    // Add each child bag to the loop, taking into account how many parent bags generate it and
                    // how many child bags are in each parent.
                    foreach (string child in bagChildren.Keys){
                        int childCount = bagChildren[child];

                        // If the Dictionary already contains this type of bag, just add the new children to that
                        // value count. Else, add a new entry to the Dictionary.
                        if (bagLayers.ContainsKey(child))
                            bagLayers[child] += childCount*bagCount;
                        else
                            bagLayers.Add(child, childCount*bagCount);
                    }
                }
            }

            Console.WriteLine("There are {0} possible bags a Shiny Gold bag could be in.\nA Shiny Gold bag will contain {1} other bags.", previousParents.Count, p2Count);

        }

    }

}