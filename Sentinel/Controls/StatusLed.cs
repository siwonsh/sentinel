using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Sentinel.Models;

namespace Sentinel.Controls;

[PseudoClasses(":operational", ":degraded", ":offline")]
public class StatusLed : TemplatedControl
{
    public static readonly StyledProperty<EquipmentStatus> StatusProperty =
        AvaloniaProperty.Register<StatusLed, EquipmentStatus>(nameof(Status));
    
    public EquipmentStatus Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        switch (Status)
        {
            case EquipmentStatus.Operational:
                SetAreaPseudoclasses(true, false, false);
                break;
            case EquipmentStatus.Degraded:
                SetAreaPseudoclasses(false, true, false);
                break;
            default:
                SetAreaPseudoclasses(false, false, true);
                break;
        }
    }
    
    private void SetAreaPseudoclasses(bool operational, bool degraded, bool offline)
    {
        PseudoClasses.Set(":operational", operational);
        PseudoClasses.Set(":degraded", degraded);
        PseudoClasses.Set(":offline", offline);
    }
}