using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    class HP : Item
    {
        internal Box box;

        internal string display;

        public HP()
        {
            buyingWorth = 10;
            sellingWorth = 2;

            heldCount = 0;
            maxStack = 99;

            moveInBulk = true;

            battleUsable = true;
            mapUsable = true;

            iconFrame = new Vector2(4, 14);

            name = "Healing Potion";
            description = "Heals your points, isn't that nice?\n\nRestores 10 Health Points to a single party member.";
        }

        public override void DrawAll(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if(box != null)
            {
                box.DrawParts(spriteBatch);

                spriteBatch.DrawString(spriteFont,
                                       display,
                                       new Vector2(580, 900),
                                       Color.Black);
            }
        }

        internal override List<SpriteBase> GetTargets(BattleState battleState)
        {
            AllAllies allAllies = new AllAllies();

            return allAllies.Call(battleState);
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

                //Consume a charge of the item
                Consume(battleState);

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
                    battleState.damageDealt = 10;
                    battleState.target.health += (int)battleState.damageDealt;

                    if(battleState.target.health >= battleState.target.maxHealth)
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
                battleState.actor.StartAnimationShort(gameTime, 48, 50, 50);

                //Reset the timer (Should only run once)
                battleState.timer = gameTime.TotalGameTime.TotalSeconds;
            }

            return false;
        }
        public override bool Call(GameTime gameTime, NaviState naviState)
        {
            if (!runOnce)
            {
                if(box == null)
                {
                    box = new Box();

                    box.frameWidth = 800;
                    box.frameHeight = 200;
                    box.UpperLeft = new Vector2(560, 880);

                    box.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                    display = naviState.target.name + " recovered\n10 Health Points!";
                }

                naviState.target.health += 10;
                if (naviState.target.health > naviState.target.maxHealth)
                {
                    naviState.target.health = naviState.target.maxHealth;
                }

                Consume(naviState);

                naviState.pointer.Scale = new Vector2(0.4f, 0.4f);
                naviState.pointer.UpperLeft = new Vector2((box.frameWidth + box.UpperLeft.X) - naviState.pointer.GetWidth() - 20,
                                                          (box.frameHeight + box.UpperLeft.Y) - naviState.pointer.GetHeight() - 20);
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
