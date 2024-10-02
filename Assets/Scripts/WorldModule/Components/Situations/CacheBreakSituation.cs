using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CacheBreakSituation : MonoBehaviour
{
    public int MinPlayers;
    public float BreakPercent;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient
            && PhotonNetwork.CurrentRoom.PlayerCount <= MinPlayers)
        {
            var caches = FindObjectsOfType<HideCache>()
                .Where(cache => cache.Owner == null
                    && !cache.IsBroken)
                .ToList();

            var breakCount = (int)(caches.Count / 100f * BreakPercent);

            for (int i = 0; i < breakCount; i++)
            {
                var cache = caches.PickRandom();
                caches.Remove(cache);
                cache.ForceBreak();
            }
        }
    }
}
