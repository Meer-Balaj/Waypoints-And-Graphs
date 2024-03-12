using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    /* public GameObject[] waypoints;
     int currentWP = 0;

     public float speed = 10.0f;
     public float rotSpeed = 10.0f;
     public float lookAhead = 10.0f;

     GameObject tracker;

     void Start()
     {
         tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
         DestroyImmediate(tracker.GetComponent<Collider>());
         tracker.GetComponent<MeshRenderer>().enabled = false;
         tracker.transform.position = this.transform.position;
         tracker.transform.rotation = this.transform.rotation;
     }

     void ProgressTracker()
     {
         if (Vector3.Distance(tracker.transform.position, this.transform.position) > lookAhead) return;
         if (Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 3)
         {
             currentWP++;
         }
         if (currentWP >= waypoints.Length)
         {
             currentWP = 0;
         }

         tracker.transform.LookAt(waypoints[currentWP].transform);
         tracker.transform.Translate(0, 0, (speed + 20) *  Time.deltaTime);
     }
     void Update()
     {
         ProgressTracker();



         Quaternion lookAtWP = Quaternion.LookRotation(tracker.transform.position - this.transform.position);
         this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAtWP, rotSpeed * Time.deltaTime);

         this.transform.Translate(0, 0, speed * Time.deltaTime);
     }*/

    Transform goal;
    float speed = 5.0f;
    float accuracy = 5.0f;
    float rotSpeed = 2.0f;

    public GameObject wpManager;
    GameObject[] waypoints;
    GameObject currentNode;
    int currentWP = 0;
    Graph g;

    private void Start()
    {
        waypoints = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = waypoints[9];
        Time.timeScale = 5;
    }

    public void GoToHeli()
    {
        g.AStar(currentNode, waypoints[9]);
        currentWP = 0;
    }

    public void GoToRuin()
    {
        g.AStar(currentNode, waypoints[2]);
        currentWP = 0;
    }
    
    public void GoToFactory()
    {
        g.AStar(currentNode, waypoints[5]);
        currentWP = 0;
    }
    public void LateUpdate()
    {
        if(g.pathList.Count == 0 || currentWP == g.pathList.Count)
        {
            return;
        }

        if(Vector3.Distance(g.pathList[currentWP].GetID().transform.position, this.transform.position) < accuracy)
        {
            currentNode = g.pathList[currentWP].GetID();
            currentWP++;
        }

        if(currentWP < g.pathList.Count)
        {
            goal = g.pathList[currentWP].GetID().transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.transform.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
