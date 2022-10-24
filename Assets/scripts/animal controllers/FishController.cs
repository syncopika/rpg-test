using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    int radius = 5;
    float angle;
    float speed = 0.2f;

    Vector3 position;

    Animator animator;

    bool movingTowardsTarget = false;
    bool hasBite = false;

    void OnTriggerStay(Collider other)
    {
        if (hasBite)
        {
            return;
        }

        if (other.transform.name.Equals("floater"))
        {
            // swim towards floater
            //Debug.DrawLine(transform.position, other.transform.position, Color.red);

            movingTowardsTarget = true;

            Vector3 targetDir = other.gameObject.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 0.8f * Time.deltaTime, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);

            //Debug.Log("other pos: " + other.gameObject.transform.position.ToString() + ", fish pos: " + transform.position.ToString());
            if(Vector3.Distance(transform.position, other.gameObject.transform.position) > 2.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, other.gameObject.transform.position, 0.7f * Time.deltaTime);
            }
            else
            {
                // in range for a bite.
                // TODO: randomize whether a bite happens or not. e.g. pick a random num, if meets certain criteria make bite happen
                // assuming bite happens, parent the fish to the floater and trigger flail animation
                transform.parent = other.transform;
                hasBite = true;
                animator.SetBool("isFlail", true);
                other.transform.parent.transform.GetComponent<FishingPoleController>().hasBite(); // other.transform is the fishing pole floater
                Debug.Log("got a bite");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        movingTowardsTarget = false;
    }

    void isCaught()
    {
        // TODO
    }

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // center of the circle swim path

        animator = transform.GetComponent<Animator>();
        animator.SetBool("isSwim", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!movingTowardsTarget && !hasBite)
        {
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle) * radius);
            transform.position = new Vector3(position.x + newPos.x, position.y, position.z + newPos.z);

            // find a vector tangent to the point on the circle path of the fish and have the fish look at it.
            // that way the fish will be facing the right direction when it's swimming in a circle
            Vector3 tangentVec = Vector3.Cross(transform.position - position, Vector3.up).normalized;

            Vector3 tangentPoint = transform.position + (tangentVec * 3f);

            //Debug.DrawLine(transform.position, tangentPoint, Color.blue);
            //Debug.DrawLine(transform.position, position, Color.red);

            transform.LookAt(tangentPoint);

            angle += Time.deltaTime * speed;
        }
    }
}
