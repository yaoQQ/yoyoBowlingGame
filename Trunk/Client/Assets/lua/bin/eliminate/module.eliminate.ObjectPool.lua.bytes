---
--- Created by Lichongzhi.
--- DateTime: 2018\1\18 0018 17:49
--- 对象池

require "eliminate:table/TableObjectPool"

ObjectPool = {}
local this = ObjectPool

local ObjectPoolManager = CS.ObjectPoolManager

function ObjectPool:new(o)
    o = o or {}
    setmetatable(o,self)
    self.__index = self
    return o
end

function ObjectPool:GetInstance()
    if self._instance == nil then
        self._instance = self:new()
    end
    return self._instance
end

ResourceObject = {}
function ResourceObject:new(resource)
    local o = {}
    o.resource = resource
    o.pool = Queue.new()
    return o
end

this.m_initialized = false
this.m_containerObject = nil -- 对象池中的对象的父容器
this.m_resourceTable = nil  -- 键值目录
this.totalObjectList = nil

function this:getPoolCount()
    --local count = 0
    --for _, v in pairs(TableObjectPool) do
    --    count = count + v.defaultCount
    --end
    --print("count = "..count)
    return  #TableObjectPool
end

function this:initObjectPool()
    if self.m_initialized then
        return
    end
    self.m_resourceTable = {}
    self.totalObjectList = {}
    self.m_containerObject = GameObject("ObjectPool")
    self.m_containerObject.transform.position = Vector3(-10000, -10000, 0)

    -- 异步加载并实例化对象
    for _, v in pairs(TableObjectPool) do
        EffectManager.Instance:CreateEffect(v.projectName, v.keyName,  function (effectControler)
            local resource = effectControler.gameObject
            if (resource == nil) then
                Loger.PrintError("消消-ObjectPool加载错误")
            else
                NoticeManager.Instance:Dispatch(EliminateNoticeType.LoadStep)
                local ro = ResourceObject:new(resource)
                self.m_resourceTable[v.keyName] = ro
                for i = 1, v.defaultCount - 1 do
                    self:addToPool(ro, self:createObject(v.keyName, ro.resource))
                end
                -- 现在在C#实例化了, 返回的是GameObject而不是Object, 所以要把第一次返回的GameObject也加入池中
                resource.name = v.keyName;
                table.insert(self.totalObjectList, resource)
                self:addToPool(ro, resource)
            end
        end)
    end
    self.m_initialized = true
end

function this:addToPool(resourceObject, targetObject)
    local t = targetObject.transform;
    local localScale = t.localScale;

    t:SetParent(self.m_containerObject.transform);
    t.localScale = localScale;
    t.localPosition = Vector3.zero;
    targetObject:SetActive(false);
    Queue.enqueue(resourceObject.pool, targetObject)
end

function this:createObject(key, resource)
    local ret = GameObject.Instantiate(resource);
    ret.name = key;
    table.insert(self.totalObjectList, ret)
    return ret;
end

function this:getObject(key)
    local resourceObject = self.m_resourceTable[key];
    if (resourceObject == nil) then
        return nil;
    end
    if (resourceObject.pool.count > 0) then
        local ret = Queue.dequeue(resourceObject.pool)
        if (ret ~= nil) then
            ret.transform:SetParent(nil);
            ret:SetActive(true);
            return ret;
        end
    end
    return self:createObject(key, resourceObject.resource);
end

function this:poolObject(obj)
    local resourceObject = self.m_resourceTable[obj.name];
    if (resourceObject == nil) then
        return false;
    end
    self:addToPool(resourceObject, obj);
    return true;
end

function this:onDestroyObjectPool()
    if self.m_initialized == false then
        return
    end
    for _, v in pairs(self.totalObjectList) do
        GameObject.Destroy(v)
    end
    GameObject.Destroy(self.m_containerObject)
    self.m_initialized = false
    self.m_containerObject = nil
    self.m_resourceTable = nil
    self.totalObjectList = nil
end
