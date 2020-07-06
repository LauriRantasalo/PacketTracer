using PacketTracer.Cables;
using PacketTracer.Devices;
using System;
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
        EntityManager entityManager;
        string[] commands = {"ping"};
        public ComputerConfigurationConsole()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            entityManager = (EntityManager)e.Parameter;
            base.OnNavigatedTo(e);
        }
        private void ConsoleInputText_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (txtBox.Text.Length > 0)
                {
                    ConsoleTextBlock.Text += "\n" + txtBox.Text;
                    ExecuteConsoleCommand(txtBox.Text);
                    txtBox.Text = "";

                }
            }

        }
        private void ExecuteConsoleCommand(string command)
        {
            List<string> commandParts = new List<string>();
            commandParts = command.Split(" ").ToList<string>();
            foreach (var item in commands)
            {
                if (item == commandParts[0])
                {
                    switch (item)
                    {
                        case "ping":
                            PingCommand(commandParts[1], commandParts[2]);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    ConsoleTextBlock.Text += "\n" + "Command " + commandParts[0] + " not found";
                }
            }
        }


        private void PingCommand(string sourceIp, string destinationIp)
        {
            Device sourceDevice = null;
            foreach (var device in entityManager.devices)
            {
                if (sourceDevice == null)
                {
                    foreach (var port in device.ethernetPorts)
                    {
                        if (port.ipAddress == sourceIp)
                        {
                            sourceDevice = device;
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
                
            }

            sourceDevice.SendPacket(destinationIp, sourceDevice.ethernetPorts[0]);


            sourceDevice = null;
        }
    }
}
