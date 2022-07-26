using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class builderMovement : MonoBehaviour
{
    [SerializeField] private Tile[] builds;

    [Range(0, 10)] [SerializeField] private float speed = 3.5f;

    [SerializeField] private Joystick joystick;

    [SerializeField] private GameObject pointOfBuild;

    [SerializeField] private GameObject ghostLayer;

    private Tilemap ghostLayerTM;

    [SerializeField] private GameObject buttonBuildObj;
    
    [SerializeField] private Button buttonBuild;
    
    [SerializeField] private Image buttonBuildColor;

    [SerializeField] private bool PhoneControl = false;

    [SerializeField] private int indexOfPlayer;

    private Tilemap[] layersOfBuild = new Tilemap[3];

    private Tilemap buildLayerTM;

    private Vector3 lastPosition;

    private Tile build;

    bool builderModeEnabled = false;

    private Rigidbody2D rb;

    private Vector3 startPosition;

    private GameObject lvlManager;

    void Awake()
    {
        layersOfBuild[0] = GameObject.Find("lvl").transform.Find("Grid").gameObject.transform.Find("bc ground").gameObject.GetComponent<Tilemap>();

        layersOfBuild[1] = GameObject.Find("lvl").transform.Find("Grid").gameObject.transform.Find("trap layer").gameObject.GetComponent<Tilemap>();

        layersOfBuild[2] = GameObject.Find("lvl").transform.Find("Grid").gameObject.transform.Find("death build zone").gameObject.GetComponent<Tilemap>();

        ghostLayerTM = ghostLayer.GetComponent<Tilemap>();

        buttonBuildColor = buttonBuildObj.GetComponent<Image>();

        buttonBuild = buttonBuildObj.GetComponent<Button>();

        startPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();

        ghostLayerTM = ghostLayer.GetComponent<Tilemap>();

        lvlManager = GameObject.Find("level manager");

        if (builderModeEnabled)
        {
            lastPosition = pointOfBuild.transform.position;

            ghostLayerTM.SetTile(ghostLayerTM.WorldToCell(pointOfBuild.transform.position), build);
        }
    }

    void Update()
    {
        if(PhoneControl)
        {
            rb.velocity = new Vector2(joystick.Horizontal * speed, joystick.Vertical * speed);
        }
        else
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        }

        if (builderModeEnabled)
        {
            if (lastPosition != pointOfBuild.transform.position)
            {
                ghostLayerTM.SetTile(ghostLayerTM.WorldToCell(lastPosition), null);

                lastPosition = pointOfBuild.transform.position;

                ghostLayerTM.SetTile(ghostLayerTM.WorldToCell(pointOfBuild.transform.position), build);

                bool isEmpty = true;

                for (int i = 0; i < layersOfBuild.Length; i++)
                {
                    if (layersOfBuild[i].GetTile(ghostLayerTM.WorldToCell(pointOfBuild.transform.position)) != null)
                    {
                        isEmpty = false;
                    }
                }

                if (isEmpty)
                {
                    buttonBuildColor.color = new Color(1, 1, 1, 1);

                    buttonBuild.enabled = true;

                    ghostLayerTM.color = new Color(1, 1, 1);
                }
                else
                {
                    buttonBuildColor.color = new Color(1, 1, 1, 0.6f);

                    buttonBuild.enabled = false;

                    ghostLayerTM.color = new Color(1, 0, 0);
                }
            }
        }
    }

    public void startBuilding(int indexOfBuild)
    {
        build = builds[indexOfBuild];

        if (indexOfBuild == 0)
        {
            buildLayerTM = layersOfBuild[0];
        }
        else if (indexOfBuild == 1)
        {
            buildLayerTM = layersOfBuild[1];
        }

        builderModeEnabled = true;
    }

    public void endBuilding()
    {
        buildLayerTM.SetTile(buildLayerTM.WorldToCell(pointOfBuild.transform.position), build);

        ghostLayerTM.SetTile(ghostLayerTM.WorldToCell(pointOfBuild.transform.position), null);

        transform.position = startPosition;

        builderModeEnabled = false;

        lvlManager.GetComponent<gameMapManager>().build(indexOfPlayer);
    }
}
