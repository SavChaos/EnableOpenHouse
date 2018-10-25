using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

    public static string Colored(this string msg, Color color)
    {
        return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), msg);
    }

    public static void CopyTransformValues(this Transform transform, Transform copyTransform)
    {
        transform.position = copyTransform.position;
        transform.rotation = copyTransform.rotation;
        transform.localScale = copyTransform.localScale;
    }

    public static void CopyLocalTransformValues(this Transform transform, Transform copyTransform)
    {
        transform.localPosition = copyTransform.localPosition;
        transform.localRotation = copyTransform.localRotation;
        transform.localScale = copyTransform.localScale;
    }

}
