using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour
{

    public Transform camera;
    public float alturaArriba = 0.5f;
    public float tiempoArriba = 0.3f;
    public float suavizado = 3f;
    private float tiempo;
    private float camYInicial;
    private bool arriba = true;


    public CharacterController characterController;
    public float speed = 15f;

    public float gravity = -3f;

    //gravedad velocidad
    Vector3 velocity;




    //comprobacion de suelo
    public Transform groundCheck;

    public float sphereRadius = 0.3f;

    //etiqueta para el suelo, para que el player sepa si esta en el suelo o no
    public LayerMask groundMask;

    bool isGrounded;



    //salto
    public float jumpheigth = 3f;


    public bool isSprinting;

    public float sprintSpeedMultiplier = 2f;

    private float sprintSpeed = 1;


    public float staminaUseAmount = 5f;

    private StaminaBar staminaSlider;


    //public Animator animator;



    void Start()
    {
        //encontrar la slider de stamina en la escena
        //como tiene el script stamina bar , lo busca y lo asigna a la variable
        staminaSlider = FindObjectOfType<StaminaBar>();
    }


    void Update()
    {
        //esto es para saber si el player esta en el suelo o no mediante una funcion de unity
        //CheckSphere crea una esfera en el punto que le digamos, en este caso groundCheck.position
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            //si el player esta en el suelo, la velocidad en y se pone a 0
            velocity.y = -2f;
        }



        //esto es para el movimiento del player asignacion de teclas de movimiento
        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        //animator.SetFloat("VelX", x);
        //animator.SetFloat("VelZ", z);
        //animator.SetBool("isSprinting", isSprinting);
        if (x != 0 || z != 0)
        {
            


            tiempo += Time.deltaTime;

            if (tiempo >= tiempoArriba)
            {
                tiempo = 0f;
                arriba = !arriba;
              
            }

            
            float objetivoY = arriba
                ? camYInicial + alturaArriba
                : camYInicial - alturaArriba;

            camera.localPosition = Vector3.Lerp(
                camera.localPosition,
                new Vector3(camera.localPosition.x, objetivoY, camera.localPosition.z),
                Time.deltaTime * suavizado
            );



        }
        else
        {
            camera.localPosition = Vector3.Lerp(
                camera.localPosition,
                new Vector3(camera.localPosition.x, camYInicial, camera.localPosition.z),
                Time.deltaTime * suavizado
            );

           
            tiempo = 0f;
        }


        //esto es para mover al jugador adelante o hacia atras 
        Vector3 move = transform.right * x + transform.forward * z;


        JunpCheck();
        RunCheck();



        //esto le asigna el movimiento al caracter controler del player 
        //y le asigna la velocidad que se le dio en el inspector
        characterController.Move(move * speed * Time.deltaTime * sprintSpeed);
        
        // si alguien juega a 30 y alguien a 60 fps, el que juega a 30 fps se movera mas lento
        //por eso se multiplica por Time.deltaTime



        //para activarle la gravedad 
        velocity.y += gravity * Time.deltaTime;
        //esto le asigna la gravedad al player
        characterController.Move(velocity * Time.deltaTime);




    }
    public void JunpCheck()
    {

        //salto

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            //sqrt raiz cuadrada
            velocity.y = Mathf.Sqrt(jumpheigth * -2f * gravity);
            //animator.SetBool("isJumping",true);
        }
        //para que cuando caiga no se quede el bool en true
        if (!isGrounded)
        {
            //animator.SetBool("isJumping", false);
        }
    }

    public void RunCheck()
    {

        //checar bien lo de las corutinas logica para correr
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = !isSprinting;
            //para que no se solapen entre ellas

            if (isSprinting)
            {
                staminaSlider.UseStamina(staminaUseAmount);
            }
            else
            {
                staminaSlider.StopSprinting();
            }
        }

        if (isSprinting)
        {
            sprintSpeed = sprintSpeedMultiplier;
        }
        else
        {
            sprintSpeed = 1;

        }
    }
}
