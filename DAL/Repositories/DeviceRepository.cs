using ASD_Lab1.DAL.Context;
using ASD_Lab1.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD_Lab1.DAL.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AppDbContext _context;

        public DeviceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DeviceEntity> GetAllDevices()
        {
            return _context.Devices
                .Include(d => d.InstalledSoftware)
                .Include(d => d.ConnectedPeripherals)
                .AsNoTracking() 
                .ToList();
        }

        public DeviceEntity? GetDeviceById(Guid id)
        {
            return _context.Devices
                .Include(d => d.InstalledSoftware)
                .Include(d => d.ConnectedPeripherals)
                .AsNoTracking() 
                .FirstOrDefault(d => d.Id == id);
        }

        public void SaveDevice(DeviceEntity device)
        {
            _context.ChangeTracker.Clear();

            var existingDevice = _context.Devices
                .Include(d => d.InstalledSoftware)
                .Include(d => d.ConnectedPeripherals)
                .AsNoTracking()
                .FirstOrDefault(d => d.Id == device.Id);

            if (existingDevice == null)
            {
                _context.Devices.Add(device);
                _context.SaveChanges();
            }
            else
            {
                foreach (var oldSw in existingDevice.InstalledSoftware)
                {
                    _context.Entry(oldSw).State = EntityState.Deleted;
                }
                foreach (var oldPer in existingDevice.ConnectedPeripherals)
                {
                    _context.Entry(oldPer).State = EntityState.Deleted;
                }
                _context.SaveChanges(); 

                _context.ChangeTracker.Clear(); 

                _context.Entry(device).State = EntityState.Modified;

                foreach (var newSw in device.InstalledSoftware)
                {
                    _context.Entry(newSw).State = EntityState.Added;
                }
                foreach (var newPer in device.ConnectedPeripherals)
                {
                    _context.Entry(newPer).State = EntityState.Added;
                }

                _context.SaveChanges(); 
            }
        }
    }
}