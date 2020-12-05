using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JTM {

    // A class which handles converting the fake-binary into real binary and then decimal.
    public class BoardingPass {

        private int row = 0;
        private int col = 0;
        private int sid = 0;

        public int Row {
            get { return row; }
        }
        public int Col {
            get { return col; }
        }
        public int SID {
            get { return sid; }
        }

        public BoardingPass(string pass){
            SetPass(pass);
        }

        public void SetPass(string pass){
            string rowS = pass.Substring(0, 7);
            string colS = pass.Substring(7, 3);

            rowS = Regex.Replace(rowS, "F", "0");
            rowS = Regex.Replace(rowS, "B", "1");
            colS = Regex.Replace(colS, "L", "0");
            colS = Regex.Replace(colS, "R", "1");
 
            // For both strings, go from right to left (pos) and add the binary 1 bits to the row and col variables.
            for (int i = 1, pos = 6; pos >= 0; i *= 2){
                this.row += i * Int32.Parse(rowS.Substring(pos--, 1));
            }

            for (int i = 1, pos = 2; pos >= 0; i *= 2){
                this.col += i * Int32.Parse(colS.Substring(pos--, 1));
            }

            this.sid = (row * 8) + col;
        }

    }

    public static class d5 {

        public static void Main() {

            // Read the input file as an array of strings, and initiate variables to find the min and max SIDs.
            string[] input = System.IO.File.ReadAllLines(@"..\input");
            int min = Int32.MaxValue, max = 0;

            // Initialise the Dictionary to keep a track of which SIDs are in use. I could just use a <int, bool>
            // Dictionary to save space, but keeping the passes in memory would be more useful if we ever wanted
            // to expand on this.
            Dictionary<int, BoardingPass> passes = new Dictionary<int, BoardingPass>();

            foreach (string inp in input){

                BoardingPass pass = new BoardingPass(inp);
                min = Math.Min(min, pass.SID);
                max = Math.Max(max, pass.SID);               
                passes[pass.SID] = pass;

            }

            Console.WriteLine("The lowest SID is {0}. The highest SID is {1}.", min, max);

            // Dictionaries have (one of the) best efficiency for searching, since they're a hash table.
            // It also means we can get a simple boolean answer as to whether a value is in the table, rather
            // than having to count through them all.
            for (int i = min; i <= max; i++){
                if (!passes.ContainsKey(i)){
                    Console.WriteLine("SID {0} is missing.", i);
                    return;
                }
            }

            return;

        }
    }
}