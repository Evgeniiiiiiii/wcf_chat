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
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
	{
		public bool start = false;
        public bool isConnected = false;
        public ServiceChatClient client;
        public int ID;
        public MainWindow()
        {
            InitializeComponent();
        }
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (start == false)
			{
				ConsoleMain.Items.Add("Сначала впишите свой никнейм и подтвердите его...");

			}
			else
			{
				Window2 winodow2 = new Window2(TBUserName.Text);
				winodow2.ShowDialog();
			}
		}
		public void MsgCallback(string msg)
		{
			ConsoleMain.Items.Add(msg);
			ConsoleMain.ScrollIntoView(ConsoleMain.Items[ConsoleMain.Items.Count - 1]);
			Console.WriteLine(msg);
		}
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			client.Disconnect(ID);
			client = null;
			System.Windows.Application.Current.Shutdown();
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (start == false)
			{
				ConsoleMain.Items.Add("Сначала впишите свой никнейм и подтвердите его...");
			}
			else
			{
				Window3 window3 = new Window3(TBUserName.Text);
				window3.ShowDialog();
			}
		}

		private void TBUserName_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private ServiceChatClient GetClient()
		{
			return client;
		}
		 
		private void BUserName_Click(object sender, RoutedEventArgs e)
		{
			if(TBUserName.Text != "")
			{
				if (!isConnected)
				{
					start = true;
					BUserName.IsEnabled = false;
					TBUserName.IsEnabled = false;
					isConnected = true;
				}
			}
		}

		private void Console_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{

		}
	}
}
