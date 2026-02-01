using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Cameralook : MonoBehaviour
{

    public float sensitivity = 80f;
    //posicion del player
    public Transform playerBody;
    

    float xRotation = 0f;
    public Transform camera;
    public Transform cameraPivot;
    public float anguloBalanceo = 10f;
    public float suavizado = 6f;

    void LateUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float balanceoZ = (x != 0 || z != 0)
            ? Mathf.Sin(Time.time * 6f) * anguloBalanceo
            : 0f;

        // la cámara sigue al pivot
        camera.position = cameraPivot.position;
        camera.rotation = Quaternion.Lerp(
            camera.rotation,
            cameraPivot.rotation * Quaternion.Euler(0f, 0f, balanceoZ),
            Time.deltaTime * suavizado
        );
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Juego iniciado");
        Cursor.lockState = CursorLockMode.Locked; // esto bloquea el cursor en el centro de la pantalla
    }

    // Update is called once per frame
    void Update()
    {

        // esto guarda la rotacion del mouse en el eje X y Y
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;


        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;  

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);//minimo y maximo de la rotacion de la camara en el eje X

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);// esto hace que la camara gire en el eje X, y se le asigna a la camara la rotacion en el eje X, Y y Z



        //el rotate hace que la camara gire en el eje X
        
        playerBody.Rotate(Vector3.up * mouseX);
        


    }
}
