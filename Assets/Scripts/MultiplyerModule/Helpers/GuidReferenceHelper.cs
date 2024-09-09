using System;
using System.Linq;
using UnityEngine;

public static class GuidReferenceHelper
{
    /// <summary>
    /// Find GuidReference components gameObject with given id.
    /// Will return null if none found.
    /// </summary>
    public static GameObject FindGameObject(Guid id)
    {
        if (id == Guid.Empty)
        {
            Debug.LogError($"Searching for an object with GUID '{id}'.");
        }

        return GameObject.FindObjectsOfType<GuidReference>()
            .FirstOrDefault(guidRef => guidRef.Id == id)
            ?.gameObject;
    }

    /// <summary>
    /// Find Component whose GuidReference component has given id.
    /// Will return null if none found.
    /// </summary>
    public static GameObject FindGameObject(string guidId)
    {
        var id = new Guid(guidId);

        return FindGameObject(id);
    }

    /// <summary>
    /// Find Component whose GuidReference component has given id.
    /// Will return null if none found.
    /// </summary>
    public static T FindGameObject<T>(Guid id)
        where T: MonoBehaviour
    {
        if (id == Guid.Empty)
        {
            Debug.LogError($"Searching for an object with GUID '{id}'.");
        }

        var obj = FindGameObject(id);

        if (obj != null)
        {
            return obj.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Find Component whose GuidReference component has given id.
    /// Will return null if none found.
    /// </summary>
    public static T FindGameObject<T>(string guidId)
        where T : MonoBehaviour
    {
        var id = new Guid(guidId);

        return FindGameObject<T>(id);
    }

    public static Guid? GetId(GameObject gameObject)
    {
        var id = gameObject.GetComponent<GuidReference>()
            ?.Id;

        if (id == Guid.Empty)
        {
            Debug.LogError($"Found an object with GUID '{id}'.");
        }

        return id;
    }

    public static Guid? GetId(MonoBehaviour gameObject)
    {
        var id = gameObject.GetComponent<GuidReference>()
            ?.Id;

        if (id == Guid.Empty)
        {
            Debug.LogError($"Found an object with GUID '{id}'.");
        }

        return id;
    }
}
