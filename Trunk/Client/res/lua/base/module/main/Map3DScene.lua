Map3DScene = BaseScene:new()
local this = Map3DScene

this.sceneName = "Map3D"
this.camera = nil --SceneCamera
this.container = nil --SceneContainer
this.tilesTrans = nil
this.mapTileObjs = {} --正在显示的地图瓦片对象
this.tpl = nil
this.defaultSprite = nil
this.noUsedTileQueue = nil

local cameraZ = -18
local cameraY = 23.04
local m_isEnter = false

function this:enterScene()
    if m_isEnter then
        this.camera.gameObject:SetActive(true)
        return
    end
    this:enter()
    m_isEnter = true
end

function this:exitScene()
    if not m_isEnter then
        return
    end
    this.camera.gameObject:SetActive(false)
end

--override 进入场景回调
function this:onEnter()
    this.camera = self:getCamera("camera")
    this.camera.transform.localPosition = Vector3(0, cameraY, cameraZ)
    this.container = self:getContainer("map3d")
    local containerTrans = this.container.transform
    local map3DTrans = containerTrans:GetChild(0)
    this.tilesTrans = map3DTrans:Find("tiles")
    this.tpl = map3DTrans:Find("tileItem").gameObject

    this.defaultSprite = this.tpl:GetComponent(typeof(MeshRenderer)).material.mainTexture
    this.tpl:SetActive(false)
    this.noUsedTileQueue = Queue:new()

    GlobalTimeManager.Instance.timerController:AddTimer("Input", -1, -1, this.onInput)

    NoticeManager.Instance:AddNoticeLister(NoticeType.Login_LoginGatewaySucceed, this.onLogin)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Add, this.onMapAdd)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Sub, this.onMapSub)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Back_My_Pos, this.onMapBackMyPos)

    self:getLocation()
end

function this:onReset()
    --Loger.PrintError(self.sceneName," 抽象方法需重写：onReset")
end

--override 离开场景回调
function this:onLeave()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("Input")

    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Login_LoginGatewaySucceed, this.onLogin)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Add, this.onMapAdd)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Sub, this.onMapSub)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Back_My_Pos, this.onMapBackMyPos)
end

function this.onCellCreated(cellGameObject, param)
    --Loger.PrintError(self.sceneName," 创建cell侦听方法要重写：onCellCreated")
end

function this.onLogin()
    this:getLocation()
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

--获取位置
function this:getLocation()
    if IS_UNITY_EDITOR then
        --编辑器直接用默认位置
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
                if string.byte(locationInfo) == 49 then
                    local strs = string.split(locationInfo, "|")
                    local lng = tonumber(strs[2])
                    local lat = tonumber(strs[3])
                    self:setUserLngLat(lng, lat)
                end
            end
        )
    end
end

local isReqNearInfo = false
--设置用户经纬度
function this:setUserLngLat(lng, lat)
    MapManager.userLng = lng
    MapManager.userLat = lat

    --首次设置用户经纬度时初始化地图瓦片
    if not this.isInitMapPos then
        this.isInitMapPos = true
        MapManager.setCurLngLatZoom(lng, lat, MapManager.zoom)

        self:createTiles()
        self:updateGroup()
    end

    self:getMapTileSprites()
    if not isReqNearInfo or lng - MapManager.userReqLng > 0.002 or lat - MapManager.userReqLat > 0.002 then
        if LoginDataProxy.isGetUserInfo then
            isReqNearInfo = true
            MapManager.userReqLng = lng
            MapManager.userReqLat = lat
            --MainModule.sendReqNearInfo(MapManager.userLng, MapManager.userLat)
        end
    end
end

