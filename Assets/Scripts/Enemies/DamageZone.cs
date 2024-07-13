using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DamageZone : MonoBehaviour
{
    [SerializeField] int damageToDeal;

    //[SerializeField] public static UnityEvent<int> OnDamageZoneEnter;

    public delegate void DamagePlayer(int damage);
    public static DamagePlayer damagePlayer; 

    private float damageTimer;
    private GameObject player;
    internal static Action<int> damageEvent;

    private void Awake()
    {
        damageTimer = 1f;
    }

    private void Update()
    {
        if (player != null)
        {
            damageTimer -= Time.deltaTime;

            if (damageTimer < 0)
            {
                damagePlayer?.Invoke(damageToDeal);

                damageTimer = 1f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            damagePlayer?.Invoke(damageToDeal);

            player = body.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            damageTimer = 1f;
            
            player = null;
        }
    }
}
