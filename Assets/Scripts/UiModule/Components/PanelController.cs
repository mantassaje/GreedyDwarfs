using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    private EndGameScorePanel _endGamePanel;
    private RulesController _rulesController;

    void Start()
    {
        _endGamePanel = FindObjectOfType<EndGameScorePanel>();
        _endGamePanel.gameObject.SetActive(false);

        _rulesController = FindObjectOfType<RulesController>();
    }

    // Update is called once per frame
    void Update()
    {
        _endGamePanel.gameObject.SetActive(_rulesController.IsGameOver);
    }
}
