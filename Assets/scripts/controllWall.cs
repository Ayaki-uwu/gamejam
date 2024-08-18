using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllWall : MonoBehaviour
{
    private SpriteRenderer renderer;
    public Vector3 size;
    private float lockedZPosition;
    private float oriXpos;
    private float oriYpos;
    public bool Toggle = false;

    public bool controlMode = false;

    public Rigidbody2D targetRigidbody;
    public Rigidbody2D[] boxRigidBody;
    
    RigidbodyConstraints2D originalConstraints;
    RigidbodyConstraints2D originalConstraintsArray;
    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update

    void Awake(){
        originalConstraints = targetRigidbody.constraints;
        foreach (Rigidbody2D Constraint in boxRigidBody){
            originalConstraintsArray=Constraint.constraints;
        }
    }
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        size = renderer.bounds.size;
        oriXpos = transform.localPosition.x;
        oriYpos = transform.localPosition.y;
        lockedZPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
        SetcontrolModes();
        toggleMove();
        controlingWall();
    }

    private void controlingWall(){
        if (controlMode){
            targetRigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            foreach (Rigidbody2D Constraint in boxRigidBody){
                Constraint.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            if (insideWall()){
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                transform.position = new Vector3(mousePosition.x, mousePosition.y, lockedZPosition);
            
            }
        }
        else {
            targetRigidbody.constraints = originalConstraints;
                foreach (Rigidbody2D Constraint in boxRigidBody){
                Constraint.constraints = originalConstraintsArray;
            }
            transform.position = new Vector3(oriXpos, oriYpos, lockedZPosition);
        }
    }

    private bool insideWall(){
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x-size.x/2 && mousePos.x<transform.position.x+size.x/2
        && mousePos.y < transform.position.y+size.y/2 && mousePos.y>transform.position.y-size.y/2 && Toggle
        ) {
            return true;
        }
        return false;
    }

    private void toggleMove(){
        if (Input.GetKey(KeyCode.LeftControl)){
           Toggle = !Toggle;
        }
    }

    private void SetcontrolModes (){
        if (Input.GetKeyDown(KeyCode.LeftAlt)){
           controlMode = !controlMode;
        }
    }
}
