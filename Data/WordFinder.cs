using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordFinder.Interfaces;

namespace WordFinder.Data
{
    public class WordFinder : IWordFinder
    {
        private IEnumerable<string> horizontalmatrix;
        private IEnumerable<string> verticalmatrix;

        private IEnumerable<string> horizontalmatrixDummy;
        private IEnumerable<string> verticalmatrixDummy;
        public WordFinder(bool _useDummyData) 
        {
            if (_useDummyData)
            {
                FillDummyData();
            }
        }
        public WordFinder(IEnumerable<string> matrix)
        {
            horizontalmatrix = matrix;
            
        }

        private void FillDummyData()
        {
            horizontalmatrixDummy = new List<string>();
            verticalmatrixDummy = new List<string>();


            horizontalmatrixDummy = new string[] { "abcdc", "fgwio", "chill", "pqnsd", "uvdxy" };
            //horizontalmatrixDummy.Append("abcdc");
            //horizontalmatrixDummy.Append("fgwio");
            //horizontalmatrixDummy.Append("chill");
            //horizontalmatrixDummy.Append("pqnsd");
            //horizontalmatrixDummy.Append("uvdxy");

            verticalmatrixDummy = new string[] { "afcpu" , "bghqv", "cwind", "dilsx","coldy" };
            //verticalmatrixDummy.Append("afcpu");
            //verticalmatrixDummy.Append("bghqv");
            //verticalmatrixDummy.Append("cwind");
            //verticalmatrixDummy.Append("dilsx");
            //verticalmatrixDummy.Append("coldy");
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            IEnumerable<string> foundwords = new List<string>();
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
            
            foreach (string word in wordstream)
            {
                //var listofwordstemp = (from a in horizontalmatrixDummy
                //                       where a.Contains(word)
                //                       select a).ToList();

                //if (listofwordstemp != null && listofwordstemp.Count > 0)
                //{ 
                //}

                //var top10 = objectList.OrderByDescending(o => o.Frequency).Take(10);

                FindWordInHorizontalmatrix(wordFrequency, word);
                FindWordInVerticalmatrixmatrix(wordFrequency, word);                
            }

            if (wordFrequency != null && wordFrequency.Count > 0)
            {
                //foundwords = wordFrequency.Select(x=>x.Key).OrderByDescending(x => x.Value).Take(10).ToList();
                foundwords = (from a in wordFrequency
                              where a.Value > 0
                              orderby a.Value descending
                              select a.Key).Take(10).ToList();
            }

            return foundwords;
        }

        private void FindWordInHorizontalmatrix(Dictionary<string, int> wordFrequency, string word)
        {
            int count = horizontalmatrixDummy.Count(x => x.Contains(word));
            if (wordFrequency.ContainsKey(word))
            {
                wordFrequency[word] = count + (int)wordFrequency[word];
            }
            else
            {
                wordFrequency.Add(word, count);
            }
        }

        private void FindWordInVerticalmatrixmatrix(Dictionary<string, int> wordFrequency, string word)
        {
            int count = verticalmatrixDummy.Count(x => x.Contains(word));
            if (wordFrequency.ContainsKey(word))
            {
                wordFrequency[word] = count + (int)wordFrequency[word];
            }
            else
            {
                wordFrequency.Add(word, count);
            }
        }
    }
}
