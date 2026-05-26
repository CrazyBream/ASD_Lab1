using ASD_Lab1.BLL.Components;
using ASD_Lab1.BLL.Interfaces;
using ASD_Lab1.BLL.Events; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD_Lab1.BLL.Models
{
    public abstract class Device : IDevice
    {
        public Guid Id { get; protected set; }
        public string ModelName { get; protected set; }
        public bool HasPowerOutlet { get; protected set; }
        public bool IsNetworkConnected { get; protected set; }
        public bool IsPoweredOn { get; protected set; }

        public Battery DeviceBattery { get; protected set; }
        public Processor Cpu { get; protected set; }
        public List<Memory> MemoryModules { get; protected set; } = new();
        public List<string> InstalledSoftware { get; protected set; } = new();
        public List<string> ConnectedPeripherals { get; protected set; } = new();

        public event EventHandler<DeviceStateChangedEventArgs>? StateChanged;
        public event EventHandler<DeviceErrorEventArgs>? ErrorOccurred;

        protected Device(Guid id, string modelName, Battery battery, Processor cpu)
        {
            Id = id;
            ModelName = modelName;
            DeviceBattery = battery;
            Cpu = cpu;

            MemoryModules.Add(new Memory("RAM", 16));
            MemoryModules.Add(new Memory("ROM", 512));
        }

        public void SetPowerOutlet(bool hasPower) => HasPowerOutlet = hasPower;

        public void TogglePower()
        {
            if (!IsPoweredOn && DeviceBattery.CurrentLevel <= 0 && !HasPowerOutlet)
            {
                NotifyError($"[{ModelName}] Неможливо увімкнути. Батарея розряджена.");
                return;
            }
            IsPoweredOn = !IsPoweredOn;
            Notify($"[{ModelName}] Пристрій {(IsPoweredOn ? "УВІМКНЕНО" : "ВИМКНЕНО")}.");
        }

        public void ConnectNetwork()
        {
            if (!IsPoweredOn) { NotifyError("Спочатку увімкніть пристрій!"); return; }
            IsNetworkConnected = true;
            Notify($"[{ModelName}] Підключено до Wi-Fi.");
        }

        public virtual void ConnectPeripheral(string peripheral)
        {
            if (!ConnectedPeripherals.Contains(peripheral))
            {
                ConnectedPeripherals.Add(peripheral);
                Notify($"[{ModelName}] Підключено: {peripheral}.");
            }
            else
            {
                NotifyError($"[{ModelName}] {peripheral} вже підключено.");
            }
        }

        public void InstallSoftware(string software, int requiredSpaceGB = 10)
        {
            if (!IsPoweredOn) { NotifyError("Спочатку увімкніть пристрій!"); return; }
            if (InstalledSoftware.Contains(software)) { NotifyError($"ПЗ {software} вже встановлено."); return; }

            var rom = MemoryModules.FirstOrDefault(m => m.Type == "ROM");
            if (rom != null && rom.Allocate(requiredSpaceGB))
            {
                InstalledSoftware.Add(software);
                Notify($"[{ModelName}] Встановлено {software}. Зайнято {requiredSpaceGB}ГБ на диску.");
            }
            else
            {
                NotifyError($"[{ModelName}] Недостатньо місця на диску для {software}!");
            }
        }

        public virtual void UninstallSoftware(string software, int freedSpaceGB = 10)
        {
            if (!IsPoweredOn) { NotifyError("Спочатку увімкніть пристрій!"); return; }

            if (InstalledSoftware.Contains(software))
            {
                InstalledSoftware.Remove(software);

                var rom = MemoryModules.FirstOrDefault(m => m.Type == "ROM");
                if (rom != null)
                {
                    rom.Free(freedSpaceGB);
                }

                Notify($"[{ModelName}] ПЗ {software} успішно видалено. Звільнено {freedSpaceGB}ГБ.");
            }
            else
            {
                NotifyError($"[{ModelName}] ПЗ {software} не знайдено на пристрої.");
            }
        }

        protected void Notify(string message) => StateChanged?.Invoke(this, new DeviceStateChangedEventArgs(message));
        protected void NotifyError(string message) => ErrorOccurred?.Invoke(this, new DeviceErrorEventArgs(message));

        protected bool TryExecuteAction(string requiredSoftware, bool requiresNetwork, string requiredPeripheral, bool isIntensive)
        {
            if (!IsPoweredOn)
            {
                NotifyError($"[{ModelName}] Пристрій вимкнено! Натисніть кнопку живлення.");
                return false;
            }

            if (isIntensive && Cpu.Cores < 4)
            {
                NotifyError($"[{ModelName}] Процесор {Cpu.Model} занадто слабкий для цієї задачі.");
                return false;
            }

            if (!string.IsNullOrEmpty(requiredSoftware) && !InstalledSoftware.Contains(requiredSoftware))
            {
                NotifyError($"[{ModelName}] Відсутнє ПЗ: {requiredSoftware}. Встановіть його спочатку.");
                return false;
            }
            if (requiresNetwork && !IsNetworkConnected)
            {
                NotifyError($"[{ModelName}] Немає інтернету. Підключіться до мережі.");
                return false;
            }
            if (!string.IsNullOrEmpty(requiredPeripheral) && !ConnectedPeripherals.Contains(requiredPeripheral))
            {
                NotifyError($"[{ModelName}] Потрібен зовнішній пристрій: {requiredPeripheral}.");
                return false;
            }

            if (HasPowerOutlet) return true;

            if (!DeviceBattery.Consume(isIntensive))
            {
                NotifyError($"[{ModelName}] Батарея сіла! Пристрій екстрено вимикається.");
                IsPoweredOn = false;
                return false;
            }
            return true;
        }
    }
}