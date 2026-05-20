using ASD_Lab1.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ASD_Lab1.DAL.Entities
{
    public class DeviceEntity
    {
        public Guid Id { get; set; }

        public string DeviceType { get; set; } = string.Empty;

        public string ModelName { get; set; } = string.Empty;
        public int BatteryCapacity { get; set; }
        public double CurrentBatteryLevel { get; set; }
        public bool HasNetworkConnection { get; set; }

        public ICollection<InstalledSoftwareEntity> InstalledSoftware { get; set; } = new List<InstalledSoftwareEntity>();
        public ICollection<PeripheralEntity> ConnectedPeripherals { get; set; } = new List<PeripheralEntity>();
        public ICollection<HardwareComponentEntity> HardwareComponents { get; set; } = new List<HardwareComponentEntity>();
    }
}