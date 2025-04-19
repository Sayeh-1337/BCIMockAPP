using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BCIMockApp.Models
{
    public class BCISignalData : INotifyPropertyChanged
    {
        private float _deltaValue;
        private float _thetaValue;
        private float _alphaValue;
        private float _betaValue;
        private float _concentrationLevel;
        private string _mentaState;

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

                    // Update mental state based on concentration level
                    if (value < 0.3f)
                        MentalState = "Relaxed";
                    else if (value < 0.7f)
                        MentalState = "Focused";
                    else
                        MentalState = "Highly Concentrated";
                }
            }
        }

        public string MentalState
        {
            get => _mentaState;
            set
            {
                if (_mentaState != value)
                {
                    _mentaState = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
