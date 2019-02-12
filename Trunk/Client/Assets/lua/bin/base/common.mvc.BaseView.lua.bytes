BaseView = {}
local this = BaseView
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.viewName = ""
--界面类型（层级）
this.viewType = UIViewType.None
--界面枚举
this.viewEnum = 0
--是否是栈界面
this.isStackView = false
--是否是白色状态栏
this.isStateBarWhiteColor = false
--加载列表
this.loadOrders = nil
--容器（层级）GameObject
this.container = nil
--是否正在加载，加载完成后设置为false
this.isLoading = false
--是否已经加载，加载完成后设置为true
this.isLoaded = false
--是否正在打开过程中（加载并显示）
this.isOpening = false
--是否已经打开（显示）
this.isOpen = false

this.main_mid = nil
this.loadedNum = 0
this.loadedNames = {}


--获取界面类型（层级）
function this:getViewType()
    if self.viewType == UIViewType.None then
        Loger.PrintError(self.viewName, " 没有设置 viewType")
    end
    return self.viewType
end

--获取界面枚举
function this:getViewEnum()
    if self.viewEnum == 0 then
        Loger.PrintError(self.viewName, " 没有设置 viewEnum")
    end
    return self.viewEnum
end

--获取是否是栈界面
function this:getIsStackView()
    return self.isStackView
end

--获取是否是白色状态栏
function this:getStateBarWhiteColor()
	return self.isStateBarWhiteColor
end

function this:setViewAttribute(_viewType, _viewEnum, _isStackView,_isStateBarWhiteColor)
    self.viewType = _viewType
    self.viewEnum = _viewEnum
    if _isStackView == nil and _viewEnum > 1000 and _viewEnum < 5000 then
        printError("界面(" .. self.viewName .. ")没有设置是否参与界面堆栈")
    end
    if _isStackView == true then
        self.isStackView = true
    end
	
	self.isStateBarWhiteColor = _isStateBarWhiteColor and true or false
end

--获取加载列表
function this:getLoadOrders()
    return self.loadOrders
end

--获取界面GameObject
function this:getViewGO()
    if self.main_mid ~= nil then
        return self.main_mid.go
    else
        Loger.PrintError(self.viewName, " main_mid 不能为空")
        return nil
    end
end

--设置容器（层级）GameObject
function this:setContainerGO(ContainerGO)
    self.container = ContainerGO
end

--获取是否正在加载
function this:getIsLoading()
    return self.isLoading
end

--获取是否已加载
function this:getIsLoaded()
    return self.isLoaded
end

--设置是否正在打开过程中（加载并显示）
function this:setOpening(value)
    self.isOpening = value
end

--获取是否正在打开过程中（加载并显示）
function this:getOpening()
    return self.isOpening
end

--获取是否已经打开（显示）
function this:getIsOpen()
    return self.isOpen
end

--开始加载
function this:startLoad()
    self.isLoading = true
    self.isLoaded = false
    self.loadedNames = {}
    self:load()
end

--加载
function this:load()
    if self.loadOrders == nil then
        Loger.PrintError(self.viewName, " loadOrders 加载列表不能为空")
    end
    for i = 1, #self.loadOrders do
        local order = self.loadOrders[i]
        local orderArr = string.split(order, ":")
        if #orderArr ~= 2 then
            Loger.PrintError(self.viewName, " loadOrders 加载列表格式错误")
        end

        UILoadControl.Instance:CreateUI(orderArr[1], orderArr[2], self)
    end
end

--一个UI资源加载结束
function this:executeLoadUIEnd(uiName, gameObject)
    self.loadedNum = self.loadedNum + 1
    self.loadedNames[self.loadedNum] = uiName
    self:onLoadUIEnd(uiName, gameObject)
    if self.loadedNum >= #self.loadOrders then
        self:endLoad()
    end
end

--override
function this:onLoadUIEnd(uiName, gameObject)
    Loger.PrintWarning(self.viewName, " -- ", uiName, " 需重定义onLoadUIEnd")
end

--加载结束
function this:endLoad()
    self.isLoading = false
    self.isLoaded = true
end

--已废弃，保留在这用于兼容旧代码
function this:endInit()
	--什么都不做
end


local ease_container = nil
function this:setEaseContainer(container)
    self.ease_container = container
end

--显示界面
function this:show(msg)
    printDebug("显示界面：" .. self.viewName)

    self.isOpen = true
    -- self:onAwake()
    self:getViewGO():SetActive(true)

    if self.ease_container ~= nil then
        UITools.SetUIScale(self.ease_container, Vector2(0, 0))
        self.ease_container.transform:DOScale(Vector3(1, 1, 1), 0.3):SetEase(Ease.OutBack)
    end
    if self.main_mid ~= nil then
        local img = self.main_mid.go:GetComponent(typeof(Image))
        if img ~= nil then
            local mat = img.material
            if mat ~= nil then
            --mat:SetFloat("_Size",0)
            --UITools.DOTweenMatAttribute(mat,"_Size",2,0.3)
            end
        end
    end
    self:onShowHandler(msg)
end

--override
function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName, " -- ", uiName, " 需重定义onShowHandler")
end

--隐藏界面
function this:hide()
    printDebug("隐藏界面：" .. self.viewName)

    self.isOpen = false
    if self:getViewGO() ~= nil then
        self:getViewGO():SetActive(false)
    end
    self:onClose()
end

--override
function this:onClose()
    --子类可重定义
end

--override
--返回键关闭界面
function this:closeByEsc()
	--子类可重定义
	ViewManager.close(self.viewEnum)
end

function this:BindMonoTable(gameObject, table)
    table = table or {}
    if not gameObject then
        return printError(tostring(self.viewName) .. "绑定Mono界面为空")
    end
    gameObject:GetComponent(typeof(CS.UIBaseMono)):BindMonoTable(table)
	if gameObject then
		local panel= gameObject:GetComponent(typeof(CS.PanelWidget))
		panel.UIViewEnum = self.viewEnum
		printDebug("=======================this:onLoadUIEnd================panel.UIViewEnum ="..tostring(panel.UIViewEnum ))
	end
end

--销毁界面
function this:onDestroy()
    --重置状态
    self.isLoading = false
    self.isLoaded = false
    self.isOpen = false
    self.main_mid = nil
    self.loadedNum = 0

    for k, v in ipairs(self.loadedNames) do
        local strs = string.split(v, ":")
        printDebug("ViewManager.destroyUIRes：" .. strs[1] .. " " .. strs[2])
        ViewManager.destroyUIRes(strs[1], strs[2])
    end

    self.loadedNames = {}
end
