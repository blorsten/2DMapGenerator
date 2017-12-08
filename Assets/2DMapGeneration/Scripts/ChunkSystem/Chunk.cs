using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MapGeneration.TileSystem;
using MapGeneration.Utils;
using UnityEditor;
using UnityEditor.Callbacks;


namespace MapGeneration.ChunkSystem
{
    /// <summary>
    /// This enum is used to determine the chunks type
    /// </summary>
    public enum ChunkType
    {
        Default, DeadEnd, Reward, Secret, Start, End, Solid
    }

    /// <summary>
    /// This calss stores the chunk's data.
    /// </summary>
    [ExecuteInEditMode]
    [SelectionBase]
    public class Chunk : MonoBehaviour
    {
        //Paths to the gizmo icons.
        private const string TRAP_ICON_PATH = "Trap.png";
        private const string TREASURE_ICON_PATH = "Treasure.png";
        private const string GROUND_ICON_PATH = "Ground.png";
        private const string FLYING_ICON_PATH = "Flying.png";

        //Grid filled with the values used by biome algorithms.
        [SerializeField, HideInInspector] private Float2DArray _biomeValues;

        [SerializeField, HideInInspector] private ConditionalChunk _conditionalChunk;

        //This sections is for generel properties 
        //Tells the width of the chunk
        [Header("Properties")]
        [SerializeField] private int _width; 

        //Tells the height of the chunk
        [SerializeField] private int _height;

        //Tells the type of the chunk
        [SerializeField] private ChunkType _chunkType; 

        //These fields tells what openings are open on the chunk 
        [Header("Openings"), SerializeField] private ChunkOpenings _chunkOpenings;

        //This is a list of TileFlags in the chunk
        [SerializeField, ReadOnly] private List<TileFlag> _openings = new List<TileFlag>();

        [SerializeField, ReadOnly] private List<TileFlag> _tileTileFlags = new List<TileFlag>();

        //This section is for refernces
        [Header("References"),SerializeField] private Tilemap _environment;
        [SerializeField, HideInInspector] private ChunkHolder _chunkHolder;

        [Header("Draw Booleans"), SerializeField] private bool _drawOpenings = true;
        [SerializeField] private bool _drawEdges = true;
        [SerializeField] private bool _drawTileFlags = true;

        [SerializeField, Tooltip("Is this chunk dependendant on some other chunk?")]
        private bool _isStandaloneChunk = true;

        [SerializeField, HideInInspector] private Chunk _recipeReference;

        //Properties for generel properties
        /// <summary>
        /// This stores the chunks width in tiles
        /// </summary>
        public int Width { get { return _width; } set { _width = value; } }
        /// <summary>
        /// This stores the chunks heigth in tiles
        /// </summary>
        public int Height { get { return _height; } set { _height = value; } }
        /// <summary>
        /// This stores the chunk's type
        /// </summary>
        public ChunkType ChunkType { get { return _chunkType; } set { _chunkType = value; } }
        /// <summary>
        /// This double array is to determine what biome every tile are in.
        /// </summary>
        public Float2DArray BiomeValues { get { return _biomeValues; } set { _biomeValues = value; } }

        /// <summary>
        /// This is a reference to the map the chunks is in.
        /// </summary>
        public Map Map { get; set; }
        /// <summary>
        /// A ID to indentify the Chunk
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// A list for the items in the chunk
        /// </summary>
        public List<GameObject> Items { get; set; } 

        //Reference to the object that this chunk was created from.
        public Chunk RecipeReference { get { return _recipeReference; } set { _recipeReference = value; } }

        /// <summary>
        /// This list stores the chunks openings.
        /// </summary>
        public List<TileFlag> Openings{ get { return _openings; } set { _openings = value; } }

        /// <summary>
        /// This list stores TileFlag. 
        /// </summary>
        public List<TileFlag> TileFlags{ get { return _tileTileFlags; } set { _tileTileFlags = value; } }

