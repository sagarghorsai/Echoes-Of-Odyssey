using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinChecker : MonoBehaviour
{
    public GameObject parentObject;

    void Update()
    {
        // Check if the parentObject is assigned
        if (parentObject == null)
        {
            Debug.LogError("Parent object is not assigned!");
            return;
        }

        if (parentObject.transform.childCount == 0)
        {
            SceneManager.LoadScene("Win");
        }
    }
}