using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Music;
    public void Play()
    {
        DontDestroyOnLoad(Music);
        SceneManager.LoadScene("Main");
    }
}
