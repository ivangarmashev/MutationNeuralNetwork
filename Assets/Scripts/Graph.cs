using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject gameobj;
    public Component[] hingeJoints;
    public int countChild;
    public Transform[] childs;
    
    private void Start()
    {
        hingeJoints = GetComponentsInChildren<HingeJoint>();
        countChild = transform.childCount;
        childs = new Transform[countChild];
        string name = "Edge";
        for (int i = 0; i < countChild - 4; i++)
        {
            string index = name + " (" + i + ")";
            Transform child = transform.Find(index);
            if (child != null)
            {
                childs.SetValue(child, i);
            }
        }
    }
}

