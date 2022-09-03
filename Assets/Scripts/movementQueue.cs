using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementQueue : MonoBehaviour
{
    // Initialization
    public int[] queue;
    public int queuePos;
    public playerMovement pmScript;
    public GameObject bulletPrefab;
    private GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        queuePos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Input to change the position in the array

        // If check horizontal axis, which direction, and if we are in bounds
        // 1 = right
        // 2 = down
        // 3 = left
        // 4 = up
        // 5 = shoot right
        // 6 = shoot down
        // 7 = shoot left
        // 8 = shoot up
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && pmScript.position.x < pmScript.boundX) {
            // set queue array to 1
            queue[queuePos] = 1;
            ++queuePos;
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && pmScript.position.x > -pmScript.boundX) {
            queue[queuePos] = 2;
            ++queuePos;
        }
        else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0 && pmScript.position.y < pmScript.boundY - 1) {
            queue[queuePos] = 3;
            ++queuePos;
        }
        else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0 && pmScript.position.y > -pmScript.boundY - 1) {
            queue[queuePos] = 4;
            ++queuePos;
        }
        else if (Input.GetKeyDown("l")) {
            queue[queuePos] = 5;
            ++queuePos;
        }
        else if (Input.GetKeyDown("k")) {
            queue[queuePos] = 6;
            ++queuePos;
        }
        else if (Input.GetKeyDown("j")) {
            queue[queuePos] = 7;
            ++queuePos;
        }
        else if (Input.GetKeyDown("i")) {
            queue[queuePos] = 8;
            ++queuePos;
        }

        // Backspace erases the current location and moves the queue position bavk
        if (Input.GetKeyDown(KeyCode.Backspace) && queuePos > 0) {
            --queuePos;
            queue[queuePos] = 0;
        }

        // Starts the queue coroutine
        if (Input.GetButtonDown("Submit")) {
            StartCoroutine(goThroughQueue());
        }
    }

    IEnumerator goThroughQueue() {
        for (int i = 0; i < 3; ++i) {
            switch(queue[i]) {
                // Switch case checks all of the possible actions
                case 1: // Move right
                    pmScript.position = new Vector2 (pmScript.position.x + 1, pmScript.position.y);
                    break;
                case 2: // Move down
                    pmScript.position = new Vector2 (pmScript.position.x - 1, pmScript.position.y);
                    break;
                case 3: // Move left
                    pmScript.position = new Vector2 (pmScript.position.x, pmScript.position.y + 1);
                    break;
                case 4: // Move up
                    pmScript.position = new Vector2 (pmScript.position.x, pmScript.position.y - 1);
                    break;
                case 5:
                    // Creates a bullet in front of the direction
                    bullet = Instantiate(bulletPrefab, new 
                        Vector3(transform.position.x + 3, transform.position.y, transform.position.z), 
                        transform.rotation * Quaternion.Euler(0, 0, -90));

                    if (pmScript.position.x > -pmScript.boundX) {
                        pmScript.position = new Vector2 (pmScript.position.x - 1, pmScript.position.y);
                    }
                    break;
                case 6:
                    bullet = Instantiate(bulletPrefab, new 
                        Vector3(transform.position.x, transform.position.y - 3, transform.position.z), 
                        transform.rotation * Quaternion.Euler(0, 0, 180));

                    if (pmScript.position.y < pmScript.boundY) {
                        pmScript.position = new Vector2 (pmScript.position.x, pmScript.position.y + 1);
                    }
                    break;
                case 7:
                    bullet = Instantiate(bulletPrefab, new 
                        Vector3(transform.position.x - 3, transform.position.y, transform.position.z), 
                        transform.rotation * Quaternion.Euler(0, 0, 90));

                    if (pmScript.position.x < pmScript.boundX - 1) {
                        pmScript.position = new Vector2 (pmScript.position.x + 1, pmScript.position.y);
                    }
                    break;
                case 8:
                    bullet = Instantiate(bulletPrefab, new 
                        Vector3(transform.position.x, transform.position.y + 3, transform.position.z), 
                        transform.rotation * Quaternion.Euler(0, 0, 0));

                    if (pmScript.position.y > -pmScript.boundY) {
                        pmScript.position = new Vector2 (pmScript.position.x, pmScript.position.y - 1);
                    }
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < 3; ++i) {
            queue[i] = 0;
        }
        queuePos = 0;
    }
}
