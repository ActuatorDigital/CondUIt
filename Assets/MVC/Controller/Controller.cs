using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Controller<M> : MonoBehaviour, IController
        where M : IModel {

    public Action<IModel> OnSaveChanges;

    IModel _context;
    public M Context {
        get { return (M)_context; }
        set { _context = value as IModel; }
    }

    List<MonoBehaviour> _views = new List<MonoBehaviour>();
    List<MonoBehaviour> _controllers = new List<MonoBehaviour>();

    private void Awake() {
        ConnectMVC();
    }

    private void Start() {
        HideViews();
    }

    private void HideViews()
    {
        foreach (MonoBehaviour v in _views)
            v.gameObject.SetActive(false);
    }

    public void ConnectMVC() {
        var behaviours = FindObjectsOfType<MonoBehaviour>();
        foreach (var b in behaviours) {
            if (b.GetType().GetInterfaces().Contains(typeof(IController)))
                _controllers.Add(b);
            if (b.GetType().GetInterfaces().Contains(typeof(IView)))
                _views.Add(b);
        }
    }

    public abstract void Display();

    public void Init(Action<IModel> onSave, IModel context) {
        OnSaveChanges = onSave;
        _context = context;
    }

    public void SaveContext() {
        OnSaveChanges.Invoke(_context);
    }

    public void Action<C>(
            string action,
            IModel context,
            params object[] args) where C : IController {

        Type controllerType = typeof(C);

        if (OnSaveChanges == null)
            throw new MissingComponentException(
                "OnSaveChanges not set for " + controllerType.GetType().FullName + "." +
                " Failed to Call action " + action + ".");

        var controller = _controllers.FirstOrDefault(
            c => c.GetType().IsAssignableFrom(controllerType));
        if (controller == null)
            throw new MissingComponentException(
                "Controller " + controllerType.GetType().FullName +
                " not found. Failed to Call action " + action + ".");

        var method = controllerType.GetMethod(action);
        if (method == null)
            throw new MissingMethodException(
                "Action " + action + " not found on " +
                controllerType.GetType().FullName + "." +
                " Failed to call action " + action + ".");

        (controller as IController).Init(OnSaveChanges, context);
        method.Invoke(controller, args);

    }

    public void View<V>(object viewModel) where V : IView {
        Type viewType = typeof(V);

        foreach (MonoBehaviour mbView in _views) {
            if (mbView.GetType().IsAssignableFrom(viewType)) {
                var view = (mbView as IView);
                view.ViewModel = viewModel;
                view.Render();
                mbView.gameObject.SetActive(true);
                return;
            }
        }

        throw new MissingComponentException(
            "Failed to Display " + viewType.FullName +
            ", View not found.");

    }

    void TypeCheck<T>(Type type) {
        // Check if we've been given a controller.
        if (type.IsAssignableFrom(typeof(T))) {
            var errorMsg = type.FullName +
                " is not a " + typeof(T).FullName;
            throw new ApplicationException(errorMsg);
        }
    }
}

public static class Extensions {

    public static IController FindControllerOfType(this UnityEngine.Object mb, Type contextType) {
        var controllerType = typeof(Controller<>).MakeGenericType(contextType);
        var controller = UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour))
            .FirstOrDefault(c => c.GetType().IsSubclassOf(controllerType))
                as IController;
        return controller;
    }

}