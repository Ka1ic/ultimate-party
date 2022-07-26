using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class levelLoader : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private GameObject[] anyOtherUI;

    [SerializeField] private bool skipScene = false;

    [SerializeField] private int skipToIndexScene = 0;

    [SerializeField] private bool connectToServer = false;

    private Slider slider;

    private TMP_Text progressText;

    private int indexScene = 0;

    private string nameOfLevel;

    public void Start()
    {
        slider = loadingScreen.transform.Find("Slider").gameObject.GetComponent<Slider>();

        progressText = slider.transform.Find("progress text").gameObject.GetComponent<TMP_Text>();

        if(skipScene)
        {
            loadScene(skipToIndexScene);
        }
    }

    public void loadScene(int sceneIndex)
    {
        if (connectToServer)
        {
            loadingScreen.SetActive(true);

            for (int i = 0; i < anyOtherUI.Length; i++)
            {
                anyOtherUI[i].SetActive(false);
            }

            indexScene = sceneIndex;

            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            StartCoroutine(LoadWithProgres(sceneIndex));
        }
    }

    public void photonCreateRoom(string nameOfRoom, string lvlName)
    {
        nameOfLevel = lvlName;

        loadingScreen.SetActive(true);

        for (int i = 0; i < anyOtherUI.Length; i++)
        {
            anyOtherUI[i].SetActive(false);
        }

        RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(nameOfRoom, roomOptions);
    }

    public void photonJoinRoom(string nameOfRoom, string lvlName)
    {
        nameOfLevel = lvlName;

        loadingScreen.SetActive(true);

        for (int i = 0; i < anyOtherUI.Length; i++)
        {
            anyOtherUI[i].SetActive(false);
        }

        PhotonNetwork.JoinRoom(nameOfRoom);
    }

    public override void OnJoinedRoom()
    {
        slider.value = 0.99f;

        progressText.text = "99%";

        PhotonNetwork.LoadLevel(nameOfLevel);
    }

    public override void OnConnectedToMaster()
    {
        StartCoroutine(LoadWithProgres(indexScene));
    }

    public void loadSceneAlreadyConnected(string name)
    {
        PhotonNetwork.LoadLevel(name);
    }

    IEnumerator LoadWithProgres(int sceneIndex)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        for (int i = 0; i < anyOtherUI.Length; i++)
        {
            anyOtherUI[i].SetActive(false);
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            progressText.text = progress * 100 + "%";

            yield return null;
        }
    }
}
