using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver = false;
    public Text txtFinalMessage;
    public AudioClip loseSFX;
    public string nextLevel;
    public AudioClip[] enemySFX;
    public Text scoretxt;
    public int enemiesKilled;
    public bool playOnce;

    public GameObject[] enemyType;

    public float spawnTime;
    public float timeSinceLastSpawn = 0;
    public float timePerEnemy = 3;
    public int spawnRate = 1;

    public float xMin = -5;

    public float xMax = 5;

    public float yMin = 20;

    public float yMax = 50;

    public float zMin = -5;

    public float zMax = 5;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = spawnRate * timePerEnemy;
        playOnce = false;
        isGameOver = false;
        enemiesKilled = 0;
        txtFinalMessage.text = "";
        UpdateScoreText();
    }

    private void Update()
    {
        if (timeSinceLastSpawn < spawnTime)
        {
            timeSinceLastSpawn += Time.deltaTime;
        }
        else
        {
            SpawnEnemies();
        }
    }


    void UpdateScoreText()
    {
        this.scoretxt.text = "Enemies Drunk: "+enemiesKilled;
    }


    public void LevelLost()
    {
        if (!playOnce)
        {
            PlayerPrefs.SetInt("lastScore", enemiesKilled);
            int bestScore = PlayerPrefs.GetInt("bestScore", 0);
            if (enemiesKilled > bestScore)
                PlayerPrefs.SetInt("bestScore", enemiesKilled);

            AudioSource.PlayClipAtPoint(this.loseSFX, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
            isGameOver = true;
            txtFinalMessage.text = "GAME OVER!";
            txtFinalMessage.enabled = true;
            Invoke("LoadLevel", 2);
            playOnce = true;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void RepeatLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        SceneManager.LoadScene(sceneName);
    }

    public void EnemyDestroyed()
    {
        enemiesKilled++;
        AudioSource.PlayClipAtPoint(this.enemySFX[Random.Range(0, enemySFX.Length)], GameObject.FindGameObjectWithTag("MainCamera").transform.position);
        UpdateScoreText();
    }

    void SpawnEnemies()
    {       
        for (int i= 0; i < spawnRate; i++){
             Vector3 enemyPosition;
            enemyPosition.x = Random.Range(xMin, xMax);
            enemyPosition.y = Random.Range(yMin, yMax);
            enemyPosition.z = Random.Range(zMin, zMax);

            GameObject newEnemy =
                Instantiate(enemyType[Random.Range(0, enemyType.Length)], transform.position + enemyPosition, transform.rotation)
                as GameObject;
        }
        timeSinceLastSpawn = 0;
        spawnRate++;
        spawnTime = spawnRate * timePerEnemy;
    }

}
