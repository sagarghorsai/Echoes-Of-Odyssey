using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenStore : MonoBehaviour
{
    public GameObject Map;
    public GameObject Shop;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void Back()
    {
        Map.SetActive(true);
        Shop.SetActive(false);
        ResumeGame(); // Ensure the game is resumed when going back
    }

    private void PauseGame()
    {
        Map.SetActive(false);
        Shop.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    private void ResumeGame()
    {
        Map.SetActive(true);
        Shop.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }
}
