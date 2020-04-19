using System;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    /// Value at which the lower bound starts.
    /// Values in the input below min, in absolute value, will get dropped, and values
    /// at or above will be replaced with -1 or 1 (keeping the same sign as the original value).
    /// This is useful to convert stick input into D-pad-like input.
    [SerializeField, Tooltip("Input binarization min threshold. Values below (in abs) are cut, values above clamped to -1 or +1.")]
    private float inputMinThreshold = 0.125f;

    /* State */
    private Vector2 m_MoveIntention;
    public Vector2 moveIntention { get { return m_MoveIntention; } }

	private void Start() {
		Setup();
	}

	private void Setup()
	{
		m_MoveIntention = Vector2.zero;
	}

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_MoveIntention = new Vector2(GetBinarizedValue(moveInput.x), GetBinarizedValue(moveInput.y));
    }

    private float GetBinarizedValue(float value)
    {
        // Process input so that each coordinate is 0/1 as in old school games with D-pad,
        // even when using gamepad left stick.
        // For analog input (stick), ceil any sufficient input coordinate to 1, independently from the other.
        // For digital input, this computation will preserve the binary value.
        return Mathf.Abs(value) > inputMinThreshold ? Mathf.Sign(value) : 0f;
    }
}
