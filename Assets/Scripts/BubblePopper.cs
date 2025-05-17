using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using System.Collections.Generic;
using UnityEngine.UI;

public class BubblePopper : MonoBehaviour
{
    private bool isPopped = false;
    [SerializeField] private float touchThreshold = 0.01f;
    [SerializeField] private AudioSource popSound;
    private SphereCollider sphereCollider;
    private bool isBeingTouched = false;
    private float pinchThreshold = 0.7f; // Umbral para detectar el pellizco

    public List<Image> circulosIndicadores; // arrastra aquí los 5 circulitos en el inspector
    public Sprite circuloActivo;             // arrastra aquí el nuevo sprite (imagen activa)

    private static List<BubblePopper> activeBubbles = new List<BubblePopper>();
    private static bool isProcessingPop = false;

    // Referencias al canvas de felicitación y textos
    public GameObject canvasFelicitaciones;
    public Text txtTotalAciertos;
    public Text txtTiempo;
    public Text txtTotalIncorrectos;

    // Variables para el tiempo
    private float tiempoTranscurrido = 0f;
    private bool juegoActivo = true;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.isTrigger = true;
        }
        
        // Agregar esta burbuja a la lista de burbujas activas
        if (!activeBubbles.Contains(this))
        {
            activeBubbles.Add(this);
        }
        
        // Asegurarse que el canvas esté desactivado al inicio
        if (canvasFelicitaciones != null)
        {
            canvasFelicitaciones.SetActive(false);
        }

        // Reiniciar el contador global cuando se inicia el nivel
        contadorGlobal = 0;
        if (txtContador != null)
        {
            txtContador.text = "0";
        }

        // Reiniciar los círculos indicadores
        foreach (var circulo in circulosIndicadores)
        {
            circulo.sprite = null;
        }

        // Reiniciar el tiempo y el estado del juego
        tiempoTranscurrido = 0f;
        juegoActivo = true;
    }

    private void OnDestroy()
    {
        // Remover esta burbuja de la lista cuando se destruye
        activeBubbles.Remove(this);
    }

    private System.Collections.IEnumerator CheckPinch(HandRef hand)
    {
        while (isBeingTouched && !isPopped)
        {
            IHand handComponent = hand.GetComponent<IHand>();
            float indexPinch = handComponent.GetFingerPinchStrength(HandFinger.Index);
            float thumbPinch = handComponent.GetFingerPinchStrength(HandFinger.Thumb);
            
            if (indexPinch > pinchThreshold && thumbPinch > pinchThreshold && !isProcessingPop)
            {
                // Obtener las posiciones de los dedos
                handComponent.GetJointPose(HandJointId.HandIndexTip, out Pose indexPose);
                handComponent.GetJointPose(HandJointId.HandThumbTip, out Pose thumbPose);
                
                // Calcular el punto medio entre los dedos
                Vector3 pinchPosition = Vector3.Lerp(indexPose.position, thumbPose.position, 0.5f);
                
                // Encontrar la burbuja más cercana al punto de pellizco
                BubblePopper closestBubble = GetClosestBubble(pinchPosition);
                
                // Verificar si esta burbuja es la más cercana y está dentro de un radio razonable
                float distanceToPinch = Vector3.Distance(transform.position, pinchPosition);
                if (closestBubble == this && distanceToPinch < 0.05f) // Ajusta este valor según necesites
                {
                    isProcessingPop = true;
                    PopBubble();
                    isProcessingPop = false;
                }
                break;
            }
            yield return null;
        }
    }

    private BubblePopper GetClosestBubble(Vector3 pinchPosition)
    {
        BubblePopper closest = null;
        float closestDistance = float.MaxValue;

        foreach (BubblePopper bubble in activeBubbles)
        {
            if (!bubble.isPopped && bubble.isBeingTouched)
            {
                float distance = Vector3.Distance(pinchPosition, bubble.transform.position);
                if (distance < closestDistance && distance < 0.05f) // Añadimos un radio máximo de detección
                {
                    closestDistance = distance;
                    closest = bubble;
                }
            }
        }

        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        // No permitir más interacciones si el juego no está activo o ya se alcanzaron 5 burbujas
        if (!juegoActivo || contadorGlobal >= 5) return;

        if (!isPopped && !isBeingTouched)
        {
            HandRef hand = other.GetComponentInParent<HandRef>();
            if (hand != null)
            {
                isBeingTouched = true;
                StartCoroutine(CheckPinch(hand));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isPopped)
        {
            HandRef hand = other.GetComponentInParent<HandRef>();
            if (hand != null)
            {
                isBeingTouched = false;
            }
        }
    }

    private static int contadorGlobal = 0;
    public Text txtContador; // Referencia al texto del contador

    private const float TIEMPO_LIMITE = 120f; // 2 minutos en segundos

    private void Update()
    {
        if (juegoActivo)
        {
            tiempoTranscurrido += Time.deltaTime;
            
            // Verificar si se acabó el tiempo
            if (tiempoTranscurrido >= TIEMPO_LIMITE)
            {
                MostrarPanelFelicitaciones();
            }
        }
    }

    private void MostrarPanelFelicitaciones()
    {
        if (!juegoActivo) return; // Evitar que se muestre múltiples veces
        
        juegoActivo = false;
        
        if (canvasFelicitaciones != null)
        {
            canvasFelicitaciones.SetActive(true);
            
            // Mostrar total de aciertos (burbujas reventadas)
            if (txtTotalAciertos != null)
                txtTotalAciertos.text = contadorGlobal.ToString();
            
            // Mostrar tiempo transcurrido en minutos (máximo 2 minutos)
            float tiempoFinal = Mathf.Min(tiempoTranscurrido, TIEMPO_LIMITE);
            if (txtTiempo != null)
                txtTiempo.text = (tiempoFinal / 60f).ToString("F1") + " minutos";
            
            // Calcular y mostrar incorrectos (burbujas faltantes)
            if (txtTotalIncorrectos != null)
                txtTotalIncorrectos.text = (5 - contadorGlobal).ToString();
        }
    }

    private void PopBubble()
    {
        if (isPopped) return;
        
        isPopped = true;
        
        // Incrementar el contador global y actualizar el texto
        contadorGlobal++;
        if (txtContador != null)
        {
            txtContador.text = contadorGlobal.ToString();
        }

        // Actualizar el círculo indicador correspondiente
        if (contadorGlobal - 1 < circulosIndicadores.Count)
        {
            circulosIndicadores[contadorGlobal - 1].sprite = circuloActivo;
        }
        
        // Verificar si se han completado todas las burbujas
        if (contadorGlobal >= 5)
        {
            MostrarPanelFelicitaciones();
        }

        if (popSound != null)
        {
            popSound.Play();
        }

        if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
        }

        Invoke("DisableBubble", 0.1f);
    }

    private void DisableBubble()
    {
        gameObject.SetActive(false);
    }
}