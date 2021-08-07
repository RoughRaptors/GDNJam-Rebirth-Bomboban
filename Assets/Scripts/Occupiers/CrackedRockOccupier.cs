using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class CrackedRockOccupier : TileOccupier
    {
        void Start()
        {
            health = 1;
        }

        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            health -= 1;
            if(health <= 0)
            {
                curTile.SetTileOccupier(null);
                Destroy(this.gameObject);
            }

            return true;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            return false;
        }
    }
}