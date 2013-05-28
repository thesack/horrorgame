using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {
	
	public AudioClip creaking;
	public float forceAmount = 150f;
	
	private GameObject player;
	private PlayerInventory playerInventory;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerInventory = player.GetComponent<PlayerInventory>();
	}
	
	void OnMouseDown()
	{
		if(playerInventory.hasKey)
		{
			AudioSource.PlayClipAtPoint(creaking, transform.position, 100f);
			rigidbody.AddForce(transform.forward * forceAmount, ForceMode.Acceleration);
			rigidbody.useGravity = true;
		}
	}
}
