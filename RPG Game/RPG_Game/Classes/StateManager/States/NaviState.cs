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
        public Texture2D heroFace;
        public Mover heroMover;
        public Hero heroBattler;

        Texture2D hiroMoverTexture;
        public Texture2D hiroFace;
        Mover hiroMover;
        public Hero hiroBattler;

        Texture2D hearoMoverTexture;
        public Texture2D hearoFace;
        Mover hearoMover;
        public Hero hearoBattler;

        Texture2D hieroMoverTexture;
        public Texture2D hieroFace;
        Mover hieroMover;
        public Hero hieroBattler;

        Texture2D shopkeepTexture;
        Shantae shopkeep;

        Texture2D arenaTexture;
        ArenaManager arena;

        public Texture2D werewolfTexture;

        public List<PotentialEnemy> potentialEnemies;
        public List<Troop> potentialTroops;

        List<Mover> movers = new List<Mover>();
        List<BattleMover> battleMovers = new List<BattleMover>();
        List<Hero> battlers = new List<Hero>();

        Random rand = new Random();

        Camera camera;

        Box menuBox;
        public Box inventoryBox;
        Box skillsBox;
        MultiBox heroBox;

        Tile[,] map = new Tile[100, 50];
        Tile tile;
        Tile eventTile;

        public EventMover eventMover;
        
        Vector2 playerView = new Vector2(20, 11);

        public Battler target;
        public Battler actor;
        
        public Event currentEvent;

        public int encounterRate;

        private void textureInitialize(Main main)
        {
            calibri = main.Content.Load<SpriteFont>("Fonts\\Calibri");

            pointerTexture = main.Content.Load<Texture2D>("Misc\\Pointer");
            iconTexture = main.Content.Load<Texture2D>("Misc\\IconSet");

            cornerTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Corner");
            wallTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Wall Expandable");
            backTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Back");

            arrowTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Arrow");
            arrowSelectedTexture = main.Content.Load<Texture2D>("Misc\\Interface\\Interface Arrow Selected");

            heroMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Adorable Manchild");
            hiroMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Absolutely-Not-Into-It Love Interest");
            hearoMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Endearing Father Figure");
            hieroMoverTexture = main.Content.Load<Texture2D>("Characters\\Heroes\\Navigation\\The Comic Relief");

            shopkeepTexture = main.Content.Load<Texture2D>("Characters\\Friendlies\\Navigation\\Shopkeep The First");
            arenaTexture = main.Content.Load<Texture2D>("Characters\\Friendlies\\Navigation\\Arena");

            heroFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Adorable Manchild");
            hiroFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Absolutely-Not-Into-It Love Interest");
            hearoFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Endearing Father Figure");
            hieroFace = main.Content.Load<Texture2D>("Characters\\Heroes\\Portraits\\The Comic Relief");

            werewolfTexture = main.Content.Load<Texture2D>("Characters\\Enemies\\Werewolf");
        }

        private void pointerInitialize(Main main)
        {
            pointer = new Sprite();

            pointer.SetTexture(pointerTexture);
            pointer.Scale = new Vector2(0.8f, 0.8f);
            pointer.Origin = pointer.GetCenter();
        }

        private void cameraInitialize(Main main)
        {
            camera = new Camera();
            camera.WorldWidth = 3840;
            camera.WorldHeight = 2160;
            camera.ViewWidth = 1920;
            camera.ViewHeight = 1080;
            camera.UpperLeft = new Vector2(0, 0);
        }

        private void moverInitialise(Main main)
        {
            //Hero Mover
            heroMover = new Mover();
            heroMover.ContinuousAnimation = false;
            heroMover.AnimationInterval = 100;
            heroMover.SetTexture(heroMoverTexture, 3, 4);
            heroMover.Scale = new Vector2(1, 1);
            heroMover.gridPosition = new Vector2(49, 24);
            heroMover.UpperLeft = new Vector2(heroMover.gridPosition.X * 48, heroMover.gridPosition.Y * 48);
            movers.Add(heroMover);

            //Hiro Mover
            hiroMover = new Mover();
            hiroMover.SetTexture(heroMoverTexture, 3, 4);
            hiroMover.UpperLeft = new Vector2(hiroMover.GetWidth(), hiroMover.GetHeight());
            movers.Add(hiroMover);

            //Hearo Mover
            hearoMover = new Mover();
            hearoMover.SetTexture(heroMoverTexture, 3, 4);
            hearoMover.UpperLeft = new Vector2(hearoMover.GetWidth(), hearoMover.GetHeight());
            movers.Add(hearoMover);

            //Hiero Mover
            hieroMover = new Mover();
            hieroMover.SetTexture(heroMoverTexture, 3, 4);
            hieroMover.UpperLeft = new Vector2(hieroMover.GetWidth(), hieroMover.GetHeight());
            movers.Add(hieroMover);

            //Shopkeep Mover
            shopkeep = new Shantae();
            shopkeep.ContinuousAnimation = false;
            shopkeep.AnimationInterval = 100;
            shopkeep.SetTexture(shopkeepTexture, 3, 4);
            shopkeep.Scale = new Vector2(1, 1);
            shopkeep.gridPosition = new Vector2(49, 30);
            shopkeep.UpperLeft = new Vector2(shopkeep.gridPosition.X * 48, shopkeep.gridPosition.Y * 48);
            movers.Add(shopkeep);

            //Arena Mover
            arena = new ArenaManager();
            arena.ContinuousAnimation = false;
            arena.SetTexture(arenaTexture, 3, 4);
            arena.setCurrentFrame(1, 0);
            arena.Scale = new Vector2(1, 1);
            arena.gridPosition = new Vector2(49, 20);
            arena.UpperLeft = new Vector2(arena.gridPosition.X * 48, arena.gridPosition.Y * 48);
            arena.InitializeTroops(this, main);
            movers.Add(arena);
            battleMovers.Add(arena);
        }

        private void battlerInitialise(Main main)
        {
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
        }

        private void mapInitialize(Main main)
        {
            //Grass Base
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int o = 0; o < map.GetLength(1); o++)
                {
                    tile = new Tile("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13));

                    tile.walkable = true;
                    tile.occupied = false;
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

            map[49, 31].currentEvent = new Statue();
            map[49, 31].walkable = false;
            map[49, 31].interactable = true;
            map[49, 31].tiles = new List<TileParts>();
            map[49, 31].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[49, 31].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(2, 12), new Vector2(6, 13)));
            

            map[54, 23].currentEvent = new Battle01();
            (map[54, 23].currentEvent as Battle01).InitializePotentials(this, main);
            map[54, 23].walkable = true;
            map[54, 23].interactable = false;
            map[54, 23].tiles = new List<TileParts>();
            map[54, 23].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[54, 23].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(3, 0), new Vector2(6, 13)));

            map[55, 23].currentEvent = new Battle01();
            (map[55, 23].currentEvent as Battle01).InitializePotentials(this, main);
            map[55, 23].walkable = true;
            map[55, 23].interactable = false;
            map[55, 23].tiles = new List<TileParts>();
            map[55, 23].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[55, 23].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(4, 0), new Vector2(6, 13)));

            map[56, 23].currentEvent = new Battle01();
            (map[56, 23].currentEvent as Battle01).InitializePotentials(this, main);
            map[56, 23].walkable = true;
            map[56, 23].interactable = false;
            map[56, 23].tiles = new List<TileParts>();
            map[56, 23].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[56, 23].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(5, 0), new Vector2(6, 13)));

            map[54, 24].currentEvent = new Battle01();
            (map[54, 24].currentEvent as Battle01).InitializePotentials(this, main);
            map[54, 24].walkable = true;
            map[54, 24].interactable = false;
            map[54, 24].tiles = new List<TileParts>();
            map[54, 24].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[54, 24].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(3, 1), new Vector2(6, 13)));

            map[55, 24].currentEvent = new Battle01();
            (map[55, 24].currentEvent as Battle01).InitializePotentials(this, main);
            map[55, 24].walkable = true;
            map[55, 24].interactable = false;
            map[55, 24].tiles = new List<TileParts>();
            map[55, 24].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[55, 24].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(4, 1), new Vector2(6, 13)));

            map[56, 24].currentEvent = new Battle01();
            (map[56, 24].currentEvent as Battle01).InitializePotentials(this, main);
            map[56, 24].walkable = true;
            map[56, 24].interactable = false;
            map[56, 24].tiles = new List<TileParts>();
            map[56, 24].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[56, 24].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(5, 1), new Vector2(6, 13)));

            map[54, 25].currentEvent = new Battle01();
            (map[54, 25].currentEvent as Battle01).InitializePotentials(this, main);
            map[54, 25].walkable = true;
            map[54, 25].interactable = false;
            map[54, 25].tiles = new List<TileParts>();
            map[54, 25].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[54, 25].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(3, 2), new Vector2(6, 13)));

            map[55, 25].currentEvent = new Battle01();
            (map[55, 25].currentEvent as Battle01).InitializePotentials(this, main);
            map[55, 25].walkable = true;
            map[55, 25].interactable = false;
            map[55, 25].tiles = new List<TileParts>();
            map[55, 25].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[55, 25].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(4, 2), new Vector2(6, 13)));

            map[56, 25].currentEvent = new Battle01();
            (map[56, 25].currentEvent as Battle01).InitializePotentials(this, main);
            map[56, 25].walkable = true;
            map[56, 25].interactable = false;
            map[56, 25].tiles = new List<TileParts>();
            map[56, 25].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(1, 1), new Vector2(6, 13)));
            map[56, 25].tiles.Add(new TileParts("World\\Tilesets\\Outside", new Vector2(5, 2), new Vector2(6, 13)));

            //World Map Initialization Ends//

            //using (FileStream stream = File.Open("C:\\Users\\Nye\\Dropbox\\Programming\\C#\\Programs\\RPG-Game\\Saves\\TestMap.bin", FileMode.Create))
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(stream, map);
            //}

            //using (FileStream stream = File.Open("C:\\Users\\Nye\\Dropbox\\Programming\\C#\\Programs\\RPG-Game\\Saves\\TestMap.bin", FileMode.Open))
            //{
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    map = (Tile[,])formatter.Deserialize(stream);
            //}
        }

        private void menuBoxInitialise(Main main)
        {
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
        }

        private void inventoryBoxInitialize(Main main)
        {
            //Inventory Box
            inventoryBox = new Box();
            inventoryBox.frameWidth = 800;
            inventoryBox.frameHeight = 800;
            inventoryBox.UpperLeft = new Vector2(menuBox.GetWidth() + 10, 5);
            inventoryBox.SetParts(cornerTexture, wallTexture, backTexture);
            inventoryBox.activatorState = 3;
            inventoryBox.buttons = new List<Button>();
            allBoxes.Add(inventoryBox);
        }

        private void skillsBoxInitialise(Main main)
        {
            skillsBox = new Box();
            skillsBox.frameWidth = 800;
            skillsBox.frameHeight = 800;
            skillsBox.UpperLeft = new Vector2(menuBox.GetWidth() + 10, 5);
            skillsBox.SetParts(cornerTexture, wallTexture, backTexture);
            skillsBox.activatorState = 4;
            skillsBox.buttons = new List<Button>();
            allBoxes.Add(skillsBox);
        }

        private void heroBoxInitialize(Main main)
        {
            heroBox = new MultiBox();
            heroBox.frameWidth = 820;
            heroBox.frameHeight = 800;
            heroBox.UpperLeft = new Vector2(main.graphics.PreferredBackBufferWidth - heroBox.GetWidth() - 5, 5);
            heroBox.SetParts(cornerTexture, wallTexture, backTexture);
            heroBox.activatorState = 6;
            heroBox.multiButtons = new List<MultiButton>();
            heroBox.buttons = new List<Button>();
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
            multiButton.extraButtons[0].frameWidth = 490;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Adorable Manchild";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 85;
            multiButton.extraButtons[1].frameHeight = 50;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth(), multiButton.extraButtons[0].UpperLeft.Y);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "L" + hiroBattler.Level;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 45;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = "HP:";
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[3].frameWidth = 115;
            multiButton.extraButtons[3].frameHeight = 42;
            multiButton.extraButtons[3].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[3].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[3].display = heroBattler.health.ToString() + "/" + heroBattler.maxHealth.ToString();
            multiButton.extraButtons[3].displaySize = 0.75f;
            multiButton.extraButtons[3].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[4].frameWidth = 45;
            multiButton.extraButtons[4].frameHeight = 42;
            multiButton.extraButtons[4].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[4].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[4].display = "MP:";
            multiButton.extraButtons[4].displaySize = 0.75f;
            multiButton.extraButtons[4].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[5].frameWidth = 115;
            multiButton.extraButtons[5].frameHeight = 42;
            multiButton.extraButtons[5].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[5].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[5].display = heroBattler.mana.ToString() + "/" + heroBattler.maxMana.ToString();
            multiButton.extraButtons[5].displaySize = 0.75f;
            multiButton.extraButtons[5].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[6].frameWidth = 45;
            multiButton.extraButtons[6].frameHeight = 42;
            multiButton.extraButtons[6].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 6, multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[6].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[6].display = "XP:";
            multiButton.extraButtons[6].displaySize = 0.75f;
            multiButton.extraButtons[6].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[7].frameWidth = 115;
            multiButton.extraButtons[7].frameHeight = 42;
            multiButton.extraButtons[7].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[7].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[7].display = heroBattler.XP.ToString() + "/" + heroBattler.XPToLevel.ToString();
            multiButton.extraButtons[7].displaySize = 0.75f;
            multiButton.extraButtons[7].icon = null;

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
            multiButton.extraButtons[0].frameWidth = 490;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Absolutely-Not-Into-It Love Interest";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 85;
            multiButton.extraButtons[1].frameHeight = 50;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth(), multiButton.extraButtons[0].UpperLeft.Y);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "L" + hiroBattler.Level;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 45;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = "HP:";
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[3].frameWidth = 115;
            multiButton.extraButtons[3].frameHeight = 42;
            multiButton.extraButtons[3].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[3].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[3].display = hiroBattler.health.ToString() + "/" + hiroBattler.maxHealth.ToString();
            multiButton.extraButtons[3].displaySize = 0.75f;
            multiButton.extraButtons[3].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[4].frameWidth = 45;
            multiButton.extraButtons[4].frameHeight = 42;
            multiButton.extraButtons[4].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[4].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[4].display = "MP:";
            multiButton.extraButtons[4].displaySize = 0.75f;
            multiButton.extraButtons[4].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[5].frameWidth = 115;
            multiButton.extraButtons[5].frameHeight = 42;
            multiButton.extraButtons[5].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[5].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[5].display = hiroBattler.mana.ToString() + "/" + hiroBattler.maxMana.ToString();
            multiButton.extraButtons[5].displaySize = 0.75f;
            multiButton.extraButtons[5].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[6].frameWidth = 45;
            multiButton.extraButtons[6].frameHeight = 42;
            multiButton.extraButtons[6].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 6, multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[6].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[6].display = "XP:";
            multiButton.extraButtons[6].displaySize = 0.75f;
            multiButton.extraButtons[6].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[7].frameWidth = 115;
            multiButton.extraButtons[7].frameHeight = 42;
            multiButton.extraButtons[7].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[7].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[7].display = hiroBattler.XP.ToString() + "/" + hiroBattler.XPToLevel.ToString();
            multiButton.extraButtons[7].displaySize = 0.75f;
            multiButton.extraButtons[7].icon = null;

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
            multiButton.extraButtons[0].frameWidth = 490;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Endearing Father Figure";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 85;
            multiButton.extraButtons[1].frameHeight = 50;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth(), multiButton.extraButtons[0].UpperLeft.Y);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "L" + hiroBattler.Level;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 45;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = "HP:";
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[3].frameWidth = 115;
            multiButton.extraButtons[3].frameHeight = 42;
            multiButton.extraButtons[3].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[3].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[3].display = hearoBattler.health.ToString() + "/" + hearoBattler.maxHealth.ToString();
            multiButton.extraButtons[3].displaySize = 0.75f;
            multiButton.extraButtons[3].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[4].frameWidth = 45;
            multiButton.extraButtons[4].frameHeight = 42;
            multiButton.extraButtons[4].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[4].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[4].display = "MP:";
            multiButton.extraButtons[4].displaySize = 0.75f;
            multiButton.extraButtons[4].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[5].frameWidth = 115;
            multiButton.extraButtons[5].frameHeight = 42;
            multiButton.extraButtons[5].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[5].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[5].display = hearoBattler.mana.ToString() + "/" + hearoBattler.maxMana.ToString();
            multiButton.extraButtons[5].displaySize = 0.75f;
            multiButton.extraButtons[5].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[6].frameWidth = 45;
            multiButton.extraButtons[6].frameHeight = 42;
            multiButton.extraButtons[6].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 6, multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[6].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[6].display = "XP:";
            multiButton.extraButtons[6].displaySize = 0.75f;
            multiButton.extraButtons[6].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[7].frameWidth = 115;
            multiButton.extraButtons[7].frameHeight = 42;
            multiButton.extraButtons[7].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[7].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[7].display = hearoBattler.XP.ToString() + "/" + hearoBattler.XPToLevel.ToString();
            multiButton.extraButtons[7].displaySize = 0.75f;
            multiButton.extraButtons[7].icon = null;

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
            multiButton.extraButtons[0].frameWidth = 490;
            multiButton.extraButtons[0].frameHeight = 50;
            multiButton.extraButtons[0].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y);
            multiButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[0].display = "The Comic Relief";
            multiButton.extraButtons[0].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[1].frameWidth = 85;
            multiButton.extraButtons[1].frameHeight = 50;
            multiButton.extraButtons[1].UpperLeft = new Vector2(multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth(), multiButton.extraButtons[0].UpperLeft.Y);
            multiButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[1].display = "L" + hiroBattler.Level;
            multiButton.extraButtons[1].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[2].frameWidth = 45;
            multiButton.extraButtons[2].frameHeight = 42;
            multiButton.extraButtons[2].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[2].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[2].display = "HP:";
            multiButton.extraButtons[2].displaySize = 0.75f;
            multiButton.extraButtons[2].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[3].frameWidth = 115;
            multiButton.extraButtons[3].frameHeight = 42;
            multiButton.extraButtons[3].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 50);
            multiButton.extraButtons[3].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[3].display = hieroBattler.health.ToString() + "/" + hieroBattler.maxHealth.ToString();
            multiButton.extraButtons[3].displaySize = 0.75f;
            multiButton.extraButtons[3].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[4].frameWidth = 45;
            multiButton.extraButtons[4].frameHeight = 42;
            multiButton.extraButtons[4].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 5, multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[4].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[4].display = "MP:";
            multiButton.extraButtons[4].displaySize = 0.75f;
            multiButton.extraButtons[4].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[5].frameWidth = 115;
            multiButton.extraButtons[5].frameHeight = 42;
            multiButton.extraButtons[5].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 90);
            multiButton.extraButtons[5].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[5].display = hieroBattler.mana.ToString() + "/" + hieroBattler.maxMana.ToString();
            multiButton.extraButtons[5].displaySize = 0.75f;
            multiButton.extraButtons[5].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[6].frameWidth = 45;
            multiButton.extraButtons[6].frameHeight = 42;
            multiButton.extraButtons[6].UpperLeft = new Vector2(multiButton.UpperLeft.X + multiButton.GetWidth() + 6, multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[6].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[6].display = "XP:";
            multiButton.extraButtons[6].displaySize = 0.75f;
            multiButton.extraButtons[6].icon = null;

            multiButton.extraButtons.Add(new Button());
            multiButton.extraButtons[7].frameWidth = 115;
            multiButton.extraButtons[7].frameHeight = 42;
            multiButton.extraButtons[7].UpperLeft = new Vector2((multiButton.extraButtons[0].UpperLeft.X + multiButton.extraButtons[0].GetWidth()) - 30,
                                                                multiButton.UpperLeft.Y + 130);
            multiButton.extraButtons[7].SetParts(cornerTexture, wallTexture, backTexture);
            multiButton.extraButtons[7].display = hieroBattler.XP.ToString() + "/" + hieroBattler.XPToLevel.ToString();
            multiButton.extraButtons[7].displaySize = 0.75f;
            multiButton.extraButtons[7].icon = null;

            heroBox.multiButtons.Add(multiButton);

            int temp = multiButton.GetHeight();

            Button tempSingle = new Button();

            //Money MultiButton
            tempSingle.frameWidth = 200;
            tempSingle.frameHeight = 50;
            tempSingle.UpperLeft = new Vector2(heroBox.UpperLeft.X + 80, 25 + ((temp + 25) * heroBox.multiButtons.Count));
            tempSingle.SetParts(cornerTexture, wallTexture, backTexture);
            tempSingle.display = gold.ToString() + "G";
            tempSingle.icon.SetTexture(iconTexture, 16, 20);
            tempSingle.icon.setCurrentFrame(10, 19);
            tempSingle.icon.UpperLeft = new Vector2(tempSingle.UpperLeft.X + 10, tempSingle.UpperLeft.Y + 9);

            heroBox.buttons.Add(tempSingle);
        }

        private void stateInitialize(Main main)
        {
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
            switchStateMethods[4] = SkillsMenuSwitch;
            switchStateMethods[5] = MovementSwitch;
            switchStateMethods[6] = TargetSwitch;
            switchStateMethods[7] = StateSwitch;

            targetState = 1;
        }

        public override void LoadContent(Main main)
        {
            gold = main.gold;

            allItems = main.heldItems;
            
            activeButtons = new List<Button>();
            drawButtons = new List<Button>();

            textureInitialize(main);

            pointerInitialize(main);

            cameraInitialize(main);

            moverInitialise(main);

            battlerInitialise(main);

            mapInitialize(main);

            menuBoxInitialise(main);

            inventoryBoxInitialize(main);

            skillsBoxInitialise(main);

            heroBoxInitialize(main);

            stateInitialize(main);
        }


        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            for (int i = 0; i < movers.Count; i++)
            {
                movers[i].Animate(gameTime);

                if (currentState != 0)
                {
                    if (movers[i].movementModifier != new Vector2(0, 0))
                    {
                        movers[i].Move(gameTime, map);
                    }
                    else
                    {
                        movers[i].timeSinceStop = gameTime.TotalGameTime.TotalSeconds - movers[i].timerDifference;
                    }
                }
            }

            stateMethods[currentState].Invoke();
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

            for(int i = 0; i < movers.Count; i++)
            {
                movers[i].Draw(spriteBatch, camera.UpperLeft);
            }

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
                else if(eventMover != null)
                {
                    eventMover.DrawAll(spriteBatch, calibri);
                }
                else if (currentEvent != null)
                {
                    currentEvent.DrawAll(spriteBatch, calibri);
                }
            }

            bool temp = false;

            //Boxes \ Buttons Draw Cycle
            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (state[allBoxes[i].activatorState])
                {
                    allBoxes[i].DrawParts(spriteBatch);

                    int skipIndex = 0;

                    for (int o = 0; o < allBoxes[i].GetButtons().Count; o++)
                    {
                        temp = false;

                        if (allBoxes[i].GetButtons()[o].selectable == false)
                        {
                            skipIndex++;
                        }
                        else if (allBoxes[i].activatorState == currentState && o - skipIndex == buttonIndex)
                        {
                            temp = true;
                        }

                        allBoxes[i].GetButtons()[o].DrawParts(spriteBatch, calibri, temp);
                    }
                }
            }
            pointer.Draw(spriteBatch);
        }


        public override void ReInitialize(StateManager state, Main main)
        {
            if(state is BattleState)
            {
                BattleState tempState = (BattleState)state;

                double goldIncrease = 0;

                for (int i = 0; i < tempState.enemies.Count; i++)
                {
                    double temp;

                    temp = rand.Next(0, 101);
                    temp += 50;
                    temp /= 100;
                    temp *= tempState.enemies[i].goldYield;

                    goldIncrease += temp;

                    for (int o = 0; o < tempState.heroes.Count; o++)
                    {
                        temp = rand.Next(0, 101);
                        temp += 50;
                        temp /= 100;
                        temp *= tempState.enemies[i].XPYield;

                        tempState.heroes[i].XP += (int)Math.Round(temp, 0, MidpointRounding.AwayFromZero);

                        while (tempState.heroes[i].XP > tempState.heroes[i].XPToLevel)
                        {
                            tempState.heroes[i].Level++;

                            tempState.heroes[i].XP -= tempState.heroes[i].XPToLevel;

                            tempState.heroes[i].XPToLevel = (int)Math.Round(tempState.heroes[i].XPToLevel * 1.5, 0, MidpointRounding.AwayFromZero);
                        }
                    }

                    if (tempState.enemies[i].potentialDrops != null)
                    {
                        int result = rand.Next(0, 101);
                        int rollOver = 0;
                        
                        for (int o = 0; o < tempState.enemies[i].potentialDrops.Count; o++)
                        {
                            if (result > rollOver && result < tempState.enemies[i].potentialDrops[o].proportion + rollOver)
                            {
                                result = rand.Next(0, 101);
                                rollOver = 0;

                                for (int p = 0; p < tempState.enemies[i].potentialDrops[o].potentialCounts.Count; p++)
                                {
                                    if (result > rollOver && result < tempState.enemies[i].potentialDrops[o].potentialCounts[p].proportion + rollOver)
                                    {
                                        tempState.enemies[i].potentialDrops[o].item.heldCount += tempState.enemies[i].potentialDrops[o].potentialCounts[p].count;

                                        if(tempState.enemies[i].potentialDrops[o].item.heldCount > tempState.enemies[i].potentialDrops[o].item.maxStack)
                                        {
                                            tempState.enemies[i].potentialDrops[o].item.heldCount = tempState.enemies[i].potentialDrops[o].item.maxStack;
                                        }

                                        break;
                                    }

                                    rollOver += tempState.enemies[i].potentialDrops[o].potentialCounts[p].proportion;
                                }

                                break;
                            }

                            rollOver += tempState.enemies[i].potentialDrops[o].proportion;
                        }
                    }
                }

                gold += (int)Math.Round(goldIncrease, 0, MidpointRounding.AwayFromZero);

                if(eventMover != null)
                {
                    (eventMover as BattleMover).InitializeTroops(this, main);

                    eventMover = null;
                }
                if(eventTile != null)
                {
                    (eventTile.currentEvent as BattleEvent).InitializePotentials(this, main);

                    eventTile = null;
                }

                potentialEnemies = null;
                potentialTroops = null;
            }
        }


        public void Movement()
        {
            if (pointer.isAlive)
            {
                pointer.isAlive = false;
            }

            if (heroMover.movementModifier != new Vector2(0, 0))
            {
                if (step.Invoke(gameTime, heroMover.timer, 2, 0.4, heroMover, heroMover.movementModifier))
                {
                    if (heroMover.UpperLeft != heroMover.gridPosition * 48)
                    {
                        heroMover.UpperLeft = heroMover.gridPosition * 48;
                    }

                    if(map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y].currentEvent != null)
                    {
                        ActivateState(1);

                        eventTile = map[(int)heroMover.gridPosition.X, (int)heroMover.gridPosition.Y];
                    }

                    heroMover.movementModifier = new Vector2(0, 0);
                }
            }
            else
            {
                if (upInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.MoveOnce(new Vector2(0, -1), map))
                    {
                        heroMover.StartAnimationShort(gameTime, 9, 11, 10);
                    }
                    else
                    {
                        heroMover.setCurrentFrame(1, 3);
                    }

                    heroMover.timer = gameTime.TotalGameTime.TotalSeconds;
                }
                else if (downInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.MoveOnce(new Vector2(0, 1), map))
                    {
                        heroMover.StartAnimationShort(gameTime, 0, 2, 1);
                    }
                    else
                    {
                        heroMover.setCurrentFrame(1, 0);
                    }

                    heroMover.timer = gameTime.TotalGameTime.TotalSeconds;
                }
                else if (leftInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.MoveOnce(new Vector2(-1, 0), map))
                    {
                        heroMover.StartAnimationShort(gameTime, 3, 5, 4);
                    }
                    else
                    {
                        heroMover.setCurrentFrame(1, 1);
                    }

                    heroMover.timer = gameTime.TotalGameTime.TotalSeconds;
                }
                else if (rightInput.inputState == Input.inputStates.held)
                {
                    if (heroMover.MoveOnce(new Vector2(1, 0), map))
                    {
                        heroMover.StartAnimationShort(gameTime, 6, 8, 7);
                    }
                    else
                    {
                        heroMover.setCurrentFrame(1, 2);
                    }

                    heroMover.timer = gameTime.TotalGameTime.TotalSeconds;
                }


                if (activateInput.inputState == Input.inputStates.pressed)
                {
                    if (map[(int)(heroMover.gridPosition.X + heroMover.lastMoved.X), (int)(heroMover.gridPosition.Y + heroMover.lastMoved.Y)].occupied)
                    {
                        ActivateState(1);

                        eventMover = (EventMover)map[(int)(heroMover.gridPosition.X + heroMover.lastMoved.X), (int)(heroMover.gridPosition.Y + heroMover.lastMoved.Y)].occupier;
                    }
                    else if (map[(int)(heroMover.gridPosition.X + heroMover.lastMoved.X), (int)(heroMover.gridPosition.Y + heroMover.lastMoved.Y)].interactable)
                    {
                        ActivateState(1);

                        eventTile = map[(int)(heroMover.gridPosition.X + heroMover.lastMoved.X), (int)(heroMover.gridPosition.Y + heroMover.lastMoved.Y)];
                    }
                }
                if (menuInput.inputState == Input.inputStates.pressed)
                {
                    ActivateState(6);

                    MenuSwitch();
                }
            }

            for(int i = 0; i < movers.Count; i++)
            {
                movers[i].Move(gameTime, map);
            }

            camera.UpperLeft = new Vector2((heroMover.UpperLeft.X + heroMover.GetWidth() / 2) - (camera.ViewWidth / 2),
                                           (heroMover.UpperLeft.Y + heroMover.GetHeight() / 2) - (camera.ViewHeight / 2));
        }

        private void Event()
        {
            if (currentAction != null)
            {
                currentAction.Call(gameTime, this);
            }
            else if(eventMover != null)
            {
                eventMover.Call(gameTime, this);
            }
            else
            {
                eventTile.currentEvent.Call(gameTime, this);
            }
        }

        private void Menu()
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

            if (temp.activate)
            {
                activeButtons[buttonIndex].action.Invoke();
            }
            else if (temp.menu)
            {
                SwitchState(2, gameTime);
            }
        }

        private void Inventory()
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

            if (temp.activate)
            {
                if (heldItems[buttonIndex].mapUsable)
                {
                    TargetSwitch();

                    currentAction = heldItems[buttonIndex];
                }
            }
            else if (temp.menu)
            {
                SwitchState(3, gameTime);
            }
        }

        private void Skills()
        {
            MenuUpdateReturn temp = MenuUpdate();
            buttonIndex = temp.index;

            pointer.UpperLeft = new Vector2(activeButtons[buttonIndex].UpperLeft.X - pointer.GetWidth() - 15, activeButtons[buttonIndex].UpperLeft.Y + 5);

            if (temp.activate)
            {
                if (actor.mana > actor.abilities[buttonIndex].cost)
                {
                    if (actor.abilities[buttonIndex].mapUsable)
                    {
                        TargetSwitch();

                        currentAction = actor.abilities[buttonIndex];
                    }
                }
            }
            else if (temp.menu)
            {
                SwitchState(4, gameTime);
                TargetSwitch();
            }
        }

        private void Map()
        {

        }

        private void Status()
        {

        }

        private void Target()
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

                SwitchState(6, gameTime);
                int i = currentState;
                SwitchState(6, gameTime);
                currentState = i;
            }
        }

        private void StateSwitch()
        {

        }


        private void MovementSwitch()
        {
            ClearStates();
            ActivateState(0);
        }

        private void MenuSwitch()
        {
            SwitchState(2, gameTime);

            activeButtons.Clear();

            for (int i = 0; i < allBoxes.Count; i++)
            {
                if (allBoxes[i].activatorState == currentState)
                {
                    for (int o = 0; o < allBoxes[i].buttons.Count; o++)
                    {
                        if(allBoxes[i].buttons[o].selectable)
                        {
                            activeButtons.Add(allBoxes[i].buttons[o]);
                        }
                        else
                        {
                            drawButtons.Add(allBoxes[i].buttons[o]);
                        }
                    }
                }
            }

            for (int i = 0; i < heroBox.multiButtons.Count; i++)
            {
                if (heroBox.multiButtons[i].selectable)
                {
                    heroBox.multiButtons[i].extraButtons[1].display = "L" + battlers[i].Level;

                    heroBox.multiButtons[i].extraButtons[3].display = battlers[i].health.ToString() + "/" + battlers[i].maxHealth.ToString();

                    heroBox.multiButtons[i].extraButtons[5].display = battlers[i].mana.ToString() + "/" + battlers[i].maxMana.ToString();

                    heroBox.multiButtons[i].extraButtons[7].display = battlers[i].XP.ToString() + "/" + battlers[i].XPToLevel.ToString();
                }
            }

            heroBox.buttons[0].display = gold + "G";
        }

        private void ItemSwitch()
        {
            SwitchState(3, gameTime);

            ItemRefresh();
        }

        public void ItemRefresh()
        {
            MultiButton tempButton;

            inventoryBox.buttons.Clear();
            drawButtons.Clear();

            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();

            tempButton.selectable = false;
            tempButton.UpperLeft = new Vector2(inventoryBox.UpperLeft.X + 80, inventoryBox.UpperLeft.Y + 10);
            tempButton.display = "Item";
            tempButton.frameWidth = inventoryBox.frameWidth - 165;
            tempButton.frameHeight = 50;
            tempButton.SetParts(cornerTexture, wallTexture, backTexture);
            for(int i = 0; i < tempButton.parts.Count; i++)
            {
                tempButton.parts[i].drawColour = Color.Navy;
            }
            tempButton.icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), inventoryBox.UpperLeft.Y + 10);
            tempButton.extraButtons[0].display = "Qty.";
            tempButton.extraButtons[0].frameWidth = 75;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            for (int i = 0; i < tempButton.extraButtons[0].parts.Count; i++)
            {
                tempButton.extraButtons[0].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[0].icon = null;
            
            tempButton.displayColour = Color.PaleGoldenrod;
            tempButton.extraButtons[0].displayColour = Color.PaleGoldenrod;

            inventoryBox.buttons.Add(tempButton);
            
            heldItems.Clear();

            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].heldCount != 0)
                {
                    heldItems.Add(allItems[i]);
                }
            }

            if (heldItems.Count == 0)
            {
                Item item = new Item();
                item.name = "--Empty, really empty--";
                item.description = "Looks like you don't have any items on you,\nmaybe this is a good time to stock up?";
                item.iconFrame = new Vector2(8, 10);
                heldItems.Add(item);
            }

            string tempString;

            for (int i = 0; i < heldItems.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(inventoryBox.UpperLeft.X + 80, inventoryBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.display = heldItems[i].name;
                tempButton.icon.SetTexture(iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)heldItems[i].iconFrame.X, (int)heldItems[i].iconFrame.Y);
                tempButton.frameWidth = inventoryBox.frameWidth - 165;
                tempButton.frameHeight = 50;
                tempButton.SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                if (heldItems[i].heldCount.ToString().Equals("-1"))
                {
                    tempString = "------";
                }
                else
                {
                    tempString = heldItems[i].heldCount.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), inventoryBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[0].display = tempString;
                tempButton.extraButtons[0].frameWidth = 75;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[0].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].display = heldItems[i].description;
                tempButton.extraButtons[1].frameWidth = inventoryBox.frameWidth - 90;
                tempButton.extraButtons[1].frameHeight = 100;
                tempButton.extraButtons[1].showOnSelected = true;
                tempButton.extraButtons[1].icon = null;

                if (!heldItems[i].mapUsable)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                }

                inventoryBox.buttons.Add(tempButton);
            }

            inventoryBox.frameHeight = (int)inventoryBox.buttons[inventoryBox.buttons.Count - 1].UpperLeft.Y + 60;
            inventoryBox.SetParts(cornerTexture, wallTexture, backTexture);

            activeButtons.Clear();
            drawButtons.Clear();

            for (int i = 0; i < inventoryBox.buttons.Count; i++)
            {
                if (inventoryBox.buttons[i].selectable)
                {
                    tempButton = (MultiButton)inventoryBox.buttons[i];

                    tempButton.extraButtons[1].UpperLeft = new Vector2(inventoryBox.UpperLeft.X + 80, inventoryBox.UpperLeft.Y + inventoryBox.frameHeight);
                    tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);

                    activeButtons.Add(tempButton);
                }
                else
                {
                    drawButtons.Add(inventoryBox.buttons[i]);
                }
            }

            buttonIndex = 0;
        }


        private void SkillsSwitch()
        {
            TargetSwitch();
            
            currentAction = new SkillsMenuSwitcher();
        }

        public void SkillsMenuSwitch()
        {
            //Switch to Skill Menu State
            SwitchState(4, gameTime);
            
            currentAction = new SkillsMenuSwitcher();

            SkillsRefresh();
        }

        public void SkillsRefresh()
        {
            MultiButton tempButton;

            skillsBox.buttons.Clear();
            drawButtons.Clear();

            tempButton = new MultiButton();
            tempButton.extraButtons = new List<Button>();

            tempButton.selectable = false;
            tempButton.UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, skillsBox.UpperLeft.Y + 10);
            tempButton.display = "Ability";
            tempButton.frameWidth = skillsBox.frameWidth - 165;
            tempButton.frameHeight = 50;
            tempButton.SetParts(cornerTexture, wallTexture, backTexture);
            for (int i = 0; i < tempButton.parts.Count; i++)
            {
                tempButton.parts[i].drawColour = Color.Navy;
            }
            tempButton.icon = null;

            tempButton.extraButtons.Add(new Button());
            tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), skillsBox.UpperLeft.Y + 10);
            tempButton.extraButtons[0].display = "Cost";
            tempButton.extraButtons[0].frameWidth = 75;
            tempButton.extraButtons[0].frameHeight = 50;
            tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
            for (int i = 0; i < tempButton.extraButtons[0].parts.Count; i++)
            {
                tempButton.extraButtons[0].parts[i].drawColour = Color.Navy;
            }
            tempButton.extraButtons[0].icon = null;

            tempButton.displayColour = Color.PaleGoldenrod;
            tempButton.extraButtons[0].displayColour = Color.PaleGoldenrod;

            skillsBox.buttons.Add(tempButton);

            for (int i = 0; i < actor.abilities.Count; i++)
            {
                if (actor.abilities[i].name.Equals("--Overwhelming Ineptitude--"))
                {
                    actor.abilities.RemoveAt(i);
                }
            }
            if (actor.abilities.Count == 0)
            {
                Ability ability = new Ability();
                ability.name = "--Overwhelming Ineptitude--";
                ability.description = actor.name + " doesn't seem very clever.\nThey don't know a single ability,\nand might not be much use in a fight.";
                ability.iconFrame = new Vector2(5, 0);
                actor.abilities.Add(ability);
            }

            string tempString;

            for (int i = 0; i < actor.abilities.Count; i++)
            {
                tempButton = new MultiButton();
                tempButton.extraButtons = new List<Button>();

                tempButton.UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, skillsBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.display = actor.abilities[i].name;
                tempButton.icon.SetTexture(iconTexture, 16, 20);
                tempButton.icon.setCurrentFrame((int)actor.abilities[i].iconFrame.X, (int)actor.abilities[i].iconFrame.Y);
                tempButton.frameWidth = skillsBox.frameWidth - 165;
                tempButton.frameHeight = 50;
                tempButton.SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.icon.UpperLeft = new Vector2(tempButton.UpperLeft.X + 10, tempButton.UpperLeft.Y + 9);

                if (actor.abilities[i].cost.ToString().Equals("-1"))
                {
                    tempString = "------";
                }
                else
                {
                    tempString = actor.abilities[i].cost.ToString();
                }

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[0].UpperLeft = new Vector2(tempButton.UpperLeft.X + tempButton.GetWidth(), skillsBox.UpperLeft.Y + 10 + (60 * (i + 1)));
                tempButton.extraButtons[0].display = tempString;
                tempButton.extraButtons[0].frameWidth = 75;
                tempButton.extraButtons[0].frameHeight = 50;
                tempButton.extraButtons[0].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[0].icon = null;

                tempButton.extraButtons.Add(new Button());
                tempButton.extraButtons[1].UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, 200);
                tempButton.extraButtons[1].display = actor.abilities[i].description;
                tempButton.extraButtons[1].frameWidth = skillsBox.frameWidth - 90;
                tempButton.extraButtons[1].frameHeight = 100;
                tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);
                tempButton.extraButtons[1].showOnSelected = true;
                tempButton.extraButtons[1].icon = null;


                if (actor.mana < actor.abilities[i].cost)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                }
                if (!actor.abilities[i].mapUsable)
                {
                    tempButton.displayColour = Color.OrangeRed;
                    tempButton.extraButtons[0].displayColour = Color.OrangeRed;
                }

                skillsBox.buttons.Add(tempButton);
            }

            skillsBox.frameHeight = (int)skillsBox.buttons[skillsBox.buttons.Count - 1].UpperLeft.Y + 60;
            skillsBox.SetParts(cornerTexture, wallTexture, backTexture);

            activeButtons.Clear();
            drawButtons.Clear();

            for (int i = 0; i < skillsBox.buttons.Count; i++)
            {
                if (skillsBox.buttons[i].selectable)
                {
                    tempButton = (MultiButton)skillsBox.buttons[i];

                    tempButton.extraButtons[1].UpperLeft = new Vector2(skillsBox.UpperLeft.X + 80, skillsBox.UpperLeft.Y + skillsBox.frameHeight);
                    tempButton.extraButtons[1].SetParts(cornerTexture, wallTexture, backTexture);

                    activeButtons.Add(tempButton);
                }
                else
                {
                    drawButtons.Add(skillsBox.buttons[i]);
                }
            }

            buttonIndex = 0;
        }


        private void MapSwitch()
        {

        }

        private void StatusSwitch()
        {

        }

        public void TargetSwitch()
        {
            currentState = 6;

            TargetRefresh();
        }

        public void TargetRefresh()
        {
            activeButtons = new List<Button>();

            for (int i = 0; i < heroBox.multiButtons.Count; i++)
            {
                if(heroBox.multiButtons[i].selectable)
                {
                    heroBox.multiButtons[i].extraButtons[1].display = "L" + battlers[i].Level;

                    heroBox.multiButtons[i].extraButtons[3].display = battlers[i].health.ToString() + "/" + battlers[i].maxHealth.ToString();

                    heroBox.multiButtons[i].extraButtons[5].display = battlers[i].mana.ToString() + "/" + battlers[i].maxMana.ToString();

                    heroBox.multiButtons[i].extraButtons[7].display = battlers[i].XP.ToString() + "/" + battlers[i].XPToLevel.ToString();

                    activeButtons.Add(heroBox.multiButtons[i]);
                }
            }
        }


        public void BattleBegin(List<PotentialEnemy> enemies)
        {
            finished = true;

            potentialEnemies = new List<PotentialEnemy>();

            for(int i = 0; i < enemies.Count; i++)
            {
                potentialEnemies.Add(enemies[i]);
            }

            ActivateState(0);
        }
        public void BattleBegin(List<Troop> troops)
        {
            finished = true;

            potentialTroops = new List<Troop>();

            for (int i = 0; i < troops.Count; i++)
            {
                potentialTroops.Add(troops[i]);
            }

            ActivateState(0);
        }

        private void Exit()
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


        internal bool Type(TypingStrings strings, GameTime gameTime)
        {
            return Type(strings, gameTime, 0.05);
        }
        internal bool Type(TypingStrings strings, GameTime gameTime, double typeSpeed)
        {
            if (strings.lines.Count == 0)
            {
                pointer.Scale = new Vector2(0.8f, 0.8f);

                return true;
            }

            if (strings.line.Length >= strings.previousLines.Length)
            {
                strings.line = strings.line.Substring(strings.previousLines.Length);
            }

            if (strings.line.Equals(strings.lines[0]))
            {
                timer = gameTime.TotalGameTime.TotalSeconds;

                pointer.isAlive = true;

                if (activateInput.inputState == Input.inputStates.pressed)
                {
                    if (strings.lines[0].EndsWith("\n") && strings.lines.Count > 1)
                    {
                        strings.previousLines += strings.lines[0];
                        strings.lines.RemoveAt(0);
                        strings.line = "";
                    }
                    else
                    {
                        strings.lines.RemoveAt(0);
                        strings.line = "";
                        strings.previousLines = "";
                    }
                }
            }
            else
            {
                pointer.isAlive = false;
            }

            if (gameTime.TotalGameTime.TotalSeconds < timer + typeSpeed)
            {
                strings.line = strings.previousLines + strings.line;

                return false;
            }

            if (strings.line.Length + 1 < strings.lines[0].Length)
            {
                string i = strings.lines[0].Substring(strings.line.Length);
                if (i.StartsWith(" "))
                {
                    int o = 0;

                    while (i.StartsWith(" "))
                    {
                        i = strings.lines[0].Substring(strings.line.Length + o);

                        o++;
                    }

                    strings.line = strings.lines[0].Remove(strings.line.Length + o);
                }
                else
                {
                    strings.line = strings.lines[0].Remove(strings.line.Length + 1);
                }
            }
            else
            {
                strings.line = strings.lines[0];
            }

            strings.line = strings.previousLines + strings.line;

            timer = gameTime.TotalGameTime.TotalSeconds;

            return false;
        }
    }
}