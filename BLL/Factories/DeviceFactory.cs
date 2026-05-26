using ASD_Lab1.BLL.Components;
using ASD_Lab1.BLL.Models;
using System;

namespace ASD_Lab1.BLL.Factories
{
    public static class DeviceFactory
    {
        public static Device CreateNewDevice(string deviceType, string modelName, int batteryCapacity, Guid id)
        {
            var battery = new Battery(batteryCapacity);
            return AssembleDevice(deviceType, modelName, battery, id);
        }

        public static Device RestoreDevice(string deviceType, string modelName, int batteryCapacity, double currentBatteryLevel, Guid id)
        {
            var battery = new Battery(batteryCapacity, currentBatteryLevel);
            return AssembleDevice(deviceType, modelName, battery, id);
        }

        private static Device AssembleDevice(string deviceType, string modelName, Battery battery, Guid id)
        {
            TouchScreen screen = deviceType switch
            {
                "Tablet" => new TouchScreen(11.0, true),
                "Smartphone" => new TouchScreen(6.1, true),
                _ => new TouchScreen(0, false)
            };

            Processor cpu = deviceType switch
            {
                "Laptop" => new Processor("Intel Core i7", 8),
                "Tablet" => new Processor("Apple M1", 4),
                "Smartphone" => new Processor("ARM Cortex", 2),
                _ => new Processor("Generic", 4)
            };

            return deviceType switch
            {
                "Laptop" => new Laptop(id, modelName, battery, cpu),
                "Smartphone" => new Smartphone(id, modelName, battery, cpu, screen),
                "Tablet" => new Tablet(id, modelName, battery, cpu, screen),
                _ => throw new ArgumentException($"Невідомий тип пристрою: {deviceType}")
            };
        }
    }
}