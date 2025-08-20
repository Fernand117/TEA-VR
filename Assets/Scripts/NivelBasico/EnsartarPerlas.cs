using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnsartarPerlas : MonoBehaviour
{
    [Header("UI References")]
    public Text txtContador;
    public List<Image> circulosIndicadores; // arrastrar aquí los 5 circulitos en el inspector
    public Sprite circuloActivo;             // arrastrar aquí el nuevo sprite (imagen activa)

    [Header("Audio")]
    public AudioClip checkPerla;
    public AudioSource audioSource;

    [Header("Canvas de Felicitaciones")]
    public GameObject canvasFelicitaciones;
    public Text txtTotalAciertos;
    public Text txtTiempo;
    public Text txtTotalIncorrectos;

    [Header("Configuración del Juego")]
    public string nivelDificultad = "Normal"; // Fácil, Normal, Difícil
    
    // Variables del juego
    private int contador = 0;
    private float tiempoTranscurrido = 0f;
    private bool juegoActivo = true;
    private int intentosFallidos = 0; // Contador de intentos fallidos

    private void Start()
    {
        // Asegurarse que el canvas esté desactivado al inicio
        if (canvasFelicitaciones != null)
        {
            canvasFelicitaciones.SetActive(false);
        }
        
        // Inicializar contadores
        contador = 0;
        intentosFallidos = 0;
        
        // Actualizar UI
        if (txtContador != null)
        {
            txtContador.text = contador.ToString();
        }
    }

    private void Update()
    {
        // Solo contar tiempo mientras el juego esté activo
        if (juegoActivo)
        {
            tiempoTranscurrido += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Agregar esta verificación al inicio
        if (!juegoActivo || contador >= 5) return;

        Debug.Log("Colisión detectada con: " + collision.GetComponent<Collider>().name);

        if (collision.GetComponent<Collider>().CompareTag("Perla"))
        {
            // Ensartar la perla usando el método original
            EnsartarPerlaColisionV2(collision.gameObject);
        }
    }

    /// <summary>
    /// Ensarta una perla cuando hay colisión (método original)
    /// </summary>
    private void EnsartarPerlaColision(GameObject perla)
    {
        if (audioSource != null && checkPerla != null)
        {
            audioSource.clip = checkPerla;
            audioSource.Play();
        }

        Transform perlaTransform = perla.transform;

        // Fijar la Perla como hija de la Cápsula
        perlaTransform.SetParent(transform, true); // Se mantiene en su posición global

        // Desactivar la física para que no se mueva más
        Rigidbody rb = perlaTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Evitar que la física lo siga afectando
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero; // Evitar que se muevan después de pegarse
            rb.angularVelocity = Vector3.zero; // Evitar giros indeseados
        }

        // 🚨 IMPORTANTE: Desactivar el collider
        Collider col = perla.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Incrementar el contador
        contador++;
        if (txtContador != null)
        {
            txtContador.text = contador.ToString();
        }
        Debug.Log("Contador actualizado a: " + contador);

        // Cambiar la imagen del círculo correspondiente
        if (contador - 1 < circulosIndicadores.Count)
        {
            circulosIndicadores[contador - 1].sprite = circuloActivo;
        }

        // Verificar si se completaron las 5 perlas
        if (contador >= 5)
        {
            MostrarPanelFelicitaciones();
        }
    }

    private void EnsartarPerlaColisionV2(GameObject perla)
    {
        // Reproducir sonido
        if (audioSource != null && checkPerla != null)
        {
            audioSource.clip = checkPerla;
            audioSource.Play();
        }

        // ❌ En lugar de pegarla al hilo → la eliminamos
        Destroy(perla); // O perla.SetActive(false) si prefieres solo ocultarla

        // Incrementar el contador
        contador++;
        if (txtContador != null)
        {
            txtContador.text = contador.ToString();
        }
        Debug.Log("Contador actualizado a: " + contador);

        // Cambiar la imagen del círculo correspondiente
        if (contador - 1 < circulosIndicadores.Count)
        {
            circulosIndicadores[contador - 1].sprite = circuloActivo;
        }

        // Verificar si se completaron las 5 perlas
        if (contador >= 5)
        {
            MostrarPanelFelicitaciones();
        }
    }


    private void MostrarPanelFelicitaciones()
    {
        juegoActivo = false;

        // Calcular estadísticas
        int totalAciertos = contador;
        int totalIncorrectos = intentosFallidos; // Ahora usa intentos fallidos en lugar de 5 - contador
        float tiempoMinutos = tiempoTranscurrido / 60f;

        // Actualizar textos
        if (txtTotalAciertos != null)
            txtTotalAciertos.text = totalAciertos.ToString();

        if (txtTiempo != null)
            txtTiempo.text = tiempoMinutos.ToString("F1") + " minutos";

        if (txtTotalIncorrectos != null)
            txtTotalIncorrectos.text = totalIncorrectos.ToString();

        // Mostrar el canvas
        if (canvasFelicitaciones != null)
        {
            canvasFelicitaciones.SetActive(true);
        }

        // 🎯 GUARDAR RESULTADOS
        GuardarResultadosEjercicio();
    }

    /// <summary>
    /// Guarda los resultados del ejercicio usando el ResultadosManager
    /// </summary>
    private void GuardarResultadosEjercicio()
    {
        // Guardar los resultados del ejercicio
        ResultadosManager.Instance.GuardarResultado(
            nombreEjercicio: "Ensartar Perlas",
            exitos: contador,
            fallas: intentosFallidos,
            tiempo: tiempoTranscurrido,
            dificultad: nivelDificultad
        );
        
        Debug.Log($"✅ Resultados guardados: {contador} éxitos, {intentosFallidos} fallas, {tiempoTranscurrido:F1}s");
    }

    /// <summary>
    /// Método para exportar todos los datos (llamar desde un botón en el menú)
    /// </summary>
    public void ExportarTodosLosDatos()
    {
        ResultadosManager.Instance.ExportarCSV();
        Debug.Log("📊 Datos exportados a CSV");
    }
}