--创建瓦片
function this:createTiles()
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
                tileTrans:SetParent(this.tilesTrans)
            end
            tile:SetActive(true)
            local mat = tileTrans:GetComponent(typeof(MeshRenderer)).material
            --local image = tileTrans:GetComponent(typeof(Image))
            local itemTileX = j - 1 - math.floor((MapManager.colCount - 1) / 2) + MapManager.curTileX
            local itemTileY = -i + 1 + math.floor((MapManager.rowCount - 1) / 2) + MapManager.curTileY
            local key = MapManager.zoom .. "_" .. itemTileX .. "_" .. itemTileY
            local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize3D
            local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize3D
            if not MapManager.isBaiduMap then
                localPosY = -localPosY
            end
            tile.name = key
            tileTrans.localPosition = Vector3(localPosX, 0, localPosY)
            tileTrans.localScale = Vector3(1, 1, 1)

            this.mapTileObjs[key] = {}
            this.mapTileObjs[key].trans = tileTrans
            this.mapTileObjs[key].mat = mat
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
        local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize3D
        local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize3D
        if not MapManager.isBaiduMap then
            localPosY = -localPosY
        end
        tileTrans.localPosition = Vector3(localPosX, 0, localPosY)
    end
end

--更新瓦片容器位置
function this:updateGroup()
    local localPosX = (MapManager.tileSize3D / 2 - MapManager.curPixelX * 10 / 256) * MapManager.scale
    local localPosY = (MapManager.tileSize3D / 2 - MapManager.curPixelY * 10 / 256) * MapManager.scale
    if MapManager.isBaiduMap then
        localPosY = (MapManager.tileSize3D / 2 - MapManager.curPixelY * 10 / 256) * MapManager.scale
    else
        localPosY = (MapManager.tileSize3D / 2 - 10 + MapManager.curPixelY * 10 / 256) * MapManager.scale
    end
    self.tilesTrans.localPosition = Vector3(localPosX, 0, localPosY)
    self.tilesTrans.localScale = Vector3(MapManager.scale, 1, MapManager.scale)
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
                local mat = this.mapTileObjs[key].mat
                mat.mainTexture = sprite.texture
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
                local mat = this.mapTileObjs[key].mat
                mat.mainTexture = sprite.texture
            end
        end
    )
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
            tileTrans:GetComponent(typeof(MeshRenderer)).material.mainTexture = this.defaultSprite
            tileTrans.gameObject:SetActive(false)
        else
            local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize3D
            local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize3D
            if not MapManager.isBaiduMap then
                localPosY = -localPosY
            end
            tileTrans.localPosition = Vector3(localPosX, 0, localPosY)
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
                local mat = tileTrans:GetComponent(typeof(MeshRenderer)).material
                local localPosX = (itemTileX - MapManager.curTileX) * MapManager.tileSize3D
                local localPosY = (itemTileY - MapManager.curTileY) * MapManager.tileSize3D
                if not MapManager.isBaiduMap then
                    localPosY = -localPosY
                end
                tile.name = key
                tileTrans.localPosition = Vector3(localPosX, 0, localPosY)
                tileTrans.localScale = Vector3(1, 1, 1)

                this.mapTileObjs[key] = {}
                this.mapTileObjs[key].trans = tileTrans
                this.mapTileObjs[key].mat = mat
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
--输入处理
function this.onInput()
    if IS_UNITY_EDITOR then
        --滚轮缩放
        local wheelInput = Input.GetAxis("Mouse ScrollWheel")
        if wheelInput > 0 then
            this:mapAdd()
        elseif wheelInput < 0 then
            this:mapSub()
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
                    this:mapAdd()
                elseif enlarge < 0 then
                    this:mapSub()
                end
            end
            --备份上一次触摸点的位置，用于对比
            oldPosition1 = tempPosition1
            oldPosition2 = tempPosition2
        else
            oldPosition1 = Vector2.zero
            oldPosition2 = Vector2.zero
        end
    end
end

--改变缩放
function this:changeZoom()
    --先移除所有瓦片
    for _, v in pairs(this.mapTileObjs) do
        Queue.enqueue(this.noUsedTileQueue, v.trans)
        v.trans:GetComponent(typeof(MeshRenderer)).material.mainTexture = this.defaultSprite
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
    MapManager.scale = MapManager.defaultScale
    if MapManager.zoom == MapManager.defaultZoom then
        MapManager.setCurLngLatZoom(MapManager.userLng, MapManager.userLat, MapManager.defaultZoom)
        this:updateTileMove()
        this:updateGroup()
    else
        MapManager.setCurLngLatZoom(MapManager.userLng, MapManager.userLat, MapManager.defaultZoom)
        self:changeZoom()
    end
end
