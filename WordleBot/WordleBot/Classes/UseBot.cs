using System.Runtime.CompilerServices;

namespace Solution.Classes
{   
    /*
     * Call static method UseBot.go() to start the bot.
     * Creates an instance of the bot and interfaces with the user through the console.
     */
    public class UseBot
    {
        public static void go()
        {
            bool isBotRunning = true;
            WordListTrimmer trimmer = new WordListTrimmer();
            List<string> remaining = trimmer.remainingWords;
            LargeListWordSelector selector = new LargeListWordSelector(remaining);
            selector.setInitialWordScores();
            selector.setSecondaryWordScores();
            string word;
            string colorString;
            int guesses = 0;

            while (isBotRunning)
            {
                if (remaining.Count == 0)
                {
                    isBotRunning = false;
                    Console.Write("No words exist that match those criteria");
                    continue;
                }
                
                int tries = 0;

                do
                {
                    Console.WriteLine($"Guess options are below: \n");
                    selector.printBestWords();
                    Console.WriteLine("\nWrite the word you chose: \n");
                    word = Console.ReadLine();
                    Console.WriteLine($"\nInput the game results below.\ng = grey, y = yellow, G = Green\nExample: Gggyg\n");
                    colorString = Console.ReadLine();
                    tries++;

                } while (colorString == "NA");
                
                char[] colors = colorString.ToCharArray();
                guesses++;
                if (colorString == "GGGGG")
                {
                    Console.WriteLine("\nAmazing! WordleBot won in " + guesses + " guesses!");
                    Console.WriteLine("Would you like to play again? (y/n)\n");
                    string response = Console.ReadLine();
                    if (response == "y" || response == "Y")
                    {
                        trimmer = new WordListTrimmer();
                        remaining = trimmer.remainingWords;
                        selector = new LargeListWordSelector(remaining);
                        selector.setInitialWordScores();
                        selector.setSecondaryWordScores();
                        guesses = 0;
                    }
                    else
                    {
                        isBotRunning = false;
                    }
                }
                else
                {
                    trimmer.addGuess(word, colors);
                    selector.RemainingWords = trimmer.remainingWords;
                    selector.setInitialWordScores();
                    selector.setSecondaryWordScores();
                }
                Console.WriteLine();
            }
        }
    }
}
