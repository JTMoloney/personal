using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JTM {

    public static class d4 {

        public static void Main() {

            // Read the input file as a string, and initiate the valid counter.
            string inp = System.IO.File.ReadAllText(@"..\input");
            int valid = 0;

            // Split the string by detecting \r\n\r\n, which shows an empty line, and then removing empty entries
            string[] passports = inp.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
    
            // Go through each "passport"
            foreach (string passport in passports){
                
                // Creating a dictionary means I can quickly check if a field is missing (cid)
                // Though creating a class for the passport is another option.
                Dictionary<string, string> pass = new Dictionary<string, string>();

                // Further split up the passport data into individual fields. Fields have either a space or a
                // newline between them. 
                string[] fields = passport.Split(new char[]{ ' ', '\n' });

                // For the first part, I could just do this for efficiency, but where's the fun in that? 
                // It's also a lot harder for future expansion. As can be seen in part two!
                /*if (fields.Length == 8)
                    valid++;
                else if (fields.Length == 7) {
                    foreach (string field in fields) {
                       //add field to the dictionary
                    }
                    if (pass["cid"] is null)
                        valid++;
                }*/
                
                foreach (string field in fields){

                    string key = field.Substring(0, 3);
                    string value = field.Substring(4);

                    // If I'd made a passport class, I could've moved all of these checks into the set methods.
                    // Instead this ugly mess will have to do.
                    switch (key){
                    
                        case "byr":
                            if (Int32.Parse(value) >= 1920 && Int32.Parse(value) <= 2002)
                                pass.Add(key, value);
                            continue;
                        
                        case "iyr":
                            if (Int32.Parse(value) >= 2010 && Int32.Parse(value) <= 2020)
                                pass.Add(key, value);
                            continue;

                        case "eyr":
                            if (Int32.Parse(value) >= 2020 && Int32.Parse(value) <= 2030)
                                pass.Add(key, value);
                            continue;

                        case "hgt":
                            /*  So either "1" followed by either a number between 50 and 89 or between 90 and 93
                                then followed by "cm" OR a number between 59 and 76 followed by "in".
                                You could instead just search for a "cm" or "in" and then handle the number before
                                it, but there's a lot of processing required for that, whereas I can do it in one
                                go here, which should be faster? I didn't test. */
                            if (Regex.IsMatch(value, @"^(1([5-8][0-9]|9[0-3])cm|(59|6[0-9]|7[0-6])in)\s*$"))
                                pass.Add(key,value);
                            continue;

                        case "hcl":
                            if (Regex.IsMatch(value, @"^#[0-9a-f]{6}\s*$"))
                                pass.Add(key, value);
                            continue;

                        case "ecl":
                            if (Regex.IsMatch(value, @"^(amb|blu|brn|gry|grn|hzl|oth)\s*$"))
                                pass.Add(key, value);
                            continue;
                        
                        case "pid":
                            if (Regex.IsMatch(value, @"^[0-9]{9}\s*$"))
                                pass.Add(key, value);
                            continue;

                        case "cid":
                            pass.Add(key,value);
                            continue;

                        default:
                            //If it's a different key, just don't add it.
                            continue;
                    }                    
                }

                // If there are 8 fields then it's a full, valid passport. If there are 7, we can be missing
                // the cid field and still be valid.
                if (pass.Count == 8)
                    valid++;
                else if (pass.Count == 7){
                    if (!pass.ContainsKey("cid"))
                        valid++;
                }
            }

            Console.WriteLine("There are {0} valid passports.", valid);
        }
    }
}