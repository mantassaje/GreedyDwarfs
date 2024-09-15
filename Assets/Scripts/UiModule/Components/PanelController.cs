using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    private EndGameScorePanel _endGamePanel;
    private RulesController _rulesController;
    private ScorePanel _scorePanel;
    private Camera _mapCamera;

    void Start()
    {
        _endGamePanel = FindObjectOfType<EndGameScorePanel>();
        _endGamePanel.gameObject.SetActive(false);

        _rulesController = FindObjectOfType<RulesController>();
        _scorePanel = FindObjectOfType<ScorePanel>();

        _mapCamera = FindObjectOfType<MapCamera>().GetComponent<Camera>();
    }

    void Update()
    {
        _endGamePanel.gameObject.SetActive(_rulesController.IsGameOver);
        _scorePanel.gameObject.SetActive(_mapCamera.enabled);
    }
}
