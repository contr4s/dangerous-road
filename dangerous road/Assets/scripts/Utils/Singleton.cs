using UnityEngine;

[DefaultExecutionOrder(-10)]
public class Singleton<T>: MonoBehaviour where T : Singleton<T>
{
    private static T _S;
    public static T S
    {
        get
        {
            if (!IsInitialized)
                Debug.LogErrorFormat("trying to get singleton instance {0} before it hasn't been set", typeof(T));
            return _S;
        }
    }

    public static bool IsInitialized => _S != null;

    protected virtual void Awake()
    {
        if (IsInitialized)
        {
            Debug.LogErrorFormat("Trying to instantiate a second instance of singleton {0}", typeof(T));
        }
        else
        {
            _S = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_S == this)
        {
            _S = null;
        }
    }
}
