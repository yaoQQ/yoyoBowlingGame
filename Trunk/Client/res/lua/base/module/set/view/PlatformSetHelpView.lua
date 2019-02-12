
require "base:enum/UIViewEnum"
require "base:mid/set/Mid_platform_set_help_panel"

PlatformSetHelpView=BaseView:new()
local this=PlatformSetHelpView
this.viewName="PlatformSetHelpView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Set_Help_View, true)

--设置加载列表
this.loadOrders=
{
	"base:set/platform_set_help_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	this:updateQueList()
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

this.Image=nil
this.Text=nil

function this:addNotice()
end

function this:removeNotice()
end
local isDownLoad=false
local answerHeight=0
local childNum=0
this.preInputText = nil
this.referText=nil
function this:addEvent()
	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_Set_Help_View)
	end )
	

	self.main_mid.set_Button:AddEventListener(UIEvent.PointerClick,self.onBtnSetEvent)
	self.main_mid.redbag_Button:AddEventListener(UIEvent.PointerClick,self.onBtnRedBagEvent)
	self.main_mid.guess_Button:AddEventListener(UIEvent.PointerClick,self.onBtnGuessEvent)
	self.main_mid.other_Button:AddEventListener(UIEvent.PointerClick,self.onBtnOtherEvent)

	self.main_mid.answer_back_Image:AddEventListener(UIEvent.PointerClick,function ( )
		isDownLoad=false
		answerHeight=0
		this:onBtnDestroy(childNum)
		self.main_mid.answer_Panel.gameObject:SetActive(false)
		self.main_mid.answer_ScrollPanel.transform:GetChild(0).transform:
		GetComponent(typeof(RectTransform)).anchoredPosition = Vector2(0, 0) 
	end)
	self.main_mid.right_Button:AddEventListener(UIEvent.PointerClick,self.onBtnSendEvent)
	self.main_mid.left_Button:AddEventListener(UIEvent.PointerClick,self.onBtnServiceEvent)
	self.main_mid.send_back_Image:AddEventListener(UIEvent.PointerClick,function ( )
		this.main_mid.send_Panel.gameObject:SetActive(false)
	end)
	self.main_mid.feedback_InputField:OnValueChanged(function ()
			this:GetTextCount()
		end)
	self.main_mid.refer_Button:AddEventListener(UIEvent.PointerClick,self.onBtnReferEvent)
	

	--[[local btnFunction ={}

	btnFunction["Onexxxx"]=function ( )
		this.goQueList(1)
	end)
....


	btnFunction


	
	--]]

end
----------------------------------------------------------问题帮助------------------------------------------------------------------

function this:updateQueList()
	local selfData=PlatformUserProxy:GetInstance():getUserInfo()
	printDebug("++++++++++++++++++++++我是自己的data"..table.tostring(selfData))
	this.main_mid.userId_Text.text="用戶ID:"..selfData.player_id
	local textName = {"one","two","three","four","five","six","seven"}
	local hotList={}
	local setList={}
	local redbagList={}
	local guessList={}
	local otherList={}
	for i=1,#TableBaseSetHelpList.data do
		if TableBaseSetHelpList.data[i].ishot==0 then 
			table.insert(hotList,TableBaseSetHelpList.data[i].id)
			printDebug("++++++++++++++++hotList"..table.tostring(hotList))
		end
		if TableBaseSetHelpList.data[i].type==1 then
			table.insert(setList,TableBaseSetHelpList.data[i].id)
			printDebug("++++++++++++++++setList"..table.tostring(setList))
			--this.main_mid[string.concat("set_Text_",textName[i])].text=TableBaseSetHelpList.data[i].title
		elseif TableBaseSetHelpList.data[i].type==2 then
			table.insert(redbagList,TableBaseSetHelpList.data[i].id)
			printDebug("++++++++++++++++redbagList"..table.tostring(redbagList))
			--this.main_mid[string.concat("redbag_Text_",textName[i])].text=TableBaseSetHelpList.data[i].title
		elseif TableBaseSetHelpList.data[i].type==3 then
			table.insert(guessList,TableBaseSetHelpList.data[i].id)
			printDebug("++++++++++++++++guessList"..table.tostring(guessList))
			--this.main_mid[string.concat("guess_Text_",textName[i])].text=TableBaseSetHelpList.data[i].title
		elseif TableBaseSetHelpList.data[i].type==4 then
			table.insert(otherList,TableBaseSetHelpList.data[i].id)
			printDebug("++++++++++++++++otherList"..table.tostring(otherList))
			--this.main_mid[string.concat("other_Text_",textName[i])].text=TableBaseSetHelpList.data[i].title
		else
			printDebug("未知的问题类型")
		end

	end
		this.doIt(0,hotList)
	    this.doIt("set_",setList)
	    this.doIt("redbag_",redbagList)
	    this.doIt("guess_",guessList)
	    this.doIt("other_",otherList)

