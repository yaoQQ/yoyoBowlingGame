

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"

require "base:mid/common/Mid_common_feedbacktips_panel"

FeedbackTipsView = BaseView:new()
local this=FeedbackTipsView
this.viewName="FeedbackTipsView"


--设置面板特性
this:setViewAttribute(UIViewType.Feedback_Tip,UIViewEnum.FeedbackTips, false)


--设置加载列表
this.loadOrders=
{
	"base:common/common_feedbacktips_panel", 
}


this.tweenTime=5
this.moveMaxPos=3
this.maxTipsCount=2

--消息队列
this.tipContentQueue={}

--移动队列
this.tweenItemList = {}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self.main_mid.go:SetActive(true)		
	self:resetView()
end


function this:onShowHandler(msg)
	printDebug("FeedbackTipsView ============> onShowHandler() ")
	self:updateTipsView(msg)
end


function this:resetView()
	self.tipContentQueue={}
	self.tweenItemList={}

	for i=1,#self.main_mid.feedbackItemArr do
		self.main_mid.feedbackItemArr[i].go:SetActive(false)
	end
	--self.main_mid.go:SetActive(false)	
end


function this:showTip(msg)
	if FeedbackTipsView.isOpen then
		self:updateTipsView(msg)
	else
    	ViewManager.open(UIViewEnum.FeedbackTips,msg)
	end 
end	


function this:updateTipsView(msg)
	--self.main_mid.go:SetActive(true)
	self:onTipsEnqueue(msg)
	self:openTipsView()
end


function this:openTipsView()
    local feedbackItem = self:GetFreeItem()	
	if feedbackItem == nil or #self.tipContentQueue == 0 then
		return
	end
     
    -- if self.getTweenItemIndex(feedbackItem) ~= -1 then
 	  --  return
    -- end

	feedbackItem.go:SetActive(true)
	feedbackItem.feedback_text.text = self:onGetFeedbackTip()
      
	local tweenItem = {}
	tweenItem.originPos = feedbackItem.go.transform.position
	tweenItem.originTextAplha = feedbackItem.feedback_text.Txt.color
	tweenItem.originImgAplha = feedbackItem.feedback_image.Img.color

	local tweenMove1 =feedbackItem.go.transform:DOMove(Vector3(feedbackItem.go.transform.position.x, 
		feedbackItem.go.transform.position.y+self.moveMaxPos,feedbackItem.go.transform.position.z),
	 	self.tweenTime/2):OnComplete(function()


		local tweenMove2 =feedbackItem.go.transform:DOMove(Vector3(feedbackItem.go.transform.position.x, 
			feedbackItem.go.transform.position.y,feedbackItem.go.transform.position.z),
		 	self.tweenTime/3):OnComplete(function()
	     		feedbackItem.go:SetActive(false)
	     		feedbackItem.go.transform.position = tweenItem.originPos
	     		feedbackItem.feedback_text.Txt.color = tweenItem.originTextAplha
	     		feedbackItem.feedback_image.Img.color = tweenItem.originImgAplha
	     		self.OnTweenItemAction()
	   	end)
   	end)

	-- feedbackItem.feedback_text.Txt:CrossFadeAlpha(0, self.tweenTime/2)
	-- feedbackItem.feedback_image.Img:CrossFadeAlpha(0, self.tweenTime/2)

end


function this.OnTweenItemAction()
	if #this.tweenItemList==0 and #this.tipContentQueue==0 then
        return
    --elseif #this.tweenItemStructList > 0 then    
	end
    this:openTipsView()
end


function this:onTipsEnqueue(msg)
	if self.tipContentQueue == nil then
		return
	elseif #self.tipContentQueue > self.maxTipsCount then	
		return
	end
   table.insert(self.tipContentQueue,msg)
end


function this:onGetFeedbackTip()
	local feedbackMsg = nil
   	if self.tipContentQueue ~= nil and #self.tipContentQueue ~= 0 then
   	   feedbackMsg = self.tipContentQueue[1]
       table.remove(self.tipContentQueue,1)
	   --CSLoger.debug(Color.Yellow,"剩余消息："..#self.tipContentQueue)
	end
	return feedbackMsg
end


function this.getTweenItemIndex(item)
	for i=1,#this.tweenItemList do
		if this.tweenItemList[i].item == item then
		    return i
		end	
	end
	return -1
end



function this:GetFreeItem()
	for i=1,#self.main_mid.feedbackItemArr do
		local feedbackItem = self.main_mid.feedbackItemArr[i]
		if feedbackItem.go.activeSelf == false then
			return feedbackItem
		end
	end
	return nil
end