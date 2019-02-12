AbstractPackage={}
local this=AbstractPackage

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.initSign=false
this.packName=""

this.moduleList=nil
this.protoList=nil
this.viewList=nil
this.tableList = nil


function this:init(finishCallback)
	if self.packName=="" then
		CSLoger.printError(self," 模块名没有初始化")
	end
	if self.initSign==false then
		self.initSign=true
		self:initModule()
		self:initView()
        self:initTable()
		if #self.protoList > 0 then
			self:initProto(finishCallback)
		else
			if finishCallback ~= nil then
				finishCallback()
			end
		end
	else
		if finishCallback ~= nil then
			finishCallback()
		end
	end
end


function this:initModule()
	if self.moduleList==nil then
		CSLoger.printError(self.packName," 模块列表没有初始化")
	else
		for i=1,#self.moduleList do
			CSModueManager.Instance:RegisterLuaModule(self.moduleList[i])
		end
	end
end

 
function this:initProto(finishCallback)
	if self.protoList==nil then
		CSLoger.printError(self.packName," 协议列表没有初始化")
	else
		ProtobufManager:initPackPb(self.packName, self.protoList, finishCallback)
		
	end
end

function this:initView()
	if self.viewList==nil then
		CSLoger.printError(self.packName," 视图列表没有初始化")
	else
		for i=1,#self.viewList do
			ViewManager.regist(self.packName, self.viewList[i])
		end
	end
end

function this:initTable()
	if self.tableList == nil then
		--CSLoger.printError(self.packName," 视图列表没有初始化")
        return
	else
        for _, v in pairs(self.tableList) do
            v.init()
        end
	end
end

function this:getPackAllUIMidList()
	if self.viewList==nil then
		return nil
	else
		local arr = {}
		for i=1,#self.viewList do
			local view = self.viewList[i]
			table.insert(arr, view)
		end
		return arr
	end
end

return AbstractPackage