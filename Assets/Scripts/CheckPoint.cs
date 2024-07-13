using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public delegate void SavePontEntered(Vector2 playerPosition);
    public static SavePontEntered onSavePontEntered;

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            onSavePontEntered?.Invoke(body.transform.position);
        }
    }
    
}
