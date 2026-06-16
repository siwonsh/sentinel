using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sentinel.Models;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public ObservableCollection<EquipmentUnit> Units { get; set; } = [];

    private IEquipmentService _equipmentService;
    
    public HomeViewModel(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;

        _ = LoadAsync();

        _equipmentService.TelemetryChanged += OnTelemetryChanged;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        Units.Clear();
        var units = await _equipmentService.GetFleetAsync();
        foreach (var unit in units)
        {
            Units.Add(unit);
        }
    }

    private void OnTelemetryChanged(object? sender, TelemetryChangedEventArgs e)
    {
        var id = e.Unit.Id;
        for (var i=0; i < Units.Count; i++)
        {
            if (Units[i].Id == id)
            {
                Units[i] = e.Unit;
                break;
            }
        }
    }
}