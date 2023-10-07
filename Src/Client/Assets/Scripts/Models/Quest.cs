using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using Managers;

namespace Models
{
    public class Quest
    {
        public QuestDefine Define;
        public NQuestInfo Info;

        public Quest()
        {

        }

        public Quest(NQuestInfo info)
        {
            Info = info;
            Define = DataManager.Instance.Quests[info.QuestId];
        }

        public Quest(QuestDefine define)
        {
            Define = define;
            Info = null;
        }

        public string GetTypeName()
        {
            return EnumUtil.GetEnumDescription(Define.Type);
        }
    }
}
