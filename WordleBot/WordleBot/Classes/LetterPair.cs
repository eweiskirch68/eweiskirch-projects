using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Classes
{
    public class LetterPair
    {
        public double Score { get; set; }

        public char Char1 { get; set; }

        public char Char2 { get; set; }

        public int Index1 { get; set; }

        public int Index2 { get; set; }


        public LetterPair(char Char1, char Char2, int Index1, int Index2)
        {
            this.Char1 = Char1;
            this.Char2 = Char2;
            this.Index1 = Index1;
            this.Index2 = Index2;
            Score = 0;
        }

        public char this[int i]
        {
            get => this.getAt(i);

            set => this.setAt(i, value);
        }

        public char getAt(int i)
        {
            if (i == Index1) return Char1;
            if (i == Index2) return Char2;
            else throw new IndexOutOfRangeException("Letter Pairs have indexes based on their corresponding word and where those letters occur in the word");
        }

        public void setAt(int i, char value)
        {
            if (i == Index1) Char1 = value;
            else if (i == Index2) Char2 = value;
            else throw new IndexOutOfRangeException("Letter Pairs have indexes based on their corresponding word and where those letters occur in the word");
        }
    }
}
