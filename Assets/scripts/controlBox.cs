using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class controlBox : MonoBehaviour
{
    private SpriteRenderer renderer;
    public Vector3 size;

    private Rigidbody2D myRigidBody;

    [SerializeField]
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        size = renderer.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
        controlingBoxSize();
    }

    private void controlingBoxSize (){
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (insideBox() &&(renderer.CompareTag("targetbox") || renderer.CompareTag("nontargetbox"))){
            if (transform.localScale.x<0 && transform.localScale.y<0){
                transform.localScale = Vector3.one;
            }
            else if (insideBox() && scroll >0f && (Input.GetKey(KeyCode.LeftShift)
            ||Input.GetKeyDown(KeyCode.RightShift))){
                transform.localScale += new Vector3(0.03f,0.03f,0);
            }
            else if (insideBox()&& scroll <0f &&(Input.GetKey(KeyCode.LeftShift)
            ||Input.GetKeyDown(KeyCode.RightShift))){
                transform.localScale += new Vector3(-0.03f,-0.03f,0);
            }
        }
    }

    private bool insideBox(){
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x-size.x/2 && mousePos.x<transform.position.x+size.x/2
        && mousePos.y < transform.position.y+size.y/2 && mousePos.y>transform.position.y-size.y/2
        ) {
            return true;
        }
        return false;
    }
}
