using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Classes
{
    public class WordListTrimmer
    {
        //master list of possible words
        public string[] WORD_LIST = File.ReadAllLines("words.txt");

        //list the game keeps track of to determine its guess by eliminating ones that aren't possible anymore
        public List<string> remainingWords { get; set; }

        //A dictionary of guesses already made in the format of (key = guess, value = [array of corresponding colors in char format])
        public Dictionary<string, char[]> guessesMade { get; set; }

        //a dictionary with each letter in the alphabet to determine if it is part of the word or not (ie 0 = "greyed out", each value starts at 5 because its possible for 5 of each letter to exist)
        public Dictionary<char, int> maxOccurrencesPerLetter { get; set; }

        //a dictionary mapping each non-grey letter in previous guesses to the maximum number of times that letter wasn't grey.
        public Dictionary<char, int> totalGuessOccurrences { get; set; }

        //A list of letters that are green/yellow in one spot and grey in another.
        List<char> multicoloredLetters { get; set; } = new List<char>();

        //an int representing how many entries should  program should run all of it's min-maxing capabilities.
        public int minMaxThreshold { get; } = 500;

        //A rating for each word based on how likely it should be guessed.
        public Dictionary<string, double> wordScores;
        public WordListTrimmer()
        {
            remainingWords = new List<string>();
            guessesMade = new Dictionary<string, char[]>();
            maxOccurrencesPerLetter = new Dictionary<char, int>();
            totalGuessOccurrences = new Dictionary<char, int>();

            //adding all words from the master list to the remaining words possible.
            for (int i = 0; i < this.WORD_LIST.Length; i++)
            {
                remainingWords.Add(this.WORD_LIST[i]);
            }

            //adding all letters in the alphabet to the maxOccurrencesPerLetter array with value 5
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < alphabet.Length; i++)
            {
                maxOccurrencesPerLetter[alphabet[i]] = 5;
            }
        }

        //Constructs a bot that operates with a specific List of words.
        public WordListTrimmer(List<String> customWordList)
        {
            remainingWords = customWordList;
            guessesMade = new Dictionary<string, char[]>();
            maxOccurrencesPerLetter = new Dictionary<char, int>();
            totalGuessOccurrences = new Dictionary<char, int>();
            wordScores = new Dictionary<string, double>();

            //adding all letters in the alphabet to the maxOccurrencesPerLetter array with value 5
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < alphabet.Length; i++)
            {
                maxOccurrencesPerLetter[alphabet[i]] = 5;
            }
        }



        /*
         * ------------------------------------------------LIST-TRIMMING METHODS:--------------------------------------
         * (trimming the master list of words)
         */

        //adding a guess to the guessesMade Dictionary
        public void addGuess(string guess, char[] colors)
        {
            guessesMade[guess] = colors;
            trimList();
        }

        //changing totalGuessOccurrences to reflect the results of the last guess.
        public void addGuessOccurrences()
        {
            string guess = guessesMade.Keys.Last();
            char[] colors = guessesMade[guess];
            Dictionary<char, int> guessOccurrences = new Dictionary<char, int>();

            //creating guessOccurrences, a dictionary that maps non-grey letters in guess to their number of non-grey occurrences in guess
            for (int i = 0; i < 5; i++)
            {
                if (colors[i] != 'g')
                {
                    if (!guessOccurrences.ContainsKey(guess[i]))
                    {
                        guessOccurrences[guess[i]] = 1;
                    }
                    else
                    {
                        guessOccurrences[guess[i]]++;
                    }
                }
            }

            //merge guessOccurrences to totalGuessOccurrences by finding all of the keys that match, then using the one with more occurrences in totalGuessOccurrences
            foreach (KeyValuePair<char, int> pair in guessOccurrences)
            {
                char letter = pair.Key;
                if ((totalGuessOccurrences.ContainsKey(letter) && totalGuessOccurrences[letter] < guessOccurrences[letter]) || !totalGuessOccurrences.ContainsKey(letter))
                {
                    totalGuessOccurrences[letter] = guessOccurrences[letter];
                    //Upgrade - add capability to detect when a letter pops up green but in different locations in two different words 
                }
            }
        }

        //If a letter is grey, sets value in maxOccurrencesPerLetter to 0
        public void eliminateGreyLettersFromMaxOccurrencesPerLetter()
        {
            string guess = guessesMade.Keys.Last();
            char[] colors = guessesMade[guess];
            multicoloredLetters = new List<char>();
            int greyCounter = 0;

            for (int i = 0; i < 5; i++)
            {
                if (colors[i] == 'g')
                {
                    greyCounter++;
                    bool duplicateLetterIsNotGrey = false;
                    //check to see if it is a double letter with one that is already green or yellow
                    for (int j = 0; j < 5; j++)
                    {
                        if (guess[i] == guess[j] && i != j && colors[j] != 'g')
                        {
                            duplicateLetterIsNotGrey = true;
                            multicoloredLetters.Add(guess[i]);
                        }
                    }
                    if (!duplicateLetterIsNotGrey)
                    {
                        maxOccurrencesPerLetter[guess[i]] = 0;
                    }
                }
            }
        }

        //Returns a dictionary mapping a letter to the number of times it was grey in the most recent word.
        public Dictionary<char, int> getGreyGuessOccurrences()
        {
            string guess = guessesMade.Keys.Last();
            char[] colors = guessesMade[guess];

            Dictionary<char, int> greyGuessOccurrences = new Dictionary<char, int>();

            for (int i = 0; i < 5; i++)
            {
                if (colors[i] == 'g')
                {
                    if (!greyGuessOccurrences.ContainsKey(guess[i]))
                    {
                        greyGuessOccurrences[guess[i]] = 1;
                    }
                    else
                    {
                        greyGuessOccurrences[guess[i]]++;
                    }
                }
            }
            return greyGuessOccurrences;
        }

        //Returns an int with the number of non-grey letters in all previous words.
        public int getNonGreyCounter()
        {
            int nonGreyCounter = 0;
            foreach (KeyValuePair<char, int> pair in totalGuessOccurrences)
            {
                nonGreyCounter += pair.Value;
            }
            return nonGreyCounter;
        }

        //Altering maxOccurrencesPerLetter to reflect the result of a guess.
        public void trimMaxOccurrencesPerLetter()
        {
            eliminateGreyLettersFromMaxOccurrencesPerLetter();

            string guess = guessesMade.Keys.Last();
            char[] colors = guessesMade[guess];
            Dictionary<char, int> greyGuessOccurrences = getGreyGuessOccurrences();
            int nonGreyCounter = getNonGreyCounter();


            //setting the letters status to the corresponding value in maxOccurrencesPerLetter plus (5-nonGreyCounter) (max possible number for that letter in the solution)
            for (int i = 0; i < 5; i++)
            {
                //greySubtractor represents the number of greyed occurrences for a given letter that is also green somewhere else. It is subtracted from maxOccurrencesPerLetter when relevent
                int greySubtractor = 0;
                if (greyGuessOccurrences.ContainsKey(guess[i]))
                {
                    foreach (KeyValuePair<char, int> pair in greyGuessOccurrences)
                    {
                        if (pair.Key == guess[i])
                        {
                            greySubtractor += greyGuessOccurrences[guess[i]];
                        }
                    }
                }
                if (totalGuessOccurrences.ContainsKey(guess[i]) && (totalGuessOccurrences[guess[i]] + (5 - nonGreyCounter - greySubtractor)) < maxOccurrencesPerLetter[guess[i]])
                {

                    maxOccurrencesPerLetter[guess[i]] = (totalGuessOccurrences[guess[i]] + (5 - nonGreyCounter - greySubtractor));
                }
            }

            //setting letters that aren't greyed out OR in guess to (5-nonGreyCounter)
            Dictionary<char, int>.KeyCollection alphabetKeys = maxOccurrencesPerLetter.Keys;
            char[] alphabetArray = alphabetKeys.ToArray();

            for (int i = 0; i < alphabetArray.Length; i++)
            {
                if (maxOccurrencesPerLetter.ElementAt(i).Value != 0 && !guess.Contains(alphabetArray[i]))
                {
                    maxOccurrencesPerLetter[maxOccurrencesPerLetter.ElementAt(i).Key] = (5 - nonGreyCounter);
                }
                if (multicoloredLetters.Contains(maxOccurrencesPerLetter.ElementAt(i).Key))
                {
                    maxOccurrencesPerLetter[maxOccurrencesPerLetter.ElementAt(i).Key] = maxOccurrencesPerLetter[alphabetArray[i]] = totalGuessOccurrences[maxOccurrencesPerLetter.ElementAt(i).Key];
                }
            }
        }

        //remove all words from the list that aren't possible because they contain letters that are greyed out.
        public void eliminateImpossibleWordsBasedOnMaxOccurrences()
        {
            List<string> wordsToRemove = new List<string>();
            foreach (string word in remainingWords)
            {
                Dictionary<char, int> wordOccurrences = new Dictionary<char, int>();
                for (int i = 0; i < 5; i++)
                {
                    if (!wordOccurrences.ContainsKey(word[i]))
                    {
                        wordOccurrences[word[i]] = 1;
                    }
                    else
                    {
                        wordOccurrences[word[i]]++;
                    }

                }
                foreach (KeyValuePair<char, int> pair in wordOccurrences)
                {
                    if (pair.Value > maxOccurrencesPerLetter[pair.Key] && !wordsToRemove.Contains(word))
                    {
                        wordsToRemove.Add(word);
                    }
                }
            }
            List<string> remainingWordsPlaceholder = new List<string>();
            foreach (string word in remainingWords)
            {
                if (!wordsToRemove.Contains(word))
                {
                    remainingWordsPlaceholder.Add(word);
                }
            }
            remainingWords = remainingWordsPlaceholder;
        }

        //remove all words from the list that aren't possible because they don't have letters that are either green or yellow
        public void eliminateImpossibleWordsBasedOnCorrectLetters()
        {
            List<string> remainingWordsPlaceholder = new List<string>();

            foreach (string word in remainingWords)
            {
                Dictionary<char, int> wordOccurrences = new Dictionary<char, int>();
                for (int i = 0; i < 5; i++)
                {
                    if (!wordOccurrences.ContainsKey(word[i]))
                    {
                        wordOccurrences[word[i]] = 1;
                    }
                    else
                    {
                        wordOccurrences[word[i]]++;
                    }

                }

                bool passesTest = true;
                foreach (KeyValuePair<char, int> pair in totalGuessOccurrences)
                {
                    if (!wordOccurrences.ContainsKey(pair.Key) || (wordOccurrences.ContainsKey(pair.Key) && wordOccurrences[pair.Key] < pair.Value))
                    {
                        passesTest = false;
                    }
                }
                if (passesTest)
                {
                    remainingWordsPlaceholder.Add(word);
                }
            }
            remainingWords = remainingWordsPlaceholder;
        }

        //remove all words from the list that aren't possible because of positions of green letters.
        public void eliminateImpossibleWordsBasedOnWrongGreenPositions()
        {
            List<string> remainingWordsPlaceholder = new List<string>();
            string guess = guessesMade.Keys.Last();
            char[] colors = guessesMade[guess];

            foreach (string word in remainingWords)
            {
                bool passesTest = true;
                for (int i = 0; i < 5; i++)
                {
                    if (colors[i] == 'G' && guess[i] != word[i])
                    {
                        passesTest = false;
                    }
                }
                if (passesTest)
                {
                    remainingWordsPlaceholder.Add(word);
                }
            }
            remainingWords = remainingWordsPlaceholder;
        }

        //remove all words from the list that aren't possible because of positions of yellow letters.
        public void eliminateImpossibleWordsBasedOnWrongYellowPositions()
        {
            List<string> remainingWordsPlaceholder = new List<string>();
            string guess = guessesMade.Keys.Last();
            char[] colors = guessesMade[guess];

            foreach (string word in remainingWords)
            {
                bool passesTest = true;
                for (int i = 0; i < 5; i++)
                {
                    if (colors[i] == 'y' && guess[i] == word[i])
                    {
                        passesTest = false;
                    }
                }
                if (passesTest)
                {
                    remainingWordsPlaceholder.Add(word);
                }
            }
            remainingWords = remainingWordsPlaceholder;
        }

        //calls all above LIST-TRIMMING METHODS that are involved in trimming the master list of remaining words
        public void trimList()
        {
            addGuessOccurrences();
            trimMaxOccurrencesPerLetter();
            eliminateImpossibleWordsBasedOnMaxOccurrences();
            eliminateImpossibleWordsBasedOnCorrectLetters();
            eliminateImpossibleWordsBasedOnWrongGreenPositions();
            eliminateImpossibleWordsBasedOnWrongYellowPositions();
        }

        //test method (prints all remaining possible words)
        public void printRemainingToConsole()
        {
            for (int i = 0; i < remainingWords.Count; i++)
            {
                Console.WriteLine(remainingWords[i]);
            }
            Console.WriteLine("remaining entries: " + remainingWords.Count + "\n");
        }
    }
}
