require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:manager/MapManager"
require "base:mid/Mid_map_panel"
require "base:module/main/Map3DScene"

MapView = BaseView:new()
local this = MapView
this.viewName = "MapView"

--设置面板特性
this:setViewAttribute(UIViewType.Map_View, UIViewEnum.MapView, false)

local UIExEventTool = CS.UIExEventTool

--设置加载列表
this.loadOrders = {
    "base:map_panel"
}

this.isInitMapPos = false
this.bg = nil
this.cellGroupTrans = nil
this.mapTileObjs = {} --正在显示的地图瓦片对象
this.tpl = nil
this.defaultSprite = nil
this.noUsedTileQueue = nil

--开启时的默认坐标
local m_defaultlng = nil
local m_defaultlat = nil

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    --.静态方法
    UITools.SetParentAndAlign(gameObject, self.container)

    --初始化地图
    self:initMap()
    self.main_mid.tiles:AddEventListener(UIEvent.Drag, this.onDrag)
    self.main_mid.tiles:AddExEventListener(UIEvent.PointerDoubleClick, this.onDoubleClick)
end

--override 打开UI回调
function this:onShowHandler(msg)
    self:addNotice()

    if msg == nil then
        m_defaultlng = nil
        m_defaultlat = nil
    else
        m_defaultlng = msg.lng
        m_defaultlat = msg.lat
        self:setCurLngLatZoom(m_defaultlng, m_defaultlat, MapManager.defaultZoom)
    end

    if Main.is3DMap then
        this.bg:SetActive(false)
        Map3DScene:enterScene()
    else
        self:getLocation()
    end
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()

    if Main.is3DMap then
        Map3DScene:exitScene()
    end
end

function this:addNotice()
    if not Main.is3DMap then
        GlobalTimeManager.Instance.timerController:AddTimer("Input", -1, -1, this.onInput)

        NoticeManager.Instance:AddNoticeLister(NoticeType.Login_LoginGatewaySucceed, this.onConnect)
        NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Add, this.onMapAdd)
        NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Sub, this.onMapSub)
        NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Back_My_Pos, this.onMapBackMyPos)
        NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Change_Cur_Pos, this.onMapChangeCurPos)
        NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Change_Cur_Zoom, this.onMapChangeCurZoom)
    end
end

function this:removeNotice()
    if not Main.is3DMap then
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey("Input")

        NoticeManager.Instance:RemoveNoticeLister(NoticeType.Login_LoginGatewaySucceed, this.onConnect)
        NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Add, this.onMapAdd)
        NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Sub, this.onMapSub)
        NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Back_My_Pos, this.onMapBackMyPos)
        NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Change_Cur_Pos, this.onMapChangeCurPos)
        NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Change_Cur_Zoom, this.onMapChangeCurZoom)
    end
end

function this.onConnect()
    if not Main.is3DMap then
        this:getLocation()
    end
end

function this.onMapAdd()
    this:mapAdd()
end

function this.onMapSub()
    this:mapSub()
end

function this.onMapBackMyPos()
    this:mapBackMyPos()
end

function this.onMapChangeCurPos(notice, data)
    local mapPos = data:GetObj()
    MapManager.setCurLngLatZoom(mapPos.lng, mapPos.lat, MapManager.zoom)
    this:updateTileMove()
    this:updateGroup()
end

function this.onMapChangeCurZoom(notice, data)
    this:updateTileMove()
    this:updateGroup()
end

--初始化地图瓦片
function this:initMap()
    this.bg = self.main_mid.bg.gameObject
    this.cellGroupTrans = self.main_mid.tiles.transform
    this.tpl = self.main_mid.tileItem.gameObject
    this.defaultSprite = this.tpl:GetComponent(typeof(Image)).sprite
    this.tpl:SetActive(false)
    this.noUsedTileQueue = Queue:new()
end

