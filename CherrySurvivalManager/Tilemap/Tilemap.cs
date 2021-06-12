using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CherrySurvivalManager.Tilemap {
    class Tilemap : Transformable, Drawable {
        private Dictionary<Tuple<int, int>, Chunk> _chunks;
        private Texture _tileset;
        private int _chunkSize;
        private int _tileSize;
        private int _numLayers;

        public int ChunkSize { get => _chunkSize; private set => _chunkSize = value; }
        public int TileSize { get => _tileSize; private set => _tileSize = value; }
        public int NumLayers { get => _numLayers; private set => _numLayers = value; }

        public Tilemap(string tilesetPath, int chunkSize, int tileSize, int numLayers) {
            _chunkSize = chunkSize;
            _tileSize = tileSize;
            _numLayers = numLayers;
            _tileset = new Texture(tilesetPath);
        }

        public void AddChunk(Tuple<int, int> pos) {
            _chunks[pos] = new Chunk(this, pos);
        }

        public void AddChunk(int x, int y) {
            AddChunk(new Tuple<int, int>(x, y));
        }

        public Chunk GetChunk(int x, int y) {
            return GetChunk(new Tuple<int, int>(x, y));
        }

        public Chunk GetChunk(Tuple<int, int> pos) {
            if (_chunks.ContainsKey(pos))
                return _chunks[pos];
            else return null;
        }

        public void Draw(RenderTarget target, RenderStates states) {
            foreach(KeyValuePair<Tuple<int, int>, Chunk> pair in _chunks) {
                Chunk c = pair.Value;
                if (c.IsActive)
                    c.Draw()
            }
        }
    }
}
