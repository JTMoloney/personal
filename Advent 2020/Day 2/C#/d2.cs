using System;

namespace JTM {

    public static class d2 {

        public static void Main() {

            // Read the input file as an array of strings, and initialise the counting variables
            string[] inp = System.IO.File.ReadAllLines(@"..\input");
            int p1 = 0;
            int p2 = 0;

            //Loop through the password lines.
            foreach (string pass in inp) {

                int min = 0, max = 0;
                string passChar, password;

                //Console.WriteLine("{0}, {1}: {2}", pass, pass.Substring(0, pass.IndexOf('-')), pass.IndexOf('-'));

                try {
                    //Get the minimum and maximum values of the allowed character.
                    int dashPos = pass.IndexOf('-');
                    int spacePos = pass.IndexOf(' ');
                    int colonPos = pass.IndexOf(':');
                    //Substring requires the starting pos and the length, which is awkward.
                    min = Int32.Parse(pass.Substring(0, dashPos));
                    max = Int32.Parse(pass.Substring(dashPos + 1, spacePos - dashPos));
                    passChar = pass.Substring(spacePos + 1, 1);
                    password = pass.Substring(colonPos + 2); //There's another space after the colon.
                }
                catch {
                    Console.WriteLine("Couldn't process input...");
                    return;
                }

                // Get a string with just the passChar in, count its length.
                int count = password.Split(passChar).Length - 1;

                if (count >= min && count <= max)
                    p1++;

                // ^ is XOR.
                if ((password.Substring(min-1, 1) == passChar ^ password.Substring(max-1, 1) == passChar))
                    p2++;

            }

            Console.WriteLine("According to the first policy, there are {0} valid passwords.\nAccording to the second policy, there are {1} valid passwords.", p1, p2);
            return;
        }
    }
}