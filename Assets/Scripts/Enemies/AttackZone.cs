using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackZone : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> OnAttackZoneEnter;
    [SerializeField] private UnityEvent<Collider2D> OnAttackZoneExit;

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            OnAttackZoneEnter?.Invoke(body);
        }
    }

    private void OnTriggerExit2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            OnAttackZoneExit?.Invoke(body);
        }
    }
}
