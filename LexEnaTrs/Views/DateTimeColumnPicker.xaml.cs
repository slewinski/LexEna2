using System.Windows;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System;
using System.Windows.Controls;

namespace LexEnaTrs
{
    

public partial class DateTimePicker : UserControl
{
    public static readonly DependencyProperty SelectedDateProperty =
        DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(null, OnSelectedDateChanged));
    public DateTimePicker()
    {
        this.InitializeComponent();
    }
    public DateTime? SelectedDate
    {
        get
        {
            return (DateTime?)this.GetValue(SelectedDateProperty);
        }
        set
        {
            this.SetValue(SelectedDateProperty, value);
        }
    }
    private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (DateTimePicker)d;
        picker.OnSelectedDateChanged((DateTime)e.NewValue);
    }
    private void HandlePickersSelectionChanged()
    {
        this.SelectedDate = this.Calender.SelectedDate; 
    }
    private void OnCalenderSelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
    {
        this.HandlePickersSelectionChanged();
    }
    private void OnSelectedDateChanged(DateTime selectedDate)
    {
        
        this.Calender.SelectedDate = selectedDate;
    }
    private void OnTimeChanged(object sender, EventArgs e)
    {
        this.HandlePickersSelectionChanged();
    }
  }

public class DateTimePickerColumn : GridViewBoundColumnBase
{
    public override FrameworkElement CreateCellEditElement(GridViewCell cell, object dataItem)
    {
        this.BindingTarget = DateTimePicker.SelectedDateProperty;
        var picker = new DateTimePicker();
        picker.SetBinding(this.BindingTarget, this.CreateValueBinding());
        return picker;
    }
    private Binding CreateValueBinding()
    {
        var valueBinding = new Binding();
        valueBinding.Mode = BindingMode.TwoWay;
        valueBinding.NotifyOnValidationError = true;
        valueBinding.ValidatesOnExceptions = true;
        valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
        valueBinding.Path = new PropertyPath(this.DataMemberBinding.Path.Path);
        return valueBinding;
    }
}
}