using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(5, 15)] [SerializeField] private int maxHealth;

    Rigidbody2D rb;
    int playerHealth;
    //int playerScore;
    int playerLives;
    int potHpIncrease;

    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private AudioClip hurtClip;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        DamageZone.damagePlayer += TakeDamage;
        GameManager.increaseHP += IncreaseHP;
    }

    void OnDisable()
    {
        DamageZone.damagePlayer -= TakeDamage;
        GameManager.increaseHP -= IncreaseHP;
    }

    void Start()
    {
        maxHealth = 10;
        playerHealth = maxHealth;
        potHpIncrease = 3;

        //playerScore = 0;
        playerLives = 3;

        GameManager.instance.PlayerHealth = playerHealth;

    }

    void TakeDamage(int damage)
    {
        playerHealth -= damage;

        GameManager.instance.PlayerHealth = playerHealth;

        myAudioSource.clip = hurtClip;
        myAudioSource.loop = false;
        myAudioSource.Play();


        if (playerHealth <= 0)
        {
            Die();
        }
    }

    void IncreaseHP()
    {
        if (!(playerHealth + potHpIncrease  > maxHealth))
        {
            playerHealth += potHpIncrease;   
        }
    }

    void Die()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            GameOver();
        }
        else
        {
            PlayerRespawn();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }

    void PlayerRespawn()
    {
        transform.position = GameManager.instance.LastSavePosition;

        playerHealth = 10;
    }

    public void PauseGame()
    {
        GameManager.instance.PauseGame(true);
    }

    public void ResumeGame()
    {
        GameManager.instance.PauseGame(false);
    }
}
