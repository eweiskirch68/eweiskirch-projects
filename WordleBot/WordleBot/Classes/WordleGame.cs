namespace Solution.Classes
{
    /*
     * A class for the game of wordle.
     * Separate from the bot and allows the user to play the game.
     */
    public class WordleGame
    {

        public string solution;
        public string guess;

        //new instance of the game with only a solution and not a guess
        public WordleGame(string solution)
        {
            this.solution = solution;
        }

        //new instance of the game with a solution and a guess
        public WordleGame(string solution, string guess)
        {
            this.solution = solution;
            this.guess = guess;
        }

        //enters a guess
        public void enterGuess(string guess)
        {
            this.guess = guess;
        }

        //compares the guess with the solution and returns an array of colors representing how correct each letter was
        public char[] compareGuess()
        {
            //the comparison of the guess and solution is represented by an array of five characters (g = grey, y = yellow, G = Green)
            char[] colors = { 'g', 'g', 'g', 'g', 'g' };

            //iterates through solution and guess and compares to turn correct characters green
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == j && this.guess[i] == this.solution[j])
                    {
                        colors[i] = 'G';
                        break;
                    }

                }
            }

            //alphabet dictionary (key = 'letter', value = # of occurrences in solution)
            Dictionary<char, int> alphabetMap = new Dictionary<char, int>();
            for (int i = 0; i < 5; i++)
            {
                char letter = this.solution[i];
                if (!alphabetMap.ContainsKey(letter))
                {
                    alphabetMap[letter] = 1;
                }
                else
                {
                    alphabetMap[letter]++;
                }
            }

            //comparing the alphabet map to the guess to determine how many of each letter are already green to decrement to the number of remaining yellow spots
            for (int i = 0; i < 5; i++)
            {
                char letter = this.guess[i];
                if (alphabetMap.ContainsKey(letter) && alphabetMap[letter] > 0 && colors[i] == 'G')
                {
                    alphabetMap[letter]--;
                }
            }
            //comparing the alphabet map to the guess and turning any characters yellow if there are still spots.
            for (int i = 0; i < 5; i++)
            {
                char letter = this.guess[i];
                if (alphabetMap.ContainsKey(letter) && alphabetMap[letter] > 0 && colors[i] != 'G')
                {
                    colors[i] = 'y';
                    alphabetMap[letter]--;
                }
            }
            return colors;
        }
    }
}
