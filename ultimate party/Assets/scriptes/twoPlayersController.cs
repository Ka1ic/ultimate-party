using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twoPlayersController : MonoBehaviour
{
    [SerializeField] private GameObject[] mainPlayers;

    private player[] players;

    private int mainPlayerIndex = 0;

    private int indexOfMode = 0;

    public class player
    {
        public GameObject levelPlayer;

        public GameObject menuBuilderPlayer;

        public GameObject lvlBuilderPlayer;

        public GameObject buttonBuild;

        public bool lastState = true;
    }

    void Start()
    {
        players = new player[mainPlayers.Length];

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new player();

            players[i].levelPlayer = mainPlayers[i].transform.Find("level player").gameObject;

            players[i].menuBuilderPlayer = mainPlayers[i].transform.Find("menu builder player").gameObject;

            players[i].lvlBuilderPlayer = mainPlayers[i].transform.Find("lvl builder").gameObject.transform.Find("builder player").gameObject;

            players[i].buttonBuild = mainPlayers[i].transform.Find("lvl builder").gameObject.transform.Find("Canvas").gameObject.transform.Find("Button build").gameObject;

            if (i == mainPlayerIndex)
            {
                players[i].levelPlayer.GetComponent<playerController>().enabled = true;
            }
            else
            {
                players[i].levelPlayer.GetComponent<playerController>().enabled = false;
            }
        }
    }

    public void switchMode()
    {
        indexOfMode++;

        if(indexOfMode > 2)
        {
            indexOfMode = 0;
        }

        if(indexOfMode == 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (i == mainPlayerIndex)
                {
                    players[i].levelPlayer.GetComponent<playerController>().enabled = true;
                }
                else
                {
                    players[i].levelPlayer.GetComponent<playerController>().enabled = false;
                }

                players[i].lastState = true;
            }
        }
        else if(indexOfMode == 1)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (i == mainPlayerIndex)
                {
                    players[i].menuBuilderPlayer.GetComponent<menuBuilderControls>().enabled = true;
                }
                else
                {
                    players[i].menuBuilderPlayer.GetComponent<menuBuilderControls>().enabled = false;
                }
            }
        }
        else if(indexOfMode == 2)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (i == mainPlayerIndex)
                {
                    players[i].lvlBuilderPlayer.GetComponent<builderMovement>().enabled = true;

                    players[i].buttonBuild.SetActive(true);
                }
                else
                {
                    players[i].lvlBuilderPlayer.GetComponent<builderMovement>().enabled = false;

                    players[i].buttonBuild.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Q))
        {
            SwitchPlayer();
        }
    }

    void SwitchPlayer()
    {
        if (indexOfMode == 0)
        {
            players[mainPlayerIndex].lastState = players[mainPlayerIndex].levelPlayer.GetComponent<playerController>().enabled;

            players[mainPlayerIndex].levelPlayer.GetComponent<playerController>().enabled = false;

            players[mainPlayerIndex].levelPlayer.GetComponent<Animator>().SetBool("speed", false);

            players[mainPlayerIndex].levelPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            mainPlayerIndex++;

            if (mainPlayerIndex >= players.Length)
            {
                mainPlayerIndex = 0;
            }

            players[mainPlayerIndex].levelPlayer.GetComponent<playerController>().enabled = players[mainPlayerIndex].lastState;
        }
        else if (indexOfMode == 1)
        {
            players[mainPlayerIndex].menuBuilderPlayer.GetComponent<menuBuilderControls>().enabled = false;

            players[mainPlayerIndex].menuBuilderPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            mainPlayerIndex++;

            if (mainPlayerIndex >= players.Length)
            {
                mainPlayerIndex = 0;
            }

            players[mainPlayerIndex].menuBuilderPlayer.GetComponent<menuBuilderControls>().enabled = true;
        }
        else if (indexOfMode == 2)
        {
            players[mainPlayerIndex].lvlBuilderPlayer.GetComponent<builderMovement>().enabled = false;

            players[mainPlayerIndex].lvlBuilderPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            players[mainPlayerIndex].buttonBuild.SetActive(false);

            mainPlayerIndex++;

            if (mainPlayerIndex >= players.Length)
            {
                mainPlayerIndex = 0;
            }

            players[mainPlayerIndex].lvlBuilderPlayer.GetComponent<builderMovement>().enabled = true;

            players[mainPlayerIndex].buttonBuild.SetActive(true);
        }
    }
}
