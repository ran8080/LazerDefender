using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float secondsToWaitBeforeGameOver = 2f;

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad(secondsToWaitBeforeGameOver, "Game Over"));
    }

    private IEnumerator WaitAndLoad(float waitInterval, string sceneName)
    {
        yield return new WaitForSeconds(waitInterval);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGameScene() {
        FindObjectOfType<GameSession>().ResetGameSession();
        SceneManager.LoadScene("Game");
    }

    public void LoadStartMenu() {
        SceneManager.LoadScene("Start Menu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
