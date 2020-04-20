using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

public class Ignitor : MonoBehaviour
{
    /* Asset references */
    
    [Tooltip("Fire Pit Light On Audio Clip")]
    public AudioClip firePitLightOnSound;
    
    /* External references */

    [Tooltip("Flame to ignite")]
    public GameObject flame;

    /* Sibling components */
    
    private AudioSource audioSource;
    private Collider2D collider2D;
    
    private void Awake()
    {
        audioSource = this.GetComponentOrFail<AudioSource>();
        collider2D = this.GetComponentOrFail<Collider2D>();
    }

    public void Ignite()
    {
        flame.SetActive(true);
        
        // disable collider to make sure we don't try to ignite the same ignitor later
        // don't deactivate the whole game object, it would prevent ignition sound from playing
        collider2D.enabled = false;

        // audio
        audioSource.PlayOneShot(firePitLightOnSound);
    }
}
