﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;

        public CharacterManager()
        {

        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }
        public void Clear()
        {
            int[] keys = Characters.Keys.ToArray();
            foreach (var key in keys)
            {
                RemoveCharacter(key);
            }
            Characters.Clear();
        }

        public void AddCharacter(NCharacterInfo cha)
        {
            Debug.Log($"AddCharacter: {cha.Id}: {cha.Name} Map: {cha.mapId} Entity: {cha.Entity.String()}");
            Character character = new Character(cha);
            Characters[cha.Id] = character;
            EntityManager.Instance.AddEntity(character);
            if (OnCharacterEnter != null)
            {
                OnCharacterEnter(character);
            }
        }

        public void RemoveCharacter(int characterId)
        {
            Debug.Log($"RemoveCharacter:{characterId}");
            if (Characters.ContainsKey(characterId))
            {
                EntityManager.Instance.RemoveEntity(Characters[characterId].Info.Entity);
                if (OnCharacterLeave != null)
                {
                    OnCharacterLeave(Characters[characterId]);
                }
                Characters.Remove(characterId);
            }
        }

    }
}
