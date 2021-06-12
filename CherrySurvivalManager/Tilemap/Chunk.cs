using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace CherrySurvivalManager.Tilemap {
    class Chunk : Transformable, Drawable {
        private VertexArray _vertices;                  // Holds the position and texture information of all tiles to be drawn
        private Vector2f[][] _texCoords;                // Buffer for the texture information for all layers
        private Texture _tileset;                       // Tileset texture
        private Tuple<int, int> _worldPos;              // Chunk position in the world space
        private Tilemap _map;                           // Reference to the tilemap class
        private int[][] _tiles;                         // Tile information for all layers
        private RenderTexture[] _textureBuffer;         // Buffer texture to render all layers on it
        private RenderTexture _textureBufferInvisible;  // Buffer texture to display last known instance of chunk in case it has become invisible
        private bool _needsRenderUpdate;                // Flag for updating the chunk
        private bool _active;                           // If a chunk is inactive it won't be drawn on the tilemap at all
        private bool _visible;                          // If a chunk isn't visible the last instance of it being seen will be drawn
        private Sprite _renderSprite;

        public Chunk(Tilemap map, Tuple<int, int> pos) {
            _map = map;
            _active = true;
            _visible = true;
            _worldPos = pos;
            _tileset = map.Tileset;
            int chunkSize = _map.ChunkSize;
            int tileSize = _map.TileSize;
            int numLayers = _map.NumLayers;
            _needsRenderUpdate = true;
            _textureBuffer = new RenderTexture[numLayers];
            _textureBufferInvisible = new RenderTexture((uint)(chunkSize * tileSize), (uint)(chunkSize * tileSize));

            for (int i = 0; i < numLayers; i++) {
                _textureBuffer[i] = new RenderTexture((uint)(chunkSize * tileSize), (uint)(chunkSize * tileSize));
            }

            // Reserve memory for all tiles on all layers on the tilemap
            _tiles = new int[_map.NumLayers][];          
            for (int i = 0; i < numLayers; i++) {
                _tiles[i] = new int[chunkSize * chunkSize];
                for (int j = 0; j < chunkSize * chunkSize; j++) {
                    _tiles[i][j] = -1;
                }
            }

            // Reserve memory for the texture of all tiles on all layers
            _texCoords = new Vector2f[_map.NumLayers][];
            for (int i = 0; i < _map.NumLayers; i++) {
                _texCoords[i] = new Vector2f[chunkSize * chunkSize * 4];
            }

            _vertices = new VertexArray();
            _vertices.PrimitiveType = PrimitiveType.Quads;
            _vertices.Resize((uint)(chunkSize * chunkSize * 4));
            
            for (int i = 0; i < chunkSize; i++) {
                for (int j = 0; j < chunkSize; j++) {

                    int tileNumber = _tiles[0][i + j * chunkSize];
                    int tu;
                    int tv;

                    // A negative tile numbers indicates an empty tile
                    if (tileNumber < 0) {
                        tu = 0;
                        tv = 0;
                    } else {
                        tu = tileNumber % ((int)_tileset.Size.X / tileSize);
                        tv = tileNumber / ((int)_tileset.Size.X / tileSize);
                    }

                    uint idx = (uint)(i + j * chunkSize) * 4;
                    Vertex q1 = _vertices[idx];
                    Vertex q2 = _vertices[idx + 1];
                    Vertex q3 = _vertices[idx + 2];
                    Vertex q4 = _vertices[idx + 3];



                    // Set position for tiles on world space
                    q1.Position = new Vector2f(i * tileSize, j * tileSize);
                    q2.Position = new Vector2f((i + 1) * tileSize, j * tileSize);
                    q3.Position = new Vector2f((i + 1) * tileSize, (j + 1) * tileSize);
                    q4.Position = new Vector2f(i * tileSize , (j + 1) * tileSize);

                    // Set texture coordinates for the first layer
                    q1.TexCoords = new Vector2f(tu * tileSize, tv * tileSize);
                    q2.TexCoords = new Vector2f((tu + 1) * tileSize, tv * tileSize);
                    q3.TexCoords = new Vector2f((tu + 1) * tileSize, (tv + 1) * tileSize);
                    q4.TexCoords = new Vector2f(tu * tileSize, (tv + 1) * tileSize);

                    _vertices[idx] = q1;
                    _vertices[idx + 1] = q2;
                    _vertices[idx + 2] = q3;
                    _vertices[idx + 3] = q4;             
                }
            }

            // Set tile textures for all layers in a separate array
            //
            for (int k = 0; k < _map.NumLayers; k++) {
                for (int i = 0; i < chunkSize; i++) {
                    for (int j = 0; j < chunkSize; j++) {
                        int tileNumber = _tiles[k][i + j * chunkSize];
                        int tu;
                        int tv;

                        // A negative tile numbers indicates an empty tile
                        if (tileNumber < 0) {
                            tu = 0;
                            tv = 0;
                        } else {
                            tu = tileNumber % ((int)_tileset.Size.X / tileSize);
                            tv = tileNumber / ((int)_tileset.Size.X / tileSize);
                        }

                        uint idx = (uint)(i + j * chunkSize) * 4;
                        _texCoords[k][idx] = new Vector2f(tu * tileSize, tv * tileSize);
                        _texCoords[k][idx + 1] = new Vector2f((tu + 1) * tileSize, tv * tileSize);
                        _texCoords[k][idx + 2] = new Vector2f((tu + 1) * tileSize, (tv + 1) * tileSize);
                        _texCoords[k][idx + 3] = new Vector2f(tu * tileSize, (tv + 1) * tileSize);
                    }
                }
            }
        }

        protected void RecalculateTextureCoordinates(int layer) {
            int chunkSize = _map.ChunkSize;
            int tileSize = _map.TileSize;

            for (int i = 0; i < chunkSize; i++) {
                for (int j = 0; j < chunkSize; j++) {
                    int tileNumber = _tiles[layer][i + j * chunkSize];
                    int tu;
                    int tv;

                    // A negative tile numbers indicates an empty tile
                    if (tileNumber < 0) {
                        tu = 0;
                        tv = 0;
                    } else {
                        tu = tileNumber % ((int)_tileset.Size.X / tileSize);
                        tv = tileNumber / ((int)_tileset.Size.X / tileSize);
                    }

                    uint idx = (uint)(i + j * chunkSize) * 4;
                    _texCoords[layer][idx] = new Vector2f(tu * tileSize, tv * tileSize);
                    _texCoords[layer][idx + 1] = new Vector2f((tu + 1) * tileSize, tv * tileSize);
                    _texCoords[layer][idx + 2] = new Vector2f((tu + 1) * tileSize, (tv + 1) * tileSize);
                    _texCoords[layer][idx + 3] = new Vector2f(tu * tileSize, (tv + 1) * tileSize);
                }
            }
            
        }

        public bool IsActive { get => _active; set => _active = value; }
        public bool IsVisible { get => _visible; set => _visible = value; }

        public void SetTiles(int layer, int[] tiles) {
            _tiles[layer] = tiles;
            RecalculateTextureCoordinates(layer);
            _needsRenderUpdate = true;
        }

        public int[] GetTiles(int layer) {
            return _tiles[layer];
        }

        public void Draw(RenderTarget target, RenderStates states) {
            int chunkSize = _map.ChunkSize;
            int tileSize = _map.TileSize;

            // Apparently something changed and the texture buffers need an update
            if (_needsRenderUpdate) {               
                states.Texture = _tileset;

                _textureBufferInvisible.Clear();

                // Go through all layers
                for (int k = 0; k < _map.NumLayers; k++) {
                    // Copy the texture info of all vertices for the current layer
                    for (int ij = 0; ij < _map.ChunkSize * _map.ChunkSize * 4; ij++) {
                        Vertex v = _vertices[(uint)ij];
                        v.TexCoords = _texCoords[k][ij];
                        _vertices[(uint)ij] = v;
                    }

                    // Draw layer to texture buffer
                    _textureBuffer[k].Clear(Color.Transparent);
                    _textureBuffer[k].Draw(_vertices, states);
                    _textureBuffer[k].Display();

                    _textureBufferInvisible.Draw(_vertices, states);
                }

                _textureBufferInvisible.Display();
                _needsRenderUpdate = false;
            }

            // Apply transforms now since it's supposed to affect all layers the same way
            states.Transform *= Transform;
            int offsetX = _worldPos.Item1 * chunkSize * tileSize;
            int offsetY = _worldPos.Item2 * chunkSize * tileSize;

            if (_visible) {
                // Draw all layers
                for (int i = 0; i < _map.NumLayers; i++) {
                    Sprite sprite = new Sprite(_textureBuffer[i].Texture);
                    sprite.Position = new Vector2f(offsetX, offsetY);
                    target.Draw(sprite, states);
                    sprite.Dispose();
                }
            } else {
                Sprite sprite = new Sprite(_textureBufferInvisible.Texture);
                sprite.Position = new Vector2f(offsetX, offsetY);
                target.Draw(sprite, states);
                sprite.Dispose();
            }
        }
    }
}
