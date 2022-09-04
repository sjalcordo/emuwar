using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserEnemy : enemyMovement
{
    double projX;
    double projY;
    double dir;

    
    public Vector2 findTarget()
    {
        //Finds the closer sight line to the player (either vertical or horizontal).
        //Tries to get on this sight line while maintaining some level of distance.
        //Two pathfinding things come out of this function: target and dir. The laser emu
        //navigates toward target the same way a regular emu might. Dir is used for laser firing logic.
       double x = position.x;
       double y = position.y;
       target.x = playerPos.x;
       target.y = playerPos.y;
       double diffX = playerPos.x - projX;
       double diffY = playerPos.y - projY;
       double dist = abs(diffX) + abs(diffY);
       if(abs(diffX)<abs(diffY))
       {
            dir = 2+diffY;
            if(dist<3)
            {
                target.y = (float)(y+diffY);
            } else {
                target.y = (float)y;
            }
       }
       dir = 1+diffX;
       if(dist<3)
            {
                target.x = (float)(x+diffY);
            } else {
                target.x = (float)x;
            }
        return target;
    }

    //This is where dir pays off. We can just do dir%2 to get our horizontal/vertical split.
    //From there, we check the player's x and y directly against our own (y must be the same, x
    //must be within 3 or vice versa) and if we get a match, we count the hit. 
    public void attack(double dir)
    {
        if(dir%2==0)
        {
            //horizontal
            if(playerPos.y==position.y)
            {
                if(abs(position.x-playerPos.x)<=3)
                {
                    playerMovement a = playerScript.GetComponent<playerMovement>();
                    a.damagePlayer();
                }
            }
        } else {
            //vertical
            if(playerPos.x==position.x)
            {
                if(abs(position.y-playerPos.y)<=3)
                {
                    playerMovement a = playerScript.GetComponent<playerMovement>();
                    a.damagePlayer();
                }
            }
        }
        
    }

    //Uses 2 movement to reach our target destination. Uses any remaining turns to fire lasers
    //in the direction decided in FindTarget.
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
                    attack(0);
                    break;
                case 6:
                    attack(1);
                    break;
                case 7:
                    attack(2);
                    break;
                case 8:
                    attack(3);
                    break;

        }
        queue[i] = 0;
    }

    public void takeTurn () {
        playerPos = playerScript.getPosition();
        //basic queue addition
        target = findTarget();
        double shotDir = dir;
        projX = position.x;
        projY = position.y;
        double diffX = target.x - projX;
        double diffY = target.y - projY;
        double dist = diffX+diffY;
        for(int i = 0; i<2; ++i)
        {
            if(dist>0)
            {
                move();
            }
        }
        while(queuePos<3)
        {
            queue[queuePos] = (int)(5 + shotDir);
            queuePos ++;
        }
        
       
        queuePos = 0;




    }
}
