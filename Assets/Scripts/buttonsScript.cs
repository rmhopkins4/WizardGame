using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class buttonsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        //Application.LoadLevel("SampleScene");
    }

    public void QuitButton()
    {
        //EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
