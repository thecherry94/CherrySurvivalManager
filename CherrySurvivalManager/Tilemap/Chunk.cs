using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace CherrySurvivalManager.Tilemap {
    class Chunk : Transformable, Drawable {
        private VertexArray _vertices;
        private Vector2f[] _texCoords;
        private Texture _tileset;
        private Tuple<int, int> _worldPos;
        private Tilemap _map;
        private int[][] _tiles;

        public Chunk(Tilemap map, Tuple<int, int> pos, Texture tileset) {
            int chunkSize = _map.ChunkSize;
            int tileSize = _map.TileSize;

            // Reserve memory for all tiles on all layers
            _tiles = new int[_map.NumLayers][];
            _texCoords = new Vector2f[_map.NumLayers];
            for (int i = 0; i < _map.ChunkSize; i++) {
                _tiles[i] = new int[_map.ChunkSize];
            }

            _vertices.PrimitiveType = PrimitiveType.Quads;
            _vertices.Resize((uint)(chunkSize * chunkSize * 4));
            
            for (int i = 0; i < chunkSize; i++) {
                for (int j = 0; j < chunkSize; j++) {
                    

                    Vertex q1 = _vertices[(uint)(i + j * chunkSize) * 4];
                    Vertex q2 = _vertices[(uint)(i + j * chunkSize) * 4 + 1];
                    Vertex q3 = _vertices[(uint)(i + j * chunkSize) * 4 + 2];
                    Vertex q4 = _vertices[(uint)(i + j * chunkSize) * 4 + 3];

                    q1.Position = new Vector2f(i * tileSize, j * tileSize);
                    q2.Position = new Vector2f((i + 1) * tileSize, j * tileSize);
                    q3.Position = new Vector2f((i + 1) * tileSize, (j + 1) * tileSize);
                    q4.Position = new Vector2f(i * tileSize, (j + 1) * tileSize);

                    for (int k = 0; k < _map.NumLayers; k++) {
                        int tileNumber = _tiles[k][i + j * chunkSize];
                        int tu = tileNumber % ((int)_tileset.Size.X / tileSize);
                    }
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states) {
            throw new NotImplementedException();
        }
    }
}
