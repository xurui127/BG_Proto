using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public static class Util
{
    public static IEnumerable<Type> GetTypesWith<T>() where T : Attribute
    {
        return from a in AppDomain.CurrentDomain.GetAssemblies()
               from t in a.GetTypes()
               let attributes = t.GetCustomAttributes(typeof(T), true)
               where attributes != null && attributes.Length > 0 
               select t;
    }
}
