using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class ChunkGrid : MonoBehaviour
    {
        [SerializeField, Tooltip("Draw gizmos that shows the grid id true")]
        private bool _drawGizmos = true;

        [SerializeField, Tooltip("This tells how many cells are in the grid")]
        private Vector2Int _size = new Vector2Int(2, 2);

        [SerializeField, Tooltip("This tells how large the cells are in unity units")]
        private Vector2 _cellSize = new Vector2Int(1, 1);

        [SerializeField, Tooltip("This tells how big the gap between cells are in unity units")]
        private Vector2 _cellGap = new Vector2(0, 0);

        //This is the actual grid
        public ChunkHolder[,] Grid { get; set; }

        public Vector2Int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                UpdateGrid();
            }
        }

        /// <summary>
        /// Call this to place a chunk holder in the grid, will return false if the postion isn't 
        /// valid or the position already contains a chunk holder
        /// </summary>
        /// <param name="chunk">The chunk holder</param>
        /// <param name="position">The position in the grid, will only place if the 
        /// position is valid</param>
        /// <returns></returns>
        public virtual bool PlaceChunk(ChunkHolder chunk, Vector2Int position)
        {
            //This will instantiate the grid if the grid isn't instantiaded
            if(Grid == null)
                Grid = new ChunkHolder[Size.x,Size.y];

            //This will check if the postion is in the grid
            if (Grid.GetLength(0) > position.x && Grid.GetLength(1) > position.y)
            {
                //If the position dosen't already have a chunk holder
                if (Grid[position.x, position.y].Instance != null)
                {
                    Grid[position.x, position.y] = chunk;
                    UpdateGrid();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Call this to update the grid
        /// </summary>
        /// <param name="placeGrid">If this is true, then the grid will be placed in the world</param>
        public virtual void UpdateGrid(bool placeGrid = false)
        {
            //This will instantiate the grid if the grid isn't instantiaded
            if (Grid == null)
                Grid = new ChunkHolder[Size.x, Size.y];

            //This saves the current grid, so when the new grid is made, then the data can be put
            //in the new grid
            ChunkHolder[,] _oldGridData = (ChunkHolder[,])Grid.Clone();

            //This Creates the new grid and inserts the old data
            Grid = new ChunkHolder[Size.x, Size.y];
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Grid[x, y] = new ChunkHolder();
                }
            }

            for (int x = 0; x < Size.x; x++)
            {
                //If the old data's row lenght is less than the new grid, then break
                if (_oldGridData.GetLength(0) <= x)
                    break;

                for (int y = 0; y < Size.y; y++)
                {
                    //If the old data's column lenght is less than the new grid, then break
                    if (_oldGridData.GetLength(1) <= y)
                        break;

                    //This inserts the the old data and removes the data from the old array
                    if (_oldGridData[x, y] != null)
                    {
                        Grid[x, y] = _oldGridData[x, y];
                        _oldGridData[x, y] = null;
                    }
                }
            }

            //If the new grid is smaller than the old grid, then this will destroy the object that
            //there wasn't place for.
            foreach (var o in _oldGridData)
            {
                if (o != null)
                    Destroy(o.Instance);
            }

            //This will place the chunks in unity
            if(placeGrid)
                PlaceGrid();
        }


        /// <summary>
        /// This will instantiate the chunks that aren't instantiated and place them in a grid
        /// </summary>
        protected virtual void PlaceGrid()
        {
            float xOffset = Size.x * (_cellGap.x + _cellSize.x) / 2 - _cellSize.x / 2;
            float yOffset = Size.y * (_cellGap.y + _cellSize.y) / 2 - _cellSize.y / 2;

            //This will go through the grid 
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    //Will place the chunk if the cells contrains a chunk holder
                    if (Grid[x, y] != default(ChunkHolder))
                    {
                        //Here is the world position calculated
                        float xPosition = (_cellGap.x + _cellSize.x) * x;
                        float yPosition = (_cellGap.y + _cellSize.y) * y;

                        Vector2 point = new Vector2(transform.position.x + xPosition - xOffset,
                            transform.position.y + yPosition - yOffset);

                        //Instantiate the chunk if it isn't and place it correctly if it is.
                        if (!Grid[x, y].Instance && Grid[x, y].Prefab)
                            Grid[x, y].Instantiate(point, transform);
                        else if (Grid[x, y].Instance)
                            Grid[x, y].Instance.gameObject.transform.position = point;
                    }
                }
            }
        }

        /// <summary>
        /// This draws the grid
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if(!_drawGizmos)
                return;

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    float xPosition = (_cellGap.x + _cellSize.x) * x;
                    float yPosition = (_cellGap.y + _cellSize.y) * y;

                    float xOffset = Size.x * (_cellGap.x + _cellSize.x) / 2 - _cellSize.x / 2;
                    float yOffset = Size.y * (_cellGap.y + _cellSize.y) / 2 - _cellSize.y / 2;

                    Vector2 point = new Vector2(
                        transform.position.x + xPosition - xOffset, 
                        transform.position.y + yPosition - yOffset);

                    Gizmos.DrawWireCube(point, _cellSize);
                }
            }
        }
    }

}

