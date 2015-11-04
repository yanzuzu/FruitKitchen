using UnityEngine;
using System.Collections;

public static class CPDebug
{
    public static void Log(string p_str)
    {
        #if CP_DEBUG
        Debug.Log(p_str);
        #endif
    }
    public static void LogWarning(string p_str)
    {
        #if CP_DEBUG
        Debug.LogWarning(p_str);
        #endif
    }
    public static void LogError(string p_str)
    {
        #if CP_DEBUG
        Debug.LogError(p_str);
        #endif
    }
}
