using Microsoft.Xna.Framework;
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
        List<Battler> battlers = new List<Battler>();

        Camera camera;

        Box menuBox;
        Box inventoryBox;
        MultiBox heroBox;

        Tile[,] map = new Tile[100, 50];
        Tile tile;
        Tile eventTile;

        Vector2 movementModifier;
        Vector2 playerView = new Vector2(20, 11);

        public Battler target;

        bool runOnce = false;

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

            activeButtons = new List<Button>();

            heldItems = main.heldItems;

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
            battlers.Add(heroBattler);

            //Hiro Initialization
            hiroBattler = main.hiro;
            battlers.Add(hiroBattler);

            //Hearo Initialization
            hearoBattler = main.hearo;
            battlers.Add(hearoBattler);

            //Hiero Initialization
            hieroBattler = main.hiero;
            battlers.Add(hieroBattler);
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
            button.action = ItemSwitch;
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
            button.action = SkillsSwitch;
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
            button.action = MapSwitch;
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
            button.action = StatusSwitch;
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

            //Inventory Box
            inventoryBox = new Box();
            inventoryBox.frameWidth = 800;
            inventoryBox.frameHeight = 800;
            inventoryBox.UpperLeft = new Vector2(menuBox.GetWidth() + 10, 5);
            inventoryBox.SetParts(cornerTexture, wallTexture, backTexture);
            inventoryBox.activatorState = 3;
            inventoryBox.buttons = new List<Button>();
            allBoxes.Add(inventoryBox);

            ////Hero Buttons Box
            heroBox = new MultiBox();
            heroBox.frameWidth = 820;
            heroBox.frameHeight = 725;
            heroBox.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth - heroBox.GetWidth() - 5, 5);
            heroBox.SetParts(cornerTexture, wallTexture, backTexture);
            heroBox.activatorState = 6;
            heroBox.multiButtons = new List<MultiButton>();
            allBoxes.Add(heroBox);

            //Hero MultiButton
            multiButton = new MultiButton();
            multiButton.extraButtons = new List<Button>();
            multiButton.frameWidth = 150;
            multiButton.frameHeight = 150;
            multiButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((multiButton.GetHeight() + 25) * heroBox.multiButtons.Count));
            multiButton.SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.icon.SetTexture(heroFace);
            multiButton.icon.UpperLeft = new Vector2(multiButton.UpperLeft.X + 3, multiButton.UpperLeft.Y + 2);

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[0].frameWidth = 560;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Adorable Manchild";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 45;
            multiButton.extraButtons[1].frameHeight = 42;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "HP:";
            multiButton.extraButtons[1].displaySize = 0.75f;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 115;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - multiButton.extraButtons[2].GetWidth(), multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = heroBattler.health.ToString() + "/" + heroBattler.maxHealth.ToString();
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            heroBox.multiButtons.Add(multiButton);

            //Hiro MultiButton
            multiButton = new MultiButton();
            multiButton.extraButtons = new List<Button>();
            multiButton.frameWidth = 150;
            multiButton.frameHeight = 150;
            multiButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((multiButton.GetHeight() + 25) * heroBox.multiButtons.Count));
            multiButton.SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.icon.SetTexture(hiroFace);
            multiButton.icon.UpperLeft = new Vector2(multiButton.UpperLeft.X + 3, multiButton.UpperLeft.Y + 2);

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[0].frameWidth = 560;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Absolutely-Not-Into-It Love Interest";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 45;
            multiButton.extraButtons[1].frameHeight = 42;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "HP:";
            multiButton.extraButtons[1].displaySize = 0.75f;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 115;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - multiButton.extraButtons[2].GetWidth(), multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = hiroBattler.health.ToString() + "/" + hiroBattler.maxHealth.ToString();
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            heroBox.multiButtons.Add(multiButton);

            //Hearo MultiButton
            multiButton = new MultiButton();
            multiButton.extraButtons = new List<Button>();
            multiButton.frameWidth = 150;
            multiButton.frameHeight = 150;
            multiButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((multiButton.GetHeight() + 25) * heroBox.multiButtons.Count));
            multiButton.SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.icon.SetTexture(hearoFace);
            multiButton.icon.UpperLeft = new Vector2(multiButton.UpperLeft.X + 3, multiButton.UpperLeft.Y + 2);

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[0].frameWidth = 560;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Endearing Father Figure";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 45;
            multiButton.extraButtons[1].frameHeight = 42;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "HP:";
            multiButton.extraButtons[1].displaySize = 0.75f;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 115;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - multiButton.extraButtons[2].GetWidth(), multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = hearoBattler.health.ToString() + "/" + hearoBattler.maxHealth.ToString();
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            heroBox.multiButtons.Add(multiButton);

            //Hiero MultiButton
            multiButton = new MultiButton();
            multiButton.extraButtons = new List<Button>();
            multiButton.frameWidth = 150;
            multiButton.frameHeight = 150;
            multiButton.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((multiButton.GetHeight() + 25) * heroBox.multiButtons.Count));
            multiButton.SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.icon.SetTexture(hieroFace);
            multiButton.icon.UpperLeft = new Vector2(multiButton.UpperLeft.X + 3, multiButton.UpperLeft.Y + 2);

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[0].frameWidth = 560;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Comic Relief";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 45;
            multiButton.extraButtons[1].frameHeight = 42;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "HP:";
            multiButton.extraButtons[1].displaySize = 0.75f;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 115;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - multiButton.extraButtons[2].GetWidth(), multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = hieroBattler.health.ToString() + "/" + hieroBattler.maxHealth.ToString();
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            heroBox.multiButtons.Add(multiButton);

            stateMethods[0] = Movement;
            stateMethods[1] = Event;
            stateMethods[2] = Menu;
            stateMethods[3] = Inventory;
            stateMethods[4] = Skills;
            stateMethods[5] = Map;
            stateMethods[6] = Target;
            stateMethods[7] = StateSwitch;

            switchStateMethods[0] = MovementSwitch;
            switchStateMethods[1] = MovementSwitch;
            switchStateMethods[2] = MenuSwitch;
            switchStateMethods[3] = ItemSwitch;
            switchStateMethods[4] = MovementSwitch;
            switchStateMethods[5] = MovementSwitch;
            switchStateMethods[6] = TargetSwitch;
            switchStateMethods[7] = StateSwitch;

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
                if (currentAction != null)
                {
                    currentAction.DrawAll(spriteBatch, calibri);
                }
                else
                {
                    if (currentEvent.line != null)
                    {
                        currentEvent.eventBox.DrawParts(spriteBatch);

                        spriteBatch.DrawString(calibri, currentEvent.line, currentEvent.lineUpperLeft, Color.White);
                    }
                }
            }

            bool temp = false;

            //Boxes \ Buttons Draw Cycle
            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (state[allBoxes[i].activatorState])
                {
                    allBoxes[i].DrawParts(spriteBatch);

                    for (int o = 0; o < allBoxes[i].GetButtons().Count; o++)
                    {
                        if (allBoxes[i].activatorState == currentState && o == buttonIndex)
                        {
                            temp = true;
                        }
                        else
                        {
                            temp = false;
                        }

                        allBoxes[i].GetButtons()[o].DrawParts(spriteBatch, calibri, temp);
                    }
                }
            }
            pointer.Draw(spriteBatch);
        }


        private void Movement(GameTime gameTime)
        {
            if (pointer.isAlive)
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
                if (menuInput.inputState == Input.inputStates.pressed)
                {
                    ActivateState(6);

                    MenuSwitch(gameTime);
                }
            }

            camera.UpperLeft = new Vector2((heroMover.UpperLeft.X + heroMover.GetWidth() / 2) - (camera.ViewWidth / 2),
                                           (heroMover.UpperLeft.Y + heroMover.GetHeight() / 2) - (camera.ViewHeight / 2));
        }

        private void Event(GameTime gameTime)
        {
            if (currentAction != null)
            {
                if (currentAction.Call(gameTime, this))
                {
                    int tempButtons = inventoryBox.buttons.Count;
                    int tempItems = heldItems.Count;
                    
                    if(heldItems.IndexOf((Item)currentAction) == -1)
                    {
                        ItemRefresh();
                        currentState = 3;
                    }
                    else
                    {
                        TargetRefresh();
                        currentState = 6;
                    }
                    
                    state[1] = false;
                    previousState[1] = false;

                    runOnce = false;
                }
                if(currentAction.runOnce)
                {
                    if (!runOnce)
                    {
                        ItemRefresh();
                        TargetRefresh();

                        runOnce = true;
                    }
                }
            }
            else
            {
                eventTile.eventAction.Invoke(this, gameTime);

                if (currentEvent.complete)
                {
                    ActivateState(0);

                    currentEvent.complete = false;
                }
            }
        }

        private void Menu(GameTime gameTime)
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

            if (temp.activate)
            {
                activeButtons[buttonIndex].action.Invoke(gameTime);
            }
            else if (temp.menu)
            {
                SwitchState(2, gameTime);
            }
        }

        private void Inventory(GameTime gameTime)
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

            if (temp.activate)
            {
                if (heldItems[buttonIndex].mapUsable)
                {
                    TargetSwitch(gameTime);

                    currentAction = heldItems[buttonIndex];
                }
            }
            else if (temp.menu)
            {
                SwitchState(3, gameTime);
            }
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

        private void Target(GameTime gameTime)
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 50);

            if (temp.activate)
            {
                pointer.isAlive = false;

                target = battlers[buttonIndex];
                
                SwitchState(1, gameTime);
            }
            else if (temp.menu)
            {
                currentAction = null;

                SwitchState(3, gameTime);
                ItemSwitch(gameTime);
                currentState = 3;
            }
        }

        private void StateSwitch(GameTime gameTime)
        {

        }


        private void MovementSwitch(GameTime gameTime)
        {
            ClearStates();
            ActivateState(0);
        }

        private void MenuSwitch(GameTime gameTime)
        {
            SwitchState(2, gameTime);

            activeButtons.Clear();

            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (allBoxes[i].activatorState == currentState)
                {
                    for (int o = 0; o < allBoxes[i].buttons.Count; o++)
                    {
                        activeButtons.Add(allBoxes[i].buttons[o]);
                    }
                }
            }

            for(int i = 0; i < heroBox.multiButtons.Count; i++)
            {
                heroBox.multiButtons[i].extraButtons[2].display = battlers[i].health + "/" + battlers[i].maxHealth;
            }
        }

        private void ItemSwitch(GameTime gameTime)
        {
            SwitchState(3, gameTime);

            ItemRefresh();
        }

        private void ItemRefresh()
        {
            MultiButton tempButton;

            inventoryBox.buttons.Clear();
            inventoryBox.buttons.Capacity = 0;

            if (heldItems.Count == 0)
            {
                Item item = new Item();
                item.name = "--Empty, really empty--";
                item.description = "Looks like you don't have any items on you,\nmaybe this is a good time to stock up?";
                item.iconFrame = new Vector2(7, 10);
                item.heldCount = 13;
                heldItems.Add(item);
            }

            for (int i = 0; i < heldItems.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(inventoryBox.UpperLeft.X + 80, inventoryBox.UpperLeft.Y + 10 + (60 * i));
                tempButton.display = heldItems[i].name;
                tempButton.icon.SetTexture(iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)heldItems[i].iconFrame.X, (int)heldItems[i].iconFrame.Y);
                tempButton.frameWidth = inventoryBox.frameWidth - 140;
                tempButton.frameHeight = 50;
                tempButton.SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), inventoryBox.UpperLeft.Y + 10 + (60 * i));
                tempButton.extraButtons[0].display = heldItems[i].heldCount.ToString();
                tempButton.extraButtons[0].frameWidth = 50;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[0].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].UpperLeft = new Vector2(inventoryBox.UpperLeft.X + 80, 200);
                tempButton.extraButtons[1].display = heldItems[i].description;
                tempButton.extraButtons[1].frameWidth = inventoryBox.frameWidth - 90;
                tempButton.extraButtons[1].frameHeight = 100;
                tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[1].showOnSelected = true;
                tempButton.extraButtons[1].icon = null;

                if (!heldItems[i].mapUsable)
                {
                    tempButton.displayColour = Color.Gray;
                    tempButton.extraButtons[0].displayColour = Color.Gray;
                }
                inventoryBox.buttons.Add(tempButton);
            }

            inventoryBox.frameHeight = (int)inventoryBox.buttons[inventoryBox.buttons.Count - 1].UpperLeft.Y + 60;
            inventoryBox.SetParts(cornerTexture, wallTexture, backTexture);

            activeButtons = inventoryBox.buttons;

            buttonIndex = 0;
        }

        private void SkillsSwitch(GameTime gameTime)
        {

        }

        private void MapSwitch(GameTime gameTime)
        {

        }

        private void StatusSwitch(GameTime gameTime)
        {

        }

        private void TargetSwitch(GameTime gameTime)
        {
            currentState = 6;

            TargetRefresh();
        }

        private void TargetRefresh()
        {
            activeButtons = new List<Button>();

            for (int i = 0; i < heroBox.multiButtons.Count; i++)
            {
                heroBox.multiButtons[i].extraButtons[2].display = battlers[i].health.ToString() + "/" + battlers[i].maxHealth.ToString();

                activeButtons.Add(heroBox.multiButtons[i]);
            }
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