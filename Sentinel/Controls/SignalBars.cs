using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Sentinel.Controls;

[PseudoClasses(":l1", ":l2", ":l3", ":l4")]
public class SignalBars : TemplatedControl
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<SignalBars, double>(nameof(Value));
    
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<SignalBars, double>(nameof(Maximum));
    
    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property != ValueProperty) return;
        
        if (Value <= 0)
            SetAreaPseudoclasses(false, false, false, false);
        else if (Value > 0 && Value < Maximum / 4)
            SetAreaPseudoclasses(true, false, false, false);
        else if (Value > Maximum / 4 && Value < Maximum / 2)
            SetAreaPseudoclasses(false, true, false, false);
        else if (Value > Maximum / 2 && Value < 3 * Maximum / 4)
            SetAreaPseudoclasses(false, false, true, false);
        else
            SetAreaPseudoclasses(false, false, false, true);
    }
    
    private void SetAreaPseudoclasses(bool l1, bool l2, bool l3, bool l4)
    {
        PseudoClasses.Set(":l1", l1);
        PseudoClasses.Set(":l2", l2);
        PseudoClasses.Set(":l3", l3);
        PseudoClasses.Set(":l4", l4);
    }
}