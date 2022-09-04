using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public movementQueue playerMQ;
    public bool going = false;

    bool stop = false;

    //ported from movementQueue.cs
    public GameObject[] enemies;
    public List<Vector2> walls;

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
        walls.Add(new Vector2(-1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // Starts the queue coroutine
        if (Input.GetButtonDown("Submit") && !stop) {
            // go through queue
            StartCoroutine(goThroughQueue());
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator goThroughQueue() {
        playerMQ.queueStart();
        going = true;
        for (int i = 0; i < 3; ++i) {
            playerMQ.queueStep(i);
            //enemy.queueStep (implemented below
            foreach(GameObject enemy in enemies) {
                if(enemy != null){
                    enemy.GetComponent<enemyMovement>().turn(i);
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
            
            foreach(GameObject enemy in enemies)
            {
                if(enemy != null) {
                    enemy.GetComponent<enemyMovement>().takeTurn();
                }
            }
        } 
        else {
            Destroy(player);
            SceneManager.LoadScene(0);
        }
        playerMQ.setQueuePos(0);
        playerMQ.queueStop();
        going = false;
    }

    public List<Vector2> getWalls(){
        return walls;
    }
}
