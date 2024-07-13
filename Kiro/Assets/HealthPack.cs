using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    int bonus = 20;
    public HealthBar healthBar;
    [SerializeField]Player player;
    public void increaseHealth() {

        player.currentHealth += bonus;
        healthBar.setHealth(player.currentHealth);
    }
    
}
