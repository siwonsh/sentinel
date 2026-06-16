using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Sentinel.ViewModels;

public partial class EquipmentEditViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [MinLength(1)]
    private string _name;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [MinLength(1)]
    private string _zone;

    public EquipmentEditViewModel()
    {
        ValidateAllProperties();
    }
}