using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace RPG_Game
{
    [Serializable()]
    public class Tile
    {
        public string texture;
        public Vector2 textureDimensions;

        public Action testAction;

        public Vector2 currentFrame;

        public bool interactable;
        public bool walkable;

        public List<TileParts> tiles;

        public List<string> lines;
        public string eventLine;

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
            lines = new List<string>();
            eventLine = "";

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
            lines = new List<string>();
            eventLine = "";

            texture = tileTexture;
            currentFrame = tileFrame;
            textureDimensions = tileTextureDimensions;
        }
    }
}
