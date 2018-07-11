using System.Threading.Tasks;
using Orleans;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorGrain : IGrainWithIntegerKey
    {
        Task RecordValue();
    }
}