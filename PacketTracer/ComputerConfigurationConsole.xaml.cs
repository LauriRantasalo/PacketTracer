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
        Device device;
        public ComputerConfigurationConsole()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            (entityManager, device) = ((EntityManager, Device))e.Parameter;
            ConsoleInputText.Text = device.Name;
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
                    ConsoleTextBlock.Text += "\n" + device.Terminal.ExecuteCommand(txtBox.Text);
                    txtBox.Text = "";

                }
            }

        }
       


        private void PingCommand(string sourceIp, string destinationIp)
        {
            Device sourceDevice = null;
            foreach (var device in entityManager.Devices)
            {
                if (sourceDevice == null)
                {
                    foreach (var port in device.EthernetPorts)
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
            // ConsoleTextBlock.Text += "\n" +
           sourceDevice.SendPacket(destinationIp, sourceDevice.EthernetPorts[0]);
        }
    }
}
