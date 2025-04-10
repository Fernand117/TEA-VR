using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Perla : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool pegada = false;
    private Vector3 escalaOriginal; // Guardamos la escala original de la perla

    private void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Guardamos la escala original para restaurarla despu�s
        escalaOriginal = transform.localScale;

        // Suscribir evento al soltar el objeto
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(Soltar);
        }
    }

    private void Soltar(SelectExitEventArgs args)
    {
        if (!pegada) // Solo pegar una vez
        {
            GameObject capsula = GameObject.Find("Capsule"); // Aseg�rate de que el nombre es correcto
            if (capsula != null)
            {
                transform.SetParent(capsula.transform, true); // Mantiene la posici�n relativa
                transform.localScale = escalaOriginal; // Evita que la Perla se deforme

                // Desactivar f�sica para que no caiga ni se mueva
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                pegada = true; // Para evitar que se fije m�s de una vez
            }
        }
    }
}
