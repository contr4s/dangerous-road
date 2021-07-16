using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonFunctions
{
    public static IEnumerator InvokeFuncAfterNSeconds<T>(float n, Func<T> func)
    {
        yield return new WaitForSeconds(n);
        func();
    }

    public static IEnumerator InvokeFuncAfterNSeconds(float n, Action func)
    {
        yield return new WaitForSeconds(n);
        func();
    }
}
