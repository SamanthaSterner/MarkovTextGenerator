using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovTextGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Chain chain = new Chain();

            Console.WriteLine("Welcome to Marky Markov's Random Text Generator!");
            /*
            Console.WriteLine("Enter some text I can learn from (enter single ! to finish): ");

            while (true)
            {

                Console.Write("> ");

                String line = Console.ReadLine();
                if (line == "!")
                    break;

                chain.AddString(line);  // Let the chain process this string
            }
            */
            string[] lines = File.ReadAllLines(@"Files/Script.txt");
            foreach (String s in lines)
            {
                chain.AddString(s);
            }

            // Now let's update all the probabilities with the new data
            chain.UpdateProbabilities();

            // Okay now for the fun part
            Console.WriteLine("Done learning!  Now give me a word and I'll tell you what comes next.");
            Console.Write("> ");

            String output = "";
            String word = Console.ReadLine();
            while (true)
            {
                output += word + " ";
                word = chain.GetNextWord(word);
             
                if (word == "")
                {
                    break;
                }
            }

            output = output.Trim();
            output = output.Substring(0, 1).ToUpper() + output.Substring(1) + ".";
            Console.WriteLine(output);
       }
    }
}
