using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public bool door0 = false;
    public bool door1 = true;
    private bool internalFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        internalFlag = true;
        PathFinding pathScript = GameObject.FindGameObjectWithTag("Graph").GetComponent<PathFinding>();
        bool flag = pathScript.waitFlag;
        int[] pathArray = pathScript.shortestPath;
        while (flag == false && internalFlag == false)
        {
            string start = "StartDoor " + pathArray[0] + "-" + pathArray[1];
            for(int i = 0; i < pathArray.Length - 2; i++)
            {
                string door = "Door " + pathArray[i] + "-" + pathArray[i + 1] + "-" + pathArray[i + 2];
                transform.Find("Doors");
                Transform doorObject = transform.Find("Doors").Find(door);

                if (doorObject.childCount > 0)
                {
                    for (int j = 0; j < doorObject.transform.childCount; j++)
                    {
                        doorObject.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Door");
                        doorObject.GetChild(j).GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                else
                {
                    doorObject.gameObject.layer = LayerMask.NameToLayer("Door");
                    doorObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
