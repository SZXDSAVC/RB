using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//øÿ÷∆”Œœ∑∞¥≈•
public class GameManager : MonoBehaviour
{

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(2);
        }
        if(Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene("Main");
        }
    }
    public void ReturnGame()
    {
        SceneManager.LoadScene(2);

    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnMain()
    {      
        SceneManager.LoadScene("Main");
    }
}
