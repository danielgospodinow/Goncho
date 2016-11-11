using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour
{

    public LayerMask ground;
    private Vector3 targetPosition;
    public float speed=5;
    public bool lookRight = true;
    
    private Animator animator;

	void Start ()
	{
	    targetPosition = transform.position;
	    animator = GetComponent<Animator>();
	}
	

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 8888, ground))
            {
                targetPosition = hit.point + new Vector3(0,0,-2);
                //targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(Random.Range(0f, 1.0f) > 0.5f)
                animator.SetTrigger("attack");
            else
                animator.SetTrigger("special");
        }

       if (targetPosition.x > transform.position.x && !lookRight)
          Flip();
        if(targetPosition.x < transform.position.x && lookRight)
            Flip();

	    var p = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

	    //Vector3 vel = targetPosition - transform.position;
        //vel = Vector3.ClampMagnitude(vel, speed * Time.deltaTime);
        //transform.position += vel;

        animator.SetFloat("speed", (transform.position - p).magnitude/Time.deltaTime);
	}

    public void Flip()
    {
        var s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
        lookRight = !lookRight;
    }
}
