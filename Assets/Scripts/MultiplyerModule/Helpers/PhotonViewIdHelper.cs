using Photon.Pun;
using System;
using System.Linq;
using UnityEngine;

public static class PhotonViewIdHelper
{
    /// <summary>
    /// Find PhotonView components gameObject with given id.
    /// Will return null if none found.
    /// </summary>
    public static GameObject FindGameObject(int id)
    {
        return GameObject.FindObjectsOfType<PhotonView>()
            .FirstOrDefault(view => view.ViewID == id)
            ?.gameObject;
    }

    /// <summary>
    /// Find Component whose PhotonView component has given id.
    /// Will return null if none found.
    /// </summary>
    public static T FindGameObject<T>(int id)
        where T: MonoBehaviour
    {
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

    public static int? GetId(GameObject gameObject)
    {
        var id = gameObject.GetComponent<PhotonView>()
            ?.ViewID;

        return id;
    }

    public static int? GetId(MonoBehaviour gameObject)
    {
        var id = gameObject.GetComponent<PhotonView>()
            ?.ViewID;

        return id;
    }
}
