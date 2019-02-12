ModelManager = {}

--创建模型
function ModelManager.createModel(packName, modelName, callback)
	CS.ModelManager.Instance:CreateModel(packName, modelName, function (gameObject)
		if callback ~= nil then
			callback(gameObject)
		end
	end)
end

--销毁模型
function ModelManager.destroyModel(gameObject)
	CS.ModelManager.Instance:DestroyModel(gameObject)
end