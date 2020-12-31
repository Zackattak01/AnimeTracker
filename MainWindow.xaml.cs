 using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Diagnostics;

namespace AnimeTracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private Dictionary<int, List<AnimeInfo>> dict;

		public MainWindow()
		{
			InitializeComponent();
			
			Dictionary<int, List<AnimeInfo>> newDict = JsonManager.Load();

			if (newDict == null)
			{
				dict = new Dictionary<int, List<AnimeInfo>>();
				
				foreach (var item in (Status[])Enum.GetValues(typeof(Status)))
                {
					dict.Add((int)item, new List<AnimeInfo>());
				}
			}
			else
			{
				dict = newDict;
			}

			this.ListView.ItemsSource = dict[0];

			List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem>();

			foreach (var item in (Status[])Enum.GetValues(typeof(Status)))
			{
				var tmpMenuItem = new ComboBoxItem()
				{
					Content = item.Description()
				};

				comboBoxItems.Add(tmpMenuItem);
			}

			ComboBox.ItemsSource = comboBoxItems;

			ComboBox.SelectedIndex = 0;

			List<MenuItem> menuItems = new List<MenuItem>();

            foreach (var item in (Status[])Enum.GetValues(typeof(Status)))
            {
				var tmpMenuItem = new MenuItem()
				{
					Header = item.Description()
				};

				tmpMenuItem.Click += Button_Click_Move_To;

				menuItems.Add(tmpMenuItem);
			}

			menuItemMoveTo.ItemsSource = menuItems;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(this.ListView != null)
				this.ListView.ItemsSource = dict[this.ComboBox.SelectedIndex];
		}

		private void Button_Click_Add(object sender, RoutedEventArgs e)
		{
			AddDialogBox addDialogBox = new AddDialogBox();

			addDialogBox.Owner = this;

			addDialogBox.ShowDialog();

			if(addDialogBox.DialogResult == true)
			{
				dict[this.ComboBox.SelectedIndex].Add(addDialogBox.AnimeResult);
			}

			this.ListView.Items.Refresh();
			JsonManager.Save(dict);
		}

		private void Button_Click_Remove(object sender, RoutedEventArgs e)
		{
			if (ListView.SelectedValue == null)
				return;

			//AnimeInfo toRemove = dict[this.ComboBox.SelectedIndex].FirstOrDefault(x => x.ID == (ListView.SelectedValue as AnimeInfo).ID);
			dict[this.ComboBox.SelectedIndex].Remove(ListView.SelectedValue as AnimeInfo);

			this.ListView.Items.Refresh();
			JsonManager.Save(dict);
		}

		private void Button_Click_Edit(object sender, RoutedEventArgs e)
		{
			if (ListView.SelectedValue == null)
				return;

			AnimeInfo animeInfo = ListView.SelectedValue as AnimeInfo;

			AddDialogBox addDialogBox = new AddDialogBox("Edit Anime", animeInfo.Name, animeInfo.SeasonCount, animeInfo.EpisodeCount, animeInfo.Url);

			addDialogBox.Owner = this;


			addDialogBox.ShowDialog();


			if (addDialogBox.DialogResult == false)
				return;


			AnimeInfo result = addDialogBox.AnimeResult;

			animeInfo.Name = result.Name;

			animeInfo.SeasonCount = result.SeasonCount;

			animeInfo.EpisodeCount = result.EpisodeCount;

			animeInfo.Url = result.Url;


			this.ListView.Items.Refresh();
			JsonManager.Save(dict);
		}

		private void Button_Click_Up(object sender, RoutedEventArgs e)
		{
			if (ListView.SelectedValue == null)
				return;

			AnimeInfo selectedAnime = ListView.SelectedValue as AnimeInfo;

			List<AnimeInfo> animeInfo = dict[this.ComboBox.SelectedIndex];

			int index = animeInfo.FindIndex(x => x == selectedAnime) - 1;

			if(index >= 0)
			{
				animeInfo.Remove(ListView.SelectedItem as AnimeInfo);
				animeInfo.Insert(index, selectedAnime);
			}

			this.ListView.Items.Refresh();
			JsonManager.Save(dict);
		}

		private void Button_Click_Down(object sender, RoutedEventArgs e)
		{
			if (ListView.SelectedValue == null)
				return;

			AnimeInfo selectedAnime = ListView.SelectedValue as AnimeInfo;

			List<AnimeInfo> animeInfo = dict[this.ComboBox.SelectedIndex];

			int index = animeInfo.FindIndex(x => x == selectedAnime) + 1;

			if(index < animeInfo.Count)
			{
				animeInfo.Remove(ListView.SelectedItem as AnimeInfo);
				animeInfo.Insert(index, selectedAnime);
			}
			

			this.ListView.Items.Refresh();
			JsonManager.Save(dict);
		}

		private void ToolBar_Loaded(object sender, RoutedEventArgs e)
		{
			ToolBar toolBar = sender as ToolBar;
			var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
			if (overflowGrid != null)
			{
				overflowGrid.Visibility = Visibility.Collapsed;
			}
			var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
			if (mainPanelBorder != null)
			{
				mainPanelBorder.Margin = new Thickness();
			}
		}

		private void Button_Click_Watch(object sender, RoutedEventArgs e)
		{
			AnimeInfo selectedAnime = ListView.SelectedValue as AnimeInfo;
			if(ListView.SelectedValue != null)
			{
				string application = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Google\Chrome\Application\chrome.exe";
				ProcessStartInfo info = new ProcessStartInfo(application, selectedAnime.Url);
				Process.Start(info);
			}
			else
			{
				MessageBox.Show("Please select an anime");
			}
		}

		private void Button_Click_Move_To (object sender, RoutedEventArgs e)
		{
			if (ListView.SelectedValue == null)
				return;

			MenuItem menuItem = sender as MenuItem;
			int selectedList = this.ComboBox.SelectedIndex;
			int listToSendTo = -1;

			switch (menuItem.Header)
			{
				case "Currently Watching":
					listToSendTo = 0;
					break;
				case "Up To Date":
					listToSendTo = 1;
					break;
				case "Finished":
					listToSendTo = 2;
					break;
				case "Queue":
					listToSendTo = 3;
					break;
				
			}

			if (selectedList == listToSendTo)
				return;

			AnimeInfo animeToMove = ListView.SelectedValue as AnimeInfo;

			dict[selectedList].Remove(animeToMove);
			dict[listToSendTo].Add(animeToMove);

			this.ListView.Items.Refresh();
			JsonManager.Save(dict);
		}
	}

	

}
