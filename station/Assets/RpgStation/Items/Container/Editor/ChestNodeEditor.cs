﻿using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Editor;
using UnityEditor;
using UnityEngine;

namespace Station
{
    [CustomEditor(typeof(ChestNode))]
    public class ChestNodeEditor : UnityEditor.Editor
    {
        private ItemsDb _itemDb;
        private ChestNodesDb _chestDb;
        private bool _showDefaultData;
        
        private void OnEnable()
        {
            _itemDb = (ItemsDb) EditorStatic.GetDb(typeof(ItemsDb));
            _chestDb = (ChestNodesDb) EditorStatic.GetDb(typeof(ChestNodesDb));
        }

        public override void OnInspectorGUI()
        {
            ChestNode component = (ChestNode) target;
            if (component == null)
            {
                return;
            }

            _showDefaultData = EditorStatic.LevelFoldout("Base Configuration", _showDefaultData, 32, Color.grey);
            if (_showDefaultData)
            {
                base.DrawDefaultInspector();
            }
            EditorGUILayout.Space();
            EditorStatic.DrawLargeLine();
            EditorStatic.DrawSectionTitle(22, "Chest Configuration");
            if (_chestDb.Count() == 0)
            {
                EditorGUILayout.HelpBox("the chest db is empty", MessageType.Warning);
            }
            else
            {
                
                string chestId = component.ChestNodeId;
                int entryIndex = 0;
                if (string.IsNullOrEmpty(chestId))
                {
                    chestId = _chestDb.GetKey(0);
                }

                entryIndex = _chestDb.GetIndex(chestId);
                entryIndex = EditorGUILayout.Popup("ChestNode: ", entryIndex, _chestDb.ListEntryNames());
                
                component.ChestNodeId = _chestDb.GetKey(entryIndex);
            }

            if (string.IsNullOrEmpty(component._id))
            {
                component._id = $"chest_id_{Guid.NewGuid().ToString()}";
            }

            component.StateSaved = EditorGUILayout.Toggle("State saved:", component.StateSaved);
            if (component.StateSaved)
            {
             
                EditorGUILayout.HelpBox(component._id, MessageType.None);
                if (EditorStatic.Button(true, 32, "Refresh save id", "arrow_refresh"))
                {
                    component._id = $"chest_id_{Guid.NewGuid().ToString()}";
                }
            }
            
            EditorStatic.DrawThinLine();
            EditorGUILayout.BeginVertical("box");
            var chestData = _chestDb.GetEntry(component.ChestNodeId);
            foreach (var loot in chestData.Loots)
            {
               
                var item = _itemDb.GetEntry(loot.ItemId);
                string textLine =  $"  | {loot.QuantityMin} - {loot.QuantityMax} | {item.Name.GetValue()} at {loot.Chance}%";
                EditorGUILayout.LabelField(textLine);
            }
            EditorGUILayout.EndVertical();
        }

    }

}
