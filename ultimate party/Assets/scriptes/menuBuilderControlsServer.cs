using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class menuBuilderControlsServer : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] private float speed = 3.5f;

    [SerializeField] private GameObject choisePoint;

    public GameObject[] blockList; // Changed from menuBuildsCreate

    [SerializeField] private float distance;

    [SerializeField] private int indexOfPlayer;

    [SerializeField] private PhotonView view;

    public int[] indexesOfBuilds = new int[2];

    private GameObject lvlManager;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lvlManager = GameObject.Find("level manager");
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);

            if (Input.GetKeyUp(KeyCode.Return))
            {
                for (int i = 0; i < blockList.Length; i++)
                {
                    if (blockList[i] != null)
                    {
                        if (Vector3.Distance(choisePoint.transform.position, blockList[i].transform.position) < distance)
                        {
                            if (lvlManager != null)
                            {
                                lvlManager.GetComponent<gameMapManager>().chose(indexesOfBuilds[i], indexOfPlayer);
                            }

                            Destroy(blockList[i]);

                            this.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
