﻿using ChatClient.ServiceChat;
using System;
using System.Collections.Generic;
using System.Globalization;
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
	/// Логика взаимодействия для Window3.xaml
	/// </summary>
	public partial class Window3 : Window, IServiceChatCallback
	{
		ServiceChatClient client;
		string DecimalSeparator => CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
		decimal FirstValue { get; set; }
		decimal? SecondValue { get; set; }
		int ID = 0;
		IOperation CurrentOperation;
		public Window3(string name)
		{
			InitializeComponent();
			btnPoint.Content = DecimalSeparator;
			btnSum.Tag = new Sum();
			btnSubtraction.Tag = new Subtraction();
			btnDivision.Tag = new Division();
			btnMultiplication.Tag = new Multiplication();
			client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
			TB.Text = name;
			TB.IsEnabled = false;
			ID = client.Connect(TB.Text);

		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
		}

		private void regularButtonClick(object sender, RoutedEventArgs e)
			=> SendToInput(((Button)sender).Content.ToString());
		private void SendToInput(string content)
		{
			//Prevent 0 from appearing on the left of new numbers
			if (txtInput.Text == "0")
				txtInput.Text = "";

			txtInput.Text = $"{txtInput.Text}{content}";
		}

		private void Console_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

        }

		private void btnPoint_Click(object sender, RoutedEventArgs e)
		{
			if (txtInput.Text.Contains(this.DecimalSeparator))
				return;

			regularButtonClick(sender, e);
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			if (txtInput.Text == "0")
				return;

			txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);
			if (txtInput.Text == "")
				txtInput.Text = "0";
		}
		private void operationButton_Click(object sender, RoutedEventArgs e)
		{
			//if current operation is not null then we already have the FirstValue
			if (CurrentOperation == null)
				FirstValue = Convert.ToDecimal(txtInput.Text);

			CurrentOperation = (IOperation)((Button)sender).Tag;
			SecondValue = null;
			txtInput.Text = "";
		}

		private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			switch (e.Text)
			{
				case "0":
				case "1":
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
					SendToInput(e.Text);
					break;

				case "*":
					btnMultiplication.PerformClick();
					break;

				case "-":
					btnSubtraction.PerformClick();
					break;

				case "+":
					btnSum.PerformClick();
					break;

				case "/":
					btnDivision.PerformClick();
					break;

				case "=":
					btnEquals.PerformClick();
					break;

				default:
					//Can't use directly from switch because it is not a constant
					if (e.Text == DecimalSeparator)
						btnPoint.PerformClick();
					else if (e.Text[0] == (char)8)
						btnBack.PerformClick();
					else if (e.Text[0] == (char)13)
						btnEquals.PerformClick();

					break;
			}

			//This will prevent other buttons focus firing its click event on <ENTER> while typing
			btnEquals.Focus();
		}
		public void MsgCallback(string msg)
		{
			Console.Items.Add(msg);
			Console.ScrollIntoView(Console.Items[Console.Items.Count - 1]);
		}
		private void btnEquals_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentOperation == null)
				return;

			if (txtInput.Text == "")
				return;
			
			//SecondValue is used for multiple clicks on Equals bringing the newest result of last operation
			decimal val2 = SecondValue ?? Convert.ToDecimal(txtInput.Text);
			try
			{
				txtInput.Text = (FirstValue = CurrentOperation.DoOperation(FirstValue, (decimal)(SecondValue = val2))).ToString();
				client.SendMsg(txtInput.Text, 0);
			}
			catch (DivideByZeroException)
			{
				MessageBox.Show("Can't divide by zero", "Divided by zero", MessageBoxButton.OK, MessageBoxImage.Error);
				btnC.PerformClick();
			}
		}

		private void btnCE_Click(object sender, RoutedEventArgs e)
		=> txtInput.Text = "0";

		private void btnC_Click(object sender, RoutedEventArgs e)
		{
			FirstValue = 0;
			CurrentOperation = null;
			txtInput.Text = "0";
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
		{

        }

		private void Grid_Loaded(object sender, RoutedEventArgs e)
		{

		}
	}
}
