using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace WPF45URHO
{
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    class EnumItem
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal object value;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public override string ToString()
        {
            return EnumItem.ToString(value);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal static List<EnumItem> get(ObjectProperty obj, out EnumItem currentItem)
        {
            var currentValue = obj.Value;
            currentItem = null;
            var list = new List<EnumItem>();
            foreach (var val in Enum.GetValues(currentValue.GetType()))
            {
                if ((obj.excludedValues != null) && obj.excludedValues.Contains(val))
                    continue;
                var item = new EnumItem();
                item.value = val;
                if (val.ToString() == currentValue.ToString())
                    currentItem = item;
                list.Add(item);
            }
            if (obj.flags.HasFlag(PropertyFlags.ReverseComboValues))
                list.Reverse();
            return list;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal static string ToString(object obj)
        {
            if (obj == null)
                return "?";
            if (obj is Enum)
            {
                var str = obj.ToString();
                if (str.StartsWith("_"))
                    str = str.Substring(1);
                return str.Replace('_', ' ');
            }
            return obj.ToString();
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    public class PropertyColumn : DataGridBoundColumn
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal PropertyColumn(string name)
        {
            Header = name;
            Binding = new Binding(name);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            var b = (Binding as Binding).Path.Path;
            if (b == "Name")
                return GenerateElement(cell, dataItem);
            var obj = dataItem as ObjectProperty;
            if (obj.Value is string)
            {
                var tb = new TextBox();
                tb.Text = obj.Value as string;
                tb.LostKeyboardFocus += Tb_LostKeyboardFocus;
                tb.Tag = obj;
                return tb;
            }
            else if (obj.Value is Enum)
            {
                var cb = new ComboBox();
                EnumItem currentItem;
                cb.ItemsSource = EnumItem.get(obj, out currentItem);
                if (currentItem != null)
                    cb.SelectedItem = currentItem;
                else
                    cb.SelectedIndex = 0;
                cb.SelectionChanged += Cb_SelectionChanged;
                cb.Tag = obj;
                return cb;
            }
            else if (obj.Value is int)
            {
                if (obj.flags.HasFlag(PropertyFlags.UpDownSpinner))
                {
                    var sp = new IntegerUpDown();
                    sp.Maximum = (int)obj.max;
                    sp.Minimum = (int)obj.min;
                    sp.Value = (int)obj.Value;
                    sp.ValueChanged += Sp_ValueChanged;
                    sp.Tag = obj;
                    return sp;
                }
                else
                {
                    var tb = new TextBox();
                    tb.Text = obj.Value.ToString();
                    tb.LostKeyboardFocus += Tb_LostKeyboardFocus;
                    tb.Tag = obj;
                    return tb;
                }
            }
            else if (obj.Value is bool)
            {
                var cb = new CheckBox();
                cb.IsChecked = (bool)obj.Value;
                cb.Checked += Cb_CheckedChanged;
                cb.Unchecked += Cb_CheckedChanged;
                cb.Tag = obj;
                return cb;
            }
            return GenerateElement(cell, dataItem);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Sp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var iud = sender as IntegerUpDown;
            var obj = iud.Tag as ObjectProperty;
            if ((int)obj.Value != iud.Value.Value)
            {
                obj.Value = iud.Value.Value;
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Cb_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var obj = cb.Tag as ObjectProperty;
            if ((bool)obj.Value != cb.IsChecked)
            {
                obj.Value = cb.IsChecked;
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Tb_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            var tb = sender as TextBox;
            var obj = tb.Tag as ObjectProperty;
            if (obj.Value.ToString() != tb.Text)
            {
                obj.Value = tb.Text;
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            var obj = cb.Tag as ObjectProperty;
            obj.Value = (cb.SelectedItem as EnumItem).value;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var b = (Binding as Binding).Path.Path;
            var obj = dataItem as ObjectProperty;
            if (b == "Name")
            {
                var content = new TextBlock();
                content.Text = obj.Name;
                return content;
            }
            else
            {
                var content = new TextBlock();

                content.Text = EnumItem.ToString(obj.Value);
                return content;
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
}
// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
