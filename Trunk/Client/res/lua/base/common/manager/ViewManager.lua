ViewManager = {}

--创建UI层级面板
function ViewManager.createUILayerPanel(layerIndex, name)
	CSUIViewManager.Instance:CreateUILayerPanel(layerIndex, name)
end

--注册栈底界面（主界面）
function ViewManager.registerStackButtomView(viewEnum)
	CSUIViewManager.Instance:RegisterStackButtomView(viewEnum)
end

--注册界面
function ViewManager.regist(packageName, view)
	CSUIViewManager.Instance:RegisterView(packageName, view)
end

--预加载界面
function ViewManager.preload(viewEnumValue, preloadCallback)
	CSUIViewManager.Instance:Preload(viewEnumValue, preloadCallback)
end

function ViewManager.open(viewEnumValue, msg, openCallback)
	CSUIViewManager.Instance:Open(viewEnumValue, msg, openCallback)
end

function ViewManager.close(viewEnumValue)
	CSUIViewManager.Instance:Close(viewEnumValue)
end

function ViewManager.closeAll()
	CSUIViewManager.Instance:CloseAllView()
end

function ViewManager.saveStackAndCloseAllView()
	CSUIViewManager.Instance:SaveStackAndCloseAllView()
end

function ViewManager.closeAllViewAndRevertStack()
	CSUIViewManager.Instance:CloseAllViewAndRevertStack()
end

function ViewManager.destroyView(viewEnumValue)
	CSUIViewManager.Instance:DestroyView(viewEnumValue)
end

function ViewManager.destroyUIRes(packageName, relativePath)
	CSUIViewManager.Instance:DestroyUIRes(packageName, relativePath)
end

function ViewManager.setCanvasMatch(uiViewType, value)
	CSUIViewManager.Instance:SetCanvasMatch(uiViewType, value)
end