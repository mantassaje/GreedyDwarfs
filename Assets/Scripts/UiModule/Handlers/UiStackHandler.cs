using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiStackHandler<TItem>
    where TItem : MonoBehaviour
{
    private MonoBehaviour _parent;
    private TItem _itemTemplate;
    private float _margin;
    private PoolHandler<TItem> _pool;

    private List<TItem> _drawenItems = new List<TItem>();

    public UiStackHandler(MonoBehaviour parent, TItem itemTemplate, float margin)
    {
        _parent = parent;
        _itemTemplate = itemTemplate;
        _margin = margin;
        _pool = new PoolHandler<TItem>(itemTemplate);
    }

    public void Redraw<T>(IEnumerable<T> iterator, Action<TItem, T> callback = null)
    {
        _drawenItems.ForEach(item => _pool.PoolAndDeactivate(item));
        _drawenItems.RemoveAll(value => true);

        var changeY = (_itemTemplate.GetComponent<RectTransform>().rect.height + _margin) * _itemTemplate.transform.lossyScale.y;

        var list = iterator.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            var position = new Vector3(
                _itemTemplate.transform.position.x,
                _itemTemplate.transform.position.y - changeY * i,
                _itemTemplate.transform.position.z
            );

            var item = _pool.GetActivated(position, _parent.transform);
            _drawenItems.Add(item);

            callback?.Invoke(item, list[i]);
        }
    }
}
