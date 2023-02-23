namespace Solution.Classes
{
    /*
     * An exception for handling user input while playing Wordle Game to make sure the user inputs 5 letter words.
     */
    public class WordleGuessException : Exception
    {
        public WordleGuessException(string message)
            : base(message)
        {
        }
    }
}
