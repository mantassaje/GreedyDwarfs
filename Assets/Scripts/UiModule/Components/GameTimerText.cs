using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimerText : MonoBehaviour
{
    // Start is called before the first frame update

    private RulesController _ruleController;

    public TMP_Text Text { get; private set; }

    void Start()
    {
        _ruleController = FindObjectOfType<RulesController>();
        Text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float minutes = Mathf.FloorToInt(_ruleController.RoundTimer.SecondsRemaining / 60);
        float seconds = Mathf.FloorToInt(_ruleController.RoundTimer.SecondsRemaining % 60);

        Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
