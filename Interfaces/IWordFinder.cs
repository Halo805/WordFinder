using System.Collections.Generic;

namespace WordFinder.Interfaces
{
    interface IWordFinder
    {
        IEnumerable<string> Find(IEnumerable<string> wordstream);

        void FindPositionInMatrix(string _word, ref int _x, ref int _y, ref int _start, ref int _end);
    }
}
