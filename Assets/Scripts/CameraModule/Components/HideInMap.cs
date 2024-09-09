using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInMap : MonoBehaviour
{
    private Camera MapCamera;
    private SpriteRenderer SpriteRenderer;
    private Sprite _originalSprite;

    private void Awake()
    {
        MapCamera = FindObjectOfType<MapCamera>().GetComponent<Camera>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// HACK Unsafe hide object in map. If sprites will be switchabe in fututure it will not work.
    /// </summary>
    private void Update()
    {
        if (MapCamera != null
            && MapCamera.enabled)
        {
            if (_originalSprite == null)
            {
                _originalSprite = SpriteRenderer.sprite;
            }

            SpriteRenderer.sprite = null;
        }
        else if (_originalSprite != null)
        {
            SpriteRenderer.sprite = _originalSprite;
            _originalSprite = null;
        }
    }
}
