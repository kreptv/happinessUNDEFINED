using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueQuests
{
    [System.Serializable]
    public class DialogueMessageData
    {
        public string actor_id;
        public string actor_title;
        public string text;

        public DialogueMessageData(string actor_id, string actor_title, string text)
        {
            this.actor_id = actor_id;
            this.actor_title = actor_title;
            this.text = text;
        }
    }

    [System.Serializable]
    public class NarrativeData
    {
        public string filename;
        public string version;
        public DateTime last_save;
        public int player_id;
        public float time;

        //Dialogue data
        public List<DialogueMessageData> dialogue_history = new List<DialogueMessageData>();
        public Dictionary<string, int> trigger_counts = new Dictionary<string, int>();
        public Dictionary<string, float> trigger_interval = new Dictionary<string, float>();

        //Quest data
        public Dictionary<string, int> quests_status = new Dictionary<string, int>(); //0=NotStarted, 1=Ongoing, 2=Completed, 3=Failed
        public Dictionary<string, int> quests_values = new Dictionary<string, int>(); //ID is quest_id+progress_id
        public Dictionary<string, int> quests_steps = new Dictionary<string, int>(); //ID is quest_id

        //Custom data
        public Dictionary<string, int> custom_values_int = new Dictionary<string, int>();
        public Dictionary<string, float> custom_values_float = new Dictionary<string, float>();
        public Dictionary<string, string> custom_values_str = new Dictionary<string, string>();
        public Dictionary<string, int> actor_values = new Dictionary<string, int>();

        public static Func<int, NarrativeData> getData;
        public static Func<List<NarrativeData>> getDataAll;
        public static Action<int, NarrativeData> overrideData;

        private static string file_loaded = "";
        private static NarrativeData narrative_data = null;

        public const string last_save_id = "last_save_dq";
        public const string extension = ".dq";

        public NarrativeData(string filename) {
            this.filename = filename;
            version = Application.version;
            last_save = DateTime.Now;
        }

        public NarrativeData(string filename, int player_id)
        {
            this.filename = filename;
            version = Application.version;
            last_save = DateTime.Now;
            this.player_id = player_id;
        }

        public void FixData()
        {
            //Fix data when data version is different
            if (dialogue_history == null)
                dialogue_history = new List<DialogueMessageData>();
            if (trigger_counts == null)
                trigger_counts = new Dictionary<string, int>();
            if (trigger_interval == null)
                trigger_interval = new Dictionary<string, float>();

            if (quests_status == null)
                quests_status = new Dictionary<string, int>();
            if (quests_values == null)
                quests_values = new Dictionary<string, int>();
            if (quests_steps == null)
                quests_steps = new Dictionary<string, int>();

            if (custom_values_int == null)
                custom_values_int = new Dictionary<string, int>();
            if (custom_values_float == null)
                custom_values_float = new Dictionary<string, float>();
            if (custom_values_str == null)
                custom_values_str = new Dictionary<string, string>();
            if (actor_values == null)
                actor_values = new Dictionary<string, int>();
        }

        public void AddToHistory(DialogueMessageData msg)
        {
            dialogue_history.Add(msg);
        }

        public void SetTriggerCount(string event_id, int value)
        {
            if (!string.IsNullOrWhiteSpace(event_id))
            {
                trigger_counts[event_id] = value;
            }
        }

        public int GetTriggerCount(string event_id)
        {
            if (trigger_counts.ContainsKey(event_id))
                return trigger_counts[event_id];
            return 0;
        }

        public void SetLastInterval(string event_id, float value)
        {
            if (!string.IsNullOrWhiteSpace(event_id))
                trigger_interval[event_id] = value;
        }

        public float GetLastInterval(string event_id)
        {
            if (trigger_interval.ContainsKey(event_id))
                return trigger_interval[event_id];
            return float.MinValue;
        }

        public void StartQuest(string quest_id)
        {
            if(!IsQuestStarted(quest_id))
                quests_status[quest_id] = 1;
        }

        public void CancelQuest(string quest_id)
        {
            if (IsQuestStarted(quest_id))
                quests_status[quest_id] = 0;
        }

        public void CompleteQuest(string quest_id)
        {
            if (IsQuestStarted(quest_id))
                quests_status[quest_id] = 2;
        }

        public void FailQuest(string quest_id)
        {
            if (IsQuestStarted(quest_id))
                quests_status[quest_id] = 3;
        }

        public int GetQuestStatus(string quest_id)
        {
            if (quests_status.ContainsKey(quest_id))
                return quests_status[quest_id];
            return 0;
        }

        public bool IsQuestStarted(string quest_id)
        {
            int status = GetQuestStatus(quest_id);
            return status >= 1;
        }

        public bool IsQuestActive(string quest_id)
        {
            int status = GetQuestStatus(quest_id);
            return status == 1;
        }

        public bool IsQuestCompleted(string quest_id)
        {
            int status = GetQuestStatus(quest_id);
            return status == 2;
        }

        public bool IsQuestFailed(string quest_id)
        {
            int status = GetQuestStatus(quest_id);
            return status == 3;
        }

        public void AddQuestValue(string quest_id, string variable_id, int value)
        {
            string id = quest_id + "-" + variable_id;
            if (quests_values.ContainsKey(id))
                quests_values[id] += value;
            else
                quests_values[id] = value;
        }

        public void SetQuestValue(string quest_id, string variable_id, int value)
        {
            string id = quest_id + "-" + variable_id;
            quests_values[id] = value;
        }

        public int GetQuestValue(string quest_id, string variable_id)
        {
            string id = quest_id + "-" + variable_id;
            if (quests_values.ContainsKey(id))
                return quests_values[id];
            return 0;
        }

        public void AddQuestStep(string quest_id, int value)
        {
            if (quests_steps.ContainsKey(quest_id))
                quests_steps[quest_id] += value;
            else
                quests_steps[quest_id] = value;
        }

        public void SetQuestStep(string quest_id,  int value)
        {
            quests_steps[quest_id] = value;
        }

        public int GetQuestStep(string quest_id)
        {
            if (quests_steps.ContainsKey(quest_id))
                return quests_steps[quest_id];
            return 0;
        }

        public void SetActorValue(string actor_id, int value)
        {
            if (!string.IsNullOrWhiteSpace(actor_id))
            {
                actor_values[actor_id] = value;
            }
        }

        public bool HasActorValue(string actor_id)
        {
            return actor_values.ContainsKey(actor_id);
        }

        public int GetActorValue(string actor_id)
        {
            if (actor_values.ContainsKey(actor_id))
            {
                return actor_values[actor_id];
            }
            return 0;
        }

        public void SetCustomInt(string val_id, int value)
        {
            if (!string.IsNullOrWhiteSpace(val_id))
            {
                custom_values_int[val_id] = value;
            }
        }

        public void SetCustomFloat(string val_id, float value)
        {
            if (!string.IsNullOrWhiteSpace(val_id))
            {
                custom_values_float[val_id] = value;
            }
        }

        public void SetCustomString(string val_id, string value)
        {
            if (!string.IsNullOrWhiteSpace(val_id))
            {
                custom_values_str[val_id] = value;
            }
        }

        public bool HasCustomInt(string val_id)
        {
            return custom_values_int.ContainsKey(val_id);
        }

        public bool HasCustomFloat(string val_id)
        {
            return custom_values_float.ContainsKey(val_id);
        }

        public bool HasCustomString(string val_id)
        {
            return custom_values_str.ContainsKey(val_id);
        }

        public int GetCustomInt(string val_id)
        {
            if (custom_values_int.ContainsKey(val_id))
            {
                return custom_values_int[val_id];
            }
            return 0;
        }

        public float GetCustomFloat(string val_id)
        {
            if (custom_values_float.ContainsKey(val_id))
            {
                return custom_values_float[val_id];
            }
            return 0;
        }

        public string GetCustomString(string val_id)
        {
            if (custom_values_str.ContainsKey(val_id))
            {
                return custom_values_str[val_id];
            }
            return "";
        }

        public void DeleteCustomInt(string val_id)
        {
            custom_values_int.Remove(val_id);
        }

        public void DeleteCustomFloat(string val_id)
        {
            custom_values_float.Remove(val_id);
        }

        public void DeleteCustomString(string val_id)
        {
            custom_values_str.Remove(val_id);
        }

        // --- Save/load/new --------

        public void Save()
        {
            Save(file_loaded, this);
        }

        public static void Save(string filename, NarrativeData data)
        {
            if (!string.IsNullOrWhiteSpace(filename) && data != null)
            {
                data.filename = filename;
                data.last_save = DateTime.Now;
                data.version = Application.version;
                narrative_data = data;
                file_loaded = filename;

                SaveTool.SaveFile<NarrativeData>(filename + extension, data);
                SetLastSave(filename);
            }
        }

        public static void NewGame()
        {
            NewGame(GetLastSave()); //default name
        }

        //You should reload the scene right after NewGame
        public static NarrativeData NewGame(string filename)
        {
            file_loaded = filename;
            narrative_data = new NarrativeData(filename);
            narrative_data.FixData();
            return narrative_data;
        }

        public static NarrativeData Load(string filename)
        {
            if (narrative_data == null || file_loaded != filename)
            {
                narrative_data = SaveTool.LoadFile<NarrativeData>(filename + extension);
                if (narrative_data != null)
                {
                    file_loaded = filename;
                    narrative_data.FixData();
                }
            }
            return narrative_data;
        }

        public static NarrativeData LoadLast()
        {
            return AutoLoad(GetLastSave());
        }

        //Load if found, otherwise new game
        public static NarrativeData AutoLoad(string filename)
        {
            if (narrative_data == null)
                narrative_data = Load(filename);
            if (narrative_data == null)
                narrative_data = NewGame(filename);
            return narrative_data;
        }

        public static void SetLastSave(string filename)
        {
            if (SaveTool.IsValidFilename(filename))
            {
                PlayerPrefs.SetString(last_save_id, filename);
            }
        }

        public static string GetLastSave()
        {
            string name = PlayerPrefs.GetString(last_save_id, "");
            if (string.IsNullOrEmpty(name))
                name = "player"; //Default name
            return name;
        }

        public static bool HasLastSave()
        {
            return HasSave(GetLastSave());
        }

        public static bool HasSave(string filename)
        {
            return SaveTool.DoesFileExist(filename + extension);
        }

        public static void Unload()
        {
            narrative_data = null;
            file_loaded = "";
        }

        public static void Delete(string filename)
        {
            if (file_loaded == filename)
            {
                narrative_data = new NarrativeData(filename);
                narrative_data.FixData();
            }

            SaveTool.DeleteFile(filename + extension);
        }

        public static void Override(NarrativeData data)
        {
            narrative_data = data;
        }

        public static bool IsLoaded()
        {
            return narrative_data != null && !string.IsNullOrEmpty(file_loaded);
        }
		
		public static void Override(int player_id, NarrativeData data)
        {
            if (overrideData != null)
                overrideData.Invoke(player_id, data);
            else
                narrative_data = data;
        }

		public static NarrativeData Get(int player_id)
        {
            if (getData != null)
                return getData.Invoke(player_id);
            return narrative_data;
        }

        public static NarrativeData Get()
        {
            if (getData != null)
                return getData.Invoke(-1);
            return narrative_data;
        }
		
		public static List<NarrativeData> GetAll()
        {
            if (getDataAll != null)
                return getDataAll.Invoke();
            List<NarrativeData> list = new List<NarrativeData>();
            list.Add(narrative_data);
            return list;
        }
    }

}