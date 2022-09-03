using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public movementQueue playerMQ;

    //ported from movementQueue.cs
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        playerMQ = player.GetComponent<movementQueue>();
        

        //ported from movementQueue.cs
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        // Starts the queue coroutine
        if (Input.GetButtonDown("Submit")) {
            // go through queue
            StartCoroutine(goThroughQueue());
        }
    }

    IEnumerator goThroughQueue() {
        playerMQ.queueStart();
        for (int i = 0; i < 3; ++i) {
            playerMQ.queueStep(i);
            //enemy.queueStep (implemented below)
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<enemyMovement>().turn(i);
                Debug.Log("did it");
            }
            yield return new WaitForSeconds(0.5f);
        }
        //Added setQueuePos because it was missing here
        playerMQ.setQueuePos(0);
        playerMQ.queueStop();
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<enemyMovement>().takeTurn();
        }
    }
}
