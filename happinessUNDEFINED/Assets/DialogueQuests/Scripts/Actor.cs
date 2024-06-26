﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Actor for dialogues
/// </summary>

namespace DialogueQuests
{
    public class Actor : MonoBehaviour
    {
        public ActorData data;

        [Header("Trigger")]
        [Tooltip("How far from the player can this actor be triggered?")]
        public float trigger_range = 2f;

        [Header("Chat/Interact icons")]
        public bool show_chat;
        public bool show_interact;
        public Vector3 icon_offset;
        public float icon_size = 1f;

        [HideInInspector]
        public int player_id = -1;

        [HideInInspector]
        public bool auto_interact_enabled = true; //Put this to false if you want to code your own interact controls (and call Interact() instead)

        public UnityAction<Actor, Actor> onInteract; //Player, Actor
        public UnityAction<Actor, Actor> onNear;    //Player, Actor

        private List<NarrativeEvent> events_list;
        private bool in_range = false;
		
		private static List<Actor> actor_list = new List<Actor>();

        protected virtual void Awake()
        {
            actor_list.Add(this);

            //Make sure its a prefab, not a scene object
            if (data.load_dialogue != null && data.load_dialogue.scene.name == null)
                Instantiate(data.load_dialogue);
        }

        void Start()
        {
            events_list = NarrativeEvent.GetAllOf(this);

            foreach (NarrativeEvent evt in events_list)
            {
                evt.AddActor(this);
            }

            if (NarrativeControls.Get())
            {
                NarrativeControls.Get().onPressTalk += OnClick;
                NarrativeControls.Get().onPressTalkMouse += OnClick;
            }
        }

        private void OnDestroy()
        {
            actor_list.Remove(this);

            if (NarrativeControls.Get())
            {
                NarrativeControls.Get().onPressTalk -= OnClick;
                NarrativeControls.Get().onPressTalkMouse -= OnClick;
            }
        }
        
        private void Update()
        {
            if (!NarrativeManager.IsReady())
                return;

            Actor player = GetPlayerActor();
            if (player != null && player != this)
            {
                if (!in_range && IsInRange(player.transform.position) && CanInteract(player))
                {
                    in_range = true;
                    if (onNear != null)
                        onNear.Invoke(player, this);
                }
                else if (in_range && !IsInRange(player.transform.position))
                {
                    in_range = false;
                }
            }
        }

        private void OnClick()
        {
            if(auto_interact_enabled && NarrativeManager.IsReady()){
                Actor player = GetPlayerActor();
                if (player != null && player != this && CanInteract(player))
                {
                    Interact(player);
                }
            }
        }

        public void Interact(Actor player)
        {
            if (CanInteract(player))
            {
                if (onInteract != null)
                    onInteract.Invoke(player, this);
            }
        }

        //Is it currently possible to interact with this actor
        public bool CanInteract(Actor player)
        {
            if (!NarrativeManager.IsActive() || player == null || gameObject == null)
                return false;

            if (!gameObject.activeSelf || !enabled || !NarrativeManager.IsReady())
                return false;

            if (IsPlayer())
                return false; //Cant interact with other players

            if(!IsInRange(player.transform.position))
                return false; //Too far
            
            if (NarrativeManager.Get().GetEventTimer() < 0.5f)
                return false; //Just finished another event

            return true;
        }

        public bool HasEvents(Actor player)
        {
            bool has_events = false;
            foreach (NarrativeEvent evt in events_list)
            {
                if (evt.AreConditionsMet(player, this))
                    has_events = true;
            }
            return has_events;
        }

        public bool HasEvents(NarrativeEventType type, Actor player)
        {
            bool has_events = false;
            foreach (NarrativeEvent evt in events_list)
            {
                if (evt.trigger_type == type && evt.AreConditionsMet(player, this))
                    has_events = true;
            }
            return has_events;
        }

        public bool HasImportantEvents(NarrativeEventType type, Actor player)
        {
            bool has_events = false;
            foreach (NarrativeEvent evt in events_list)
            {
                if (evt.trigger_type == type && evt.important && evt.AreConditionsMet(player, this))
                    has_events = true;
            }
            return has_events;
        }

        public bool IsPlayer()
        {
            if (data != null)
                return data.is_player;
            return false;
        }

        public bool IsSelf()
        {
            return true;
        }

        public bool IsInRange(Vector3 pos) {
            float dist = (transform.position - pos).magnitude;
            return dist < trigger_range;
        }

        public static Actor GetPlayerActor()
        {
            foreach (Actor actor in actor_list)
            {
                if (actor.IsPlayer() && actor.enabled)
                    return actor;
            }
            return null;
        }

        public static Actor Get(ActorData actor_data)
        {
            foreach (Actor actor in actor_list)
            {
                if (actor.enabled && actor.data == actor_data)
                    return actor;
            }
            return null;
        }

        public static List<Actor> GetAllActor(ActorData actor_data)
        {
            List<Actor> valid_list = new List<Actor>();
            foreach (Actor actor in actor_list)
            {
                if (actor.enabled && actor.data == actor_data)
                    valid_list.Add(actor);
            }
            return valid_list;
        }

        public static Actor GetNearestActor(ActorData actor_data, Vector3 pos, float range = 999f)
        {
            float min_dist = range;
            Actor nearest = null;
            foreach (Actor actor in actor_list)
            {
                float dist = (actor.transform.position - pos).magnitude;
                if (actor.enabled && actor_data == actor.data && dist < min_dist)
                {
                    min_dist = dist;
                    nearest = actor;
                }
            }
            return nearest;
        }

        public static Actor GetNearestNPC(Vector3 pos, float range = 999f)
        {
            float min_dist = range;
            Actor nearest = null;
            foreach (Actor actor in actor_list)
            {
                float dist = (actor.transform.position - pos).magnitude;
                if (actor.enabled && !actor.IsPlayer() && dist < min_dist)
                {
                    min_dist = dist;
                    nearest = actor;
                }
            }
            return nearest;
        }

        public static Actor GetNearest(Vector3 pos, float range = 999f)
        {
            float min_dist = range;
            Actor nearest = null;
            foreach (Actor actor in actor_list)
            {
                float dist = (actor.transform.position - pos).magnitude;
                if (actor.enabled && dist < min_dist)
                {
                    min_dist = dist;
                    nearest = actor;
                }
            }
            return nearest;
        }

        public static List<Actor> GetAll()
        {
            return actor_list ;
        }

    }

}