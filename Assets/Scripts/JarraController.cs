using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class JarraController : MonoBehaviour
    {
        public Text contadorTexto;    // Texto del número
        public Image[] esferasUI;                // Arreglo de imágenes (las 5 bolitas)
        public Sprite esferaActiva;              // Imagen de la esfera cuando acierta
        public GameObject finalWindow;           // Panel FinalWindow
        public Text textoAciertos;
        public Text textoTiempo;
        public Text textoIncorrectos;

        private int aciertos = 0;
        private float tiempoInicio;
        private bool ejercicioTerminado = false;

        void Start()
        {
            tiempoInicio = Time.time;
            contadorTexto.text = "0";
            finalWindow.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ejercicioTerminado) return;

            if (other.CompareTag("Vaso")) // Tus vasos deben tener el tag "Vaso"
            {
                aciertos++;
                contadorTexto.text = aciertos.ToString();

                // Cambiar imagen en esferasUI
                if (aciertos <= esferasUI.Length)
                {
                    esferasUI[aciertos - 1].sprite = esferaActiva;
                }

                // Desactivar el vaso para que no cuente doble
                other.gameObject.SetActive(false);

                if (aciertos >= 5)
                {
                    TerminarEjercicio();
                }
            }
        }

        private void TerminarEjercicio()
        {
            ejercicioTerminado = true;

            float tiempoTotal = Time.time - tiempoInicio;
            int incorrectos = 5 - aciertos;

            finalWindow.SetActive(true);
            textoAciertos.text = aciertos.ToString();
            textoTiempo.text = tiempoTotal.ToString("F2") + " segundos";
            textoIncorrectos.text = incorrectos.ToString();

            ResultadosManager.Instance.GuardarResultado(
                nombreEjercicio: "Servir bebidas en copas",
                exitos: aciertos,
                fallas: incorrectos,
                tiempo: tiempoTotal,
                dificultad: "Básico"
            );
        }
    }

}
