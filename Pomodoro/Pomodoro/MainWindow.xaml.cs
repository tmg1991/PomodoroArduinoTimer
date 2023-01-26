using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO.Ports;

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<int> _countDownMinutes;
        private SerialPort _serialPort;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<int> CountDownMinutes
        {
            get { return _countDownMinutes; }
            set
            {
                _countDownMinutes = value;
                NotifyPropertyChanged();
            }
        }

        private string _errorText;

        public string ErrorText
        {
            get { return _errorText; }
            set {
                _errorText = value;
                NotifyPropertyChanged();
            }
        }


        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            List<int> countDownMinutes  = new List<int>();
            string configValues = ConfigurationManager.AppSettings.Get("CountDownMinutes");
            string[] configSplits = configValues.Split(';');
            foreach (var split in configSplits)
            {
                int minute;
                if(int.TryParse(split, out minute))
                {
                    countDownMinutes.Add(minute);
                }
            }
            CountDownMinutes = countDownMinutes;

            string configCOMPort = ConfigurationManager.AppSettings.Get("COMPort");

            _serialPort = new SerialPort(configCOMPort, 9600, Parity.None);
            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                ErrorText = $"Could not open serial port on {configCOMPort}{Environment.NewLine}{ex}";
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            int minutes;
            if (int.TryParse(senderButton.Content.ToString(), out minutes))
            {
                _serialPort.Write($"t{minutes}");
            }
        }

        private void StopButtonClicked(object sender, RoutedEventArgs e)
        {
            _serialPort.WriteLine("S");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _serialPort?.Close();
        }
    }
}
