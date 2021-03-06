﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsHelper;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class CharacterMotor : MonoBehaviour
{
	private static readonly int WalkingParamHash = Animator.StringToHash("Walking");
	private static readonly int DirectionParamHash = Animator.StringToHash("Direction");

	/* Sibling components */
	private Rigidbody2D rigidbody2d;
	private Animator animator;
	private CharacterControl characterControl;
	private CharacterRod m_CharacterRod;

	/* Parameters */
	[SerializeField, Tooltip("Character speed")]
	private float speed = 2f;

	/* State */
	private CardinalDirection m_Direction;

	private void Awake ()
	{
		rigidbody2d = this.GetComponentOrFail<Rigidbody2D>();
		animator = this.GetComponentOrFail<Animator>();
		characterControl = this.GetComponentOrFail<CharacterControl>();
		m_CharacterRod = this.GetComponentOrFail<CharacterRod>();
	}

	private void Start() {
		Setup();
	}

	private void Setup()
	{
		m_Direction = CardinalDirection.Down;
	}

	private void FixedUpdate ()
	{
		Vector2 moveIntention = Vector2.zero;
		
		if (CanMove())
		{
			// we assume move intention coordinates are 0/1 are in old school games using D-pad
			moveIntention = characterControl.moveIntention;
		}
		
		// outside conditional block to make sure character stops after Swing
		rigidbody2d.velocity = speed * moveIntention;

		// even if character walks against a wall, show walking animation
		bool isWalkingAnim = moveIntention != Vector2.zero;
		
		animator.SetBool(WalkingParamHash, isWalkingAnim);
		
		if (isWalkingAnim)
		{
			UpdateDirection(moveIntention);
			
			animator.SetInteger(DirectionParamHash, (int) m_Direction);
		}
	}

	private bool CanMove()
	{
		// character cannot move during Swing
		return !m_CharacterRod.IsSwinging;
	}

	private void UpdateDirection(Vector2 moveIntention)
	{
		if (moveIntention.x != 0f && moveIntention.y != 0f)
		{
			// diagonal motion:
			// if one of the two directions is the current direction, preserve it
			// else, give priority to vertical direction
			if (moveIntention.x < 0f && m_Direction == CardinalDirection.Left)
			{
				m_Direction = CardinalDirection.Left;
			}
			else if (moveIntention.x > 0f && m_Direction == CardinalDirection.Right)
			{
				m_Direction = CardinalDirection.Right;
			}
			else if (moveIntention.y < 0f && m_Direction == CardinalDirection.Down)
			{
				m_Direction = CardinalDirection.Down;
			}
			else if (moveIntention.y > 0f && m_Direction == CardinalDirection.Up)
			{
				m_Direction = CardinalDirection.Up;
			}
			else if (moveIntention.y < 0f)
			{
				m_Direction = CardinalDirection.Down;
			}
			else
			{
				m_Direction = CardinalDirection.Up;
			}
		}
		else if (moveIntention.x < 0f)
		{
			m_Direction = CardinalDirection.Left;
		}
		else if (moveIntention.x > 0f)
		{
			m_Direction = CardinalDirection.Right;
		}
		else if (moveIntention.y < 0f)
		{
			m_Direction = CardinalDirection.Down;
		}
		else // if (moveIntention.y > 0f)
		{
			m_Direction = CardinalDirection.Up;
		}
	}
}
