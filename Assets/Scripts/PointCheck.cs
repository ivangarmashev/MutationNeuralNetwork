using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCheck : MonoBehaviour
{
    public int number = 0;
    public GameObject[] objects;
    public ArrayList array = new ArrayList();
    public Collider[] colliders;
    public ContactPoint[] contactPoints;
    public ArrayList[] mas = new ArrayList[10];
    public int countPoint = 0;
    public int countContact = 0;
    // array = ;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        countContact++;
    }


    private void OnTriggerExit(Collider other)
    {
        countContact--;
        //Debug.Log(other.name + " покинул вершину " + gameObject.name);
    }

}
