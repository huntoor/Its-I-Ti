using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public static GameManager instance;

    public delegate void IncreaseHP();
    public static IncreaseHP increaseHP;

    public Vector2 LastSavePosition { get; set; }
    public int PlayerHealth { get; set; }
    public int BocketMoney { get; set; }

    private int unlockedLevels;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        CheckPoint.onSavePontEntered += SaveCurrentLocation;
    }

    private void Start()
    {
        LastSavePosition = player.transform.position;

        unlockedLevels = 0;
    }

    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SaveCurrentLocation(Vector2 playerCurrentLocation)
    {
        LastSavePosition = playerCurrentLocation;
    }

    public int GetUnlockedLevels()
    {
        return unlockedLevels;
    }

    public void UnlockNextLevel()
    {
        unlockedLevels += 1;
    }

    public void IncreaseHealth()
    {
        increaseHP?.Invoke();
    }
}
