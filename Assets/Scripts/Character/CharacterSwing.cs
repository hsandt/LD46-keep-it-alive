using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

public class CharacterSwing : MonoBehaviour
{
    private static readonly int SwingingParamHash = Animator.StringToHash("Swinging");
    
    /* Sibling components */
    private Animator animator;
    private CharacterControl characterControl;
    
    /* State */
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
        m_IsSwinging = false;
    }

    private void FixedUpdate()
    {
        if (characterControl.ConsumeSwingIntention())
        {
            StartSwing();
        }
    }

    private void StartSwing()
    {
        m_IsSwinging = true;
        
        // animation-driven move (events will call back)
        animator.SetBool(SwingingParamHash, true);
    }

    // Anim event callback
    private void StopSwing()
    {
        m_IsSwinging = false;
        
        animator.SetBool(SwingingParamHash, false);
    }

}