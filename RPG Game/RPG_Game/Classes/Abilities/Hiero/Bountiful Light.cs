using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    class Bountiful_Light : Ability
    {
        internal Box box;

        internal string display;

        public Bountiful_Light()
        {
            iconFrame = new Vector2(6, 4);

            cost = 20;

            battleUsable = true;
            mapUsable = true;

            name = "Bountiful Light";
            description = "This spell's healing capacity is second only to its beauty.\n\nRestores 250HP and looks great while doing so.";
        }

        internal override List<SpriteBase> GetTargets(BattleState battleState)
        {
            List<SpriteBase> temp = new List<SpriteBase>();

            for(int i = 0; i < battleState.heroes.Count; i++)
            {
                temp.Add(battleState.heroes[i]);
            }

            return temp;
        }

        public override void DrawAll(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (box != null)
            {
                box.DrawParts(spriteBatch);

                spriteBatch.DrawString(spriteFont,
                                       display,
                                       new Vector2(580, 900),
                                       Color.Black);
            }
        }

        public override bool Call(GameTime gameTime, BattleState battleState)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= battleState.timer + 1.5)
            {
                //Reset all values ready for continuing the fight
                battleState.damageDealt = 0;
                battleState.damageLocation = 0;
                battleState.timer = 0;

                //Make sure our actor isn't moving or animating and is in the correct position
                battleState.actor.UpperLeft = battleState.actor.battleOrigin;
                battleState.actor.setCurrentFrame(0, 0);
                battleState.actor.animationShortStarted = false;
                
                return true;
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= battleState.timer + 1.25)
            {
                battleState.actor.animationShortStarted = false;
                battleState.actor.setCurrentFrame(0, 1);

                //If the target is dead
                if (battleState.target.health <= 0)
                {
                    battleState.battlers.Remove(battleState.target);
                    battleState.enemies.Remove(battleState.target);
                    battleState.heroes.Remove(battleState.target);
                }

                //Reset target to a neutral frame
                battleState.target.setCurrentFrame(0, 0);

                //Stepping characters back to starting position
                battleState.step.Invoke(gameTime, battleState.timer, -2, 1.5, battleState.actor, new Vector2(battleState.IFF(battleState.actor), 0));
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= battleState.timer + 0.75)
            {
                //If we haven't dealt damage yet (Single run conditional)
                if (battleState.damageDealt == 0)
                {
                    //Deal damage according to physical attack, reduced by physical defence
                    battleState.damageDealt = 250;
                    battleState.target.health += (int)battleState.damageDealt;

                    if (battleState.target.health >= battleState.target.maxHealth)
                    {
                        battleState.target.health = battleState.target.maxHealth;
                    }

                    //Reset the damage indicator
                    battleState.damageLocation = 30;
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds <= battleState.timer)
            {
                //Start animating the actor's attack
                battleState.actor.reverseAnimating = false;
                battleState.actor.AnimationInterval = 250;
                battleState.actor.StartAnimationShort(gameTime, 30, 32, 32);

                //Reset the timer (Should only run once)
                battleState.timer = gameTime.TotalGameTime.TotalSeconds;
            }

            return false;
        }
        public override bool Call(GameTime gameTime, NaviState naviState)
        {
            if (!runOnce)
            {
                if (box == null)
                {
                    box = new Box();

                    box.frameWidth = 800;
                    box.frameHeight = 200;
                    box.UpperLeft = new Vector2(560, 880);

                    box.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                    display = naviState.actor.name + " healed\n" + naviState.target.name + "\nby 250 HP!\n\n\n...It looked kinda pretty, too.";
                }

                naviState.target.health += 250;
                if(naviState.target.health > naviState.target.maxHealth)
                {
                    naviState.target.health = naviState.target.maxHealth;
                }
                
                naviState.pointer.Scale = new Vector2(0.4f, 0.4f);
                naviState.pointer.UpperLeft = new Vector2(1360 - naviState.pointer.GetWidth() - 20, 1080 - naviState.pointer.GetHeight() - 20);
                naviState.pointer.isAlive = true;

                LoadOnce(naviState);

                runOnce = true;
            }

            if (naviState.activateInput.inputState == Input.inputStates.pressed)
            {
                naviState.pointer.Scale = new Vector2(0.8f, 0.8f);
                naviState.pointer.isAlive = false;

                box = null;
                runOnce = false;

                Complete(naviState);

                return true;
            }

            return false;
        }
    }
}
