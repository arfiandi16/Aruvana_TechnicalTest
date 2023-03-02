using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{


    public void LoadScene(string a)
    {
        SceneManager.LoadScene(a);  
    }

    public void Quit()
    {
        Application.Quit();
    }
}

