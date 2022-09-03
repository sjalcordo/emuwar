using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementQueue : MonoBehaviour
{
    public int[] queue;
    public int queuePos;
    public playerMovement pmScript;

    // Start is called before the first frame update
    void Start()
    {
        queuePos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Input to change the position in the array
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && pmScript.position.x < pmScript.boundX) {
            queue[queuePos] = 1;
            ++queuePos;
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && pmScript.position.x > -pmScript.boundX) {
            queue[queuePos] = 2;
            ++queuePos;
        }
        else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0 && pmScript.position.y < pmScript.boundY) {
            queue[queuePos] = 3;
            ++queuePos;
        }
        else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0 && pmScript.position.y > -pmScript.boundY) {
            queue[queuePos] = 4;
            ++queuePos;
        }

        if (Input.GetButtonDown("Fire1")) {
            StartCoroutine(goThroughQueue());
        }
    }

    IEnumerator goThroughQueue() {
        for (int i = 0; i < 3; ++i) {
            if (queue[i] == 1) {
                pmScript.position = new Vector2 (pmScript.position.x + 1, pmScript.position.y);
            }
            else if (queue[i] == 2) {
                pmScript.position = new Vector2 (pmScript.position.x - 1, pmScript.position.y);
            }
            else if (queue[i] == 3) {
                pmScript.position = new Vector2 (pmScript.position.x, pmScript.position.y + 1);
            }
            else if (queue[i] == 4) {
                pmScript.position = new Vector2 (pmScript.position.x, pmScript.position.y - 1);
            }
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < 3; ++i) {
            queue[i] = 0;
        }
        queuePos = 0;
    }
}
