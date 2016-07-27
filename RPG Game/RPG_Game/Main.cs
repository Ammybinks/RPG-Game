using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace RPG_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rand = new Random();
        
        Vector2 screenSize;

        public List<Item> heldItems = new List<Item>();
        HP hPotion = new HP();
        HPPlus hPotionPlus = new HPPlus();
        HPPlusPlus hPotionPlusPlus = new HPPlusPlus();

        int currentState = 1;
        List<StateManager> states = new List<StateManager>();

        NaviState naviState = new NaviState();
        BattleState battleState = new BattleState();
        
        public Battler hero;
        
        public Battler hiro;
        
        public Battler hearo;
        
        public Battler hiero;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenSize = new Vector2(1920, 1080);
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            hPotion.heldCount = 5;
            heldItems.Add(hPotion);

            hPotionPlus.heldCount = 5;
            heldItems.Add(hPotionPlus);

            hPotionPlusPlus.heldCount = 5;
            heldItems.Add(hPotionPlusPlus);

            //Heroes Initialization Begins//
            //Hero Initialization
            hero = new Battler();

            //Hiro Initialization
            hiro = new Battler();

            //Hearo Initialization
            hearo = new Battler();

            //Hiero Initialization
            hiero = new Battler();
            //Heroes Initialization Ends//

            battleState.LoadContent(this);
            naviState.LoadContent(this);

            states.Add(naviState);
            states.Add(battleState);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param display="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            for(int i = 0; i < states.Count; i++)
            {
                if(i == currentState)
                {
                    states[i].GetInput(gameTime);
                    states[i].Update(gameTime);

                    currentState = states[i].targetState;
                    states[i].targetState = i;

                    if(states[i].quitting)
                    {
                        Exit();
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param display="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].targetState == currentState)
                {
                    states[i].Draw(spriteBatch, this);
                }
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}