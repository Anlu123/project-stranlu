using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement1 : MonoBehaviour
{
    private CharacterController controller;

    [Header("Estadisticas Normales")]
    public float velocidad;
    public float velCorriendo;
    public float alturaDeSalto;
    public float tiempoAlGirar;
    public Transform orientation;

    [Header("Datos sobre el piso")]
    public Transform detectaPiso;
    public float distanciaPiso;
    public LayerMask mascaraPiso;

    float gravedad = -20f;
    Vector3 velocity;
    bool tocaPiso;

    [Header("Deteccion suelo animacion")]
    public float distanciaDeteccion;
    private float lastTimeTouchedFloor;
    public float tiempoMinimoAnim;

    Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        tocaPiso = Physics.CheckSphere(detectaPiso.position, distanciaPiso, mascaraPiso);

        if (tocaPiso && velocity.y < 0)
        {
            velocity.y = -2f;

        }

        if (tocaPiso)
        {
            anim.SetBool("saltando", false);
            anim.SetBool("aterrizando", false);
        }

        else if (!tocaPiso)
        {
            Debug.DrawRay(transform.position, -transform.up * distanciaDeteccion, Color.red);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, distanciaDeteccion, mascaraPiso))
            {
                anim.SetBool("aterrizando", true);
                anim.SetBool("saltando", true);
            }
        }

        if (Input.GetButtonDown("Jump") && tocaPiso)
        {

            anim.SetBool("saltando", true);
            velocity.y = Mathf.Sqrt(alturaDeSalto * -2 * gravedad);
        }
                velocity.y += gravedad * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 direccion = new Vector3(x, 0, z).normalized;

        //Idle
        if (direccion.magnitude <= 0 && tocaPiso)
        {
            anim.SetFloat("movimientos", 0, 0.1f, Time.deltaTime);
        }

        if (direccion.magnitude >= 0.1f)
        {
            //Calculo de Movimiento
            Vector3 move = orientation.forward * z + orientation.right * x;

            //Corriendo
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(move * velCorriendo * Time.deltaTime);
                anim.SetFloat("movimientos", 1, 0.1f, Time.deltaTime);
            }
            // Caminando
            else
            {
                controller.Move(move * velocidad * Time.deltaTime);
                anim.SetFloat("movimientos", 0.5f, 0.1f, Time.deltaTime);
            }
        }

    }

}