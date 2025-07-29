using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ResultadoEjercicio
{
    public string nombreEjercicio;
    public int totalExitos;
    public int totalFallas;
    public float tiempoTranscurrido; // en segundos
    public string fechaHora;
    public string nivelDificultad;
    
    public ResultadoEjercicio(string nombre, int exitos, int fallas, float tiempo, string dificultad = "Normal")
    {
        nombreEjercicio = nombre;
        totalExitos = exitos;
        totalFallas = fallas;
        tiempoTranscurrido = tiempo;
        fechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        nivelDificultad = dificultad;
    }
}

[System.Serializable]
public class SesionJuego
{
    public string fechaSesion;
    public string idSesion;
    public List<ResultadoEjercicio> ejercicios;
    
    public SesionJuego()
    {
        fechaSesion = DateTime.Now.ToString("yyyy-MM-dd");
        idSesion = Guid.NewGuid().ToString();
        ejercicios = new List<ResultadoEjercicio>();
    }
}

[System.Serializable]
public class HistorialCompleto
{
    public string versionApp = "1.0";
    public string dispositivo;
    public List<SesionJuego> sesiones;
    
    public HistorialCompleto()
    {
        dispositivo = SystemInfo.deviceModel;
        sesiones = new List<SesionJuego>();
    }
}

public class ResultadosManager : MonoBehaviour
{
    [Header("Configuración de Guardado")]
    public bool guardarAutomaticamente = true;
    public bool mostrarLogGuardado = true;
    
    private static ResultadosManager instance;
    private SesionJuego sesionActual;
    private HistorialCompleto historial;
    
    // Rutas de almacenamiento
    private string rutaGuardado;
    private string nombreArchivoSesion;
    private string nombreArchivoHistorial;
    
    public static ResultadosManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ResultadosManager");
                instance = go.AddComponent<ResultadosManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InicializarSistemaGuardado();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void InicializarSistemaGuardado()
    {
        // Configurar rutas de guardado
        ConfigurarRutasGuardado();
        
        // Inicializar sesión actual
        sesionActual = new SesionJuego();
        
        // Cargar historial existente
        CargarHistorial();
        
        if (mostrarLogGuardado)
        {
            Debug.Log($"ResultadosManager inicializado. Guardando en: {rutaGuardado}");
        }
    }
    
    [Header("Configuración de Guardado")]
    [SerializeField] private TipoUbicacionGuardado ubicacionGuardado = TipoUbicacionGuardado.DocumentosPublicos;

    public enum TipoUbicacionGuardado
    {
        DocumentosPublicos,     // /sdcard/Documents/TEA_VR_Results (más fácil de encontrar)
        DescargasPublicas,      // /sdcard/Download/TEA_VR_Results (como las descargas del navegador)
        AlmacenamientoApp,      // /sdcard/Android/data/[app]/files (ubicación actual)
        AlmacenamientoInterno   // Almacenamiento interno de la app (menos accesible)
    }
    
    private void ConfigurarRutasGuardado()
    {
        // Para Quest/Android, usar diferentes ubicaciones según configuración
        #if UNITY_ANDROID && !UNITY_EDITOR
            switch (ubicacionGuardado)
            {
                case TipoUbicacionGuardado.DocumentosPublicos:
                    // Carpeta Documentos - más fácil de encontrar
                    rutaGuardado = Path.Combine("/sdcard/Documents", "TEA_VR_Results");
                    break;
                    
                case TipoUbicacionGuardado.DescargasPublicas:
                    // Carpeta Descargas - como archivos descargados
                    rutaGuardado = Path.Combine("/sdcard/Download", "TEA_VR_Results");
                    break;
                    
                case TipoUbicacionGuardado.AlmacenamientoApp:
                    // Ubicación específica de la app (actual)
                    rutaGuardado = Path.Combine("/sdcard/Android/data", Application.identifier, "files", "ResultadosVR");
                    break;
                    
                case TipoUbicacionGuardado.AlmacenamientoInterno:
                    // Almacenamiento interno de Unity (menos accesible)
                    rutaGuardado = Path.Combine(Application.persistentDataPath, "ResultadosVR");
                    break;
            }
        #else
            // Para editor y otras plataformas
            rutaGuardado = Path.Combine(Application.persistentDataPath, "ResultadosVR");
        #endif
        
        // Crear directorio si no existe
        if (!Directory.Exists(rutaGuardado))
        {
            Directory.CreateDirectory(rutaGuardado);
        }
        
        // Nombres de archivos con fecha
        string fecha = DateTime.Now.ToString("yyyy-MM-dd");
        nombreArchivoSesion = $"Sesion_{fecha}_{DateTime.Now.ToString("HHmm")}.json";
        nombreArchivoHistorial = "HistorialCompleto.json";
    }
    
