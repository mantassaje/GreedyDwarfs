using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalText : MonoBehaviour
{
    private RulesController _ruleController;

    public TMP_Text Text { get; private set; }

    void Start()
    {
        _ruleController = FindObjectOfType<RulesController>();
        Text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        Text.text = $"Quota gold is {_ruleController.GoldGoal}      Team collected {_ruleController.GoldCount}";
    }
}
