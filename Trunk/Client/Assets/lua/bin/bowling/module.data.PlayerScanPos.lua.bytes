
PlayerScanPos = {}
local this = PlayerScanPos

--"time": 1548669128.0365636, "content": ["person_id": 1, "info": "right_wrist": [567.008391399868, 534.373706516928, 1463.6448455418245], "left_wrist": [936.0287094824542, 507.37302222910057, 1811.8522032925746]], "pid": 3, "sequence": 3
--[[2019-01-28 18:04:42,368  LUA: @@@@@@@@@@@@@@@acceptPositionContentRsp 返回=json.object: 0000000072DD0930
{
  sequence = 1826
  pid = 3
  content = json.array: 0000000072DD0A30
  {
    [1] = json.object: 0000000072DD0EF0
    {
      info = json.object: 0000000072DD0A70
      {
        left_wrist = json.array: 0000000072DD1030
        {
          [1] = 902.27701622627
          [2] = 687.37813731226
          [3] = 1482.4932255999
        }
        right_wrist = json.array: 0000000072DD0FF0
        {
          [1] = 524.25567007645
          [2] = 345.3680728191
          [3] = 1756.3897074569
        }
      }
      person_id = 1
    }
  }
  time = 1548669870.982
}--]]

PlayerScanPos.personList={}
this.selectPerson=1
this.farward ={isLeft =0,isRight =1,isUP =2,isDown =3,none=-1}
this.perfectPosList={}
PlayerScanPos.isRightHandle=true
PlayerScanPos.isSelectPerson =false
this.posMaxCount=12

function PlayerScanPos.initPerson()
	PlayerScanPos.personList={}
	local person={}

	person.leftPosList={}
	person.rightPosList={}
	this.perfectPosList={}
	table.insert(PlayerScanPos.personList,person)
	BowlingScene.printColor("blue","PlayerScanPos.initPerson()")
