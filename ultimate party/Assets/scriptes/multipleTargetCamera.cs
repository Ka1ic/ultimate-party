using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class multipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float smoothTime = 0.5f;

    [SerializeField] private float minZoom = 40f;

    [SerializeField] private float maxZoom = 10f;

    [SerializeField] private float zoomLimiter = 50f;

    [SerializeField] private GameObject lvlManager;

    [SerializeField] private bool lobby = false;

    private Vector3 velosity;

    private Camera cam;

    private bool isEnded;

    void Start()
    {
        lvlManager = GameObject.Find("level manager");

        cam = GetComponent<Camera>();
    }

    public void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        nullRefrence();

        Move();

        nullRefrence();

        Zoom();
    }

    void nullRefrence()
    {
        for(int i = 0; i < targets.Count; i++)
        {
            if(targets[i] == null)
            {
                targets.RemoveAt(i);

                i--;
            }
        }
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPosition();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velosity, smoothTime);
    }

    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds();

        bool first = true;

        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i] != null)
            {
                if (targets[i].gameObject.activeInHierarchy)
                {
                    if (targets[i].gameObject.CompareTag("Player"))
                    {
                        if (!lobby)
                        {
                            //Debug.Log(lvlManager.GetComponent<endAttemptControls>().players[i].isAttemptOver);

                            isEnded = lvlManager.GetComponent<endAttemptControls>().players[i].isAttemptOver;
                        }

                        if (lobby || !isEnded)
                        {
                            if (first)
                            {
                                first = false;

                                bounds = new Bounds(targets[i].position, Vector3.zero);
                            }

                            bounds.Encapsulate(targets[i].position);
                        }
                    }
                    else
                    {
                        if (first)
                        {
                            first = false;

                            bounds = new Bounds(targets[i].position, Vector3.zero);
                        }

                        bounds.Encapsulate(targets[i].position);
                    }
                }
            }
            else
            {
                targets.RemoveAt(i);

                i--;
            }
        }

        if (bounds.size.x > bounds.size.y)
        {
            return bounds.size.x;
        }
        else
        {
            return bounds.size.y;
        }
    }

    Vector3 GetCenterPosition()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        Bounds bounds = new Bounds();

        bool first = true;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                if (targets[i].gameObject.activeInHierarchy)
                {
                    if (targets[i].gameObject.CompareTag("Player"))
                    {
                        if (!lobby)
                        {
                            isEnded = lvlManager.GetComponent<endAttemptControls>().players[i].isAttemptOver;
                        }

                        if (lobby || !isEnded)
                        {
                            if (first)
                            {
                                first = false;

                                bounds = new Bounds(targets[i].position, Vector3.zero);
                            }

                            bounds.Encapsulate(targets[i].position);
                        }
                    }
                    else
                    {
                        if (first)
                        {
                            first = false;

                            bounds = new Bounds(targets[i].position, Vector3.zero);
                        }

                        bounds.Encapsulate(targets[i].position);
                    }
                }
            }
            else
            {
                targets.RemoveAt(i);

                i--;
            }
        }

        return bounds.center;
    }
}


