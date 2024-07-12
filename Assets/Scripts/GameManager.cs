using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public int playerHealth;
    public Items.ItemType[] playerUnlockedSkills;
    public Transform lastSavePosition;
    public int bocketMoney;

    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
}
