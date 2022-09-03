using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public Vector2 position = new Vector2(0,0);
    public Vector3 offset = new Vector3(2.35f, 0, 0);
    public int boundX;
    public int boundY;

    public playerMovement playerScript;
    public int[] queue;
    public int queuePos;
    public Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        queuePos = 0;
        playerPos = playerScript.getPosition();

        //initialize queue array
        queue = new int[3];
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
        switch(queue[i]) {   
                // Switch case checks all of the possible actions
                case 1: // Move right
                    position = new Vector2(position.x + 1, position.y);
                    break;
                case 2: // Move down
                    position = new Vector2(position.x, position.y - 1);
                    break;
                case 3: // Move left
                    position = new Vector2(position.x - 1, position.y);
                    break;
                case 4: // Move up
                    position = new Vector2(position.x, position.y + 1);
                    break;
        }
        queue[i] = 0;
    }

    //Added basic move logic
    public void move(double  x,double y) {
        double diffX = playerPos.x - position.x;
        double diffY = playerPos.y - position.y;

        //+ or - 1 in the direction we want to move
        double xDir = diffX/abs(diffX);
        double yDir = diffY/abs(diffY);

        //checks if both x and y are offset from target
        if(xDir!=0&&yDir!=0)
        {
            //randomly decides between adding an x movement or y movement to the queue
            if(Random.Range(-1,1)<0)
            {
                //Adds a number to the queue using the xDir or yDir as an offset from a base number to 
                //Achieve the desired queue number without conditionals
                queue[queuePos]=2-(int)xDir;
                queuePos++;
                return;
            } else {
                queue[queuePos]=3+(int)yDir;
                queuePos++;
                return;
            }
        } else if(xDir!=0)
        {
            queue[queuePos]=2-(int)xDir;
            queuePos++;
            return;
        }
        queue[queuePos] = 3+(int)yDir;
        queuePos++;
        return;

        //int xDir
    }

    public void takeTurn () {
        playerPos = playerScript.getPosition();
        //basic queue addition
       
        for(int i = 0; i<3; ++i)
        {
            //move();
        }
        queuePos = 0;




    }
}
