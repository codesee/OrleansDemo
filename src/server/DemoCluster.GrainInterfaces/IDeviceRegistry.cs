using DemoCluster.GrainInterfaces.Patterns;
using Orleans;
using System;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceRegistry : IRegistryGrain<IDeviceGrain>
    {
        Task Initialize();
        Task<bool> GetLoadedDeviceState(string deviceId);
        Task StartDevice(string deviceId);
        Task StopDevice(string deviceId);
    }
}