using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
namespace WPF45URHO
{
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    /*public class Property1 : INotifyPropertyChanged
    {
        public Property1(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public object Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    public class Property
    {
        //private readonly ObservableCollection<Property1> properties = new ObservableCollection<Property1>();
        public string Name { get; set; }
        public object Value { get; set; }

        internal object target;
        internal PropertyInfo pi;
        /*public Record(params Property1[] properties)
        {
            foreach (var property in properties)
                Properties.Add(property);
        }

        public ObservableCollection<Property1> Properties
        {
            get { return properties; }
        }
    }*/
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    /*class EditableProperties : List<Property>
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal void add(object target, string method)
        {
            var pi = target.GetType().GetProperty(method);
            if (pi == null)
                return;
            var p = new Property();
            p.target = target;
            p.pi = pi;
            p.Value = pi.GetValue(target);
            p.Name = method;
            Add(p);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }*/
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    /// <summary>
    /// Logique d'interaction pour SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Loaded += SettingsWindow_Loaded;
            //DataGridColumn.GetCellContent(null);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.AutoGenerateColumns = false;
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;
            dataGrid.CanUserReorderColumns = false;

            dataGrid.Columns.Add(new PropertyColumn("Name"));
            dataGrid.Columns.Add(new PropertyColumn("Value"));

            /*var records = new ObservableCollection<Property>();
            records.Add(new Property() { Name = "Name", Value = "Paul" });
            records.Add(new Property() { Name = "First", Value = "Black" });
            dataGrid.ItemsSource = records;*/
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal void set(List<ObjectProperty> list)
        {
            dataGrid.ItemsSource = null;
            /*var list1 = new List<PropertyObject>()
            {
                new PropertyObject(){ name="Coucou", value="C'est moi!"},
                new PropertyObject(){ name="Bonjour", value=VerticalAlignment.Center},
                new PropertyObject(){ name="Activé", value=true}
            };*/
            dataGrid.ItemsSource = list;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    /*public class PropertyColumn : DataGridBoundColumn
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal PropertyColumn(string name)
        {
            Header = name;
            Binding = new Binding(name);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var prop = dataItem as Property;
            if (((Binding)Binding).Path.Path == "Name")
            {
                var tb = new TextBlock();
                tb.Text = prop.Name;
                return tb;
            }
            var value = prop.Value;
            if (value is string)
            {
                var tb = new TextBox();
                tb.Text = prop.Value as string;
                return tb;
            }
            if (value is bool)
            {
                var cb = new CheckBox();
                cb.IsChecked = (bool)value;
                return cb;
            }
            if (value is Int32)
            {
                
            }

            {
                var tb = new TextBlock();
                tb.Text = value.GetType().Name+":"+value.ToString();
                return tb;
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateElement(cell, dataItem);
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }*/
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
}
// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