--获取位置
function this:getLocation()
    if IS_UNITY_EDITOR then
        --编辑器直接用默认位置
        MapManager.userCityCode = "020"
        MapManager.userCountry = "中国"
        MapManager.userProvince = "广东省"
        MapManager.userCity = "广州市"
        MapManager.userDistrict = "天河区"
        self:setUserLngLat(MapManager.defaultLng, MapManager.defaultLat)
        GlobalTimeManager.Instance.timerController:AddTimer(
            "getLocation",
            3000,
            -1,
            function()
                self:setUserLngLat(MapManager.defaultLng, MapManager.defaultLat)
            end
        )
    else
        require "base:manager/MapManager"
        MapManager.StartLocation(
            function(locationInfo)
                --回调locationInfo格式：
                --成功：1|经度|纬度|城市编码|国家名|省名|城市名|城区名
                --失败：错误描述
                --printDebug("locationInfo:" .. locationInfo)
                if string.byte(locationInfo) == 49 then
                    local strs = string.split(locationInfo, "|")
                    local lng = tonumber(strs[2])
                    local lat = tonumber(strs[3])
                    MapManager.userCityCode = strs[4]
                    MapManager.userCountry = strs[5]
                    MapManager.userProvince = strs[6]
                    MapManager.userCity = strs[7]
                    MapManager.userDistrict = strs[8]
                    self:setUserLngLat(lng, lat)
                    MapManager.defaultLng = lng
                    MapManager.defaultLat = lat
                elseif (locationInfo == "unauthorized") then
                    if  MapManager.defaultLng == 113.356334 and MapManager.defaultLat == 23.135237  then
                            MapManager.userCityCode = "020"
                            MapManager.userCountry = "中国"
                            MapManager.userProvince = "广东省"
                            MapManager.userCity = "广州市"
                            MapManager.userDistrict = "天河区"
                    end
                    self:setUserLngLat(MapManager.defaultLng, MapManager.defaultLat)
                end
            end
        )
    end
end

--设置用户经纬度
function this:setUserLngLat(lng, lat)
    MapManager.userLng = lng
    MapManager.userLat = lat

    --首次设置用户经纬度时初始化地图瓦片
    if not this.isInitMapPos then
        this.isInitMapPos = true
        MapManager.setCurLngLatZoom(lng, lat, MapManager.zoom)

        --[[local mapPos = MapPos:new(lng, lat)
		local x, y = mapPos:getScreenPosFromCenter()
		printDebug("x".."_"..x)
		printDebug("y".."_"..y)--]]
        --[[local x, y = MapManager.getScreenPosFromCenter(MapManager.zoom, MapManager.curTileX, MapManager.curTileY, MapManager.curPixelX, MapManager.curPixelY)
		printDebug("x".."_"..x)
		printDebug("y".."_"..y)--]]
        --[[local lng, lat = MapManager.TileXYToLatLng(49295, 10281, MapManager.zoom, 128, 200)
		printDebug("lng".."_"..lng)
		printDebug("lat".."_"..lat)--]]
        self:createTiles()
        self:updateGroup()
		
		--刷新官方赛位置
		PlatformLBSDataProxy.updateOfficalActivityLngLat()
    end

    if lng - MapManager.userReqLng > 0.002 or lat - MapManager.userReqLat > 0.002 then
        if LoginDataProxy.isGetUserInfo then
            MapManager.userReqLng = lng
            MapManager.userReqLat = lat
            PlatformUserModule.sendReqUpUserPosition(MapManager.userLng, MapManager.userLat, MapManager.userCityCode)
        end
    end
end

