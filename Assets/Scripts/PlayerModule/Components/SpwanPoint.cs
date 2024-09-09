using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpwanPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = new Color(0f, 0f, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
