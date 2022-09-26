using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    int radius = 5;
    float angle;
    float speed = 0.2f;

    Vector3 position;

    // TODO: if fishing pole floater is within range, swim towards it
    // currently normal is perpendicular to the head of the fish.
    //
    // to swim to floater, rotate fish until head (x-axis?) faces floater. (use RotateTowards?)
    // then swim towards via adding to transform.position.

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Equals("floater"))
        {
            // TODO: swim towards floater
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle)* radius);
        transform.position = new Vector3(position.x + newPos.x, position.y, position.z + newPos.z);
        transform.LookAt(position);
        angle += Time.deltaTime * speed;
    }
}
