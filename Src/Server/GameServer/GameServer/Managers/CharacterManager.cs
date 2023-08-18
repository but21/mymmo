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
            Characters[tCharacter.ID] = character;
            return character;
        }

        public void RemoveCharacter(int characterId)
        {
            Characters.Remove(characterId);
        }
    }
}
