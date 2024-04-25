using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject controlPanel;

    public AudioMixer audioMixer;
    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadScene("Main");
    }

    public void OpenOptions()
    {
        // Disable the main menu panel
        mainMenuPanel.SetActive(false);

        // Enable the options panel
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        // Enable the main menu panel
        mainMenuPanel.SetActive(true);

        // Disable the options panel
        optionsPanel.SetActive(false);
    }

    public void OpenControls()
    {
        // Enable the main menu panel
        optionsPanel.SetActive(false);

        // Disable the options panel
        controlPanel.SetActive(true);
    }
    public void CloseControls()
    {
        // Enable the main menu panel
        optionsPanel.SetActive(true);

        // Disable the options panel
        controlPanel.SetActive(false);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

}
