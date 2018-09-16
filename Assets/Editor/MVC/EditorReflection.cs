using System.Collections.Generic;
using System.Linq;
using System;
using MVC;

public static class EditorReflection{

    private static List<Type> GetDomainTypes<T>(){
        var type = typeof(T);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
        return types.ToList();
    }

    public static List<Type> GetModelTypes(){
        return GetDomainTypes<IModel>()
				.Where( t => !t.IsInterface && !t.IsAbstract )
				.ToList();
    }

}