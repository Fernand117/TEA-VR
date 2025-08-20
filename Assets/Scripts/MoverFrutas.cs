namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System.Collections.Generic;

    public class MoverFrutas : MonoBehaviour
    {
        [Header("UI References")]
        public Text txtContador;
        public List<Image> circulosIndicadores;
        public Sprite circuloActivo;

        [Header("Canvas Final")]
        public GameObject canvasFelicitaciones;
        public Text txtTotalAciertos;
        public Text txtTiempo;
        public Text txtTotalIncorrectos;

        [Header("Config Juego")]
        public int maxFrutas = 5;

        private int aciertos = 0;
        private int errores = 0;
        private float tiempoTranscurrido = 0f;
        private bool juegoActivo = true;

        void Start()
        {
            if (canvasFelicitaciones != null) canvasFelicitaciones.SetActive(false);
            aciertos = 0;
            errores = 0;
            tiempoTranscurrido = 0f;
            if (txtContador != null) txtContador.text = "0";
        }

        void Update()
        {
            if (juegoActivo) tiempoTranscurrido += Time.deltaTime;
        }

        public void RegistrarLlegadaFruta(GameObject fruta, bool pasoPorCamino)
        {
            if (!juegoActivo) return;

            if (pasoPorCamino)
            {
                aciertos++;
                if (aciertos - 1 < circulosIndicadores.Count)
                    circulosIndicadores[aciertos - 1].sprite = circuloActivo;
            }
            else
            {
                errores++;
            }

            // Actualizar contador
            if (txtContador != null)
                txtContador.text = aciertos.ToString();

            //Destroy(fruta);

            if ((aciertos + errores) >= maxFrutas)
                MostrarPanelFinal();
        }

        private void MostrarPanelFinal()
        {
            juegoActivo = false;
            float minutos = tiempoTranscurrido / 60f;

            if (txtTotalAciertos != null) txtTotalAciertos.text = aciertos.ToString();
            if (txtTiempo != null) txtTiempo.text = minutos.ToString("F2") + " minutos";
            if (txtTotalIncorrectos != null) txtTotalIncorrectos.text = errores.ToString();

            if (canvasFelicitaciones != null)
                canvasFelicitaciones.SetActive(true);
        }
    }

}