end



function this.doIt(str,dataID)
	local thisName={"one","two","three","four","five","six","seven"}
	printDebug("我要开始了哦"..table.tostring(dataID))
	for i=1,#dataID do
		if str==0 then
			this.main_mid[string.concat(thisName[i],"_name_Text")].text=TableBaseSetHelpList.data[dataID[i]].title
			this.main_mid[string.concat(thisName[i],"_press_Image")]:GetComponent(typeof(ImageWidget)):AddEventListener(UIEvent.PointerClick,function ( )
				printDebug("我要开始发送了哦"..table.tostring(dataID))
				this.goQueList(i,dataID)
			end)
		else
			this.main_mid[string.concat(str,"Text_",thisName[i])].text=TableBaseSetHelpList.data[dataID[i]].title
			this.main_mid[string.concat(str,"Image_",thisName[i])]:GetComponent(typeof(ImageWidget)):AddEventListener(UIEvent.PointerClick,function ( )
				printDebug("我要开始发送了哦"..table.tostring(dataID))
				this.goQueList(i,dataID)
			end)	
		end
	end
end





function this.goQueList(index,dataID)
	printDebug("我要开始接收了哦"..table.tostring(dataID))
    
    if dataID[index]==nil then return end
    local QuestionData=TableBaseSetHelpList.data[dataID[index]]

   -- if QuestionData.ishot==0 then
    	this.main_mid.answer_title_Text.text=QuestionData.title
		this:answerSplitNew(QuestionData)
	--else
--[[
		if QuestionData.type==1 then
			this.main_mid.answer_title_Text.text=QuestionData.title
			this:answerSplitNew(str,QuestionData)
		
		elseif QuestionData.type==2 then
			this.main_mid.answer_title_Text.text=QuestionData.title
			this.main_mid.answer_text.text=QuestionData.answer
		elseif QuestionData.type==3 then
			this.main_mid.answer_title_Text.text=QuestionData.title
			this.main_mid.answer_text.text=QuestionData.answer
		elseif QuestionData.type==4 then
			this.main_mid.answer_title_Text.text=QuestionData.title
			this.main_mid.answer_text.text=QuestionData.answer
		else
			printDebug("未知参数")
		end
	end
--]]
end

