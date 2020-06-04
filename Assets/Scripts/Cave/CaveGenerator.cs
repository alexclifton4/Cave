using UnityEngine;
using UnityEngine.Tilemaps;

public enum caveTile { air, wall, wallBack, playerStart, entity}

public class CaveGenerator : MonoBehaviour
{
    public Tilemap mainTilemap;
    public Tilemap mapTilemap;
    public Tile MapTile;
    public Tile FloorTile;
    public Tile[] WallTiles;
    public Tile WallBackTile;
    public Tile WallBackLeftTile;
    public Tile WallBackRightTile;

    public Vector2Int mapSize;
    public float initialAir;
    public int spawnLevel;
    public int despawnLevel;

    public caveTile[,] cave;

    public void GenerateCave()
    {
        cave = new caveTile[mapSize.x, mapSize.y];

        // Loop through and create initial random tiles
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                cave[x, y] = Random.value < initialAir ? caveTile.air : caveTile.wall; ;
            }
        }

        // Run the simulation until no changes are made
        while (RunSimulation());

        // Make sure the cave is connected
        CaveConnector.mapSize = mapSize;
        CaveConnector.Connect(cave);

        // Set the tiles on the tilemap
        SetTiles();
    }

    // Allocates a random empty tile in the cave and returns it
    public Vector2Int AllocateRandomTile(caveTile tile)
    {
        bool valid = false;
        Vector2Int position = Vector2Int.zero;
        while (!valid)
        {
            position.Set(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y));
            if (cave[position.x, position.y] == caveTile.air)
            {
                valid = true;
            }
        }

        // Allocate the tile and return the position
        cave[position.x, position.y] = tile;
        return position;
    }

    // Runs a single step of the simulation
    private bool RunSimulation()
    {
        bool tileChanged = false;

        // Create a temporary cave
        caveTile[,] tempCave = (caveTile[,]) cave.Clone();

        // Loop through every tile in the cell
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                // Count the neighbours
                int neighbours = CountNeighbours(x, y);

                // Change depending on the rules
                caveTile tile = cave[x, y];
                if (tile == caveTile.air && neighbours > spawnLevel)
                {
                    // Spawn a wall tile
                    tempCave[x, y] = caveTile.wall;
                    tileChanged = true;
                } else if (tile == caveTile.wall && neighbours < despawnLevel)
                {
                    // Despawn the wall tile
                    tempCave[x, y] = caveTile.air;
                    tileChanged = true;
                }
            }
        }

        // Set the cave to the temp cave
        cave = tempCave;

        return tileChanged;
    }

    // Returns how many neighbours a cell has
    private int CountNeighbours(int x, int y)
    {
        int count = 0;

        // Defines the 4 directions to check in
        int[,] directions = new int[,] { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 } };
        // Loop through each direction
        for (int i = 0; i < 8; i++)
        {
            Vector2Int toCheck = new Vector2Int(x + directions[i, 0], y + directions[i, 1]);
            // Check its outside the range of the map
            if (toCheck.x < 0 || toCheck.x >= mapSize.x || toCheck.y < 0 || toCheck.y >= mapSize.y)
            {
                // Count as a neighbour
                count++;
            }
            // Check the tile
            else if (cave[toCheck.x, toCheck.y] == caveTile.wall)
            {
                count++;
            }
        }

        return count;
    }

    // Returns an array of which neighbours a cell has
    // Starting at top left and working around clockwise
    private bool[] GetNeighbours(int x, int y)
	{
        bool[] neighbours = new bool[8];
        // Check neighbours, but mark as true if the neghbour would be out of bounds
        neighbours[0] =             x == 0 || y == mapSize.y - 1 || cave[x - 1, y + 1] == caveTile.wall;
        neighbours[1] =                       y == mapSize.y - 1 || cave[x, y + 1] == caveTile.wall;
        neighbours[2] = x == mapSize.x - 1 || y == mapSize.y - 1 || cave[x + 1, y + 1] == caveTile.wall;
        neighbours[3] = x == mapSize.x - 1 ||                       cave[x + 1, y] == caveTile.wall;
        neighbours[4] = x == mapSize.x - 1 ||             y == 0 || cave[x + 1, y - 1] == caveTile.wall;
        neighbours[5] =                                   y == 0 || cave[x, y - 1] == caveTile.wall;
        neighbours[6] =             x == 0 ||             y == 0 || cave[x - 1, y - 1] == caveTile.wall;
        neighbours[7] =             x == 0 ||                       cave[x - 1, y] == caveTile.wall;

        return neighbours;
	}

    // Post-processes the tiles and sets them on the tilemap
    private void SetTiles()
    {
        // Post-process the tiles
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                // Get neighbours
                bool[] neighbours = GetNeighbours(x, y);

                // 1) Edge tiles should be walls
                if (x == 0 || x == mapSize.x - 1 || y == 0 || y == mapSize.y - 1)
                {
                    cave[x, y] = caveTile.wall;
                }

                // 2) Walls with nothing infront or behind should be removed
                else if ((cave[x, y] == caveTile.wall) && !neighbours[1] && !neighbours[5])
                {
                    cave[x, y] = caveTile.air;
                }

                // 3) Walls with nothing to the left or right should be removed
                else if ((cave[x, y] == caveTile.wall) && !neighbours[3] && !neighbours[7])
                {
                    cave[x, y] = caveTile.air;
                }
            }
        }

        for (int x = 0; x < mapSize.x; x++)
		{
            for (int y = 0; y < mapSize.y; y++)
			{
                // 4) Walls with air in front become wallBack
                if (cave[x,y] == caveTile.wall && y != 0 && cave[x, y - 1] == caveTile.air)
				{
                    cave[x, y] = caveTile.wallBack;
				}
            }
        }

        // Work out which tile to set and set it
        for (int x = 0; x < mapSize.x; x++)
		{
            for (int y = 0; y < mapSize.y; y++)
			{
                // Work out which tile to set
                Tile tileToSet = WhichTile(x, y);
                Tile tileToSetOnMap = cave[x, y] == caveTile.wall ? MapTile : null;

                // Set the tile
                Vector3Int pos = new Vector3Int(x, y, 0);
                mainTilemap.SetTile(pos, tileToSet);
                mapTilemap.SetTile(pos, tileToSetOnMap);
            }
		}
    }

    // Works out which tile to set for a given x y coordinate
    private Tile WhichTile(int x, int y)
    {
        Tile tile;

        // Check if its a wall or floor tile
        if (cave[x, y] == caveTile.wall)
        {
            // Its a wall
            tile = WallTiles[0];

            bool[] neighbours = GetNeighbours(x, y);
            // Choose correct tile
            // Corner tiles
            if (!neighbours[0] && !neighbours[1] && !neighbours[7])
            {
                tile = WallTiles[1];
            } else if (!neighbours[1] && !neighbours[2] && !neighbours[3])
			{
                tile = WallTiles[3];
			} else if(!neighbours[3] && !neighbours[4] && !neighbours[5])
			{
                tile = WallTiles[5];
			} else if(!neighbours[5] && !neighbours[6] && !neighbours[7])
			{
                tile = WallTiles[7];
			}
            // Edge tiles
            else if (!neighbours[1])
			{
                tile = WallTiles[2];
			} else if (!neighbours[3])
			{
                tile = WallTiles[4];
			} else if (!neighbours[5])
			{
                tile = WallTiles[6];
			} else if (!neighbours[7])
			{
                tile = WallTiles[8];
			}
            // Inside corner tiles
            else if (!neighbours[4])
			{
                tile = WallTiles[9];
			} else if (!neighbours[6])
			{
                tile = WallTiles[10];
			} else if (!neighbours[0])
			{
                tile = WallTiles[11];
			} else if (!neighbours[2])
			{
                tile = WallTiles[12];
			}
        } else if (cave[x,y] == caveTile.wallBack)
		{
            // Its a back wall
            tile = WallBackTile;

            // Check if it should be a corner piece
            if (x != 0 && cave[x - 1, y] == caveTile.air)
			{
                tile = WallBackLeftTile;
			} else if (x != mapSize.x - 1 && cave[x+1, y] == caveTile.air)
			{
                tile = WallBackRightTile;
			}
		} else
        {
            // Its floor
            tile = FloorTile;
        }

        return tile;
    }
}
