using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class WallOccupier : TileOccupier
    {
        void Start()
        {

        }

        public override void ReactToExplosion(int fromRow, int fromCol, DataManager.Direction collisionDirection)
        {

        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
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