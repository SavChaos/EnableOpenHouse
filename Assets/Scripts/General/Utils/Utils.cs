using UnityEngine;

public static class Utils
{
    public static void PrintVec2(Vector2 vec, string tag = null)
    {
        string printStr = "";
        if (!string.IsNullOrEmpty(tag))
        {
            printStr += tag + "  ";
        }

        printStr += "x [" + vec.x + "]  y [" + vec.y + "]";

        Debug.Log(printStr);
    }

    public static void PrintVec3(Vector3 vec, string tag = null, Color _color = default(Color))
    {
        string printStr = "";
        if(!string.IsNullOrEmpty(tag))
        {
            printStr += tag + "  ";
        }

        Color color = (_color == default(Color)) ? Color.white : _color;
        printStr += ("x [" + vec.x + "]  y [" + vec.y + "]  z [" + vec.z + "]").Colored(color);

        Debug.Log(printStr);
    }

    public static Transform CreateMarker(Transform parent, float scale = 0.5f, Color32 _color = default(Color32))
    {
        GameObject markerSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        if (parent != null)
        {
            markerSphere.transform.SetParent(parent);
        }

        markerSphere.transform.localPosition = Vector3.zero;
        markerSphere.transform.localRotation = Quaternion.identity;
        markerSphere.transform.localScale = Vector3.one;
        markerSphere.transform.localScale *= scale;
        markerSphere.name = "marker";
        MonoBehaviour.Destroy(markerSphere.GetComponent<Collider>());

        markerSphere.GetComponent<MeshRenderer>().material.color = _color;

        return markerSphere.transform;
    }
}
