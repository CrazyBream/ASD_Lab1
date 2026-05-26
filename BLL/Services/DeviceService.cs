using System;
using System.Linq;
using System.Collections.Generic;
using ASD_Lab1.DAL.Repositories;
using ASD_Lab1.DAL.Entities;
using ASD_Lab1.BLL.Models;
using ASD_Lab1.BLL.Factories;

namespace ASD_Lab1.BLL.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public void InitializeDefaultDevices()
        {
            var existing = _repository.GetAllDevices();
            if (!existing.Any())
            {
                CreateDevice("Laptop", "Asus ROG", 6000);
                CreateDevice("Smartphone", "iPhone 15", 3500);
                CreateDevice("Tablet", "iPad Pro", 5000);
            }
        }

        public Device CreateDevice(string deviceType, string modelName, int batteryCapacity)
        {
            var newId = Guid.NewGuid();

            Device device = DeviceFactory.CreateNewDevice(deviceType, modelName, batteryCapacity, newId);

            SaveDeviceState(device);
            return device;
        }

        public Device? GetDevice(Guid id)
        {
            var entity = _repository.GetDeviceById(id);
            if (entity == null) return null;

            Device device = DeviceFactory.RestoreDevice(
                entity.DeviceType,
                entity.ModelName,
                entity.BatteryCapacity,
                entity.CurrentBatteryLevel,
                entity.Id);

            if (entity.HasNetworkConnection) device.ConnectNetwork();

            var rom = device.MemoryModules.FirstOrDefault(m => m.Type == "ROM");
            foreach (var sw in entity.InstalledSoftware)
            {
                device.InstalledSoftware.Add(sw.Name);
                rom?.Allocate(GetSoftwareSize(sw.Name));
            }

            foreach (var peripheral in entity.ConnectedPeripherals)
            {
                device.ConnectedPeripherals.Add(peripheral.Name);
            }

            return device;
        }

        private int GetSoftwareSize(string name)
        {
            return name switch
            {
                "Office" => 15,
                "Game" => 50,
                "VideoPlayer" => 2,
                "AudioPlayer" => 2,
                "PrinterDriver" => 1,
                "Messenger" => 3,
                _ => 10
            };
        }

        public void SaveDeviceState(Device device)
        {
            var entity = new DeviceEntity
            {
                Id = device.Id,
                DeviceType = device.GetType().Name,
                ModelName = device.ModelName,
                BatteryCapacity = device.DeviceBattery.Capacity,
                CurrentBatteryLevel = device.DeviceBattery.CurrentLevel,
                HasNetworkConnection = device.IsNetworkConnected,

                InstalledSoftware = device.InstalledSoftware.Select(s => new InstalledSoftwareEntity
                {
                    Id = Guid.NewGuid(),
                    Name = s,
                    DeviceId = device.Id
                }).ToList(),

                ConnectedPeripherals = device.ConnectedPeripherals.Select(p => new PeripheralEntity
                {
                    Id = Guid.NewGuid(),
                    Name = p,
                    DeviceId = device.Id
                }).ToList()
            };

            _repository.SaveDevice(entity);
        }

        public List<DeviceEntity> GetAllDevicesInfo()
        {
            return _repository.GetAllDevices().ToList();
        }
    }
}