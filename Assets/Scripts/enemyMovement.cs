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

    public Vector2 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        queuePos = 0;
        playerPosition = playerScript.getPosition();
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

    void turn () {
        //if (position.x )
    }

    public void takeTurn (Vector2 position) {
        playerPosition = position;

    }
}
