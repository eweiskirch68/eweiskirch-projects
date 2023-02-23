using Solution.Classes;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


/*
 * Classes:
 * 
 * WordleBot - a class for using the color feedback provided by the game and providing the next guess for the user.
 * UseBot - a class for interfacing with WordleBot via the console.
 * WordleGame - a class for playing the game.
 * PlayWordle - a class for interfacing with the game via the console.
 * WordleGuessException - a custom exception for handling user guesses of the wrong length.
 * 
 * Call static UseBot.go() to interface with the bot.
 * Call non-static PlayWordle.playGame() to interface with the game.
 */
class Program
{
    static void Main(string[] args)
    {

        UseBot.go();

        //PlayWordle wordle = new PlayWordle();
        //wordle.playGame();


        //WordListTrimmer trimmer = new WordListTrimmer();
        //trimmer.addGuess("reias", new char[] { 'y', 'g', 'G', 'g', 'g' });
        //Console.WriteLine(trimmer.remainingWords.Contains("reias"));

        //LargeListWordSelector selector = new LargeListWordSelector(trimmer.remainingWords);
        //selector.setInitialWordScores();
        //selector.setSecondaryWordScores();
        //selector.printMostCommonLetterPairs();

    }
}