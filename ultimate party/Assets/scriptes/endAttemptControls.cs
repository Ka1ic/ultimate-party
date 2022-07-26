using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endAttemptControls : MonoBehaviour
{
    [HideInInspector] public GameObject[] mainPlayers;

    [HideInInspector] public player[] players;

    private int endedPlayerCount = 0;

    private int numWinnerPlayers = 0;

    private int earning = 200;

    public class player
    {
        public GameObject levelPlayer;

        public Vector3 startPositions;

        public bool isAttemptOver;
    }

    public void Start()
    {
        players = new player[mainPlayers.Length];

        for (int i = 0; i < mainPlayers.Length; i++)
        {
            players[i] = new player();

            players[i].levelPlayer = mainPlayers[i].transform.Find("level player").gameObject;

            players[i].startPositions = players[i].levelPlayer.transform.position;

            players[i].isAttemptOver = false;
        }
    }

    public void Update()
    {
        //Debug.Log("1 - " + players[0].isAttemptOver);
        //Debug.Log("2 - " + players[1].isAttemptOver);
    }

    public void endAttempt(GameObject mainPlayer, bool isFinish)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (mainPlayer.gameObject.transform.Find("level player").gameObject == players[i].levelPlayer)
            {
                if (!players[i].isAttemptOver)
                {
                    if (isFinish)
                    {
                        mainPlayer.GetComponent<playerInfo>().earndPoints += earning;

                        numWinnerPlayers++;
                    }

                    players[i].isAttemptOver = true;

                    endedPlayerCount++;
                }
            }
        }

        if (endedPlayerCount >= mainPlayers.Length)
        {
            bool ease = false;

            if (numWinnerPlayers == mainPlayers.Length)
            {
                foreach (GameObject i in mainPlayers)
                {
                    i.GetComponent<playerInfo>().earndPoints -= earning;
                }

                ease = true;
            }

            GetComponent<gameMapManager>().menuScoreReveal(ease);

            for (int i = 0; i < mainPlayers.Length; i++)
            {
                players[i].levelPlayer.transform.position = players[i].startPositions;

                players[i].levelPlayer.GetComponent<PlayerControllerServer>().enabled = true;

                players[i].isAttemptOver = false;
            }

            numWinnerPlayers = 0;

            endedPlayerCount = 0;
        }
    }
}
