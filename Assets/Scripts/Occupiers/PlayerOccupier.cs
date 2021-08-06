using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class PlayerOccupier : TileOccupier
    {
        void Start()
        {

        }

        public override void ReactToExplosion(int fromRow, int fromCol, DataManager.Direction collisionDirection)
        {

        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            return false;
        }
    }
}