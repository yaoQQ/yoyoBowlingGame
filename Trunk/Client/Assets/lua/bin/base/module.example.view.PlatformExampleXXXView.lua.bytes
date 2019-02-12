require "base:mid/example/Mid_platform_example_xxx_panel"

--平台示例界面
PlatformExampleXXXView = BaseView:new()
local this = PlatformExampleXXXView
this.viewName = "PlatformExampleXXXView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Example_XXX_View, true)

--设置加载列表
this.loadOrders = {
    "base:example/platform_example_xxx_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    --下面两行默认需要调用
    
    UITools.SetParentAndAlign(gameObject, self.container)

    --设置UI中间代码
    self.main_mid = {} --Mid_platform_example_xxx_panel:new(gameObject)
    self:BindMonoTable(gameObject, self.main_mid)
    --添加UI事件监听
    self:addEvent()
end

function this:addEvent()
    self.main_mid.Button:AddEventListener(UIEvent.PointerClick, this.onClickButton)
end

function this.onClickButton()
    --按钮点击响应
    --按钮点击响应可能是发一条协议
    --PlatformExampleModule.sendReqExample(param1, param2)
end

--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()

    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
    self:initView()
end

--override 关闭UI回调
function this:onClose()
    --关闭界面时移除UI通知监听
    self:removeNotice()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(NoticeType.Example, this.onExample)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Example, this.onExample)
end

--打开界面时初始化
function this:initView()
    this.main_mid.Text.text = ""
end

function this.onExample()
    --通知响应处理
end
