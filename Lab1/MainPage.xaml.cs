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
using Windows.Devices.Gpio;
using Windows.UI.Popups;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GpioController gpio;
        GpioPin ledpin;
        DispatcherTimer timer;
        GpioPinValue ledpinvalue;

        public MainPage()
        {
            this.InitializeComponent();
            initializeGPIO();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;            
        }

        private void Timer_Tick(object sender, object e)
        {
            ledpinvalue = ledpin.Read();

            if (ledpinvalue == GpioPinValue.High)
            {
                redEllipse.Fill = new SolidColorBrush(Colors.Gray);
                ledpin.Write(GpioPinValue.Low);
            }
            else
            {
                redEllipse.Fill = new SolidColorBrush(Colors.Red);
                ledpin.Write(GpioPinValue.High);
            }
        }

        private async void initializeGPIO()
        {
            gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                startBtn.IsEnabled = false;
                MessageDialog dialog = new MessageDialog("This device doesn't have any GPIO Controller.");
                await dialog.ShowAsync();
                return;
            }

            // define and enable led pin
            ledpin = gpio.OpenPin(18);
            ledpin.SetDriveMode(GpioPinDriveMode.Output);

        }
        

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (startBtn.Content == "Start")
            {
                startBtn.Content = "Stop";
                timer.Start();
            }
            else
            {
                startBtn.Content = "Start";
                timer.Stop();
            }
        }
    }
}
