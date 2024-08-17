using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public enum PlayerState{
        idle,
        running,
        jumping,
        dashing,
        falling
    }

    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody= GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
        currentState = PlayerState.running;
        animator.SetFloat("moveX",1);
        animator.SetFloat("moveY",0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
