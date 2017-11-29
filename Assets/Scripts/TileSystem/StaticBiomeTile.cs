using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    [CreateAssetMenu(fileName = "StaticBiomeTile", menuName = "MapGeneration/Tiles/StaticBiomeTile")]
    public class StaticBiomeTile : BiomeTile
    {

        [SerializeField]
        private List<StaticBiomeSprites> _biomesSprites= new List<StaticBiomeSprites>();

        private StaticBiomeSprites _currentSprites;

        protected override void RefreshBiomeValues(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshBiomeValues(position, tilemap);

            //if a chunk is found, then get the current biome id and use it to get sprites
            if (_chunk)
            {
                _currentSprites = _biomesSprites.Count > 0
                    ? _biomesSprites.FirstOrDefault(x => x.iD == _biome)
                    : null;
            }
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            RefreshBiomeValues(position, tilemap);
            if (_currentSprites != null && _currentSprites.iD != "")
            {
                tileData.sprite = _currentSprites.sprite;
                tilemap.GetComponent<Tilemap>().SetColor(position, _currentSprites.tint);
            }

        }
    }
}

