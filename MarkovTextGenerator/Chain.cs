using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovTextGenerator
{
    class Chain
    {
        public Dictionary<String, List<Word>> words;
        private Dictionary<String, int> sums;
        private Random rand;

        public Chain ()
        {
            words = new Dictionary<String, List<Word>>();
            sums = new Dictionary<string, int>();
            rand = new Random(System.Environment.TickCount);
        }

        // This may not be the best approach.. better may be to actually store
        // a separate list of actual sentence starting words and randomly choose from that
        public String GetRandomStartingWord ()
        {
            return words.Keys.ElementAt(rand.Next() % words.Keys.Count);
        }

        // Adds a sentence to the chain
        // You can use the empty string to indicate the sentence will end
        //
        // For example, if sentence is "The house is on fire" you would do the following:
        //  AddPair("The", "house")
        //  AddPair("house", "is")
        //  AddPair("is", "on")
        //  AddPair("on", "fire")
        //  AddPair("fire", "")

        public void AddString (String sentence)
        {
            string[] list = sentence.Split(new char[] {' ', ',', '.', '!', '?'});

            for (int i  = 0; i < list.Length - 1; i++)
            {
                AddPair( list[i], list[i + 1]); 
            }
            AddPair(list[list.Length - 1], "");
        }

        // Adds a pair of words to the chain that will appear in order
        public void AddPair(String word, String word2)
        {
            if (!words.ContainsKey(word))
            {
                sums.Add(word, 1);
                words.Add(word, new List<Word>());
                words[word].Add(new Word(word2));
            }
            else
            {
                bool found = false;
                foreach (Word s in words[word])
                {
                    if (s.ToString() == word2)
                    {
                        found = true;
                        s.Count++;
                        sums[word]++;
                    }
                }

                if (!found)
                {
                    words[word].Add(new Word(word2));
                    sums[word]++;
                }
            }
        }

        // Given a word, randomly chooses the next word
        public String GetNextWord (String word)
        {
            if (words.ContainsKey(word))
            {
                double choice = rand.NextDouble();

                double sum = 0;
                for (int i = 0; i < words[word].Count; i++)
                {
                    sum += words[word][i].Probability;

                    if (sum >= choice)
                        return words[word][i].ToString();
                }
            }

            return "idkbbq";
        }

        public void UpdateProbabilities ()
        {
            foreach (String word in words.Keys)
            {
                double sum = 0;  // Total sum of all the occurences of each followup word

                // Update the probabilities
                foreach (Word s in words[word])
                {
                    s.Probability = (double)s.Count / sums[word];
                }

            }
        }
    }
}
