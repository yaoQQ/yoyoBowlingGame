MapManager = {}

LocationError = {
    ERROR_NONE = 0, --没有错误
    ERROR_NOT_ENABLED = 1, --GPS未启用
    ERROR_TIMEOUT = 2, --请求超时
    ERROR_FAILED = 3 --请求失败
}

MapManager.tileSize = 256 --瓦片尺寸
MapManager.tileSize3D = 10 --瓦片尺寸
MapManager.rowCount = 7 --行数
MapManager.colCount = 7 --列数
--MapManager.defaultLng = 113.3735	--琶洲展馆经度
--MapManager.defaultLat = 22.9665		--琶洲展馆纬度
MapManager.defaultLng = 113.356334 --华景软件园经度
MapManager.defaultLat = 23.135237 --华景软件园纬度
MapManager.defaultZoom = 15
MapManager.minZoom = 5
MapManager.maxZoom = 17

--用户的位置
MapManager.userLng = 0 --经度
MapManager.userLat = 0 --纬度

--用户其他位置信息
MapManager.userCityCode = "" --城市编码
MapManager.userCountry = "" --国家名
MapManager.userProvince = "" --省名
MapManager.userCity = "" --城市名
MapManager.userDistrict = "" --城区名

--用户上次请求消息位置
MapManager.userReqLng = 0 --经度
MapManager.userReqLat = 0 --纬度

--当前地图显示中心的位置
MapManager.curLng = 0 --经度
MapManager.curLat = 0 --纬度
MapManager.curTileX = 0 --瓦片X坐标
MapManager.curTileY = 0 --瓦片Y坐标
MapManager.curPixelX = 0 --坐标X偏移
MapManager.curPixelY = 0 --坐标Y偏移

MapManager.zoom = 15 --可支持的缩放范围：3~19
MapManager.defaultScale = 1.4641
MapManager.scale = 1.4641

MapManager.isBaiduMap = false

function MapManager.init()
end

--设置当前地图位置信息
function MapManager.setCurLngLatZoom(lng, lat, zoom)
    MapManager.curLng = lng
    MapManager.curLat = lat
    MapManager.zoom = zoom
    printDebug("lng" .. "_" .. lng)
    printDebug("lat" .. "_" .. lat)

    local tileX,
        tileY,
        pixelX,
        pixelY = MapManager.LatLngToTileXY(MapManager.curLng, MapManager.curLat, MapManager.zoom)
    MapManager.curTileX = tileX
    MapManager.curTileY = tileY
    MapManager.curPixelX = pixelX
    MapManager.curPixelY = pixelY
    printDebug("tileX" .. "_" .. tileX)
    printDebug("tileY" .. "_" .. tileY)
    printDebug("pixelX" .. "_" .. pixelX)
    printDebug("pixelY" .. "_" .. pixelY)

    local lng2, lat2 = MapManager.TileXYToLatLng(tileX, tileY, MapManager.zoom, pixelX, pixelY)
    printDebug("lng2" .. "_" .. lng2)
    printDebug("lat2" .. "_" .. lat2)
end

function MapManager.move(x, y)
    if x == 0 and y == 0 then
        return
    end
    if not MapManager.isBaiduMap then
        y = -y
    end
    MapManager.curPixelX = MapManager.curPixelX + x
    if x > 0 then
        while MapManager.curPixelX >= MapManager.tileSize do
            MapManager.curPixelX = MapManager.curPixelX - MapManager.tileSize
            MapManager.curTileX = MapManager.curTileX + 1
        end
    elseif x < 0 then
        while MapManager.curPixelX < 0 do
            MapManager.curPixelX = MapManager.curPixelX + MapManager.tileSize
            MapManager.curTileX = MapManager.curTileX - 1
        end
    end
    MapManager.curPixelY = MapManager.curPixelY + y
    if y > 0 then
        while MapManager.curPixelY >= MapManager.tileSize do
            MapManager.curPixelY = MapManager.curPixelY - MapManager.tileSize
            MapManager.curTileY = MapManager.curTileY + 1
        end
    elseif y < 0 then
        while MapManager.curPixelY < 0 do
            MapManager.curPixelY = MapManager.curPixelY + MapManager.tileSize
            MapManager.curTileY = MapManager.curTileY - 1
        end
    end
    local lng,
        lat =
        MapManager.TileXYToLatLng(
        MapManager.curTileX,
        MapManager.curTileY,
        MapManager.zoom,
        MapManager.curPixelX,
        MapManager.curPixelY
    )
    MapManager.curLng = lng
    MapManager.curLat = lat
end

