using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovementAdvanced;

public class PlayerMovement : MonoBehaviour
{
    private float x;
    private float z;
    private CharacterController controller;
    public Transform orientation;
    public Animator squirrelAnimator;


    [Header("Checkeo Ground")]

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;


    [Header("Variables")]
    public float speed = 12f;
    public float gravity = -10f;
    public float jumpHeight = 3f;

    [Header("Landing")]
    public float landDistance = 2f; // distancia para detectar el aterrizaje
    private float lastTimeGrounded; // tiempo desde que tocó el suelo por última vez

    Vector3 velocity;

    int animatorState;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0.5f;
        controller =   GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //Es este valor y no 0 porque a veces puede detectar antes el suelo y es mas conveniente asi.
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //Mueve Pj
        Vector3 move = orientation.forward * z + orientation.right * x;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            lastTimeGrounded = Time.time;
            squirrelAnimator.SetInteger("stateAnimator", 3);
            animatorState = 3;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else if (isGrounded && IsMoving())
        {
            squirrelAnimator.SetInteger("stateAnimator", 1);
            lastTimeGrounded = Time.time; // Actualizamos la última vez que estuvo en el suelo cuando se está moviendo en el suelo
            animatorState = 1;
        }
        else if (isGrounded)
        {
            squirrelAnimator.SetInteger("stateAnimator", 0);
            lastTimeGrounded = Time.time; // Actualizamos la última vez que estuvo en el suelo cuando se está quieto en el suelo
            animatorState = 0;
        }
        else //Cayendo o aterrizando
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, landDistance, groundMask) && Time.time - lastTimeGrounded > 0.1f)
            {
                // Si el suelo está a una distancia 'landDistance', haz la animación de aterrizaje
                squirrelAnimator.SetInteger("stateAnimator", 4);
                animatorState = 4;
            }
            else if (Time.time - lastTimeGrounded > 2)
            {
                // Si ha pasado más de 2 segundos desde la última vez que tocó el suelo, haga la animación de caer
                squirrelAnimator.SetInteger("stateAnimator", 5);
                animatorState = 5;
            }
        }

        Debug.Log(animatorState);

        //Aplica gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private bool IsMoving()
    {
        // Comprobar si alguna tecla W, A, S o D está siendo presionada.
        // Si se presiona alguna de estas teclas, devolver true, de lo contrario, devolver false.
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }


}
