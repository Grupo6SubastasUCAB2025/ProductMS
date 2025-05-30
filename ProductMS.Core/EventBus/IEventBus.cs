using System.Threading.Tasks;

namespace ProductMS.Core.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : class;
    }
}