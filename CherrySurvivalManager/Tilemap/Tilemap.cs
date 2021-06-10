using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CherrySurvivalManager.Tilemap {
    class Tilemap {
        private Dictionary<Tuple<int, int>, Chunk> _chunks;
        private Texture _tileset;
        private int _chunkSize;
        private int _tileSize;
        private int _numLayers;

        public int ChunkSize { get => _chunkSize; private set => _chunkSize = value; }
        public int TileSize { get => _tileSize; private set => _tileSize = value; }
        public int NumLayers { get => _numLayers; private set => _numLayers = value; }

        public Chunk GetChunk(int x, int y) {
            return _chunks[new Tuple<int, int>(x, y)];
        }

        public Chunk GetChunk(Tuple<int, int> pos) {
            return _chunks[pos];
        }
    }
}
