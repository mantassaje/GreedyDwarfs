using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NickNameText : MonoBehaviour
{
    public TMP_Text Text { get; private set; }
    public PlayerData PlayerData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData == null)
        {
            PlayerData = FindObjectsOfType<PlayerData>().First(data => data.PhotonView.IsMine);
        }
        else
        {
            Text.text = $"{PlayerData.Name}            Steal {PlayerData.TotalStolenLootRound}              {PlayerData.TotalScore} (+{PlayerData.AddToTotalScoreRound})";
        }
    }
}
