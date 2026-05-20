using ASD_Lab1.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ASD_Lab1.DAL.Repositories
{
    public interface IDeviceRepository
    {
        IEnumerable<DeviceEntity> GetAllDevices();
        DeviceEntity? GetDeviceById(Guid id);
        void SaveDevice(DeviceEntity device);
    }
}