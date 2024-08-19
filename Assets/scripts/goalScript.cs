using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goalScript : MonoBehaviour
{
    public string sceneToload;
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("targetbox") && !other.isTrigger){
            Debug.Log("Scence change");
            SceneManager.LoadScene(sceneToload);
        }
    }

    // IEnumerator loadLevel(){
    //     SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
    // }
}
