using UnityEngine;
using Oculus.Interaction;

public class BubblePopper : MonoBehaviour
{
    private bool isPopped = false;
    [SerializeField] private float touchThreshold = 0.01f; // Adjust this value for touch sensitivity
    [SerializeField] private AudioSource popSound; // Optional: Add pop sound

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bubble hasn't been popped yet
        if (!isPopped)
        {
            // Check if the collider is a finger
            if (other.gameObject.tag == "HandTracker" || other.gameObject.name.Contains("Finger"))
            {
                PopBubble();
            }
        }
    }

    private void PopBubble()
    {
        isPopped = true;
        
        // Optional: Play pop sound
        if (popSound != null)
        {
            popSound.Play();
        }

        // Optional: Add particle effect or animation here

        // Make the bubble disappear
        gameObject.SetActive(false);
    }
}