namespace Solution.Classes
{
    /*
     * The console interface for playing the wordle game.
     * Construct a PlayWordle object and then call .playGame() to play the game.
     */
    public class PlayWordle
    {
        public string word;
        public string[] gameWords;
        public WordleGame game;
        public string[] masterList;
        public PlayWordle()
        {
            gameWords = File.ReadAllText("gamewords.txt").ToLower().Split(" ");
            masterList = File.ReadAllLines("words.txt");
            Random random = new Random();
            word = gameWords[random.Next(gameWords.Length)];
            game = new WordleGame(word);
        }
        public PlayWordle(string word)
        {
            this.word = word;
            masterList = File.ReadAllLines("words.txt");
            game = new WordleGame(word);
        }
        public void playGame()
        {
            //block of text to start the game and accept the first input. Uses a string literal so indentation is off
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"Welcome to Wordle!
You have six tries to guess the five letter word.
After each guess, the game will tell you the colors of the letters.
Green = Correct letter in the correct spot
Yellow = Letter is correct but out of place
Gray = Letter is incorrect

Enter your 1st guess below. (Use only 5 lowercase letters)
");

            bool isGameRunning = true;
            bool guessIsValid = false;
            string guess = "";
            int guessesMade = 0;

            //game loop that runs 6 times or until the correct word is guessed
            while (isGameRunning)
            {
                //loop that runs until the user enters a valid guess
                while (!guessIsValid)
                {
                    try
                    {
                        guess = Console.ReadLine();

                        if (guess.Length != 5) { throw new WordleGuessException("guess is not five letters long"); }
                        if (!masterList.Contains(guess))
                        {
                            throw new WordleGuessException("that isn't a valid guess.");
                        }

                        string alphabet = "abcdefghijklmnopqrstuvwxyz";

                        for (int i = 0; i < 5; i++)
                        {
                            if (!alphabet.Contains(guess[i]))
                            {
                                throw new WordleGuessException("guess contains uppercase letters or non-letter characters");
                            }
                        }
                    }
                    catch (Exception ex) when (ex is IOException ||
                                               ex is OutOfMemoryException ||
                                               ex is ArgumentOutOfRangeException ||
                                               ex is WordleGuessException)
                    {
                        if (ex.Message == "that isn't a valid guess.")
                        {
                            Console.WriteLine("\nThat doesn't match any of the five letter words I know.\n");
                            continue;
                        }
                        Console.WriteLine("\nPlease enter a five letter word with only lowercase letters.\n");
                        continue;
                    }
                    guessIsValid = true;
                }
                guessesMade++;
                game.enterGuess(guess);


                //ends the game if the guess is correct
                if (guess == game.solution)
                {
                    Console.WriteLine("\nThat's correct!\nFinished in " + guessesMade + " guesses.");
                    isGameRunning = false;
                    continue;
                }//ends the game if the player has guessed 6 times.
                else if (guessesMade == 6)
                {
                    Console.WriteLine("\nIncorrect! The word was " + game.solution);
                    isGameRunning = false;
                    continue;
                }


                //printing out the letters in colors based on their state
                char[] colors = game.compareGuess();

                for (int i = 0; i < 5; i++)
                {
                    switch (colors[i])
                    {
                        case ('g'):
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case ('y'):
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case ('G'):
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                    }
                    Console.Write(guess[i]);
                }
                Console.ForegroundColor = ConsoleColor.White;

                guessIsValid = false;

                //guess rank helps prepare a custom message for each number for the user to keep track of which guess they are on.
                string guessRank = "1st";
                switch (guessesMade)
                {
                    case 1:
                        guessRank = "2nd";
                        break;
                    case 2:
                        guessRank = "3rd";
                        break;
                    case 3:
                        guessRank = "4th";
                        break;
                    case 4:
                        guessRank = "5th";
                        break;
                    case 5:
                        guessRank = "6th";
                        break;
                }


                Console.WriteLine("\n\nEnter your " + guessRank + " guess below\n");
            }
        }
    }
}
