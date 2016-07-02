using System;
using System.Reflection;

public class Singleton <T>  where T : new()
{
    private static T instance;
    static public T GetInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }

    static public bool IsInstance()
    {
        if (instance == null)
        {
            return false;
        }
        return true;
    }

    static public void ShutDown()
    {
        instance = default(T);
    }
}
