using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreExplainText : MonoBehaviour
{
    private TMP_Text ExplainText;

    private RulesController _rulesController;

    void Start()
    {
        ExplainText = GetComponent<TMP_Text>();
        _rulesController = FindObjectOfType<RulesController>();
    }

    void Update()
    {
        ExplainText.text = _rulesController.IsGoldCollected 
            ? @"+1 All workers except worst worker(s).

+1 Best worker(s).

+1 Best thief(s)."
            : @"+3 Best thief(s).

Everybody else score zero.";
    }
}
