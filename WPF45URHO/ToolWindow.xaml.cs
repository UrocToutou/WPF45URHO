using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WPF45URHO
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class ToolWindow : Window
    {
        public ToolWindow()
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;
        }

        private void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<PropertyItem>()
            {
                new PropertyItem(){ name="Coucou", value="C'est moi!"},
                new PropertyItem(){ name="Bonjour", value=VerticalAlignment.Center},
                new PropertyItem(){ name="Activé", value=true}
            };
            this.DataContext = list;
            dataGridProterties.DataContext = list;

            /*
            dataGridProterties.ItemsSource = list;
            dataGridProterties.HeadersVisibility = DataGridHeadersVisibility.None;
            dataGridProterties.AutoGenerateColumns = true;

            dataGridProterties.AddingNewItem += DataGridProterties_AddingNewItem;
            dataGridProterties.InitializingNewItem += DataGridProterties_InitializingNewItem;
            dataGridProterties.PreparingCellForEdit += DataGridProterties_PreparingCellForEdit;

            //DataGridTemplateColumn  DataTemplate
            var xx = new DataGridTemplateColumn();
            xx.CellTemplate = new DataTemplate();
            xx.CellTemplate.
            */
            dataGridProterties.AutoGenerateColumns = false;
            dataGridProterties.CanUserAddRows = false;
            dataGridProterties.CanUserDeleteRows = false;
            dataGridProterties.CanUserReorderColumns = false;
            dataGridProterties.ItemsSource = list;
            

            var xx = new DataGridTextColumn();
            xx.Binding = new Binding();

        }

        private void DataGridProterties_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            var obj = e.EditingElement.DataContext as PropertyItem;
            if (e.Column.Header.ToString()=="B")
            {
                var cb=new ComboBox();
                //e.EditingElement = new ComboBox();
                
            }
            switch (e.Column.Header.ToString())
            {
                case "Col1":
                    //OriginalValue = obj.MyProperty1;
                    break;
            }
        }

        private void DataGridProterties_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
          
        }

        private void DataGridProterties_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            
        }
    }

    class PropertyItem
    {
        public string Name
        {
            get
            {
                return name;
            }
        }
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                    this.value = value;
            }
        }
        public bool boolValue { get { return value is bool ? (bool)value:false; } set { if (this.value is bool) this.value = value; } }
        public string textValue { get { return value is string ? value as string: string.Empty; } set { if (this.value is string) this.value= value; } }
        public int intValue { get { return (value is int) ? (int)value : 0; } set { if (this.value is int) this.value= value; } }
        public string CAT
        {
            get
            {
                if (value is Enum)
                    return "enum";
                else if (value is bool)
                    return "bool";
                else
                    return "string";
            }
            set
            {
                this.value = value;
            }
        }
        internal string name;
        internal object value;
        public Visibility textVisible { get { return CAT == "string" ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility boolVisible { get { return CAT == "bool" ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility enumVisible { get { return CAT == "enum" ? Visibility.Visible : Visibility.Hidden; } }
    }
}
