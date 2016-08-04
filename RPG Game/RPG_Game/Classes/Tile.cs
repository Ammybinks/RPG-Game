using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace RPG_Game
{
    [Serializable()]
    public class Tile
    {
        public string texture;
        public Vector2 textureDimensions;

        public Event currentEvent;
        public Action<NaviState, GameTime> eventAction;

        [NonSerialized()]public Mover occupier;

        public Vector2 currentFrame;

        public bool interactable;
        public bool occupied;
        public bool walkable;

        public List<TileParts> tiles;
        
        public Tile()
        {
            interactable = false;
            walkable = false;
        }
        public Tile(string tileTexture, Vector2 tileFrame, Vector2 tileTextureDimensions)
        {
            interactable = false;
            walkable = false;
            tiles = new List<TileParts>();

            texture = tileTexture;
            currentFrame = tileFrame;
            textureDimensions = tileTextureDimensions;
        }
    }

    [Serializable()]
    public class TileParts : Tile
    {
        public bool above;

        public TileParts()
        {
            interactable = false;
            walkable = false;
        }
        public TileParts(string tileTexture, Vector2 tileFrame, Vector2 tileTextureDimensions)
        {
            interactable = false;
            walkable = false;
            tiles = new List<TileParts>();

            texture = tileTexture;
            currentFrame = tileFrame;
            textureDimensions = tileTextureDimensions;
        }
    }
}
