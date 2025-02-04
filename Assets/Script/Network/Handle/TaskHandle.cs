
using UnityEngine;
using System.Collections.Generic;
using Tianyu;

public class TaskHandle : CHandleBase
{
    public TaskHandle(CHandleMgr mgr)
        : base(mgr)
    {

    }

    public override void RegistAllHandle()
    {
        RegistHandle(MessageID.common_open_mission_dialog_ret, OpenDialogUI);
        RegistHandle(MessageID.common_mission_list_ret, TaskListResult);
        RegistHandle(MessageID.common_mission_complete_list_ret, CompleteTaskListResult);
        RegistHandle(MessageID.common_notice_reward_dialogue_common_ret, CompleteTaskReward);
        RegistHandle(MessageID.common_notice_common_ex_req, TaskCommonResult);
        RegistHandle(MessageID.common_mission_box_info_ret, CommonBoxInfoHandle);
        RegistHandle(MessageID.common_offer_reward_mission_list_ret, RewardMissionList);
        RegistHandle(MessageID.common_ask_daily_mission_ret, Daily_mission_ret);
        //RegistHandle(MessageID.Task_S2C_OpenDialog, OpenDialogUI);
        //RegistHandle(MessageID.Task_S2C_List, TaskListResult);
        //RegistHandle(MessageID.Task_S2C_CompleteList, CompleteTaskListResult);
        //RegistHandle(MessageID.Task_S2C_CompleteReward,CompleteTaskReward);
        //RegistHandle(MessageID.Task_S2C_notice_common_ex, TaskCommonResult);
		RegistHandle(MessageID.err_message_common_ret,ErrorMessage);
    }
	private bool ErrorMessage(CReadPacket packet)
	{
		//if (UIPromptBox.Instance != null) {
		//	UIPromptBox.Instance.ShowLabel ("慢点！请慢点！");
		//}
		return true;
	}
    private bool Daily_mission_ret(CReadPacket packet)
    {
        UIActivitiesManager.Instance.DailyHandler(packet.data);
        return true;
    }


    private bool RewardMissionList(CReadPacket packet)
    {
        Debug.Log("RewardMissionList悬赏任务");
        Dictionary<string, object> data = packet.data;
        int resolt = int.Parse(data["ret"].ToString());
        if (resolt == 0)
        {         
            UIActivitiesManager.Instance.RewardListHandler(data);
        }
        else
        {
            Debug.Log(data["desc"].ToString());
        }
        return true;
    }
    //日常任务奖励宝箱信息
    private bool CommonBoxInfoHandle(CReadPacket packet)
    {
        Debug.Log("CommonBoxInfoHandle日常任务奖励宝箱信息");
        Dictionary<string, object> data = packet.data;
        int resolt = int.Parse(data["ret"].ToString());
        if (resolt == 0)
        {
            if (data.ContainsKey("mState"))
            {
                int[] mState = data["mState"] as int[];
                if (mState.Length > 0)
                {
                    int dat = mState[0];
                    if ((int)dat != 0)
                    {
                        playerData.GetInstance().taskDataList.box1State = TaskManager.Single().GetBitValue(dat, 4, 4);
                        playerData.GetInstance().taskDataList.box2State = TaskManager.Single().GetBitValue(dat, 8, 4);
                        playerData.GetInstance().taskDataList.box3State = TaskManager.Single().GetBitValue(dat, 12, 4);
                        playerData.GetInstance().taskDataList.box4State = TaskManager.Single().GetBitValue(dat, 16, 4);
                        playerData.GetInstance().taskDataList.ActiveIndex = TaskManager.Single().GetBitValue(dat, 20, 8);
                        //if (Singleton<SceneManage>.Instance.Current == EnumSceneID.UI_MajorCity01)
                        //{
                        //    if (LivenessView.instance.IsShow())
                        //    {
                        //        LivenessView.instance.InitUI();
                        //    }
                        //}
                    }

                }

            }
        }
        else
        {
            Debug.Log(data["desc"].ToString());
        }
        return true;
    }

