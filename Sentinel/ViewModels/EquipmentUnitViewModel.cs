using CommunityToolkit.Mvvm.ComponentModel;
using Sentinel.Models;

namespace Sentinel.ViewModels;

public partial class EquipmentUnitViewModel : ViewModelBase
{
    [ObservableProperty] private Guid _id;
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _zone;
    [ObservableProperty] private EquipmentStatus _status;
    [ObservableProperty] private double _battery;
    [ObservableProperty] private double _temperature;
    [ObservableProperty] private int _signal;
    [ObservableProperty] private DateTimeOffset _lastSeen;

    public void ApplyTelemetry(EquipmentUnit unit)
    {
        Name = unit.Name;
        Zone = unit.Zone;
        Status = unit.Status;
        Battery = unit.Battery;
        Temperature = unit.Temperature;
        Signal = unit.Signal;
        LastSeen = unit.LastSeen;
    }

    public EquipmentUnitViewModel(EquipmentUnit unit)
    {
        Id = unit.Id;
        Name = unit.Name;
        Zone = unit.Zone;
        Status = unit.Status;
        Battery = unit.Battery;
        Temperature = unit.Temperature;
        Signal = unit.Signal;
        LastSeen = unit.LastSeen;
    }
}