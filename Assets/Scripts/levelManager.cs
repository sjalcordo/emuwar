using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class level {
    public Vector2 startLocation;
    public List<Vector2> enemyLocations;
    public List<Vector2> wallLocations;
    public level(Vector2 startLocation_, List<Vector2> enemyLocations_, List<Vector2> wallLocations_) {
        startLocation = startLocation_;
        enemyLocations = enemyLocations_;
        wallLocations = wallLocations_;
    }
}

public class levelManager : MonoBehaviour
{
    public List<level> levelList;
    public Vector2 levelOneStart;
    public List<Vector2> levelOneEnemies;
    public List<Vector2> levelOneWallLocations;
    
    public Vector2 levelTwoStart;
    public List<Vector2> levelTwoEnemies;
    public List<Vector2> levelTwoWallLocations;
    public playerMovement pm;

    public GameObject enemyPrefab;
    GameObject enemy;
    public GameObject wallPrefab;
    GameObject wall;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        levelList.Add(new level(levelOneStart, levelOneEnemies, levelOneWallLocations));
        levelList.Add(new level(levelTwoStart, levelTwoEnemies, levelTwoWallLocations));

        pm = GameObject.Find("Player").GetComponent<playerMovement>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        startLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            clearLevel();
            startLevel(0);
        }
        if (Input.GetKeyDown("2")) {
            clearLevel();
            startLevel(1);
        }
    }

    public void startLevel(int i) {
        pm.position = levelList[i].startLocation;
        foreach (Vector2 enemyLoc in levelList[i].enemyLocations) {
            enemy = Instantiate(enemyPrefab, new Vector3(enemyLoc.x, enemyLoc.y, transform.position.z), transform.rotation);
            enemy.GetComponent<enemyMovement>().position = enemyLoc;
        }
        foreach (Vector2 wallLoc in levelList[i].wallLocations) {
            wall = Instantiate(wallPrefab, new Vector3(wallLoc.x * 2.0f + 2.35f, wallLoc.y * 2.0f, transform.position.z), transform.rotation);
        }
        gm.walls = levelList[i].wallLocations;
        gm.enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void clearLevel() {
        
        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in currentEnemies) {
            Destroy(enemy);
        }
        GameObject[] currentWalls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in currentWalls) {
            Destroy(wall);
        }
    }
}
