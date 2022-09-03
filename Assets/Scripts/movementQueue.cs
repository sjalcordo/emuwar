using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementQueue : MonoBehaviour
{
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
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && pmScript.position.x < pmScript.boundX) {
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

        if (Input.GetKeyDown(KeyCode.Backspace) && queuePos > 0) {
            --queuePos;
            queue[queuePos] = 0;
        }

        if (Input.GetButtonDown("Submit")) {
            StartCoroutine(goThroughQueue());
        }
    }

    IEnumerator goThroughQueue() {
        for (int i = 0; i < 3; ++i) {
            switch(queue[i]) {
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
