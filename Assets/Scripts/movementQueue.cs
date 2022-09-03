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
    public bool notMoving;

    public GameObject[] queueArray;
    public Sprite[] sprites;

    public GameObject[] enemies;

    //animation things
    public Animator aussieAnimator;
    public int currentAnim;
    // 0 = idle
    // 1 = attack hori
    public SpriteRenderer aussieSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        queuePos = 0;
        notMoving = true;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        if (queuePos < 3 && notMoving) {
            if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && pmScript.position.x < pmScript.boundX - 1) {
                // set queue array to 1
                queue[queuePos] = 1;
                switchSprite(queuePos, 1);
                ++queuePos;
            }
            else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && pmScript.position.x > -pmScript.boundX) {
                queue[queuePos] = 3;
                switchSprite(queuePos, 3);
                ++queuePos;
            }
            else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0 && pmScript.position.y < pmScript.boundY) {
                queue[queuePos] = 4;
                switchSprite(queuePos, 4);
                ++queuePos;
            }
            else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0 && pmScript.position.y > -pmScript.boundY) {
                queue[queuePos] = 2;
                switchSprite(queuePos, 2);
                ++queuePos;
            }
            else if (Input.GetKeyDown("l")) {
                queue[queuePos] = 5;
                switchSprite(queuePos, 5);
                ++queuePos;
            }
            else if (Input.GetKeyDown("k")) {
                queue[queuePos] = 6;
                switchSprite(queuePos, 6);
                ++queuePos;
            }
            else if (Input.GetKeyDown("j")) {
                queue[queuePos] = 7;
                switchSprite(queuePos, 7);
                ++queuePos;
            }
            else if (Input.GetKeyDown("i")) {
                queue[queuePos] = 8;
                switchSprite(queuePos, 8);
                ++queuePos;
            }
        }

        // Backspace erases the current location and moves the queue position bavk
        if (Input.GetKeyDown(KeyCode.Backspace) && queuePos > 0) {
            --queuePos;
            queue[queuePos] = 0;
            switchSprite(queuePos, 0);
        }

        aussieAnimator.SetInteger("currentAnim", currentAnim);
        
    }

    void turnStart() {
        foreach (GameObject enemy in enemies) {
            //commented this out temporarily... Not sure where this turnStart method is getting used
            //enemy.GetComponent<enemyMovement>().takeTurn(pmScript.getPosition());
        }
    }

    void switchSprite(int queuePos, int spriteNum) {
        queueArray[queuePos].GetComponent<SpriteRenderer>().sprite = sprites[spriteNum];
    }

    void shoot(int xOffset, int yOffset, Vector3 rotation) {
        bullet = Instantiate(bulletPrefab, new 
            Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), 
            transform.rotation * Quaternion.Euler(rotation));
    }

    void move(int x, int y) {
        if (    pmScript.position.x + x <= pmScript.boundX - 1 &&
                pmScript.position.x + x >= -pmScript.boundX &&
                pmScript.position.y + y <= pmScript.boundY &&
                pmScript.position.y + y >= -pmScript.boundY) {

            pmScript.position = new Vector2 (pmScript.position.x + x, pmScript.position.y + y);
        }
    }
    /*
    IEnumerator goThroughQueue() {
        notMoving = false;
        for (int i = 0; i < 3; ++i) {
            switch(queue[i]) {
                case 0:
                    continue;   
                // Switch case checks all of the possible actions
                case 1: // Move right
                    move(1, 0);
                    break;
                case 2: // Move down
                    move(0, -1);
                    break;
                case 3: // Move left
                    move(-1, 0);
                    break;
                case 4: // Move up
                    move(0, 1);
                    break;
                case 5:
                    // Creates a bullet in front of the direction
                    shoot(3, 0, new Vector3(0, 0, -90));

                    if (pmScript.position.x > -pmScript.boundX) {
                        move(-1, 0);
                    }
                    break;
                case 6:
                    shoot(0, -3, new Vector3(0, 0, 180));

                    if (pmScript.position.y < pmScript.boundY) {
                        move(0, 1);
                    }
                    break;
                case 7:
                    shoot(-3, 0, new Vector3(0, 0, 90));

                    if (pmScript.position.x < pmScript.boundX - 1) {
                        move(1, 0);
                    }
                    break;
                case 8:
                    shoot(0, 3, new Vector3(0, 0, 0));

                    if (pmScript.position.y > -pmScript.boundY) {
                        move(0, -1);
                    }
                    break;
            }
            queue[i] = 0;
            switchSprite(i, 0);
            yield return new WaitForSeconds(0.5f);
        }
        queuePos = 0;
        notMoving = true;
    }*/

    public void setQueuePos(int a)
    {
        queuePos = a;
    }

    public void queueStart() {
        notMoving = false;
    }

    public void queueStop() {
        notMoving = true;
    }

    public void queueStep(int i) {
        switch(queue[i]) {
            case 0:
                break;   
            // Switch case checks all of the possible actions
            case 1: // Move right
                move(1, 0);
                break;
            case 2: // Move down
                move(0, -1);
                break;
            case 3: // Move left
                move(-1, 0);
                break;
            case 4: // Move up
                move(0, 1);
                break;
            case 5: // shoot right
                // Creates a bullet in front of the direction
                setAussieAnim(1);
                aussieSpriteRenderer.flipX = false;

                break;
            case 6: //shoot down
                //setAussieAnim(2);

                break;
            case 7: //shoot left
                setAussieAnim(1);
                aussieSpriteRenderer.flipX = true;
                
                break;
            case 8: //shoot up
                //setAussieAnim(3);

                break;
        }
        queue[i] = 0;
        switchSprite(i, 0);
    }

    public void setAussieAnim(int i)
    {
        currentAnim = i;
    }

    public void triggerShoot(int i)
    {
        switch (i) {
            case 5://horizontal
                if (!aussieSpriteRenderer.flipX) //right
                {
                    shoot(3, 0, new Vector3(0, 0, -90));
                    if (pmScript.position.x > -pmScript.boundX)
                    {
                        move(-1, 0);
                    }
                }
                else // left
                {
                    shoot(-3, 0, new Vector3(0, 0, 90));
                    if (pmScript.position.x < pmScript.boundX - 1)
                    {
                        move(1, 0);
                    }
                }
                break;
            case 6://down
                shoot(0, -3, new Vector3(0, 0, 180));
                if (pmScript.position.y < pmScript.boundY)
                {
                    move(0, 1);
                }
                break;
            /* case 7://left
                shoot(-3, 0, new Vector3(0, 0, 90));
                if (pmScript.position.x < pmScript.boundX - 1)
                {
                    move(1, 0);
                }
                break;*/
            case 8://up
                shoot(0, 3, new Vector3(0, 0, 0));
                if (pmScript.position.y > -pmScript.boundY)
                {
                    move(0, -1);
                }
                break;
        }
            
    }

}