--将tile(瓦片)坐标系转换为LatLngt(地理)坐标系，pixelX，pixelY为图片偏移像素坐标
function MapManager.TileXYToLatLng(tileX, tileY, zoom, pixelX, pixelY)
    if MapManager.isBaiduMap then
        local R = 6378137
        local size = math.pow(2, 26 - zoom)
        local pixelXToTileAddition = pixelX / 256.0
        local lng = (tileX + pixelXToTileAddition) * 180 * size / math.pi / R
        local lat = CSMapManager.TileYToLat(tileY, zoom, pixelY)
        return lng, lat
    else
        local size = math.pow(2, zoom)
        local pixelXToTileAddition = pixelX / 256.0
        local lng = (tileX + pixelXToTileAddition) / size * 360.0 - 180.0
        local lat = CSMapManager.TileYToLat(tileY, zoom, pixelY)
        return lng, lat
    end
end

--将LatLngt地理坐标系转换为tile瓦片坐标系，pixelX，pixelY为图片偏移像素坐标
function MapManager.LatLngToTileXY(lng, lat, zoom)
    if MapManager.isBaiduMap then
        local R = 6378137
        local size = math.pow(2, 26 - zoom)
        local x = math.pi * lng * R / (180 * size)
        local tileX = math.floor(x)
        local pixelX = (x - tileX) * 256
        local tileY, pixelY = CSMapManager.LatToTileY(lat, zoom)
        return tileX, tileY, pixelX, pixelY
    else
        local size = math.pow(2, zoom)
        local x = ((lng + 180) / 360) * size
        local tileX = math.floor(x)
        local pixelX = (x - tileX) * 256
        local tileY, pixelY = CSMapManager.LatToTileY(lat, zoom)
        return tileX, tileY, pixelX, pixelY
    end
end

--计算2个坐标直接的距离
function MapManager.getDistance(lng1, lat1, lng2, lat2)
    return CSMapManager.GetDistance(lng1, lat1, lng2, lat2)
end

--获取位置
--回调locationInfo格式：
--成功：1|经度|纬度|城市编码|国家名|省名|城市名|城区名
--失败：错误描述
function MapManager.StartLocation(callback)
    printDebug("获取位置")
    CSPlatformSDK.StartLocation(
        function(locationInfo)
            --printDebug("获取位置成功："..locationInfo)
            callback(locationInfo)
        end
    )
end

--获取地图瓦片
function MapManager.loadMapTiles(zoom, x, y, rangeX, rangeY, callback)
    --printDebug("获取地图瓦片"..x.."_"..y)
    local url
    if MapManager.isBaiduMap then
        url = "http://online1.map.bdimg.com/onlinelabel/?qt=tile&x={0}&y={1}&z={2}"
    else
        url = "http://wprd01.is.autonavi.com/appmaptile?style=7&x={0}&y={1}&z={2}"
    end
    CSMapManager.GetMapTiles(
        url,
        zoom,
        x,
        y,
        rangeX,
        rangeY,
        function(_zoom, _x, _y, sprite)
            --printDebug("获取地图瓦片成功".._x.."_".._y)
            callback(_zoom, _x, _y, sprite)
        end
    )
end

--获取地图瓦片
function MapManager.loadMapTile(zoom, x, y, callback)
    --printDebug("获取地图瓦片"..x.."_"..y)
    local url
    if MapManager.isBaiduMap then
        url = "http://online1.map.bdimg.com/onlinelabel/?qt=tile&x={0}&y={1}&z={2}"
    else
        url = "http://wprd01.is.autonavi.com/appmaptile?style=7&x={0}&y={1}&z={2}"
    end
    CSMapManager.GetMapTile(
        url,
        zoom,
        x,
        y,
        function(_zoom, _x, _y, sprite)
            --printDebug("获取地图瓦片成功".._x.."_".._y)
            callback(_zoom, _x, _y, sprite)
        end
    )
end

--通过瓦片坐标获取相对于屏幕中心的屏幕坐标（传入的zoom必须和MapManager.zoom相等）
--如果没有对应缩放比例的瓦片坐标，需要先通过MapManager.LatLngToTileXY(lng, lat, zoom)获取
function MapManager.getScreenPosFromCenter(zoom, tileX, tileY, pixelX, pixelY)
    if zoom ~= MapManager.zoom then
        printError("缩放比例错误")
        return nil, nil
    end
    if Main.is3DMap then
        local x =
            ((tileX - MapManager.curTileX) * MapManager.tileSize3D + (pixelX - MapManager.curPixelX) * 10 / 256) *
            MapManager.scale
        local y =
            ((tileY - MapManager.curTileY) * MapManager.tileSize3D + (pixelY - MapManager.curPixelY) * 10 / 256) *
            MapManager.scale
        if not MapManager.isBaiduMap then
            y = -y
        end
        local pos = Map3DScene.camera.cam:WorldToScreenPoint(Vector3(x, 0, y))
        if MapManager.isBaiduMap then
            return math.floor((pos.x - Main.screenWidthHeight.x / 2) * 1080 / Main.screenWidthHeight.x), math.floor(
                (pos.y - Main.screenWidthHeight.y / 2) * 1080 / Main.screenWidthHeight.x
            )
        else
            return math.floor((pos.x - Main.screenWidthHeight.x / 2) * 1080 / Main.screenWidthHeight.x), math.floor(
                (pos.y - Main.screenWidthHeight.y / 2) * 1080 / Main.screenWidthHeight.x
            )
        end
    else
        local x =
            math.floor(
            ((tileX - MapManager.curTileX) * MapManager.tileSize + pixelX - MapManager.curPixelX) * MapManager.scale
        )
        local y =
            math.floor(
            ((tileY - MapManager.curTileY) * MapManager.tileSize + pixelY - MapManager.curPixelY) * MapManager.scale
        )
        if not MapManager.isBaiduMap then
            y = -y
        end
        return x, y
    end
