using ASD_Lab1.BLL.Components;
using ASD_Lab1.BLL.Interfaces;
using System;

namespace ASD_Lab1.BLL.Models
{
    public class Smartphone : Device, IPlayable, IMediaPlayer, ICommunicable
    {
        public TouchScreen Screen { get; private set; }

        public Smartphone(Guid id, string model, Battery battery, Processor cpu, TouchScreen screen)
            : base(id, model, battery, cpu) { Screen = screen; }

        public override void ConnectPeripheral(string peripheral)
        {
            if (peripheral == "Printer")
            {
                NotifyError($"[{ModelName}] Цей пристрій не підтримує підключення принтера!");
                return;
            }
            base.ConnectPeripheral(peripheral);
        }

        public void Play()
        {
            if (!Screen.IsMultiTouchSupported)
            {
                NotifyError($"[{ModelName}] Без підтримки Multi-Touch грати неможливо!");
                return;
            }

            if (TryExecuteAction("Game", false, "", true))
                Notify($"[{ModelName}] Запущено мобільну гру. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void Chat()
        {
            if (TryExecuteAction("Messenger", true, "", false))
                Notify($"[{ModelName}] Чатимось. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void ListenMusic()
        {
            if (TryExecuteAction("AudioPlayer", true, "Headphones", false))
                Notify($"[{ModelName}] Музика в навушниках. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void WatchVideo()
        {
            if (TryExecuteAction("VideoPlayer", true, "", false))
                Notify($"[{ModelName}] Перегляд відео. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }
    }
}