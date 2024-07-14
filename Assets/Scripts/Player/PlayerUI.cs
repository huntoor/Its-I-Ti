using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    int playerHealth;

    [SerializeField] private Transform container;
    [SerializeField] private Sprite heartImage;

    void Start()
    {
        playerHealth = GameManager.instance.PlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void CreateHeart(Sprite heartSprite, int posIndex)
    {
        
    }
}
