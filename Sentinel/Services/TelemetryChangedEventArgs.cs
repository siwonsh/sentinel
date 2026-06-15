using Sentinel.Models;

namespace Sentinel.Services;

public class TelemetryChangedEventArgs(EquipmentUnit unit) : EventArgs
{
    public EquipmentUnit Unit { get; } = unit;
}