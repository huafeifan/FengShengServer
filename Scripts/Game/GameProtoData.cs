using System.Collections.Generic;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public class GameProtoData
    {

        public GameTurnStart_C2S GameTurnStart_C2S { get; set; }
        public DealCards_C2S DealCards_C2S { get; set; }
        public GameTurnOpertateEnd_C2S GameTurnOpertateEnd_C2S { get; set; }
        public bool IsGameTurnOpertateEnd { get; set; }
        public GameTurnDisCard_C2S GameTurnDisCard_C2S { get; set; }
        public GameTurnEnd_C2S GameTurnEnd_C2S { get; set; }
        public InformationDeclaration_C2S InformationDeclaration_C2S { get; set; }
        public InformationTransmitReady_C2S InformationTransmitReady_C2S { get; set; }
        public WaitInformationReceive_C2S WaitInformationReceive_C2S { get; set; }
        public Queue<PlayHandCardResponse_C2S> PlayHandCardResponse_C2SQueue { get; set; }
        public PlayHandCard_C2S PlayHandCard_C2S { get; set; }
        public Queue<AskUseShiPo_C2S> AskUseShiPo_C2SQueue { get; set; }
        public TriggerCardEnd_C2S TriggerCardEnd_C2S { get; set; }

        public void Clear()
        {
            GameTurnStart_C2S = null;
            DealCards_C2S = null;
            GameTurnOpertateEnd_C2S = null;
            IsGameTurnOpertateEnd = false;
            GameTurnDisCard_C2S = null;
            InformationDeclaration_C2S = null;
            InformationTransmitReady_C2S = null;
            WaitInformationReceive_C2S = null;
            PlayHandCardResponse_C2SQueue?.Clear();
            PlayHandCard_C2S = null;
            AskUseShiPo_C2SQueue?.Clear();
            TriggerCardEnd_C2S = null;
        }
    }
}
