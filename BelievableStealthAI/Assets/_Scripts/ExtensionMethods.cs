using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange)
    {
        return 1.0f - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
    }

}
