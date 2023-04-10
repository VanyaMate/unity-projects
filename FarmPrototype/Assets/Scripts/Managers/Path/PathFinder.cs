using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Managers.Path
{
    public class PathFinder
    {
        private static bool _finded = false;

        public static List<PathNode> GetPathFromTo (PathNode start, PathNode finish)
        {
            List<PathNode> path = new List<PathNode>();

            if (start != finish)
            {

                PathFinder.SetWeights(start, finish, 0);

                if (PathFinder._finded)
                {
                    path = PathFinder.GetPath(path, finish);

                    PathFinder._finded = false;
                    PathFinder.ResetNodes(PathManager.instance.pathNodes);
                    return path;
                }

                PathFinder._finded = false;
                PathFinder.ResetNodes(PathManager.instance.pathNodes);
            }
            else
            {
                path.Add(start);
            }

            return path;
        }

        public static void SetWeights (PathNode current, PathNode finish, float weight)
        {
            current.weight = weight;
            current.connects.ForEach((connect) =>
            {
                if (connect.pathNodeType == PathType.Disabled) return;

                float connectWeight = weight + Vector3.Distance(current.transform.position, connect.transform.position);

                if (connect == finish)
                {
                    if (finish.weight > connectWeight)
                    {
                        PathFinder._finded = true;
                        connect.weight = connectWeight;
                        connect.weightFrom = current;
                    }

                    return;
                }
                else
                {
                    if (connect.weight > connectWeight)
                    {
                        connect.weightFrom = current;
                        PathFinder.SetWeights(connect, finish, connectWeight);
                    }
                }
            });
        }

        public static List<PathNode> GetPath (List<PathNode> path, PathNode current)
        {
            path.Add(current);

            if (current.weightFrom == null)
            {
                return path;
            }
            else
            {
                return PathFinder.GetPath(path, current.weightFrom);
            }
        }

        public static PathNode FindNearestPathNode (Transform transform)
        {
            if (PathManager.instance.pathNodes.Count != 0)
            {
                PathNode nearest = PathManager.instance.pathNodes[0];
                float distanceToNearest = Vector3.Distance(transform.position, nearest.transform.position);

                PathManager.instance.pathNodes.ForEach((node) =>
                {
                    float distanceToNode = Vector3.Distance(transform.position, node.transform.position);
                    if (distanceToNode < distanceToNearest)
                    {
                        nearest = node;
                        distanceToNearest = distanceToNode;
                    }
                });

                return nearest;
            }

            return null;
        }

        public static void ResetNodes (List<PathNode> nodes)
        {
            nodes.ForEach((node) => {
                node.weight = 9999;
                node.weightFrom = null;    
            });
        }
    }
}
