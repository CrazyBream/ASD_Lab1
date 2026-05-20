using ASD_Lab1.BLL.Components;
using ASD_Lab1.BLL.Interfaces;
using System;

namespace ASD_Lab1.BLL.Models
{
    public class Tablet : Device, IWorkable, IPlayable, IMediaPlayer, ICommunicable
    {
        public TouchScreen Screen { get; private set; }

        public Tablet(Guid id, string model, Battery battery, Processor cpu, TouchScreen screen)
            : base(id, model, battery, cpu) { Screen = screen; }

        public override void ConnectPeripheral(string peripheral)
        {
            if (peripheral == "Printer")
            {
                NotifyError($"[{ModelName}] Планшет не має USB-порту для принтера!");
                return;
            }
            base.ConnectPeripheral(peripheral);
        }

        public void Work()
        {
            if (TryExecuteAction("Office", false, "", false))
                Notify($"[{ModelName}] Робота з документами. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void Play()
        {
            if (TryExecuteAction("TabletGame", false, "", true))
                Notify($"[{ModelName}] Граємо на планшеті. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void Chat()
        {
            if (TryExecuteAction("Messenger", true, "", false))
                Notify($"[{ModelName}] Чат відкритий. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void ListenMusic()
        {
            if (!IsPoweredOn) { NotifyError($"[{ModelName}] Пристрій вимкнено!"); return; }

            if (!ConnectedPeripherals.Contains("Headphones"))
            {
                NotifyError($"[{ModelName}] Не заважай іншим, підключи навушники!");
                return;
            }

            if (TryExecuteAction("AudioPlayer", false, "", false))
                Notify($"[{ModelName}] Музика в навушниках. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void WatchVideo()
        {
            if (TryExecuteAction("VideoPlayer", false, "", true))
                Notify($"[{ModelName}] Дивимось фільм. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }
    }
}