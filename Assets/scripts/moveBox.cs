using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBox : MonoBehaviour
{
    public Rigidbody2D boxRigidBody;

    public bool boxInRange;

    void Update(){
        if (boxInRange){
            boxRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("wallcollider") || other.CompareTag("nontargetbox")
        ||other.CompareTag("targetbox") ){
            Debug.Log("Toucs wall");
            boxInRange = true;
            // boxRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    // private void OnTriggerExit2D(Collider2D other){
    //     if (!other.CompareTag("wallcollider")){
    //         boxRigidBody.constraints = originalConstraints;
    //     }
    // }
}
