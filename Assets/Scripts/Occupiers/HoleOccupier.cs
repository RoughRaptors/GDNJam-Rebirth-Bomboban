using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class HoleOccupier : TileOccupier
    {
        void Start()
        {

        }

        public override void ReactToExplosion(int fromRow, int fromCol, DataManager.Direction collisionDirection)
        {

        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            // titanium box replaces hole
            if(collidingOccupier is TitaniumBoxOccupier)
            {
                Destroy(this.gameObject);
                Destroy(collidingOccupier.gameObject);
            }

            return false;
        }
    }
}