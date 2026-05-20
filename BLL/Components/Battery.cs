using System;

namespace ASD_Lab1.BLL.Components
{
    public class Battery
    {
        public int Capacity { get; private set; }
        public double CurrentLevel { get; private set; }

        private readonly double _intensiveDrainRate;
        private readonly double _nonIntensiveDrainRate;

        public event Action? OnBatteryDepleted;

        public Battery(int capacity)
        {
            Capacity = capacity;
            CurrentLevel = capacity;

            if (capacity >= 2000 && capacity <= 3000)
            {
                _nonIntensiveDrainRate = capacity / 48.0;
                _intensiveDrainRate = capacity / 16.0;
            }
            else if (capacity >= 5000 && capacity <= 7000)
            {
                _nonIntensiveDrainRate = capacity / 12.0;
                _intensiveDrainRate = capacity / 4.0;
            }
            else
            {
                _nonIntensiveDrainRate = capacity / 24.0;
                _intensiveDrainRate = capacity / 8.0;
            }
        }

        public Battery(int capacity, double currentLevel) : this(capacity)
        {
            CurrentLevel = currentLevel;
        }

        public bool Consume(bool isIntensive, double hours = 1.0)
        {
            double drain = (isIntensive ? _intensiveDrainRate : _nonIntensiveDrainRate) * hours;

            if (CurrentLevel - drain > 0)
            {
                CurrentLevel -= drain;
                return true;
            }

            CurrentLevel = 0;
            OnBatteryDepleted?.Invoke();
            return false;
        }

        public void Charge()
        {
            CurrentLevel = Capacity;
        }
    }
}