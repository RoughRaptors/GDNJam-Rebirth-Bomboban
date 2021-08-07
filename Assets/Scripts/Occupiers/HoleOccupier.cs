using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class HoleOccupier : TileOccupier
    {
        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            return false;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            // titanium box and ice replaces hole
            if(collidingOccupier is TitaniumBoxOccupier)
            {
                Destroy(this.gameObject);
                Destroy(collidingOccupier.gameObject);
            }

            return false;
        }
    }
}