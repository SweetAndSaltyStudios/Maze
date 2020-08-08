using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class LevelManager : Singelton<LevelManager>
    {
        [Header("Maze Size")]
        public int Maze_Width = 4;
        public int Maze_Height = 4;
        private int totalTileCount;
        private int visitedCellCount;
        private readonly float cellOffset = 0.5f;

        [Header("Colors")]
        public Color cellColor;
        public Color backTrackColor;

        private Stack<Cell> visitedCells = new Stack<Cell>();
        private Cell targetCell;
        private Cell startingCell;
        private Cell endingCell;

        private bool isGridCreated;
        private bool isPathCreated;
        private bool isGameRunning;

        private Cell[,] tiles;

        private Action<SpriteMaskInteraction> OnGameStart;

        private IEnumerator Start()
        {
            totalTileCount = Maze_Width * Maze_Height;

            GenerateGrid();
            yield return new WaitUntil(() => isGridCreated);

            StartCoroutine(GeneratePath());
            yield return new WaitUntil(() => isPathCreated);

            OnGameStart.Invoke(SpriteMaskInteraction.VisibleInsideMask);

            endingCell = GetCell(19, 0);
            SpawnPrefab(ResourceManager.Instance.GoalPrefab, null, new Vector2(endingCell.Position_X + cellOffset, endingCell.Position_Y + cellOffset));
            var ball = SpawnPrefab(ResourceManager.Instance.BallPrefab, null, new Vector2(startingCell.Position_X + cellOffset, startingCell.Position_Y + cellOffset));

            Time.timeScale = 0;

            UIManager.Instance.ViewMask.AnimateScale();

            yield return new WaitUntil(() => UIManager.Instance.ViewMask.IsScalingDone);

            UIManager.Instance.ViewMask.AnimatePosition(ball.transform.position);

            yield return new WaitUntil(() => UIManager.Instance.ViewMask.IsMovingDone);

            UIManager.Instance.ViewMask.transform.parent = ball.transform;

            yield return new WaitForSecondsRealtime(0.25f);

            UIManager.Instance.HintMessage.enabled = true;

            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

            UIManager.Instance.HintMessage.enabled = false;

            isGameRunning = true;

            Time.timeScale = 1;

            while(isGameRunning)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                UIManager.Instance.MainCameraEngine.Foo_1();

                yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

                UIManager.Instance.MainCameraEngine.Foo_2();
            }
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            isGameRunning = false;
        }

        public void RegisterOnGameStart(Action<SpriteMaskInteraction> action)
        {
            OnGameStart += action;
        }

        public void UnregisterOnGameStart(Action<SpriteMaskInteraction> action)
        {
            OnGameStart -= action;
        }

        private void GenerateGrid()
        {
            var cellParent = new GameObject("Cells").transform;
            tiles = new Cell[Maze_Width, Maze_Height];

            var spawnDelay = new WaitForSeconds(0.1f);

            for(int x = 0; x < Maze_Width; x++)
            {
                for(int y = 0; y < Maze_Height; y++)
                {
                    var newTile = SpawnPrefab(ResourceManager.Instance.CellPrefab, cellParent, new Vector2(x + cellOffset, y + cellOffset));
                    newTile.Initialize(x, y, "Cell (" + (x + 1) + ", " + (y + 1) + " )");
                    newTile.ChangeColor(Color.clear);
                    tiles[x, y] = newTile;
                }
            }

            isGridCreated = true;
        }

        private IEnumerator GeneratePath()
        {
            targetCell = startingCell = GetCell(0, 39);

            var waitDelay = new WaitForSeconds(0.001f);

            if(targetCell == null)
            {
                Debug.LogWarning(targetCell);
                yield break;
            }

            var neighbours = GetValidCellNeighbours(targetCell);

            AddVisitedTile(targetCell, cellColor);

            while(visitedCellCount < totalTileCount)
            {
                var currentCell = neighbours[UnityEngine.Random.Range(0, neighbours.Length)];
                var direction = currentCell.Position - targetCell.Position;

                if(currentCell == null)
                {
                    Debug.LogWarning(currentCell);
                    yield break;
                }

                currentCell.SetWall(direction * -1, false);
                targetCell.SetWall(direction, false);

                targetCell = GetCell(currentCell.Position_X, currentCell.Position_Y);
                neighbours = GetValidCellNeighbours(targetCell);
                AddVisitedTile(targetCell, cellColor);

                while(neighbours.Length == 0)
                {
                    if(visitedCells.Count == 0 || visitedCellCount == totalTileCount)
                    {
                        break;
                    }

                    targetCell = visitedCells.Pop();
                    targetCell.ChangeColor(backTrackColor);
                    yield return waitDelay;
                    targetCell.ChangeColor(cellColor);
                    neighbours = GetValidCellNeighbours(targetCell);
                }

                yield return waitDelay;
            }

            isPathCreated = true;
        }

        private Cell[] GetValidCellNeighbours(Cell cell)
        {
            var tempNeigbours = new List<Cell>();

            var cell_N = GetCell(cell.Position_X, cell.Position_Y + 1);
            var cell_E = GetCell(cell.Position_X + 1, cell.Position_Y);
            var cell_S = GetCell(cell.Position_X, cell.Position_Y - 1);
            var cell_W = GetCell(cell.Position_X - 1, cell.Position_Y);

            if(cell_N && cell_N.IsVisited == false)
            {
                tempNeigbours.Add(cell_N);
            }

            if(cell_E && cell_E.IsVisited == false)
            {
                tempNeigbours.Add(cell_E);
            }

            if(cell_S && cell_S.IsVisited == false)
            {
                tempNeigbours.Add(cell_S);
            }

            if(cell_W && cell_W.IsVisited == false)
            {
                tempNeigbours.Add(cell_W);
            }

            return tempNeigbours.ToArray();
        }

        private Cell GetCell(int x, int y)
        {
            return x >= 0 && x < Maze_Width && y >= 0 && y < Maze_Height ? tiles[x, y] : null;
        }

        private void AddVisitedTile(Cell cell, Color color)
        {
            visitedCells.Push(cell);
            cell.ChangeColor(color);

            cell.IsVisited = true;

            visitedCellCount++;
        }

        private PREFAB SpawnPrefab<PREFAB>(PREFAB prefab, Transform parent = null, Vector2 position = new Vector2(), Quaternion rotation = new Quaternion()) where PREFAB : Component
        {
            return Instantiate(prefab, position, rotation, parent) as PREFAB;
        }
    }
}