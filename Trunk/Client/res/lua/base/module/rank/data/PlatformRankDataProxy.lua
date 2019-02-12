--平台数据层示例
PlatformSignDataProxy = {}
local this = PlatformSignDataProxy


local m_exampleData = nil

--设置数据示例
function this.setRspExampleData(msg)
	--msg的数据类型为RspExample
	
	--先进行相关数据处理和保存
	--m_exampleData = msg
	
	--然后根据需求打开相关界面
	--ViewManager.open(UIViewEnum.Platform_Example_XXX_View)
	
	--或派发数据更新的通知，用于刷新相关界面
	--NoticeManager.Instance:Dispatch(NoticeType.Example)
end

--获取数据示例
function this.getExampleData()
	return m_exampleData
end