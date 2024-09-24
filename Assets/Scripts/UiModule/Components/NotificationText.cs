using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationText : MonoBehaviour
{
    public TMP_Text Text { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<TMP_Text>();
        Text.text = null;
    }

    public void Notify(string message, bool doOverwrite)
    {
        if (!doOverwrite && !string.IsNullOrWhiteSpace(Text.text))
        {
            return;
        }

        Text.text = message;
        CancelInvoke();
        Invoke(nameof(HideMessage), 5f);
    }

    private void HideMessage()
    {
        Text.text = null;
    }
}
