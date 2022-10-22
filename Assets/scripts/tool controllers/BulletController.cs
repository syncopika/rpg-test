using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private bool isDead;
    IEnumerator destroy()
    {
        {
            yield return new WaitForSeconds(0.2f);
            Destroy(this.gameObject);
        }
    }

    void checkCollision(Vector3 forward)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, 1))
        {
            if (hit.transform)
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                transform.GetComponent<ParticleSystem>().Play();
                StartCoroutine(destroy());
                isDead = true;
            }
        }
    }

    void Start()
    {
        transform.GetComponent<ParticleSystem>().Pause();
    }

    void Update()
    {
        if (!isDead)
        {
            Vector3 forward = Vector3.Cross(transform.up, transform.forward); // transform.forward isn't really the forward we want
            forward.Normalize();

            //Debug.DrawRay(transform.position, forward * 10, Color.blue);
            
            checkCollision(forward);
        }
    }
}
