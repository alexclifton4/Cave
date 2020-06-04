using UnityEngine;

public class CheatMenu : MonoBehaviour
{
	public bool cheatsOpened = false;
	public PlayerController player;
	public EntityDescription key;
	public GameObject fog;

	private bool showCheats = false;

	// Draws the menu
	private void OnGUI()
	{
		// See if the menu should be shown
		if (showCheats)
		{
			// Show cheat menu
			if (GUI.Button(new Rect(10, 10, 50, 30), "Close"))
			{
				showCheats = false;
			}
			if (GUI.Button(new Rect(10, 50, 80, 30), "Add key"))
			{
				player.inventory.Add(key);
			}
			if (GUI.Button(new Rect(10, 90, 80, 30), "Clear fog"))
			{
				fog.SetActive(false);
			}
			if (GUI.Button(new Rect(10, 130, 80, 30), "Win game"))
			{
				GameManager.Instance.EndGame(true);
			}
		} else
		{
			// Cheat menu not showing, show button
			if (GUI.Button(new Rect(10, 10, 50, 30), "Cheats"))
			{
				showCheats = true;
				if (!cheatsOpened)
				{
					cheatsOpened = true;
					print("Cheat menu opened");
				}
			}
		}
	}
}
