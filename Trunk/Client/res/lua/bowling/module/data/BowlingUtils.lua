

BowlingUtils = {}
local this = BowlingUtils


local RectTransformUtility = CS.UnityEngine.RectTransformUtility
this.everyAngel=1
this.UICamera  = CS.UIManager.Instance.UICamera

--[[public Camera mycamera;//要转化到的目的摄像机，通常canvas在这个摄像机下（即canvas的render mode设置为这个摄像机）
Image kongjian;//自己要获取屏幕坐标的控件，可以是image，也可以是button等等
 
 
float x=mycamera.WorldToScreenPoint(kongjian.transform.position).x;
float y=mycamera.WorldToScreenPoint(kongjian.transform.position).y;
--]]


--屏幕位置点转为保龄球朝向度数
function this.countBallfowardAngel(pos1,pos2)

	local ballToCurrPosDis = Vector3.Distance(pos1,pos2) --球与目标点距离
	if  ballToCurrPosDis<=1 then
		printDebug("pos1="..tostring(pos1).."  pos2="..tostring(pos2).." angel="..tostring(angel))
		return this.everyAngel
	end
	local currPosToBallXDis = pos2.x -pos1.x
	local angel = Mathf.Asin(currPosToBallXDis/ballToCurrPosDis)*Mathf.Rad2Deg  -- 轨迹圆弧角度
--	angel=angel/10
	printDebug("pos1="..tostring(pos1).."  pos2="..tostring(pos2).." angel="..tostring(angel))
	return angel
end



function BowlingUtils.WorldPosToScreen(worldPos)

	local screenPos=nil
	if screenPos then
		screenPos=this.UICamera:WorldToScreenPoint(worldPos)
	end
	return screenPos
end


function BowlingUtils.UguiToScreen(ui,screenPos)
	
	if screenPos then
		local value=nil
		local worldPos=Vector3.zero
		value, worldPos = RectTransformUtility.ScreenPointToWorldPointInRectangle(ui.rectTransform, Vector2(screenPos.x,screenPos.y), this.UICamera, worldPos)
		return worldPos
	end
	return nil
end

function BowlingUtils.isScreenPosInUI(ui,screenPos)
	if ui then
		--local isCenterIn = RectTransformUtility.RectangleContainsScreenPoint(self.main_mid.cal_panel.rectTransform, Vector2(centerPoint.x, centerPoint.y), UICamera)
		local isCenterIn = RectTransformUtility.RectangleContainsScreenPoint(ui.rectTransform, Vector2(screenPos.x, screenPos.y), this.UICamera)
		return isCenterIn
	end
	return false
end
