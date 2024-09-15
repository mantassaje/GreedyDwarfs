using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NickNameText : MonoBehaviour
{
    public TMP_Text Text { get; private set; }
    public Player Player;

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = Player.PhotonView.Owner.NickName;
    }
}
