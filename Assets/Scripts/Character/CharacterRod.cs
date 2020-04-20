using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityConstants;
using CommonsHelper;

public class CharacterRod : MonoBehaviour
{
    private static readonly int SwingingParamHash = Animator.StringToHash("Swinging");

    private static readonly Collider2D[] physicsResults = new Collider2D[2];
    
    /* Sibling components */
    private Animator animator;
    private CharacterControl characterControl;
    
    /* Children components */
    [Tooltip("Swing Hitbox, used in Circle Overlap test")]
    public CircleCollider2D swingHitbox;
    
    [Tooltip("Rod Flame, activated when rod is lit")]
    public GameObject rodFlame;
    
    /* State */
    private bool m_IsLit;
    private bool m_IsSwinging;
    public bool IsSwinging => m_IsSwinging;

    private void Awake()
    {
        animator = this.GetComponentOrFail<Animator>();
        characterControl = this.GetComponentOrFail<CharacterControl>();
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
        animator.SetBool(SwingingParamHash, true);
    }

    private void SwingHit()
    {
        if (!m_IsLit)
        {
            // we don't check for triggers specifically, anyway all Fire Sources should be triggers
            int resultsCount = Physics2D.OverlapCircleNonAlloc(
                (Vector2)swingHitbox.transform.position + swingHitbox.offset,
                swingHitbox.radius, physicsResults,
                Layers.FireSourceMask);
            if (resultsCount > 0)
            {
                // we touched a fire source, light rod on
                LightOn();
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