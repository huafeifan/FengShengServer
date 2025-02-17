
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
        WaitInformationDeclaration,
        /// <summary>
        /// �ȴ������鱨
        /// </summary>
        WaitInformationTransmit,
        /// <summary>
        /// �ȴ���ҽ����鱨
        /// </summary>
        WaitInformationReceive,
        /// <summary>
        /// �ȴ��鱨���ݽ���
        /// </summary>
        WaitEnd,
    }
}
