using System.Collections.Generic;
using System.Reflection;
using System;

namespace Conduit {
    public class ViewBinding {

        Dictionary<string, SavedDelegate> _activeBindings =
            new Dictionary<string, SavedDelegate>();
        Dictionary<string, MulticastDelegate> _pendingViewBindings =
            new Dictionary<string, MulticastDelegate>();

        public void BindService( Object service ){

            var properties = service.GetType()
                .GetProperties();
            foreach (var property in properties) {

                var isDelegate =
                    typeof(MulticastDelegate)
                    .IsAssignableFrom(property.PropertyType.BaseType);
                if (!isDelegate) continue;

                var matchedBinding = _pendingViewBindings.ContainsKey(property.Name);
                if (!matchedBinding) continue;

                var binding = _pendingViewBindings[property.Name];
                var matchType = binding.GetType() == property.PropertyType;
                if (!matchType) continue;

                var bindingDelegate = property.GetValue(service) as Delegate;
                var combinedDelegate = Delegate
                    .Combine(bindingDelegate, binding) ;
                property.SetValue( service, combinedDelegate );

                _pendingViewBindings.Remove(property.Name);
                _activeBindings.Add(property.Name, new SavedDelegate {
                    Service = service,
                    Delegate = combinedDelegate,
                    Field = property
                });
            }
            
        }

        public void BindView<T>(Action<T> bindMethod) {
            var methodName = bindMethod.Method.Name;
            _pendingViewBindings[methodName] = bindMethod;
        }

        public void UnbindView<T>(Action<T> unbindMethod) {
            var methodName = unbindMethod.Method.Name;
            var binding = _activeBindings[methodName];
            var removedDelegate = Delegate
                .Remove( binding.Delegate, unbindMethod );
            binding.Field.SetValue(binding.Service, removedDelegate);
        }

        struct SavedDelegate {
            public Object Service { get; set; }
            public Delegate Delegate { get; set; }
            public PropertyInfo Field { get; set; } 
        }

    }
}
