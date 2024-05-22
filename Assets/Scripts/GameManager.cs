using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score;
    private int explosions;

    [Header("GUI")]
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    [SerializeField] GameObject titleScreen;

    [SerializeField] List<GameObject> targets;
    [SerializeField] private float spawnRate = 1.0f;

    [SerializeField] GameObject explosionsObject;
    public bool isGameActive;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSecondsRealtime(spawnRate);
            int rangomIndex = Random.Range(0, targets.Count);
            Instantiate(targets[rangomIndex]);
        }
    }

    public void UpdateScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score;

        if (score < 0)
        {
            GameOver();
        }
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
        spawnRate /= difficulty;
        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        UpdateScore(0);

        StartCoroutine(SpawnTarget());
    }

    private void GameOver()
    {
        isGameActive = false;
        gameOverScreen.gameObject.SetActive(true);
    }

    [SerializeField] void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
