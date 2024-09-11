using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSetupController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerDataPrefab;

    private GameObject _localPlayer;
    private GameObject _localPlayerData;

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
        SetupPlayerData();

        var spwanPoint = FindObjectsOfType<SpwanPoint>().ToList().PickRandom();

        _localPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, spwanPoint.transform.position, Quaternion.identity, 0);

        var hideComponent = _localPlayer.GetComponent<HideInMap>();
        Destroy(hideComponent);

        var mainCamera = FindObjectOfType<MainCamera>().GetComponent<SmoothCamera2D>();
        mainCamera.target = _localPlayer.transform;

        Invoke(nameof(InvokeDelayedSetup), 2);
    }

    private void SetupPlayerData()
    {
        _localPlayerData = FindObjectsOfType<PlayerData>()
            .ToList()
            .FirstOrDefault(data => data.PhotonView.IsMine)
            ?.gameObject;

        if (_localPlayerData == null)
        {
            _localPlayerData = PhotonNetwork.Instantiate(PlayerDataPrefab.name, Vector3.zero, Quaternion.identity, 0);
        }
    }

    /// <summary>
    /// Hack
    /// </summary>
    private void InvokeDelayedSetup()
    {
        //Another weird hack. For some reason DontDestroyOnLoad object are not created at master side.
        DontDestroyOnLoad(_localPlayerData);

        //This can break the game.
        _localPlayer.GetComponent<Player>().SetPlayerData(_localPlayerData.GetComponent<PlayerData>());

        //TODO change to request from master. Collide risk.
        FindObjectOfType<RulesController>().SetHiddenCacheOwner(_localPlayer.GetComponent<InteractActor>());

        _localPlayer.GetComponent<Player>().Notify("Joined game");
    }

}
