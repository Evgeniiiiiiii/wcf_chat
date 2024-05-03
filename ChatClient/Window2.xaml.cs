using ChatClient.ServiceChat;
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
using System.Windows.Shapes;

namespace ChatClient
{
	/// <summary>
	/// Логика взаимодействия для Window2.xaml
	/// </summary>
	public partial class Window2 : Window, IServiceChatCallback
	{
		bool isConnected = false;
		ServiceChatClient client;
		int ID;
		public Window2(string name)
		{
			InitializeComponent();
			TextBoxUserName.Text = name;
			TextBoxUserName.IsEnabled = false;
			//ID = id;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
		}

		void ConnectUser()
		{
			if (!isConnected)
			{
				client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
				TextBoxUserName.IsEnabled = false;
				ID = client.Connect(TextBoxUserName.Text);
				ButtonConnectDisconnect.Content = "Disconnect";
				isConnected = true;
			}
		}

		void DisconnectUser()
		{
			if (isConnected)
			{
				client.Disconnect(ID);
				client = null;
				//TextBoxUserName.IsEnabled = true;
				ButtonConnectDisconnect.Content = "Connect";
				isConnected = false;
			}

		}


		public void MsgCallback(string msg)
		{
			ListBoxChat.Items.Add(msg);
			ListBoxChat.ScrollIntoView(ListBoxChat.Items[ListBoxChat.Items.Count - 1]);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DisconnectUser();
		}

		private void ButtonConnectDisconnect_Click(object sender, RoutedEventArgs e)
		{
			if (isConnected)
			{
				DisconnectUser();
			}
			else
			{
				ConnectUser();
			}
		}
		private void TextBoxMessage_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (string.IsNullOrWhiteSpace(TextBoxMessage.Text))
				{ return; }
				if (client != null)
				{
					
					client.SendMsg(TextBoxMessage.Text, ID);
					TextBoxMessage.Text = string.Empty;
				}
			}
		}

		private void ListBoxChat_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		

		private void ButtonExit_Click(object sender, RoutedEventArgs e)
		{
			DisconnectUser();
			Close();
		}

		private void TextBoxUserName_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}
}
