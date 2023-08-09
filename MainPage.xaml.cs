using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WordFinder.Interfaces;
using WordFinder.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WordFinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int Rows = 0;
        int Columns = 0;
        
        List<string> Matrix;        
        List<TextBox> TxtsToDraw;
        List<TextBlock> LblInMatrix;

        //Emulate the ViewModel
        //public ObservableCollection<string> Top10Words = new ObservableCollection<string>();
        //public ObservableCollection<string> WordStream = new ObservableCollection<string>();
        public AlphabetSoupViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            /* test Dummy Data
            IEnumerable<string> list = new List<string>();
            IWordFinder wf = new WordFinder.Data.WordFinder(true);

            list = new string[] { "cold", "wind", "snow", "chill" };
            //list.Append("cold");
            //list.Append("wind");
            //list.Append("snow");
            //list.Append("chill");

            IEnumerable<string> foundWords = wf.Find(list);
            */

            //listTop10Words.ItemsSource = Top10Words;
            //listWordsToFind.ItemsSource = WordStream;
            ViewModel = new AlphabetSoupViewModel();

        }

        private async void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            int rows = 0;
            int columns = 0;

            string errorMessage = "";

            int.TryParse(this.txtRows.Text, out rows);
            int.TryParse(this.txtColumns.Text, out columns);

            if (rows <= 0 || rows > 64)
            {
                errorMessage += "The Maxtrix only accepts a number between 1 and 64 for rows\n";
            }
            if (columns <= 0 || columns > 64)
            {
                errorMessage += "The Maxtrix only accepts a number between 1 and 64 for columns\n";
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                //Create Matrix
                DrawMatrix(rows, columns);

                
            }
            else
            {
                var messageDialog = new MessageDialog(errorMessage);
                await messageDialog.ShowAsync();
            }

        }

        private void DrawMatrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            TxtsToDraw = new List<TextBox>();
            Matrix = new List<string>();
            LblInMatrix = new List<TextBlock>();

            ClearGridMatrix();
            
            for (int i = 0; i < rows; i++)
            {
                //Add Rows
                RowDefinition r = new RowDefinition();
                r.Height = new GridLength(40);

                grdMatrix.RowDefinitions.Add(r);

                for (int j = 0; j < columns; j++)
                {
                    //Add Columns
                    ColumnDefinition c = new ColumnDefinition();
                    //c.Width = new GridLength(40);
                    c.Width = new GridLength(1, GridUnitType.Star);
                    grdMatrix.ColumnDefinitions.Add(c);

                    Border cell = new Border();
                    cell.BorderThickness = new Thickness(1);
                    cell.BorderBrush = new SolidColorBrush(Colors.Black);
                    cell.Width = 40;
                    cell.Height = 40;
                    cell.VerticalAlignment = VerticalAlignment.Center;
                    cell.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);


                    //Add the other elements
                    TextBlock lbl = new TextBlock();
                    lbl.Name = string.Format("lbl_{0}_{1}", i, j);
                    //lbl.Height = 35;
                    //lbl.Width = 35;
                    lbl.Text = "X";
                    //lbl.VerticalAlignment = VerticalAlignment.Center;
                    //Grid.SetRow(lbl, i);
                    //Grid.SetColumn(lbl, j);
                    //grdMatrix.Children.Add(lbl);
                    cell.Child = lbl;
                    grdMatrix.Children.Add(cell);

                    LblInMatrix.Add(lbl);
                }

                //Add the txt to build the Matrix's Row
                ColumnDefinition cAux = new ColumnDefinition();
                //cAux.Width = GridLength.Auto;
                //cAux.Width = new GridLength(100, GridUnitType.Pixel);
                cAux.Width = new GridLength(3, GridUnitType.Star);
                //cAux.Width = new GridLength(60);
                //cAux.Width = new GridLength();
                //cAux.Width = GridLength.Auto;
                grdMatrix.ColumnDefinitions.Add(cAux);

                Border cellAux = new Border();
                cellAux.BorderThickness = new Thickness(1);
                //cellAux.BorderBrush = new SolidColorBrush(Colors.Black);
                cellAux.Width = 150;
                cellAux.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(cellAux, i);
                Grid.SetColumn(cellAux, columns + 1);

                TextBox txt = new TextBox();
                txt.Height = 30;
                //txt.Width = 9000;
                txt.FontSize = 10;
                txt.PlaceholderText = string.Format("Type {0} character{1}", columns, (columns > 1 ? "s" : ""));
                txt.Name= string.Format("txt_{0}", i);
                //Grid.SetRow(txt, i);
                //Grid.SetColumn(txt, columns+1);
                cellAux.Child = txt;
                grdMatrix.Children.Add(cellAux);

                TxtsToDraw.Add(txt);
            }

            //Disabe and enable controls
            btnReset.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnDraw.IsEnabled = false;
            txtRows.IsEnabled = false;
            txtColumns.IsEnabled = false;
        }

        private void ClearGridMatrix()
        {
            //Clear the Grid Matrix
            grdMatrix.RowDefinitions.Clear();
            grdMatrix.ColumnDefinitions.Clear();
            grdMatrix.Children.Clear();
        }

        private void txtRows_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(char.IsDigit).ToArray());
        }

        private void txtColumns_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = new String(sender.Text.Where(char.IsDigit).ToArray());
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearGridMatrix();

            TxtsToDraw = new List<TextBox>();
            Matrix = new List<string>();
            LblInMatrix = new List<TextBlock>();

            //Disabe and enable controls
            btnReset.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnDraw.IsEnabled = true;
            txtRows.IsEnabled = true;
            txtColumns.IsEnabled = true;
            btnAddWord.IsEnabled = false;
            txtWord.IsEnabled = false;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            string idStr;
            string value;
            string strForMatrix;

            int id;
            //var v = from a in grdMatrix.Children where a.Na;

            //foreach (var item in grdMatrix.Children) 
            //{
            //    var vv = 0;
            //}
            foreach (TextBox txt in TxtsToDraw)
            {
                idStr = txt.Name.ToLower().Replace("txt_","");
                value = txt.Text.ToUpper().Replace(" ","");

                if (value.Length > Columns)
                {
                    errorMessage += String.Format("The word in position {0} cannot be greater than {1} characters\n", idStr, Columns);
                    txt.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    int.TryParse(idStr, out id);

                    strForMatrix = value.PadRight(Columns,'X');
                    txt.Text = strForMatrix;

                    Matrix.Add(strForMatrix);

                    int position = 0;
                    foreach (char letter in strForMatrix.ToCharArray())
                    {
                        TextBlock lbl = LblInMatrix.Where(x => x.Name.Equals(string.Format("lbl_{0}_{1}", id, position))).FirstOrDefault();
                        if (lbl != null)
                        {
                            lbl.Text = letter.ToString();
                        }
                        position++;
                    }

                    //txt.IsEnabled = false;
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                var messageDialog = new MessageDialog(errorMessage);
                await messageDialog.ShowAsync();

            }
            else 
            {
                //btnSave.IsEnabled = false;
                btnReset.IsEnabled = true;
                btnAddWord.IsEnabled = true;
                txtWord.IsEnabled = true;
            }
        }

        private async void btnAddWord_Click(object sender, RoutedEventArgs e)
        {
            //WordStream
            string word = txtWord.Text.ToUpper().Replace(" ","");

            if (!string.IsNullOrEmpty(word))
            {
                if (!ViewModel.WordStream.Contains(word))
                {
                    ViewModel.WordStream.Add(word);
                }

                btnSave.IsEnabled = false;
                btnSearh.IsEnabled = true;
                txtWord.Text = "";
            }
            else
            {
                var messageDialog = new MessageDialog("Add a word");
                await messageDialog.ShowAsync();
            }
        }

        private void btnSearh_Click(object sender, RoutedEventArgs e)
        {
            IWordFinder finder = new WordFinder.Data.WordFinder(Matrix);
            //ViewModel.Top10Words =new ObservableCollection<string>(finder.Find(ViewModel.WordStream).Cast<string>());
            var toplstTemp = finder.Find(ViewModel.WordStream);
            foreach (string str in toplstTemp)
            {
                ViewModel.Top10Words.Add(str);
            }
        }
    }
}
