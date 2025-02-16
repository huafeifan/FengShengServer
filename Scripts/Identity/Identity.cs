
using LoginServer.Game;
using System.Collections.Generic;

namespace FengShengServer
{
    public class Identity
    {
        /// <summary>
        /// 根据房间椅子数量得到对应的身份数量列表并打乱
        /// </summary>
        /// <param name="roomInfo"></param>
        /// <returns></returns>
        public static List<IdentityType> GetIdentityList(RoomInfo roomInfo)
        {
            int playerCount = roomInfo.GetChairCount();
            var result = new List<IdentityType>();
            switch (playerCount)
            {
                case 3:
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.TeGong);
                    result.Add(IdentityType.TeGong);
                    break;
                case 4:
                case 6:
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.TeGong);
                    result.Add(IdentityType.TeGong);
                    break;
                case 5:
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.TeGong);
                    break;
                case 7:
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.QianFu);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.JunQing);
                    result.Add(IdentityType.TeGong);
                    break;
            }
            result.FisherYatesShuffle();
            return result;
        }

        public static IIdentity GetIdentity(IdentityType identity)
        {
            switch (identity)
            {
                case IdentityType.JunQing:
                    return new JunQing();
                case IdentityType.QianFu:
                    return new QianFu();
                case IdentityType.TeGong:
                    return new TeGong();
            }
            return null;
        }
    }
}
