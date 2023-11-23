using System.Reflection;

namespace LightMotor.Persistence.Tests;

public static class TestHelper
{
    /// <summary>
    /// Implements a new function to each object which determines a field's value inside the object
    /// </summary>
    /// <param name="sut">The object itself</param>
    /// <param name="name">The name of the field</param>
    /// <typeparam name="T">The type of the field</typeparam>
    /// <returns>The value of the field</returns>
    public static T GetFieldValue<T>(this object sut, string name)
    {
        var field = sut
            .GetType()
            .GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        return (T)field?.GetValue(sut)!;
    }

    /// <summary>
    /// Implements a new function to each object which determines a property's value inside the object
    /// </summary>
    /// <param name="sut">The object itself</param>
    /// <param name="name">The name of the property</param>
    /// <typeparam name="T">The type of the property</typeparam>
    /// <returns>The value of the property</returns>
    public static T GetPropertyValue<T>(this object sut, string name)
    {
        var field = sut
            .GetType()
            .GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        return (T)field?.GetValue(sut)!;
    }
}