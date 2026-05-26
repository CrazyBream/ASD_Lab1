using System;

namespace ASD_Lab1.BLL.Interfaces
{
    public interface IDevice
    {
        Guid Id { get; }
        string ModelName { get; }
        bool HasPowerOutlet { get; }
        bool IsNetworkConnected { get; }

        void SetPowerOutlet(bool hasPower);
        void ConnectNetwork();

        void InstallSoftware(string software, int requiredSpaceGB = 10);

        void ConnectPeripheral(string peripheral);
        void DisconnectPeripheral(string peripheral);
    }
}