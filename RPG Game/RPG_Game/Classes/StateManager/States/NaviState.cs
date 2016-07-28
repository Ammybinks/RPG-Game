﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG_Game
{
    public class NaviState : StateManager
    {
        Texture2D heroMoverTexture;
        Texture2D heroFace;
        Mover heroMover;
        Battler heroBattler;

        Texture2D hiroMoverTexture;
        Texture2D hiroFace;
        Mover hiroMover;
        Battler hiroBattler;

        Texture2D hearoMoverTexture;
        Texture2D hearoFace;
        Mover hearoMover;
        Battler hearoBattler;

        Texture2D hieroMoverTexture;
        Texture2D hieroFace;
        Mover hieroMover;
        Battler hieroBattler;

        LinkedList<Mover> movers = new LinkedList<Mover>();

        Camera camera;

        Box menuBox;
        Box heroBox;

        Tile[,] map = new Tile[100, 50];
        Tile tile;
        Tile eventTile;

        Action<GameTime>[] stateMethods = new Action<GameTime>[7];
        bool[] state = new bool[7];
        int currentState;

        Vector2 movementModifier;
        Vector2 playerView = new Vector2(20, 11);

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

            heroFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Adorable Manchild");
            hiroFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Absolutely-Not-Into-It Love Interest");
            hearoFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Endearing Father Figure");
            hieroFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Comic Relief");

            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);

            camera = new Camera();
            camera.WorldWidth = 3840;
            camera.WorldHeight = 2160;
            camera.ViewWidth = 1920;
            camera.ViewHeight = 1080;
            camera.UpperLeft = new Vector2(0, 0);

            //Mover Initialization Begins//
            //Hero Mover
            heroMover = new Mover();
            heroMover.ContinuousAnimation = false;
            heroMover.AnimationInterval = 100;
            heroMover.SetTexture(heroMoverTexture, 3, 4);
            heroMover.Scale = new Vector2(1, 1);
            heroMover.gridPosition = new Vector2(49, 24);
            heroMover.UpperLeft = new Vector2(heroMover.gridPosition.X * 48, heroMover.gridPosition.Y * 48);
            movers.AddFirst(heroMover);

            //Hiro Mover
            hiroMover = new Mover();
            hiroMover.SetTexture(heroMoverTexture, 3, 4);
            hiroMover.UpperLeft = new Vector2(hiroMover.GetWidth(), hiroMover.GetHeight());
            movers.AddFirst(hiroMover);

            //Hearo Mover
            hearoMover = new Mover();
            hearoMover.SetTexture(heroMoverTexture, 3, 4);
            hearoMover.UpperLeft = new Vector2(hearoMover.GetWidth(), hearoMover.GetHeight());
            movers.AddFirst(hearoMover);

            //Hiero Mover
            hieroMover = new Mover();
            hieroMover.SetTexture(heroMoverTexture, 3, 4);
            hieroMover.UpperLeft = new Vector2(hieroMover.GetWidth(), hieroMover.GetHeight());
            movers.AddFirst(hieroMover);
            //Mover Initialization Ends//

            //Battler Initialization Begins//
            //Hero Initialization
            heroBattler = main.hero;

            //Hiro Initialization
            hiroBattler = main.hiro;

            //Hearo Initialization
            hearoBattler = main.hearo;

            //Hiero Initialization
            hieroBattler = main.hiero;
            //Battler Initialization Ends//

            ////World Map Initialization Begins//
            ////Grass Base
            //for (int i = 0; i < map.GetLength(0); i++)
            //{
            //    for (int o = 0; o < map.GetLength(1); o++)
            //    {
            //        tile = new Tile("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13));

            //        tile.walkable = true;
            //        tile.interactable = false;

            //        map[i, o] = tile;
            //    }
            //}

            ////Stone Edges
            //for (int i = 0; i < playerView.X; i++)
            //{
            //    for (int o = 0; o < map.GetLength(1); o++)
            //    {
            //        map[i, o].walkable = false;
            //        map[i, o].currentFrame = new Vector2(1, 4);
            //    }
            //}

            //for (int i = map.GetLength(0) - (int)playerView.X; i < map.GetLength(0); i++)
            //{
            //    for (int o = 0; o < map.GetLength(1); o++)
            //    {
            //        map[i, o].walkable = false;
            //        map[i, o].currentFrame = new Vector2(1, 4);
            //    }
            //}

            //for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            //{
            //    for (int o = 0; o < playerView.Y; o++)
            //    {
            //        map[i, o].walkable = false;
            //        map[i, o].currentFrame = new Vector2(1, 4);
            //    }
            //}

            //for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            //{
            //    for (int o = map.GetLength(1) - (int)playerView.Y; o < map.GetLength(1); o++)
            //    {
            //        map[i, o].walkable = false;
            //        map[i, o].currentFrame = new Vector2(1, 4);
            //    }
            //}

            ////Stone Edge Edges
            //for (int o = (int)playerView.Y; o < map.GetLength(1) - playerView.Y; o++)
            //{
            //    map[(int)playerView.X - 1, o].currentFrame = new Vector2(2, 4);
            //}

            //for (int i = (int)playerView.X; i < map.GetLength(0) - playerView.X; i++)
            //{
            //    map[i, (int)playerView.Y - 3].currentFrame = new Vector2(1, 5);
            //}

            //for (int i = (int)playerView.X; i < map.GetLength(0) - (int)playerView.X; i++)
            //{
            //    map[i, map.GetLength(1) - (int)playerView.Y].currentFrame = new Vector2(1, 3);
            //}

            //for (int o = (int)playerView.Y; o < map.GetLength(1) - playerView.Y; o++)
            //{
            //    map[map.GetLength(0) - (int)playerView.X, o].currentFrame = new Vector2(0, 4);
            //}

            ////Stone Walls
            //for (int i = (int)playerView.X + 1; i < map.GetLength(0) - playerView.X - 1; i++)
            //{
            //    map[i, (int)playerView.Y - 2].currentFrame = new Vector2(1, 6);
            //}

            //for (int i = (int)playerView.X + 1; i < map.GetLength(0) - playerView.X - 1; i++)
            //{
            //    map[i, (int)playerView.Y - 1].currentFrame = new Vector2(1, 8);
            //}

            //map[(int)playerView.X, (int)playerView.Y - 2].currentFrame = new Vector2(0, 6);
            //map[(int)playerView.X, (int)playerView.Y - 1].currentFrame = new Vector2(0, 8);

            //map[map.GetLength(0) - (int)playerView.X - 1, (int)playerView.Y - 2].currentFrame = new Vector2(2, 6);
            //map[map.GetLength(0) - (int)playerView.X - 1, (int)playerView.Y - 1].currentFrame = new Vector2(2, 8);

            //map[49, 30].tiles = new List<TileParts>();
            //map[49, 30].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            //map[49, 30].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(2, 11), new Vector2(6, 13)));
            //map[49, 30].tiles[1].above = true;

            //map[49, 31].eventAction = area01.Statue;
            //map[49, 31].walkable = false;
            //map[49, 31].interactable = true;
            //map[49, 31].tiles = new List<TileParts>();
            //map[49, 31].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            //map[49, 31].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(2, 12), new Vector2(6, 13)));
            ////World Map Initialization Ends//

            //using (FileStream stream = File.Open("C:\\Users\\Nye\\Dropbox\\Programming\\C#\\Programs\\RPG-Game\\Saves\\TestMap.bin", FileMode.Create))
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(stream, map);
            //}

            using (FileStream stream = File.Open("C:\\Users\\Nye\\Dropbox\\Programming\\C#\\Programs\\RPG-Game\\Saves\\TestMap.bin", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                map = (Tile[,])formatter.Deserialize(stream);
            }

            //Boxes Initialization Begins//
            ////Base Menu Box
            menuBox = new Box();
            menuBox.frameWidth = 280;
            menuBox.frameHeight = 335;
            menuBox.UpperLeft = new Vector2(5, 5);
            menuBox.SetParts(cornerTexture, wallTexture, backTexture);
            menuBox.activatorState = 2;
            menuBox.buttons = new List<Button>(5);
            allBoxes.Add(menuBox);

            //Button that advances into inventory menu
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 200;
            button.UpperLeft = new Vector2(75, 15 + ((button.GetHeight() + 15) * menuBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "Inventory";
            button.action = SwitchInventory;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(9, 7);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            menuBox.buttons.Add(button);

            //Button that advances into skills menu
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 200;
            button.UpperLeft = new Vector2(75, 15 + ((button.GetHeight() + 15) * menuBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "Skills";
            button.action = SwitchSkills;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(15, 4);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            menuBox.buttons.Add(button);

            //Button that advances into map menu
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 200;
            button.UpperLeft = new Vector2(75, 15 + ((button.GetHeight() + 15) * menuBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "Map";
            button.action = SwitchMap;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(14, 11);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            menuBox.buttons.Add(button);

            //Button that advances into status menu
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 200;
            button.UpperLeft = new Vector2(75, 15 + ((button.GetHeight() + 15) * menuBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "Status";
            button.action = SwitchStatus;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(3, 0);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            menuBox.buttons.Add(button);

            //Button that exits the game
            button = new Button();
            button.frameHeight = 50;
            button.frameWidth = 200;
            button.UpperLeft = new Vector2(75, 15 + ((button.GetHeight() + 15) * menuBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "Exit";
            button.action = Exit;
            button.icon.SetTexture(iconTexture, 16, 20);
            button.icon.setCurrentFrame(5, 5);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 10, button.UpperLeft.Y + 9);
            menuBox.buttons.Add(button);

            ////Hero Buttons Box
            heroBox = new Box();
            heroBox.frameWidth = 900;
            heroBox.frameHeight = 725;
            heroBox.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth - heroBox.GetWidth() - 5, 5);
            heroBox.SetParts(cornerTexture, wallTexture, backTexture);
            heroBox.activatorState = 3;
            heroBox.buttons = new List<Button>();
            allBoxes.Add(heroBox);

            //Hero Button
            button = new Button();
            button.frameWidth = 800;
            button.frameHeight = 150;
            button.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth - button.GetWidth() - 25, 25 + ((button.GetHeight() + 25) * heroBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "The Adorable Manchild\n\nHP\nMP\nEXP";
            button.action = Status;
            button.icon.SetTexture(heroFace);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 15, button.UpperLeft.Y + 2);
            heroBox.buttons.Add(button);
            
            //Hiro Button
            button = new Button();
            button.frameWidth = 800;
            button.frameHeight = 150;
            button.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth - button.GetWidth() - 25, 25 + ((button.GetHeight() + 25) * heroBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "The Absolutely-Not-Into-It Love Interest\n\nHP\nMP\nEXP";
            button.action = Status;
            button.icon.SetTexture(hiroFace);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 15, button.UpperLeft.Y + 2);
            heroBox.buttons.Add(button);

            //Hearo Button
            button = new Button();
            button.frameWidth = 800;
            button.frameHeight = 150;
            button.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth - button.GetWidth() - 25, 25 + ((button.GetHeight() + 25) * heroBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "The Endearing Father Figure\n\nHP\nMP\nEXP";
            button.action = Status;
            button.icon.SetTexture(hearoFace);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 15, button.UpperLeft.Y + 2);
            heroBox.buttons.Add(button);

            //Hiero Button
            button = new Button();
            button.frameWidth = 800;
            button.frameHeight = 150;
            button.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth - button.GetWidth() - 25, 25 + ((button.GetHeight() + 25) * heroBox.buttons.Count));
            button.SetParts(cornerTexture, wallTexture, backTexture);
            button.display = "The Comic Relief\n\nHP\nMP\nEXP";
            button.action = Status;
            button.icon.SetTexture(hieroFace);
            button.icon.UpperLeft = new Vector2(button.UpperLeft.X + 15, button.UpperLeft.Y + 2);
            heroBox.buttons.Add(button);

            stateMethods[0] = Movement;
            stateMethods[1] = Event;
            stateMethods[2] = Menu;
            stateMethods[3] = Inventory;
            stateMethods[4] = Skills;
            stateMethods[5] = Map;
            stateMethods[6] = StateSwitch;

            targetState = 0;
        }
        
        public override void Update(GameTime gameTime)
        {
            stateMethods[currentState].Invoke(gameTime);

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
                        currentEvent.eventBox.DrawParts(spriteBatch, calibri);

                        spriteBatch.DrawString(calibri, currentEvent.line, currentEvent.lineUpperLeft, Color.White);
                    }
                }
            }
            
            //Boxes \ Buttons Draw Cycle
            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (state[allBoxes[i].activatorState])
                {
                    allBoxes[i].DrawParts(spriteBatch, calibri);

                    for (int o = 0; o < allBoxes[i].buttons.Count; o++)
                    {
                        allBoxes[i].buttons[o].DrawParts(spriteBatch, calibri);

                        if (allBoxes[i].buttons[o].icon != null)
                        {
                            allBoxes[i].buttons[o].icon.Draw(spriteBatch);
                        }
                    }
                }
            }

            pointer.Draw(spriteBatch);
        }


        private void Movement(GameTime gameTime)
        {
            if(pointer.isAlive)
            {
                pointer.isAlive = false;
            }

            if (movementModifier != new Vector2(0, 0))
            {
                if (step.Invoke(gameTime, timer, 2, 0.4, heroMover, movementModifier))
                {
                    movementModifier = new Vector2(0, 0);
                }
            }
            else
            {
                if (upInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.gridPosition.Y != 0)
                    {
                        if (movementModifier == new Vector2(0, 0))
                        {
                            movementModifier = new Vector2(0, -1);
                            
                            heroMover.gridPosition.Y--;
                            if (heroMover.gridPosition.Y < 0 || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.Y++;
                                
                                movementModifier = new Vector2(0, 0);

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
                        if (movementModifier == new Vector2(0, 0))
                        {
                            movementModifier = new Vector2(0, 1);
                            
                            heroMover.gridPosition.Y++;
                            if (heroMover.gridPosition.Y > map.GetLength(1) || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.Y--;
                                
                                movementModifier = new Vector2(0, 0);

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
                        if (movementModifier == new Vector2(0, 0))
                        {
                            movementModifier = new Vector2(-1, 0);
                            
                            heroMover.gridPosition.X--;
                            if (heroMover.gridPosition.X < 0 || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.X++;
                                
                                movementModifier = new Vector2(0, 0);

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
                        if (movementModifier == new Vector2(0, 0))
                        {
                            movementModifier = new Vector2(1, 0);
                            
                            heroMover.gridPosition.X++;
                            if (heroMover.gridPosition.Y > map.GetLength(0) || !map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].walkable)
                            {
                                heroMover.gridPosition.X--;
                                
                                movementModifier = new Vector2(0, 0);

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
                if(menuInput.inputState == Input.inputStates.pressed)
                {
                    ActivateState(2);
                    state[3] = true;

                    activeButtons = new List<Button>();

                    for (int i = 0; i < allBoxes.Count; i++)
                    {
                        if (allBoxes[i].activatorState == currentState)
                        {
                            for(int o = 0; o < allBoxes[i].buttons.Count; o++)
                            {
                                activeButtons.Add(allBoxes[i].buttons[o]);
                            }
                        }
                    }
                }
            }

            camera.UpperLeft = new Vector2((heroMover.UpperLeft.X + heroMover.GetWidth() / 2) - (camera.ViewWidth / 2),
                                           (heroMover.UpperLeft.Y + heroMover.GetHeight() / 2) - (camera.ViewHeight / 2));
        }

        private void Event(GameTime gameTime)
        {
            eventTile.eventAction.Invoke(this, gameTime);

            if(currentEvent.complete)
            {
                ActivateState(0);

                currentEvent.complete = false;
            }
        }

        private void Menu(GameTime gameTime)
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            if (temp.activate)
            {
                activeButtons[buttonIndex].action.Invoke(gameTime);
            }

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);
        }

        private void Inventory(GameTime gameTime)
        {

        }

        private void Skills(GameTime gameTime)
        {

        }

        private void Map(GameTime gameTime)
        {

        }

        private void Status(GameTime gameTime)
        {

        }

        private void StateSwitch(GameTime gameTime)
        {

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


        private void SwitchInventory(GameTime gameTime)
        {

        }

        private void SwitchSkills(GameTime gameTime)
        {

        }

        private void SwitchMap(GameTime gameTime)
        {

        }

        private void SwitchStatus(GameTime gameTime)
        {

        }

        private void Exit(GameTime gameTime)
        {
            quitting = true;
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