using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallex : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallexEffect;

    private float length, startPos;

    void Start()
    {
        startPos = transform.position.x;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float movedDistance = cam.transform.position.x * (1 - parallexEffect);
        float distance = cam.transform.position.x * parallexEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movedDistance > startPos + length)
        {
            startPos += length;
        } 
        else if (movedDistance < startPos - length)
        {
            startPos -= length;
        }
    }
}
