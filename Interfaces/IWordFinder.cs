using System.Collections.Generic;

namespace WordFinder.Interfaces
{
    interface IWordFinder
    {
        IEnumerable<string> Find(IEnumerable<string> wordstream);
    }
}
