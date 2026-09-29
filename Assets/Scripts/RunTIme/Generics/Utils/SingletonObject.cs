using UnityEngine;

/// <summary>
/// Provides a generic singleton pattern for Unity <see cref="MonoBehaviour"/> objects.
/// </summary>
/// <typeparam name="T">The type of the singleton component.</typeparam>
public class SingletonMonoObject<T> : MonoBehaviour where T: SingletonMonoObject<T>
{
    /// <summary>
    /// Stores the current singleton instance.
    /// </summary>
    private static T _instance;
    /// <summary>
    /// Gets the current singleton instance.
    /// </summary>
    public static T Instance => _instance;
    /// <summary>
    /// Initializes the singleton instance when the component is created.
    /// Destroys the current GameObject when another instance already exists.
    /// </summary>

    protected virtual void Awake()
    {
        if(!_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
    }
}
/// <summary>
/// Provides a generic singleton pattern for regular C# objects.
/// The singleton instance is created lazily when first accessed.
/// </summary>
/// <typeparam name="T">The type of the singleton object.</typeparam>
public class SingletonObject<T> where T : SingletonObject<T>, new()
{
    /// <summary>
    /// Stores the current singleton instance.
    /// </summary>
    private static T _instance;
    /// <summary>
    /// Gets the singleton instance, creating it when it does not already exist.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();

            return _instance;
        }
    }
    
}
