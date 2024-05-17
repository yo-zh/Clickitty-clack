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
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject explosionsObject;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Button restartButton;
    [SerializeField] Button quitButton;

    [SerializeField] List<GameObject> targets;
    [SerializeField] private float spawnRate = 1.0f;

    public bool isGameActive;
    private void Start()
    {
        isGameActive = true;
        UpdateScore(0);

        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);

        StartCoroutine(SpawnTarget());
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

    private void GameOver()
    {
        isGameActive = false;
        gameOverScreen.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
