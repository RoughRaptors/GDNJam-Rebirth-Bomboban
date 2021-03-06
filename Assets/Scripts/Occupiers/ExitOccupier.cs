using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class ExitOccupier : TileOccupier
    {
        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            return false;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            if (collidingOccupier is PlayerOccupier)
            {
                Destroy(gameObject);
                // we win
                GameManager.Instance.CompleteLevel();

                return true;
            }

            return false;
        }
    }
}