--创建瓦片
function this:createTiles()
    printDebug("createTiles")
    for i = 1, MapManager.rowCount do
        for j = 1, MapManager.colCount do
            local tile
            local tileTrans
            if this.noUsedTileQueue.count > 0 then
                tileTrans = Queue.dequeue(this.noUsedTileQueue)
                tile = tileTrans.gameObject
            else
                tile = GameObject.Instantiate(this.tpl)
                tileTrans = tile.transform
                tileTrans:SetParent(this.cellGroupTrans)
            end
            tile:SetActive(true)
            local image = tileTrans:GetComponent(typeof(Image))
            local itemTileX = j - 1 - math.floor((MapManager.colCount - 1) / 2) + MapManager.curTileX
            local itemTileY = -i + 1 + math.floor((MapManager.rowCount - 1) / 2) + MapManager.curTileY
            local key = MapManager.zoom .. "_" .. itemTileX .. "_" .. itemTileY
            local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize
            local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize
            if not MapManager.isBaiduMap then
                localPosY = -localPosY
            end
            tile.name = key
            tileTrans.localPosition = Vector3(localPosX, localPosY, 0)
            tileTrans.localScale = Vector3(1, 1, 1)

            this.mapTileObjs[key] = {}
            this.mapTileObjs[key].trans = tileTrans
            this.mapTileObjs[key].image = image
            this.mapTileObjs[key].zoom = MapManager.zoom
            this.mapTileObjs[key].x = itemTileX
            this.mapTileObjs[key].y = itemTileY
            --printDebug("初始化地图瓦片"..tile.name.."_"..tileTrans.name.."_"..itemTileX.."_"..itemTileY)

            self:getMapTileSprite(MapManager.zoom, itemTileX, itemTileY)
        end
    end
end

--更新瓦片位置
function this:updateTile()
    for _, v in pairs(this.mapTileObjs) do
        local tileTrans = v.trans
        local itemTileX = v.x
        local itemTileY = v.y
        local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize
        local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize
        if not MapManager.isBaiduMap then
            localPosY = -localPosY
        end
        tileTrans.localPosition = Vector3(localPosX, localPosY, 0)
    end
end

--更新瓦片容器位置
function this:updateGroup()
    local localPosX = (MapManager.tileSize / 2 - MapManager.curPixelX) * MapManager.scale
    local localPosY = (MapManager.tileSize / 2 - MapManager.curPixelY) * MapManager.scale
    if MapManager.isBaiduMap then
        localPosY = (MapManager.tileSize / 2 - MapManager.curPixelY) * MapManager.scale
    else
        localPosY = (MapManager.tileSize / 2 - 256 + MapManager.curPixelY) * MapManager.scale
    end
    this.cellGroupTrans.localPosition = Vector3(localPosX, localPosY, 0)
    this.cellGroupTrans.localScale = Vector3(MapManager.scale, MapManager.scale, 1)
end

--获取地图瓦片
function this:getMapTileSprites()
    MapManager.loadMapTiles(
        MapManager.zoom,
        MapManager.curTileX,
        MapManager.curTileY,
        (MapManager.colCount - 1) / 2,
        (MapManager.rowCount - 1) / 2,
        function(zoom, x, y, sprite)
            local key = MapManager.zoom .. "_" .. x .. "_" .. y
            if this.mapTileObjs[key] ~= nil then
                local image = this.mapTileObjs[key].image
                --image.transform.parent.name = key
                image.sprite = sprite
            end
        end
    )
end

--获取地图瓦片
function this:getMapTileSprite(zoom, x, y)
    MapManager.loadMapTiles(
        MapManager.zoom,
        MapManager.curTileX,
        MapManager.curTileY,
        (MapManager.colCount - 1) / 2,
        (MapManager.rowCount - 1) / 2,
        function(zoom, x, y, sprite)
            local key = MapManager.zoom .. "_" .. x .. "_" .. y
            if this.mapTileObjs[key] ~= nil then
                local image = this.mapTileObjs[key].image
                --image.transform.parent.name = key
                image.sprite = sprite
            end
        end
    )
end
--双击放大并移动到点击的位置
function this.onDoubleClick(data)
    -- local localPos =
    --     UIExEventTool.ScreenToUIRootLocal(data.pointerCurrentRaycast.gameObject, data.position, data.pressEventCamera)
    -- if localPos and localPos.x ~= 99999 and localPos.y ~= 99999 then
    --     MapManager.move(localPos.x, localPos.y)
    -- end
    MapManager.changeCurZoom(MapManager.zoom + 1)
