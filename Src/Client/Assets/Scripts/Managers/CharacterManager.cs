using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public UnityAction<Character> OnCharacterEnter;

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
            Characters.Clear();
        }

        public void AddCharacter(NCharacterInfo cha)
        {
            Debug.Log($"AddCharacter: {cha.Id}: {cha.Name} Map: {cha.mapId} Entity: {cha.Entity.String()}");
            Character character = new Character(cha);
            Characters[cha.Id] = character;

            if(OnCharacterEnter != null)
            {
                OnCharacterEnter(character);
            }
        }

        public void RemoveCharacter(int  characterId)
        {
            Characters.Remove(characterId);
        }

    }
}
