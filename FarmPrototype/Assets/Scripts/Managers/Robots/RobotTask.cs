using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Managers.Robots
{
    [Serializable]
    public abstract class RobotTask
    {
        public bool started = false;
        public bool ended = false;

        public abstract void OnStart();
        public abstract void Action();
    }
}