using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    public CharacterController controller;

    public float WalkSpeed = 12f;

    private float MoveSpeed;

    public float SprintSpeed = 20f;

    public float jumpHeight = 3f;

    public float gravity = -18f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    Vector3 velocity;

    bool isGrounded;

    public MovementState State;

    public enum MovementState
    {
        walking, 
        sprinting,
        air
    }

    //Jump
    void Update()
    {

        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        StateHandler();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * MoveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void StateHandler () 
    {
        //Sprint
        if (isGrounded && Input.GetKey(KeyCode.LeftShift)) 
        {
            State = MovementState.sprinting;
            MoveSpeed = SprintSpeed;
        }
        //Camina
        else if (isGrounded) 
        { 
            State = MovementState.walking;
            MoveSpeed = WalkSpeed;
        }
        //Esta en el aire
        else
        {
            State = MovementState.air;
        }
    }
}
