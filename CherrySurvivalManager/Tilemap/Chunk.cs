using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CherrySurvivalManager.Tilemap {
    class Chunk {
        private VertexArray _vertices;
        private Texture _tileset;
        private Tuple<int, int> _worldPos;
        private Tilemap _map;
        private int[][] _tiles;

        public Chunk(Tilemap map, Tuple<int, int> pos, Texture tileset) {
            
            // Reserve memory for all tiles on all layers
            _tiles = new int[_map.NumLayers][];
            for (int i = 0; i < _map.ChunkSize; i++) {
                _tiles[i] = new int[_map.ChunkSize];
            }


        }
    }
}
