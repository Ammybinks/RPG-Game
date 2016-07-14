using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RPG_Game
{
    class NaviState : StateManager
    {
        Texture2D heroMoverTexture;
        Mover heroMover;

        Texture2D hiroMoverTexture;
        Mover hiroMover;

        Texture2D hearoMoverTexture;
        Mover hearoMover;

        Texture2D hieroMoverTexture;
        Mover hieroMover;

        LinkedList<Mover> movers = new LinkedList<Mover>();

        Texture2D outside;

        Camera camera;

        Tile[,] map = new Tile[100, 50];
        Tile tile;
        Tile eventTile;

        bool[] state = new bool[2];
        int currentState;

        Vector2 playerMovement;
        Vector2 playerView = new Vector2(20, 11);

        public override void LoadContent(Main main)
        {
            calibri = main.Content.Load<SpriteFont>("Fonts\\Calibri");

            outside = main.Content.Load<Texture2D>("World\\Tilesets\\Outside");

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
                    tile = new Tile();

                    tile.SetTexture(outside, 6, 13);
                    tile.setCurrentFrame(1, 1);
                    tile.walkable = true;
                    tile.interactable = false;
                    tile.UpperLeft = new Vector2(tile.GetWidth() * i, tile.GetHeight() * o);

                    map[i, o] = tile;
                }
            }

            //Stone Edges
            for (int i = 0; i < playerView.X; i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].setCurrentFrame(1, 4);
                }
            }

            for (int i = map.GetLength(0) - (int)playerView.X; i < map.GetLength(0); i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].setCurrentFrame(1, 4);
                }
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            {
                for (int o = 0; o < playerView.Y; o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].setCurrentFrame(1, 4);
                }
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            {
                for (int o = map.GetLength(1) - (int)playerView.Y; o < map.GetLength(1); o++)
                {
                    map[i, o].walkable = false;
                    map[i, o].setCurrentFrame(1, 4);
                }
            }

            //Stone Edge Edges
            for (int o = (int)playerView.Y; o < map.GetLength(1) - playerView.Y; o++)
            {
                map[(int)playerView.X - 1, o].setCurrentFrame(2, 4);
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            {
                map[i, (int)playerView.Y - 3].setCurrentFrame(1, 5);
            }

            for (int i = (int)playerView.X; i < map.GetLength(0) - (int)playerView.X; i++)
            {
                map[i, map.GetLength(1) - (int)playerView.Y].setCurrentFrame(1, 3);
            }

            for (int o = (int)playerView.Y; o < map.GetLength(1) - playerView.Y; o++)
            {
                map[map.GetLength(0) - (int)playerView.X, o].setCurrentFrame(0, 4);
            }

            //Stone Walls
            for (int i = (int)playerView.X + 1; i < map.GetLength(0) - playerView.X - 1; i++)
            {
                map[i, (int)playerView.Y - 2].setCurrentFrame(1, 6);
            }

            for (int i = (int)playerView.X + 1; i < map.GetLength(0) - playerView.X - 1; i++)
            {
                map[i, (int)playerView.Y - 1].setCurrentFrame(1, 8);
            }

            map[(int)playerView.X, (int)playerView.Y - 2].setCurrentFrame(0, 6);
            map[(int)playerView.X, (int)playerView.Y - 1].setCurrentFrame(0, 8);

            map[map.GetLength(0) - (int)playerView.X - 1, (int)playerView.Y - 2].setCurrentFrame(2, 6);
            map[map.GetLength(0) - (int)playerView.X - 1, (int)playerView.Y - 1].setCurrentFrame(2, 8);

            map[49, 30].tiles = new List<Tile>();
            map[49, 30].tiles.Add(new Tile());
            map[49, 30].tiles[0].UpperLeft = map[49, 30].UpperLeft;
            map[49, 30].tiles[0].SetTexture(outside, 6, 13);
            map[49, 30].tiles[0].setCurrentFrame(1, 1);
            map[49, 30].tiles.Add(new Tile());
            map[49, 30].tiles[1].UpperLeft = map[49, 30].UpperLeft;
            map[49, 30].tiles[1].SetTexture(outside, 6, 13);
            map[49, 30].tiles[1].setCurrentFrame(2, 11);
            map[49, 30].tiles[1].above = true;

            map[49, 31].lines = new List<string>(1);
            map[49, 31].lines.Add("This is, well. I don't know what it is quite");
            map[49, 31].walkable = false;
            map[49, 31].interactable = true;
            map[49, 31].tiles = new List<Tile>();
            map[49, 31].tiles.Add(new Tile());
            map[49, 31].tiles[0].UpperLeft = map[49, 31].UpperLeft;
            map[49, 31].tiles[0].SetTexture(outside, 6, 13);
            map[49, 31].tiles[0].setCurrentFrame(1, 1);
            map[49, 31].tiles.Add(new Tile());
            map[49, 31].tiles[1].UpperLeft = map[49, 31].UpperLeft;
            map[49, 31].tiles[1].SetTexture(outside, 6, 13);
            map[49, 31].tiles[1].setCurrentFrame(2, 12);
            //World Map Initialization Ends//

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            List<Vector3> drawAbove = new List<Vector3>();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    if (map[i, o].tiles != null)
                    {
                        for (int p = 0; p < map[i, o].tiles.Count; p++)
                        {
                            if (!map[i, o].tiles[p].above)
                            {
                                map[i, o].tiles[p].Draw(spriteBatch, camera.UpperLeft);
                            }
                            else
                            {
                                drawAbove.Add(new Vector3(i, o, p));
                            }
                        }
                    }
                    else
                    {
                        map[i, o].Draw(spriteBatch, camera.UpperLeft);
                    }
                }
            }

            heroMover.Draw(spriteBatch, camera.UpperLeft);

            for (int i = 0; i < drawAbove.Count; i++)
            {
                map[(int)drawAbove[i].X, (int)drawAbove[i].Y].tiles[(int)drawAbove[i].Z].Draw(spriteBatch, camera.UpperLeft);
            }

            if (currentState == 1)
            {
                if (eventTile.eventLine != null)
                {
                    for (int i = 0; i < eventTile.eventBox.parts.Count; i++)
                    {
                        eventTile.eventBox.parts[i].Draw(spriteBatch);
                    }

                    if (eventTile.eventLine.Equals(eventTile.lines[0]))
                    {
                        pointer.Draw(spriteBatch);
                    }

                    spriteBatch.DrawString(calibri, eventTile.eventLine, new Vector2(10, 10), Color.White);
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
            if (eventTile.eventLine == null)
            {
                box = new Box();

                box.frameWidth = 800;
                box.frameHeight = 200;

                box.SetParts(cornerTexture, wallTexture, backTexture);

                pointer.Scale = new Vector2(0.4f, 0.4f);
                pointer.UpperLeft = new Vector2(800 - pointer.GetWidth() - 20, 200 - pointer.GetHeight() - 20);

                eventTile.eventLine = "";

                eventTile.eventBox = box;
            }

            if (eventTile.eventLine.Equals(eventTile.lines[0]))
            {
                timer = gameTime.TotalGameTime.TotalSeconds;

                if (activateInput.inputState == Input.inputStates.pressed)
                {
                    eventTile.lines.RemoveAt(0);

                    if (eventTile.lines.Count == 0)
                    {
                        pointer.Scale = new Vector2(0.8f, 0.8f);

                        eventTile.lines = null;
                        eventTile.eventBox = null;
                        eventTile.eventLine = null;

                        ActivateState(0);
                    }
                }
            }

            if (gameTime.TotalGameTime.TotalSeconds < timer + 0.5)
            {
                return;
            }

            if (eventTile.eventLine.Length + 1 < eventTile.lines[0].Length)
            {
                string i = eventTile.lines[0].Substring(eventTile.eventLine.Length);
                if (i.StartsWith(" "))
                {
                    int o = 0;

                    while (i.StartsWith(" "))
                    {
                        i = eventTile.lines[0].Substring(eventTile.eventLine.Length + o);

                        o++;
                    }

                    eventTile.eventLine = eventTile.lines[0].Remove(eventTile.eventLine.Length + o);
                }
                else
                {
                    eventTile.eventLine = eventTile.lines[0].Remove(eventTile.eventLine.Length + 1);
                }
            }
            else
            {
                eventTile.eventLine = eventTile.lines[0];
            }

            timer = gameTime.TotalGameTime.TotalSeconds;
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

    }
}
