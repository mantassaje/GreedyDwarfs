using Photon.Pun;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSetupController : MonoBehaviour
{
    public GameObject PlayerPrefab;

    private GameObject _localPlayer;

    void Start()
    {
        Invoke(nameof(InitPlayer), 2f);
    }

    public void InitPlayer()
    {
        var spwanPoint = FindObjectsOfType<SpwanPoint>().ToList().PickRandom();

        _localPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, spwanPoint.transform.position, Quaternion.identity, 0);

        var hideComponent = _localPlayer.GetComponent<HideInMap>();
        Destroy(hideComponent);

        var mainCamera = FindObjectOfType<MainCamera>().GetComponent<SmoothCamera2D>();
        mainCamera.target = _localPlayer.transform;
    }

}
