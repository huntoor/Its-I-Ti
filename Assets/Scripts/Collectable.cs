using UnityEngine;

public class Collectable : MonoBehaviour
{
    enum Type
    {
        healthPot,
        coin,
    }

   [SerializeField] private Type collectableType;

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            switch (collectableType)
            {
                case Type.healthPot:
                    GameManager.instance.IncreaseHealth();
                    break;
                default:
                    break;
            }
        }
    }
}
