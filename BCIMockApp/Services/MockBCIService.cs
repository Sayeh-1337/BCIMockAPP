using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCIMockApp.Models;
using System.Diagnostics;

namespace BCIMockApp.Services
{
    public class MockBCIService
    {
        private readonly Random _random = new Random();
        private readonly Timer _dataTimer;
        private bool _isConnected;
        private BCIDeviceInfo _deviceInfo;

        public event EventHandler<BCISignalData> OnDataReceived;
        public event EventHandler<BCIDeviceInfo> OnConnectionChanged;

        public MockBCIService()
        {
            _deviceInfo = new BCIDeviceInfo
            {
                DeviceId = "MOCK-001",
                DeviceName = "Simulated BCI Headset",
                IsConnected = false,
                BatteryLevel = 100,
                SignalQuality = "N/A"
            };

            // Create a timer that fires every 250ms (4Hz) to simulate data updates
            _dataTimer = new Timer(GenerateData, null, Timeout.Infinite, 250);
        }

        public async Task ConnectAsync()
        {
            if (_isConnected)
                return;

            // Simulate connection delay
            await Task.Delay(2000);

            _isConnected = true;
            _deviceInfo.IsConnected = true;
            _deviceInfo.SignalQuality = "Good";

            // Notify about connection
            OnConnectionChanged?.Invoke(this, _deviceInfo);

            // Start generating data
            _dataTimer.Change(0, 250);

            Debug.WriteLine("Connected to mock BCI device");
        }

        public Task DisconnectAsync()
        {
            if (!_isConnected)
                return Task.CompletedTask;

            // Stop generating data
            _dataTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _isConnected = false;
            _deviceInfo.IsConnected = false;
            _deviceInfo.SignalQuality = "N/A";

            // Notify about disconnection
            OnConnectionChanged?.Invoke(this, _deviceInfo);

            Debug.WriteLine("Disconnected from mock BCI device");

            return Task.CompletedTask;
        }

        private void GenerateData(object state)
        {
            if (!_isConnected)
                return;

            // Generate mock brain wave data with realistic patterns
            // In a real BCI app, this would come from the device

            // Create simulated signal data
            var data = new BCISignalData
            {
                // Generate brain wave values with some randomness but within realistic ranges
                // Delta waves (0.5-4 Hz) - Deep sleep
                DeltaValue = 0.2f + (float)_random.NextDouble() * 0.3f,

                // Theta waves (4-8 Hz) - Drowsiness, meditation
                ThetaValue = 0.3f + (float)_random.NextDouble() * 0.4f,

                // Alpha waves (8-13 Hz) - Relaxed but alert state
                AlphaValue = 0.4f + (float)_random.NextDouble() * 0.5f,

                // Beta waves (13-30 Hz) - Active thinking, focus
                BetaValue = 0.3f + (float)_random.NextDouble() * 0.6f
            };

            // Calculate a "concentration level" based on beta/alpha ratio
            // Higher beta relative to alpha often indicates more concentration
            float concentrationLevel = Math.Clamp(data.BetaValue / (data.AlphaValue + 0.1f) - 0.5f, 0, 1);
            data.ConcentrationLevel = concentrationLevel;

            // Occasionally simulate "battery drain"
            if (_random.Next(100) < 5 && _deviceInfo.BatteryLevel > 0)
            {
                _deviceInfo.BatteryLevel--;
                OnConnectionChanged?.Invoke(this, _deviceInfo);
            }

            // Occasionally simulate signal quality changes
            if (_random.Next(100) < 3)
            {
                string[] qualities = { "Excellent", "Good", "Fair", "Poor" };
                _deviceInfo.SignalQuality = qualities[_random.Next(qualities.Length)];
                OnConnectionChanged?.Invoke(this, _deviceInfo);
            }

            // Invoke the event to send the data
            OnDataReceived?.Invoke(this, data);
        }

        // Add a method to simulate concentration scenarios
        public void SimulateConcentrationScenario(string scenario)
        {
            if (!_isConnected)
                return;

            // Stop the normal data generation temporarily
            _dataTimer.Change(Timeout.Infinite, Timeout.Infinite);

            // Start a task to simulate the requested scenario
            Task.Run(async () =>
            {
                switch (scenario.ToLower())
                {
                    case "meditation":
                        // Simulate meditation - increase alpha, decrease beta
                        for (int i = 0; i < 20; i++)
                        {
                            var data = new BCISignalData
                            {
                                DeltaValue = 0.2f + (float)_random.NextDouble() * 0.2f,
                                ThetaValue = 0.5f + (float)_random.NextDouble() * 0.3f,
                                AlphaValue = 0.6f + (float)(Math.Min(0.3, 0.02 * i)) + (float)_random.NextDouble() * 0.1f,
                                BetaValue = Math.Max(0.2f, 0.5f - (float)(0.01 * i)) + (float)_random.NextDouble() * 0.1f
                            };

                            // Lower concentration in meditation
                            data.ConcentrationLevel = Math.Max(0.1f, 0.4f - (float)(0.01 * i));

                            OnDataReceived?.Invoke(this, data);
                            await Task.Delay(250);
                        }
                        break;

                    case "focus":
                        // Simulate increasing focus - increase beta
                        for (int i = 0; i < 20; i++)
                        {
                            var data = new BCISignalData
                            {
                                DeltaValue = 0.2f + (float)_random.NextDouble() * 0.1f,
                                ThetaValue = 0.3f + (float)_random.NextDouble() * 0.2f,
                                AlphaValue = Math.Max(0.2f, 0.5f - (float)(0.01 * i)) + (float)_random.NextDouble() * 0.1f,
                                BetaValue = 0.5f + (float)(Math.Min(0.4, 0.02 * i)) + (float)_random.NextDouble() * 0.1f
                            };

                            // Increasing concentration
                            data.ConcentrationLevel = Math.Min(1.0f, 0.5f + (float)(0.025 * i));

                            OnDataReceived?.Invoke(this, data);
                            await Task.Delay(250);
                        }
                        break;

                    case "distraction":
                        // Simulate distraction - erratic pattern
                        for (int i = 0; i < 20; i++)
                        {
                            var data = new BCISignalData
                            {
                                DeltaValue = 0.2f + (float)_random.NextDouble() * 0.6f,
                                ThetaValue = 0.3f + (float)_random.NextDouble() * 0.5f,
                                AlphaValue = 0.3f + (float)_random.NextDouble() * 0.4f,
                                BetaValue = 0.4f + (float)_random.NextDouble() * 0.3f
                            };

                            // Erratic concentration
                            if (i % 3 == 0)
                                data.ConcentrationLevel = 0.2f + (float)_random.NextDouble() * 0.3f;
                            else
                                data.ConcentrationLevel = 0.5f + (float)_random.NextDouble() * 0.4f;

                            OnDataReceived?.Invoke(this, data);
                            await Task.Delay(250);
                        }
                        break;
                }

                // Restart normal data generation
                _dataTimer.Change(0, 250);
            });
        }
    }
}