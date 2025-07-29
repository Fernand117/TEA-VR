using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InfoGuardadoUI : MonoBehaviour
{
    [Header("UI References")]
    public Text txtRutaGuardado;
    public Text txtEstadisticas;
    public Button btnMostrarRuta;
    public Button btnExportarCSV;
    public Button btnVerEstadisticas;
    public Button btnCambiarUbicacion;
    
    [Header("Panel de Información")]
    public GameObject panelInfo;
    public Text txtInformacion;
    
    [Header("Panel Cambiar Ubicación")]
    public GameObject panelUbicaciones;
    public Button btnDocumentos;
    public Button btnDescargas;
    public Button btnAlmacenamientoApp;
    public Button btnAlmacenamientoInterno;
    
    private void Start()
    {
        // Configurar botones
        if (btnMostrarRuta != null)
            btnMostrarRuta.onClick.AddListener(MostrarRutaGuardado);
            
        if (btnExportarCSV != null)
            btnExportarCSV.onClick.AddListener(ExportarDatos);
            
        if (btnVerEstadisticas != null)
            btnVerEstadisticas.onClick.AddListener(MostrarEstadisticas);
            
        if (btnCambiarUbicacion != null)
            btnCambiarUbicacion.onClick.AddListener(MostrarPanelUbicaciones);
            
        // Configurar botones de ubicaciones
        if (btnDocumentos != null)
            btnDocumentos.onClick.AddListener(() => CambiarUbicacion(ResultadosManager.TipoUbicacionGuardado.DocumentosPublicos));
            
        if (btnDescargas != null)
            btnDescargas.onClick.AddListener(() => CambiarUbicacion(ResultadosManager.TipoUbicacionGuardado.DescargasPublicas));
            
        if (btnAlmacenamientoApp != null)
            btnAlmacenamientoApp.onClick.AddListener(() => CambiarUbicacion(ResultadosManager.TipoUbicacionGuardado.AlmacenamientoApp));
            
        if (btnAlmacenamientoInterno != null)
            btnAlmacenamientoInterno.onClick.AddListener(() => CambiarUbicacion(ResultadosManager.TipoUbicacionGuardado.AlmacenamientoInterno));
            
        // Actualizar información inicial
        ActualizarInformacion();
    }
    
    public void MostrarRutaGuardado()
    {
        string ruta = ResultadosManager.Instance.ObtenerRutaGuardado();
        
        string mensaje = $"📁 UBICACIÓN DE ARCHIVOS:\n\n" +
                        $"Ruta: {ruta}\n\n" +
                        $"📱 CÓMO ACCEDER EN QUEST:\n" +
                        $"1. Conecta Quest a PC con cable USB\n" +
                        $"2. En Quest: Permitir acceso a archivos\n" +
                        $"3. En PC: Abrir 'Este equipo'\n" +
                        $"4. Buscar dispositivo Quest\n" +
                        $"5. Navegar a la ruta mostrada arriba\n\n" +
                        $"📊 ARCHIVOS GENERADOS:\n" +
                        $"• Sesion_YYYY-MM-DD_HHMM.json (sesión actual)\n" +
                        $"• HistorialCompleto.json (todos los datos)\n" +
                        $"• Resultados_Export_YYYY-MM-DD_HHMM.csv (para Excel)\n\n" +
                        $"💡 TIP: Para cambiar la ubicación, usa el botón 'Cambiar Ubicación'";
        
        MostrarPanel(mensaje);
        
        if (txtRutaGuardado != null)
            txtRutaGuardado.text = ruta;
            
        Debug.Log($"📁 Ruta de guardado: {ruta}");
    }
    
    public void ExportarDatos()
    {
        try
        {
            ResultadosManager.Instance.ExportarCSV();
            
            string mensaje = $"✅ DATOS EXPORTADOS EXITOSAMENTE\n\n" +
                           $"Se ha creado un archivo CSV con todos los datos.\n" +
                           $"Este archivo se puede abrir en Excel para análisis.\n\n" +
                           $"📍 Ubicación: {ResultadosManager.Instance.ObtenerRutaGuardado()}\n\n" +
                           $"📊 El archivo contiene:\n" +
                           $"• Fecha y hora de cada ejercicio\n" +
                           $"• Nombre del ejercicio\n" +
                           $"• Número de éxitos\n" +
                           $"• Número de fallas\n" +
                           $"• Tiempo transcurrido\n" +
                           $"• Nivel de dificultad";
            
            MostrarPanel(mensaje);
            Debug.Log("📊 Datos exportados a CSV exitosamente");
        }
        catch (System.Exception e)
        {
            string mensaje = $"❌ ERROR AL EXPORTAR\n\n{e.Message}";
            MostrarPanel(mensaje);
            Debug.LogError($"Error al exportar: {e.Message}");
        }
    }
    
    public void MostrarEstadisticas()
    {
        string estadisticas = ResultadosManager.Instance.ObtenerEstadisticasSesion();
        
        string mensaje = $"📊 ESTADÍSTICAS DE LA SESIÓN ACTUAL\n\n" +
                        $"{estadisticas}\n\n" +
                        $"💡 INFORMACIÓN ADICIONAL:\n" +
                        $"• Los datos se guardan automáticamente\n" +
                        $"• Cada ejercicio completado se registra\n" +
                        $"• Los archivos se pueden transferir a PC\n" +
                        $"• Compatible con Excel y Google Sheets";
        
        MostrarPanel(mensaje);
        
        if (txtEstadisticas != null)
            txtEstadisticas.text = estadisticas;
            
        Debug.Log($"📊 Estadísticas: {estadisticas}");
    }
    
    private void MostrarPanel(string mensaje)
    {
        if (panelInfo != null && txtInformacion != null)
        {
            txtInformacion.text = mensaje;
            panelInfo.SetActive(true);
        }
    }
    
    public void CerrarPanel()
    {
        if (panelInfo != null)
        {
            panelInfo.SetActive(false);
        }
    }
    
    private void ActualizarInformacion()
    {
        // Actualizar información en la UI
        if (txtRutaGuardado != null)
        {
            string ruta = ResultadosManager.Instance.ObtenerRutaGuardado();
            txtRutaGuardado.text = $"Guardando en: {Path.GetFileName(ruta)}";
        }
    }
    
    /// <summary>
    /// Verifica si los archivos existen y muestra el estado
    /// </summary>
    public void VerificarArchivos()
    {
        string rutaBase = ResultadosManager.Instance.ObtenerRutaGuardado();
        
        string mensaje = "📋 ESTADO DE ARCHIVOS:\n\n";
        
        // Verificar historial
        string rutaHistorial = Path.Combine(rutaBase, "HistorialCompleto.json");
        bool existeHistorial = File.Exists(rutaHistorial);
        mensaje += $"• HistorialCompleto.json: {(existeHistorial ? "✅ Existe" : "❌ No encontrado")}\n";
        
        // Verificar archivos de sesión
        string[] archivosSesion = Directory.GetFiles(rutaBase, "Sesion_*.json");
        mensaje += $"• Archivos de sesión: {archivosSesion.Length} encontrados\n";
        
        // Verificar archivos CSV
        string[] archivosCSV = Directory.GetFiles(rutaBase, "Resultados_Export_*.csv");
        mensaje += $"• Archivos CSV: {archivosCSV.Length} encontrados\n\n";
        
        if (existeHistorial)
        {
            try
            {
                string contenido = File.ReadAllText(rutaHistorial);
                var historial = JsonUtility.FromJson<HistorialCompleto>(contenido);
                mensaje += $"📊 DATOS EN HISTORIAL:\n";
                mensaje += $"• Sesiones registradas: {historial.sesiones.Count}\n";
                mensaje += $"• Dispositivo: {historial.dispositivo}\n";
                mensaje += $"• Versión: {historial.versionApp}";
            }
            catch (System.Exception e)
            {
                mensaje += $"❌ Error al leer historial: {e.Message}";
            }
        }
        
        MostrarPanel(mensaje);
    }
    
    /// <summary>
    /// Muestra el panel para cambiar ubicación de guardado
    /// </summary>
    public void MostrarPanelUbicaciones()
    {
        string mensaje = ResultadosManager.Instance.ObtenerInformacionUbicaciones();
        
        mensaje += "\n\n💡 RECOMENDACIONES:\n";
        mensaje += "📁 DOCUMENTOS: Más fácil de encontrar (RECOMENDADO)\n";
        mensaje += "📥 DESCARGAS: Accesible desde el navegador Quest\n";
        mensaje += "📱 APP: Ubicación actual por defecto\n";
        mensaje += "🔒 INTERNO: Para uso avanzado solamente\n\n";
        mensaje += "⚠️ Los archivos existentes se moverán automáticamente";
        
        MostrarPanel(mensaje);
        
        // Mostrar panel de botones de ubicación si existe
        if (panelUbicaciones != null)
        {
            panelUbicaciones.SetActive(true);
        }
    }
    
    /// <summary>
    /// Cambia la ubicación de guardado
    /// </summary>
    public void CambiarUbicacion(ResultadosManager.TipoUbicacionGuardado nuevaUbicacion)
    {
        try
        {
            string nombreUbicacion = ResultadosManager.Instance.ObtenerNombreUbicacion(nuevaUbicacion);
            
            ResultadosManager.Instance.CambiarUbicacionGuardado(nuevaUbicacion);
            
            string mensaje = $"✅ UBICACIÓN CAMBIADA EXITOSAMENTE\n\n" +
                           $"Nueva ubicación: {nombreUbicacion}\n" +
                           $"Ruta: {ResultadosManager.Instance.ObtenerRutaGuardado()}\n\n" +
                           $"📁 Los archivos existentes se han movido a la nueva ubicación.\n" +
                           $"📊 Los próximos datos se guardarán aquí.\n\n" +
                           $"💡 Conecta las Quest a tu PC y navega a la nueva ruta para acceder a los archivos.";
            
            MostrarPanel(mensaje);
            
            // Cerrar panel de ubicaciones
            if (panelUbicaciones != null)
            {
                panelUbicaciones.SetActive(false);
            }
            
            // Actualizar información en la UI
            ActualizarInformacion();
            
            Debug.Log($"📁 Ubicación cambiada a: {nombreUbicacion}");
        }
        catch (System.Exception e)
        {
            string mensaje = $"❌ ERROR AL CAMBIAR UBICACIÓN\n\n{e.Message}";
            MostrarPanel(mensaje);
            Debug.LogError($"Error al cambiar ubicación: {e.Message}");
        }
    }
    
    /// <summary>
    /// Cierra el panel de ubicaciones
    /// </summary>
    public void CerrarPanelUbicaciones()
    {
        if (panelUbicaciones != null)
        {
            panelUbicaciones.SetActive(false);
        }
    }
}
