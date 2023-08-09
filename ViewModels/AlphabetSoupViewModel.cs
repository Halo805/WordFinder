using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFinder.ViewModels
{
    public class AlphabetSoupViewModel//: INotifyPropertyChanged
    {
        //ObservableCollection<string> top10Words = new ObservableCollection<string>();
        //ObservableCollection<string> wordStream = new ObservableCollection<string>();
        public AlphabetSoupViewModel()
        {
            Top10Words = new ObservableCollection<string>();
            WordStream = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Top10Words { get; set; }
        public ObservableCollection<string> WordStream { get; set; }

    }
}
