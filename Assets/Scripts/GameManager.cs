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
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<enemyMovement>().takeTurn();
        }
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
        bool stop = false;
        playerMQ.queueStart();
        for (int i = 0; i < 3; ++i) {
            playerMQ.queueStep(i);
            //enemy.queueStep (implemented below
            for (int j = 0; j < enemies.Length; ++j) {  
                Debug.Log(enemies[j]);
                if(enemies[j] != null){
                    enemies[j].GetComponent<enemyMovement>().turn(i);
                }
                
                if(player.GetComponent<playerMovement>().getHealth()<=0)
                {
                    playerMQ.queueStart();
                    stop = true; 
                    break;
                }
            }
            //change to wait for animation to finish
            yield return new WaitForSeconds(0.5f);
            while(player.GetComponent<movementQueue>().currentAnim != 0){
                yield return new WaitForSeconds(0.1f);
            }
        }
        if(!stop)
        {
            //Added setQueuePos because it was missing here
            playerMQ.setQueuePos(0);
            playerMQ.queueStop();
            
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<enemyMovement>().takeTurn();
            }
        }
    }
}
