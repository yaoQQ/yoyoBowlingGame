ActionManager={}
local this=ActionManager

this.groupList={}


function this.init()
	LuaActionController.Instance:RegistManager(this)
end

function this.start(group)
	local  exist=false
	if #this.groupList>0 then
		for i=1,#this.groupList do
			local tempG =this.groupList[i]
			if tempG==group then
				exist=true
			end
		end
	end
	if not exist then
		group:init()
		table.insert(this.groupList,group)
	end
end

-- local num = 0
function this:execute(deltaTime_ms)
	
	-- num=num+1
	if  #this.groupList==0 then
		return
	end
	local len = #this.groupList
	local  group
	for i=len,1,-1 do
		group=this.groupList[i]
		if group:getRunningSign() then
			group:execute(deltaTime_ms)
		else
			NoticeManager.Instance:Dispatch(CommonNoticeType.Action_Group_Over,group.groupName)
			table.remove(this.groupList,i)
		end
		
	end
end
