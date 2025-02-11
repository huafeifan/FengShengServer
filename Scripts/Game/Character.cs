
using LoginServer.Game;
using System.Collections.Generic;
using System.Linq;

namespace FengShengServer
{
    public class Character
    {
        /// <summary>
        /// ��ɫ������
        /// </summary>
        public static List<CharacterType> CharacterConfigList = new List<CharacterType>()
        {
            CharacterType.LaoJin, 
            CharacterType.DaMeiNv, 
            CharacterType.EmeiFeng
        };

        /// <summary>
        /// ������Ϸ��ɫ��
        /// </summary>
        public static List<CharacterType> CharacterList;

        public void Init()
        {
            CharacterList = CharacterConfigList.ToList();
            CharacterList.FisherYatesShuffle();
        }

        public List<CharacterType> GetCharacterChooseList()
        {
            return CharacterList.GetRange(0, 3);
        }
    }
}
