using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class PlayerController : MonoBehaviour
    {
        public const KeyCode moveUpKeybind = KeyCode.W;
        public const KeyCode moveDownKeybind = KeyCode.S;
        public const KeyCode moveLeftKeybind = KeyCode.A;
        public const KeyCode moveRightKeybind = KeyCode.D;
        
        public const KeyCode explodeKeybind = KeyCode.Space;
        public const KeyCode quickResetKeybind = KeyCode.R;
        
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
                    GameManager.Instance.IncrementNumMoves();
                }
                else
                {
                    HandleExplosionLogic();
                    HandleQuickReset();
                }
            }
        }

        bool HandleMovementLogic()
        {
            int newRow = 0;
            int newCol = 0;
            bool isValidMoveTile = false;
            if (Input.GetKey(moveUpKeybind))
            {
                // moving up
                newRow = player.GetCurTile().GetRow() - 1;
                newCol = player.GetCurTile().GetCol();
                inputDirection = DataManager.Direction.Up;

                isValidMoveTile = GameManager.Instance.IsValidTile(newRow, newCol);
            }
            else if (Input.GetKey(moveDownKeybind))
            {
                // moving down
                newRow = player.GetCurTile().GetRow() + 1;
                newCol = player.GetCurTile().GetCol();
                inputDirection = DataManager.Direction.Down;

                isValidMoveTile = GameManager.Instance.IsValidTile(newRow, newCol);
            }
            else if (Input.GetKey(moveLeftKeybind))
            {
                // moving left
                newRow = player.GetCurTile().GetRow();
                newCol = player.GetCurTile().GetCol() - 1;
                isValidMoveTile = GameManager.Instance.IsValidTile(newRow, newCol);

                inputDirection = DataManager.Direction.Left;
            }
            else if (Input.GetKey(moveRightKeybind))
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

        void HandleExplosionLogic()
        {
            int explodeStartTileRow = player.GetCurTile().GetRow();
            int explodeStartTileCol = player.GetCurTile().GetCol();
            int explodeTileRow;
            int explodeTileCol;
            if (Input.GetKeyDown(explodeKeybind))
            {
                // explode in all 4 orthogonal directions, the explosion goes until we hit something or until the edge of the map

                // up
                inputDirection = DataManager.Direction.Up;
                for (explodeTileRow = explodeStartTileRow; explodeTileRow >= 0; --explodeTileRow)
                {
                    if(GameManager.Instance.IsValidTile(explodeTileRow - 1, explodeStartTileCol))
                    {
                        if (HandleExplosion(explodeTileRow - 1, explodeStartTileCol))
                        {
                            break;
                        }
                    }
                }

                // down
                inputDirection = DataManager.Direction.Down;
                var rowCount = GameManager.Instance.GetCurLevelRowCount();

                for (explodeTileRow = explodeStartTileRow; explodeTileRow < rowCount; ++explodeTileRow)
                {
                    if (GameManager.Instance.IsValidTile(explodeTileRow + 1, explodeStartTileCol))
                    {
                        if (HandleExplosion(explodeTileRow + 1, explodeStartTileCol))
                        {
                            break;
                        }
                    }
                }

                // left
                inputDirection = DataManager.Direction.Left;
                for (explodeTileCol = explodeStartTileCol; explodeTileCol >= 0; --explodeTileCol)
                {
                    if (GameManager.Instance.IsValidTile(explodeStartTileRow, explodeTileCol - 1))
                    {
                        if (HandleExplosion(explodeStartTileRow, explodeTileCol - 1))
                        {
                            break;
                        }
                    }
                }

                int colCount = GameManager.Instance.GetCurLevelColCount();
                // right
                inputDirection = DataManager.Direction.Right;
                for (explodeTileCol = explodeStartTileCol; explodeTileCol < colCount; ++explodeTileCol)
                {
                    if (GameManager.Instance.IsValidTile(explodeStartTileRow, explodeTileCol + 1))
                    {
                        if (HandleExplosion(explodeStartTileRow, explodeTileCol + 1))
                        {
                            break;
                        }
                    }
                }

                GameManager.Instance.IncrementNumExplodes();
            }
        }
        
        private void HandleQuickReset()
        {
            if (Input.GetKeyDown(quickResetKeybind))
                GameManager.Instance.ResetCurrentLevel();
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

        bool HandleExplosion(int explodeRow, int explodeCol)
        {
            int fromRow = player.GetCurTile().GetRow();
            int fromCol = player.GetCurTile().GetCol();

            // keep track of our input time
            lastMovementTime = Time.time;

            // if our new tile has an occupier, explode it
            bool validExplode = false;
            Tile newTile = GameManager.Instance.GetTileAtLocation(explodeRow, explodeCol);
            if (newTile.GetTileOuccupier())
            {
                // holes and exits do not react to explosions
                bool didCollideWithHole = newTile.GetTileOuccupier() is HoleOccupier;
                bool didCollideWithExit = newTile.GetTileOuccupier() is ExitOccupier;
                if(didCollideWithHole || didCollideWithExit)
                {
                    return false;
                }

                validExplode = newTile.GetTileOuccupier().ReactToExplosion(explodeRow, explodeCol, inputDirection);
            }

            return validExplode;
        }

        public void SetPlayerObject(TileOccupier player)
        {
            this.player = player;
        }
    }
}