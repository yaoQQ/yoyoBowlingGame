local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ImageWidget=CS.ImageWidget
local GridRecycleScrollWidget=CS.GridRecycleScrollWidget

Mid_platform_global_shop_panel={}
local this = Mid_platform_global_shop_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.head_Image=nil
this.name_Text=nil
this.baselabel_CellRecycleScrollPanel=nil
this.addr_Text=nil
this.distance_Text=nil
this.label_CellRecycleScrollPanel=nil
this.more_Text=nil
this.back_Image=nil
this.cellitem1_shop_image=nil
this.cellitem4_shop_image=nil
this.cellitem2_shop_image=nil
this.cellitem5_shop_image=nil
this.cellitem3_shop_image=nil
this.cellitem6_shop_image=nil
this.game_CellGroup=nil
this.gameCount_Text=nil
--Baselabelcell数组
this.baselabelcellArr={}
--Labelcell数组
this.labelcellArr={}
--Gamecell数组
this.gamecellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.head_Image=self.go.transform:Find("top_Panel/head_Image"):GetComponent(typeof(CircleImageWidget))
	self.name_Text=self.go.transform:Find("top_Panel/name_Text"):GetComponent(typeof(TextWidget))
	self.baselabel_CellRecycleScrollPanel=self.go.transform:Find("top_Panel/baselabel_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.addr_Text=self.go.transform:Find("top_Panel/addr_Text"):GetComponent(typeof(TextWidget))
	self.distance_Text=self.go.transform:Find("top_Panel/distance_Text"):GetComponent(typeof(TextWidget))
	self.label_CellRecycleScrollPanel=self.go.transform:Find("top_Panel/label_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.more_Text=self.go.transform:Find("top_Panel/more_Text"):GetComponent(typeof(TextWidget))
	self.back_Image=self.go.transform:Find("top_Panel/back_Image"):GetComponent(typeof(CircleImageWidget))
	self.cellitem1_shop_image=self.go.transform:Find("mid_Panel/pic_CellGroup/cellitem1_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem4_shop_image=self.go.transform:Find("mid_Panel/pic_CellGroup/cellitem4_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem2_shop_image=self.go.transform:Find("mid_Panel/pic_CellGroup/cellitem2_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem5_shop_image=self.go.transform:Find("mid_Panel/pic_CellGroup/cellitem5_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem3_shop_image=self.go.transform:Find("mid_Panel/pic_CellGroup/cellitem3_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem6_shop_image=self.go.transform:Find("mid_Panel/pic_CellGroup/cellitem6_shop_image"):GetComponent(typeof(ImageWidget))
	self.game_CellGroup=self.go.transform:Find("bottom_Panel/game_CellGroup"):GetComponent(typeof(GridRecycleScrollWidget))
	self.gameCount_Text=self.go.transform:Find("bottom_Panel/gameCount_Text"):GetComponent(typeof(TextWidget))
	self.baselabelcellArr={}
	table.insert(self.baselabelcellArr,self.new_Baselabelcell(self.go.transform:Find("top_Panel/baselabel_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.baselabelcellArr,self.new_Baselabelcell(self.go.transform:Find("top_Panel/baselabel_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.baselabelcellArr,self.new_Baselabelcell(self.go.transform:Find("top_Panel/baselabel_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.baselabelcellArr,self.new_Baselabelcell(self.go.transform:Find("top_Panel/baselabel_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	self.labelcellArr={}
	table.insert(self.labelcellArr,self.new_Labelcell(self.go.transform:Find("top_Panel/label_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.labelcellArr,self.new_Labelcell(self.go.transform:Find("top_Panel/label_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.labelcellArr,self.new_Labelcell(self.go.transform:Find("top_Panel/label_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.labelcellArr,self.new_Labelcell(self.go.transform:Find("top_Panel/label_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	self.gamecellArr={}
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_0_1").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_0_2").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_0_3").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_1_0").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_1_1").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_1_2").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_1_3").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_2_0").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_2_1").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_2_2").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_2_3").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_3_0").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_3_1").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_3_2").gameObject))
	table.insert(self.gamecellArr,self.new_Gamecell(self.go.transform:Find("bottom_Panel/game_CellGroup/content/cellitem_3_3").gameObject))
end

--Baselabelcell复用单元
function this.new_Baselabelcell(itemGo)
	local item = { }
	item.go = itemGo
	item.baselabel_Text=itemGo.transform:Find("baselabel_Text"):GetComponent(typeof(TextWidget))
	return item
end
--Labelcell复用单元
function this.new_Labelcell(itemGo)
	local item = { }
	item.go = itemGo
	item.base_Image=itemGo.transform:Find("base_Image"):GetComponent(typeof(ImageWidget))
	item.baselabel_Text=itemGo.transform:Find("baselabel_Text"):GetComponent(typeof(TextWidget))
	return item
end
--Gamecell复用单元
function this.new_Gamecell(itemGo)
	local item = { }
	item.go = itemGo
	item.press_Image=itemGo.transform:Find("press_Image"):GetComponent(typeof(ImageWidget))
	item.head_Image=itemGo.transform:Find("head_Image"):GetComponent(typeof(ImageWidget))
	item.gameName_Text=itemGo.transform:Find("gameName_Text"):GetComponent(typeof(TextWidget))
	item.reward_Text=itemGo.transform:Find("reward_Text"):GetComponent(typeof(TextWidget))
	item.cd_Text=itemGo.transform:Find("cd_Text"):GetComponent(typeof(TextWidget))
	item.des_Text=itemGo.transform:Find("des_Text"):GetComponent(typeof(TextWidget))
	return item
end

