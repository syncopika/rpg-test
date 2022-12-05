using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private bool isDead;
    private Vector3 forward;
    IEnumerator stopExplosion()
    {
        {
            yield return new WaitForSeconds(0.2f);
            transform.GetComponent<ParticleSystem>().Stop();
            StartCoroutine(destroy());
        }
    }

    IEnumerator destroy()
    {
        {
            yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);
        }
    }

    void checkCollision(Vector3 forward)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.GetComponent<Rigidbody>().position, forward, out hit, 0.1f)) // using transform.position for raycast origin seems to allow the bullet ray to hit its own collider, which is not good
        {
            if (hit.transform)
            {
                // helpful: https://forum.unity.com/threads/is-it-possible-to-fully-turn-off-the-bouncing-at-collision-of-rigidbody.1276091/
                // https://answers.unity.com/questions/462907/how-do-i-stop-a-projectile-cold-when-colliding-wit.html
                // still not sure why the bullet seems to bounce off things sometimes though :/
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                transform.GetComponent<Rigidbody>().detectCollisions = false;
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

                transform.GetComponent<ParticleSystem>().Play();
                StartCoroutine(stopExplosion());
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

    void FixedUpdate()
    {
        if (!isDead)
        {
            checkCollision(forward);

            if(transform.GetComponent<Rigidbody>().position.y <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        //Debug.DrawRay(transform.position, forward, Color.red);
    }
}
