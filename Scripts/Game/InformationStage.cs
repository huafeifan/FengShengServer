
using LoginServer.Game;
using System.Collections.Generic;

namespace FengShengServer
{
    public enum InformationStage
    {
        None,
        /// <summary>
        /// �ȴ��鱨����
        /// </summary>
        InformationDeclaration,
        /// <summary>
        /// �ȴ������鱨
        /// </summary>
        WaitInformationTransmit,
        /// <summary>
        /// �����鱨
        /// </summary>
        InformationTransmit,
        /// <summary>
        /// �ȴ���ҽ����鱨
        /// </summary>
        WaitInformationReceive,
        /// <summary>
        /// ��ҽ����鱨
        /// </summary>
        InformationReceive,
    }
}
