using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG_Game
{
    public class NaviState : StateManager
    {
        public Texture2D heroMoverTexture;
        public Mover heroMover;

        public Texture2D hiroMoverTexture;
        public Mover hiroMover;

        public Texture2D hearoMoverTexture;
        public Mover hearoMover;

        public Texture2D hieroMoverTexture;
        public Mover hieroMover;

        public LinkedList<Mover> movers = new LinkedList<Mover>();

        public Camera camera;

        public Tile[,] map = new Tile[100, 50];
        public Tile tile;
        public Tile eventTile;

        public bool[] state = new bool[2];
        public int currentState;

        public Vector2 playerMovement;
        public Vector2 playerView = new Vector2(20, 11);

        public Event currentEvent;
        Area01 area01 = new Area01();

        public override void LoadContent(Main main)
        {
            calibri = main.Content.Load<SpriteFont>("Fonts\\Calibri");
            
            pointerTexture = main.Content.Load<Texture2D>("Misc\\Pointer");
            iconTexture = main.Content.Load<Texture2D>("Misc\\IconSet");

            cornerTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Corner");
            wallTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Wall Expandable");
            backTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Back");

            heroMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Adorable Manchild");
            hiroMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Absolutely-Not-Into-It Love Interest");
            hearoMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Endearing Father Figure");
            hieroMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Comic Relief");

            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);

            camera = new Camera();
            camera.WorldWidth = 3840;
            camera.WorldHeight = 2160;
            camera.ViewWidth = 1920;
            camera.ViewHeight = 1080;
            camera.UpperLeft = new Vector2(0, 0);
            //Miscellaneous Initialization Ends//

            heroMover = new Mover();
            heroMover.ContinuousAnimation = false;
            heroMover.AnimationInterval = 100;
            heroMover.SetTexture(heroMoverTexture, 3, 4);
            heroMover.Scale = new Vector2(1, 1);
            heroMover.gridPosition = new Vector2(49, 24);
            heroMover.UpperLeft = new Vector2(heroMover.gridPosition.X * 48, heroMover.gridPosition.Y * 48);
            movers.AddFirst(heroMover);

            hiroMover = new Mover();
            hiroMover.SetTexture(heroMoverTexture, 3, 4);
            hiroMover.UpperLeft = new Vector2(hiroMover.GetWidth(), hiroMover.GetHeight());
            movers.AddFirst(hiroMover);

            hearoMover = new Mover();
            hearoMover.SetTexture(heroMoverTexture, 3, 4);
            hearoMover.UpperLeft = new Vector2(hearoMover.GetWidth(), hearoMover.GetHeight());
            movers.AddFirst(hearoMover);

            hieroMover = new Mover();
            hieroMover.SetTexture(heroMoverTexture, 3, 4);
            hieroMover.UpperLeft = new Vector2(hieroMover.GetWidth(), hieroMover.GetHeight());
            movers.AddFirst(hieroMover);

            //World Map Initialization Begins//
            //Grass Base
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    tile = new Tile("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13));

                    tile.walkable = true;
                    tile.interactable = false;

                    map[i, o] = tile;
                }
            }

            //Stone Edges
            for (int i = 0; i < playerView.X; i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].currentFrame = new Vector2(1, 4);
                }
            }

            for (int i = map.GetLength(0) - (int)playerView.X; i < map.GetLength(0); i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].currentFrame = new Vector2(1, 4);
                }
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            {
                for (int o = 0; o < playerView.Y; o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].currentFrame = new Vector2(1, 4);
                }
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            {
                for (int o = map.GetLength(1) - (int)playerView.Y; o < map.GetLength(1); o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].currentFrame = new Vector2(1, 4);
                }
            }

            //Stone Edge Edges
            for (int o = (int)playerView.Y; o < map.GetLength(1) - playerView.Y; o++)
            {
                map[(int)playerView.X - 1, o].currentFrame = new Vector2(2, 4);
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            {
                map[i, (int)playerView.Y - 3].currentFrame = new Vector2(1, 5);
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - (int)playerView.X; i++)
            {
                map[i, map.GetLength(1) - (int)playerView.Y].currentFrame = new Vector2(1, 3);
            }

            for (int o = (int)playerView.Y; o < map.GetLength(1) - playerView.Y; o++)
            {
                map[map.GetLength(0) - (int)playerView.X, o].currentFrame = new Vector2(0, 4);
            }

            //Stone Walls
            for (int i = (int)playerView.X + 1; i < map.GetLength(0) - playerView.X - 1; i++)
            {
                map[i, (int)playerView.Y - 2].currentFrame = new Vector2(1, 6);
            }

            for (int i = (int)playerView.X + 1; i < map.GetLength(0) - playerView.X - 1; i++)
            {
                map[i, (int)playerView.Y - 1].currentFrame = new Vector2(1, 8);
            }

            map[(int)playerView.X, (int)playerView.Y - 2].currentFrame = new Vector2(0, 6);
            map[(int)playerView.X, (int)playerView.Y - 1].currentFrame = new Vector2(0, 8);

            map[map.GetLength(0) - (int)playerView.X - 1, (int)playerView.Y - 2].currentFrame = new Vector2(2, 6);
            map[map.GetLength(0) - (int)playerView.X - 1, (int)playerView.Y - 1].currentFrame = new Vector2(2, 8);

            map[49, 30].tiles = new List<TileParts>();
            map[49, 30].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[49, 30].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(2, 11), new Vector2(6, 13)));
            map[49, 30].tiles[1].above = true;

            map[49, 31].eventAction = area01.Statue;
            map[49, 31].walkable = false;
            map[49, 31].interactable = true;
            map[49, 31].tiles = new List<TileParts>();
            map[49, 31].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[49, 31].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(2, 12), new Vector2(6, 13)));
            //World Map Initialization Ends//

            using (FileStream stream = File.Open("C:\\Users\\Nye\\Dropbox\\Programming\\C#\\Programs\\RPG-Game\\Saves\\TestMap.bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, map);
            }

            //using (FileStream stream = File.Open("C:\\Users\\Nye\\Dropbox\\Programming\\C#\\Programs\\RPG-Game\\Saves\\TestMap.bin", FileMode.Open))
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    map = (Tile[,])formatter.Deserialize(stream);
            //}

            targetState = 0;
        }
        
        public override void Update(GameTime gameTime)
        {
            targetState = 0;

            if (currentState == 0)
            {
                NaviMovement(gameTime);
            }
            else if (currentState == 1)
            {
                NaviEvent(gameTime);
            }

            heroMover.Animate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Main main)
        {
            List<Vector3> drawAbove = new List<Vector3>();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    if (map[i, o].tiles.Count != 0)
                    {
                        for (int p = 0; p < map[i, o].tiles.Count; p++)
                        {
                            if (!map[i, o].tiles[p].above)
                            {
                                tileDraw(map[i, o].tiles[p], new Vector2(i, o), spriteBatch, main);
                            }
                            else
                            {
                                drawAbove.Add(new Vector3(i, o, p));
                            }
                        }
                    }
                    else
                    {
                        tileDraw(map[i, o], new Vector2(i, o), spriteBatch, main);
                    }
                }
            }

            heroMover.Draw(spriteBatch, camera.UpperLeft);

            for (int i = 0; i < drawAbove.Count; i++)
            {
                tileDraw(map[(int)drawAbove[i].X, (int)drawAbove[i].Y].tiles[(int)drawAbove[i].Z], new Vector2((int)drawAbove[i].X, (int)drawAbove[i].Y), spriteBatch, main);
            }

            if (currentState == 1)
            {
                if (currentEvent != null)
                {
                    if (currentEvent.line != null)
                    {
                        for (int i = 0; i < currentEvent.eventBox.parts.Count; i++)
                        {
                            currentEvent.eventBox.parts[i].Draw(spriteBatch);
                        }

                        if (currentEvent.line.Equals(currentEvent.lines[0]))
                        {
                            pointer.Draw(spriteBatch);
                        }

                        spriteBatch.DrawString(calibri, currentEvent.line, new Vector2(10, 10), Color.White);
                    }
                }
            }
        }


        private void NaviMovement(GameTime gameTime)
        {
            if (playerMovement != new Vector2(0, 0))
            {
                if (gameTime.TotalGameTime.TotalSeconds <= timer + 0.4)
                {
                    heroMover.UpperLeft += playerMovement;
                }
                else
                {
                    playerMovement = new Vector2(0, 0);
                }
            }
            else
            {
                if (upInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.gridPosition.Y != 0)
                    {
                        if (playerMovement == new Vector2(0, 0))
                        {
                            playerMovement = new Vector2(0, -2);

                            heroMover.UpperLeft += playerMovement;

                            heroMover.gridPosition.Y--;
                            if (heroMover.gridPosition.Y < 0 || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.Y++;

                                heroMover.UpperLeft -= playerMovement;

                                playerMovement = new Vector2(0, 0);

                                heroMover.setCurrentFrame(1, 3);
                            }
                            else
                            {
                                heroMover.StartAnimationShort(gameTime, 9, 11, 10);
                            }

                            heroMover.lastMoved = new Vector2(0, -1);

                            timer = gameTime.TotalGameTime.TotalSeconds;
                        }
                    }
                }
                if (downInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.gridPosition.Y != map.GetLength(1) - 1)
                    {
                        if (playerMovement == new Vector2(0, 0))
                        {
                            playerMovement = new Vector2(0, 2);

                            heroMover.UpperLeft += playerMovement;

                            heroMover.gridPosition.Y++;
                            if (heroMover.gridPosition.Y > map.GetLength(1) || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.Y--;

                                heroMover.UpperLeft -= playerMovement;

                                playerMovement = new Vector2(0, 0);

                                heroMover.setCurrentFrame(1, 0);
                            }
                            else
                            {
                                heroMover.StartAnimationShort(gameTime, 0, 2, 1);
                            }

                            heroMover.lastMoved = new Vector2(0, 1);

                            timer = gameTime.TotalGameTime.TotalSeconds;
                        }
                    }
                }
                if (leftInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.gridPosition.X != 0)
                    {
                        if (playerMovement == new Vector2(0, 0))
                        {
                            playerMovement = new Vector2(-2, 0);

                            heroMover.UpperLeft += playerMovement;

                            heroMover.gridPosition.X--;
                            if (heroMover.gridPosition.X < 0 || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.X++;

                                heroMover.UpperLeft -= playerMovement;

                                playerMovement = new Vector2(0, 0);

                                heroMover.setCurrentFrame(1, 1);
                            }
                            else
                            {
                                heroMover.StartAnimationShort(gameTime, 3, 5, 4);
                            }

                            heroMover.lastMoved = new Vector2(-1, 0);

                            timer = gameTime.TotalGameTime.TotalSeconds;
                        }
                    }
                }
                if (rightInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.gridPosition.X != map.GetLength(0) - 1)
                    {
                        if (playerMovement == new Vector2(0, 0))
                        {
                            playerMovement = new Vector2(2, 0);

                            heroMover.UpperLeft += playerMovement;

                            heroMover.gridPosition.X++;
                            if (heroMover.gridPosition.Y > map.GetLength(0) || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.X--;

                                heroMover.UpperLeft -= playerMovement;

                                playerMovement = new Vector2(0, 0);

                                heroMover.setCurrentFrame(1, 2);
                            }
                            else
                            {
                                heroMover.StartAnimationShort(gameTime, 6, 8, 7);
                            }

                            heroMover.lastMoved = new Vector2(1, 0);

                            timer = gameTime.TotalGameTime.TotalSeconds;
                        }
                    }
                }
                if (activateInput.inputState == Input.inputStates.pressed)
                {
                    if (map[(int)(heroMover.gridPosition.X + heroMover.lastMoved.X), (int)(heroMover.gridPosition.Y + heroMover.lastMoved.Y)].interactable)
                    {
                        ActivateState(1);

                        eventTile = map[(int)(heroMover.gridPosition.X + heroMover.lastMoved.X), (int)(heroMover.gridPosition.Y + heroMover.lastMoved.Y)];
                    }
                }
            }

            camera.UpperLeft = new Vector2((heroMover.UpperLeft.X + heroMover.GetWidth() / 2) - (camera.ViewWidth / 2),
                                           (heroMover.UpperLeft.Y + heroMover.GetHeight() / 2) - (camera.ViewHeight / 2));
        }

        private void NaviEvent(GameTime gameTime)
        {
            eventTile.eventAction.Invoke(this, gameTime);

            if(currentEvent.complete)
            {
                ActivateState(0);

                currentEvent.complete = false;
            }
        }


        //Activates the target state, setting all states to false, while keeping the target state true
        private void ActivateState(int targetState)
        {
            //Set all states to false
            for (int i = 0; i < state.Length; i++)
            {
                state[i] = false;
            }

            currentState = targetState;
            //Set the target state to true
            state[targetState] = true;
        }

        //Switches the target state, setting it to true if it was false, and false otherwise
        //If target state was false, currentState will be set to the last state still active
        private void SwitchState(int targetState)
        {
            if (state[targetState])
            {
                state[targetState] = false;

                //Find the highest value state still active, and set currentState to it
                for (int i = state.Length; i >= 0; i--)
                {
                    if (state[i])
                    {
                        currentState = i;
                    }
                }
            }
            else
            {
                state[targetState] = true;

                currentState = targetState;
            }
        }

        private void tileDraw(Tile tile, Vector2 gridPosition, SpriteBatch spriteBatch, Main main)
        {
            Sprite drawSprite = new Sprite();

            Texture2D texture = main.Content.Load<Texture2D>(tile.texture);

            drawSprite.SetTexture(texture, (int)tile.textureDimensions.X, (int)tile.textureDimensions.Y);
            drawSprite.setCurrentFrame((int)tile.currentFrame.X, (int)tile.currentFrame.Y);
            drawSprite.UpperLeft = new Vector2(gridPosition.X * drawSprite.frameWidth, gridPosition.Y * drawSprite.frameHeight);
            drawSprite.Draw(spriteBatch, camera.UpperLeft);
        }
    }
}
