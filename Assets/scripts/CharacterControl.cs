using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum FacingDirection{
    Left,
    Right
}

public enum Playerstate
{
    idle,
    running,
    jumping,
    dashing,
    falling
}

public class CharacterControl : MonoBehaviour
{
    public Playerstate state;
    Rigidbody2D myRigidBody;
    Vector2 moveInput;
    Animator myAnimator;

    [Header("General")]
    public bool isControlable = true;
    public GameObject interactable;
    public FacingDirection facingDirection;
    [Header("Movement")]
    public float speed;
    
    //for 2D platformer jump
    [Header("Jump")]
    public float jumpForce;
    // public float doubleJumpForce;
    public bool grounded;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public float upwardGravity;
    public float midAirGravity;
    public float downwardGravity;
    // public bool canDoubleJump;
    public float jumpTime;
    bool jumped;

    // [Header("Dash")]
    // public bool isDashed;
    // public float dashCooldown;
    // public float dashSpeed;
    // public float dashDuration;

    // [Header("Wall Climb")]
    // public bool isWallContact;
    // public Vector2 wallBoxSize;
    // public float wallCastDistance;


    bool isJumping;
    float jumpTimeCounter;

    private void Awake() {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isControlable){
            CheckMove();
            CheckJump();
            // CheckDoubleJump();
            // CheckDash();
        }
        UpdateGravity();

        CheckInteraction();
        
        // CheckFire();
        grounded = isGrounded();
        UpdateAnimator();
    }

    // ================================================================================================ 
    // -----------------------------------------2D Platformer------------------------------------------
    // ================================================================================================ 
    void CheckMove(){
        moveInput.x = Input.GetAxisRaw("Horizontal");
        myRigidBody.velocity = new Vector2(moveInput.x * speed , myRigidBody.velocity.y);
        if(moveInput.x == 0){
            //idle animation
            if(isGrounded() && (state == Playerstate.falling || state == Playerstate.running || state == Playerstate.dashing)){
                state = Playerstate.idle;
            }
        }else{
            Debug.Log("moving" + moveInput.x);
            //walking animation
            facingDirection = moveInput.x > 0? FacingDirection.Right : FacingDirection.Left;
            if(isGrounded() && (state == Playerstate.falling || state == Playerstate.idle || state == Playerstate.dashing)){
                state = Playerstate.running;
            }
            
        }
    }

    void CheckJump(){
        // if(isGrounded()){
        //     canDoubleJump = true;
        // }
        if(Input.GetButtonDown("Jump") && isGrounded()){        //spacebar in default
            jumpTimeCounter = jumpTime;
            isJumping = true;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpForce);
            state = Playerstate.jumping;
        }

        if(Input.GetButton("Jump") && isJumping){
            if(jumpTimeCounter > 0){
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }else{
                isJumping = false;
            }
        }

        if(Input.GetButtonUp("Jump")){
            isJumping = false;
        }
        
    }
    // void CheckDoubleJump(){
    //     if(canDoubleJump && !isGrounded() && Input.GetButtonDown("Jump")){
    //         canDoubleJump = false;
    //         myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, doubleJumpForce);
    //         state = Playerstate.jumping;
    //     }
    // }

    // void CheckDash(){
    //     if(Input.GetKeyDown(KeyCode.Z) && !isDashed){
    //         isDashed = true;
    //         myRigidBody.velocity = new Vector2((facingDirection == FacingDirection.Right? 1 : -1) * dashSpeed, 0);
    //         isControlable = false;
    //         myRigidBody.gravityScale = 0;
    //         state = Playerstate.dashing;
    //         DOVirtual.DelayedCall(dashDuration, ()=>{
    //             isControlable = true;
    //             myRigidBody.gravityScale = downwardGravity;
    //             state = Playerstate.idle;
    //             DOVirtual.DelayedCall(dashCooldown, ()=>{
    //                 isDashed = false;
    //             });
    //         });
            
            
    //     }
    // }

    void UpdateGravity(){
        if(!isGrounded()){
            float y_velocity = myRigidBody.velocity.y;
            if(y_velocity < 1 && y_velocity > -1){
                myRigidBody.gravityScale = midAirGravity;
            }
            else if(y_velocity > 0){
                myRigidBody.gravityScale = upwardGravity;
            }else if(y_velocity < 0){
                myRigidBody.gravityScale = downwardGravity;
                state = Playerstate.falling;
            }
        }else{
            myRigidBody.gravityScale = 1;
        }
        
    }

    // bool isGrounded(){
    //     if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer)){
    //         return true;
    //     }else{
    //         return false;
    //     }
    // }

    bool isGrounded(){
        return true;
    }

    void CheckInteraction(){
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(interactable != null && scroll >0f){
            // Debug.Log("interact with "+ interactable.name);
            // interactable.transform.parent.GetComponent<Interactable>().Interact(carryingCat.Count);
            interactable.transform.localScale += new Vector3(0.03f,0.03f,0);
        }
        else if (interactable != null && scroll <0f){
            interactable.transform.localScale += new Vector3(-0.03f,-0.03f,0);
        }
        else if (interactable != null && interactable.transform.localScale.x<0.2 && interactable.transform.localScale.y<0.2){
                interactable.transform.localScale = Vector3.one;
            }
        else if (interactable != null && interactable.transform.localScale.x>1.5 && interactable.transform.localScale.y>1.5){
                interactable.transform.localScale = Vector3.one;
            }
    }    

    // void CheckFire(){
    //     if(Input.GetButtonDown("Fire1")){
    //         Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("targetbox")||other.CompareTag("nontargetbox")){
            interactable = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("targetbox")||other.CompareTag("nontargetbox")){
            interactable = null;
        }
    }


    // for configuring BoxCast
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    public void UpdateAnimator(){
        // if(myRigidBody.velocity.x < 0){
        //     transform.localScale = new (-0.4f, 0.4f, 0.4f);
        // }else if(myRigidBody.velocity.x > 0){
        //     transform.localScale = new (0.4f, 0.4f, 0.4f);
        // }

        if(myRigidBody.velocity.x < 0){
            transform.localScale = new Vector3(-1,1,1);
        }else if(myRigidBody.velocity.x > 0){
            transform.localScale = Vector3.one;
        }

        myAnimator.SetBool("IsIdle", state == Playerstate.idle);
        myAnimator.SetBool("IsRun", state == Playerstate.running);
        myAnimator.SetBool("IsJump", state == Playerstate.jumping);
        myAnimator.SetBool("IsDash", state == Playerstate.dashing);
        myAnimator.SetBool("IsFall", state == Playerstate.falling);

    }
}
