using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab; // our prefab obstacle set up by dragging in editor
    public GameObject[] wayPoints;    // the waypoints (set in editor)
    public int maxToSpawn;  // maximum objects to be spawned (set in editor)

    // Start is called before the first frame update
    void Start()
    {
        GameObject groundPlane = GameObject.FindGameObjectWithTag("GroundPlane"); // needed to determine boundaries of playfield
        GameObject mainCamera  = GameObject.FindGameObjectWithTag("MainCamera");  // needed to restrict height of objects we are spawning (assumed a 3D game!)

        // I know the ground plane exists, so I won't do any checks / set ANY defaults if can't be found in this code example,
        // in the real world, I would.
        // Also, the question doesn't say if 2D or 3D points P1,P2,P3 in example, so assumed points can be in mid air too!

        float groundXmax   = new float(); // width of playfield
        float groundZmax   = new float(); // depth of playfield (implemented as a third person view)

        groundXmax = groundPlane.GetComponent<Transform>().localScale.x; // x scale factor of the ground plane, so we use this * 10 (Unity 10*10 squares) for positions
        groundZmax = groundPlane.GetComponent<Transform>().localScale.z; // y scale factor of the ground plane, so we use this * 10 (Unity 10*10 squares) for positions
                                                                         
        float randomY = 0; // height to spawn at
        float maxHeight = wayPoints[0].transform.position.y; // first waypoint height
        bool bDiff = false;

        // check heights of waypoints
        for (int i = 0; i < 3; i++)
        {
            if (wayPoints[i].transform.position.y > maxHeight)
            {
                // different heights
                maxHeight = wayPoints[i].transform.position.y;
                bDiff = true;
            }
        }

        randomY = wayPoints[0].transform.position.y; // assume same height here

        for (int nToSpawn = 0; nToSpawn <maxToSpawn; nToSpawn++)
        {
            // spawn our obstacles at random postions, restrict to within boundary of playfield, and always visible (Y>0) above plane
            // obstacles will be all same height above plane IF waypoints are at same height,
            // or will be spawned in a 3D volume if they are different heights.

            float randomX = Random.Range(-10*(groundXmax/2), groundXmax *10 /2);  // restrict width
            float randomZ = Random.Range(-10*(groundZmax /2),groundZmax *10 /2);  // restrict depth

            if (bDiff)
            {
                // differing heights - so do a random height
                randomY = Random.Range(0.5f, maxHeight);
            }

            Vector3 randomSpawnPos = new Vector3(randomX, randomY, randomZ); // create the spawn vector

            GameObject newObstacle;

            newObstacle = Instantiate(obstaclePrefab, randomSpawnPos, Quaternion.identity); // display the obstacle
        }
    }
}
