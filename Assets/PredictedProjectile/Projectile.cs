using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    private const bool V = false;
    public Rigidbody projectile;
    public GameObject cursor;
    public Transform shootPoint;
    public LayerMask layer;
    public LineRenderer lineVisual;
    public int lineSegment = 10;
    public float flightTime = 1f;

    private Camera cam;

    public float speed = 10f;
    public float enemyspeed = 12f;

    public GameObject enemy;
    public GameObject Trigger;


    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        lineVisual.positionCount = lineSegment + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        LaunchProjectile();

        if (Variables.moveforward == false)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

        }

        enemy.transform.Translate(Vector3.forward * Time.deltaTime * enemyspeed);
    }

    void LaunchProjectile()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point + Vector3.up * 0.1f;

            Vector3 vo = CalculateVelocty(hit.point, shootPoint.position, flightTime);

            Visualize(vo, cursor.transform.position); //we include the cursor position as the final nodes for the line visual position

            //transform.rotation = Quaternion.LookRotation(vo);

            if (Input.GetMouseButtonUp(0))
            {
                Rigidbody obj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
                obj.velocity = vo;
            }
        }
    }

    //added final position argument to draw the last line node to the actual target
    void Visualize(Vector3 vo, Vector3 finalPos)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, (i / (float)lineSegment) * flightTime);
            lineVisual.SetPosition(i, pos);
        }

        lineVisual.SetPosition(lineSegment, finalPos);
    }

    Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz / time;
        float Vy = (sY / time) - (0.5f * Physics.gravity.y * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;

        result.y = sY;

        return result;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trigger")
        {
            //gameObject.transform.Translate(new Vector3(0, 0, 0));
            Variables.moveforward = true;
            anim.SetBool("moveforward", true);
        }


    }


    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
