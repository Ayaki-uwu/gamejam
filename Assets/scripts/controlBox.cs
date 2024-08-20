using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class controlBox : MonoBehaviour
{
    private SpriteRenderer Renderer;
    public Vector3 size;

    public GameObject trapWall;
    public GameObject nonpressButton;
    public GameObject pressedButton;
    public GameObject lockedSign;
    private Rigidbody2D myRigidBody;
    RigidbodyConstraints2D originalConstraints;

    Vector3 pos;

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
        pos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
        RestartScene(); // Call the restart method
        }

        mainCamera.ScreenToWorldPoint(Input.mousePosition);
        SetcontrolModes();

        // setBoxMove();
        // if (boxInRange){
        if(leftBlockingWall != null || rightBlockingWall != null){
            //lock all XYZ to freeze the box
            myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX|RigidbodyConstraints2D.FreezeRotation;
            
        }
        else if (controlMode){
            lockedSign.SetActive(true);
            //only freeze box rotation -> which means X and Y are free
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else {
            lockedSign.SetActive(false);
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }

        private void RestartScene()
        {
            Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
            SceneManager.LoadScene(currentScene.name); // Reload the scene by its name
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
            pressedButton.SetActive(true);
            nonpressButton.SetActive(false);
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
