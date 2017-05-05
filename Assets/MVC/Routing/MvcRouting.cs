using System;
using MVC.Controllers;
using MVC.Views;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.Routing
{
    public class MvcRouting : IMvcRouting
    {
        private Dictionary<Type, object> _mvcObjects = new Dictionary<Type, object>();
        private const string MISSING_OBJ_MSG = "Routing failed, could not find object of type {0}.";
        private const string MISSING_METHOD_MSG = "Action failed, {0} does not have a method named {1}.";

        public void RegisterView (IView view)
        {
            RegisterObject(view);
        }

        public void RegisterController (IController controller)
        {
            RegisterObject(controller);
        }

        public void Action<C>(string action, params object[] args) 
            where C : IController
        {
            var controller = GetObject<C>();

            if (controller == null)
                return;

            var controllerType = typeof(C);
            var method = controllerType.GetMethod(action);
            if (method == null)
                throw new MissingMethodException(
                    string.Format(MISSING_METHOD_MSG, controllerType.FullName, action));

            method.Invoke(controller, args);
        }

        public void View<V>(object model) where V : IView
        {
            var view = GetObject<V>();

            if (view == null)
                return;

            view.Render(model);
        }

        private void RegisterObject (object obj)
        {
            _mvcObjects[obj.GetType()] = obj;
        }

        private T GetObject<T> ()
        {
            var type = typeof(T);
            if (_mvcObjects.ContainsKey(type))
                return (T)_mvcObjects[type];
            else
                throw new MissingComponentException(string.Format(MISSING_OBJ_MSG, type.Name));
        }
    }
}

