using System;
using System.Collections.Generic;
using System.Linq;

namespace FengShengServer
{
    public class Room
    {
        private CSConnect mCSConnect;

        private bool mIsDebug;

        public void SetCSConnect(CSConnect cSConnect)
        {
            mCSConnect = cSConnect;
        }

        /// <summary>
        /// 是否打印房间模块日志
        /// </summary>
        /// <param name="flag"></param>
        public void SetDebug(bool flag)
        {
            mIsDebug = flag;
        }

        public void Start()
        {
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.RoomList_C2S, OnReceiveRoomList);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.CreateRoom_C2S, OnReceiveCreateRoom);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.EnterRoom_C2S, OnReceiveEnterRoom);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.ExitRoom_C2S, OnReceiveExitRoom);
        }

        public void Close()
        {
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.RoomList_C2S, OnReceiveRoomList);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.CreateRoom_C2S, OnReceiveCreateRoom);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.EnterRoom_C2S, OnReceiveEnterRoom);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.ExitRoom_C2S, OnReceiveExitRoom);
        }

        private void OnReceiveRoomList(object obj)
        {
            LoginServer.Room.RoomList_S2C sendData = new LoginServer.Room.RoomList_S2C();

            var roomList = RoomDataManager.Instance.RoomList;
            for (int i = 1; i <= roomList.Count; i++)
            {
                if (roomList[i] != null && roomList[i].IsOpen == true)
                {
                    var roomInfo = new LoginServer.Room.RoomInfo();
                    RoomDataManager.Instance.RoomInfoConvert(roomList[i], roomInfo);
                    sendData.RoomList.Add(roomInfo);
                }
            }
            ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.RoomList_S2C, sendData);
        }

        private void OnReceiveCreateRoom(object obj)
        {
            var data = obj as LoginServer.Room.CreateRoom_C2S;
            if (data == null)
                return;

            //尝试添加房间
            if (RoomDataManager.Instance.TryAddRoom(data.RoomName, data.ChairCount, out RoomInfo roomInfo))
            {
                //设置服务端数据
                mCSConnect.UserData.SetRoomInfo(roomInfo);

                //设置createRoomData
                var sendRoomInfo = new LoginServer.Room.RoomInfo();
                var createRoomData = new LoginServer.Room.CreateRoom_S2C();
                RoomDataManager.Instance.RoomInfoConvert(roomInfo, sendRoomInfo);
                createRoomData.RoomInfo = sendRoomInfo;
                createRoomData.Code = LoginServer.Room.CreateRoom_S2C.Types.Ret_Code.Success;
                createRoomData.Msg = "创建成功";

                //单播创建房间信息
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.CreateRoom_S2C, createRoomData);

                //设置enterRoomData
                var enterRoomData = new LoginServer.Room.EnterRoom_S2C();
                enterRoomData.RoomInfo = sendRoomInfo;
                enterRoomData.Code = LoginServer.Room.EnterRoom_S2C.Types.Ret_Code.Success;
                enterRoomData.Msg = "进入成功";

                //单播进入房间信息
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.EnterRoom_C2S, createRoomData);
            }
            else
            {
                var createRoomData = new LoginServer.Room.CreateRoom_S2C();
                createRoomData.RoomInfo = null;
                createRoomData.Code = LoginServer.Room.CreateRoom_S2C.Types.Ret_Code.Failed;
                createRoomData.Msg = "创建失败,房间数量已达到上限";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.CreateRoom_S2C, createRoomData);
            }
        }

        private void OnReceiveEnterRoom(object obj)
        {
            var data = obj as LoginServer.Room.EnterRoom_C2S;
            if (data == null)
                return;

            var enterRoomData = new LoginServer.Room.EnterRoom_S2C();
            var roomInfo = RoomDataManager.Instance.GetRoomInfo(data.RoomNub);
            if (roomInfo == null || roomInfo.IsOpen == false)
            {
                enterRoomData.RoomInfo = null;
                enterRoomData.Code = LoginServer.Room.EnterRoom_S2C.Types.Ret_Code.Failed;
                enterRoomData.Msg = "房间不存在";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.EnterRoom_S2C, enterRoomData);
            }
            else if (roomInfo.IsFull() == true)
            {
                enterRoomData.RoomInfo = null;
                enterRoomData.Code = LoginServer.Room.EnterRoom_S2C.Types.Ret_Code.Failed;
                enterRoomData.Msg = "房间已满";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.EnterRoom_S2C, enterRoomData);
            }
            else
            {
                var user = UserDataManager.Instance.GetUserData(data.UserName);
                if (user == null)
                {
                    enterRoomData.RoomInfo = null;
                    enterRoomData.Code = LoginServer.Room.EnterRoom_S2C.Types.Ret_Code.Failed;
                    enterRoomData.Msg = "用户数据丢失";
                    ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.EnterRoom_S2C, enterRoomData);
                }
                else 
                {
                    //设置服务端数据
                    roomInfo.AddUser(user);

                    //设置enterRoomData
                    var sendRoomInfo = new LoginServer.Room.RoomInfo();
                    RoomDataManager.Instance.RoomInfoConvert(roomInfo, sendRoomInfo);
                    enterRoomData.RoomInfo = sendRoomInfo;
                    enterRoomData.Code = LoginServer.Room.EnterRoom_S2C.Types.Ret_Code.Success;
                    enterRoomData.Msg = "进入成功";

                    //单播进入房间信息
                    ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.EnterRoom_S2C, enterRoomData);

                    //设置roomInfoChangeData
                    var roomInfoChangeData = new LoginServer.Room.RoomInfoChange_S2C();
                    roomInfoChangeData.RoomInfo = sendRoomInfo;

                    //获取多播连接列表
                    var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();

                    //多播房间信息变更
                    ProtosManager.Instance.Multicast(connectList, CmdConfig.RoomInfoChange_S2C, roomInfoChangeData);
                }
            }
        }

        private void OnReceiveExitRoom(object obj)
        {
            var data = obj as LoginServer.Room.ExitRoom_C2S;
            if (data == null)
                return;

            var exitRoomData = new LoginServer.Room.ExitRoom_S2C();
            var roomInfo = RoomDataManager.Instance.GetRoomInfo(data.RoomNub);

            if (roomInfo == null || roomInfo.IsOpen == false)
            {
                exitRoomData.Code = LoginServer.Room.ExitRoom_S2C.Types.Ret_Code.Failed;
                exitRoomData.Msg = "房间不存在";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.ExitRoom_S2C, exitRoomData);
                return;
            }

            //房主退出，所有玩家都被踢出
            if (roomInfo.IsRoomOwner(data.UserName))
            {
                //设置exitRoomData
                exitRoomData.Code = LoginServer.Room.ExitRoom_S2C.Types.Ret_Code.Success;
                exitRoomData.Msg = "已离开房间";

                //单播房主离开信息
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.ExitRoom_S2C, exitRoomData);

                //获取房间其他用户连接列表
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).Where(c => c.ID != mCSConnect.ID).ToList();

                //多播玩家离开房间
                exitRoomData.Msg = "房主已离开房间";
                ProtosManager.Instance.Multicast(connectList, CmdConfig.ExitRoom_S2C, exitRoomData);

                RoomDataManager.Instance.CloseRoom(roomInfo.RoomNub);
                return;
            }

            //非房主退出
            if (roomInfo.RemoveUser(data.UserName))
            {
                exitRoomData.Code = LoginServer.Room.ExitRoom_S2C.Types.Ret_Code.Success;
                exitRoomData.Msg = "退出成功";

                //单播用户退出房间信息
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.ExitRoom_S2C, exitRoomData);

                //设置roomInfoChangeData
                var roomInfoChangeData = new LoginServer.Room.RoomInfoChange_S2C();
                RoomDataManager.Instance.RoomInfoConvert(roomInfo, roomInfoChangeData.RoomInfo);

                //获取多播连接列表
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();

                //多播房间信息变更
                ProtosManager.Instance.Multicast(connectList, CmdConfig.RoomInfoChange_S2C, roomInfoChangeData);
            }
            else
            {
                exitRoomData.Code = LoginServer.Room.ExitRoom_S2C.Types.Ret_Code.Failed;
                exitRoomData.Msg = "用户不在房间内";

                //单播用户退出房间失败信息
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.ExitRoom_S2C, exitRoomData);
            }

        }
    }
}
