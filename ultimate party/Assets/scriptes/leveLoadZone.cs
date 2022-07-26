using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

[RequireComponent(typeof(BoxCollider2D))]
public class leveLoadZone : MonoBehaviour
{
    [SerializeField] private GameObject[] countedNumbers;

    [SerializeField] private GameObject lvlManager;

    [SerializeField] private string nameOfLvl;

    private List<GameObject> enteredPlayers = new List<GameObject>();

    private bool isCountdown = false;

    private Coroutine countdown;

    private int numPlayers = 0;

    private GameObject[] players;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        bool notCoincidence = true;

        for (int i = 0; i < enteredPlayers.Count; i++)
        {
            if (enteredPlayers[i] == collision.gameObject)
            {
                notCoincidence = false;

                break;
            }
        }

        if(notCoincidence)
        {
            enteredPlayers.Add(collision.gameObject);
        }

        players = GameObject.FindGameObjectsWithTag("mainPlayer");

        if (enteredPlayers.Count == players.Length && !isCountdown)
        {
            countdown = StartCoroutine(Countdown());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < enteredPlayers.Count; i++)
        {
            if(enteredPlayers[i] == collision.gameObject)
            {
                if(isCountdown)
                {
                    StopCoroutine(countdown);

                    for (int j = 0; j < countedNumbers.Length; j++)
                    {
                        TMP_Text number = countedNumbers[j].GetComponent<TMP_Text>();

                        number.color = new Color(number.color.r, number.color.g, number.color.b, 0);
                    }

                    isCountdown = false;
                }

                enteredPlayers.Remove(collision.gameObject);

                break;
            }
        }
    }

    IEnumerator Countdown()
    {
        isCountdown = true;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < countedNumbers.Length; i++)
        {
            TMP_Text number = countedNumbers[i].GetComponent<TMP_Text>();

            for (float j = number.color.a; j < 1.05f; j += 0.05f)
            {
                Color color = new Color(number.color.r, number.color.g, number.color.b, j);

                number.color = color;

                //playerConnect();

                yield return new WaitForSeconds(0.05f);
            }

            //playerConnect();

            yield return new WaitForSeconds(0.3f);

            for (float j = number.color.a; j > -0.01; j -= 0.05f)
            {
                Color color = new Color(number.color.r, number.color.g, number.color.b, j);

                number.color = color;

                //playerConnect();

                yield return new WaitForSeconds(0.05f);
            }
        }

        //playerConnect();

        yield return new WaitForSeconds(0.3f);

        isCountdown = false;

        dataHolder.numberJoinedPlayers = players.Length;

        lvlManager.GetComponent<levelLoader>().loadSceneAlreadyConnected(nameOfLvl);
    }

    void playerConnect()
    {
        if (players.Length < GameObject.FindGameObjectsWithTag("mainPlayer").Length)
        {
            for (int p = 0; p < countedNumbers.Length; p++)
            {
                TMP_Text numbe = countedNumbers[p].GetComponent<TMP_Text>();

                numbe.color = new Color(numbe.color.r, numbe.color.g, numbe.color.b, 0);
            }

            isCountdown = false;

            StopCoroutine(countdown);
        }
    }
}
