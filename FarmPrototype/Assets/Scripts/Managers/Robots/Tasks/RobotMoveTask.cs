using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Managers.Path;

namespace VM.Managers.Robots
{
    public class RobotMoveTask : RobotTask
    {
        private RobotUnit _unit = null;
        private List<PathNode> _cashPath = new List<PathNode>();

        public List<PathNode> currentPath = new List<PathNode>();

        public int currentPathStep;
        public PathNode currentPathNode = null;
        public int nextPathStep;
        public PathNode nextPathNode = null;

        public RobotMoveTask (RobotUnit unit, PathNode to)
        {
            this._unit = unit;

            List<PathNode> path = PathFinder.GetPathFromTo(
                start: PathFinder.FindNearestPathNode(this._unit.transform),
                finish: to
            );

            this._cashPath = path;
        }

        public override void OnStart ()
        {
            this.SetPath(this._cashPath);
            this.started = true;
        }

        public override void Action ()
        {
            if (!this.started)
            {
                this.OnStart();
            }

            this.MoveTo(this.nextPathNode);
        }

        public void SetPath(List<PathNode> path)
        {
            List<PathNode> pathToTask = this._GetPathToTaskPath(path[^1]);
            path.AddRange(pathToTask);
            this.currentPath = path;
            this._NextPathStep(0);
        }

        public void MoveTo(PathNode pathNode)
        {
            Vector3 direction = (pathNode.transform.position - this._unit.transform.position).normalized;
            Vector3 moveToPosition = direction * Time.deltaTime * this._unit.speed;

            float distanceToNode = Vector3.Distance(this._unit.transform.position, pathNode.transform.position);
            float distanceToMovePosition = Vector3.Distance(this._unit.transform.position, this._unit.transform.position + moveToPosition);

            if (distanceToNode <= distanceToMovePosition + .3f)
            {
                this._NextPathStep(this.nextPathStep);
            }

            this._unit.transform.position += moveToPosition;
            this._unit.transform.forward = direction;
        }

        private void _NextPathStep(int step = 0)
        {
            if (step < this.currentPath.Count)
            {
                this.currentPathStep = step;
                this.currentPathNode = this.currentPathStep == 0 ? null : this.currentPath[^this.currentPathStep];
                this.nextPathStep = step + 1;
                this.nextPathNode = this.currentPath[^this.nextPathStep];
            }
            else
            {
                this.ended = true;
                this._ResetPath();
            }
        }

        private List<PathNode> _GetPathToTaskPath(PathNode startNodeOfTaskPath)
        {
            return PathFinder.GetPathFromTo(this._FindNearestPathNode(), startNodeOfTaskPath);
        }

        private PathNode _FindNearestPathNode()
        {
            if (PathManager.instance.pathNodes.Count != 0)
            {
                PathNode nearest = PathManager.instance.pathNodes[0];
                float distanceToNearest = Vector3.Distance(this._unit.transform.position, nearest.transform.position);

                PathManager.instance.pathNodes.ForEach((node) =>
                {
                    float distanceToNode = Vector3.Distance(this._unit.transform.position, node.transform.position);
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

        private void _ResetPath()
        {
            this.currentPath = null;
            this.currentPathStep = 0;
            this.currentPathNode = null;
            this.nextPathStep = 0;
            this.nextPathNode = null;
        }
    }
}