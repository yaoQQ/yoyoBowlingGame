local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget

Mid_common_feedbacktips_panel={}
local this = Mid_common_feedbacktips_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
--FeedbackItem数组
this.feedbackItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.feedbackItemArr={}
	table.insert(self.feedbackItemArr,self.new_FeedbackItem(self.go.transform:Find("feedback_item_1").gameObject))
	table.insert(self.feedbackItemArr,self.new_FeedbackItem(self.go.transform:Find("feedback_item_2").gameObject))
end

--FeedbackItem复用单元
function this.new_FeedbackItem(itemGo)
	local item = { }
	item.go = itemGo
	item.feedback_image=itemGo.transform:Find("feedback_image"):GetComponent(typeof(ImageWidget))
	item.feedback_text=itemGo.transform:Find("feedback_text"):GetComponent(typeof(TextWidget))
	return item
end