    /// <summary>
    /// 完成任务奖励
    /// </summary>
    /// <param name="packet"></param>
    /// <returns></returns>
    private bool CompleteTaskReward(CReadPacket packet)
    {
        Debug.Log("CompleteTaskReward任务奖励——---------------------------------------------------------------");
        Dictionary<string, object> data = packet.data;

        object[] itemList = data["rwdData"] as object[];
        TaskManager.Single().itemlist.Clear();
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                Dictionary<string, object> itemDataDic = itemList[i] as Dictionary<string, object>;
                if (itemDataDic != null)
                {
                    ItemData itemdata = new ItemData();
                    itemdata.Id = int.Parse(itemDataDic["id"].ToString());
                    itemdata.Count = int.Parse(itemDataDic["num"].ToString());
                    if (itemdata.Id == 0)
                    {
                        continue;
                    }
                    //if (itemdata.Id == 101 || itemdata.Id == 10101 || itemdata.Id == 1010101 || itemdata.Id == 102 || itemdata.Id == 103)
                    //{
                    //    itemdata.GradeTYPE = GradeType.Gray;
                    //}
                    //if (itemdata.Id == 101)
                    //{
                    //    itemdata.Name = "金币";
                    //    itemdata.GradeTYPE = GradeType.Gray;
                    //    itemdata.IconName = "jinbi";
                    //    itemdata.GoodsType = MailGoodsType.GoldType;
                    //    itemdata.Gold = itemdata.Count;
                    //}
                    //if (itemdata.Id == 10101)
                    //{
                    //    itemdata.Name = "钻石";
                    //    itemdata.GradeTYPE = GradeType.Gray;
                    //    itemdata.IconName = "zuanshi";
                    //    itemdata.GoodsType = MailGoodsType.DiamomdType;
                    //    itemdata.Diamond = itemdata.Count;
                    //}
                    //if (itemdata.Id == 1010101)
                    //{
                    //    itemdata.Name = "战队经验";
                    //    itemdata.GradeTYPE = GradeType.Gray;
                    //    itemdata.IconName = "zhandui-exp";
                    //    itemdata.GoodsType = MailGoodsType.ExE;
                    //    itemdata.Exe = itemdata.Count;
                    //}
                    //if (itemdata.Id == 102)
                    //{
                    //    itemdata.Name = "英雄经验";
                    //    itemdata.GradeTYPE = GradeType.Gray;
                    //    itemdata.IconName = "exp";
                    //    itemdata.GoodsType = MailGoodsType.HeroExp;
                    //    itemdata.HeroExp = itemdata.Count;

                    //}
                    //if (itemdata.Id == 103)
                    //{
                    //    itemdata.Name = "悬赏币";
                    //    itemdata.GradeTYPE = GradeType.Gray;
                    //    itemdata.IconName = "xuanshangbi";
                    //    itemdata.GoodsType = MailGoodsType.XuanshangGold;
                    //    itemdata.XuanshangGold = itemdata.Count;
                    //}
                    ItemNodeState itemnodestate = null;
                    if (GameLibrary.Instance().ItemStateList.ContainsKey(itemdata.Id))
                    {
                        itemnodestate = GameLibrary.Instance().ItemStateList[itemdata.Id];
                        itemdata.Name = itemnodestate.name;
                        itemdata.Types = itemnodestate.types;
                        itemdata.Describe = itemnodestate.describe;

                        itemdata.Sprice = itemnodestate.sprice;
                        itemdata.Piles = itemnodestate.piles;
                        itemdata.IconName = itemnodestate.icon_name;
                        switch (itemnodestate.grade)//(VOManager.Instance().GetCSV<ItemCSV>("Item").GetVO(itemdata.Id).grade)
                        {
                            case 1:
                                itemdata.GradeTYPE = GradeType.Gray;
                                break;
                            case 2:
                                itemdata.GradeTYPE = GradeType.Green;
                                break;
                            case 4:
                                itemdata.GradeTYPE = GradeType.Blue;
                                break;
                            case 7:
                                itemdata.GradeTYPE = GradeType.Purple;
                                break;
                            case 11:
                                itemdata.GradeTYPE = GradeType.Orange;
                                break;
                            case 16:
                                itemdata.GradeTYPE = GradeType.Red;
                                break;
                        }

                    }
                    if (itemdata.Types == 6)
                    {
                        itemdata.UiAtlas = ResourceManager.Instance().GetUIAtlas("UIHeroHead");
                    }
                    else
                    {
                        itemdata.UiAtlas = ResourceManager.Instance().GetUIAtlas("Prop");
                    }
                    TaskManager.Single().itemlist.Add(itemdata);
                }
            }
        }

        //不需要自己判断，只要服务器给我发送奖励协议，就弹出奖励面板(任务状态和任务列表 需要看服务器什么时候发给我 然后再做更新界面)
        if (TaskManager.Single().itemlist.Count > 0)
        {
            //任务奖励弹窗
            //TaskEffectManager.instance.ShowTaskEffect(TaskEffectType.SucceedTask);//任务完成会伴随任务奖励界面，目前方案是先出现任务完成特效 0.5s后显示再显示奖励面板
            UITaskEffectPanel.instance.ShowTaskEffect(TaskEffectType.SucceedTask);
            //Control.ShowGUI(GameLibrary.UITaskRewardPanel);
            //领取奖励后将该任务设置为已领取奖励，并从追踪列表中删除(这个放在读取任务列表和任务状态时候再处理)
            //Debug.LogError(TaskManager.Single().CurrentTaskItem);
            //TaskManager.Single().CurrentTaskItem.taskProgress = TaskProgress.Reward;
            //TaskManager.Single().ModifeTask(TaskManager.Single().CurrentTaskItem, TaskModifyType.Remove);
        }
        return true;
    }

    /// <summary>
    /// 打开对话框
    /// </summary>
    /// <param name="packet"></param>
    /// <returns></returns>
    public bool OpenDialogUI(CReadPacket packet)
    {
        Debug.Log("OpenDialogUIResult");
        Dictionary<string, object> data = packet.data;
        int resolt = int.Parse(data["ret"].ToString());
        if (resolt == 0)
        {
            bool isoperation = false;
            DialogItem dialogitem = new DialogItem();
            if (data.ContainsKey("title"))
            {
                if (data["title"] == null)
                {
                    dialogitem.title = "";
                }
                else
                {
                    dialogitem.title = data["title"].ToString();
                }
            }
            if (data.ContainsKey("disc"))
            {
                dialogitem.disc = data["disc"].ToString();
            }

            if (data.ContainsKey("opt1"))
            {
                isoperation = true;
                dialogitem.opt[0] = data["opt1"].ToString();
                dialogitem.opt[1] = data["opt2"].ToString();
                dialogitem.opt[2] = data["opt3"].ToString();
                dialogitem.opt[3] = data["opt4"].ToString();
                if (string.IsNullOrEmpty(dialogitem.opt[0]) && string.IsNullOrEmpty(dialogitem.opt[1]) && string.IsNullOrEmpty(dialogitem.opt[2]) && string.IsNullOrEmpty(dialogitem.opt[3]))
                {
                    isoperation = false;
                }
            }

            //if (TaskManager.Single().isAcceptTask)
            //{
            //    TaskEffectManager.instance.ShowTaskEffect(TaskEffectType.AcceptTask);
            //}

            dialogitem.msId = int.Parse(data["msId"].ToString());
            dialogitem.scrId = int.Parse(data["scrId"].ToString());
            dialogitem.user[0] = int.Parse(data["user1"].ToString());//如果是5010 5020的话 表示闲聊
            dialogitem.user[1] = int.Parse(data["user2"].ToString());
            dialogitem.user[2] = int.Parse(data["user3"].ToString());

            TaskManager.Single().CurrentShowDialogItem = dialogitem;
            if (isoperation)
            {
                //当前npcID 和 移动到npc要触发哪个类型事件
                //TaskOperation.Single().MoveToNpc(TaskOperation.Single().taskNpc.NPCID, TaskOperation.MoveToNpcType.OpenTaskList);
                TaskOperation.Single().MoveToNpc(TaskOperation.Single().currentTaskNpcID, TaskOperation.MoveToNpcType.OpenTaskList);
            }
            else
            {
                if (dialogitem.user[0] == 5010 || dialogitem.user[0] == 5020)
                {
                    //TaskOperation.Single().MoveToNpc(TaskOperation.Single().taskNpc.NPCID, TaskOperation.MoveToNpcType.Smalltalk);
                    TaskOperation.Single().MoveToNpc(TaskOperation.Single().currentTaskNpcID, TaskOperation.MoveToNpcType.Smalltalk);
                }
                else
                {
                    //if (TaskOperation.Single() != null && TaskOperation.Single().taskNpc != null)
                    //TaskOperation.Single().MoveToNpc(TaskOperation.Single().taskNpc.NPCID, TaskOperation.MoveToNpcType.OpenChatDialog);
                    if (TaskOperation.Single() != null && TaskOperation.Single().currentTaskNpcID != 0)
                        TaskOperation.Single().MoveToNpc(TaskOperation.Single().currentTaskNpcID, TaskOperation.MoveToNpcType.OpenChatDialog);
                }

            }
        }
        else
        {
            Debug.Log(data["desc"].ToString());
        }
        return true;
    }

    /// <summary>
    /// 任务列表
    /// </summary>
    /// <param name="packet"></param>
    /// <returns></returns>
    public bool TaskListResult(CReadPacket packet)
    {
        Debug.Log("TaskListResult任务列表");
        ClearTaskData();
        Dictionary<string, object> data = packet.data;
        int resolt = int.Parse(data["ret"].ToString());
        if (resolt == 0)
        {
            object[] missionlist = data["misinfo"] as object[];
            if (missionlist == null)
            {
                return false;
            }
            TaskInstructionsNode taskInstructionsNode = null;
            for (int i = 0; i < missionlist.Length; i++)
            {
                object[] taskDataDic = missionlist[i] as object[];
                if (i == 0)
                {
                    TaskItem taskitem = new TaskItem();
                    taskitem.taskindex = int.Parse(taskDataDic[0].ToString());//0是主线 其他的都是次线任务
                    taskitem.missionid = int.Parse(taskDataDic[1].ToString());
                    TaskDataNode taskDataNode = FSDataNodeTable<TaskDataNode>.GetSingleton().FindDataByType(taskitem.missionid);
                    //if (taskDataNode == null)
                    //{
                    //    continue;
                    //}
                    taskitem.tasknode = taskDataNode;
                    taskitem.scripid = int.Parse(taskDataDic[2].ToString());
                    taskitem.able = bool.Parse(taskDataDic[3].ToString());
                    //if (!TaskManager.Single().taskstate.ContainsKey(taskitem.missionid))
                    //{
                    //    continue;
                    //}
                    //if (!TaskManager.Single().TaskInstructionsNodeMap.ContainsKey(taskitem.missionid))
                    //{
                    //    continue;
                    //}
                    if (TaskManager.Single().TaskInstructionsNodeMap.ContainsKey(taskitem.missionid))
                    {
                        taskInstructionsNode = TaskManager.Single().TaskInstructionsNodeMap[taskitem.missionid];
                        int statevalue = TaskManager.Single().taskstate[taskInstructionsNode.indexarr];
                        int state = TaskManager.Single().GetBitValue(statevalue, taskInstructionsNode.zeroposition, taskInstructionsNode.length);
                        taskitem.taskProgress = (TaskProgress)state;
                    }
                    taskitem.parm0 = long.Parse(taskDataDic[4].ToString());
                    taskitem.parm1 = long.Parse(taskDataDic[5].ToString());//第一个数量
                    taskitem.parm2 = long.Parse(taskDataDic[6].ToString());//第二个数量
                    taskitem.parm3 = long.Parse(taskDataDic[7].ToString());
                    taskitem.npcid = long.Parse(taskDataDic[8].ToString());//当前任务的npc
                    taskitem.parm4 = long.Parse(taskDataDic[9].ToString());

                    ////针对杀怪任务掉落物 相关信息在这里接收
                    //if (taskDataNode != null && taskDataNode.Requiretype == 7)
                    //{
                    //    FubenTaskData fubenTaskData = new FubenTaskData();
                    //    // fubenTaskData.opt1 = int.Parse(data["opt1"].ToString());
                    //    //fubenTaskData.taskType = int.Parse(data["opt2"].ToString());
                    //    fubenTaskData.taskId = taskitem.missionid;
                    //    fubenTaskData.opt4 = taskitem.parm0;
                    //    fubenTaskData.opt5 = taskitem.parm1;//已经获得的掉落物数量
                    //    fubenTaskData.opt6 = taskitem.parm2;//怪物id

                    //    if (TaskManager.Single().TaskToSMGoodsDic.ContainsKey(fubenTaskData.taskId))
                    //    {
                    //        TaskManager.Single().TaskToSMGoodsDic[fubenTaskData.taskId] = fubenTaskData;
                    //    }
                    //    else
                    //    {
                    //        TaskManager.Single().TaskToSMGoodsDic.Add(fubenTaskData.taskId, fubenTaskData);
                    //    }
                    //    //存储杀怪掉落物的数量 杀怪掉落物id --  杀怪掉落物数量  (后端告诉我的是怪物id 读表转换)
                    //    if (TaskManager.Single().TaskToSMGoodsDic.ContainsKey(taskitem.missionid))
                    //    {
                    //        //long monsterId1 = TaskManager.Single().TaskToSMGoodsDic[taskitem.missionid].opt6;
                    //        long trackingIndex = taskDataNode.Opt2;//采集表id 得到怪物id
                    //        long itemId = 0;
                    //        if (FSDataNodeTable<CollectNode>.GetSingleton().DataNodeList.ContainsKey(trackingIndex))
                    //        {
                    //            itemId = FSDataNodeTable<CollectNode>.GetSingleton().DataNodeList[trackingIndex].collectid[0, 0];
                    //        }

                    //        if (TaskManager.Single().TaskSMGoodsCountDic.ContainsKey(itemId))
                    //        {
                    //            TaskManager.Single().TaskSMGoodsCountDic[itemId] = (int)fubenTaskData.opt5;
                    //        }
                    //        else
                    //        {
                    //            TaskManager.Single().TaskSMGoodsCountDic.Add(itemId, (int)fubenTaskData.opt5);
                    //        }

                    //    }

                    //}
                    if (TaskManager.Single().CurrentShowDialogItem != null)
                    {
                        if (taskitem.taskProgress == TaskProgress.Complete && taskitem.missionid == TaskManager.Single().CurrentShowDialogItem.msId)
                        {
                            TaskManager.Single().CurrentTaskItem = taskitem;
                        }
                    }

                    TaskManager.Single().ModifeTask(taskitem, TaskModifyType.Add);
                }
                if (i >= 1 && i <= 9)
                {

                }
            }
            if (Singleton<SceneManage>.Instance.Current == EnumSceneID.UI_MajorCity01 || Singleton<SceneManage>.Instance.Current == EnumSceneID.LGhuangyuan)
            {
                //Control.ShowGUI(GameLibrary.UITaskTracker);
            }

        }
        else
        {
            Debug.Log(data["desc"].ToString());
        }
        return true;
    }



    private void ClearTaskData()
    {
        TaskManager.NpcTaskStateDic.Clear();
        TaskManager.NpcTaskListDic.Clear();
        TaskManager.BranchList.Clear();
        TaskManager.MainTask = null;
        //TaskManager.TaskList.Clear();
        TaskManager.Single().TaskList = new Dictionary<TaskProgress, List<TaskItem>>()
        {
            { TaskProgress.CantAccept,new List<TaskItem>()},
            { TaskProgress.NoAccept,new List<TaskItem>()},
            { TaskProgress.Accept,new List<TaskItem>()},
            { TaskProgress.Complete,new List<TaskItem>()},
            { TaskProgress.Reward,new List<TaskItem>()}
        };
    }


    /// <summary>
    /// 任务完成列表 首先调用
    /// </summary>
    /// <param name="packet"></param>
    /// <returns></returns>
    public bool CompleteTaskListResult(CReadPacket packet)
    {
        Debug.Log("CompleteTaskListResult任务状态数组");
        Dictionary<string, object> data = packet.data;
        int resolt = int.Parse(data["ret"].ToString());
        if (resolt == 0)
        {
            TaskManager.Single().taskstate.Clear();
            //任务状态
            int[] mState = data["mState"] as int[];
            //根据表获取任务状态 数值 起始位置 长度
            for (int i = 0; i < mState.Length; i++)
            {
                TaskManager.Single().taskstate.Add(i, mState[i]);
            }
        }
        else
        {
            Debug.Log(data["desc"].ToString());
        }
        return true;
    }

    public bool TaskCommonResult(CReadPacket packet)
    {
        //{msgid=305, opt1=内部协议号/子协议号, opt2=类型/任务2/引导5, opt3= 预留/任务Id/1024引导解锁, opt4=预留/地图Id/引导起始位置,opt5= 预留/副本Id/引导Id,opt6= 预留,opt7= 预留,user1=内部协议内容 user2=发送人名字可为空 user3="保留"}
        Debug.Log("TaskCommonResult任务通用协议");
        Dictionary<string, object> data = packet.data;
        int resolt = int.Parse(data["ret"].ToString());
        if (resolt == 0)
        {
            switch (int.Parse(data["opt2"].ToString()))
            {
                case 2:
                    FubenTaskData fubenTaskData = new FubenTaskData();
                    fubenTaskData.opt1 = int.Parse(data["opt1"].ToString());
                    fubenTaskData.taskType = int.Parse(data["opt2"].ToString());
                    fubenTaskData.taskId = int.Parse(data["opt3"].ToString());
                    fubenTaskData.opt4 = long.Parse(data["opt4"].ToString());//副本任务 -- 地图id 采集任务--物品id 杀怪任务 --怪物id
                    fubenTaskData.opt5 = int.Parse(data["opt5"].ToString());//副本任务-- 副本id 采集任务--数量  杀怪任务 -- 怪物数量
                    fubenTaskData.opt6 = long.Parse(data["opt6"].ToString());//采集任务--物品id 杀怪任务 --怪物id
                    fubenTaskData.opt7 = int.Parse(data["opt7"].ToString());//采集任务--数量 杀怪任务 -- 怪物数量
                                                                            //fubenTaskData.user1 = data["user1"].ToString();
                                                                            //fubenTaskData.user2 = data["user2"].ToString();
                                                                            //fubenTaskData.user3 = data["user3"].ToString();
                                                                            /*1：对话；2：通关副本；3：采集；4：提升技能等级；5：提升英雄装备等级；6：杀怪；7：怪物掉落物；8：背包物品；9：指定地点；*/
                    if (FSDataNodeTable<TaskDataNode>.GetSingleton().DataNodeList.ContainsKey(fubenTaskData.taskId))
                    {
                        // 是副本任务 将副本数据存入副本任务数据列表
                        if (FSDataNodeTable<TaskDataNode>.GetSingleton().DataNodeList[fubenTaskData.taskId].Requiretype == 2)
                        {
                            if (TaskManager.Single().TaskToFubenDic.ContainsKey(fubenTaskData.taskId))
                            {
                                TaskManager.Single().TaskToFubenDic[fubenTaskData.taskId] = fubenTaskData;
                            }
                            else
                            {
                                TaskManager.Single().TaskToFubenDic.Add(fubenTaskData.taskId, fubenTaskData);
                            }
                        }// 是采集任务 将采集数据存入采集任务数据列表
                        else if (FSDataNodeTable<TaskDataNode>.GetSingleton().DataNodeList[fubenTaskData.taskId].Requiretype == 3)
                        {
                            if (TaskManager.Single().TaskToCaijiDic.ContainsKey(fubenTaskData.taskId))
                            {
                                TaskManager.Single().TaskToCaijiDic[fubenTaskData.taskId] = fubenTaskData;
                            }
                            else
                            {
                                TaskManager.Single().TaskToCaijiDic.Add(fubenTaskData.taskId, fubenTaskData);
                            }
                            //存一下数量
                            if (fubenTaskData.opt4 != 0)
                            {

                                if (TaskManager.Single().TaskItemCountsDic.ContainsKey(fubenTaskData.opt4))
                                {
                                    if (TaskManager.Single().TaskItemCountsDic[fubenTaskData.opt4]< (int)TaskManager.MainTask.parm1)
                                    {
                                        TaskManager.Single().TaskItemCountsDic[fubenTaskData.opt4] = (int)TaskManager.MainTask.parm1;
                                    }
                                    
                                }
                                else
                                {
                                    TaskManager.Single().TaskItemCountsDic.Add(fubenTaskData.opt4, (int)TaskManager.MainTask.parm1);
                                }
                            }
                            if (fubenTaskData.opt6 != 0)
                            {
                                if (TaskManager.Single().TaskItemCountsDic.ContainsKey(fubenTaskData.opt6))
                                {
                                    if (TaskManager.Single().TaskItemCountsDic[fubenTaskData.opt6]< (int)TaskManager.MainTask.parm2)
                                    {
                                        TaskManager.Single().TaskItemCountsDic[fubenTaskData.opt6] = (int)TaskManager.MainTask.parm2;
                                    }
                                    
                                }
                                else
                                {
                                    TaskManager.Single().TaskItemCountsDic.Add(fubenTaskData.opt6, (int)TaskManager.MainTask.parm2);
                                }
                            }



                        }// 是杀怪任务 将杀怪数据存入杀怪任务数据列表
                        else if (FSDataNodeTable<TaskDataNode>.GetSingleton().DataNodeList[fubenTaskData.taskId].Requiretype == 6)
                        {
                            if (TaskManager.Single().TaskToSkillMonsterDic.ContainsKey(fubenTaskData.taskId))
                            {
                                TaskManager.Single().TaskToSkillMonsterDic[fubenTaskData.taskId] = fubenTaskData;
                            }
                            else
                            {
                                TaskManager.Single().TaskToSkillMonsterDic.Add(fubenTaskData.taskId, fubenTaskData);
                            }

                            if (fubenTaskData.opt4 != 0)
                            {

                                if (TaskManager.Single().TaskSkillMonsterCountsDic.ContainsKey(fubenTaskData.opt4))
                                {
                                    if (TaskManager.Single().TaskSkillMonsterCountsDic[fubenTaskData.opt4]< (int)TaskManager.MainTask.parm1)
                                    {
                                        TaskManager.Single().TaskSkillMonsterCountsDic[fubenTaskData.opt4] = (int)TaskManager.MainTask.parm1;
                                    }
                                   
                                }
                                else
                                {
                                    TaskManager.Single().TaskSkillMonsterCountsDic.Add(fubenTaskData.opt4, (int)TaskManager.MainTask.parm1);
                                }
                            }
                            if (fubenTaskData.opt6 != 0)
                            {
                                if (TaskManager.Single().TaskSkillMonsterCountsDic.ContainsKey(fubenTaskData.opt6))
                                {
                                    if (TaskManager.Single().TaskSkillMonsterCountsDic[fubenTaskData.opt6] < (int) TaskManager.MainTask.parm2)
                                    {
                                        TaskManager.Single().TaskSkillMonsterCountsDic[fubenTaskData.opt6] = (int)TaskManager.MainTask.parm2;
                                    }
                                    
                                }
                                else
                                {
                                    TaskManager.Single().TaskSkillMonsterCountsDic.Add(fubenTaskData.opt6, (int)TaskManager.MainTask.parm2);
                                }
                            }



                        }//是使用道具任务 将到指定地点使用道具数据存入列表
                        else if (FSDataNodeTable<TaskDataNode>.GetSingleton().DataNodeList[fubenTaskData.taskId].Requiretype == 9)
                        {
                            if (TaskManager.Single().TaskToTargetUseItemDic.ContainsKey(fubenTaskData.taskId))
                            {
                                TaskManager.Single().TaskToTargetUseItemDic[fubenTaskData.taskId] = fubenTaskData;
                            }
                            else
                            {
                                TaskManager.Single().TaskToTargetUseItemDic.Add(fubenTaskData.taskId, fubenTaskData);
                            }
                        }//是杀怪掉落物任务 将杀怪掉落物的物品id和数量存入列表
                        else if (FSDataNodeTable<TaskDataNode>.GetSingleton().DataNodeList[fubenTaskData.taskId].Requiretype == 7)
                        {
                            if (TaskManager.Single().TaskToSMGoodsDic.ContainsKey(fubenTaskData.taskId))
                            {
                                TaskManager.Single().TaskToSMGoodsDic[fubenTaskData.taskId] = fubenTaskData;
                            }
                            else
                            {
                                TaskManager.Single().TaskToSMGoodsDic.Add(fubenTaskData.taskId, fubenTaskData);
                            }
                            if (fubenTaskData.opt4 != 0)
                            {

                                if (TaskManager.Single().TaskSMGoodsCountDic.ContainsKey(fubenTaskData.opt4))
                                {
                                    if (TaskManager.Single().TaskSMGoodsCountDic[fubenTaskData.opt4]< (int)TaskManager.MainTask.parm1)
                                    {
                                        TaskManager.Single().TaskSMGoodsCountDic[fubenTaskData.opt4] = (int)TaskManager.MainTask.parm1;
                                    }
                                }
                                else
                                {
                                    TaskManager.Single().TaskSMGoodsCountDic.Add(fubenTaskData.opt4, (int)TaskManager.MainTask.parm1);
                                }
                            }
                            if (fubenTaskData.opt6 != 0)
                            {
                                if (TaskManager.Single().TaskSMGoodsCountDic.ContainsKey(fubenTaskData.opt6))
                                {
                                    if (TaskManager.Single().TaskSMGoodsCountDic[fubenTaskData.opt6]< (int)TaskManager.MainTask.parm2)
                                    {
                                        TaskManager.Single().TaskSMGoodsCountDic[fubenTaskData.opt6] = (int)TaskManager.MainTask.parm2;
                                    }
                                }
                                else
                                {
                                    TaskManager.Single().TaskSMGoodsCountDic.Add(fubenTaskData.opt6, (int)TaskManager.MainTask.parm2);
                                }
                            }
                        }

                    }
                    if (Singleton<SceneManage>.Instance.Current == EnumSceneID.UI_MajorCity01 || Singleton<SceneManage>.Instance.Current == EnumSceneID.LGhuangyuan)
                    {
                        //Control.ShowGUI(GameLibrary.UITaskTracker);
                    }
                    break;
                case 5:

                    if (FunctionOpenMng.GetInstance().GetIndexbypos(int.Parse(data["opt3"].ToString()), int.Parse(data["opt4"].ToString())))
                    {
                        //Debug.Log("<color=#10DF11>GetInstance uId</color>" + playerData.GetInstance().guideData.uId);
                        if (playerData.GetInstance().guideData.uId != 0 && playerData.GetInstance().guideData.uId != int.Parse(data["opt5"].ToString()))
                        {
                            //Debug.Log("<color=#10DF11>GetInstance uId</color>" + playerData.GetInstance().guideData.uId);
                            //Debug.Log("<color=#10DF11>TaskHandle uId</color>" + int.Parse(data["opt5"].ToString()));
                            ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidStep(99);

                            playerData.GetInstance().guideData.uId = int.Parse(data["opt5"].ToString());
                            //Debug.Log("<color=#10DF11>GetInstance uId=TaskHandle uId</color>" + playerData.GetInstance().guideData.uId);
                            GuidAskEvent();
                        }
                        else if(playerData.GetInstance().guideData.uId==0)
                        {
                            //Debug.Log("<color=#10DF11>GetInstance uId</color>" + playerData.GetInstance().guideData.uId);
                            playerData.GetInstance().guideData.uId = int.Parse(data["opt5"].ToString());
                            GuidAskEvent();
                        }
                       

                    }

                    break;
                default:
                    break;
            }



        }

        return true;
    }

    public void GuidAskEvent()
    {
        switch (playerData.GetInstance().guideData.uId)
        {
            case 919:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 1, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    //Debug.Log("<color=#10DF11>GuidAskEvent</color>" + GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[830], 0, 16));
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 1219:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 2, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 2125:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 3, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 2319:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 4, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 2719:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 5, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 2919:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 6, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 3019:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 7, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 3219:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 8, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;

            case 4419:
                if (GuideManager.Single().GetBitValue(playerData.GetInstance().selfData.infodata[833], 9, 1) != 1)
                {
                    //Debug.Log("<color=#10DF11>GuidAskEvent uId</color>" + playerData.GetInstance().guideData.uId);
                    ClientSendDataMgr.GetSingle().GetGuideSend().SendGuidAskEvent();
                }
                break;
            default:
                break;
        }
    }
}