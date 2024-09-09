using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSetupController : MonoBehaviour
{
    public GameObject PlayerPrefab;

    private GameObject _localPlayer;

    private void Awake()
    {
        if (FindObjectOfType<MultiplayerController>() == null)
        {
            SceneManager.LoadScene("Setup");
            return;
        }
    }

    void Start()
    {
        var spwanPoint = FindObjectsOfType<SpwanPoint>().ToList().PickRandom();

        _localPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, spwanPoint.transform.position, Quaternion.identity, 0);

        var hideComponent = _localPlayer.GetComponent<HideInMap>();
        Destroy(hideComponent);

        var mainCamera = FindObjectOfType<MainCamera>().GetComponent<SmoothCamera2D>();
        mainCamera.target = _localPlayer.transform;

        Invoke(nameof(InvokeSetHiddenCacheOwner), 2);
    }

    /// <summary>
    /// Hack
    /// </summary>
    private void InvokeSetHiddenCacheOwner()
    {
        FindObjectOfType<RulesController>().SetHiddenCacheOwner(_localPlayer.GetComponent<InteractActor>());
    }

}
