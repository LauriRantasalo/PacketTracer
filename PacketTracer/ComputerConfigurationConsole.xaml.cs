﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PacketTracer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComputerConfigurationConsole : Page
    {
        public ComputerConfigurationConsole()
        {
            this.InitializeComponent();
        }
        /*
        private void ConsoleInputText_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (txtBox.Text.Length > 0)
                {
                    ConsoleTextBlock.Text += "\n" + txtBox.Text;
                    txtBox.Text = "";

                }
            }

        }
        */
    }
}