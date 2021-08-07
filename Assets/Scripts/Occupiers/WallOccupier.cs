using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class WallOccupier : TileOccupier
    {
        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            return false;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction explodeDirection)
        {
            // do nothing if we collided with player
            if (collidingOccupier is PlayerOccupier)
            {
                return false;
            }

            return true;
        }
    }
}