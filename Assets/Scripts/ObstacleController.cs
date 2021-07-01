using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private PlayerController thePlayerScript;  // player script
    private AudioSource theAudio; // audio component
    private GameObject  collisionParticles; // particle effect (set in editor) on colliding
    public  AudioClip   hitNoise;      // explosion audio clip to play on a collision with us (set in editor)
    public  AudioClip   gameOverNoise; // game over voice (set in editor)
    
    // Start is called before the first frame update
    void Start()
    {
        // get audio component 
        theAudio = GetComponent<AudioSource>();
        
        // get player script to stop updates on collision here - no game controller in simple game example
        thePlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // find particle effect
        collisionParticles = GameObject.FindGameObjectWithTag("CollisionFlames");
    }

    private void OnTriggerEnter(Collider other)
    {
        // something has collided with the obstacle
        if (other.gameObject.CompareTag("Player"))
        {
            // collided with player - play explosion noise & particle effect and remove
            // as haven't implemented a game controller, just hide the player character!
            thePlayerScript.bFinished = true; // signal game over

            other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            // play explosion noise and particle effect, then game over voice too!
            theAudio.PlayOneShot(hitNoise);
            collisionParticles.transform.position = transform.position;
            collisionParticles.GetComponentInChildren<ParticleSystem>().Play();

            // start coroutine for game over voice
            StartCoroutine("PlayGameOver");
        }
    }
    
    IEnumerator PlayGameOver()
    {
        yield return new WaitForSeconds(hitNoise.length +1f); // wait till this has finished, plus a small gap
        theAudio.PlayOneShot(gameOverNoise);
    }
}
