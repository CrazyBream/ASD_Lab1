using ASD_Lab1.BLL.Interfaces;
using ASD_Lab1.BLL.Models;
using ASD_Lab1.BLL.Services;
using System;
using System.Linq;

namespace ASD_Lab1.PL.UI
{
    public class ConsoleInterface
    {
        private readonly DeviceService _deviceService;

        public ConsoleInterface(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public void RunDemo()
        {
            Console.WriteLine("=== Симулятор комп'ютерної техніки (Варіант 5) ===");

            _deviceService.InitializeDefaultDevices();

            while (true)
            {
                Console.WriteLine("\n--- ВИБІР ПРИСТРОЮ ---");
                Console.WriteLine("1. Взяти Ноутбук (Asus ROG)");
                Console.WriteLine("2. Взяти Смартфон (iPhone 15)");
                Console.WriteLine("3. Взяти Планшет (iPad Pro)");
                Console.WriteLine("0. Вийти з програми");
                Console.Write("Ваш вибір: ");

                string? choice = Console.ReadLine();
                Device? currentDevice = null;

                var allDevices = _deviceService.GetAllDevicesInfo();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var laptopId = allDevices.First(d => d.ModelName == "Asus ROG").Id;
                            currentDevice = _deviceService.GetDevice(laptopId);
                            break;
                        case "2":
                            var phoneId = allDevices.First(d => d.ModelName == "iPhone 15").Id;
                            currentDevice = _deviceService.GetDevice(phoneId);
                            break;
                        case "3":
                            var tabletId = allDevices.First(d => d.ModelName == "iPad Pro").Id;
                            currentDevice = _deviceService.GetDevice(tabletId);
                            break;
                        case "0": return;
                        default: Console.WriteLine("Невірний вибір!"); continue;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Помилка: База даних пошкоджена або містить дублікати. Видаліть її в SSMS і перезапустіть.");
                    continue;
                }

                if (currentDevice != null)
                {
                    SubscribeToEvents(currentDevice);
                    ManageDevice(currentDevice);
                }
            }
        }

        private void ManageDevice(Device device)
        {
            bool inDeviceMenu = true;
            while (inDeviceMenu)
            {
                string status = device.IsPoweredOn ? "УВІМКНЕНО" : "ВИМКНЕНО";

                var rom = device.MemoryModules.FirstOrDefault(m => m.Type == "ROM");
                string romInfo = rom != null ? $"{rom.UsedGB}/{rom.CapacityGB} ГБ" : "0/0 ГБ";

                Console.WriteLine($"\n--- ЕКРАН: {device.ModelName} | Стан: {status} | Заряд: {device.DeviceBattery.CurrentLevel:F0} мАг | Диск ROM: {romInfo} ---");

                Console.WriteLine("--- БАЗОВЕ КЕРУВАННЯ ---");
                Console.WriteLine("1. Увімкнути / Вимкнути пристрій");
                Console.WriteLine("2. Підключитися до Wi-Fi");
                Console.WriteLine("3. Встановити потрібне ПЗ (App Store)");
                Console.WriteLine("4. Підключити периферію (Принтер / Навушники)");
                Console.WriteLine("5. Поставити на зарядку");

                Console.WriteLine("--- ДОДАТКИ ---");
                Console.WriteLine("6. Запустити Office (Робота)");
                Console.WriteLine("7. Запустити Гра (Інтенсивне)");
                Console.WriteLine("8. Запустити YouTube (Відео)");
                Console.WriteLine("9. Слухати музику (Аудіоплеєр)"); 
                Console.WriteLine("10. Роздрукувати файл");
                Console.WriteLine("11. Відкрити Месенджер");
                Console.WriteLine("0. Покласти пристрій на стіл (Назад до вибору)");

                Console.Write("Оберіть дію: ");
                string? action = Console.ReadLine();
                Console.WriteLine();

                switch (action)
                {
                    case "1": device.TogglePower(); break;
                    case "2": device.ConnectNetwork(); break;
                    case "3": InstallSoftwareMenu(device); break;
                    case "4": ConnectPeripheralMenu(device); break;
                    case "5": device.DeviceBattery.Charge(); Console.WriteLine("> [Система] Батарея повністю заряджена."); break;

                    case "6": if (device is IWorkable w) w.Work(); else Console.WriteLine("Цей пристрій не підтримує роботу."); break;
                    case "7": if (device is IPlayable p) p.Play(); else Console.WriteLine("Цей пристрій не підтримує ігри."); break;
                    case "8": if (device is IMediaPlayer m1) m1.WatchVideo(); else Console.WriteLine("Цей пристрій не підтримує відео."); break;
                    case "9": if (device is IMediaPlayer m2) m2.ListenMusic(); else Console.WriteLine("Цей пристрій не підтримує музику."); break; 
                    case "10": if (device is IPrintable pr) pr.Print(); else Console.WriteLine("Цей пристрій не підтримує друк."); break;
                    case "11": if (device is ICommunicable c) c.Chat(); else Console.WriteLine("Цей пристрій не підтримує чати."); break;

                    case "0":
                        _deviceService.SaveDeviceState(device);
                        Console.WriteLine($"[Система] Стан {device.ModelName} успішно зафіксовано. Повернення...");
                        inDeviceMenu = false;
                        break;
                    default: Console.WriteLine("Невідома команда."); break;
                }
            }
        }

        private void InstallSoftwareMenu(Device device)
        {
            Console.WriteLine("Оберіть ПЗ для встановлення:");
            Console.WriteLine("1. Office (15 ГБ)\n2. Game (50 ГБ)\n3. VideoPlayer (2 ГБ)\n4. PrinterDriver (1 ГБ)\n5. Messenger (3 ГБ)\n6. AudioPlayer (2 ГБ)");
            string? swChoice = Console.ReadLine();

            switch (swChoice)
            {
                case "1": device.InstallSoftware("Office", 15); break;
                case "2": device.InstallSoftware("Game", 50); break;
                case "3": device.InstallSoftware("VideoPlayer", 2); break;
                case "4": device.InstallSoftware("PrinterDriver", 1); break;
                case "5": device.InstallSoftware("Messenger", 3); break;
                case "6": device.InstallSoftware("AudioPlayer", 2); break;
                default: Console.WriteLine("Скасовано."); break;
            }
        }

        private void ConnectPeripheralMenu(Device device)
        {
            Console.WriteLine("Оберіть пристрій:");
            Console.WriteLine("1. Printer\n2. Headphones");
            string? perChoice = Console.ReadLine();

            if (perChoice == "1") device.ConnectPeripheral("Printer");
            else if (perChoice == "2") device.ConnectPeripheral("Headphones");
        }

        private void SubscribeToEvents(Device device)
        {
            device.OnStateChanged -= OnStateChangedHandler;
            device.OnError -= OnErrorHandler;
            device.DeviceBattery.OnBatteryDepleted -= OnBatteryDepletedHandler;

            device.OnStateChanged += OnStateChangedHandler;
            device.OnError += OnErrorHandler;
            device.DeviceBattery.OnBatteryDepleted += OnBatteryDepletedHandler;
        }

        private void OnStateChangedHandler(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"> {message}");
            Console.ResetColor();
        }

        private void OnErrorHandler(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ПОМИЛКА] {error}");
            Console.ResetColor();
        }

        private void OnBatteryDepletedHandler()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n[УВАГА] Акумулятор розряджений! Роботу завершено.");
            Console.ResetColor();
        }
    }
}