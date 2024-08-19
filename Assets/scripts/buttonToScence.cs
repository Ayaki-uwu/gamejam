using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonToScence : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneToload;
    public void loadScence(string sceneToload){
        SceneManager.LoadScene(sceneToload);
    }
}
