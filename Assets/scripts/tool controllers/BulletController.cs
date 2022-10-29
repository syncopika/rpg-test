using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private bool isDead;
    private Vector3 forward;
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
        if (Physics.Raycast(transform.position, forward, out hit, 0.5f))
        {
            if (hit.transform)
            {
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                //transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //transform.GetComponent<Rigidbody>().isKinematic = true;

                transform.GetComponent<ParticleSystem>().Play();
                StartCoroutine(destroy());
                isDead = true;
            }
        }
    }

    void Start()
    {
        transform.GetComponent<ParticleSystem>().Pause();

        forward = Vector3.Cross(transform.up, transform.forward); // transform.forward isn't really the forward we want
        forward.Normalize();
    }

    void Update()
    {
        if (!isDead)
        {
            checkCollision(forward);

            if(transform.position.y <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        //Debug.DrawRay(transform.position, forward, Color.red);
    }
}
