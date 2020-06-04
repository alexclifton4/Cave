using UnityEngine;

public class CaveConnector
{
	public static Vector2Int mapSize;

	private enum ConnectedState { connected, unconnected, wall };
	
	private static ConnectedState[,] caveCopy;
	private static int airCount;
	private static int connectedCount;
	private static bool needToRepeat;

	// Connects all the separate caves in a cave
	public static void Connect(caveTile[,] cave)
	{
		// This may need to run more than once
		needToRepeat = true;
		while (needToRepeat)
		{
			// Create a copy of the cave
			airCount = 0;
			caveCopy = new ConnectedState[mapSize.x, mapSize.y];
			for (int x = 0; x < mapSize.x; x++)
			{
				for (int y = 0; y < mapSize.y; y++)
				{
					if (cave[x, y] == caveTile.wall)
					{
						caveCopy[x, y] = ConnectedState.wall;
					} else
					{
						// Mark all air as unconnected initially
						caveCopy[x, y] = ConnectedState.unconnected;
						airCount++;
					}
				}
			}

			// Find the first unconnected tile
			// All tiles are currently unconnected, so this is first air tile
			Vector2Int position = FindUnconnected();

			// Perform a flood fill on the cave
			connectedCount = 0;
			FloodFill(position.x, position.y);

			// There is now a connected area of the cave, and potentially unconnected areas
			// See if there is an unconnected area, and if so, get it
			position = FindUnconnected();
			if (position.x != -1)
			{
				// There is an unconnected area, so remove it
				RemoveUnconnected(cave);

				// There was at least one unconnected area, but there may be more
				needToRepeat = true;
			} else
			{
				// No unconnected areas
				needToRepeat = false;
			}
		}
	}

	// Returns coordinates of the first unconnected tile
	// Or (-1, -1) if none exist
	private static Vector2Int FindUnconnected()
	{
		Vector2Int pos = Vector2Int.one * -1;
		bool found = false;

		for (int x = 0; x < mapSize.x && !found; x++)
		{
			for (int y = 0; y < mapSize.y && !found; y++)
			{
				if (caveCopy[x, y] == ConnectedState.unconnected)
				{
					found = true;
					pos = new Vector2Int(x, y);
				}
			}
		}

		return pos;
	}

	// Performs a flood fill starting at startPos
	// Fills all 'unconnected' with 'connected'
	private static void FloodFill(int x, int y)
	{
		// If startPos is out of bounds, do nothing
		if (x < 0 || y < 0 || x >= mapSize.x || y >= mapSize.y)
		{
			return;
		}

		// If the tile is already connected, do nothing
		if (caveCopy[x, y] == ConnectedState.connected)
		{
			return;
		}

		// If its a wall tile, do nothing
		if (caveCopy[x, y] == ConnectedState.wall)
		{
			return;
		}

		// Change this tile and recurse on four neighbours
		caveCopy[x, y] = ConnectedState.connected;
		connectedCount++;
		FloodFill(x + 1, y);
		FloodFill(x - 1, y);
		FloodFill(x, y + 1);
		FloodFill(x, y - 1);
	}

	// Removes the unconnected area of a cave
	private static void RemoveUnconnected(caveTile[,] cave)
	{
		// Remove the smaller unconnected area
		ConnectedState tileToRemove;
		if (connectedCount < airCount / 2)
		{
			// Remove the tiles marked as connected
			tileToRemove = ConnectedState.connected;
		} else
		{
			// Remove tiles marked as unconnected
			tileToRemove = ConnectedState.unconnected;
		}

		// Loop through the cave copy
		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				// See if the tile needs to be removed
				if (caveCopy[x,y] == tileToRemove)
				{
					cave[x, y] = caveTile.wall;
				}
			}
		}
	}
}
