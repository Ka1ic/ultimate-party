using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endAttempt : MonoBehaviour
{
    [SerializeField] private endAttemptControls eACScript;

    [SerializeField] private bool isFinish = false;

    private GameObject lastCollision = null;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && lastCollision != collision.gameObject)
        {
            collision.gameObject.GetComponent<PlayerControllerServer>().enabled = false;

            collision.gameObject.GetComponent<Animator>().SetBool("speed", false);

            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            StartCoroutine("End", collision.gameObject);
        }

        lastCollision = collision.gameObject;
    }

    IEnumerator End(GameObject player)
    {
        yield return new WaitForSeconds(3f);

        lastCollision = null;

        eACScript.endAttempt(player.transform.parent.gameObject, isFinish);
    }
}
