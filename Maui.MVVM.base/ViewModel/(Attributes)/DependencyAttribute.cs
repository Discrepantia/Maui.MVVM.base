using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Maui.MVVM.Base.ViewModel
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    [ViewModelInitializer]
    public class DependencyAttribute : Attribute
    {
        public string[] dependencies;

        public DependencyAttribute(params string[] _dependencies)
        {
            dependencies = _dependencies ?? new string[0];
        }

        public string[] Dependencies
        {
            get { return dependencies; }
        }

        [ViewModelInitializer]
        public static void Initialize(ViewModelBase vm)
        {
            foreach (var item in vm.GetType().GetPropertiesWithAttributes<DependencyAttribute>())
            {
                var dps = item.Attributes.SelectMany(x => x.Dependencies).ToArray();

                if (typeof(ICommand).IsAssignableFrom(item.Property.PropertyType))
                {
                    vm.AddCommandSupplyer(item.Property.Name, dps);
                }
                else
                    vm.AddSupplier(item.Property.Name, dps);
            }
        }
    }
}
