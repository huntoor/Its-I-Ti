using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    [SerializeField] GameObject currentOneWayPlatform;

    [SerializeField] private BoxCollider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {

            if (currentOneWayPlatform != null) {

                StartCoroutine(DisableCollision());
            
            }
        
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {

            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentOneWayPlatform = null;
    }


    private IEnumerator DisableCollision() {
    
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(2f);

        Physics2D.IgnoreCollision(playerCollider, platformCollider,false);

    }
}
