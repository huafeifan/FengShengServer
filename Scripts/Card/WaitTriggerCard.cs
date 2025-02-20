using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public class WaitTriggerCard
    {
        public string UserName { get; set; }
        public ICard CardXiaoGuo { get; set; }
        public CardType CardInfo { get; set; }
        /// <summary>
        /// 是否被识破
        /// </summary>
        public bool IsShiPo { get; set; }
    }

}
