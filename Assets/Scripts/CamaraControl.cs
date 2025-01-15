using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float speed = 5f;
    public CharacterController characterController;

    float xRotation = 0f;
    float yRotation = 0f;
    bool cursosLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        CursorLock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            CursorLock();
        }
        
        if (cursosLocked)
        {
            RotateCamera();
            MovePlayer();
        }
    }

    void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        cursosLocked = true;
    }

    void RotateCamera()
    {
        // Obtiene el movimiento del mouse en los ejes X e Y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Actualiza las rotaciones en los ejes X e Y
        xRotation -= mouseY;
        yRotation += mouseX;

        // Limita la rotación vertical para evitar que la cámara gire completamente
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplica las rotaciones a la cámara usando Quaternion.Euler para ambos ejes
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // La dirección del movimiento se basa en la orientación actual de la cámara
        Vector3 direccion = transform.right * horizontal + transform.forward * vertical;
        //transform.Translate(direccion * speed * Time.deltaTime, Space.World);
        characterController.Move(direccion * speed * Time.deltaTime);
    }
}
