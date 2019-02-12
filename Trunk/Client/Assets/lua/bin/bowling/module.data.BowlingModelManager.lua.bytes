

BowlingModelManager = {}
local this = BowlingModelManager
BowlingModelManager.ModelDic = {}




--创建模型
function BowlingModelManager.createModel(packName, modelName, callback)
	printDebug("BowlingModelManager.createModel() 获取存储 ModelDic["..tostring(modelName).."]="..tostring(BowlingModelManager.ModelDic[modelName]))
	if BowlingModelManager.ModelDic[modelName]~=nil then
		if callback ~= nil then
			printDebug("BowlingModelManager.createModel() 获取存储 返回 obj="..tostring(BowlingModelManager.ModelDic[modelName]))
			callback(BowlingModelManager.ModelDic[modelName])
		end
		return
	end
	
	CS.ModelManager.Instance:CreateModel(packName, modelName, function (gameObject)
		if callback ~= nil then
			printDebug("ModelManager.Instance:CreateModel() 加载返回 obj="..tostring(gameObject))
			BowlingModelManager.ModelDic[modelName] = gameObject
			callback(gameObject)
		end
	end)
end
