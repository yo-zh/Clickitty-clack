using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private int score;
    private int level;

    [SerializeField] int levelMultiplier;
    [SerializeField] float spawnRateAdjustmentForAllDifficulties; // delete after test
    private int explosions;

    [Header("GUI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    //
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;
    //
    [SerializeField] GameObject titleScreen;

    [SerializeField] List<GameObject> targets;
    [SerializeField] private float spawnRate = 1.0f;

    [SerializeField] GameObject explosionsObject;
    [SerializeField] GameObject trailObject;
    [SerializeField] TrailRenderer trail;
    public bool isGameActive;

    [Header("Sounds")]
    [SerializeField] AudioClip[] meowSounds = new AudioClip[5];
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip pingSound;

    private void Start()
    {
        trail = trailObject.transform.GetComponent<TrailRenderer>();
        trail.enabled = false;
        score = 0; 
        level = 1;
        levelText.text = "Level: " + level;
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }

        if (Input.GetMouseButton(0))
        {
            trail.enabled = true;
        }
        else
        {
            trail.enabled = false;
        }
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int rangomIndex = Random.Range(0, targets.Count);
            Instantiate(targets[rangomIndex]);
        }
    }

    public void PlayRandomMeow()
    {
        AudioClip randomClip = meowSounds[Random.Range(0, meowSounds.Length)];
        AudioSource.PlayClipAtPoint(randomClip, transform.position);
    }
    public void PlayExplosion()
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
    }

    public void UpdateScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score;

        if (score < 0)
        {
            GameOver();
        }

        if (score >= level * levelMultiplier)
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        AudioSource.PlayClipAtPoint(pingSound, transform.position);
        score = 0;
        scoreText.text = "Score: " + score;
        level++;
        levelText.text = "Level: " + level;
    }

    public void IncreaseExplosions()
    {
        explosionsObject.transform.GetChild(explosions).gameObject.GetComponent<RawImage>().color = Color.white;
        explosions++;

        if (explosions == explosionsObject.transform.childCount)
        {
            GameOver();
        }
    }

    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty * spawnRateAdjustmentForAllDifficulties; // delete multiplication after test
        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        UpdateScore(0);

        Cursor.visible = false;

        StartCoroutine(SpawnTarget());
    }

    private void GameOver()
    {
        isGameActive = false;
        gameOverScreen.gameObject.SetActive(true);

        Cursor.visible = true;
    }

    [SerializeField] void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PauseGame()
    {
        if (isGameActive)
        {
            isGameActive = false;
            pauseScreen.SetActive(true);

            Cursor.visible = true;

            Time.timeScale = 0.0f;
        }
        else
        {
            isGameActive = true;
            pauseScreen.SetActive(false);

            Cursor.visible = false;

            Time.timeScale = 1.0f;
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
