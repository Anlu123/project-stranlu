using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walljumping : MonoBehaviour
{
    [Header("Walljumping")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    private PlayerMovementAdvanced pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {

        // State 1 - WallJumping
        if ((wallLeft || wallRight) && AboveGround())
        {
            // wall jump
            if (Input.GetKeyDown(jumpKey))
            {
                pm.walljumping = true;
                WallJump();
                pm.walljumping = false;
            }
        }

    }

    private void WallJump()
    {
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
