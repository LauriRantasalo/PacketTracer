using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

using PacketTracer.Devices;
using PacketTracer.Cables;
using PacketTracer.Devices.Console;
// Number of commits made just to keep Github pretty: 3
// Collection of excuses to not work on this project:
// 1. Sick x3
namespace PacketTracer
{
    public sealed partial class MainPage : Page
    {
        EntityManager entityManager = new EntityManager();
        bool cableEditMode = false;
        Point cablePointA = new Point(-1, -1);
        Device connectedA;
        Point cablePointB = new Point(-1, -1);
        Device connectedB;


        public MainPage()
        {
            this.InitializeComponent();
            CreateDevices();
        }

        /// <summary>
        /// Just for early development purposes
        /// </summary>
        void CreateDevices()
        {
            Grid baseGrid = new Grid();
            Rectangle router = new Rectangle();
            TextBlock text = new TextBlock();
            router.Name = "router0";
            baseGrid.Name = "router0";
            text.Text = router.Name;
            baseGrid.Children.Add(router);
            baseGrid.Children.Add(text);
            baseCanvas.Children.Add(baseGrid);
            router.Fill = new SolidColorBrush(Windows.UI.Colors.Blue);
            baseGrid.Width = 40;
            baseGrid.Height = 40;
            Canvas.SetLeft(baseGrid, 50);
            Canvas.SetTop(baseGrid, 400);
            baseGrid.PointerMoved += Entity_PointerMoved;
            baseGrid.PointerPressed += Entity_PointerPressed;
            baseGrid.PointerReleased += Entity_PointerReleased;
            Router tempR = new Router(baseGrid, router.Name, 4);
            entityManager.Devices.Add(tempR);

            for (int i = 0; i < 3; i++)
            {
                baseGrid = new Grid();
                Rectangle pc = new Rectangle();
                text = new TextBlock();
                pc.Name = "pc" + i;
                baseGrid.Name = "pc" + i;
                text.Text = pc.Name;
                baseGrid.Children.Add(pc);
                baseGrid.Children.Add(text);
                baseCanvas.Children.Add(baseGrid);
                pc.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
                baseGrid.Width = 40;
                baseGrid.Height = 40;
                Canvas.SetLeft(baseGrid, 50 + i * 100);
                Canvas.SetTop(baseGrid, 200 + i * 100);
                baseGrid.PointerMoved += Entity_PointerMoved;
                baseGrid.PointerPressed += Entity_PointerPressed;
                baseGrid.PointerReleased += Entity_PointerReleased;
                Computer temp = new Computer(baseGrid, pc.Name, 1, "192.168.0." + (i + 1).ToString());
                entityManager.Devices.Add(temp);
            }

            foreach (var computer in entityManager.GetComputers())
            {
                EthernetCable ethernetCable = new EthernetCable(computer.BaseGrid.TransformToVisual(baseCanvas).TransformPoint(new Point(0, 0)), tempR.BaseGrid.TransformToVisual(baseCanvas).TransformPoint(new Point(0,0)));
                ethernetCable.DeviceA = computer;
                ethernetCable.DeviceB = tempR;

                computer.AddCable(ethernetCable, tempR);
                tempR.AddCable(ethernetCable, computer);
                baseCanvas.Children.Add(ethernetCable.CableLine);
            }
        }

        /// <summary>
        /// Gets run whenever an entity (computer, router, switch) is clicked on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Entity_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint ptrPt = e.GetCurrentPoint(baseCanvas);
            Grid senderBaseGrid = (Grid)sender;

            //TODO: Find out if the flyouts could be premade so I would not need to make them in code
            // Right click on entity shows a pop up menu
            if (ptrPt.Properties.IsRightButtonPressed)
            {
                Flyout optionsFlyout = new Flyout();
                Grid optionsGrid = new Grid();
                optionsFlyout.Content = optionsGrid;

                optionsGrid.RowDefinitions.Add(new RowDefinition());
                optionsGrid.RowDefinitions.Add(new RowDefinition());
                optionsGrid.RowDefinitions.Add(new RowDefinition());
                optionsGrid.RowDefinitions.Add(new RowDefinition());

                TextBlock nameText = new TextBlock();
                nameText.Name = "nameText";
                Button consoleBTN = new Button();
                Button connectionsBTN = new Button();
                Button settingsBTN = new Button();

                nameText.Text = senderBaseGrid.Name;
                consoleBTN.Content = "Console";
                connectionsBTN.Content = "Connections";
                settingsBTN.Content = "Settings";

                nameText.SetValue(Grid.RowProperty, 0);
                consoleBTN.SetValue(Grid.RowProperty, 1);
                connectionsBTN.SetValue(Grid.RowProperty, 2);
                settingsBTN.SetValue(Grid.RowProperty, 3);

                optionsGrid.Children.Add(nameText);
                optionsGrid.Children.Add(consoleBTN);
                optionsGrid.Children.Add(connectionsBTN);
                optionsGrid.Children.Add(settingsBTN);

                connectionsBTN.Click += OptionsBTN_Clicked;

                optionsFlyout.ShowAt(senderBaseGrid);
            }

