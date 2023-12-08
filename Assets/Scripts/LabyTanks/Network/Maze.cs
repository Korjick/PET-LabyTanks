using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LabyTanks.Network
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class Maze : NetworkBehaviour
    {
        [SerializeField] private int _width = 8;
        [SerializeField] private int _height = 8;
        [SerializeField] private float _wallProbability = 0.5f;
        [Space]
        [SerializeField] private int _startX = -35;
        [SerializeField] private int _startZ = 35;
        [SerializeField] private int _shift = 10;
        [Space]
        [SerializeField] private Cell _cellsPrefab;
        
        public override void OnStartServer()
        {
            List<Cell> cellList = InitMaze(_width);
            for (int mazeIdx = 0; mazeIdx <= _height - 1; mazeIdx++)
            {
                List<Cell> subList = cellList.GetRange(mazeIdx * _width, _width);

                if (mazeIdx == _height - 1)
                {
                    ReorderLast(subList);
                    break;
                }

                if (mazeIdx != 0) 
                    ReorderIntermedia(subList);
                
                CombineClusters(subList);
                SetDown(subList);

                List<Cell> copy = new List<Cell>();
                foreach (Cell cell in subList)
                {
                    Cell newCell = SpawnCell(cell.transform.localPosition - Vector3.forward * _shift);
                    newCell.Init(cell);
                    newCell.SetWallActive(Cell.WallDirection.Up, cell.GetWallActive(Cell.WallDirection.Down));
                    copy.Add(newCell);
                }
                cellList.AddRange(copy);
            }
            
            cellList.ForEach(c =>
            {
                c._left = c.GetWallActive(Cell.WallDirection.Left);
                c._right = c.GetWallActive(Cell.WallDirection.Right);
                c._up = c.GetWallActive(Cell.WallDirection.Up);
                c._down = c.GetWallActive(Cell.WallDirection.Down);
            });
        }
        
        private List<Cell> InitMaze(int mazeSize)
        {
            List<Cell> cellList = new List<Cell>();
            for (int i = 0; i < mazeSize; i++)
            {
                Cell newCell = SpawnCell(new Vector3(_startX + i * _shift, 0, _startZ));
                newCell.Init(i, false, false, true, false);
                if (i == 0)
                    newCell.SetWallActive(Cell.WallDirection.Left, true);
                else if (i == mazeSize - 1) 
                    newCell.SetWallActive(Cell.WallDirection.Right, true);

                cellList.Add(newCell);
            }

            return cellList;
        }

        private void CombineClusters(List<Cell> cellList)
        {
            for (int i = 0; i < cellList.Count - 1; i++)
            {
                Cell left = cellList[i];
                Cell right = cellList[i + 1];
                if (left.ClusterId == right.ClusterId)
                {
                    left.SetWallActive(Cell.WallDirection.Right, true);
                    right.SetWallActive(Cell.WallDirection.Left, true);
                }
                else
                {
                    if (Random.Range(0f, 1f) < _wallProbability)
                    {
                        left.SetWallActive(Cell.WallDirection.Right, true);
                        right.SetWallActive(Cell.WallDirection.Left, true);
                    }
                    else
                    {
                        right.SetClusterId(left.ClusterId);
                    }
                }
            }
        }

        private void SetDown(List<Cell> cellList)
        {
            Dictionary<int, List<Cell>> clusters = 
                cellList
                    .GroupBy(c => c.ClusterId)
                    .ToDictionary(c => c.Key, c => c.ToList());
            foreach (var key in clusters.Keys.ToList())
            {
                if (clusters[key].Count == 1)
                    continue;

                int downCount = Random.Range(1, clusters[key].Count);
                clusters[key] = clusters[key].OrderBy(_ => Guid.NewGuid()).ToList();
                for (int i = 0; i < downCount; i++) 
                    clusters[key][i].SetWallActive(Cell.WallDirection.Down, true);
            }
        }

        private void ReorderIntermedia(List<Cell> cellList)
        {
            HashSet<int> clusters = cellList.Select(c => c.ClusterId).ToHashSet();
            for (int i = 0; i < cellList.Count; i++)
            {
                Cell tmpCell = cellList[i];
                if (i != cellList.Count - 1) 
                    tmpCell.SetWallActive(Cell.WallDirection.Right, false);

                if (i != 0) 
                    tmpCell.SetWallActive(Cell.WallDirection.Left, false);

                if (tmpCell.GetWallActive(Cell.WallDirection.Down))
                {
                    tmpCell.SetWallActive(Cell.WallDirection.Down, false);
                    int randCluster;
                    do
                    {
                        randCluster = Random.Range(0, cellList.Count);
                    } while (clusters.Contains(randCluster));

                    tmpCell.SetClusterId(randCluster);
                    clusters.Add(randCluster);
                }
            }
        }

        private void ReorderLast(List<Cell> cellList)
        {
            for (int i = 0; i < cellList.Count - 1; i++)
            {
                Cell left = cellList[i];
                Cell right = cellList[i + 1];
                left.SetWallActive(Cell.WallDirection.Down, true);
                right.SetWallActive(Cell.WallDirection.Down, true);
                if (left.ClusterId != right.ClusterId)
                {
                    left.SetWallActive(Cell.WallDirection.Right, false);
                    right.SetWallActive(Cell.WallDirection.Left, false);
                    right.SetClusterId(left.ClusterId);
                }
            }
        }

        private Cell SpawnCell(Vector3 position)
        {
            Cell newCell = Instantiate(_cellsPrefab, position, Quaternion.identity, transform);
            NetworkServer.Spawn(newCell.gameObject);
            return newCell;
        }
    }
}