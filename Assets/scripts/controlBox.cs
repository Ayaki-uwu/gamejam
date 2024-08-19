using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class controlBox : MonoBehaviour
{
    private SpriteRenderer Renderer;
    public Vector3 size;

    private Rigidbody2D myRigidBody;
    RigidbodyConstraints2D originalConstraints;

    public bool boxInRange;
    public bool playerControl;

    public bool controlMode;

    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        size = Renderer.bounds.size;
        originalConstraints=myRigidBody.constraints;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
        SetcontrolModes();

        // setBoxMove();
        if (boxInRange){
            //lock all XYZ to freeze the box
            myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
            
        }
        else if (controlMode){
            //only freeze box rotation -> which means X and Y are free
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else {
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }


    private void SetcontrolModes (){
        if (Input.GetKeyDown(KeyCode.LeftAlt)){
           controlMode = !controlMode;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("wallcollider") ){
            Debug.Log("Toucs wall");
            boxInRange = true;
            // boxRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }

    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("wallcollider")){
            Debug.Log("Leaves wall");
            boxInRange = false;
        }
         // boxRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    }

}
