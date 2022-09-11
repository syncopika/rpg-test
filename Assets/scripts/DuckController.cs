using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    private int health = 20;
    private bool isDead = false;
    private Quaternion deadStateRotation;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.name.ToLower().Contains("bullet") && health > 0)
        {
            health -= 10;

            if(health == 0)
            {
                isDead = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        deadStateRotation = transform.rotation * Quaternion.Euler(0, 180f, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, deadStateRotation, Time.deltaTime * 5f);
        }
    }
}
