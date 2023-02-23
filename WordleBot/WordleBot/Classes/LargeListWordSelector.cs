using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Classes
{
    public class LargeListWordSelector
    {
        //List of remaining possible words to select from
        public List<string> RemainingWords { get; set; }

        //Dictionary mapping each word to a score based on how good of a guess it is
        public Dictionary<string, double> WordScores { get; set; }
        
        
        /*
         * WEIGHTS - To adjust in the future to optimize the word selection
         */

        //The threshold for how often a double occurs to be considered a common double.
        public double commonDoubleThreshold = 0.01;

        //The number of words that should be considered when looking at the "best words" for things like doubles
        public int numBestWords = 10;

        //The score multiplier when a word contains a double letter and is above the doublesNegativeThreshold
        public double positiveDoublesMultiplier = 2.0;

        //The score multiplier when a word contains a double letter and is under the doublesNegativeThreshold
        public double negativeDoublesMultiplier = -2.0;

        //The threshold number of remaining total words to start considering double letters a negative influence.
        public int doublesNegativeThreshold = 1000;

        //The threshold number of remaining total words to stop considering doubles at all.
        public int doublesIrrelevantThreshold = 100;
        



        //Default Constructor
        public LargeListWordSelector(List<string> remainingWords)
        {
            this.RemainingWords = remainingWords;
            WordScores = new Dictionary<string, double>();
        }

        /*
         * UNSCORING - resetting each word in wordScores that is no longer a possibility to 0.
         */
        public void resetObsoleteWordScores()
        {
            foreach (KeyValuePair<string, double> wordScore in WordScores)
            {
                if (!RemainingWords.Contains(wordScore.Key)) 
                { 
                    WordScores.Remove(wordScore.Key);
                }
            }
        }

        /*
         * FIRST PASS SCORING - Methods for considering the prevalence of letters in the overall list when selecting a word.
         */

        //Gets a dictionary mapping each letter to how many times it appears in the remainingWords list
        public Dictionary<char, int> getRemainingLetterOccurrences()
        {
            //assigns each letter to 0 before adding occurences to them.
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Dictionary<char, int> remainingLetterOccurrences = new Dictionary<char, int>();
            foreach (char letter in alphabet)
            {
                remainingLetterOccurrences[letter] = 0;
            }

            //adding up all of the letters and adding the number to the dictionary
            foreach (string word in RemainingWords)
            {
                foreach (char letter in word)
                {
                    remainingLetterOccurrences[letter] += 1;
                }
            }

            return remainingLetterOccurrences;
        }

        //Calculates an individual word's score based on what letters it has, and how that corresponds with remaining letter occurrences.
        public double getInitialWordScore(string word, Dictionary<char, int> remainingLetterOccurrences)
        {
            double totalLettersInRemaining = RemainingWords.Count * 5;
            double score = 0;
            List<char> lettersUsed = new List<char>();

            //Establishing the score based on RemainingLetterOccurrences
            foreach (char letter in word)
            {
                if (!lettersUsed.Contains(letter))
                {
                    double letterScore = remainingLetterOccurrences[letter] / totalLettersInRemaining;
                    score += letterScore;
                    lettersUsed.Add(letter);
                }
            }
            return score;
        }

        //Sets all words in the remaining word list to their corresponding initial score (based on single letters)
        public void setInitialWordScores()
        {
            Dictionary<char, int> remainingLetterOccurrences = getRemainingLetterOccurrences();

            foreach (string word in RemainingWords)
            {
                WordScores[word] = getInitialWordScore(word, remainingLetterOccurrences);
            }
        }
        
        

        /*
         * SECOND PASS SCORING - Methods for considering the prevalence of letter pairs in the overall list when selecting a word.
         */

        //Gets the most common letter pairs ("doubles") in the overall word list
        public List<LetterPair> getMostCommonLetterPairs()
        {
            List<LetterPair> mostCommonLetterPairs = new List<LetterPair>();
            Dictionary<string, double> bestWords = getBestWords();

            foreach (KeyValuePair<string, double> wordScore in bestWords)
            {
                string goodWord = wordScore.Key;
                List<LetterPair> goodWordLetterPairs = new List<LetterPair>
                    {
                        new LetterPair(goodWord[0], goodWord[1], 0, 1),
                        new LetterPair(goodWord[0], goodWord[2], 0, 2),
                        new LetterPair(goodWord[0], goodWord[3], 0, 3),
                        new LetterPair(goodWord[0], goodWord[4], 0, 4),
                        new LetterPair(goodWord[1], goodWord[2], 1, 2),
                        new LetterPair(goodWord[1], goodWord[3], 1, 3),
                        new LetterPair(goodWord[1], goodWord[4], 1, 4),
                        new LetterPair(goodWord[2], goodWord[3], 2, 3),
                        new LetterPair(goodWord[2], goodWord[4], 2, 4),
                        new LetterPair(goodWord[3], goodWord[4], 3, 4)

                    };

                for (int i = 0; i < 10; i++)
                {
                    LetterPair goodLetterPair = goodWordLetterPairs[i];
                    (char, char) goodLetterPairLetters = (goodLetterPair.Char1, goodLetterPair.Char2);

                    foreach (string word in RemainingWords)
                    {
                        (char, char) correspondingLetterPairInWord = (word[goodLetterPair.Index1], word[goodLetterPair.Index2]);

                        if (goodLetterPairLetters == correspondingLetterPairInWord)
                        {
                            goodLetterPair.Score += 1.0 / RemainingWords.Count;
                        }
                    }
                    if (goodLetterPair.Score > commonDoubleThreshold && !mostCommonLetterPairs.Contains(goodLetterPair))
                    {
                        mostCommonLetterPairs.Add(goodLetterPair);
                    }
                }
            }
            return mostCommonLetterPairs;
        }

        //Multiplying an individual word's score based on the number of common double letters it contains.
        public double getSecondaryWordScore(string word, List<LetterPair> mostCommonLetterPairs)
        {
            double wordScore = WordScores[word];
            double doublesMultiplier;

            //Setting the multiplier depending on how many words remain
            if (RemainingWords.Count > doublesNegativeThreshold)
            {
                doublesMultiplier = positiveDoublesMultiplier;
            }
            else if (RemainingWords.Count > doublesIrrelevantThreshold)
            {
                doublesMultiplier = negativeDoublesMultiplier;
            }
            else
            {
                doublesMultiplier = 0;
            }


            //Multiplying the word's score by that multiplier based on how many doubles it contains.
            foreach (LetterPair commonLetterPairs in mostCommonLetterPairs)
            {
                (char, char) commonLetterPair = (commonLetterPairs.Char1, commonLetterPairs.Char2);
                (char, char) testedLetterPair = (word[commonLetterPairs.Index1], word[commonLetterPairs.Index2]);
                if (testedLetterPair == commonLetterPair)
                {
                    wordScore = wordScore * (1.0 + (commonLetterPairs.Score * doublesMultiplier));
                }
            }
            return wordScore;
        }

        //Sets all words in the remaining word list to their corresponding secondary score (based on double letters)
        public void setSecondaryWordScores()
        {
            List<LetterPair> mostCommonLetterPairs = getMostCommonLetterPairs();

            foreach (string word in RemainingWords)
            {
                WordScores[word] = getSecondaryWordScore(word, mostCommonLetterPairs);
            }
        }

        //Returns the best (n = numBestWords) words based on their scores
        public Dictionary<string, double> getBestWords()
        {
            Dictionary<string, double> bestWords = new Dictionary<string, double>();
            resetObsoleteWordScores();

            if (WordScores.Count > 0) 
            {
                for (int i = 0; i < numBestWords; i++)
                {
                    KeyValuePair<string, double> highestScoringWord = new KeyValuePair<string, double>( "aaaaa", 0.0 );
                    foreach (KeyValuePair<string, double> wordScore in WordScores)
                    {
                        if (wordScore.Value > highestScoringWord.Value && !bestWords.ContainsKey(wordScore.Key))
                        {
                            highestScoringWord = wordScore;
                        }
                    }
                    bestWords[highestScoringWord.Key] = highestScoringWord.Value;
                }
            }
            if (bestWords.ContainsKey("aaaaa")) bestWords.Remove("aaaaa");
            return bestWords;
        }






        /*
         * CONSOLE TESTING METHODS
         */


        public void printBestWords()
        {
            foreach (KeyValuePair<string, double> wordScore in getBestWords())
            {
                Console.WriteLine($"<{wordScore.Key}, {wordScore.Value}>");
            }
        }

        public void printMostCommonLetterPairs()
        {
            foreach (LetterPair lp in getMostCommonLetterPairs())
            {
                Console.WriteLine($"{parseRank(lp.Index1)} = {lp.Char1}, {parseRank(lp.Index2)} = {lp.Char2}. Score: {lp.Score}");
            }
        }

        public string parseRank(int index)
        {
            switch (index)
            {
                case 0:
                    return "1st";
                case 1:
                    return "2nd";
                case 2:
                    return "3rd";
                case 3:
                    return "4th";
                case 4:
                    return "5th";
            }
            return "";
        }

        public void printWordListDiffs()
        {
            foreach (string word in RemainingWords)
            {
                if (!WordScores.ContainsKey(word))
                {
                    Console.WriteLine(word);
                }
            }
        }

        public void setCustomWordScore()
        {
            Dictionary<char, int> remainingLetterOccurrences = getRemainingLetterOccurrences();
            foreach (string word in new List<string> { "tuile", "ingle", "incle", "ligne", "untie", "utile", "guile", "cline", "mulie"})
            {
                WordScores[word] = getInitialWordScore(word, remainingLetterOccurrences);
                Console.WriteLine($"{word}, {WordScores[word]}");
            }
        }

        public void printWordScores()
        {
            foreach (KeyValuePair<string, double> wordScore in WordScores)
            {
                Console.WriteLine($"<{wordScore.Key}, {wordScore.Value}>");
            }

            Console.WriteLine("Total scores: " + WordScores.Count.ToString());
        }
    }
}
