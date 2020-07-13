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
        Device device;
        public ComputerConfiguration()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (entityManager, device) = ((EntityManager, Device))e.Parameter;
            //frame.Navigate(typeof(ComputerConfigurationConsole));
        }

        private void SettignsButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(ComputerConfigurationSettings), (entityManager, device));
        }

        private void ConnectionsButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(ComputerConfigurationConnections), (entityManager, device));
        }

        private void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(ComputerConfigurationConsole), (entityManager, device));
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
