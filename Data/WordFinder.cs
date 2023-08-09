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
        private int Rows = 0;
        private int Columns = 0;
        private IEnumerable<string> Horizontalmatrix;
        private IEnumerable<string> Verticalmatrix;

        private IEnumerable<string> HorizontalmatrixDummy;
        private IEnumerable<string> VerticalmatrixDummy;
        public WordFinder(bool _useDummyData) 
        {
            if (_useDummyData)
            {
                FillDummyData();
            }
        }
        public WordFinder(IEnumerable<string> matrix)
        {
            Horizontalmatrix = matrix;
            SetRows();
            SetColumns();
            FillVerticalMatrix();
        }

        private void FillVerticalMatrix()
        {
            if (Rows > 0 && Columns > 0 && Horizontalmatrix != null && Horizontalmatrix.Count() > 0)
            {
                string[] verticalMatrixArray = new string[Columns];
                

                foreach (string word in Horizontalmatrix)
                {
                    int position = 0;
                    foreach (char letter in word.ToCharArray())
                    {
                        verticalMatrixArray[position] = (verticalMatrixArray[position] != null ? verticalMatrixArray[position].ToString() + letter.ToString() : letter.ToString());
                        position++;
                    }                    
                }

                Verticalmatrix = (from a in verticalMatrixArray
                                 select a.ToString().PadRight(Rows, 'X')).ToList<string>();
            }
        }

        private void SetRows()
        {
            if (Horizontalmatrix != null && Horizontalmatrix.Count() > 0)
            {
                Rows = Horizontalmatrix.Count();
            }
        }

        private void SetColumns()
        {
            if (Horizontalmatrix != null && Horizontalmatrix.Count() > 0)
            {
                Columns = Horizontalmatrix.FirstOrDefault().Length;
            }
        }

        private void FillDummyData()
        {
            HorizontalmatrixDummy = new List<string>();
            VerticalmatrixDummy = new List<string>();

            HorizontalmatrixDummy = new string[] { "abcdc", "fgwio", "chill", "pqnsd", "uvdxy" };
            
            VerticalmatrixDummy = new string[] { "afcpu" , "bghqv", "cwind", "dilsx","coldy" };            
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
            //int count = HorizontalmatrixDummy.Count(x => x.Contains(word));
            int count = Horizontalmatrix.Count(x => x.Contains(word));
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
            //int count = VerticalmatrixDummy.Count(x => x.Contains(word));
            int count = Verticalmatrix.Count(x => x.Contains(word));
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
