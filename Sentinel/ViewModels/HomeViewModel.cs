using System.Collections.ObjectModel;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private IEquipmentService _equipmentService;
    private ObservableCollection<EquipmentUnitViewModel> Units { get; set; } = [];
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelection))]
    private EquipmentUnitViewModel? _selectedUnit;
    
    [ObservableProperty] private string? _filterText;
    public DataGridCollectionView FilteredUnits { get; }

    public bool HasSelection => SelectedUnit is not null;
    
    [ObservableProperty] private EquipmentEditViewModel _editViewModel = new();
    
    public HomeViewModel(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;

        _ = LoadAsync();

        _equipmentService.TelemetryChanged += OnTelemetryChanged;
        
        FilteredUnits = new DataGridCollectionView(Units)
        {
            Filter = o =>
            {
                if (string.IsNullOrEmpty(FilterText)) return true;
            
                if (o is EquipmentUnitViewModel equipmentUnit)
                {
                    return equipmentUnit.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                           equipmentUnit.Zone.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                           equipmentUnit.Status.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }
        };
        
        EditViewModel.ErrorsChanged += (_, _) => SaveCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        Units.Clear();
        var units = await _equipmentService.GetFleetAsync();
        foreach (var unit in units)
        {
            Units.Add(new EquipmentUnitViewModel(unit));
        }
    }

    private void OnTelemetryChanged(object? sender, TelemetryChangedEventArgs e)
    {
        var id = e.Unit.Id;
        for (var i=0; i < Units.Count; i++)
        {
            if (Units[i].Id == id)
            {
                Units[i].ApplyTelemetry(e.Unit);
                break;
            }
        }
    }

    partial void OnFilterTextChanged(string? value)
    {
        FilteredUnits.Refresh();
    }

    public bool CanSave => SelectedUnit is not null && !EditViewModel.HasErrors;

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        SelectedUnit!.Name = EditViewModel.Name;
        SelectedUnit.Zone = EditViewModel.Zone;
    }
}