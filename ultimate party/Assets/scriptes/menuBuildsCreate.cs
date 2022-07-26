using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuBuildsCreate : MonoBehaviour
{
    [SerializeField] private GameObject[] blockList;

    /*[HideInInspector]*/ public GameObject[] builderMenuPlayers;

    private float border_x = 9, border_y = 3;

    private GameObject[] blockListObj;

    public void createMenuChoice()
    {
        int randCount = Random.Range(6, 9);

        blockListObj = new GameObject[randCount];

        foreach (GameObject j in builderMenuPlayers)
        {
            j.GetComponent<menuBuilderControls>().blockList = new GameObject[randCount];

            j.GetComponent<menuBuilderControls>().indexesOfBuilds = new int[randCount];
        }

        float[] x = new float[randCount];

        float[] y = new float[randCount];

        for (int i = 0; i < randCount; i++)
        {
            if (i == 0)
            {
                x[i] = Random.Range(-border_x, border_x);

                y[i] = Random.Range(-border_y, border_y);

                int indexOfBlock = Random.Range(0, blockList.Length);

                GameObject block = Instantiate(blockList[indexOfBlock], new Vector3(x[i], y[i], 0), Quaternion.identity);

                blockListObj[i] = block;

                foreach (GameObject j in builderMenuPlayers)
                {
                    j.GetComponent<menuBuilderControls>().blockList[i] = block;

                    j.GetComponent<menuBuilderControls>().indexesOfBuilds[i] = indexOfBlock;
                }
            }
            else
            {
                bool close = false;

                x[i] = Random.Range(-border_x, border_x);

                y[i] = Random.Range(-border_y, border_y);

                for (int j = 0; j < i; j++)
                {
                    if (Vector3.Distance(new Vector3(x[j], y[j], 0), new Vector3(x[i], y[i], 0)) < 3f)
                    {
                        close = true;

                        i--;

                        break;
                    }
                }

                if (!close)
                {
                    int indexOfBlock = Random.Range(0, blockList.Length);

                    GameObject block = Instantiate(blockList[indexOfBlock], new Vector3(x[i], y[i], 0), Quaternion.identity);

                    blockListObj[i] = block;

                    foreach (GameObject j in builderMenuPlayers)
                    {
                        j.GetComponent<menuBuilderControls>().blockList[i] = block;

                        j.GetComponent<menuBuilderControls>().indexesOfBuilds[i] = indexOfBlock;
                    }
                }
            }
        }
    }

    public void clearMenu()
    {
        foreach (GameObject i in blockListObj)
        {
            if (i != null)
            {
                Destroy(i);
            }
        }
    }
}