        /// <summary>
        /// This class is used to determine what sides has openings.
        /// </summary>
        public ChunkOpenings ChunkOpenings { get { return _chunkOpenings; } set { _chunkOpenings = value; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsStandaloneChunk { get { return _isStandaloneChunk; } set { _isStandaloneChunk = value; } }

        /// <summary>
        /// This is used to get the gameobject's conditionalChunk components if it has any.
        /// </summary>
        public ConditionalChunk ConditionalChunk { get { return _conditionalChunk ?? (_conditionalChunk = GetComponent<ConditionalChunk>()); } }

        /// <summary>
        /// The chunks environment, a environment is the main tilemap.
        /// </summary>
        public Tilemap Environment
        {
            get
            {
                if (!_environment)
                {
                    if (RecipeReference)
                        Debug.LogWarning(string.Format("{0} needs an environment reference.", RecipeReference.name), RecipeReference.gameObject);
                    else
                        Debug.LogWarning(string.Format("{0} needs an environment reference.", name), this);
                }

                return _environment; 
            }
            set { _environment = value; }
        }

        /// <summary>
        /// This is a reference to the chunks ChunkHolder
        /// </summary>
        public ChunkHolder ChunkHolder
        {
            get { return _chunkHolder; }
            set { _chunkHolder = value; }
        }


        /// <summary>
        /// When this components gets added or gets reset, grab all references if possible.
        /// </summary>
        void Reset()
        {
            _environment = GetComponentInChildren<Tilemap>();

        }

        /// <summary>
        /// Instantiate needed collections such as biome values.
        /// </summary>
        private void Awake()
        {
            _biomeValues = new Float2DArray(_width, _height); 
        }

        /// <summary>
        /// This draws the chunks gizmos like where the chunks openings are located.
        /// </summary>
        public void OnDrawGizmos()
        {
            //This draws connections
            if (_drawOpenings && Environment)
            {
                Gizmos.color = new Color(231f / 255f, 76f / 255f, 60f / 255f);

                foreach (var c in Openings)
                {
                    Vector3 cellPosition = Environment.GetCellCenterWorld(c.Position);
                    Vector2 cellSize = Environment.cellSize;

                    Vector3 top = new Vector3(cellPosition.x, cellPosition.y + cellSize.y / 2);
                    Vector3 bottom = new Vector3(cellPosition.x, cellPosition.y - cellSize.y / 2);
                    Vector3 right = new Vector3(cellPosition.x + cellSize.x / 2, cellPosition.y);
                    Vector3 left = new Vector3(cellPosition.x - cellSize.x / 2, cellPosition.y);

                    switch (c.Type)
                    {
                        case FlagType.Top:
                            GizmoUtilities.DrawArrow(top, ArrowDirection.Up);
                            break;
                        case FlagType.Bottom:
                            GizmoUtilities.DrawArrow(bottom, ArrowDirection.Down);
                            break;
                        case FlagType.Left:
                            GizmoUtilities.DrawArrow(left, ArrowDirection.Left);
                            break;
                        case FlagType.Right:
                            GizmoUtilities.DrawArrow(right, ArrowDirection.Right);
                            break;
                    }
                }
            }

            //This draws tile flags
            if (_drawTileFlags && Environment)
            {
                foreach (var flag in TileFlags)
                {
                    Vector3 postition = Environment.GetCellCenterWorld(flag.Position);

                    switch (flag.Type)
                    {
                        case FlagType.Trap:
                            UnityEngine.Gizmos.DrawIcon(postition, TRAP_ICON_PATH, true);
                            break;
                        case FlagType.Treasure:
                            UnityEngine.Gizmos.DrawIcon(postition, TREASURE_ICON_PATH, true);
                            break;
                        case FlagType.FlyingSpawn:
                            UnityEngine.Gizmos.DrawIcon(postition, FLYING_ICON_PATH, true);
                            break;
                        case FlagType.GroundSpawn:
                            UnityEngine.Gizmos.DrawIcon(postition, GROUND_ICON_PATH, true);
                            break;

                    }
                }
            }
            //This draws edges
            if (_drawEdges && Environment)
            {
                UnityEngine.Gizmos.color = Color.white;
                Vector2 gridSize = new Vector2(Width, Height);
                Vector2 cellSize = Environment.cellSize;

                float yMin = transform.position.y;
                float yMax = transform.position.y + gridSize.y;
                float xMin = transform.position.x;
                float xMax = transform.position.x + gridSize.x;

                UnityEngine.Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMin, yMax));
                UnityEngine.Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
                UnityEngine.Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin));
                UnityEngine.Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMax, yMax));
            }

        }

        /// <summary>
        /// Goes through the chunks tilemap(s) and refreshes all tiles.
        /// </summary>
        public void RefreshTilemaps()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Tilemap tilemap = transform.GetChild(i).gameObject.GetComponent<Tilemap>();
                if(tilemap != null)
                    tilemap.RefreshAllTiles();
            }
        }

        /// <summary>
        /// When the game isnt playing make sure all the tiles are refreshed after compilation.
        /// </summary>
        void OnEnable()
        {
            if (!Application.isPlaying)
                RefreshTilemaps(); 
        }
    }
}
