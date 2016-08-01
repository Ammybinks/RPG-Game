using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class Murder : Ability
    {
        public Murder()
        {
            iconFrame = new Vector2(10, 0);

            cost = 100;

            battleUsable = true;
            mapUsable = false;

            name = "Murder";
            description = "No crows here.\n\nDeals one thousand times the damage of a normal attack.";
        }

        internal override List<SpriteBase> GetTargets(BattleState battleState)
        {
            AllEnemies allEnemies = new AllEnemies();

            return allEnemies.Call(battleState);
        }

        //Animates a basic attack, animating both the actor and target, and removing health from the target.
        //Keep in mind that the actor has already been moved forwards by the time this is called, 
        //and that it is this function's job to return all its actors to their original positions
        public override bool Call(GameTime gameTime, BattleState battleState)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= battleState.timer + 1.5)
            {
                //Reset all values ready for continuing the fight
                battleState.damageDealt = 0;
                battleState.damageLocation = 0;
                battleState.timer = 0;

                //Make sure our actor and targets aren't moving or animating and are in the correct position
                battleState.actor.UpperLeft = battleState.actor.battleOrigin;
                battleState.actor.setCurrentFrame(0, 0);
                battleState.actor.animationShortStarted = false;

                battleState.target.UpperLeft = battleState.target.battleOrigin;
                battleState.target.setCurrentFrame(0, 0);
                battleState.target.animationShortStarted = false;

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
                battleState.step.Invoke(gameTime, battleState.timer, 2, 1.5, battleState.target, new Vector2(battleState.IFF(battleState.target), 0));
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= battleState.timer + 0.75)
            {
                //step.Invoke the target backwards
                battleState.step.Invoke(gameTime, battleState.timer, -2, 1, battleState.target, new Vector2(battleState.IFF(battleState.target), 0));

                //If we haven't dealt damage yet (Single run conditional)
                if (battleState.damageDealt == 0)
                {
                    //Deal damage according to physical attack, reduced by physical defence
                    battleState.damageDealt = ((battleState.actor.PhAtk * ((100 - battleState.target.PhDef) / 100)) * 1000) * -1;
                    battleState.damageDealt = (float)Math.Round(battleState.damageDealt, 0, MidpointRounding.AwayFromZero);
                    battleState.target.health += (int)battleState.damageDealt;

                    //Reset the damage indicator
                    battleState.damageLocation = 30;

                    //Start animation on target, making sure they're not reversing or stepping through too fast or slow
                    battleState.target.reverseAnimating = false;
                    battleState.target.AnimationInterval = 50;
                    battleState.target.StartAnimationShort(gameTime, 36, 38, 38);
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds <= battleState.timer)
            {
                //Start animating the actor's attack
                battleState.actor.reverseAnimating = false;
                battleState.actor.AnimationInterval = 250;
                battleState.actor.StartAnimationShort(gameTime, 2, 5, 5);

                //Reset the timer (Should only run once)
                battleState.timer = gameTime.TotalGameTime.TotalSeconds;
            }

            return false;
        }
    }
}
