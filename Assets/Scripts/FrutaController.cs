using UnityEngine;

namespace Assets.Scripts
{
    public class FrutaController : MonoBehaviour
    {
        private bool pasoPorCamino = false;
        private bool yaProcesada = false; // 👈 evita dobles conteos

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Camino"))
            {
                pasoPorCamino = true;
            }

            if (other.CompareTag("PlatoDestino") && !yaProcesada)
            {
                yaProcesada = true; // ✅ marcar como procesada

                // Buscar el GameManager en la escena
                MoverFrutas gameManager = Object.FindFirstObjectByType<MoverFrutas>();
                if (gameManager != null)
                {
                    gameManager.RegistrarLlegadaFruta(this.gameObject, pasoPorCamino);
                }
            }
        }
    }
}
