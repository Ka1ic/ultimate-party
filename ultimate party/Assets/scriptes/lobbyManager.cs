using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] mainPlayers;

    private player[] players;

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

        players = new player[mainPlayers.Length];

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new player();

            players[i].levelPlayer = mainPlayers[i].transform.Find("level player").gameObject;

            players[i].menuBuilderPlayer = mainPlayers[i].transform.Find("menu builder player").gameObject;

            players[i].lvlBuilder = mainPlayers[i].transform.Find("lvl builder").gameObject;
        }
    }
}


