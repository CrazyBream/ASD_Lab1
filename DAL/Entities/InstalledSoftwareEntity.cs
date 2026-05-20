using System;

namespace ASD_Lab1.DAL.Entities
{
    public class InstalledSoftwareEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid DeviceId { get; set; }
        public DeviceEntity Device { get; set; } = null!;
    }
}