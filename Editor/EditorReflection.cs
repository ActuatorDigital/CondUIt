using Conduit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class EditorReflection {

    public static Assembly CurrentAssembly {
        get {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a =>
                    a.FullName.Contains("Assembly-CSharp") &&
                    !a.FullName.Contains("Editor"));
        }
    }

    private static List<Type> GetDomainTypes<T>() {
        var type = typeof(T);
        var types = CurrentAssembly.GetTypes()
            .Where(p => type.IsAssignableFrom(p));
        return types.ToList();
    }

    internal static List<Type> GetControllerTypes() {
        return GetDomainTypes<IController>()
            .Where(c => !c.IsInterface & !c.IsAbstract)
            .ToList();
    }

    public static List<Type> GetModelTypes() {
        return GetDomainTypes<IContext>()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .ToList();
    }

}