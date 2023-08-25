using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;
using Common;

namespace GameServer.Managers
{
    class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

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

        public Character AddCharacter(TCharacter tCharacter)
        {
            Character character = new Character(CharacterType.Player, tCharacter);
            EntityManager.Instance.AddEntity(tCharacter.MapID, character);
            character.Info.Id = character.entityId;
            Characters[character.entityId] = character;
            return character;
        }

        public void RemoveCharacter(int characterId)
        {
            var character = Characters[characterId];
            EntityManager.Instance.RemoveEntity(character.Data.MapID, character);
            Characters.Remove(characterId);
        }
    }
}
