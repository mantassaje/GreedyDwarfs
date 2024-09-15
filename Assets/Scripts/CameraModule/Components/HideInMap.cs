using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInMap : MonoBehaviour
{
    private Camera MapCamera;
    private SpriteRenderer SpriteRenderer;
    private Sprite _originalSprite;


    private Canvas Canvas;

    private void Awake()
    {
        MapCamera = FindObjectOfType<MapCamera>().GetComponent<Camera>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Canvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// HACK Unsafe hide object in map. If sprites will be switchabe in fututure it will not work.
    /// </summary>
    private void Update()
    {
        if (MapCamera != null
            && MapCamera.enabled)
        {
            if (SpriteRenderer != null)
            {
                SpriteRenderer.enabled = false;
            }

            if (Canvas != null)
            {
                Canvas.enabled = false;
            }
        }
        else
        {
            if (SpriteRenderer != null)
            {
                SpriteRenderer.enabled = true;
            }

            if (Canvas != null)
            {
                Canvas.enabled = true;
            }
        }
    }
}
