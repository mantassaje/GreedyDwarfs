using Photon.Pun;
using System.Linq;
using UnityEngine;

public class CaveInSetup : MonoBehaviour
{
    public int RemoveCaveInsCount;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var caveIns = FindObjectsOfType<Cavein>().ToList();

            for (int i = 0; i < RemoveCaveInsCount; i++)
            {
                var caveIn = caveIns.PickRandom();
                caveIns.Remove(caveIn);
                caveIn.Destroy();
            }
        }
    }
}
