using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFog : MonoBehaviour
{
    public CaveGenerator cave;
    public Tile fogTile;
    public Tilemap fogTilemap;
    public Transform player;
    public Vector2Int uncoverSize;

    // Update is called once per frame
    void Update()
    {
        // Work out the corner to start uncovering from
        Vector2Int startPos = new Vector2Int((int)player.position.x - (uncoverSize.x / 2), (int)player.position.y - (uncoverSize.y / 2));
        
        // Uncover tiles
        for (int x = 0; x < uncoverSize.x; x++)
        {
            for (int y = 0; y < uncoverSize.y; y++)
            {
                fogTilemap.SetTile(new Vector3Int(startPos.x + x, startPos.y + y, 0), null);
            }
        }
    }

    // Resets the map
    public void Init()
    {
        // Make sure the GO is active
        fogTilemap.gameObject.SetActive(true);

        // Fill the map with fog
        for (int x = 0; x < cave.mapSize.x; x++)
        {
            for (int y = 0; y < cave.mapSize.y; y++)
            {
                fogTilemap.SetTile(new Vector3Int(x, y, 0), fogTile);
            }
        }
    }
}
