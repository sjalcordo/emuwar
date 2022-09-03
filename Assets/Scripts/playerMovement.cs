using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Vector2 position = new Vector2(-4,0);
    public Vector3 offset = new Vector3(2.35f, 0, 0);
    public int boundX;
    public int boundY;

    public int health = 3;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Input to change the position in the array
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && position.x < boundX) {
            position = new Vector2(position.x + 1, position.y);
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && position.x > -boundX) {
            position = new Vector2(position.x - 1, position.y);
        }
        else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0 && position.y < boundY) {
            position = new Vector2(position.x, position.y + 1);
        }
        else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0 && position.y > -boundY) {
            position = new Vector2(position.x, position.y - 1);
        }
        */

        // Actually moves the circle/player around the tilemap
        transform.position = new Vector3(position.x * 2f, position.y * 2f, transform.position.z) + offset;
    }

    public int damagePlayer()
    {
        health--;
        Debug.Log("Ouch! " + health);
        return health;
    }
    
    public int getHealth()
    {
        return health;
    }
    public Vector2 getPosition () {
        return position;
    }
}
