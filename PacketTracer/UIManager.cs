using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

using PacketTracer.Devices;
using System.Diagnostics;
using Windows.UI.Xaml;

namespace PacketTracer
{
    public class UIManager
    {
        public List<Page> Pages { get; set; }
        public UIManager()
        {
            Pages = new List<Page>();
        }

        public ComputerConfiguration GetComputerConfigurationWindow(Device device)
        {
            foreach (var page in Pages)
            {
                if (page.GetType() == typeof(ComputerConfiguration))
                {
                    ComputerConfiguration temp = (ComputerConfiguration)page;
                    if (temp.Device == device)
                    {
                        return temp;
                    }
                }
            }
            Debug.WriteLine("No active configuration window for device returned null");
            return null;
        }

        public async void UpdateActiveConsoleAsync(ComputerConfigurationConsole console, string value)
        {
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                //Debug.WriteLine("updating console " + value);
                console.ConsoleTextBlock.Text = value;
            });
        } 
    }
}
