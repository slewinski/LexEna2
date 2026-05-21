using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace LexEnaTrs
{
    /// <summary>
    /// A field used to display and edit content in RadDataForm.
    /// </summary>
    public class DataFormPasswordBoxField : DataFormDataField
    {
        /// <summary>
        /// Returns a control to display and edit the underlying data.
        /// </summary>
        /// <returns></returns>
        protected override Control GetControl()
        {
            DependencyProperty dependencyProperty = this.GetControlBindingProperty();
            PasswordBox control = new PasswordBox();
            if (this.DataMemberBinding != null)
            {
                control.SetBinding(dependencyProperty, this.DataMemberBinding);
            }

            control.SetBinding(PasswordBox.IsEnabledProperty, new Binding("IsReadOnly") { Source = this, Converter = new InvertedBooleanConverter() });

            return control;
        }

        /// <summary>
        /// Returns the dependency property of the control to be bound to the underlying
        /// data.
        /// </summary>
        /// <returns></returns>
        protected override DependencyProperty GetControlBindingProperty()
        {
            return PasswordBox.PasswordProperty;
        }
    }
}