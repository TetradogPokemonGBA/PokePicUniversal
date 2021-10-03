using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PokemonGBAFramework.Core;
using Gabriel.Cat.S.Extension;

namespace PokePic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        
        public MainWindow()
        {
          
            InitializeComponent();
            visorPokemon.Main = this;
            Load(LastRom);
        }
        public RomGba Rom { get; set; }

        public bool IsReadOnly { get => !visorPokemon.ImportOn; set => visorPokemon.ImportOn = !value; }
        public string LastRom
        {
            get => Properties.Settings.Default.LastRom;
            set
            {
                Properties.Settings.Default.LastRom = value;
                Properties.Settings.Default.Save();
            }
        }
        public void Save() => Rom.Data.Bytes.Save(LastRom);

        private void miCargarRom_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opnFile = new OpenFileDialog();
            opnFile.Filter = "GBA|*.gba";
            if (opnFile.ShowDialog().GetValueOrDefault())
            {
                Load(opnFile.FileName);
                LastRom = opnFile.FileName;

            }
            else MessageBox.Show("No se ha cargado nada nuevo");
        }
        void Load(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                Rom = new RomGba(fileName);
                lstPokemon.Items.Clear();
                lstPokemon.Items.AddRange(Pokemon.GetOrdenNacional(Rom));
                lstPokemon.SelectedIndex = 1;
            }
        }

        private void lstPokemon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            visorPokemon.Pokemon = lstPokemon.SelectedItem as Pokemon;
            visorPokemon.Refresh();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Border border;
            ScrollViewer scrollViewer;

            if (e.Key.Equals(Key.LeftCtrl) || e.Key.Equals(Key.RightCtrl))
            {
                IsReadOnly = !IsReadOnly;       
            }

            else if (e.Key.Equals(Key.Down) || e.Key.Equals(Key.NumPad2))
            {
                if (lstPokemon.SelectedIndex < lstPokemon.Items.Count - 1)
                {
                    lstPokemon.SelectedIndex++;
                    lstPokemon.ScrollIntoView(lstPokemon.Items[lstPokemon.SelectedIndex]);
                }
                else
                {
                    lstPokemon.SelectedIndex = 0;
                    border = (Border)VisualTreeHelper.GetChild(lstPokemon, 0);
                    scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                    scrollViewer.ScrollToTop();
                }


            }
            else if (e.Key.Equals(Key.Up) || e.Key.Equals(Key.NumPad8))
            {
                if (lstPokemon.SelectedIndex <= 0)
                {
                    lstPokemon.SelectedIndex = lstPokemon.Items.Count - 1;
                    border = (Border)VisualTreeHelper.GetChild(lstPokemon, 0);
                    scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                    scrollViewer.ScrollToBottom();
                }
                else
                {
                    lstPokemon.SelectedIndex--;
                    lstPokemon.ScrollIntoView(lstPokemon.Items[lstPokemon.SelectedIndex]);

                }


            }
            else if (e.Key.Equals(Key.Left) || e.Key.Equals(Key.NumPad4))
            {
                visorPokemon.Pic--;
                visorPokemon.Refresh();
            }
            else if (e.Key.Equals(Key.Right) || e.Key.Equals(Key.NumPad6))
            {
                visorPokemon.Pic++;
                visorPokemon.Refresh();
            }
        }
    }
}
