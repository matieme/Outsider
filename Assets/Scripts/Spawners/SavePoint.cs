﻿using System;
using System.Collections.Generic;
using Managers;
using Metadata;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    [RequireComponent(typeof(SaveGUID))]
    public class SavePoint : MonoBehaviour
    {
        [Serializable]
        public class SavePointData
        {
            public string ObjectId;
            public Vector3 Position;
            public string SceneName;
        }
        
        public UnityEvent OnSaveEvent;
        public UnityEvent OnLoadEvent;
        public UnityEvent OnSaveUsedEvent;

        private bool _isUsed;
        private SaveGUID _uniqueId;

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void LoadData()
        {
            _isUsed = true;
            
            OnLoadEvent.Invoke();
            OnSaveUsedEvent.Invoke();
            
            Log("Game Loaded!");
        }
    
        private void SaveData()
        {
            _isUsed = true;

            var currentScene = SceneManager.GetActiveScene().name;
            
            var dataToSave = new SavePointData { Position = transform.position , SceneName = currentScene, ObjectId = _uniqueId.GameObjectId};
            var serializedData = JsonUtility.ToJson(dataToSave);
            
            PlayerPrefs.SetString(SaveKeys.LastSave, serializedData);
            PlayerPrefs.SetString(string.Format(SaveKeys.LastSaveScene, currentScene), JsonUtility.ToJson(dataToSave));
            PlayerPrefs.SetString(string.Format(SaveKeys.UsedSave, _uniqueId.GameObjectId), JsonUtility.ToJson(dataToSave));
            
            OnSaveEvent.Invoke();
            Log("Game Saved!");
            
            var data = new Dictionary<string, object>
            {
                {"level_name", SceneManager.GetActiveScene().name},
                {"character_health", GameManager.Instance.Character.Stats.CurrentHealth},
                {"character_spirit", GameManager.Instance.Character.Stats.CurrentSpirit},
                {"level_duration", GameManager.Instance.CurrentLevelDuration},
                {"current_session_time", GameManager.Instance.CurrentSessionTime}
            };
            
            AnalyticsEvent.Custom("save_point", data);
        }

        private void Awake()
        {
            _uniqueId = GetComponent<SaveGUID>();
        }

        private void Start()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var saveKey = string.Format(SaveKeys.LastSaveScene, currentScene);
            
            if (PlayerPrefs.HasKey(saveKey) && JsonUtility.FromJson<SavePointData>(PlayerPrefs.GetString(saveKey)).ObjectId == _uniqueId.GameObjectId)
            {
                LoadData();
            }
            else if (PlayerPrefs.HasKey(string.Format(SaveKeys.UsedSave, _uniqueId.GameObjectId)))
            {
                OnSaveUsedEvent.Invoke();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isUsed) return;
        
            SaveData();
        }
        
        private static void Log(string message)
        {
#if DEBUG
            Debug.Log(string.Format(FormatedLog.Save, message));
#endif
        }
    }
}