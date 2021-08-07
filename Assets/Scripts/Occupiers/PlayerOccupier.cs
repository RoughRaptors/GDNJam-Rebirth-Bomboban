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

        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            return false;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            return false;
        }
    }
}