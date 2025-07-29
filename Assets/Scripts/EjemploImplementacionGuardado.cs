using UnityEngine;

/// <summary>
/// EJEMPLO: Cómo implementar el sistema de guardado en cualquier ejercicio
/// 
/// PASOS PARA AGREGAR A OTROS EJERCICIOS:
/// 1. Copiar las variables de tracking
/// 2. Llamar GuardarResultado al finalizar
/// 3. Configurar el nivel de dificultad
/// </summary>
public class EjemploImplementacionGuardado : MonoBehaviour
{
    [Header("Configuración del Ejercicio")]
    public string nombreEjercicio = "Mi Ejercicio";
    public string nivelDificultad = "Normal"; // Fácil, Normal, Difícil
    
    // Variables para tracking de resultados
    private int aciertos = 0;
    private int fallas = 0;
    private float tiempoTranscurrido = 0f;
    private bool juegoActivo = true;
    
    private void Update()
    {
        // Cronómetro del ejercicio
        if (juegoActivo)
        {
            tiempoTranscurrido += Time.deltaTime;
        }
    }
    
    /// <summary>
    /// Llamar cuando el jugador tenga un acierto
    /// </summary>
    public void RegistrarAcierto()
    {
        aciertos++;
        Debug.Log($"Acierto registrado. Total: {aciertos}");
        
        // Verificar si el ejercicio se completó
        if (AciertosSuficientes())
        {
            FinalizarEjercicio();
        }
    }
    
    /// <summary>
    /// Llamar cuando el jugador tenga una falla
    /// </summary>
    public void RegistrarFalla()
    {
        fallas++;
        Debug.Log($"Falla registrada. Total: {fallas}");
    }
    
    /// <summary>
    /// Lógica para determinar si el ejercicio está completo
    /// </summary>
    private bool AciertosSuficientes()
    {
        // Ejemplo: completar con 5 aciertos
        return aciertos >= 5;
    }
    
    /// <summary>
    /// Finalizar el ejercicio y guardar resultados
    /// </summary>
    public void FinalizarEjercicio()
    {
        juegoActivo = false;
        
        // Guardar resultados
        GuardarResultadosEjercicio();
        
        // Mostrar pantalla de resultados
        MostrarResultados();
    }
    
    /// <summary>
    /// Guarda los resultados usando el ResultadosManager
    /// </summary>
    private void GuardarResultadosEjercicio()
    {
        try
        {
            ResultadosManager.Instance.GuardarResultado(
                nombreEjercicio: nombreEjercicio,
                exitos: aciertos,
                fallas: fallas,
                tiempo: tiempoTranscurrido,
                dificultad: nivelDificultad
            );
            
            Debug.Log($"✅ Resultados guardados para {nombreEjercicio}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Error al guardar resultados: {e.Message}");
        }
    }
    
    /// <summary>
    /// Muestra los resultados al jugador
    /// </summary>
    private void MostrarResultados()
    {
        string mensaje = $"🎯 EJERCICIO COMPLETADO\n\n" +
                        $"Ejercicio: {nombreEjercicio}\n" +
                        $"Aciertos: {aciertos}\n" +
                        $"Fallas: {fallas}\n" +
                        $"Tiempo: {tiempoTranscurrido:F1} segundos\n" +
                        $"Dificultad: {nivelDificultad}";
        
        Debug.Log(mensaje);
        
        // Aquí puedes activar un canvas de resultados, etc.
    }
    
    /// <summary>
    /// Reinicia el ejercicio
    /// </summary>
    public void ReiniciarEjercicio()
    {
        aciertos = 0;
        fallas = 0;
        tiempoTranscurrido = 0f;
        juegoActivo = true;
        
        Debug.Log($"🔄 {nombreEjercicio} reiniciado");
    }
}

/* 
EJEMPLO DE USO EN OTROS SCRIPTS:

1. EN UNITY (INSPECTOR):
   - Asignar nombre del ejercicio
   - Configurar nivel de dificultad

2. EN EL CÓDIGO:
   // Cuando hay un acierto:
   RegistrarAcierto();
   
   // Cuando hay una falla:
   RegistrarFalla();
   
   // Para finalizar manualmente:
   FinalizarEjercicio();

3. INTEGRACIÓN CON UI:
   - Los resultados se guardan automáticamente
   - Se pueden mostrar en pantallas de resultados
   - Los archivos se crean en la ruta configurada

4. ARCHIVOS GENERADOS:
   - JSON con datos estructurados
   - CSV para análisis en Excel
   - Historial completo de todas las sesiones
*/
