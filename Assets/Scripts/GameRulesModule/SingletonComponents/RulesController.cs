using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class RulesController : MonoBehaviour
{
    public int GoldCount;
    public int GoldGoal = 3;
    public bool IsGameOver = false;

    public int HiddenCacheCountForOwner = 2;

    public PhotonView PhotonView { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    public void AddGoldToTotal()
    {
        GoldCount++;
        CheckGameOver();
        Sync();
    }

    private void CheckGameOver()
    {
        if (GoldCount >= GoldGoal)
        {
            IsGameOver = true;
            Debug.Log("Game over. Reloading in 5 seconds");
            Invoke(nameof(ReloadScene), 5);
        }
    }

    public void ReloadScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ReloadScene called.");
            var scene = SceneManager.GetActiveScene();
            PhotonNetwork.LoadLevel("Reload");
        }
    }

    public void SetHiddenCacheOwner(InteractActor player)
    {
        var caches = FindObjectsOfType<HideCache>()
            .Where(cache => cache.Owner == null)
            .ToList();

        for (int i = 0; i < HiddenCacheCountForOwner; i++)
        {
            var cache = caches.PickRandom();
            caches.Remove(cache);

            cache.SetOwner(player);
        }
    }

    private void Sync()
    {
        PhotonView.RPC(nameof(RpcSync), RpcTarget.AllBufferedViaServer, GoldCount);
    }

    [PunRPC]
    private void RpcSync(int goldCount)
    {
        GoldCount = goldCount;
        CheckGameOver();
    }
}
