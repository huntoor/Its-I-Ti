using System;
using UnityEngine;

public class PlayerDamageZone : MonoBehaviour
{
   [SerializeField] int damageToDeal;

    public delegate void DamageEnemy(int damage, GameObject enemy);
    public static DamageEnemy damageEnemy; 

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Enemy"))
        {
            damageEnemy?.Invoke(damageToDeal, body.gameObject);
        }
    }
}
