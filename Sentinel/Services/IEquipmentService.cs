using Sentinel.Models;

namespace Sentinel.Services;

public interface IEquipmentService
{
    public Task<IReadOnlyList<EquipmentUnit>> GetFleetAsync(CancellationToken cancellationToken = default);

    public event EventHandler<TelemetryChangedEventArgs> TelemetryChanged;
}