end
--拖拽
function this.onDrag(data)
    if not IS_UNITY_EDITOR and Input.touchCount ~= 1 then
        return
    end
    --printDebug("onDrag:"..data.delta.x.."_"..data.delta.y)
    MapManager.move(-data.delta.x, -data.delta.y)
    if Main.is3DMap then
        Map3DScene:updateTileMove()
        Map3DScene:updateGroup()
    else
        this:updateTileMove()
        this:updateGroup()
    end
end

function this:updateTileMove()
    --先移除出范围的，更新在范围内的
    for _, v in pairs(this.mapTileObjs) do
        local tileTrans = v.trans
        local itemTileX = v.x
        local itemTileY = v.y
        if
            itemTileX - MapManager.curTileX > math.floor((MapManager.colCount - 1) / 2) or
                MapManager.curTileX - itemTileX > math.floor((MapManager.colCount - 1) / 2) or
                itemTileY - MapManager.curTileY > math.floor((MapManager.rowCount - 1) / 2) or
                MapManager.curTileY - itemTileY > math.floor((MapManager.rowCount - 1) / 2)
         then
            local key = v.zoom .. "_" .. itemTileX .. "_" .. itemTileY
            this.mapTileObjs[key] = nil
            Queue.enqueue(this.noUsedTileQueue, tileTrans)
            tileTrans:GetComponent(typeof(Image)).sprite = this.defaultSprite
            tileTrans.gameObject:SetActive(false)
        else
            local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize
            local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize
            if not MapManager.isBaiduMap then
                localPosY = -localPosY
            end
            tileTrans.localPosition = Vector3(localPosX, localPosY, 0)
        end
    end
    --再创建需要增加的
    for i = 1, MapManager.rowCount do
        for j = 1, MapManager.colCount do
            local itemTileX = j - 1 - math.floor((MapManager.colCount - 1) / 2) + MapManager.curTileX
            local itemTileY = -i + 1 + math.floor((MapManager.rowCount - 1) / 2) + MapManager.curTileY
            local key = MapManager.zoom .. "_" .. itemTileX .. "_" .. itemTileY
            if this.mapTileObjs[key] == nil then
                local tile
                local tileTrans
                if this.noUsedTileQueue.count > 0 then
                    tileTrans = Queue.dequeue(this.noUsedTileQueue)
                    tile = tileTrans.gameObject
                else
                    tile = GameObject.Instantiate(this.tpl)
                    tileTrans = tile.transform
                    tileTrans:SetParent(this.cellGroupTrans)
                end
                tile:SetActive(true)
                local image = tileTrans:GetComponent(typeof(Image))
                local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize
                local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize
                if not MapManager.isBaiduMap then
                    localPosY = -localPosY
                end
                tile.name = key
                tileTrans.localPosition = Vector3(localPosX, localPosY, 0)
                tileTrans.localScale = Vector3(1, 1, 1)

                this.mapTileObjs[key] = {}
                this.mapTileObjs[key].trans = tileTrans
                this.mapTileObjs[key].image = image
                this.mapTileObjs[key].zoom = MapManager.zoom
                this.mapTileObjs[key].x = itemTileX
                this.mapTileObjs[key].y = itemTileY
                --printDebug("初始化地图瓦片"..tile.name.."_"..tileTrans.name.."_"..itemTileX.."_"..itemTileY)

                self:getMapTileSprite(MapManager.zoom, itemTileX, itemTileY)
            end
        end
    end
end

local zoomDistance = 3
local function isEnlarge(oP1, oP2, nP1, nP2)
    --计算两个触摸点的距离，进行对比， 第一个距离比第二个距离大则是缩小，反之为放大
    local oldDis = math.sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y))
    local newDis = math.sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y))
    if newDis - oldDis > zoomDistance then
        return 1
    elseif oldDis - newDis > zoomDistance then
        return -1
    end
    return 0
end

