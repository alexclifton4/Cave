using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject hintText;

	// Called when something is over the door
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// See if its the player
		PlayerController player = collision.GetComponent<PlayerController>();
		if (player)
		{
			// See if they have the key
			if (player.inventory.Contains("Key"))
			{
				// Tell the game manager to end the game
				GameManager.Instance.EndGame(true);
			} else
			{
				hintText.SetActive(true);
			}
		}
	}

	// When something leaves the door
	private void OnTriggerExit2D(Collider2D collision)
	{
		// Make sure its the player
		if (collision.GetComponent<PlayerController>())
		{
			hintText.SetActive(false);
		}
	}
}
