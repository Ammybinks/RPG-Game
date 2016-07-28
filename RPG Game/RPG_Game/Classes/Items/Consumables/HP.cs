using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class HP : Item
    {
        public HP()
        {
            buyingWorth = 50;
            sellingWorth = 10;

            heldCount = 1;
            maxStack = 99;

            iconFrame = new Vector2(9, 5);

            name = "Healing Potion";
            description = "Heals your points, isn't that nice?\n\nRestores 10 Health Points to a single party member.";
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
    }
}
