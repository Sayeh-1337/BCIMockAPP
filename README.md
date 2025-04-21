# BCI Mock App

[Watch the presentation video](https://youtu.be/cPttTWAGo64)

A .NET MAUI application that simulates a Brain-Computer Interface (BCI) with mock data. This demo app allows developers to experiment with BCI UI patterns without requiring actual BCI hardware.

## Features

- **Device Connection**: Simulates connecting to a BCI device
- **Brain Wave Visualization**: Shows simulated delta, theta, alpha, and beta waves
- **Concentration Level**: Displays a calculated concentration level based on the wave patterns
- **Mental State Feedback**: Provides visual feedback on the user's mental state
- **Scenario Simulation**: Includes buttons to simulate different mental states:
  - Meditation (high alpha waves)
  - Focus (high beta waves)
  - Distraction (erratic patterns)

## Getting Started

### Prerequisites

- Visual Studio 2022 with .NET MAUI workload
- .NET 7.0 or later

### Building and Running

1. Open the solution in Visual Studio 2022
2. Ensure the project builds without errors
3. Select your target platform (Windows, Android, iOS, macOS)
4. Press F5 to run the application

## Project Structure

- **Models/**: Data models for BCI signals and device information
- **Services/**: Contains the MockBCIService for generating simulated BCI data
- **ViewModels/**: Contains the MainViewModel for UI data binding
- **Converters/**: Value converters for the UI
- **MainPage.xaml**: Main UI layout

## How It Works

The application uses a timer-based approach to generate mock BCI data:

1. When you press "Connect", the app simulates connecting to a BCI device
2. Once connected, the MockBCIService generates simulated brain wave data at 4Hz (4 times per second)
3. The data is processed and visualized in real-time on the UI
4. The concentration level is calculated based on the ratio of beta to alpha waves


# BCI Mock App Data Flow

## Overview

The BCI Mock App uses a clean MVVM architecture to simulate a brain-computer interface. Here's how data flows through the application:

## 1. Data Generation (MockBCIService)

```
┌─────────────────────┐
│  MockBCIService     │
│                     │
│  ┌───────────────┐  │
│  │ Timer (250ms) │──┼──> Generates mock BCI data
│  └───────────────┘  │      - Brain wave values
│                     │      - Concentration level
│  ┌───────────────┐  │      - Device status
│  │ Event System  │──┼──> Publishes data via events
│  └───────────────┘  │
└─────────────────────┘
```

- The `MockBCIService` uses a Timer that triggers every 250ms (4Hz)
- Each trigger generates realistic mock values for delta, theta, alpha, and beta waves
- The service publishes data through two events:
  - `OnDataReceived`: Brain wave data and derived values
  - `OnConnectionChanged`: Device status (connection, battery, signal quality)

## 2. Business Logic (MainViewModel)

```
┌─────────────────────┐     ┌─────────────────────┐
│  MockBCIService     │     │  MainViewModel      │
│                     │     │                     │
│  OnDataReceived     │────>│  Update Properties  │
│  OnConnectionChanged│────>│  Update UI State    │
│                     │     │                     │
└─────────────────────┘     └─────────────────────┘
```

- The `MainViewModel` subscribes to the service's events
- When data is received, it updates the corresponding properties
- The view model has commands to:
  - Toggle connection (Connect/Disconnect)
  - Simulate specific mental states

## 3. User Interface (MainPage)

```
┌─────────────────────┐     ┌─────────────────────┐
│  MainViewModel      │     │  MainPage           │
│                     │     │                     │
│  Properties         │────>│  Data Binding       │
│  Commands           │<────│  User Interactions  │
│                     │     │                     │
└─────────────────────┘     └─────────────────────┘
```

- The UI binds to properties in the view model
- User interactions trigger commands in the view model
- Value converters translate between view model data and UI representation

## 4. Simulation Scenarios

```
┌─────────────────────┐     ┌─────────────────────┐     ┌─────────────────────┐
│  User Interaction   │     │  MainViewModel      │     │  MockBCIService     │
│  (Pressed Button)   │────>│  Command            │────>│  SimulateScenario   │
└─────────────────────┘     └─────────────────────┘     └─────────────────────┘
```

- When a simulation button is pressed, the command is executed
- The view model calls the corresponding scenario method in the service
- The service temporarily overrides the normal data generation with scenario-specific patterns
- After the scenario completes, normal data generation resumes

## Data Processing Logic

The concentration level is calculated based on the ratio of beta to alpha waves:
```
concentrationLevel = BetaValue / (AlphaValue + 0.1) - 0.5
```

This simulates how focus/concentration typically corresponds to increased beta activity relative to alpha activity in real BCI applications.

## Extending the App

Here are some ideas for extending this demo:

1. **Add data recording**: Implement functionality to record the simulated data to a file
2. **Visualization improvements**: Add line charts to show wave activity over time
3. **Additional mental states**: Add more simulated mental state scenarios
4. **Game integration**: Create a simple game controlled by the concentration level
5. **Multiple device types**: Simulate different types of BCI devices

## Real BCI Integration

To adapt this app for use with real BCI hardware:

1. Replace the MockBCIService with a real implementation that connects to your specific BCI device
2. Update the signal processing algorithms to work with real brain data
3. Calibrate the concentration level calculation based on real user data

## Learn More

To learn more about Brain-Computer Interfaces:

- [Introduction to EEG and BCI](https://www.sciencedirect.com/topics/neuroscience/brain-computer-interface)
- [OpenBCI](https://openbci.com/) - Open source brain-computer interfaces
- [EMOTIV](https://www.emotiv.com/) - Commercial BCI solutions
- [Muse by InteraXon](https://choosemuse.com/) - Consumer EEG headsets
- [Presentation Video](https://youtu.be/cPttTWAGo64) - Video demonstration of BCI applications
- [PowerPoint Presentation](Presentation\Building Brain-Computer Interface Applications with.pptx) - Detailed slides on building BCI applications

## License

This project is licensed under the MIT License - see the LICENSE file for details.