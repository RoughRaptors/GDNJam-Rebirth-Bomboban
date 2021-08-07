using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class PlayerController : MonoBehaviour
    {
        private TileOccupier player;
        private Coroutine lerpCoroutine = null;

        DataManager.Direction inputDirection = DataManager.Direction.None;

        // for now, limit how fast the player can move to avoid any problems like moving diagonally or diagonal lerping
        float lastMovementTime;
        const float TIME_BETWEEN_MOVES = 0.25f;

        void Update()
        {
            if (player && Time.time - lastMovementTime >= TIME_BETWEEN_MOVES)
            {
                bool isValidMoveTile = HandleMovementLogic();
                if (isValidMoveTile)
                {

                }
                else
                {
                    HandleExplosionLogic();
                }
            }
        }

        bool HandleMovementLogic()
        {
            int newRow = 0;
            int newCol = 0;
            bool isValidMoveTile = false;
            if (Input.GetKeyDown(DataManager.moveUpKeybind))
            {
                // moving up
                newRow = player.GetCurTile().GetRow() - 1;
                newCol = player.GetCurTile().GetCol();
                inputDirection = DataManager.Direction.Up;

                isValidMoveTile = GameManager.Instance.IsValidTile(newRow, newCol);
            }
            else if (Input.GetKeyDown(DataManager.moveDownKeybind))
            {
                // moving down
                newRow = player.GetCurTile().GetRow() + 1;
                newCol = player.GetCurTile().GetCol();
                inputDirection = DataManager.Direction.Down;

                isValidMoveTile = GameManager.Instance.IsValidTile(newRow, newCol);
            }
            else if (Input.GetKeyDown(DataManager.moveLeftKeybind))
            {
                // moving left
                newRow = player.GetCurTile().GetRow();
                newCol = player.GetCurTile().GetCol() - 1;
                isValidMoveTile = GameManager.Instance.IsValidTile(newRow, newCol);

                inputDirection = DataManager.Direction.Left;
            }
            else if (Input.GetKeyDown(DataManager.moveRightKeybind))
            {
                // moving right
                newRow = player.GetCurTile().GetRow();
                newCol = player.GetCurTile().GetCol() + 1;
                isValidMoveTile = GameManager.Instance.IsValidTile(newRow, newCol);

                inputDirection = DataManager.Direction.Right;
            }

            if(isValidMoveTile)
            {
                HandlePlayerMovement(newRow, newCol);
                return true;
            }

            return false;
        }

        void HandlePlayerMovement(int newRow, int newCol)
        {
            int fromRow = player.GetCurTile().GetRow();
            int fromCol = player.GetCurTile().GetCol();

            // keep track of our input time
            lastMovementTime = Time.time;

            // if our new tile has an occupier, handle collision between player and occupier
            bool validMovement = true;
            Tile newTile = GameManager.Instance.GetTileAtLocation(newRow, newCol);
            if (newTile.GetTileOuccupier() != null)
            {
                // we collided, meaning we tried to move from our current tile with an existing tile occupier
                // we block this unless it's the exit tile occupier, in which case we win
                validMovement = newTile.GetTileOuccupier().ReactToCollision(player, inputDirection);
            }

            if (validMovement)
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

        bool HandleExplosionLogic()
        {
            int explodeTileRow = 0;  
            int explodeTileCol = 0;
            bool isValidExplodeTile = false;
            if (Input.GetKeyDown(DataManager.explodeUpKeybind))
            {
                explodeTileRow = player.GetCurTile().GetRow() - 1;
                explodeTileCol = player.GetCurTile().GetCol();
                isValidExplodeTile = GameManager.Instance.IsValidTile(explodeTileRow, explodeTileCol);

                inputDirection = DataManager.Direction.Up;
            }
            else if (Input.GetKeyDown(DataManager.explodeDownKeybind))
            {
                explodeTileRow = player.GetCurTile().GetRow() + 1;
                explodeTileCol = player.GetCurTile().GetCol();
                isValidExplodeTile = GameManager.Instance.IsValidTile(explodeTileRow, explodeTileCol);

                inputDirection = DataManager.Direction.Down;
            }
            else if (Input.GetKeyDown(DataManager.explodeLeftKeybind))
            {
                explodeTileRow = player.GetCurTile().GetRow();
                explodeTileCol = player.GetCurTile().GetCol() - 1;
                isValidExplodeTile = GameManager.Instance.IsValidTile(explodeTileRow, explodeTileCol);

                inputDirection = DataManager.Direction.Left;
            }
            else if (Input.GetKeyDown(DataManager.explodeRightKeybind))
            {
                explodeTileRow = player.GetCurTile().GetRow();
                explodeTileCol = player.GetCurTile().GetCol() + 1;
                isValidExplodeTile = GameManager.Instance.IsValidTile(explodeTileRow, explodeTileCol);

                inputDirection = DataManager.Direction.Right;
            }

            if(isValidExplodeTile)
            {
                HandleExplosion(explodeTileRow, explodeTileCol);
                return true;
            }

            return false;
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

        void HandleExplosion(int explodeRow, int explodeCol)
        {
            int fromRow = player.GetCurTile().GetRow();
            int fromCol = player.GetCurTile().GetCol();

            // keep track of our input time
            lastMovementTime = Time.time;

            // if our new tile has an occupier, explode it
            bool validExplode = false;
            Tile newTile = GameManager.Instance.GetTileAtLocation(explodeRow, explodeCol);
            if (newTile.GetTileOuccupier() != null)
            {
                validExplode = newTile.GetTileOuccupier().ReactToExplosion(fromRow, fromCol, inputDirection);
            }

            if(validExplode)
            {

            }
        }

        public void SetPlayerObject(TileOccupier player)
        {
            this.player = player;
        }
    }
}