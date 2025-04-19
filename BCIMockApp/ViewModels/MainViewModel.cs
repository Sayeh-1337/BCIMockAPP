using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCIMockApp.Models;
using BCIMockApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace BCIMockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MockBCIService _bciService;
        private bool _isConnected;
        private string _connectionStatus = "Disconnected";
        private string _connectionButtonText = "Connect";
        private int _batteryLevel = 0;
        private string _signalQuality = "N/A";
        private string _mentalState = "Unknown";
        private float _deltaValue;
        private float _thetaValue;
        private float _alphaValue;
        private float _betaValue;
        private float _concentrationLevel;
        private string _feedbackImageSource = "neutral.png";
        private string _themeIcon = "theme_light_mode_icon.png";

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged();
                    ConnectionButtonText = value ? "Disconnect" : "Connect";
                    ConnectionStatus = value ? "Connected" : "Disconnected";
                }
            }
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                if (_connectionStatus != value)
                {
                    _connectionStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConnectionButtonText
        {
            get => _connectionButtonText;
            set
            {
                if (_connectionButtonText != value)
                {
                    _connectionButtonText = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                if (_batteryLevel != value)
                {
                    _batteryLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SignalQuality
        {
            get => _signalQuality;
            set
            {
                if (_signalQuality != value)
                {
                    _signalQuality = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MentalState
        {
            get => _mentalState;
            set
            {
                if (_mentalState != value)
                {
                    _mentalState = value;
                    OnPropertyChanged();
                }
            }
        }

        public float DeltaValue
        {
            get => _deltaValue;
            set
            {
                if (_deltaValue != value)
                {
                    _deltaValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public float ThetaValue
        {
            get => _thetaValue;
            set
            {
                if (_thetaValue != value)
                {
                    _thetaValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public float AlphaValue
        {
            get => _alphaValue;
            set
            {
                if (_alphaValue != value)
                {
                    _alphaValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public float BetaValue
        {
            get => _betaValue;
            set
            {
                if (_betaValue != value)
                {
                    _betaValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public float ConcentrationLevel
        {
            get => _concentrationLevel;
            set
            {
                if (_concentrationLevel != value)
                {
                    _concentrationLevel = value;
                    OnPropertyChanged();

                    // Update the feedback image based on concentration level
                    if (value < 0.3f)
                        FeedbackImageSource = "relaxed.png";
                    else if (value < 0.7f)
                        FeedbackImageSource = "focused.png";
                    else
                        FeedbackImageSource = "concentrated.png";
                }
            }
        }

        public string FeedbackImageSource
        {
            get => _feedbackImageSource;
            set
            {
                if (_feedbackImageSource != value)
                {
                    _feedbackImageSource = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ThemeIcon
        {
            get => _themeIcon;
            set
            {
                if (_themeIcon != value)
                {
                    _themeIcon = value;
                    OnPropertyChanged();
                }
            }
        }

        // Commands
        public ICommand ToggleConnectionCommand { get; }
        public ICommand SimulateMeditationCommand { get; }
        public ICommand SimulateFocusCommand { get; }
        public ICommand SimulateDistractionCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public MainViewModel(MockBCIService bciService)
        {
            _bciService = bciService;

            // Set up commands
            ToggleConnectionCommand = new Command(async () => await ToggleConnectionAsync());
            SimulateMeditationCommand = new Command(() => _bciService.SimulateConcentrationScenario("meditation"));
            SimulateFocusCommand = new Command(() => _bciService.SimulateConcentrationScenario("focus"));
            SimulateDistractionCommand = new Command(() => _bciService.SimulateConcentrationScenario("distraction"));
            ToggleThemeCommand = new Command(() => ToggleTheme());

            // Subscribe to events
            _bciService.OnDataReceived += OnBCIDataReceived;
            _bciService.OnConnectionChanged += OnConnectionChanged;
            
            // Set initial theme icon
            UpdateThemeIcon();
        }

        private async Task ToggleConnectionAsync()
        {
            try
            {
                if (IsConnected)
                {
                    await _bciService.DisconnectAsync();
                }
                else
                {
                    ConnectionStatus = "Connecting...";
                    await _bciService.ConnectAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle connection errors
                ConnectionStatus = $"Error: {ex.Message}";
            }
        }

        private void OnBCIDataReceived(object sender, BCISignalData data)
        {
            // Update UI properties on main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DeltaValue = data.DeltaValue;
                ThetaValue = data.ThetaValue;
                AlphaValue = data.AlphaValue;
                BetaValue = data.BetaValue;
                ConcentrationLevel = data.ConcentrationLevel;
                MentalState = data.MentalState;
            });
        }

        private void OnConnectionChanged(object sender, BCIDeviceInfo deviceInfo)
        {
            // Update UI properties on main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsConnected = deviceInfo.IsConnected;
                BatteryLevel = deviceInfo.BatteryLevel;
                SignalQuality = deviceInfo.SignalQuality;
            });
        }

        private void ToggleTheme()
        {
            App.SwitchTheme();
            UpdateThemeIcon();
        }
        
        private void UpdateThemeIcon()
        {
            ThemeIcon = App.IsLightTheme ? "theme_light_mode_icon.png" : "theme_dark_mode_icon.png";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
