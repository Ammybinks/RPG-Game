using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    class Shantae : Shopkeep
    {
        private HP hPotion = new HP();
        private HPPlus hPotionPlus = new HPPlus();
        private HPPlusPlus hPotionPlusPlus = new HPPlusPlus();

        public Shantae()
        {
            hPotion.heldCount = -1;
            allItems.Add(hPotion);

            hPotionPlus.heldCount = 20;
            allItems.Add(hPotionPlus);

            hPotionPlusPlus.heldCount = 5;
            allItems.Add(hPotionPlusPlus);
        }
        
        public override void Call(GameTime gameTime, NaviState naviState)
        {
            if (typingStrings == null)
            {
                Initialize(naviState);

                InitializeItems(naviState);

                //typingStrings.lines.Add("I'm supposed to be a shopkeeper.\n\n");
                //typingStrings.lines.Add("But really, I'm only here because Nye\nthinks I look weirdly like Shantae.");
                //typingStrings.lines.Add("What do you think? Do you agree with him?");
                typingStrings.lines.Add("What can I do for you?");

                previousState = 0;
                state = 0;
                
                index = 0;
            }

            if (state == 0)
            {
                Idle(naviState, gameTime);
            }
            else if (state == 1)
            {
                ChoiceMenu(naviState, gameTime);
            }
            else if(state == 2)
            {
                BuyMenu(naviState);
            }
            else if (state == 3)
            {
                SellMenu(naviState);
            }
            else if (state == 4)
            {
                ConfirmMenu(naviState, gameTime);
            }
        }


        internal override void Idle(NaviState naviState, GameTime gameTime)
        {
            if (typingStrings.line == "What do you think? Do you agree with him?")
            {
                Choice1(naviState);
            }
            else if (typingStrings.line == "So? What do you want?" || typingStrings.line == "Well, whatever. How can I help you?" || typingStrings.line == "What can I do for you?")
            {
                Choice2(naviState);
            }
            else
            {
                if (naviState.Type(typingStrings, gameTime, 0.01))
                {
                    Complete(naviState);

                    typingStrings = new TypingStrings();
                    typingStrings.lines = new List<string>();
                    typingStrings.lines.Add("What can I do for you?");
                    typingStrings.line = "";
                    typingStrings.previousLines = "";
                }
            }
        }
        

        private void Choice1(NaviState naviState)
        {
            SwitchState(1);
            
            naviState.pointer.isAlive = true;
            naviState.pointer.Scale = new Vector2(0.8f, 0.8f);
            naviState.pointer.RotationAngle = 180;

            EventButton button;

            button = new EventButton();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(box.UpperLeft.X + box.frameWidth + 5,
                                           box.UpperLeft.Y);
            button.icon = null;
            button.display = "Of course";
            button.action = Choice1Decision1;
            button.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            box.buttons.Add(button);

            button = new EventButton();
            button.frameHeight = 50;
            button.frameWidth = 150;
            button.UpperLeft = new Vector2(box.UpperLeft.X + box.frameWidth + 5,
                                           box.UpperLeft.Y + 55);
            button.icon = null;
            button.display = "Fuuuuck no";
            button.action = Choice1Decision2;
            button.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            box.buttons.Add(button);
        }
        private void Choice1Decision1(NaviState naviState, GameTime gameTime)
        {
            SwitchState(0);

            PointerReset(naviState);

            typingStrings.lines.Clear();
            typingStrings.lines.Add("It's as if neither of you have seen what Shantae looks like\nin ages, so I'll reiterate.\n\n");
            typingStrings.lines.Add("WE LOOK NOTHING ALIKE, the hair is all wrong,\nthe colour should make that clear enough.");
            typingStrings.lines.Add("So? What did you want?");
            typingStrings.line = "";
            typingStrings.previousLines = "";

            box.buttons.Clear();
        }
        private void Choice1Decision2(NaviState naviState, GameTime gameTime)
        {

            SwitchState(0);

            PointerReset(naviState);

            typingStrings.lines.Clear();
            typingStrings.lines.Add("THANK you, he's been calling me 'Shanty' all week,\nit's the worst.\n\n");
            typingStrings.lines.Add("Maybe you could convince him to google it, so that he isn't\nalways comparing me to her like a total dumbass?");
            typingStrings.lines.Add("Well, whatever. How can I help you?");
            typingStrings.line = "";
            typingStrings.previousLines = "";

            box.buttons.Clear();

        }


        private void Choice2(NaviState naviState)
        {
            SwitchState(1);
            
            naviState.pointer.Scale = new Vector2(0.8f, 0.8f);
            naviState.pointer.RotationAngle = 180;

            box.buttons.Clear();

            EventButton button;

            button = new EventButton();
            button.frameHeight = 50;
            button.frameWidth = 100;
            button.UpperLeft = new Vector2(box.UpperLeft.X + box.frameWidth + 5,
                                           box.UpperLeft.Y);
            button.icon = null;
            button.display = "Buy";
            button.action = Choice2Decision1;
            button.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            box.buttons.Add(button);

            button = new EventButton();
            button.frameHeight = 50;
            button.frameWidth = 100;
            button.UpperLeft = new Vector2(box.UpperLeft.X + box.frameWidth + 5,
                                           box.UpperLeft.Y + 55);
            button.icon = null;
            button.display = "Sell";
            button.action = Choice2Decision2;
            button.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            box.buttons.Add(button);

            button = new EventButton();
            button.frameHeight = 50;
            button.frameWidth = 100;
            button.UpperLeft = new Vector2(box.UpperLeft.X + box.frameWidth + 5,
                                           box.UpperLeft.Y + 110);
            button.icon = null;
            button.display = "Leave";
            button.action = Choice2Decision3;
            button.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);
            box.buttons.Add(button);
        }
        private void Choice2Decision1(NaviState naviState, GameTime gameTime)
        {
            naviState.pointer.RotationAngle = 0;
            naviState.pointer.Scale = new Vector2(0.8f, 0.8f);
            naviState.pointer.isAlive = false;

            InitializeBoxes(naviState);

            BuyRefresh(naviState);

            SwitchState(2);
        }
        private void Choice2Decision2(NaviState naviState, GameTime gameTime)
        {
            naviState.pointer.RotationAngle = 0;
            naviState.pointer.Scale = new Vector2(0.8f, 0.8f);
            naviState.pointer.isAlive = false;

            InitializeBoxes(naviState);

            SellRefresh(naviState);

            SwitchState(3);
        }
        private void Choice2Decision3(NaviState naviState, GameTime gameTime)
        {
            SwitchState(0);

            PointerReset(naviState);

            typingStrings.lines.Clear();
            typingStrings.lines.Add("Be seeing you.");
            typingStrings.line = "";
            typingStrings.previousLines = "";

            box.buttons.Clear();
        }
    }
}
