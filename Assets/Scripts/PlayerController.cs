using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] waypoints;         // an array of waypoints that we will simply move towards
    public GameObject   explosionParticle; // explosion particle effect
    public AudioClip    gameOverNoise;     // set up in editor - game over noise
    public AudioClip    explosionNoise;    // set up in editor - explosion noise
    public bool         bFinished = false; // have we finished (for obstacle script use - would maybe serialize it/hide in inspector)

    private GameObject collisionParticles; // particle effect (set in editor) on colliding
    private AudioSource  theAudio; // audio component
    private float moveSpeed = 10f; // player speed
    private int targetWaypoint;    // waypoint to go towards
    

    // Start is called before the first frame update
    void Start()
    {
        // set starting waypoint to go towards
        targetWaypoint = 1; // element 0 of the waypoint array in editor

        // get the audio source component
        theAudio = GetComponent<AudioSource>();

        // find particles
        collisionParticles = GameObject.FindGameObjectWithTag("CollisionFlames");
    }

    // Update is called once per frame
    void Update()
    {
        if (!bFinished)
        {
            // check if we have reached the target position
            if (Vector3.Distance(transform.position, waypoints[targetWaypoint - 1].transform.position) == 0)
            {
                // we are here, so switch to the next one or finish if we are at waypoint 3
                if (targetWaypoint < 3)
                {
                    targetWaypoint++;
                }
                else
                {
                    // set finished
                    bFinished = true;

                    // as haven't implemented a game controller, just hide the player character!
                    gameObject.GetComponent<MeshRenderer>().enabled = false;

                    // play noise
                    theAudio.PlayOneShot(explosionNoise);

                    // play particle effect
                    collisionParticles.transform.position = transform.position;
                    collisionParticles.GetComponentInChildren<ParticleSystem>().Play();

                    StartCoroutine("PlayGameOver");
                }
            }

            if (Vector3.Distance(transform.position, waypoints[targetWaypoint - 1].transform.position) != 0)
            {
                // move the player towards the target waypoint
                gameObject.transform.position = Vector3.MoveTowards(transform.position,
                                                                    waypoints[targetWaypoint - 1].transform.position,
                                                                    Time.deltaTime * moveSpeed);
            }
        }
    }

    // Plays a game over voice after end of explosion noise
    IEnumerator PlayGameOver()
    {
        yield return new WaitForSeconds(explosionNoise.length + 1f); // wait till this has finished, plus a small gap
        theAudio.PlayOneShot(gameOverNoise);
    }
}

