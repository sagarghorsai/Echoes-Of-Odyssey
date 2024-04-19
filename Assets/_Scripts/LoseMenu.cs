using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseMenu : MonoBehaviour
{
  
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void LoadCredit()
    {
        SceneManager.LoadScene("Credit");
    }
}