            if (ptrPt.Properties.IsLeftButtonPressed && cableEditMode)
            {
                Device selected = entityManager.Devices.Find(computer => computer.Name == senderBaseGrid.Name);
                if (cablePointA == new Point(-1,-1))
                {
                    cablePointA = senderBaseGrid.TransformToVisual(baseCanvas).TransformPoint(new Point());
                    connectedA = selected;
                }
                else if (cablePointB == new Point(-1, -1))
                {
                    cablePointB = senderBaseGrid.TransformToVisual(baseCanvas).TransformPoint(new Point());
                    connectedB = selected;
                }
                // If both ends of the cable have points assinged, then create new cable
                if (cablePointA != new Point(-1, -1) && cablePointB != new Point(-1, -1))
                {
                    // Unless there already is a cable between the 2 devices

                    foreach (var port in connectedA.EthernetPorts)
                    {
                        if (port.connectedCable != null && port.connectedCable.DeviceB == connectedB)
                        {
                            Debug.WriteLine("Already connected");
                            break;
                        }
                        else
                        {
                            int deviceAFreePorts = 0;
                            int deviceBFreePorts = 0;
                            // If there are free ethernet ports on both of the devices, create the cable
                            foreach (var prt in connectedA.EthernetPorts)
                            {
                                if (prt.connectedCable == null)
                                {
                                    deviceAFreePorts++;
                                }
                            }
                            foreach (var prt in connectedB.EthernetPorts)
                            {
                                if (prt.connectedCable == null)
                                {
                                    deviceBFreePorts++;
                                }
                            }
                            if (deviceAFreePorts > 0 && deviceBFreePorts > 0)
                            {
                                EthernetCable ethernetCable = new EthernetCable(cablePointA, cablePointB);
                                ethernetCable.DeviceA = connectedA;
                                ethernetCable.DeviceB = connectedB;

                                connectedA.AddCable(ethernetCable, connectedB);
                                connectedB.AddCable(ethernetCable, connectedA);
                                baseCanvas.Children.Add(ethernetCable.CableLine);
                                break;
                            }
                            else
                            {
                                Debug.WriteLine("NO FREE PORTS ON DEVICE");
                                break;
                            }
                        }
                    }

                    // Clear all cable values
                    cablePointA = new Point(-1, -1);
                    cablePointB = new Point(-1, -1);
                    cableEditMode = false;
                    Debug.WriteLine("Exited cable edit mode");
                }

            }
            
        }

        private void Entity_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Grid senderBaseGrid = (Grid)sender;
            PointerPoint ptrPt = e.GetCurrentPoint(baseCanvas);
            Device selected = entityManager.Devices.Find(x => x.Name == senderBaseGrid.Name);
            
            
            if (ptrPt.Properties.IsLeftButtonPressed)
            {
                Canvas.SetLeft(senderBaseGrid, ptrPt.Position.X - senderBaseGrid.Width / 2);
                Canvas.SetTop(senderBaseGrid, ptrPt.Position.Y - senderBaseGrid.Height / 2);

                
                //if (selected.connectedTo.Count > 0)
                if (selected.EthernetPorts.Count > 0)
                {
                    foreach (var port in selected.EthernetPorts)
                    {
                        if (port.connectedCable != null)
                        {
                            port.connectedCable.ReDrawCable(port.connectedCable.DeviceA.BaseGrid.TransformToVisual(baseCanvas).TransformPoint(new Point()), port.connectedCable.DeviceB.BaseGrid.TransformToVisual(baseCanvas).TransformPoint(new Point()));
                        }
                    }
                }
                
            }
        }

        private void Entity_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }

        public async void OpenDeviceConfigurationWindow(string deviceName)
        {
            Device deviceToConfigure = entityManager.GetDeviceByName(deviceName);

            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame frame = new Frame();
            frame.Navigate(typeof(ComputerConfiguration), (entityManager, deviceToConfigure));
            ElementCompositionPreview.SetAppWindowContent(appWindow, frame);
            await appWindow.TryShowAsync();
        }
        private void OptionsBTN_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid parent = VisualTreeHelper.GetParent(btn) as Grid;
            foreach (FrameworkElement child in parent.Children)
            {
                if (child.Name == "nameText")
                {
                    TextBlock temp = (TextBlock)child;
                    OpenDeviceConfigurationWindow(temp.Text);
                    break;
                }
            }
        }

        private void CableBTN_Click(object sender, RoutedEventArgs e)
        {
            cablePointA = new Point(-1, -1);
            cablePointB = new Point(-1, -1);
            connectedA = null;
            connectedB = null;
            cableEditMode = true;
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
