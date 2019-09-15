using System;
using System.Collections;
using System.Collections.Generic;

namespace Conduit {
    public class ControllerLoader : IControllerLoader {
        private Dictionary<Type, IController> _controllers = new Dictionary<Type, IController>();
        IInitialController _initialController;

        public C LoadController<C>() where C : IController {
            ValidateIsRegistered(typeof(C));
            return (C)_controllers[typeof(C)];
        }

        public IController LoadController(Type controllerType){
            ValidateIsRegistered(controllerType);
            return _controllers[controllerType];
        }

        public void ValidateIsRegistered(Type controllerType) {
            var hasKey = _controllers.ContainsKey(controllerType);
            if (!hasKey) {
                throw new InvalidOperationException(
                    controllerType.Name +" not present in the scene."
                );
            }
        }

        public void Register(IController[] controllers) {
            foreach (IController controller in controllers) {
                if (_controllers.ContainsKey(controller.GetType()))
                    throw new InvalidOperationException(
                        controller.GetType().Name + " " +
                        "should be singleton, and only " +
                        "one instance shoud be in the scene.");

                if (controller is IInitialController)
                    _initialController = controller as IInitialController;
                
                _controllers[controller.GetType()] = controller;
            }
        }
        public IInitialController GetInitialController() {
            return _initialController;
        }

        public IEnumerator GetEnumerator() {
            foreach (KeyValuePair<Type, IController> entry in _controllers)
                yield return entry.Value;
        }
    }
}