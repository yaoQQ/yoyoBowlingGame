


PlatformCouponProxy={}
local this=PlatformCouponProxy



this.data = {}

this.allCouponListData = {}
this.availCouponListData = {}
this.usedCouponListData = {}
this.overDueCouponListData = {}

--设置所有的卡券信息
function this.setAllCouponData(data)
	--printDebug("all Coupons  :" ..table.tostring(data) )
	this.allCouponData = data.user_coupon
	this.availCouponListData = {}
	this.usedCouponListData = {}
	this.overDueCouponListData = {}	
	if  this.allCouponData == nil then
		this.allCouponData={}
		return
	end
	for k,v in ipairs(this.allCouponData) do
			--printDebug("coupon state :"..table.tostring(v))
		if v.state == ProtoEnumCommon.UserCouponState.UserCouponState_Usable  then
			table.insert(this.availCouponListData,v)
		elseif  v.state == ProtoEnumCommon.UserCouponState.UserCouponState_Used then
			table.insert(this.usedCouponListData,v)
		else 
			table.insert(this.overDueCouponListData,v)
		end
	end
	
	
end

--可使用的卡券
function this.getAvailCouponListData()
	return this.availCouponListData
end

--获取已使用的卡券
function this.getUsedCouponListData()
	return this.usedCouponListData
end

--获取已过期的卡券
function this.getOverDueCouponListData()
	return this.overDueCouponListData
end

function this.whenCouponUse()
	local SingleData = this.currSelectedCouponData
	this.currSelectedCouponData.state = ProtoEnumCommon.UserCouponState.UserCouponState_Used  -- "UserCouponState_Used"
	for k,v in ipairs(this.allCouponData) do
		if v.coupon_code == SingleData.coupon_code then
			v.state = ProtoEnumCommon.UserCouponState.UserCouponState_Used --"UserCouponState_Used"
			--this.allCouponData[k] = SingleData
			printDebug("whenCouponUse change !! v  :" ..table.tostring(v) )
		end
	end
	for k,v in ipairs(this.availCouponListData) do
		if v.coupon_code == SingleData.coupon_code then
			printDebug("whenCouponUse  remove   :"..table.tostring(v) )
			table.remove(this.availCouponListData,k)
		end
	end	
	
	table.insert(this.usedCouponListData,SingleData)
end

function this.setSingleCouponData(SingleData)
	if SingleData.state ==  "UserCouponState_Overdue" then
		--找到是否已经有了 overDueCouponListData
			--如果没有，加入add后， 改变其他两个卡包 move 
			--
		
	elseif SingleData.state == "UserCouponState_Used" then
		--找到是否已经有了 usedCouponListData
			--如果没有，加入add后， 改变其他两个卡包 move 
			--
	elseif SingleData.state ==  "UserCouponState_Usable" then
		--找到是否已经有了 availCouponListData
			--如果没有，加入add后， 改变其他两个卡包 move 
			--		
	end

end




this.currSelectedCouponData = nil
--根据id设置当前卡券详细信息
function this.setSelectedCouponDataByCouponId(coupon_code)
	--循环遍历出 当前卡券
	for k,v in ipairs(this.allCouponData) do
		if v.coupon_code == coupon_code then
			this.currSelectedCouponData  =  v
			
			break
		end
	end
end

function this.getSelectedCouponData()
    return this.currSelectedCouponData
end


this.currCouponUseShopsData = nil
--根据id设置当前卡券适用门店信息
function this.setCouponUseShopsDataById(shopid)
	--从当前卡券循环遍历出当前门店
	
	for k,v in ipairs(this.currSelectedCouponData.shop_list) do
		if v.shop_id ==  shopid then
			this.currCouponUseShopsData  = v
			break
		end
	end
end

--获取当前卡券门店信息  MsgShopAttribute
function this.getCouponUseShopsData()
    return this.currCouponUseShopsData
end


--卡券对应的门店信息
this.currCouponShopList = nil
function this.setCouponShopList (data)
	this.currCouponShopList = data
end

function this.getCouponShopList ()
	if this.currCouponShopList == nil then
		this.currCouponShopList = {}
	end
	return	this.currCouponShopList
end

