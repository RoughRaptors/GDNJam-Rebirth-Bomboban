using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class WallOccupier : TileOccupier
    {
        private void Start()
        {
            health = 1;
        }

        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            return true;
            //return SubtractHealth(1, true);
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction explodeDirection)
        {
            return false;
        }
    }
}