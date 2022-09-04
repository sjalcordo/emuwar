using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public Vector2 position = new Vector2(0,0);
    public Vector3 offset = new Vector3(2.35f, 0, 0);
    public int boundX;
    public int boundY;
    double projX;
    double projY;

    public playerMovement playerScript;
    public int[] queue = new int[3];
    public int queuePos;
    public Vector2 playerPos;
    public Vector2 target;

    public GameObject[] enemies;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        queuePos = 0;
        playerScript = GameObject.Find("Player").GetComponent<playerMovement>();
        playerPos = playerScript.getPosition();
        
        target = playerScript.getPosition();
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        takeTurn();

        //initialize queue array
    }


    // Update is called once per frame
    void Update()
    {
        /*
        // Input to change the position in the array
        if (Input.GetKeyDown("[6]") && position.x < boundX) {
            position = new Vector2(position.x + 1, position.y);
        }
        else if (Input.GetKeyDown("[4]") && position.x > -boundX) {
            position = new Vector2(position.x - 1, position.y);
        }
        else if (Input.GetKeyDown("[8]") && position.y < boundY) {
            position = new Vector2(position.x, position.y + 1);
        }
        else if (Input.GetKeyDown("[5]") && position.y > -boundY) {
            position = new Vector2(position.x, position.y - 1);
        }
        */



        transform.position = new Vector3(position.x * 2f, position.y * 2f, transform.position.z) + offset;
    }

    //written because Unity doesnt like it when I try to abs doubles
    double abs(double a)
    {
        if(a<0)
        {
            a*=-1;
        }
        return a;
    }


    //Check which turn to take and then do it. Only contains movement currently.
    public void turn (int i) {
        
        Debug.Log("did it" + queue[i]);
        switch(queue[i]) {   
                // Switch case checks all of the possible actions
                case 1: // Move right
                    step(1, 0);
                    break;
                case 2: // Move down
                    step(0, -1);
                    break;
                case 3: // Move left
                    step(-1, 0);
                    break;
                case 4: // Move up
                    step(0, 1);
                    break;
                case 5:
                    attack();
                    break;
        }
        queue[i] = 0;
    }

    
    public void step(int x, int y)
    {
        double newX = playerPos.x + x;
        double newY = playerPos.y + y;
        // bounds
        if (    position.x + x <= playerScript.boundX - 1 &&
                position.x + x >= -playerScript.boundX &&
                position.y + y <= playerScript.boundY &&
                position.y + y >= -playerScript.boundY) {
            bool obstacle = false;
            if (playerScript.getPosition().x == position.x + x && 
                playerScript.getPosition().y == position.y + y) {
                obstacle = true;
            }
            Debug.Log("(" + playerScript.getPosition().x + "," + playerScript.getPosition().y + ") (" + (position.x + x) + "," + (position.y + y) + ")");
            foreach (GameObject enemy in enemies) {
                if (enemy != null){
                    if (enemy.GetComponent<enemyMovement>().position.x == position.x + x && 
                        enemy.GetComponent<enemyMovement>().position.y == position.y + y) {
                        obstacle = true;
                    }
                }
            }
            foreach (Vector2 wall in gm.getWalls()) {
                if (wall.x == position.x + x && wall.y == position.y + y) {
                    obstacle = true;
                }                
            }

            if (!obstacle) {
                position = new Vector2(position.x + x, position.y + y);
            }

        }
    }

    public void attack()
    {
        playerPos = playerScript.getPosition();
        if((abs(playerPos.x-position.x)==1&&abs(playerPos.y-position.y)==0) 
        || (abs(playerPos.y-position.y)==1&&(abs(playerPos.x-position.x)==0)))
        {
            Debug.Log((abs(playerPos.x-position.x)==1&&abs(playerPos.y-position.y)==0) + " " + (abs(playerPos.y-position.y)==1&&(abs(playerPos.x-position.x)==0)));
            GameObject player = GameObject.Find("/Player");
            playerMovement a = player.GetComponent<playerMovement>();
            a.damagePlayer();
        }
        
    }

    //Added basic move logic
    public void move() {
        double diffX = target.x - projX;
        double diffY = target.y - projY;

        //+ or - 1 in the direction we want to move
        double xDir = diffX/abs(diffX);
        double yDir = diffY/abs(diffY);

        //checks if both x and y are offset from target
        if(diffX!=0&&diffY!=0)
        {
            //randomly decides between adding an x movement or y movement to the queue
            if(Random.Range(-1,1)<0)
            {
                //Adds a number to the queue using the xDir or yDir as an offset from a base number to 
                //Achieve the desired queue number without conditionals
                queue[queuePos]=2-(int)xDir;
                projX+=xDir;
                
                queuePos++;
                return;
            } else {
                queue[queuePos]=3+(int)yDir;
                
                queuePos++;
                projY+=yDir;
                return;
            }
        } else if(diffX!=0)
        {
            queue[queuePos]=2-(int)xDir;
            
            queuePos++;
            projX+=xDir;
            return;
        } else if(diffY!=0)
        {
            queue[queuePos] = 3+(int)yDir;
            projY+=yDir;
            queuePos++;
            return;
        }
        else {
            queue[queuePos] = 5;
            queuePos++;
        }
        //int xDir
    }



    //Target finding algorithm. Always picks adjacent tile to enemy nearest self.
    public Vector2 findTarget()
    {
       double x = position.x;
       double y = position.y;
       target.x = playerPos.x;
       target.y = playerPos.y;
       double diffX = playerPos.x - projX;
       double diffY = playerPos.y - projY;
       if(diffX!=0 && diffY !=0)
       {
           if(Random.Range(-1,1)<0)
           {
                target.x-=(float)(diffX/abs(diffX));
                return target;
           }
           target.y -= (float)(diffY/abs(diffY));
           return target;
       } else if(diffX  == 0 && diffY == 0)
       {
           return target;
       } else if (diffX!=0)
       {
           target.x-=(float)(diffX/abs(diffX));
           return target;
       }
       target.y-=(float)(diffY/abs(diffY));
       return target;
    }

    public void takeTurn () {
        playerPos = playerScript.getPosition();
        //basic queue addition
        target = findTarget();
        projX = position.x;
        projY = position.y;
        double diffX = target.x - projX;
        double diffY = target.y - projY;
        double dist = diffX+diffY;
        for(int i = 0; i<2; ++i)
        {
            move();
        }
        if(dist>5)
        {
            move();
        } else if(dist > 1)
        {
           queue[queuePos] = 6;
        } else {
           queue[queuePos] = 5;
        }
       
        queuePos = 0;




    }
}
