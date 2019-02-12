--飘字提示消息
require "base:module/main/view/FeedbackTipsView"
showFloatTips = function(msg)
    --    if msg then--备注修改提示方式 2018.3.30 wen
    --		FeedbackTipsView:showTip(msg)
    --    end
    showTopTips(msg)
end

--顶部提示
require "base:module/main/view/TopTipsView"
showTopTips = function(msg)
    if msg then
        TopTipsView:onShowTopTips(msg)
    end
end

--环形等待遮罩
require "base:module/main/view/WaittingView"
ShowWaiting = function(isShow, reason)
    if isShow then
        printDebug("ShowWaiting:true " .. reason)
    else
        printDebug("ShowWaiting:false " .. reason)
    end
    WaittingView.showOrHide(isShow, reason)
end
--搜索
require "base:module/main/view/SearchingView"
ShowSearching = function(isShow, reason)
    -- if isShow then
    -- 	printDebug("ShowSearching:true "..reason)
    -- else
    -- 	printDebug("ShowSearching:false "..reason)
    -- end
    SearchView.showOrHide(isShow, reason)
end
require "base:module/main/view/AlertWindowView"
require "base:module/main/view/TipsView"

Alert = {}

--显示警告窗口（只带第一个按钮样式）
Alert.showAlertMsg = function(title, msg, btnName, onBtnfunc)
    AlertWindowView:showAlertMsg(title, msg, btnName, onBtnfunc)
end

--显示警告窗口（只带第二个按钮样式）
Alert.showAlertVerify = function(title, info, btnName, onBtnfunc, isAllowClose)
    AlertWindowView:showAlertVerifyWindow(title, info, btnName, onBtnfunc, isAllowClose)
end

--显示确认窗口（带两个按钮）
Alert.showVerifyMsg = function(title, msg, btnName1, onBtnfunc1, btnName2, onBtnfunc2, isAllowClose)
    AlertWindowView:showVerifyMsg(title, msg, btnName1, onBtnfunc1, btnName2, onBtnfunc2, isAllowClose)
end

--[[--警告消息窗口（小）
Alert.showCheckMsg=function(msg,onConfirmFun,onCancelFun)
   AlertCheckView:showMsg(msg,onConfirmFun,onCancelFun)
end--]]
Alert.showTipsMsg = function(msg)
    TipsView:showTip(msg)
end

require "base:module/common/view/CommonBottomSelectView"
--底部选择框，只有一个按钮
ShowBottomSelectWind1 =
    function(btnName1, onBtnfunc1)
    local msg = {}
    msg.msgType = "One"
    msg.btnName1 = btnName1
    msg.btnFunc1 = onBtnfunc1

    ViewManager.open(
        UIViewEnum.Common_Bottom_Select_View,
        msg,
        function()
            CommonBottomSelectView:openMsgView(msg)
        end
    )
end

--底部选择框，有两个按钮
ShowBottomSelectWind2 =
    function(btnName1, onBtnfunc1, btnName2, onBtnfunc2)
    local msg = {}
    msg.msgType = "Two"
    msg.btnName1 = btnName1
    msg.btnFunc1 = onBtnfunc1

    msg.btnName2 = btnName2
    msg.btnFunc2 = onBtnfunc2

    ViewManager.open(
        UIViewEnum.Common_Bottom_Select_View,
        msg,
        function()
            CommonBottomSelectView:openMsgView(msg)
        end
    )
end

--底部选择框，有三个按钮
ShowBottomSelectWind3 =
    function(btnName1, onBtnfunc1, btnName2, onBtnfunc2, btnName3, onBtnfunc3)
    local msg = {}
    msg.msgType = "Three"
    msg.btnName1 = btnName1
    msg.btnFunc1 = onBtnfunc1

    msg.btnName2 = btnName2
    msg.btnFunc2 = onBtnfunc2

    msg.btnName3 = btnName3
    msg.btnFunc3 = onBtnfunc3

    ViewManager.open(
        UIViewEnum.Common_Bottom_Select_View,
        msg,
        function()
            CommonBottomSelectView:openMsgView(msg)
        end
    )
end

ShowBottomSelectWindPhoto =
    function(onBtnfunc1, onBtnfunc2)
    local msg = {}
    msg.msgType = "Photo"
    msg.btnFunc1 = onBtnfunc1
    msg.btnFunc2 = onBtnfunc2

    ViewManager.open(
        UIViewEnum.Common_Bottom_Select_View,
        msg,
        function()
            CommonBottomSelectView:updatePhotoWindow(msg)
        end
    )
end
