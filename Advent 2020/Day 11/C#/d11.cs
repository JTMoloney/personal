using System;

namespace JTM {

    public static class d11 {

        public static void Main() {

            //Read the input as an array of strings, and initialise two 2D char arrays which will hold the map for part 1 and part 2.
            string[] input = System.IO.File.ReadAllLines(@"..\input");
            char[,] chairMap = new char[input[0].Length, input.Length]; 
            char[,] chairMap2 = new char[input[0].Length, input.Length]; 

            int p1Occupied = 0, p2Occupied = 0;

            // Loop through the string array, adding each char to the 2D Arrays.
            for (int i = 0; i < input.Length; i++){

                char[] inp = input[i].ToCharArray();

                for (int j = 0; j < inp.Length; j++){
                    chairMap[j, i] = inp[j];
                    chairMap2[j, i] = inp[j];
                }
            }

            // Loop through the char array and apply changes until no more changes are made.
            int changesMade = 1;

            while (changesMade > 0){
                changesMade = 0;

                // We need to clone the array to a temporary variable so that we can make changes to the original while keeping the information
                // on what the original state was (e.g. if we change an empty seat to a full seat, it should still be considered an empty seat
                // when looking at the seat to the right).
                char[,] temp = (char[,]) chairMap.Clone();

                // 'i' is for the y-axis, 'j' is for the x-axis. When we usually look at tables or graphs we move across the x-axis first
                // before moving up/down the y-axis, so this reverse for-looping makes it easier to comprehend what's happening.
                for (int i = 0; i < input.Length; i++){
                    for (int j = 0; j < input[0].Length; j++){

                        char space = temp[j, i];

                        if (space.Equals('.'))
                            continue;
                        else{
                            // A secondary loop which will change the x- and y-axis by -1 to 1 - i.e. go to all adjacent squares.
                            // The conditional expressions (x ? y : z) is preventing IndexOutOfBounds errors by checking if 'i' and 'j' are at
                            // the edges of the array and reducing the directional search space accordingly.
                            // (e.g. if i == 0, we don't try to search index -1)
                            int adjacent = 0;
                            for (int y = (i > 0 ? -1 : 0); y <= (i < input.Length - 1 ? 1 : 0); y++){
                                for (int x = (j > 0 ? -1 : 0); x <= (j < input[0].Length - 1 ? 1 : 0); x++) {
                                    // An extra check here to make sure we're not looking at the original [j, i] space.
                                    if ((x != 0 || y != 0) && temp[j + x, i + y].Equals('#'))
                                        adjacent++;
                                }
                            }

                            // Change the space according to our rules.
                            if (space.Equals('L') && adjacent == 0){
                                chairMap[j, i] = '#';
                                changesMade++;
                                p1Occupied++;
                            }
                            else if (space.Equals('#') && adjacent >= 4){
                                chairMap[j, i] = 'L';
                                changesMade++;
                                p1Occupied--;
                            }
                        }
                    }
                }
            }

            // Part 2

            changesMade = 1;

            // This time create an array of the directions we want to move in to check for chairs. This could be considered prettier code?
            int[][] directions = new int[8][] {
                new int[] {0,1},
                new int[] {1,1},
                new int[] {1,0},
                new int[] {1,-1},
                new int[] {0,-1},
                new int[] {-1,-1},
                new int[] {-1,0},
                new int[] {-1,1}
            };

            while (changesMade > 0){
                changesMade = 0;

                char[,] temp = (char[,]) chairMap2.Clone();

                for (int i = 0; i < input.Length; i++){
                    for (int j = 0; j < input[0].Length; j++){

                        char space = temp[j, i];

                        if (space.Equals('.'))
                            continue;
                        else{
                            int seen = 0;
                            
                            // For each direction, find the nearest seat, if there is one.
                            foreach (int[] dir in directions){

                                int x = j + dir[0], y = i + dir[1];
                                bool found = false;

                                // If we find a seat, we should stop, but if we don't, keep going in the given direction.
                                while(!found && x >= 0 && y >= 0 && x < input[0].Length && y < input.Length){
                                    if(temp[x, y].Equals('#')){
                                        seen++;
                                        found = true;
                                    }
                                    else if(temp[x,y].Equals('L')){
                                        found = true;
                                    }
                                    else{
                                        x += dir[0];
                                        y += dir[1];
                                    }
                                }
                            }

                            if (space.Equals('L') && seen == 0){
                                chairMap2[j, i] = '#';
                                changesMade++;
                                p2Occupied++;
                            }
                            else if (space.Equals('#') && seen >= 5){
                                chairMap2[j, i] = 'L';
                                changesMade++;
                                p2Occupied--;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("There are {0} occupied seats for part 1, and {1} occupied seats for part 2.", p1Occupied, p2Occupied);

        }
    }
}