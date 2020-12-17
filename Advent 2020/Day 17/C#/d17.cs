using System;
using System.Collections.Generic;

namespace JTM{

    public static class d17{

        // In my implementation I'm putting int arrays into HashSets and Dictionaries, but since they're objects two
        // seperate arrays with the same values don't hash to the same value.
        // I could just make them into strings, but decided to try making a class that extended IEqualityComparer to use
        // in the Dictionary and HashSets.
        // (Thanks to https://stackoverflow.com/questions/14663168/an-integer-array-as-a-key-for-dictionary)
        public class IntArrayEqualityComparer : IEqualityComparer<int[]>{

            // Given two int arrays, compare their contents to determine if they're equal.
            public bool Equals(int[] x, int[] y){
                
                if (x.Length != y.Length)
                    return false;

                for (int i = 0; i < x.Length; i++){
                    if (x[i] != y[i])
                        return false;
                }

                return true;
            }

            // Generate a Hash Code by multiplying the array contents with prime numbers.
            public int GetHashCode(int[] obj){

                int result = 107;

                for (int i = 0; i < obj.Length; i++){
                    unchecked{
                        result = result * 109 + obj[i];
                    }
                }
                return result;
            }
        }

        public static void Main (){

            // Read the input as an array of strings, and initialise a HashSet of int arrays for each part.
            // In my mind, using HashSets to store the coordinate values over multi-dimensional arrays is both more
            // memory efficient (doesn't have to store false values.) and has less overhead (like having to deal with 
            // the question of expanding the array in each cycle, and dealing with negative coordinates).
            string[] input = System.IO.File.ReadAllLines(@"..\input");

            HashSet<int[]> p1Active = new HashSet<int[]>(new IntArrayEqualityComparer());
            HashSet<int[]> p2Active = new HashSet<int[]>(new IntArrayEqualityComparer());

            // For each char in each line, if they're active then add them to the HashSets at z (and w) = 0.
            for (int y = input.Length - 1; y >= 0; y--){

                char[] line = input[y].ToCharArray();
                
                for (int x = 0; x < input[y].Length; x++){

                    char pos = line[x];

                    if (pos == '#'){
                        p1Active.Add(new int[]{x,y,0});
                        p2Active.Add(new int[]{x,y,0,0});
                    }
                }
            }

            // Looping for part 1.
            for (int i = 0; i < 6; i++){
                
                // A Dictionary to store how many active neighbours each cube has.
                Dictionary<int[], int> map = new Dictionary<int[], int>(new IntArrayEqualityComparer());

                // Loop through the active cubes.
                foreach(int[] pos in p1Active){

                    // Add 1 to all neighbouring spaces.
                    for(int x = -1; x <= 1; x++){
                        for(int y = -1; y <= 1; y++){
                            for(int z = -1; z <= 1; z++){
                                if (x != 0 || y != 0 || z != 0){

                                    int[] space = new int[]{pos[0] + x, pos[1] + y, pos[2] + z};

                                    if(!map.ContainsKey(space))
                                        map[space] = 1;
                                    else
                                        map[space] = map[space] + 1;
                                }
                            }
                        }
                    }
                }

                // Initialise the replacement active HashSet.
                HashSet<int[]> newAct = new HashSet<int[]>(new IntArrayEqualityComparer());

                // Add all cubes that will be active in the next cycle according to the rules.
                foreach (int[] pos in map.Keys){
                    if (map[pos] == 3 || (map[pos] == 2 && p1Active.Contains(pos))) 
                        newAct.Add(pos);
                }

                p1Active = newAct;
            }

            // Looping for part 2 - the same but with another dimension.
            for (int i = 0; i < 6; i++){
                
                Dictionary<int[], int> map = new Dictionary<int[], int>(new IntArrayEqualityComparer());

                foreach(int[] pos in p2Active){

                    for(int x = -1; x <= 1; x++){
                        for(int y = -1; y <= 1; y++){
                            for(int z = -1; z <= 1; z++){
                                for(int w = -1; w <= 1; w++){
                                    if (x != 0 || y != 0 || z != 0 || w != 0){

                                        int[] space = new int[]{pos[0] + x, pos[1] + y, pos[2] + z, pos[3] + w};

                                        if(!map.ContainsKey(space))
                                            map[space] = 1;
                                        else
                                            map[space] = map[space] + 1;
                                    }
                                }
                            }
                        }
                    }
                }

                HashSet<int[]> newAct = new HashSet<int[]>(new IntArrayEqualityComparer());

                foreach (int[] pos in map.Keys){
                    if (map[pos] == 3 || (map[pos] == 2 && p2Active.Contains(pos))) 
                        newAct.Add(pos);
                }

                p2Active = newAct;
            }

            Console.WriteLine("There are {0} active cubes in 3D, and {1} active cubes in 4D.", p1Active.Count, p2Active.Count);

        }
    }
}