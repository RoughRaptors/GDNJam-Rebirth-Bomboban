using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class PlayerController : MonoBehaviour
    {
        private TileOccupier player;
        private Coroutine lerpCoroutine = null;

        DataManager.Direction moveDirection = DataManager.Direction.None;

        // for now, limit how fast the player can move to avoid any problems like moving diagonally or diagonal lerping
        float lastMovementTime;
        const float TIME_BETWEEN_MOVES = 0.25f;

        void Update()
        {
            if (player && Time.time - lastMovementTime >= TIME_BETWEEN_MOVES)
            {
                bool isValidTile = false;
                int newRow = 0;
                int newCol = 0;
                if (Input.GetKeyDown(KeyCode.D))
                {
                    // moving right
                    newRow = player.GetCurTile().GetRow();
                    newCol = player.GetCurTile().GetCol() + 1;

                    isValidTile = IsValidTile(newRow, newCol);
                    moveDirection = DataManager.Direction.Right;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    // moving left
                    newRow = player.GetCurTile().GetRow();
                    newCol = player.GetCurTile().GetCol() - 1;

                    isValidTile = IsValidTile(newRow, newCol);
                    moveDirection = DataManager.Direction.Left;
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    // moving up
                    newRow = player.GetCurTile().GetRow() - 1;
                    newCol = player.GetCurTile().GetCol();

                    isValidTile = IsValidTile(newRow, newCol);
                    moveDirection = DataManager.Direction.Up;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    // moving down
                    newRow = player.GetCurTile().GetRow() + 1;
                    newCol = player.GetCurTile().GetCol();

                    isValidTile = IsValidTile(newRow, newCol);
                    moveDirection = DataManager.Direction.Down;
                }

                if (isValidTile)
                {
                    HandlePlayerMovement(newRow, newCol);
                }
            }
        }

        public void SetPlayerObject(TileOccupier player)
        {
            this.player = player;
        }

        private bool IsValidTile(int newRow, int newCol)
        {
            if (newRow >= 0 && newRow < DataManager.NUM_ROWS && newCol >= 0 && newCol < DataManager.NUM_COLS)
            {
                return true;
            }

            return false;
        }

        void HandlePlayerMovement(int newRow, int newCol)
        {
            int fromRow = player.GetCurTile().GetRow();
            int fromCol = player.GetCurTile().GetCol();

            // keep track of our movement time
            lastMovementTime = Time.time;

            // if our new tile has an occupier, handle collision between player and occupier
            bool validMovement = true;
            Tile newTile = GameManager.Instance.GetTileAtLocation(newRow, newCol);
            if(newTile.GetTileOuccupier() != null)
            {
                // we collided, meaning we tried to move from our current tile with an existing tile occupier
                // we block this unless it's the exit tile occupier, in which case we win
                validMovement = newTile.GetTileOuccupier().ReactToCollision(player, moveDirection);
            }

            if(validMovement)
            {
                // move our player
                Vector3 newPos = new Vector3(newTile.GetPhysicalXPos(), newTile.GetPhysicalYPos());

                // stop our old lerp if it exists, and lerp to our new position
                if (lerpCoroutine != null)
                {
                    StopCoroutine(lerpCoroutine);
                }

                lerpCoroutine = StartCoroutine(HandleMovePlayerPhysical(newPos));

                // our old tile now has no occupier
                player.GetCurTile().SetTileOccupier(null);

                // our new tile occupier is the player and the occupier's tile is the new tile
                // i hate having intertwined references here but, ugh, no time
                newTile.SetTileOccupier(player);
                player.SetCurTile(newTile);
            }
        }

        private IEnumerator HandleMovePlayerPhysical(Vector3 newPos)
        {
            float elapsedTime = 0;
            float waitTime = DataManager.LERP_COROUTINE_WAIT_TIME;

            while (elapsedTime < waitTime)
            {                
                player.transform.position = Vector3.Lerp(player.transform.position, newPos, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
        }
    }
}