using System;
using Project_Cautious.Cast.Basics;

namespace Project_Cautious.Cast{

    /// <summary>
    /// Base class for all enemies in the game.
    /// </summary>
    public class Enemy : Gunner {
        public Enemy(){
            _health = 1;
            _attack = FirePattern.getFirePattern(patternName.Single);
            SetWidth(Constants.DEFAULT_SQUARE_SIZE);
            SetHeight(Constants.DEFAULT_SQUARE_SIZE);
            SetPosition(new Point(Constants.MAX_X / 2, _height));
            _color = Raylib_cs.Color.RED;
        }

        public override bool canGetHit()
        {
            return true;
        }

        public override void TakeDamage()
        {
            _health --;
        }
    }
}