end
function PlayerScanPos.addPersonPos(data)
	--BowlingScene.printColor("red","PlayerScanPos.addPersonPos()")
	--BowlingScene.printColor("red","PlayerScanPos.addPersonPos()  data="..table.tostring(data))
	 local time = data.time
	local personCount = #data.content
	local sequence = data.sequence
	
	local sendLeftAndRightPos={}
	--BowlingScene.printColor("red","PlayerScanPos.addPersonPos()  data.content="..tostring(data.content))
	--	BowlingScene.printColor("red","PlayerScanPos.addPersonPos()  data.content="..tostring(#data.content))
	for i=1,#data.content do
	--	printDebug(" #this.personList="..tostring(#this.personList).." i="..tostring(i))
		--printDebug(" #this.personList="..table.tostring(this.personList).." i="..tostring(i))
		if #this.personList<i then
			local newPerson={}
			newPerson.leftPosList={}
			newPerson.rightPosList={}
			newPerson.person_id =  data.content[i].person_id
			table.insert(PlayerScanPos.personList,newPerson)
			--BowlingScene.printColor("blue","生成人物轨迹 person.person_id="..newPerson.person_id)
		end
		local person = this.personList[i]
		local leftPos = Vector4(data.content[i].info.left_wrist[1],data.content[i].info.left_wrist[2],data.content[i].info.left_wrist[3],sequence)
		person.left_wrist = leftPos
		sendLeftAndRightPos[1] = leftPos
	--	if this.personList[1] then

	--	end

		table.insert(person.leftPosList,leftPos)
		if #person.leftPosList>this.posMaxCount then
			table.remove(person.leftPosList,1)
			--BowlingScene.printColor("blue","生成人物轨迹 person.leftPosList="..table.tostring(person.leftPosList))
		end

		local rightPos = Vector4(data.content[i].info.right_wrist[1],data.content[i].info.right_wrist[2],data.content[i].info.right_wrist[3],sequence)
		person.right_wrist = rightPos
		sendLeftAndRightPos[2] = rightPos
		table.insert(person.rightPosList,rightPos)
		if #person.rightPosList>this.posMaxCount then
			table.remove(person.rightPosList,1)
		end
		
		this.sendLefftAndRightPos(sendLeftAndRightPos)
	end
			
	--printDebug("<color='blue'>生成人物轨迹 this.personList=="..table.tostring(this.personList).."</color>")
end

function this.sendLefftAndRightPos(sendLeftAndRightPos)
		NoticeManager.Instance:Dispatch(BowlingEvent.updateScreenPos,sendLeftAndRightPos)
end
function PlayerScanPos.addperfectPosList(posList)
		if posList==nil then
			printDebug("<color='red'>=================posList==nil =========================</color>")
			return
		end
	--	printDebug("<color='yellow'>=================begain==x移动=========================</color>")
	--	printDebug("<color='yellow'>posList="..table.tostring(posList))
		showTopTips("addperfectPosList() posList.farward="..tostring(posList.farward))
		local fwardStr = PlayerScanPos.returnStr(posList.farward)
		--table.insert(this.perfectPosList,posList)
		this.perfectPosList = posList
	--	BowlingPunishTimeView.setScreenUpdatePos("收到一条完整轨迹 count="..tostring(#this.perfectPosList))
		--showTopTips("收到一条完整轨迹 #posList="..tostring(#posList).." 方向="..tostring(fwardStr).." 完整总数据数="..tostring(#this.perfectPosList))
		

end
function PlayerScanPos.getPerfectPosList()
		if this.perfectPosList~=nil and #this.perfectPosList>0 then
			local posList = this.perfectPosList
			local fwardStr = PlayerScanPos.returnStr(posList.farward)
			showTopTips("移除一条完整轨迹 #this.perfectPosList="..tostring(#this.perfectPosList).." 方向="..tostring(fwardStr))
			return this.perfectPosList
		end
		return nil
end

function PlayerScanPos.returnStr(num)
	--showTopTips(" num="..tostring(num).." this.farward.isRight="..tostring(this.farward.isRight))
	if num==this.farward.isLeft then
		return "向左"
	elseif num==this.farward.isRight then
		return "向右"
	elseif num==this.farward.isUP then
		return "向上"
	elseif num==this.farward.isDown then
		return "向下"
	else
		return "无方向"
	end
end
function PlayerScanPos.init()
	
end
function this.addNotice()
	 NoticeManager.Instance:AddNoticeLister(BowlingEvent.onGameResetStart,this.ReStartGame)
end
function this.removeNotice()
	 NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.onGameResetStart, this.ReStartGame)
end

function this.ReStartGame()
	PlayerScanPos.personList={}
	this.selectPerson=1	
	this.perfectPosList={}
end


this.everyAngel=1
this.countPos = 10
--屏幕位置点转为保龄球朝向度数
function this.countBallfowardAngel(pos1,pos2)

	local ballToCurrPosDis = Vector3.Distance(pos1,pos2) --球与目标点距离
	if  ballToCurrPosDis<=1 then
	--	printDebug("pos1="..tostring(pos1).."  pos2="..tostring(pos2).." angel="..tostring(angel))
		return this.everyAngel
	end
	local currPosToBallXDis = pos2.x -pos1.x
	local angel = Mathf.Asin(currPosToBallXDis/ballToCurrPosDis)*Mathf.Rad2Deg  -- 轨迹圆弧角度
--	angel=angel/10
	--printDebug("pos1="..tostring(pos1).."  pos2="..tostring(pos2).." angel="..tostring(angel))
	return angel
end

this.begainCountSize=10
function PlayerScanPos.getSamePosList()
	local posiList= this.getCurrPosList()
	if posiList==nil or #posiList<this.begainCountSize  then
		return nil
	end
	--	printDebug("<color='yellow'>=================begain==z移动=========================</color>")
	local ZPosList=this.getRightZPosList(posiList)
	if ZPosList~=nil then
		PlayerScanPos.addperfectPosList(ZPosList)
		return ZPosList
	end

	return nil
end



--获取最大z坐标
function this.getMaxPosZIndex(posiList)
	local maxZ=posiList[1].z
	local index=1
	for i=1,#posiList do
		if posiList[i].z>=maxZ then
			maxZ = posiList[i].z
			index =i
		end
	end
	local maxZdata={index,maxZ}
	return maxZdata
end
--获取最小z坐标
function this.getMinPosZIndex(posiList)
	local minZ=posiList[1].z
	local index=1
	for i=1,#posiList do
		if posiList[i].z<=minZ then
			minZ = posiList[i].z
			index =i
		end
	end
	local minZdata={index,minZ}
	return minZdata
end

this.ZSizenum =10
--获取同方向的坐标数目
function this.getSameFarwordCount(posiList)
	local countUp =0
	local countDown=0
	local data={}
	if BowlingGameManager.isEditor then
		this.ZSizenum =2
	else
		this.ZSizenum =10
	end
	for i=1,#posiList-1 do 
		local begainVec = posiList[i]
		local endVec = posiList[i+1]


		local targetEndFowardZ = endVec.z-begainVec.z 
		if targetEndFowardZ>=0 then
			if targetEndFowardZ>this.ZSizenum then
				countUp = countUp+1
			end
		else
			if targetEndFowardZ<this.ZSizenum then
				countDown = countDown+1
			end
		end
	end
	if countUp<countDown then
		data[1] =this.farward.isDown
		data[2] =countDown
	else
		data[1] =this.farward.isUP
		data[2] =countUp
	end
	return data
end

--判断Z方向
function this.getRightPosList(index,posiList)
		local samePosList={}
--		printDebug("%%%%%%%%%index="..tostring(index))
		for i=1,index do
			table.insert(samePosList,posiList[1])
			table.remove(posiList,1)
		end
	--	printDebug("%%%%%%%%%samePosList="..table.tostring(samePosList))
		return samePosList
end

this.NewDiance = 300
this.canFireCount = 7
function this.getRightZPosList(posiList)


	local maxZdata = this.getMaxPosZIndex(posiList)
	local minZdata = this.getMinPosZIndex(posiList)
	local maxIndex = maxZdata[1]
	local minIndex = minZdata[1]
	local fard = this.farward.none
	local distance = Mathf.Abs(maxZdata[2]-minZdata[2])
	if distance<this.NewDiance then
		return nil
	end
	BowlingPunishTimeView.setScreenUpdatePos("max="..tostring(maxZdata[2]).." min="..tostring(minZdata[2]).." distance="..tostring(distance)) 
	if minIndex>maxIndex then
		fard = this.farward.isUP
	elseif minIndex<maxIndex then
		fard = this.farward.isDown
	end
	--this.farward ={isLeft =0,isRight =1,isUP =2,isDown =3,none=-1}
	if fard==this.farward.isUP then
		local minIndex = maxZdata[1]
		local samePosList= this.getRightPosList(minIndex,posiList)
		samePosList.farward = fard
		showFloatTips("方向向上 distance="..tostring(distance))
		return samePosList
	elseif fard==this.farward.isDown then
		showFloatTips("方向向下 distance="..tostring(distance))
		return nil
	end
	return nil
	--printDebug("%%%%%%%%%maxZdata="..table.tostring(maxZdata))
	--printDebug("%%%%%%%%%minZdata="..table.tostring(minZdata))
	
--[[	local farwardZData = this.getSameFarwordCount(posiList)

	local fardCount = farwardZData[2] --方向上的数量
	
	local canFireCount =7
	if BowlingGameManager.isEditor then
		this.canFireCount =7
	else
		this.canFireCount =8
	end
	if fardCount<this.canFireCount then
		showTopTips("无效fard="..tostring(fard).."fardCount="..tostring(fardCount.."<"..tostring(this.canFireCount)))
		return 
	end
	showTopTips("distance="..tostring(distance).."fard="..tostring(fard))--]]
	
--[[	if fard==this.farward.isUP then
		local index = maxZdata[1]

		local samePosList= this.getRightPosList(index,posiList)
		samePosList.farward = fard
		return samePosList
	elseif fard==this.farward.isDown then
		local index = minZdata[1]
		local samePosList= this.getRightPosList(index,posiList)
		samePosList.farward = fard
		return samePosList
	end--]]
	--return nil
end


	

--添加测试点
function PlayerScanPos.addTestPos()
	GlobalTimeManager.Instance.timerController:AddTimer("testAddPos", 500, -1, this.addTestPosFun)	
end

--停止点
function PlayerScanPos.stopTestPos()
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("testAddPos", 500, -1, this.addTestPosFun)	
end

function this.addTestPosFun()
	if BowlingGameManager.isEditor then
		if BowlingScene.playerBowling.m_fired then
			return
		end
		local addPos = Vector3.zero
		if this.testAddZPos then
			addPos = Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.y)
		else
			addPos = Input.mousePosition
		end
		PlayerScanPos.testAddPersonPos(addPos)
	end
	
end



function PlayerScanPos.testPerson()
	PlayerScanPos.initPerson()
	
end
function PlayerScanPos.testAddPersonPos(pos)

	
	local content1 = {}
	content1[1]={}
	content1[1].info={}
	content1[1].person_id = 1

	content1[1].info.left_wrist={pos.x,pos.y,pos.z}
	content1[1].info.right_wrist = {-pos.x,-pos.y,pos.z}
	local testPersonData={sequence=1,time=1,pid=3,content=content1}
	--testPersonData.content={content1}
	--printDebug("PlayerScanPos.addPersonPos() testPersonData="..table.tostring(testPersonData))
	PlayerScanPos.addPersonPos(testPersonData)
end




function PlayerScanPos.isPosInBall(person,UIMid)
	local ballPos =BowlingBall.getBallScreenPos()
	local rightPosList = person.rightPosList
	local leftPosList = person.leftPosList
	--BowlingScene.printColor("red"," ballPos="..tostring(ballPos))
	
	local rightPos= this.removePos(rightPosList)
	if rightPos~=nil then
		--BowlingPunishTimeView.setRightUpdatePos(rightPos) 
	end
	if rightPos~=nil then
		--local screePos = Vector3(rightPos.x,rightPos.y,ballPos.z)
		--local distance = Vector3.Distance(screePos,ballPos)
		local screenPos = Vector3(rightPos.x,rightPos.y,0)
		if BowlingUtils.isScreenPosInUI(UIMid,screenPos) then
			this.isRightHandle =true
			BowlingPunishTimeView.setScreenUpdatePos("选择了右手！！")
			return person
		end
	end

	
	local leftPos= this.removePos(leftPosList)
	if leftPos~=nil then
		--BowlingPunishTimeView.setLeftUpdatePos(leftPos) 
	end
	if leftPos~=nil  then

		local screenPos = Vector3(leftPos.x,leftPos.y,0)
		if BowlingUtils.isScreenPosInUI(UIMid,screenPos) then
			this.isRightHandle =false
			BowlingPunishTimeView.setScreenUpdatePos("选择了左手！！")
			return person
		end
	end
	return nil
end

function PlayerScanPos.switchPerson(UIMid)
	--BowlingScene.printColor("red","this.personList="..table.tostring(this.personList))
	for i=1,#this.personList do
		local isThePerson= PlayerScanPos.isPosInBall(this.personList[i],UIMid)
		if isThePerson then
			
			PlayerScanPos.initPerson()
			showFloatTips("选中人物 person_id="..tostring("1"))
			MVPGameModule.sendChoosePerson(1)
			return isThePerson
		end
	end
	return nil
end

function PlayerScanPos.getPlayPerson()
--	printDebug(" this.selectPerson="..tostring(this.selectPerson))
	--printDebug(" #this.personLis="..tostring(#this.personList))
--	printDebug(" #this.personLis="..table.tostring(this.personList))
	if this.selectPerson<=#this.personList then
		local person = this.personList[1]
		--printDebug(" getPlayPerson() person="..table.tostring(person))
		return person
	end
	return nil
end
function this.getPerson(num)
	if #this.personList>=1 then
		return this.personList[1]
	end
	return nil
end

function this.getCurrPosList()
	local person=PlayerScanPos.getPlayPerson()
--	printDebug(" this.getCurrPosList() persons="..tostring(person))
	if person==nil then
		showTopTips("this.getCurrPosList() 没有获取到对应人物  return nil")
		return nil
	end
	if this.isRightHandle then
		return person.rightPosList
	else
		return person.leftPosList
	end
end

--this.moveZDistance = 400
this.moveZDistance = 400
function PlayerScanPos.isMouseUp()
	if BowlingGameManager.isEditor then
		this.moveZDistance = 80
	else
		this.moveZDistance = 400
	end
	local currPosList= this.getCurrPosList()
	if currPosList==nil then
		return false
	end
	local count = #currPosList
	if count<this.posMaxCount-2 then
		return false
	end
	local begainPosZ = currPosList[1].z
	local endPosZ = currPosList[count].z
	local distanceZ =endPosZ - begainPosZ
	showFloatTips("fire="..tostring(this.moveZDistance).." 前后移动距离 distanceZ="..distanceZ)
	if distanceZ>this.moveZDistance then
		BowlingScene.printColor("blue","@@@@@@@PlayerScanPos.isMouseUp()  isMouseUp=true!!!!")
		--showFloatTips("前后移动距离 distanceZ="..distanceZ)
		return true
	end
end



function PlayerScanPos.getCurrPos()
	local posList= this.getCurrPosList()
	if posList==nil then
		return nil
	end
	if #posList>=1 then
		local pos = posList[1]
		table.remove(posList,1)
		return Vector3(pos.x,pos.y,pos.z)
	end
	return nil
end

function this.removePos(posList)
	if posList==nil then
		return nil
	end
	if #posList>=1 then
		local pos = posList[1]
		table.remove(posList,1)
		return Vector3(pos.x,pos.y,pos.z)
	end
	return nil
end

