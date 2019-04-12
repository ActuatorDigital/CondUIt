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

            var fields = service.GetType()
                .GetFields();
            foreach (var field in fields) {

                var isDelegate =
                    typeof(MulticastDelegate)
                    .IsAssignableFrom(field.FieldType.BaseType);
                if (!isDelegate) continue;

                var matchedBinding = _pendingViewBindings.ContainsKey(field.Name);
                if (!matchedBinding) continue;

                var binding = _pendingViewBindings[field.Name];
                var matchType = binding.GetType() == field.FieldType;
                if (!matchType) continue;

                var bindingDelegate = field.GetValue(service) as Delegate;
                var combinedDelegate = Delegate
                    .Combine(bindingDelegate, binding) ;
                field.SetValue( service, combinedDelegate );

                _pendingViewBindings.Remove(field.Name);
                _activeBindings.Add(field.Name, new SavedDelegate {
                    Service = service,
                    Delegate = combinedDelegate,
                    Field = field
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
            public FieldInfo Field { get; set; } 
        }

    }
}
