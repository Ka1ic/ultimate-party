using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class spawnPlayers : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private GameObject playerPref;

    [SerializeField] private GameObject pointOfSpawn;

    [SerializeField] private multipleTargetCamera mtc_lvlPlayers;

    [SerializeField] private bool isLobby = false;

    [SerializeField] private multipleTargetCamera mtc_builderPlayers = null;

    [SerializeField] private endAttemptControls eac = null;

    [SerializeField] private menuBuildsCreate mbc = null;

    [SerializeField] private scoreMenuControls smc = null;

    [SerializeField] private gameMapManager gmm = null;

    public void Awake()
    {
        if(!isLobby)
        {
            mtc_lvlPlayers.enabled = false;

            eac.enabled = false;

            smc.enabled = false;

            gmm.enabled = false;

            //mbc.enabled = false;
        }
    }

    public void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPref.name, pointOfSpawn.transform.position, Quaternion.identity);

        newPlayer();

        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        SendOptions sendOptions = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent(0, null, options, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch(photonEvent.Code)
        {
            case 0:
                newPlayer();
                break;
        }
    }

    private void newPlayer()
    {
        GameObject[] mainPlayers = GameObject.FindGameObjectsWithTag("mainPlayer");

        mtc_lvlPlayers.targets.Clear();

        foreach (GameObject i in mainPlayers)
        {
            mtc_lvlPlayers.targets.Add(i.transform.Find("level player").transform);
        }

        if(mtc_builderPlayers != null)
        {
            mtc_builderPlayers.targets.Clear();

            foreach (GameObject i in mainPlayers)
            {
                mtc_builderPlayers.targets.Add(i.transform.Find("lvl builder").transform.Find("builder player").transform);
            }
        }

        if(!isLobby)
        {
            if(mainPlayers.Length == dataHolder.numberJoinedPlayers)
            {
                eac.mainPlayers = mainPlayers;

                smc.mainPlayers = mainPlayers;

                gmm.mainPlayers = mainPlayers;

                GameObject[] menuBuilderPlayers = new GameObject[mainPlayers.Length];

                for(int i = 0; i < mainPlayers.Length; i++)
                {
                    menuBuilderPlayers[i] = mainPlayers[i].transform.Find("menu builder player").gameObject;
                }

                mbc.builderMenuPlayers = menuBuilderPlayers;

                //mbc.enabled = true;

                smc.enabled = true;

                eac.enabled = true;

                gmm.enabled = true;

                mtc_lvlPlayers.enabled = true;
            }
        }
    }
}
