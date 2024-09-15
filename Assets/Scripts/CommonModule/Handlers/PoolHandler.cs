using System;
using System.Collections.Generic;
using UnityEngine;


public class PoolHandler<T>
    where T : MonoBehaviour
{
    private Queue<T> _disabledItems = new Queue<T>();
    private T _template;

    public PoolHandler(T template)
    {
        _template = template;
    }

    public T GetActivated(Vector3 position, Transform parent)
    {
        var item = _disabledItems.DequeueOrDefault();

        if (item == null)
        {
            item = UnityEngine.Object.Instantiate(_template, position, _template.transform.rotation, parent);
        }
        else
        {
            item.transform.position = position;
            item.transform.SetParent(parent);
            item.transform.rotation = _template.transform.rotation;
        }

        item.gameObject.SetActive(true);

        return item;
    }

    public void PoolAndDeactivate(T item)
    {
        item.gameObject.SetActive(false);

        _disabledItems.Enqueue(item);
    }
}