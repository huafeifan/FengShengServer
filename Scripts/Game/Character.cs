
using LoginServer.Game;
using System.Collections.Generic;
using System.Linq;

namespace FengShengServer
{
    public class Character
    {
        /// <summary>
        /// 角色卡配置
        /// </summary>
        public static List<CharacterType> CharacterConfigList = new List<CharacterType>()
        {
            CharacterType.LaoJin, 
            CharacterType.DaMeiNv, 
            CharacterType.EmeiFeng
        };

        /// <summary>
        /// 本局游戏角色池
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