    /// <summary>
    /// Guarda un resultado de ejercicio en la sesión actual
    /// </summary>
    public void GuardarResultado(string nombreEjercicio, int exitos, int fallas, float tiempo, string dificultad = "Normal")
    {
        ResultadoEjercicio resultado = new ResultadoEjercicio(nombreEjercicio, exitos, fallas, tiempo, dificultad);
        sesionActual.ejercicios.Add(resultado);
        
        if (guardarAutomaticamente)
        {
            GuardarSesionActual();
            ActualizarHistorial();
        }
        
        if (mostrarLogGuardado)
        {
            Debug.Log($"Resultado guardado - {nombreEjercicio}: {exitos} éxitos, {fallas} fallas, {tiempo:F1}s");
        }
    }
    
    /// <summary>
    /// Guarda la sesión actual en un archivo JSON
    /// </summary>
    public void GuardarSesionActual()
    {
        try
        {
            string rutaCompleta = Path.Combine(rutaGuardado, nombreArchivoSesion);
            string json = JsonUtility.ToJson(sesionActual, true);
            File.WriteAllText(rutaCompleta, json);
            
            if (mostrarLogGuardado)
            {
                Debug.Log($"Sesión guardada en: {rutaCompleta}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al guardar sesión: {e.Message}");
        }
    }
    
    /// <summary>
    /// Actualiza el historial completo con la sesión actual
    /// </summary>
    public void ActualizarHistorial()
    {
        try
        {
            // Buscar si ya existe una sesión de hoy
            string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            SesionJuego sesionExistente = historial.sesiones.Find(s => s.fechaSesion == fechaHoy);
            
            if (sesionExistente != null)
            {
                // Actualizar sesión existente
                sesionExistente.ejercicios = new List<ResultadoEjercicio>(sesionActual.ejercicios);
            }
            else
            {
                // Agregar nueva sesión
                historial.sesiones.Add(new SesionJuego
                {
                    fechaSesion = sesionActual.fechaSesion,
                    idSesion = sesionActual.idSesion,
                    ejercicios = new List<ResultadoEjercicio>(sesionActual.ejercicios)
                });
            }
            
            GuardarHistorialCompleto();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al actualizar historial: {e.Message}");
        }
    }
    
    /// <summary>
    /// Guarda el historial completo
    /// </summary>
    private void GuardarHistorialCompleto()
    {
        try
        {
            string rutaCompleta = Path.Combine(rutaGuardado, nombreArchivoHistorial);
            string json = JsonUtility.ToJson(historial, true);
            File.WriteAllText(rutaCompleta, json);
            
            if (mostrarLogGuardado)
            {
                Debug.Log($"Historial actualizado: {rutaCompleta}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al guardar historial: {e.Message}");
        }
    }
    
    /// <summary>
    /// Carga el historial existente
    /// </summary>
    private void CargarHistorial()
    {
        try
        {
            string rutaCompleta = Path.Combine(rutaGuardado, nombreArchivoHistorial);
            
            if (File.Exists(rutaCompleta))
            {
                string json = File.ReadAllText(rutaCompleta);
                historial = JsonUtility.FromJson<HistorialCompleto>(json);
                
                if (mostrarLogGuardado)
                {
                    Debug.Log($"Historial cargado: {historial.sesiones.Count} sesiones encontradas");
                }
            }
            else
            {
                historial = new HistorialCompleto();
                
                if (mostrarLogGuardado)
                {
                    Debug.Log("Nuevo historial creado");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al cargar historial: {e.Message}");
            historial = new HistorialCompleto();
        }
    }
    
    /// <summary>
    /// Obtiene la ruta donde se guardan los archivos
    /// </summary>
    public string ObtenerRutaGuardado()
    {
        return rutaGuardado;
    }
    
    /// <summary>
    /// Cambia la ubicación de guardado y mueve archivos existentes
    /// </summary>
    public void CambiarUbicacionGuardado(TipoUbicacionGuardado nuevaUbicacion)
    {
        string rutaAnterior = rutaGuardado;
        ubicacionGuardado = nuevaUbicacion;
        
        // Reconfigurar rutas
        ConfigurarRutasGuardado();
        
        // Intentar mover archivos existentes a la nueva ubicación
        if (Directory.Exists(rutaAnterior) && rutaAnterior != rutaGuardado)
        {
            try
            {
                MoverArchivosExistentes(rutaAnterior, rutaGuardado);
                Debug.Log($"📁 Archivos movidos de {rutaAnterior} a {rutaGuardado}");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"⚠️ No se pudieron mover todos los archivos: {e.Message}");
            }
        }
    }
    
    /// <summary>
    /// Obtiene información sobre todas las ubicaciones disponibles
    /// </summary>
    public string ObtenerInformacionUbicaciones()
    {
        string info = "📁 UBICACIONES DISPONIBLES EN QUEST:\n\n";
        
        info += "🗂️ DOCUMENTOS (/sdcard/Documents/TEA_VR_Results)\n";
        info += "✅ Más fácil de encontrar\n";
        info += "✅ Accesible desde explorador de archivos\n";
        info += "✅ Similar a carpeta Documentos de Windows\n\n";
        
        info += "📥 DESCARGAS (/sdcard/Download/TEA_VR_Results)\n";
        info += "✅ Como archivos descargados del navegador\n";
        info += "✅ Fácil acceso desde Quest Browser\n";
        info += "✅ Visible en gestores de archivos\n\n";
        
        info += "📱 ALMACENAMIENTO APP (/sdcard/Android/data/[app]/files)\n";
        info += "• Ubicación específica de la aplicación\n";
        info += "• Requiere navegar a carpeta específica\n";
        info += "• Se elimina si desinstalas la app\n\n";
        
        info += "🔒 ALMACENAMIENTO INTERNO (Unity persistentDataPath)\n";
        info += "• Menos accesible desde explorador\n";
        info += "• Para uso interno de la aplicación\n\n";
        
        info += $"📍 UBICACIÓN ACTUAL: {ObtenerNombreUbicacion(ubicacionGuardado)}\n";
        info += $"📂 RUTA: {rutaGuardado}";
        
        return info;
    }
    
    /// <summary>
    /// Obtiene el nombre descriptivo de una ubicación
    /// </summary>
    public string ObtenerNombreUbicacion(TipoUbicacionGuardado tipo)
    {
        switch (tipo)
        {
            case TipoUbicacionGuardado.DocumentosPublicos: return "Documentos";
            case TipoUbicacionGuardado.DescargasPublicas: return "Descargas";
            case TipoUbicacionGuardado.AlmacenamientoApp: return "Almacenamiento de App";
            case TipoUbicacionGuardado.AlmacenamientoInterno: return "Almacenamiento Interno";
            default: return "Desconocido";
        }
    }
    
    /// <summary>
    /// Mueve archivos existentes de una ubicación a otra
    /// </summary>
    private void MoverArchivosExistentes(string rutaOrigen, string rutaDestino)
    {
        if (!Directory.Exists(rutaOrigen)) return;
        
        // Crear directorio destino si no existe
        if (!Directory.Exists(rutaDestino))
        {
            Directory.CreateDirectory(rutaDestino);
        }
        
        // Mover archivos JSON
        string[] archivosJSON = Directory.GetFiles(rutaOrigen, "*.json");
        foreach (string archivo in archivosJSON)
        {
            string nombreArchivo = Path.GetFileName(archivo);
            string rutaDestinol = Path.Combine(rutaDestino, nombreArchivo);
            File.Move(archivo, rutaDestinol);
        }
        
        // Mover archivos CSV
        string[] archivosCSV = Directory.GetFiles(rutaOrigen, "*.csv");
        foreach (string archivo in archivosCSV)
        {
            string nombreArchivo = Path.GetFileName(archivo);
            string rutaDestinoArchivo = Path.Combine(rutaDestino, nombreArchivo);
            File.Move(archivo, rutaDestinoArchivo);
        }
    }
    
    /// <summary>
    /// Obtiene estadísticas de la sesión actual
    /// </summary>
    public string ObtenerEstadisticasSesion()
    {
        int totalEjercicios = sesionActual.ejercicios.Count;
        int totalExitos = 0;
        int totalFallas = 0;
        float tiempoTotal = 0f;
        
        foreach (var ejercicio in sesionActual.ejercicios)
        {
            totalExitos += ejercicio.totalExitos;
            totalFallas += ejercicio.totalFallas;
            tiempoTotal += ejercicio.tiempoTranscurrido;
        }
        
        return $"Sesión actual: {totalEjercicios} ejercicios, {totalExitos} éxitos, {totalFallas} fallas, {tiempoTotal:F1}s total";
    }
    
    /// <summary>
    /// Exporta todos los datos a un archivo CSV para análisis
    /// </summary>
    public void ExportarCSV()
    {
        try
        {
            string rutaCSV = Path.Combine(rutaGuardado, $"Resultados_Export_{DateTime.Now:yyyy-MM-dd_HHmm}.csv");
            
            using (StreamWriter writer = new StreamWriter(rutaCSV))
            {
                // Encabezados
                writer.WriteLine("Fecha,Ejercicio,Exitos,Fallas,Tiempo(s),Dificultad");
                
                // Datos
                foreach (var sesion in historial.sesiones)
                {
                    foreach (var ejercicio in sesion.ejercicios)
                    {
                        writer.WriteLine($"{ejercicio.fechaHora},{ejercicio.nombreEjercicio},{ejercicio.totalExitos},{ejercicio.totalFallas},{ejercicio.tiempoTranscurrido:F1},{ejercicio.nivelDificultad}");
                    }
                }
            }
            
            Debug.Log($"Datos exportados a CSV: {rutaCSV}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al exportar CSV: {e.Message}");
        }
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && guardarAutomaticamente)
        {
            GuardarSesionActual();
            ActualizarHistorial();
        }
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && guardarAutomaticamente)
        {
            GuardarSesionActual();
            ActualizarHistorial();
        }
    }
}
