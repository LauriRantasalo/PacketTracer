using PacketTracer.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
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
    public sealed partial class ComputerConfiguration : Page
    {
        EntityManager entityManager;
        UIManager uiManager;
        public Device Device { get; set; }
        public ComputerConfiguration()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (entityManager, uiManager, Device) = ((EntityManager, UIManager, Device))e.Parameter;
            if (!uiManager.Pages.Contains(this))
            {
                uiManager.Pages.Add(this);
            }
            //frame.Navigate(typeof(ComputerConfigurationConsole));
        }

        private void SettignsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ComputerConfigurationSettings), (entityManager, uiManager, Device));
        }

        private void ConnectionsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ComputerConfigurationConnections), (entityManager, uiManager, Device));
        }

        private void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ComputerConfigurationConsole), (entityManager, uiManager, Device));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
