using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerControllerServer : MonoBehaviour
{
	[SerializeField] private CharacterController2D controller;

	[SerializeField] private Animator anim;

	[SerializeField] private float runSpeed = 40f;

	[SerializeField] PhotonView view;

	[Range(0, -5)] [SerializeField] private float fallValue = -2.5f;

	[SerializeField] private bool phoneControl;

	private Joystick joystick;

	private Rigidbody2D rb;

	private float horizontalMove = 0f;

	private bool jump = false;

	private bool crouch = false;

	private bool m_FacingRight = true;

	public void Start()
	{
		//joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();

		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (view.IsMine)
		{
			if (phoneControl)
			{
				horizontalMove = joystick.Horizontal * runSpeed;
			}
			else
			{
				horizontalMove = Input.GetAxis("Horizontal") * runSpeed;

				if(Input.GetKeyDown(KeyCode.Space))
                {
					onJumpBouttonDown();
                }
			}

			if (horizontalMove != 0)
			{
				anim.SetBool("speed", true);
			}
			else
			{
				anim.SetBool("speed", false);
			}
		}
	}

	void FixedUpdate()
	{
		if (view.IsMine)
		{
			float move = horizontalMove * Time.fixedDeltaTime;

			controller.Move(move, crouch, jump);

			jump = false;

			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}

	public void onJumpBouttonDown()
	{
		jump = true;
	}

	public void onLanding()
	{
		anim.SetBool("isFalling", false);

		anim.SetBool("isJumping", false);
	}

	public IEnumerator noLandingDelay()
	{
		yield return new WaitForSeconds(0.1f);

		anim.SetBool("isFalling", false);

		anim.SetBool("isJumping", false);

		StopCoroutine("noLandingDelay");
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
	}
}
