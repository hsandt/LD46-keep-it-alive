using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

public class Ignitor : MonoBehaviour
{
    private static readonly int Lit = Animator.StringToHash("Lit");
    
    /* Asset references */
    
    [Tooltip("Fire Pit Light On Audio Clip")]
    public AudioClip firePitLightOnSound;
    
    /* External references */

    [Tooltip("Animator of parent Fire Pit")]
    public Animator firePitAnimator;

    /* Sibling components */
    
    private AudioSource audioSource;

    /* Parameters */
    [SerializeField, Tooltip("Fire Pit is lit when player enters the room (will see Ignition animation)")]
    private bool igniteOnStart = false;
    
    private void Awake()
    {
        audioSource = this.GetComponentOrFail<AudioSource>();
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        if (igniteOnStart)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        // lit fire pit via animator, it will activate Flame and disable Ignitor collider
        firePitAnimator.SetBool(Lit, true);

        // audio
        audioSource.PlayOneShot(firePitLightOnSound);
    }
}
