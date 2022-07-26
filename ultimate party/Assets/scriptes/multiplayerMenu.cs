using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class multiplayerMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField createInput;

    [SerializeField] private TMP_InputField joinInput;

    [SerializeField] private levelLoader lvlLoader;

    [SerializeField] private string nameOfLevel;

    public void createRoom()
    {
        lvlLoader.photonCreateRoom(createInput.text, nameOfLevel);
    }

    public void joinRoom()
    {
        lvlLoader.photonJoinRoom(joinInput.text, nameOfLevel);
    }
}
