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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PacketTracer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Computer> computers = new List<Computer>();

        public bool cableEditMode = false;
        Point cablePointA = new Point(-1, -1);
        Computer connectedA;
        Point cablePointB = new Point(-1, -1);
        Computer connectedB;


        public MainPage()
        {
            this.InitializeComponent();
            CreateComputers();
        }

        /// <summary>
        /// Just for early development purposes
        /// </summary>
        void CreateComputers()
        {
            Rectangle pc = new Rectangle();
            pc.Name = "pc1";
            baseCanvas.Children.Add(pc);
            pc.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            pc.Width = 40;
            pc.Height = 40;
            Canvas.SetLeft(pc, 50);
            Canvas.SetTop(pc, 50);
            pc.PointerMoved += Entity_PointerMoved;
            pc.PointerPressed += Entity_PointerPressed;
            pc.PointerReleased += Entity_PointerReleased;
            Computer temp = new Computer(pc, pc.Name, "EMTPY");
            computers.Add(temp);

            Rectangle pc2 = new Rectangle();
            pc2.Name = "pc2";
            baseCanvas.Children.Add(pc2);
            pc2.Fill = new SolidColorBrush(Windows.UI.Colors.Blue);
            pc2.Width = 40;
            pc2.Height = 40;
            Canvas.SetLeft(pc2, 200);
            Canvas.SetTop(pc2, 50);
            pc2.PointerMoved += Entity_PointerMoved;
            pc2.PointerPressed += Entity_PointerPressed;
            pc2.PointerReleased += Entity_PointerReleased;
            temp = new Computer(pc2, pc2.Name, "EMTPY");
            computers.Add(temp);


        }


        private void Entity_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint ptrPt = e.GetCurrentPoint(baseCanvas);
            Rectangle senderRect = (Rectangle)sender;

            //TODO: Find out if the flyouts could be premade so I would not need to make them in code
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
                Button consoleBTN = new Button();
                Button connectionsBTN = new Button();
                Button settingsBTN = new Button();

                nameText.Text = senderRect.Name;
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

                optionsFlyout.ShowAt(senderRect);
            }

            if (ptrPt.Properties.IsLeftButtonPressed && cableEditMode)
            {
                Computer selected = computers.Find(x => x.name == senderRect.Name);
                if (cablePointA == new Point(-1,-1))
                {
                    cablePointA = senderRect.TransformToVisual(baseCanvas).TransformPoint(new Point());
                    connectedA = selected;
                }
                else if (cablePointB == new Point(-1, -1))
                {
                    cablePointB = senderRect.TransformToVisual(baseCanvas).TransformPoint(new Point());
                    connectedB = selected;
                }

                if (cablePointA != new Point(-1, -1) && cablePointB != new Point(-1, -1))
                {
                    if (connectedA.connectedTo.TryGetValue(connectedB, out Cable tempCable))
                    {
                        Debug.WriteLine("Already connected");
                    }
                    else
                    {
                        Cable cable = new Cable(cablePointA, cablePointB);
                        cable.computerA = connectedA;
                        cable.computerB = connectedB;

                        connectedA.connectedTo.Add(connectedB, cable);
                        connectedB.connectedTo.Add(connectedA, cable);
                        baseCanvas.Children.Add(cable.line);
                    }


                    //connectedA.connectedTo.Add(connectedB);
                    //connectedB.connectedTo.Add(connectedA);

                    cablePointA = new Point(-1, -1);
                    cablePointB = new Point(-1, -1);
                    cableEditMode = false;
                    Debug.WriteLine("Exited cable edit mode");
                }

            }
            
        }

        

        private void Entity_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            PointerPoint ptrPt = e.GetCurrentPoint(baseCanvas);
            Computer selected = computers.Find(x => x.name == rect.Name);

            if (ptrPt.Properties.IsLeftButtonPressed)
            {
                Canvas.SetLeft(rect, ptrPt.Position.X - rect.Width / 2);
                Canvas.SetTop(rect, ptrPt.Position.Y - rect.Height / 2);

                if (selected.connectedTo.Count > 0)
                {
                    foreach (var connectedPair in selected.connectedTo)
                    {
                        connectedPair.Value.ReDrawLine(rect.TransformToVisual(baseCanvas).TransformPoint(new Point()), connectedPair.Key.rect.TransformToVisual(baseCanvas).TransformPoint(new Point()));
                    }
                }
            }
        }

        private void Entity_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }

        private async void OptionsBTN_Clicked(object sender, RoutedEventArgs e)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame frame = new Frame();
            frame.Navigate(typeof(ComputerConfiguration));
            ElementCompositionPreview.SetAppWindowContent(appWindow, frame);
            await appWindow.TryShowAsync();
        }

        private void CableBTN_Click(object sender, RoutedEventArgs e)
        {
            cableEditMode = true;
        }
    }
}
