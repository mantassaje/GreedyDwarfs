using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BreakCacheCooldownText : MonoBehaviour
{
    public TMP_Text Text { get; private set; }
    private BreakCache _breakCacheAction;

    void Start()
    {
        _breakCacheAction = FindObjectsOfType<BreakCache>().First(action => action.PhotonView.IsMine);

        Text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_breakCacheAction.CooldownTimer.IsRunning)
        {
            float seconds = Mathf.FloorToInt(_breakCacheAction.CooldownTimer.SecondsRemaining);

            var secondsText = string.Format("{0:00}", seconds);

            Text.text = $"Search light cooldown - {secondsText}";
        }
        else
        {
            Text.text = "";
        }
    }
}
