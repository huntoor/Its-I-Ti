using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class DetectionZone : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> OnDetectionZoneEnter;
    [SerializeField] private UnityEvent<Collider2D> OnDetectionZoneExit;

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            OnDetectionZoneEnter?.Invoke(body);
        }
    }

    private void OnTriggerExit2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            OnDetectionZoneExit?.Invoke(body);
        }
    }
}
