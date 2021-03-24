using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;

    public GameObject Runner;
    public GameObject Trigger;

    public float speed = 10f;

    public GameObject rotation1;
    public GameObject rotation2;

    bool opendoor = false;


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sphere")
        {
            cube1.GetComponent<Renderer>().material.color = Color.green;
            cube2.GetComponent<Renderer>().material.color = Color.green;
            Runner.GetComponent<Animator>().SetBool("moveforward", false);
            Variables.moveforward = false;
            Trigger.SetActive(false);
            opendoor = true;
        }
    }

    private void Update()
    {
        if (opendoor == true)
        {
            rotation1.transform.Rotate(new Vector3(0f, -3f, 0f));
            rotation2.transform.Rotate(new Vector3(0f, 3f, 0f));
        }


    }





}
