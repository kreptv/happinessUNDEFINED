﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueQuests
{
    [CreateAssetMenu(fileName = "Actor", menuName = "DialogueQuests/Actor", order = 0)]
    public class ActorData : ScriptableObject
    {
        public string actor_id;

        [Tooltip("Is the player character")]
        public bool is_player = false;

        [Header("Character name")]
        public string title;
        [Tooltip("Portrait Image")]
        public Sprite portrait;
        [Tooltip("Animated Portrait (Optional)")]
        public RuntimeAnimatorController animation;

        [Header("Global Dialogues")]
        [Tooltip("Dialogue prefab that should be loaded in all scenes")]
        public GameObject load_dialogue;

        private static List<ActorData> actor_list = new List<ActorData>();

        public string GetTitle()
        {
            return NarrativeTool.Translate(title);
        }

        public static void Load(ActorData actor)
        {
            if (!actor_list.Contains(actor))
            {
                actor_list.Add(actor);
            }
        }

        public static ActorData Get(string actor_id)
        {
            if (NarrativeManager.Get())
            {
                foreach (ActorData actor in GetAll())
                {
                    if (actor.actor_id == actor_id)
                        return actor;
                }
            }
            return null;
        }

        public static List<ActorData> GetAll()
        {
            return actor_list;
        }
    }

}
