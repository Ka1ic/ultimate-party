using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class builderController : MonoBehaviour
{
    [SerializeField] private GameObject[] mainPlayers;

    [SerializeField] private GameObject buildMenu;

    [SerializeField] private GameObject lvl;

    [SerializeField] private GameObject intermediateCamera;

    [SerializeField] private GameObject camLvlPlayers;

    [SerializeField] private GameObject camLvlBuilders;

    [SerializeField] private GameObject mapBounds;

    [SerializeField] private Transform finish;

    [SerializeField] private GameObject deathBuildZone;

    [SerializeField] private GameObject winLetter;

    private player[] players;

    private Vector3[] menuBuiderStartPositions = new Vector3[7];

    private int numChosen = 0;

    private int[] chosenList;

    private Vector3[] camerasStartPosition = new Vector3[2];

    public class player
    {
        public GameObject levelPlayer;

        public GameObject menuBuilderPlayer;

        public GameObject lvlBuilder;
    }

    public void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("player"));

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("menuBuilderPlayer"), LayerMask.NameToLayer("menuBuilderPlayer"));

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("ground"), LayerMask.NameToLayer("menuBuilder"));

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("menuBuilder"), LayerMask.NameToLayer("menuBuilder"));

        camerasStartPosition[0] = camLvlPlayers.transform.position;

        camerasStartPosition[1] = camLvlBuilders.transform.position;

        chosenList = new int[mainPlayers.Length];

        players = new player[mainPlayers.Length];

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new player();

            players[i].levelPlayer = mainPlayers[i].transform.Find("level player").gameObject;

            players[i].menuBuilderPlayer = mainPlayers[i].transform.Find("menu builder player").gameObject;

            players[i].lvlBuilder = mainPlayers[i].transform.Find("lvl builder").gameObject;
        }

        for (int i = 0; i < 7; i++)
        {
            if(i < 4)
            {
                menuBuiderStartPositions[i] = new Vector3(-5.25f + i * 3.5f, -3.7f, 0);
            }
            else
            {
                menuBuiderStartPositions[i] = new Vector3(-3.5f + (i - 4) * 3.5f, -3.7f, 0);
            }
        }

        //menuBuiderStartPositions[1] = new Vector3(0, -3.7f, 0);
    }

    public void menuScoreReveal(bool ease)
    {
        camLvlPlayers.SetActive(false);

        camLvlPlayers.transform.position = camerasStartPosition[0];

        intermediateCamera.SetActive(true);

        foreach (GameObject i in mainPlayers)
        {
            i.SetActive(false);
        }

        int[] earndPoints = new int[mainPlayers.Length];

        for (int i = 0; i < mainPlayers.Length; i++)
        {
            earndPoints[i] = mainPlayers[i].GetComponent<playerInfo>().earndPoints;

            mainPlayers[i].GetComponent<playerInfo>().earndPoints = 0;
        }

        GetComponent<scoreMenuControls>().revealMenu(earndPoints, ease);
    }

    public void startBuilding()
    {
        foreach (GameObject i in mainPlayers)
        {
            i.SetActive(true);
        }

        GetComponent<menuBuildsCreate>().createMenuChoice();

        if (GetComponent<twoPlayersController>() != null)
        {
            GetComponent<twoPlayersController>().switchMode();
        }

        intermediateCamera.SetActive(true);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].levelPlayer.SetActive(false);

            //players[i].camera.SetActive(false);
        }

        lvl.SetActive(false);

        buildMenu.SetActive(true);

        for (int i = 0; i < players.Length; i++)
        {
            if (players.Length == 2)
            {
                players[i].menuBuilderPlayer.transform.position = menuBuiderStartPositions[i + 1];
            }
            else if (players.Length == 3)
            {
                players[i].menuBuilderPlayer.transform.position = menuBuiderStartPositions[i + 4];
            }
            else if (players.Length == 4)
            {
                players[i].menuBuilderPlayer.transform.position = menuBuiderStartPositions[i];
            }

            players[i].menuBuilderPlayer.SetActive(true);
        }
    }

    public void chose(int indexOfBuild, int indexOfPlayer)
    {
        chosenList[indexOfPlayer] = indexOfBuild;

        numChosen++;

        if(numChosen == mainPlayers.Length)
        {
            onBuilding(chosenList);

            numChosen = 0;
        }
    }

    public void onBuilding(int[] indexOfBuilds)
    {
        GetComponent<menuBuildsCreate>().clearMenu();

        buildMenu.SetActive(false);

        for(int i = 0; i < players.Length; i++)
        {
            players[i].menuBuilderPlayer.SetActive(false);
        }

        intermediateCamera.SetActive(false);

        lvl.SetActive(true);

        if (GetComponent<twoPlayersController>() != null)
        {
            GetComponent<twoPlayersController>().switchMode();
        }

        mapBounds.SetActive(true);

        deathBuildZone.SetActive(true);

        for (int i = 0; i < mainPlayers.Length; i++)
        {
            players[i].lvlBuilder.SetActive(true);

            players[i].lvlBuilder.transform.Find("builder player").gameObject.GetComponent<builderMovement>().startBuilding(indexOfBuilds[i]);
        }

        camLvlBuilders.SetActive(true);
    }

    public void build(int indexOfPlayer)
    {
        players[indexOfPlayer].lvlBuilder.SetActive(false);

        numChosen++;

        if (numChosen == mainPlayers.Length)
        {
            endBuilding();

            numChosen = 0;
        }
    }

    public void endBuilding()
    {
        for(int i = 0; i < players.Length; i++)
        {
            players[i].lvlBuilder.SetActive(false);
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i].levelPlayer.SetActive(true);
        }

        if (GetComponent<twoPlayersController>() != null)
        {
            GetComponent<twoPlayersController>().switchMode();
        }

        deathBuildZone.SetActive(false);

        mapBounds.SetActive(false);

        camLvlBuilders.SetActive(false);

        camLvlBuilders.transform.position = camerasStartPosition[1];

        camLvlPlayers.SetActive(true);
    }

    public void win(int indexWinPlayer)
    {
        for(int i = 0; i < mainPlayers.Length; i++)
        {
            if(i == indexWinPlayer)
                mainPlayers[i].SetActive(true);
            else
                mainPlayers[i].SetActive(false);
        }

        intermediateCamera.SetActive(false);

        camLvlPlayers.SetActive(true);

        players[indexWinPlayer].levelPlayer.transform.position = new Vector2(finish.position.x - 3, finish.position.y);

        players[indexWinPlayer].levelPlayer.GetComponent<playerController>().enabled = false;

        winLetter.SetActive(true); 
    }
}
