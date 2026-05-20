using System;

namespace ASD_Lab1.DAL.Entities
{
    public class HardwareComponentEntity
    {
        public Guid Id { get; set; }
        public string ComponentType { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;

        public Guid DeviceId { get; set; }
        public DeviceEntity Device { get; set; } = null!;
    }
}