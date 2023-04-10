using CS.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS.Actor
{
    [Serializable]
    public class ActorMoveProps
    {
        public MoveComponent moveComponent;
        public float walkSpeed;
        public float runSpeed;
        public float lyingDownSpeed;
    }

    [Serializable]
    public class AnimationProps
    {
        public Animator animator;
    }

    public class ActorUnit : MonoBehaviour
    {
        [SerializeField] private ActorMoveProps _actorMoveProps;
        [SerializeField] private AnimationProps _actorAnimation;

        public ActorMoveProps move => _actorMoveProps;
        public AnimationProps animations => _actorAnimation;
    }
}