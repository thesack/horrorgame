using UnityEngine;
using System.Collections;

public class Spider_AI : MonoBehaviour {
	
	private const string animIdle = "iddle";
	private const string animWalk = "walk";
	private const string animAttack = "attack_Melee";
	
	public float attackRange = 2f;
	
	private Vector3 startingPoint;
	private Quaternion startingRotation;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 targetDirection = Vector3.zero;
	private Vector3 targetLocation;
	private float moveSpeed = 0.0f;
	private GameObject player;
	private ThirdPersonController playerController;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerController = player.GetComponent<ThirdPersonController>();
		startingPoint = transform.position;
		startingRotation = transform.rotation;
		targetLocation = startingPoint;
		moveDirection = transform.TransformDirection(Vector3.forward);
		targetDirection = Vector3.zero;
		moveSpeed = 0.0f;
		
		Debug.LogWarning("rotation x:" + startingRotation.x + " y:" + startingRotation.y + " z:" + startingRotation.z);
	}
	
	
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == player)
		{
			targetLocation = player.transform.position;
			targetDirection = targetLocation - transform.position;
			moveDirection = targetDirection.normalized;
			
			float speedModifier = 0f;
			float targetDirectionMagnitude = targetDirection.magnitude;
			if (targetDirectionMagnitude != 0f)
			{
				speedModifier = Mathf.Min((3f / targetDirectionMagnitude), 5f);
			}
			else
			{
				speedModifier = 5f;
			}
			moveSpeed = 5f + speedModifier;
			
			if (targetDirectionMagnitude < attackRange)
			{
				// close enough to attack
				playerController.dead = true;
				animation.CrossFade(animAttack);
			}
			else
			{
				animation.CrossFade(animWalk);
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player)	
		{
			targetLocation = startingPoint;
			targetDirection = targetLocation - transform.position;
			moveDirection = targetDirection.normalized;
			moveSpeed = 10f;
		}
	}
	
	void Update()
	{
		
		// moveSpeed is expected to already be set
		Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
		float movementMagnitude = movement.magnitude;
		
		
		// turn towards the target
		if (movementMagnitude > 0f)
		{
			transform.rotation = Quaternion.LookRotation(movement);
		}
		
		// If close to the target position, snap to it.
		if ((targetLocation - transform.position).magnitude < movementMagnitude)
		{
			transform.position = targetLocation;
			if (targetLocation == startingPoint)
			{
				transform.rotation = startingRotation;
				animation.CrossFade(animIdle);
			}
			moveSpeed = 0.0f;
		}
		else
		{
			transform.position += movement;
		}
	}
}