function  this:answerSplitNew(data)
	local queStr=string.gsub(data.answer, " ", "" )
    local queString=string.gsub(queStr, "%b<>", " " )
	local queText=string.split(queString," ")

	--local queText=nil
   --queText=string.gsub(data.answer, "url", "5" 
	printDebug("+++++++++++++++++++++++++++++我是裁剪的回答啊"..table.tostring(queText))
	for i=1, #queText do
		printDebug("+++++++++++++++++我是列表数目啊"..#queText)
		if queText[i]=="/url" then 
			table.remove(queText,i)
		elseif queText[i]=="url" then 
			table.remove(queText,i)
		end
		printDebug("+++++++++++++++++++++++++++++我是裁剪的回答啊"..table.tostring(queText))
	end
		for i=1,#queText do
			local answertype=string.match(queText[i],"http")
			-- printDebug("++++++++++answertype"..answertype)
			if answertype==nil or answertype=="" then
					this.Text=GameObject.Instantiate(this.main_mid.answer_text.gameObject)
					this.Text.transform:SetParent(this.main_mid.answer_ScrollPanel.transform:GetChild(0).transform)
					this.Text.transform.localScale = Vector3(1, 1, 1)
					this.Text:GetComponent(typeof(TextWidget)).text=queText[i]
					answerHeight= answerHeight+this.Text:GetComponent(typeof(TextWidget)).Txt.preferredHeight
					printDebug("++++++++++++++++++++++++我是文本的高高哦："..this.Text:GetComponent(typeof(TextWidget)).Txt.preferredHeight)
					printDebug("+++++++++++++++++++++++++++++++文本i"..i)
	
			else
				this.Image=GameObject.Instantiate(this.main_mid.answer_image.gameObject)
			    this.Image.transform:SetParent(this.main_mid.answer_ScrollPanel.transform:GetChild(0).transform)
				this.Image.transform.localScale = Vector3(1, 1, 1)
				answerHeight= answerHeight+860
				local showImage=this.Image:GetComponent(typeof(ImageWidget))
			    PhotoManager.downloadNetPhoto(queText[i],function (texture2D)
					printDebug("+++++++++++++++++++++++++++++++圖片i"..i)
					if texture2D == nil then
						printDebug("下载图片失败")
						--picLocateImg:SetPng(nil)    --如果没有图片，则设置图片的默认图片
					else
						printDebug("++++++++++下载图片成功!")
						ImageUtil.setTexture2DImage(texture2D,showImage.Img)
						showImage.name="000"
					--this.Image:GetComponent(typeof(ImageWidget)).Img:SetNativeSize()
					
					-- index=index+1
						--PlatformPicManagerProxy:GetInstance():addGuessPic(picName,texture2D)
					end
				
				end)
			end
		end
		printDebug("++++++++++++++++++++++++我是最终的高高哦："..answerHeight)
		this.main_mid.answer_ScrollPanel.transform:GetChild(0).transform:
		GetComponent(typeof(RectTransform)).sizeDelta=Vector2(0,answerHeight)
		
		childNum=#queText
		printDebug("+++++++++++++++++++++++++++++我是你要刪除的child數量"..#queText)
		
		this.main_mid.answer_Panel.gameObject:SetActive(true)
end


function this:onBtnDestroy(num)
	local allChild=nil
	for i=1,num do
		allChild=this.main_mid.answer_ScrollPanel.transform:GetChild(0).transform:GetChild(i-1).gameObject
		GameObject.Destroy(allChild)
	end
end


--[[
function  this:answerSplit(str,data)
	local queStr=string.gsub(data.answer, " ", "" )
    local queString=string.gsub(queStr, "%b<>", " " )
	local queText=string.split(queString," ")

	--local queText=nil
   --queText=string.gsub(data.answer, "url", "5" 
	printDebug("+++++++++++++++++++++++++++++我是裁剪的回答啊"..table.tostring(queText))
	for i=1, #queText do
		printDebug("+++++++++++++++++我是列表数目啊"..#queText)
		if queText[i]=="/url" then 
			table.remove(queText,i)
		elseif queText[i]=="url" then 
			table.remove(queText,i)
		end
		printDebug("+++++++++++++++++++++++++++++我是裁剪的回答啊"..table.tostring(queText))
	end
	local indexList={"","1","2","3","4","5"}
	local textData={}
	local imageData={}

		for i=1,#queText do
			local answertype=string.match(queText[i],"http")
			if answertype==nil or answertype=="" then
				table.insert(textData,queText[i])
				
				--index=index+1
			else
				table.insert(imageData,queText[i])
			end
			
			
   
		end
		for i=1,#textData do
			this.main_mid[string.concat("answer_text",indexList[i])].text=textData[i]
			answerHeight= answerHeight+this.main_mid[string.concat("answer_text",indexList[i])].Txt.preferredHeight
			printDebug("++++++++++++++++++++++++我是文本的高高哦："..answerHeight)
		end
		this:downLoadImage(imageData,indexList,function (answerHeight)
			if isDownLoad then
		printDebug("++++++++++++++++++++++++我是最終的高高哦："..answerHeight)
		this.main_mid.answer_ScrollPanel.transform:GetChild(0).transform:GetComponent(typeof(RectTransform)).sizeDelta=Vector2(0,answerHeight)
		this.main_mid.answer_Panel.gameObject:SetActive(true)
		end
		end)
		
end
function this:downLoadImage(data,list,callback)
	for i=1,#data do
				 PhotoManager.downloadNetPhoto(data[i],function (texture2D,callback)
	            if texture2D == nil then
	                printDebug("下载图片失败")
	                --picLocateImg:SetPng(nil)    --如果没有图片，则设置图片的默认图片
	            else
	                printDebug("++++++++++下载图片成功!")
	                ImageUtil.setTexture2DImage(texture2D,this.main_mid[string.concat("answer_image",indexList[i])].Img)
	                this.main_mid[string.concat("answer_image",indexList[i])].Img:SetNativeSize()
	               -- index=index+1
	                --PlatformPicManagerProxy:GetInstance():addGuessPic(picName,texture2D)
	            end
            myTexture2D = texture2D
            
            end)
				 answerHeight= answerHeight+1920
		end
		 
		 isDownLoad=true
		callback(answerHeight)
		printDebug("++++++++++++++++++++++++我循環結束了啊：")
		
	
end
--]]


local isSetQueOpen=false
local isRedbagQueOpen=false
local isGuessQueOpen=false
local isOtherQueOpen=false
local panelHeight=2500
--打开账号设置
function this:onBtnSetEvent(eventData)
	printDebug("++++++我点击了啊")
	isSetQueOpen=not isSetQueOpen
	printDebug("打开弹出下框")
    local mySequence1 = DOTween.Sequence()
    local move1 = nil

    local setContent=  this.main_mid.ScrollPanel.transform:GetChild(0)
    local cRectm1 =  setContent.transform:GetComponent(typeof(RectTransform)) 
    local cRectm6 =  this.main_mid.Image.transform:GetComponent(typeof(RectTransform))
    printDebug("我是你的坐标啊"..cRectm6.localPosition.x.."呀"..cRectm6.localPosition.y)
    if isSetQueOpen then 
		this.main_mid.set_go_Image.gameObject:SetActive(false)
		this.main_mid.set_up_Image.gameObject:SetActive(true)
    	--move1 = setContent.transform:DOMove(Vector3(0,1120,0), 0.2)
    	cRectm1.anchoredPosition = Vector2(0, 1120)
	    local cRectm2 =  this.main_mid.redbag_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm2.anchoredPosition = Vector2(0, cRectm2.localPosition.y-1050)
	    local cRectm3 =  this.main_mid.guess_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm3.anchoredPosition = Vector2(0, cRectm3.localPosition.y-1050)
	    local cRectm4 =  this.main_mid.other_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm4.anchoredPosition = Vector2(0, cRectm4.localPosition.y-1050)
	    this.main_mid.set_Panel.gameObject:SetActive(true)
	    panelHeight=panelHeight+1050
    else
    	this.main_mid.set_go_Image.gameObject:SetActive(true)
    	this.main_mid.set_up_Image.gameObject:SetActive(false)
    	if isRedbagQueOpen or isGuessQueOpen or isOtherQueOpen then
    		printDebug("+++++++++++++++++++++++++++我有开着的啊")
		else
	    	move1 = setContent.transform:DOLocalMove(Vector3(0,800,0), 0.2)
	    end
	    local cRectm2 =  this.main_mid.redbag_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm2.anchoredPosition = Vector2(0, cRectm2.localPosition.y+1050)
	     local cRectm3 =  this.main_mid.guess_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm3.anchoredPosition = Vector2(0, cRectm3.localPosition.y+1050)
	    local cRectm4 =  this.main_mid.other_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm4.anchoredPosition = Vector2(0, cRectm4.localPosition.y+1050)
	    this.main_mid.set_Panel.gameObject:SetActive(false)
	    panelHeight=panelHeight-1050
   end
   
	cRectm1.sizeDelta  = Vector2(0, panelHeight)
    mySequence1:Append(move1)
	

end

--打开红包优惠券
function this:onBtnRedBagEvent(eventData)
	printDebug("++++++我点击了啊")
	isRedbagQueOpen=not isRedbagQueOpen
	printDebug("打开弹出下框")
    local mySequence1 = DOTween.Sequence();
    local move1 = nil

 
    local setContent=  this.main_mid.ScrollPanel.transform:GetChild(0)
    local cRectm1 =  setContent.transform:GetComponent(typeof(RectTransform))
    if isRedbagQueOpen then 
		this.main_mid.redbag_go_Image.gameObject:SetActive(false)
		this.main_mid.redbag_up_Image.gameObject:SetActive(true)
    	--move1 = setContent.transform:DOMove(Vector3(0,1120,0), 0.2)
    	cRectm1.anchoredPosition = Vector2(0, 1120)
	    local cRectm2 =  this.main_mid.guess_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm2.anchoredPosition = Vector2(0, cRectm2.localPosition.y-1050)
	    local cRectm3 =  this.main_mid.other_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm3.anchoredPosition = Vector2(0, cRectm3.localPosition.y-1050)
	    this.main_mid.redbag_Panel.gameObject:SetActive(true)
	    panelHeight=panelHeight+1050
       
    else
    	this.main_mid.redbag_go_Image.gameObject:SetActive(true)
    	this.main_mid.redbag_up_Image.gameObject:SetActive(false)
    	local cRectm2 =  this.main_mid.guess_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm2.anchoredPosition = Vector2(0, cRectm2.localPosition.y+1050)
	    local cRectm3 =  this.main_mid.other_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm3.anchoredPosition = Vector2(0, cRectm3.localPosition.y+1050)
	    this.main_mid.redbag_Panel.gameObject:SetActive(false)
	    panelHeight=panelHeight-1050
    	if isSetQueOpen or isGuessQueOpen or isOtherQueOpen then
    		printDebug("+++++++++++++++++++++++++++我有开着的啊")
		else
	    	move1 = setContent.transform:DOLocalMove(Vector3(0,800,0), 0.2)
	    end
	    
        
   end
   
	cRectm1.sizeDelta  = Vector2(0, panelHeight)
    mySequence1:Append(move1)
 

end

--打开赛事活动
function this:onBtnGuessEvent(eventData)
	printDebug("++++++我点击了啊")
	isGuessQueOpen=not isGuessQueOpen
	printDebug("打开弹出下框")
    local mySequence1 = DOTween.Sequence();
    local move1 = nil

    local setContent=  this.main_mid.ScrollPanel.transform:GetChild(0)
    local cRectm1 =  setContent.transform:GetComponent(typeof(RectTransform))
    if isGuessQueOpen then 
		this.main_mid.guess_go_Image.gameObject:SetActive(false)
		this.main_mid.guess_up_Image.gameObject:SetActive(true)
    	--move1 = setContent.transform:DOMove(Vector3(0,1120,0), 0.2)
    	cRectm1.anchoredPosition = Vector2(0, 1120)
	    local cRectm2 =  this.main_mid.other_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm2.anchoredPosition = Vector2(0, cRectm2.localPosition.y-1050)
	    this.main_mid.guess_Panel.gameObject:SetActive(true)
	    panelHeight=panelHeight+1050
    else
    	this.main_mid.guess_go_Image.gameObject:SetActive(true)
    	this.main_mid.guess_up_Image.gameObject:SetActive(false)
    	if isSetQueOpen or isRedbagQueOpen or isOtherQueOpen then
    		printDebug("+++++++++++++++++++++++++++我有开着的啊")
		else
	    	move1 = setContent.transform:DOLocalMove(Vector3(0,800,0), 0.2)
	    end
	    local cRectm2 =  this.main_mid.other_Button.gameObject:GetComponent(typeof(RectTransform))
	    cRectm2.anchoredPosition = Vector2(0, cRectm2.localPosition.y+1050)
	    this.main_mid.guess_Panel.gameObject:SetActive(false)
	    panelHeight=panelHeight-1050
   end
   
	cRectm1.sizeDelta  = Vector2(0, panelHeight)
    mySequence1:Append(move1)
 

end

--打开其它功能
function this:onBtnOtherEvent(eventData)
	printDebug("++++++我点击了啊")
	isOtherQueOpen=not isOtherQueOpen
	printDebug("打开弹出下框")
    local mySequence1 = DOTween.Sequence();
    local move1 = nil

    local setContent=  this.main_mid.ScrollPanel.transform:GetChild(0)
    local cRectm1 =  setContent.transform:GetComponent(typeof(RectTransform))
    if isOtherQueOpen then 
    	
		this.main_mid.other_go_Image.gameObject:SetActive(false)
		this.main_mid.other_up_Image.gameObject:SetActive(true)
    	--move1 = setContent.transform:DOMove(Vector3(0,1120,0), 0.2)
    	cRectm1.anchoredPosition = Vector2(0, 1120)
	    this.main_mid.other_Panel.gameObject:SetActive(true)
	    panelHeight=panelHeight+1050
    else
    	this.main_mid.other_go_Image.gameObject:SetActive(true)
    	this.main_mid.other_up_Image.gameObject:SetActive(false)
    	if isSetQueOpen or isGuessQueOpen or isRedbagQueOpen then
    		printDebug("+++++++++++++++++++++++++++我有开着的啊")
		else
	    	move1 = setContent.transform:DOLocalMove(Vector3(0,800,0), 0.2)
	    end
	    this.main_mid.other_Panel.gameObject:SetActive(false)
	    panelHeight=panelHeight-1050
   end
   
	cRectm1.sizeDelta  = Vector2(0, panelHeight)
    mySequence1:Append(move1)
    

end
--意见反馈
function this:onBtnSendEvent(eventData)
	this.main_mid.send_Panel.gameObject:SetActive(true)
	this.main_mid.answer_Panel.gameObject:SetActive(false)
end

function this:onBtnServiceEvent(eventData)
	showFloatTips("功能开发中敬请期待！")
	this.main_mid.answer_Panel.gameObject:SetActive(false)
	this.main_mid.send_Panel.gameObject:SetActive(false)
end
----------------------------------------------------------问题反馈------------------------------------------------------------------
function this:GetTextCount()
	local count = string.len(this.main_mid.feedback_InputField.text)
	local result = nil

	if count > 1500 then 
		--result = 500
		result = math.floor(count/3)
		this.main_mid.feedback_InputField.text = this.preInputText
		this.main_mid.number_Text.text ="<color=red>"..result.."/500</color>"
		--this.main_mid.name_prompt_Text.text="（昵称长度超出允许长度，最多为8个汉字）"
	else
		result = math.floor(count/3)
		this.preInputText = this.main_mid.feedback_InputField.text
		this.main_mid.number_Text.text = result.."/500"
		--this.main_mid.name_prompt_Text.text=""
	end

	
	this.referText = this.main_mid.feedback_InputField.text

	
  this:GetUserID()

	
end


function this:GetUserID( ... )
	-- body
end


function this:onBtnReferEvent()
	printDebug("+++++++++++++++++++++++我发送了啊")
	if this.main_mid.feedback_InputField.text=="" or this.main_mid.feedback_InputField.text==nil then
        this.main_mid.warn_Text.text="反馈内容不能为空！"
    else
        this.main_mid.warn_Text.text=""
    end
end