local oldPosition1 = Vector2.zero
local oldPosition2 = Vector2.zero
local m_isChangeScale = false
--输入处理
function this.onInput()
    if IS_UNITY_EDITOR then
        --滚轮缩放
        local wheelInput = Input.GetAxis("Mouse ScrollWheel")
        if wheelInput > 0 then
            this:mapAdd()
            NoticeManager.Instance:Dispatch(NoticeType.Map_Change_Scale_End)
        elseif wheelInput < 0 then
            this:mapSub()
            NoticeManager.Instance:Dispatch(NoticeType.Map_Change_Scale_End)
        end
    else
        --判断触摸数量为多点触摸
        if Input.touchCount == 2 then
            --获取当前两点触摸点的位置
            local tempPosition1 = Input.GetTouch(0).position
            local tempPosition2 = Input.GetTouch(1).position
            --前两只手指触摸类型都为移动触摸
            if Input.GetTouch(0).phase == TouchPhase.Moved or Input.GetTouch(1).phase == TouchPhase.Moved then
                local enlarge = isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2)
                if enlarge > 0 then
                    m_isChangeScale = true
                    this:mapAdd()
                elseif enlarge < 0 then
                    m_isChangeScale = true
                    this:mapSub()
                end
            end
            --备份上一次触摸点的位置，用于对比
            oldPosition1 = tempPosition1
            oldPosition2 = tempPosition2
        else
            oldPosition1 = Vector2.zero
            oldPosition2 = Vector2.zero
            if m_isChangeScale then
                m_isChangeScale = false
                NoticeManager.Instance:Dispatch(NoticeType.Map_Change_Scale_End)
            end
        end
    end
end

--改变缩放
function this:changeZoom()
    --先移除所有瓦片
    for _, v in pairs(this.mapTileObjs) do
        Queue.enqueue(this.noUsedTileQueue, v.trans)
        v.trans:GetComponent(typeof(Image)).sprite = this.defaultSprite
        v.trans.gameObject:SetActive(false)
    end
    this.mapTileObjs = {}
    self:createTiles()
    self:updateGroup()
end

--放大地图
function this:mapAdd()
    if MapManager.zoom >= MapManager.maxZoom and MapManager.scale >= 2 then
        return
    end

    MapManager.scale = MapManager.scale * 1.1
    if MapManager.scale >= 2 then
        if MapManager.zoom >= MapManager.maxZoom then
            MapManager.scale = 2
        else
            MapManager.scale = MapManager.scale / 2
            MapManager.setCurLngLatZoom(MapManager.curLng, MapManager.curLat, MapManager.zoom + 1)
            self:changeZoom()
            return
        end
    end
    self:updateTile()
    self:updateGroup()
end

--缩小地图
function this:mapSub()
    if MapManager.zoom <= MapManager.minZoom and MapManager.scale <= 1 then
        return
    end
    MapManager.scale = MapManager.scale / 1.1
    if MapManager.scale < 1 then
        if MapManager.zoom <= MapManager.minZoom then
            MapManager.scale = 1
        else
            MapManager.scale = MapManager.scale * 2
            MapManager.setCurLngLatZoom(MapManager.curLng, MapManager.curLat, MapManager.zoom - 1)
            self:changeZoom()
            return
        end
    end
    self:updateTile()
    self:updateGroup()
end

--回到我的位置
function this:mapBackMyPos()
    self:setCurLngLatZoom(MapManager.userLng, MapManager.userLat, MapManager.defaultZoom)
    --调用一下刷新
    NoticeManager.Instance:Dispatch(NoticeType.Map_Change_Scale_End)
end

--设置当前地图位置信息
function this:setCurLngLatZoom(lng, lat, zoom)
    MapManager.scale = MapManager.defaultScale
    if MapManager.zoom == MapManager.defaultZoom then
        MapManager.setCurLngLatZoom(lng, lat, zoom)
        this:updateTileMove()
        this:updateGroup()
    else
        MapManager.setCurLngLatZoom(lng, lat, zoom)
        self:changeZoom()
    end
end
