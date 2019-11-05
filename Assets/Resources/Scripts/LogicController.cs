using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicController : MonoBehaviour
{
    //LogicController access Variables
    public static LogicController lc;
    private GameObject[] ennemies;
    Transform asteroidParent;
    private GameObject player;
    private GameObject playerLaser;
    private GameObject alienPlayer;

    //Wave Logic Variables
    public Vector3 spawnCoords;
    private int maxEnnemyCount;
    public float spawnDelay;
    public float startDelay;
    public float waveDelay;
    public int radius;

    //UI Variables
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text levelInfo;
    public Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    //Game Logic Variables
    private bool gameOver;
    private bool restart;
    private float health;
    private int highscore;
    private int score;
    private int level;
    protected int currentEnemy;
    public void Awake()

    {
        lc = this;
        player = Resources.Load<GameObject>("Prefabs/Player");
        Instantiate(player);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Logic Setup
        level = 1;
        maxEnnemyCount = 5;
        currentEnemy = 0;
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        //AddScore(score);

        //Prefab Load / Wave start
        ennemies = Resources.LoadAll<GameObject>("Prefabs/Asteroids");
        asteroidParent = (new GameObject("AsteroidParent")).transform;
        playerLaser = Resources.Load<GameObject>("Prefabs/playerLaser");
        alienPlayer = Resources.Load<GameObject>("Prefabs/alienLaser");
        prefabDict.Add("playerLaser", playerLaser);
        prefabDict.Add("alienLaser", alienPlayer);
        StartCoroutine(SpawnEnnemies());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemy == 0)
        {
            level = 2;
        }
        switch (level)
        {
            case 2:
                maxEnnemyCount = 10;
                currentEnemy = 0;
                break;
            case 3:
                maxEnnemyCount = 15;
                currentEnemy = 0;
                break;

        }
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }

    IEnumerator SpawnEnnemies()
    {
        //Delay spawning
        yield return new WaitForSeconds(startDelay);

        //while (true)
        //{
            //if (currentEnemy == 0)
           // {
                //Spawning Loop
                for (int i = 0; i < maxEnnemyCount; i++)
                {
                    //Chooses a random prefab 
                    GameObject ennemy = ennemies[Random.Range(0, ennemies.Length)];
                    ennemy.transform.localScale = new Vector3(Random.Range(3f, 8f), Random.Range(3f, 7f), Random.Range(3f, 8f));
                    Vector3 spawnCoord = RandomPointOnCircleEdge(radius);
                    Quaternion rotation = Quaternion.identity;
                    Rigidbody ennemyRB = ennemy.GetComponent<Rigidbody>();
                    ennemyRB.drag = 0.2f;
                    Instantiate(ennemy, spawnCoord, rotation, asteroidParent);
                    currentEnemy++;
                    yield return new WaitForSeconds(spawnDelay);
                }
                //if (gameOver)
                //{
                //    restartText.text = "Press 'R' for Restart";
                //    restart = true;
                //    break;
                //}
                //yield return new WaitForSeconds(waveDelay);
            //}
        //}
    }


    //Finds a random Vector2 around Player radius to spawn Enemy
    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var Vector2 = Random.insideUnitCircle.normalized * radius;

        return new Vector3(Vector2.x, 0, Vector2.y);
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        currentEnemy--;
        UpdateLevel();
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
        restartText.text = "Press 'R' for Restart";
        restart = true;
    }

    public void UpdateLevel()
    {
        levelInfo.text = "Level " + level;
    }
}
