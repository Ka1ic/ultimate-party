using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreMenuControls : MonoBehaviour
{
    [HideInInspector] public GameObject[] mainPlayers;

    [SerializeField] private GameObject[] playersScores;

    [SerializeField] private GameObject scoreMenu;

    [SerializeField] private GameObject tooEasyText;

    Slider[] scores = new Slider[4];

    void Start()
    {
        for(int i = 0; i < playersScores.Length; i++)
        {
            scores[i] = playersScores[i].transform.Find("Slider").gameObject.GetComponent<Slider>();
        }

        for(int i = 0; i < mainPlayers.Length; i++)
        {
            playersScores[i].SetActive(true);

            playersScores[i].transform.Find("player image").gameObject.GetComponent<Image>().sprite = mainPlayers[i].GetComponent<playerInfo>().imageOfPlayer;
        }
    }

    public void revealMenu(int[] scoreValue, bool ease)
    {
        if(ease)
        {
            StartCoroutine("tooEasy", scoreValue);
        }
        else
        {
            StartCoroutine("animOfScore", scoreValue);
        }

        scoreMenu.SetActive(true);
    }

    IEnumerator tooEasy()
    {
        yield return new WaitForSeconds(0.5f);

        tooEasyText.SetActive(true);

        yield return new WaitForSeconds(3f);

        tooEasyText.SetActive(false);

        scoreMenu.SetActive(false);

        GetComponent<gameMapManager>().startBuilding();
    }

    IEnumerator animOfScore(int[] scoreValue)
    {
        int maxValue = 0;

        int[] startScore = new int[mainPlayers.Length];

        for (int i = 0; i < scoreValue.Length;i++)
        {
            startScore[i] = 0;

            if(scoreValue[i] > maxValue)
            {
                maxValue = scoreValue[i];
            }
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < maxValue; i++)
        {
            for(int j = 0; j < mainPlayers.Length; j++)
            {
                if(startScore[j] < scoreValue[j])
                {
                    scores[j].value += 1;

                    startScore[j] += 1;
                }
            }

            yield return new WaitForSeconds(0.005f);
        }

        yield return new WaitForSeconds(2f);

        scoreMenu.SetActive(false);

        bool win = false;

        int indexOfWinner = 0;

        foreach(Slider i in scores)
        {
            if(i.value >= 1000)
            {
                win = true;
            }

            if (!win) indexOfWinner++;
        }

        if(win)
        {
            GetComponent<gameMapManager>().win(indexOfWinner);
        }
        else
        {
            GetComponent<gameMapManager>().startBuilding();
        }
    }
}
