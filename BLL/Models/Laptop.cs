using ASD_Lab1.BLL.Components;
using ASD_Lab1.BLL.Interfaces;
using System;

namespace ASD_Lab1.BLL.Models
{
    public class Laptop : Device, IWorkable, IPlayable, IMediaPlayer, ICommunicable, IPrintable
    {
        public Laptop(Guid id, string model, Battery battery, Processor cpu) : base(id, model, battery, cpu) { }

        public void Work()
        {
            if (TryExecuteAction("Office", false, "", false))
                Notify($"[{ModelName}] Виконується робота. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void Play()
        {
            if (TryExecuteAction("Game", false, "", true))
                Notify($"[{ModelName}] Граємо в гру. Навантаження інтенсивне. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void Chat()
        {
            if (TryExecuteAction("Messenger", true, "", false))
                Notify($"[{ModelName}] Спілкування в чаті. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void ListenMusic()
        {
            if (TryExecuteAction("AudioPlayer", true, "Headphones", false))
                Notify($"[{ModelName}] Грає музика. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void WatchVideo()
        {
            if (TryExecuteAction("VideoPlayer", true, "", false))
                Notify($"[{ModelName}] Відтворення відео. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }

        public void Print()
        {
            if (TryExecuteAction("PrinterDriver", false, "Printer", false))
                Notify($"[{ModelName}] Друк на принтері. Заряд: {DeviceBattery.CurrentLevel:F1} мАг.");
        }
    }
}