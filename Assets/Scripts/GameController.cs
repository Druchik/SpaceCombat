using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    //[SerializeField] GameObject[] enemies;

    public TextMesh scoreText;
    public TextMesh restartText;
    public TextMesh gameOverText;
    public TextMesh healthText;
    public TextMesh waveText;

    public int score = 0;
    public bool restart;
    public bool gameOver;
    private bool _alive;

    // Создание переменной «враг»
    public Transform[] enemies;
    //public Transform enemy2;

    // Временные промежутки между событиями, кол-во врагов
    public float timeBeforeSpawning;
    public float timeBetweenEnemies;
    public float timeBeforeWaves;
    public int enemiesPerWave;
    private int currentNumberOfEnemies;
    private int wave = 0;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnEnemies());
        scoreText.text = "Score: " + 0;
        healthText.text = "Health: " + 5;
        restartText.text = "";
        gameOverText.text = "";
        waveText.text = "Wave: " + wave;
        _alive = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if(restart)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
		
	}

    // Появление волн врагов
    IEnumerator SpawnEnemies()
    {
        // Начальная задержка перед первым появлением врагов
        yield return new WaitForSeconds(timeBeforeSpawning);
        // Когда таймер истекёт, начинаем производить эти действия
        while (true)
        {
            // Не создавать новых врагов, пока не уничтожены старые
            if(currentNumberOfEnemies <= 0)
            {
                float randDirection;
                float randDistance;
                wave++;
                UpdateWave();
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    // Задаём случайные переменные для расстояния и направления
                    randDistance = Random.Range(10, 25);
                    randDirection = Random.Range(0, 360);
                    // Используем переменные для задания координат появления врага
                    float posX = this.transform.position.x + (Mathf.Cos((randDirection) * Mathf.Deg2Rad) * randDistance);
                    float posY = this.transform.position.y + (Mathf.Sin((randDirection) * Mathf.Deg2Rad) * randDistance);

                    // Создаём врага на заданных координатах
                    if(wave > 2 && i == Random.Range(0, enemiesPerWave - 1))
                    {
                        Instantiate(enemies[1], new Vector3(posX, posY, 0), this.transform.rotation);
                    }
                    else if(wave%5 == 0)
                    {
                        i = 5;
                        Instantiate(enemies[2], new Vector3(posX, posY, 0), this.transform.rotation);
                    }
                    else
                        Instantiate(enemies[0], new Vector3(posX, posY, 0), this.transform.rotation);
                    currentNumberOfEnemies++;
                    Debug.Log("Создано врагов: " + currentNumberOfEnemies);
                    yield return new WaitForSeconds(timeBetweenEnemies);
                }
            }
            // Ожидание до следующей проверки
            yield return new WaitForSeconds(timeBeforeWaves);
            

            if (gameOver)
            {
                restartText.text = "If you want restart, press \"R\"";
                restart = true;
                break;
            }
        }
    }

    // Процедура уменьшения количества врагов в переменной
    public void KilledEnemy()
    {
        currentNumberOfEnemies--;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "You loose!";
        gameOver = true;
    }

    public void UpdateHealth(int newHealthValue)
    {
        healthText.text = "Health: " + newHealthValue;
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }

    public bool CheckAlive()
    {
        return _alive;
    }

    public void UpdateWave()
    {
        waveText.text = "Wave: " + wave;
    }

}
