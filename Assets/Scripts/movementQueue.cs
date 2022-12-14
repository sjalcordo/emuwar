using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementQueue : MonoBehaviour
{
    // Initialization
    public int[] queue;
    public int queuePos;
    public playerMovement pmScript;
    public GameObject bulletLongPrefab;
    public GameObject bulletShortPrefab;
    private GameObject bullet;
    public bool notMoving;

    public GameObject[] queueArray;
    public Sprite[] sprites;

    public GameObject[] enemies;

    //animation things
    public Animator aussieAnimator;
    public int currentAnim;
    public Animator emuAnimator;
    // 0 = idle
    // 1 = attack hori
    public SpriteRenderer aussieSpriteRenderer;

    public GameManager gm;

    public bool isDefending;

    //sounds
    public AudioSource[] emuSounds = new AudioSource[3];
    // 0 is attack, 1 and 2 are death
    public AudioSource[] aussieSounds = new AudioSource[4];
    //0 is thud, 1 and 2 are hit, 3 is death

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
            if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) {
                // set queue array to 1
                queue[queuePos] = 1;
                switchSprite(queuePos, 1);
                ++queuePos;
            }
            else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) {
                queue[queuePos] = 3;
                switchSprite(queuePos, 3);
                ++queuePos;
            }
            else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0) {
                queue[queuePos] = 4;
                switchSprite(queuePos, 4);
                ++queuePos;
            }
            else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0) {
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
            else if (Input.GetButtonDown("Fire1")) {
                queue[queuePos] = 9;
                switchSprite(queuePos, 9);
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

    void shoot(float xOffset, float yOffset, Vector3 rotation, bool isLong) {
        if (isLong) {
            bullet = Instantiate(bulletLongPrefab, new 
                Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), 
                transform.rotation * Quaternion.Euler(rotation));
        }
        else {
            bullet = Instantiate(bulletShortPrefab, new 
                Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), 
                transform.rotation * Quaternion.Euler(rotation));
        }
        
    }

    void move(int x, int y) {
        bool emuInFront = false;
        bool wallInFront = false;
        foreach (GameObject enemy in enemies) {
            if (enemy != null) {
                if (pmScript.position.x + x == enemy.GetComponent<enemyMovement>().position.x &&
                    pmScript.position.y + y == enemy.GetComponent<enemyMovement>().position.y) {

                    emuInFront = true;
                }
            }
        }
        foreach (Vector2 wall in gm.getWalls()) {
            if (pmScript.position.x + x == wall.x && pmScript.position.y + y == wall.y) {
                wallInFront = true;
            }
        }
        if (    pmScript.position.x + x <= pmScript.boundX - 1 &&
                pmScript.position.x + x >= -pmScript.boundX &&
                pmScript.position.y + y <= pmScript.boundY &&
                pmScript.position.y + y >= -pmScript.boundY &&
                !emuInFront && !wallInFront) {

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

    public GameObject emuCheckX(int x, int y, int dir)
    {
        foreach(GameObject emu in enemies)
        {
            
            enemyMovement emuMov = emu.GetComponent<enemyMovement>();
            if((emuMov.position.x== x || emuMov.position.x == x+dir) && emuMov.position.y == y)
            {
                return emu;
            }
        }
        return null;
    }

    public GameObject emuCheckY(int x, int y, int dir)
    {
        foreach(GameObject emu in enemies)
        {
            enemyMovement emuMov = emu.GetComponent<enemyMovement>();
            if((emuMov.position.y== y || emuMov.position.y == y+dir) && emuMov.position.x == x)
            {
                return emu;
            }
        }
        return null;
    }
    

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
        //GameObject emu;
        isDefending = false;
        switch(queue[i]) {
            case 0:
                break;   
            // Switch case checks all of the possible actions
            case 1: // Move right
                aussieSpriteRenderer.flipX = false;
                move(1, 0);
                break;
            case 2: // Move down
                move(0, -1);
                break;
            case 3: // Move left
                aussieSpriteRenderer.flipX=true;
                move(-1, 0);
                break;
            case 4: // Move up
                move(0, 1);
                break;
            //shoot right
            /*
            case 5:
                
                setAussieAnim(1);
                aussieSpriteRenderer.flipX = false;
                // Creates a bullet in front of the direction
                shoot(3, 0, new Vector3(0, 0, -90));
                emu = emuCheckX((int)pmScript.position.x, (int)pmScript.position.y, 1);
                if(emu!=null)
                {
                    Destroy(emu);
                }
                if (pmScript.position.x > -pmScript.boundX) {
                    move(-1, 0);
                }
                break;
            //shoot down
            case 6:
                shoot(0, -3, new Vector3(0, 0, 180));

                emu = emuCheckY((int)pmScript.position.x, (int)pmScript.position.y, -1);
                if(emu!=null)
                {
                    Destroy(emu);
                }
                if (pmScript.position.y < pmScript.boundY) {
                    move(0, 1);
                }
                break;
            //shoot left
            case 7:
                shoot(-3, 0, new Vector3(0, 0, 90));
                emu = emuCheckX((int)pmScript.position.x, (int)pmScript.position.y, -1);
                if(emu!=null)
                {
                    Destroy(emu);
                }
                if (pmScript.position.x < pmScript.boundX - 1) {
                    move(1, 0);
                }
                break;
            //shoot up
            case 8:
                shoot(0, 3, new Vector3(0, 0, 0));
                emu = emuCheckY((int)pmScript.position.x, (int)pmScript.position.y, 1);
                if(emu!=null)
                {
                    Destroy(emu);
                }
                if (pmScript.position.y > -pmScript.boundY) {
                    move(0, -1);
                }
                break;
            */
            case 5: // shoot right
                // Creates a bullet in front of the direction
                setAussieAnim(1);
                aussieSpriteRenderer.flipX = false;

                break;
            case 6: //shoot down
                setAussieAnim(2);

                break;
            case 7: //shoot left
                setAussieAnim(1);
                aussieSpriteRenderer.flipX = true;
                
                break;
            case 8: //shoot up
                setAussieAnim(3);
                break;
            case 9:
                isDefending = true;
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
        bool wall1 = false;
        bool wall2 = false;
        switch (i) {
            case 5://horizontal
                if (!aussieSpriteRenderer.flipX) //right
                {
                    foreach (Vector2 wall in gm.getWalls()){
                        if (pmScript.position.x + 1 == wall.x && pmScript.position.y == wall.y) {
                            wall1 = true;
                        }
                        if (pmScript.position.x + 2 == wall.x && pmScript.position.y == wall.y) {
                            wall2 = true;
                        }
                    }
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int j = 0; j < enemies.Length; ++j) {  
                        if (wall1){

                        }
                        else if (wall2 && (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x + 1, pmScript.position.y)) ) {
                            
                            gm.enemies[j] = null;
                            shoot(2.5f, 0, new Vector3(0, 0, -90), false);
                            emuSounds[1].Play();

                            StartCoroutine(EmuDeath(j));
                        }
                        else if (wall2) {
                            shoot(2.5f, 0, new Vector3(0, 0, -90), false);
                        }
                        else if ((enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x + 1, pmScript.position.y) ||
                            (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x + 2, pmScript.position.y))) ) {
                                    
                            gm.enemies[j] = null;
                            shoot(2.5f, 0, new Vector3(0, 0, -90), true);
                            emuSounds[1].Play();
                            StartCoroutine(EmuDeath(j));
                        } else {
                            shoot(2.5f, 0, new Vector3(0, 0, -90), true);
                            Debug.Log(pmScript.position.x + ", " + enemies[j].GetComponent<enemyMovement>().position.x);
                        }
                    }
                    if (pmScript.position.x > -pmScript.boundX)
                    {
                        move(-1, 0);
                    }

                }
                else // left
                {
                    foreach (Vector2 wall in gm.getWalls()){
                        if (pmScript.position.x - 1 == wall.x && pmScript.position.y == wall.y) {
                            wall1 = true;
                        }
                        if (pmScript.position.x - 2 == wall.x && pmScript.position.y == wall.y) {
                            wall2 = true;
                        }
                    }
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    
                    for (int j = 0; j < enemies.Length; ++j) {  
                        if (wall1){

                        }
                        else if (wall2 && (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x - 1, pmScript.position.y)) ) {
                            
                            gm.enemies[j] = null;
                            shoot(-2.5f, 0, new Vector3(0, 0, 90), false);
                            emuSounds[1].Play();
                            StartCoroutine(EmuDeath(j));
                        }
                        else if (wall2) {
                            shoot(-2.5f, 0, new Vector3(0, 0, 90), false);
                        }
                        else if ((enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x - 1, pmScript.position.y) ||
                            (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x - 2, pmScript.position.y))) ) {
                                    
                            gm.enemies[j] = null;
                            shoot(-2.5f, 0, new Vector3(0, 0, 90), true);
                            emuSounds[1].Play();
                            StartCoroutine(EmuDeath(j));
                        } else {
                            shoot(-2.5f, 0, new Vector3(0, 0, 90), true);
                            Debug.Log(pmScript.position.x + ", " + enemies[j].GetComponent<enemyMovement>().position.x);
                        }
                    }
                    if (pmScript.position.x < pmScript.boundX - 1)
                    {
                        move(1, 0);
                    }
                }
                break;
            case 6://down
                foreach (Vector2 wall in gm.getWalls()){
                    if (pmScript.position.x == wall.x && pmScript.position.y - 1 == wall.y) {
                        wall1 = true;
                    }
                    if (pmScript.position.x == wall.x && pmScript.position.y - 2 == wall.y) {
                        wall2 = true;
                    }
                }
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    
                for (int j = 0; j < enemies.Length; ++j) {  
                    if ((enemies[j].GetComponent<enemyMovement>().position == 
                            new Vector2(pmScript.position.x, pmScript.position.y - 1) ||
                        (enemies[j].GetComponent<enemyMovement>().position == 
                            new Vector2(pmScript.position.x, pmScript.position.y - 2))) ) {
                                    
                        gm.enemies[j] = null;
                        emuSounds[2].Play();
                        enemies[j].GetComponent<enemyMovement>().switchEmuAnim(2);
                        
                        StartCoroutine(EmuDeath(j));

                    }
                    if (wall1){

                    }
                    else if (wall2 && (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x, pmScript.position.y - 1)) ) {
                            
                        gm.enemies[j] = null;
                        shoot(0, -2.5f, new Vector3(0, 0, 180), false);
                        emuSounds[2].Play();
                        enemies[j].GetComponent<enemyMovement>().switchEmuAnim(2);
                        StartCoroutine(EmuDeath(j));
                        
                    }
                    else if (wall2) {
                        shoot(0, -2.5f, new Vector3(0, 0, 180), false);
                    }
                    else if ((enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x, pmScript.position.y - 1) ||
                        (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x, pmScript.position.y - 2))) ) {
                                    
                        gm.enemies[j] = null;
                        shoot(0, -2.5f, new Vector3(0, 0, 180), true);
                        emuSounds[2].Play();
                        enemies[j].GetComponent<enemyMovement>().switchEmuAnim(2);
                        StartCoroutine(EmuDeath(j));
                    } else {
                        shoot(0, -2.5f, new Vector3(0, 0, 180), true);
                        Debug.Log(pmScript.position.x + ", " + enemies[j].GetComponent<enemyMovement>().position.x);
                    }
                }
                if (pmScript.position.x < pmScript.boundY)
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
                break;
            */
            case 8://up
                foreach (Vector2 wall in gm.getWalls()){
                    if (pmScript.position.x == wall.x && pmScript.position.y + 1 == wall.y) {
                        wall1 = true;
                    }
                    if (pmScript.position.x == wall.x && pmScript.position.y + 2 == wall.y) {
                        wall2 = true;
                    }
                }
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    
                for (int j = 0; j < enemies.Length; ++j) {  
                    if (wall1){

                    }
                    else if (wall2 && (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x, pmScript.position.y + 1)) ) {
                            
                        gm.enemies[j] = null;
                        shoot(0, 2.5f, new Vector3(0, 0, 0), false);
                        emuSounds[2].Play();
                        enemies[j].GetComponent<enemyMovement>().switchEmuAnim(2);
                        StartCoroutine(EmuDeath(j));
                    }
                    else if (wall2) {
                        shoot(0, 2.5f, new Vector3(0, 0, 0), false);
                    }
                    else if ((enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x, pmScript.position.y + 1) ||
                        (enemies[j].GetComponent<enemyMovement>().position == new Vector2(pmScript.position.x, pmScript.position.y + 2))) ) {
                                    
                        gm.enemies[j] = null;
                        shoot(0, 2.5f, new Vector3(0, 0, 0), true);
                        emuSounds[2].Play();
                        enemies[j].GetComponent<enemyMovement>().switchEmuAnim(2);
                        StartCoroutine(EmuDeath(j));
                    } else {
                        shoot(0, 2.5f, new Vector3(0, 0, 0), true);
                        Debug.Log(pmScript.position.x + ", " + enemies[j].GetComponent<enemyMovement>().position.x);
                    }
                }
                if (pmScript.position.y > -pmScript.boundY)
                {
                    move(0, -1);
                }

                break;
        }
            
    }

    IEnumerator EmuDeath(int i)
    {
        yield return new WaitForSeconds(.6f);
        Destroy(enemies[i]);
    }

}
