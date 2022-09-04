using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class level {
    public Vector2 startLocation;
    public List<Vector2> enemyLocations;
    public List<Vector2> wallLocations;
    public List<Vector2> laserLocation;
    public level(Vector2 startLocation_, List<Vector2> enemyLocations_, List<Vector2> wallLocations_, List<Vector2> laserLocation_) {
        startLocation = startLocation_;
        enemyLocations = enemyLocations_;
        wallLocations = wallLocations_;
        laserLocation = laserLocation_;
    }
}

public class levelManager : MonoBehaviour
{
    public List<level> levelList;
    public List<List<Vector2>> EnemieLocs;
    public List<List<Vector2>> LaserLocs;
    public List<List<Vector2>> WallsLocs;
    public List<Vector2> StartLocs;
    
    public playerMovement pm;

    int currLevel = 0;

    public Color laserEmuColor;

    public GameObject enemyPrefab;
    public GameObject laserPrefab;
    GameObject enemy;
    public GameObject wallPrefab;
    GameObject wall;

    public GameManager gm;

    bool levelClearing = false;

    // Start is called before the first frame update
    void Start()
    {

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
            currLevel = 0;
        }
        if (Input.GetKeyDown("2")) {
            clearLevel();
            startLevel(1);
            currLevel = 1;
        }
        if(Input.GetKeyDown("3")) {
            clearLevel();
            startLevel(2);
            currLevel = 2;
        }
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !gm.going) {
            clearLevel();
            Debug.Log(currLevel);
            startLevel(currLevel+1);
            if(currLevel+1<levelList.Count-1)
            {
                ++currLevel;
            }
        }

    }

    public void startLevel(int i) {

        pm.position = levelList[i].startLocation;
        foreach (Vector2 enemyLoc in levelList[i].enemyLocations) {
            enemy = Instantiate(enemyPrefab, new Vector3(enemyLoc.x, enemyLoc.y, transform.position.z), transform.rotation);
            enemy.GetComponent<enemyMovement>().position = enemyLoc;
            enemy.GetComponent<enemyMovement>().updateType(1);   
        }

        foreach(Vector2 laserLoc in levelList[i].laserLocation)
        {
            enemy = Instantiate(enemyPrefab, new Vector3(laserLoc.x, laserLoc.y, transform.position.z), transform.rotation);
            enemy.GetComponent<SpriteRenderer>().color = laserEmuColor;
            enemy.GetComponent<enemyMovement>().position = laserLoc;
            enemy.GetComponent<enemyMovement>().updateType(2);
        }


        foreach (Vector2 wallLoc in levelList[i].wallLocations) {
            wall = Instantiate(wallPrefab, new Vector3(wallLoc.x * 2.0f + 2.35f, wallLoc.y * 2.0f, transform.position.z), transform.rotation);
        }
        gm.walls = levelList[i].wallLocations;
        gm.enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool levelClearing = false;
    }

    public void clearLevel() {

        bool levelClearing = true;
        
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
