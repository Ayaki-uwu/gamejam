using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class controlBox : MonoBehaviour
{
    private SpriteRenderer Renderer;
    public Vector3 size;

    public GameObject trapWall;
    private Rigidbody2D myRigidBody;
    RigidbodyConstraints2D originalConstraints;

    public bool boxInRange;
    public bool playerControl;

    public bool controlMode;
    private Collider2D leftBlockingWall = null;  // Track the wall blocking from the left
    private Collider2D rightBlockingWall = null; // Track the wall blocking from the right

    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update
    [SerializeField]
    private controllWall[] wallcontroller;

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
        // if (boxInRange){
        if(leftBlockingWall != null || rightBlockingWall != null){
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
           foreach (controllWall walls in wallcontroller){
            if (walls != null)
        {
            walls.controlMode = controlMode;
        }
           }
        }
    }

    // private void OnTriggerEnter2D(Collider2D other){
    //     if (other.CompareTag("wallcollider")){
    //         Debug.Log("Toucs wall");
    //         boxInRange = true;
    //         // boxRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    //     }

    // }

    // private void OnTriggerExit2D(Collider2D other){
    //     if (other.CompareTag("wallcollider")){
    //         Debug.Log("Leaves wall");
    //         boxInRange = false;
    //     }
    //      // boxRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wallcollider"))
        {
            // Determine the relative position of the wall
            if (other.transform.position.x > transform.position.x)
            {
                Debug.Log("at left");
                leftBlockingWall = other;  // Wall is on the left
                controlMode = !controlMode;
                foreach (controllWall walls in wallcontroller){
                    if (walls != null)
                    {
                        walls.controlMode = controlMode;
                    }
                }
            }
            else if (other.transform.position.x < transform.position.x)
            {
                Debug.Log("at right");
                rightBlockingWall = other; // Wall is on the right
                controlMode = !controlMode;
                foreach (controllWall walls in wallcontroller){
                    if (walls != null)
                    {
                    walls.controlMode = controlMode;
                    }
                }
            }
        }

        if (other.CompareTag("TriggerButton")){
            trapWall.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("wallcollider"))
        {
            // Clear the blocking wall if it's no longer in contact
            if (leftBlockingWall == other)
            {
                leftBlockingWall = null;
                controlMode = !controlMode;
            }
            else if (rightBlockingWall == other)
            {
                rightBlockingWall = null;
                controlMode = !controlMode;
            }
        }
    }

    private void OnTriggerStay2D (Collider2D other){
        if (other.CompareTag("wallcollider")){
            if (leftBlockingWall !=null)
            {
                Debug.Log("Pushing to left");
                transform.position = new Vector2 (transform.position.x-0.01f, transform.position.y);
            }
        else if (rightBlockingWall != null)
            {
            Debug.Log("Pushing to right");
            transform.position = new Vector2 (transform.position.x+0.01f, transform.position.y);
            }
    }
}
}
