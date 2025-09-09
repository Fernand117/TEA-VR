using UnityEngine;

namespace Assets.Scripts.NivelBasico
{
    public class ReturnToOriginOnRelease : MonoBehaviour
    {
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private OVRGrabbable grabbable;

        private bool wasGrabbed = false;
        private bool returning = false;

        public float returnSpeed = 5f; // velocidad del regreso

        private Rigidbody rb;

        void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;

            grabbable = GetComponent<OVRGrabbable>();
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (grabbable == null) return;

            // Detecta agarre
            if (grabbable.isGrabbed)
            {
                wasGrabbed = true;
                returning = false; // mientras está agarrado no regresamos
            }

            // Detecta cuando se suelta
            if (!grabbable.isGrabbed && wasGrabbed)
            {
                wasGrabbed = false;
                StartReturn();
            }

            // Movimiento suave de regreso
            if (returning)
            {
                transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * returnSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * returnSpeed);

                // Si está muy cerca, fijamos exacto
                if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
                {
                    transform.position = initialPosition;
                    transform.rotation = initialRotation;
                    returning = false;
                    rb.isKinematic = false; // reactivamos físicas si lo necesitas después
                }
            }
        }

        private void StartReturn()
        {
            returning = true;
            if (rb != null)
            {
                rb.isKinematic = true; // evita que rebote
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}