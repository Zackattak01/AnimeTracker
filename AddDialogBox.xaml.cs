using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace AnimeTracker
{
	/// <summary>
	/// Interaction logic for AddDialogBox.xaml
	/// </summary>
	public partial class AddDialogBox : Window
	{
		public AddDialogBox()
		{
			InitializeComponent();
			
		}

        public AddDialogBox(string title, string name, string season, string episode, string url)
		{
            InitializeComponent();
            this.Title = title;
            this.nameTextBox.Text = name;
            this.seasonCountTextBox.Text = season;
            this.episodeCountTextBox.Text = episode;
            this.urlTextBox.Text = url;
		}

        public AnimeInfo AnimeResult { get; set; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Dialog box canceled
            DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {

            AnimeResult = new AnimeInfo(this.nameTextBox.Text, this.seasonCountTextBox.Text, this.episodeCountTextBox.Text, this.urlTextBox.Text);
            // Dialog box accepted
            DialogResult = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9./]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Validate all dependency objects in a window

    }
}