end

function MapManager.getScale3D(zoom, tileX, tileY, pixelX, pixelY)
    if zoom ~= MapManager.zoom then
        printError("缩放比例错误")
        return nil, nil
    end

    local y =
        ((tileY - MapManager.curTileY) * MapManager.tileSize3D + (pixelY - MapManager.curPixelY) * 10 / 256) *
        MapManager.scale
    if not MapManager.isBaiduMap then
        y = -y
    end
    local scale = 1
    if y < -40 then
        scale = 3
    else
        scale = MapManager.scale * 40 / (y + 50)
        if scale > 3 then
            scale = 3
        end
        if scale < 0.1 then
            scale = 0.1
        end
    end
    return Vector3(scale, scale, 1)
end

--指定坐标打开地图（地图界面未打开时调用）
function MapManager.openMapViewByLngLat(lng, lat)
    local msg = {}
    msg.lng = lng
    msg.lat = lat
    ViewManager.open(UIViewEnum.MapView, msg)
end

--指定坐标跳转地图（地图界面已打开时调用）
function MapManager.changeCurLngLat(lng, lat)
    local mapPos = {}
    mapPos.lng = lng
    mapPos.lat = lat
    NoticeManager.Instance:Dispatch(NoticeType.Map_Change_Cur_Pos, mapPos)
end

--指定缩放比例跳转地图
--可支持的缩放范围：3~19
function MapManager.changeCurZoom(zoom, scale)
    zoom = zoom < MapManager.minZoom and MapManager.minZoom or zoom
    zoom = zoom > MapManager.maxZoom and MapManager.maxZoom or zoom
    MapManager.scale = MapManager.defaultScale
    MapManager.setCurLngLatZoom(MapManager.curLng, MapManager.curLat, zoom)
    NoticeManager.Instance:Dispatch(NoticeType.Map_Change_Cur_Zoom)
end

--打开外部高德地图App导航
--回调result格式：
--成功：1
--失败：0|错误描述
function MapManager.openGaodeMapApp(fromLng, fromLat, fromName, toLng, toLat, toName)
    CSPlatformSDK.OpenGaodeMapApp(
        fromLng,
        fromLat,
        fromName,
        toLng,
        toLat,
        toName,
        function(result)
            if string.byte(result) ~= 49 then
                showFloatTips("使用此功能需要先安装高德地图")
            end
        end
    )
end

--根据MapPos获取地图方位
--return EnumMapDirection
function MapManager.getDirectionByMapPos(mapPos)
    local screenPosX, screenPosY = mapPos:getScreenPosFromCenter()
    local screenHeight = 1080 * Main.screenWidthHeight.y / Main.screenWidthHeight.x
    if screenPosX > 1080 / 2 then
        if screenPosY > screenHeight / 2 then
            return EnumMapDirection.NorthEast
        elseif screenPosY < -screenHeight / 2 then
            return EnumMapDirection.SouthEast
        else
            return EnumMapDirection.East
        end
    elseif screenPosX < -1080 / 2 then
        if screenPosY > screenHeight / 2 then
            return EnumMapDirection.NorthWest
        elseif screenPosY < -screenHeight / 2 then
            return EnumMapDirection.SouthWest
        else
            return EnumMapDirection.West
        end
    else
        if screenPosY > screenHeight / 2 then
            return EnumMapDirection.North
        elseif screenPosY < -screenHeight / 2 then
            return EnumMapDirection.South
        else
            return EnumMapDirection.None
        end
    end
end
--获取当前屏幕距离
function MapManager.getCurScreenMapPos(pos)
    local scale = (MapManager.scale / MapManager.defaultScale) / (2 ^ (MapManager.maxZoom - MapManager.zoom))
    local distance = math.floor(pos / scale)
    return distance
end
