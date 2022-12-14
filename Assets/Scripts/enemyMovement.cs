using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public int type = 1;
    public Vector2 position = new Vector2(0,0);
    public Vector3 offset = new Vector3(2.35f, 0, 0);
    public int boundX;
    public int boundY;
    public double projX;
    public double projY;

    public AudioClip hit1;
    public AudioClip hit2;
    public AudioSource source;
    public GameObject Laser;
    private GameObject bullet;
    public playerMovement playerScript;
    public int[] queue = new int[3];
    public int queuePos;
    public Vector2 playerPos;
    public Vector2 target;
    double seed = 0;
    double dir;

    public GameObject[] enemies;
    public GameManager gm;

    public Animator emuAnimator;
    public int currentEmuAnim;
    public SpriteRenderer emuSpriteRenderer;

    //sounds
    public AudioSource[] emuAttackSounds = new AudioSource[2];
    //0 is melee, 1 is laser

    // Start is called before the first frame update
    void Start()
    {
        queuePos = 0;
        playerScript = GameObject.Find("Player").GetComponent<playerMovement>();
        playerPos = playerScript.getPosition();
        
        target = playerScript.getPosition();
        
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
        takeTurn();

        emuAttackSounds[0] = GameObject.Find("emu click").GetComponent<AudioSource>();
        emuAttackSounds[1] = GameObject.Find("emu laser").GetComponent<AudioSource>();

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

        if(type==2)
        {
        }

        transform.position = new Vector3(position.x * 2f, position.y * 2f, transform.position.z) + offset;

        emuAnimator.SetInteger("emuAni", currentEmuAnim);
    }

    public void updateType(int type_)
    {
        type = type_;
    }
    //written because Unity doesnt like it when I try to abs doubles
    public double abs(double a)
    {
        if(a<0)
        {
            a*=-1;
        }
        return a;
    }


    //Check which turn to take and then do it. Only contains movement currently.
    public void turn (int i) {
        switch(type)
        {
            case 1:    
                Debug.Log("did it" + queue[i]);
                switch(queue[i]) {   
                        // Switch case checks all of the possible actions
                        case 1: // Move right
                            step(1, 0);
                            emuSpriteRenderer.flipX = true;
                            break;
                        case 2: // Move down
                            step(0, -1);
                            break;
                        case 3: // Move left
                        emuSpriteRenderer.flipX = false;
                        step(-1, 0);
                            break;
                        case 4: // Move up
                            step(0, 1);
                            break;
                        case 5:
                            attack();
                            break;
                }
                break;
            case 2:
                switch(queue[i]) {   
                        // Switch case checks all of the possible actions
                        case 1: // Move right
                        emuSpriteRenderer.flipX = true;
                        step(1, 0);
                            break;
                        case 2: // Move down
                            step(0, -1);
                            break;
                        case 3: // Move left
                        emuSpriteRenderer.flipX = false;
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

    public void switchEmuAnim(int i)
    {
        if(currentEmuAnim != 2)
            currentEmuAnim = i;
    }

    public void attack(double dir)
    {
        playerPos = playerScript.getPosition();
        double isX = dir%2;
        double isY = abs(dir%2-1);
        shoot((float)((2.5*(1-dir))*-isY), (float)(2.5*(2-dir)*-isX), new Vector3(0, 0, (float)(90*(dir))), true);
        if(dir%2==0)
        {
            //horizontal
            if(playerPos.y==position.y)
            {
                if(abs(position.x-playerPos.x)<=3)
                {
                    Debug.Log("Horiz shot from " + position + " to " + playerPos);
                    playerMovement a = playerScript.GetComponent<playerMovement>();
                    emuAttackSounds[0].Play();
                    if(position.x - playerPos.x > 0)
                    {
                        emuSpriteRenderer.flipX = false;
                    }
                    else if (position.x - playerPos.x < 0)
                    {
                        emuSpriteRenderer.flipX = true;
                    }

                    switchEmuAnim(1);
                    
                    a.damagePlayer();
                }
            }
        } else {
            //vertical
            if(playerPos.x==position.x)
            {
                if(abs(position.y-playerPos.y)<=3)
                {
                    Debug.Log("Vert shot from " + position + " to " + playerPos);
                    playerMovement a = playerScript.GetComponent<playerMovement>();
                    emuAttackSounds[0].Play();
                    switchEmuAnim(1);

                    a.damagePlayer();
                }
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
            emuAttackSounds[0].Play();
            switchEmuAnim(1);
            a.damagePlayer();
            if (position.x - playerPos.x > 0)
            {
                emuSpriteRenderer.flipX = false;
            }
            else if (position.x - playerPos.x < 0)
            {
                emuSpriteRenderer.flipX = true;
            }

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
            if(Random.Range(-1,1)<0+seed)
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
        target.x = playerPos.x;
        target.y = playerPos.y;
        double diffX = playerPos.x - position.x;
        double diffY = playerPos.y - position.y;
        double xDir = diffX/abs(diffX);
        double yDir = diffY/abs(diffY);
        double dist = abs(abs(diffX) + abs(diffY));
        if(type==2)
        {
            //Finds the closer sight line to the player (either vertical or horizontal).
            //Tries to get on this sight line while maintaining some level of distance.
            //Two pathfinding things come out of this function: target and dir. The laser emu
            //navigates toward target the same way a regular emu might. Dir is used for laser firing logic.
            double posx = position.x;
            double posy = position.y;
            if(abs(diffX)<abs(diffY))
            {
                Debug.Log("I'm here");
                seed = .8;
                dir = 2+yDir;
                if(dist<3)
                {
                    target.y = (float)(posy+diffY);
                } else {
                    target.y = (float)(posy+diffY);
                }
                return target;
            }
            Debug.Log("Actually here");
            seed = -.8;
            dir = 1+xDir;
            if(dist<3)
                {
                        target.x = (float)(posx+diffX);
                } else {
                    target.x = (float)(posx + diffX);
                }
            return target;
        }
       double x = position.x;
       double y = position.y;
       target.x = playerPos.x;
       target.y = playerPos.y;
        diffX = playerPos.x - projX;
        diffY = playerPos.y - projY;
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
        if(type==2)
        {
            playerPos = playerScript.getPosition();
        //basic queue addition
        target = findTarget();
        Debug.Log(target + " " + dir);
        double shotDir = dir;
        projX = position.x;
        projY = position.y;
        double diffX = target.x - projX;
        double diffY = target.y - projY;
        double dist = diffX+diffY;
        for(int i = 0; i<2; ++i)
        {
            if(abs(dist)>0)
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
        } else 
        {
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
        if(dist>3)
        {
            move();
        } else if(dist > 1)
        {
           //queue[queuePos] = 6;
           move();
        } else {
           queue[queuePos] = 5;
        }
       
        queuePos = 0;

        }


    }

    void shoot(float xOffset, float yOffset, Vector3 rotation, bool isLong) {
        
        if (isLong) {
            bullet = Instantiate(Laser, new 
                Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), 
                transform.rotation * Quaternion.Euler(rotation));
            
        }
        else {
            bullet = Instantiate(Laser, new 
                Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), 
                transform.rotation * Quaternion.Euler(rotation));
        }
        
    }
}
