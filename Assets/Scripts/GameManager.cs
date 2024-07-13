using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public static GameManager instance;

    public Vector2 LastSavePosition { get; private set; }
    public int PlayerHealth { get; private set; }
    public int BocketMoney { get; private set; }

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
}
