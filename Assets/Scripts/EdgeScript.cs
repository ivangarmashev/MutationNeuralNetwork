using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EdgeScript : MonoBehaviour
{
    public List<string> ListObjects = new List<string>() {};   
    public GameObject[] points = {null, null};
    public float lenght;
    [Range(1f, 1.1f)] public float carWeitgh = 1f;
    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        lenght = gameObject.transform.localScale.y;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.name.StartsWith("Mover") == true)
        {
            lenght *= carWeitgh;
        }
        else if (other.name.StartsWith("Point") == true && points[counter] == null)
        {
            points[counter] = other.gameObject;
            counter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.name.StartsWith("Mover") == true)
        {
            lenght /= carWeitgh;
        }
    }
}
