using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public TMP_InputField NickNameInput;
    public Button PlayButton;

    void Start()
    {
        PlayButton.onClick.AddListener(Play);
    }

    void Update()
    {
        PlayButton.interactable = !string.IsNullOrWhiteSpace(NickNameInput.text);
    }

    public void Play()
    {
        if (string.IsNullOrWhiteSpace(NickNameInput.text))
        {
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;

        PhotonNetwork.JoinOrCreateRoom(
            "SingleTestRoom",
            new RoomOptions()
            {
            },
            TypedLobby.Default
        );
    }
}
