using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityConstants;
using CommonsHelper;

public class CharacterRod : MonoBehaviour
{
    private static readonly int StartSwingParamHash = Animator.StringToHash("StartSwing");
    private static readonly int SwingingParamHash = Animator.StringToHash("Swinging");

    private static readonly Collider2D[] PhysicsResults = new Collider2D[2];
    
    /* External references */

    [Tooltip("Swing Audio Clip")]
    public AudioClip swingSound;
    
    /* Children components */
    
    [Tooltip("Swing Hitbox, used in Circle Overlap test")]
    public CircleCollider2D swingHitbox;
    
    [Tooltip("Rod Flame, activated when rod is lit")]
    public GameObject rodFlame;
    
    /* Sibling components */
    
    private Animator animator;
    private AudioSource audioSource;
    private CharacterControl characterControl;
    
    /* State */
    
    private bool m_IsLit;
    private bool m_IsSwinging;
    public bool IsSwinging => m_IsSwinging;

    private void Awake()
    {
        animator = this.GetComponentOrFail<Animator>();
        characterControl = this.GetComponentOrFail<CharacterControl>();
        audioSource = this.GetComponentOrFail<AudioSource>();
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        // allows to work on Flame active in the editor, but deactivate on start
        LightOff();
        m_IsSwinging = false;
    }

    private void FixedUpdate()
    {
        // consume swing intention even if cannot swing now (no input buffering)
        if (characterControl.ConsumeSwingIntention())
        {
            // check if character can swing
            if (CanSwing())
            {
               StartSwing();
            }
        }
    }

	private bool CanSwing()
	{
        // character cannot interrupt Swing for another Swing (unlike Zelda GB)
		return !m_IsSwinging;
	}
        private void StartSwing()
    {
        m_IsSwinging = true;
        
        // animation-driven move (events will call back)
        // Transitions to Swing anim states must set Can Transition To Self
        animator.SetTrigger(StartSwingParamHash);
        animator.SetBool(SwingingParamHash, true);
        
        // audio
        audioSource.PlayOneShot(swingSound);
    }

    private void SwingHit()
    {
        if (!m_IsLit)
        {
            // we don't check for triggers specifically, anyway all Fire Sources should be triggers
            int resultsCount = Physics2D.OverlapCircleNonAlloc(
                (Vector2)swingHitbox.transform.position + swingHitbox.offset,
                swingHitbox.radius, PhysicsResults,
                Layers.FireSourceMask);
            if (resultsCount > 0)
            {
                // we touched a fire source, light rod on
                LightOn();
            }
        }
        else
        {
            // rod is lit, it can ignite via ignitors
            // we don't check for triggers specifically, anyway all Ignitors should be triggers
            int resultsCount = Physics2D.OverlapCircleNonAlloc(
                (Vector2)swingHitbox.transform.position + swingHitbox.offset,
                swingHitbox.radius, PhysicsResults,
                Layers.IgnitorMask);
            
            for (int i = 0; i < resultsCount; i++)
            {
                var ignitor = PhysicsResults[i].GetComponentOrFail<Ignitor>();
                ignitor.Ignite();
            }
        }
    }

    // Anim event callback
    private void StopSwing()
    {
        m_IsSwinging = false;
        
        animator.SetBool(SwingingParamHash, false);
    }


    private void LightOn()
    {
        m_IsLit = true;
        rodFlame.SetActive(true);
    }
    
    private void LightOff()
    {
        m_IsLit = false;
        rodFlame.SetActive(false);
    }
}