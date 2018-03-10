using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
namespace WPF45URHO
{
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    [Flags]
    internal enum PropertyFlags
    {
        None,
        ReverseComboValues,
        UpDownSpinner,
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
    class ObjectProperty
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal protected object target;
        internal protected string label;
        internal protected string keyName;
        internal protected FieldInfo field;
        internal protected PropertyInfo prop;
        internal protected object[] excludedValues = null;
        internal protected PropertyFlags flags = PropertyFlags.None;
        internal object min = null;
        internal object max = null;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        internal ObjectProperty(object target, string keyName, string label=null, PropertyFlags flags=PropertyFlags.None, object min=null, object max=null)
        {
            this.target = target;
            this.label = label!=null ? label:keyName;
            this.keyName = keyName;
            field = target.GetType().GetField(keyName);
            if (field == null)
                prop = target.GetType().GetProperty(keyName);
            this.flags = flags;
            this.min = min;
            this.max = max;
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public string Name
        {
            get
            {
                return label;
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        virtual public object Value
        {
            get
            {
                return (field != null) ? field.GetValue(target) : (prop != null) ? prop.GetValue(target) : null;
            }
            set
            {
                if (field != null)
                    field.SetValue(target, value);
                else if (prop != null)
                    prop.SetValue(target, value);
            }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    }
    // mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
}
// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
