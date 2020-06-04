using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static bool Playing { get; private set; }
    public static GameManager Instance { get; private set; }

    public GameMode gameMode;
    public PlayerController player;
    public Tilemap tilemap;
    public GameObject winScreen;

    public CaveGenerator caveGenerator;
    public EntityGenerator entityGenerator;
    public MapFog mapFog;

#if UNITY_EDITOR
    [Header("Debug Tools")]
    public bool newGame = false;
    public uint createWithSeed;
    public bool newGameWithSeed = false;
#endif

	// Setup singleton instance
	private void Awake()
	{
        Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        StartGame();
    }

// Update is only used for testing within the editor
#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        // Used for debug within Unity
        if (newGame)
        {
            StartGame();
            newGame = false;
        }

        if (newGameWithSeed)
        {
            StartGame();
        }
    }
#endif

    // Starts a game
    private void StartGame()
    {
        // Reset the game
        entityGenerator.ResetEntities();
        player.Reset();

        // Seed the RNG
        // Although Unity seeds it automatically, I want to use a known value
        // So that it can be logged and therefore replicated
        uint seed = (uint)System.DateTime.Now.Ticks;

#if UNITY_EDITOR
        // Used for debugging within editor
        if (newGameWithSeed)
        {
            newGameWithSeed = false;
            // Get the seed from the inspector
            seed = createWithSeed;
        }

        // Output the seed to inspector
        createWithSeed = seed;
#endif

        Debug.LogFormat("Creating level. Seed: {0}. Mode: {1}", seed, gameMode.name);
        Random.InitState((int)seed);

        caveGenerator.GenerateCave();

        // Pick a starting position for the player
        Vector2Int position = caveGenerator.AllocateRandomTile(caveTile.playerStart);
        player.transform.position = tilemap.GetCellCenterWorld((Vector3Int)position);

        // Generate all the entities
        entityGenerator.GenerateEntities(gameMode.entities);

        // Initialise the map
        mapFog.Init();

        Playing = true;
    }

    // Ends a game
    public void EndGame(bool win)
	{
        Playing = false;

        if (win)
		{
            // Show the win screen
            winScreen.SetActive(true);
		}
	}

    // Play again button pressed from an end screen
    public void PlayAgain()
	{
        // Close any end screens
        winScreen.SetActive(false);

        StartGame();
	}

    // Menu button pressed from end screen
    public void GoToMenu()
	{
        SceneManager.LoadScene(0);
	}
}
