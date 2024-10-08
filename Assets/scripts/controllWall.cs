using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllWall : MonoBehaviour
{
    //instances of outside wall?(should be)
    private SpriteRenderer Renderer;
    //the size of the outside wall
    public Vector3 size;
    //lock x y z coords of wall
    private float lockedXPosition;
    private float lockedYPosition;
    private float lockedZPosition;
    private float oriXpos;
    private float oriYpos;
    
    //toggle to movement of wall
    public bool Toggle = false;

    //getin to wall controlmode
    public bool controlMode = false;

    //player instance
    public Rigidbody2D targetRigidbody;

    //array of boxes instance
    public Rigidbody2D[] boxRigidBody;
    
    //the ori pos of player
    RigidbodyConstraints2D originalConstraints;
    //the array of ori pos of boxes
    RigidbodyConstraints2D[] originalConstraintsArray;

    public GameObject wallInstance;
    public static GameObject currentlySelectedWall = null; 

    bool boxInRange;
    //use for get camera pos
    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update

    void Awake(){
        originalConstraints = targetRigidbody.constraints;
        originalConstraintsArray = new RigidbodyConstraints2D[boxRigidBody.Length];

        for (int i = 0; i < boxRigidBody.Length; i++)
        {
            originalConstraintsArray[i] = boxRigidBody[i].constraints;
        }
    }
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        size = Renderer.bounds.size;
        oriXpos = transform.localPosition.x;
        oriYpos = transform.localPosition.y;

        lockedXPosition = transform.position.x;
        lockedYPosition = transform.position.y;
        lockedZPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // SetcontrolModes();

        if (wallInstance == null){
            toggleMove();
            }
            toggleFreeToMove();
        Debug.Log(wallInstance);
        controlingWall();
    }

    private void controlingWall(){
        if (controlMode){
            //this fix player moving
            targetRigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            
            if (insideWall() && wallInstance == gameObject && currentlySelectedWall == gameObject){
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (CompareTag("horizontal wall")){
                    transform.position = new Vector3(mousePosition.x, lockedYPosition, lockedZPosition);}
                    else if (CompareTag("vertical wall")){
                        transform.position = new Vector3(lockedXPosition, mousePosition.y, lockedZPosition);
                    }
            
            }
        }
        else {
            //when getout conrolMode of wall, player can move again
            targetRigidbody.constraints = originalConstraints;

            //outside wall will back to ori pos
            transform.position = new Vector3(oriXpos, oriYpos, lockedZPosition);
        }
    }

    private bool insideWall() {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float buffer = 0.01f; // Small buffer to handle precision issues
        if (mousePos.x > transform.position.x - size.x / 2 - buffer && mousePos.x < transform.position.x + size.x / 2 + buffer
            && mousePos.y < transform.position.y + size.y / 2 + buffer && mousePos.y > transform.position.y - size.y / 2 - buffer) {
            return true;
        }
        return false;
    }

    private void toggleMove(){
        if (Input.GetKeyDown(KeyCode.Mouse0)){
        //    Toggle = !Toggle;
        if (insideWall()){
        currentlySelectedWall = gameObject;
        wallInstance = this.gameObject;
        }
        }
    }

    private void toggleFreeToMove(){
        if (Input.GetKeyUp(KeyCode.Mouse0)){
            wallInstance=null;
            currentlySelectedWall = null;
        }
    }

    // private void SetcontrolModes (){
    //     if (Input.GetKeyDown(KeyCode.LeftAlt)){
    //        controlMode = !controlMode;
    //     }
    // }

}
