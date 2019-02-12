---
--- Created by lichongzhi.
--- DateTime: 2017/12/21
--- 设置定义类
EliminateConfig = {}
local this = EliminateConfig

local Vector3 = CS.UnityEngine.Vector3


-- 匹配类型
this.MatchType =
{
    None = 0,   --
    Line = 1,   -- 直线型
    Poly = 2,   -- 折线型
}
-- 寻找可能匹配模式
this.MayMatchMode =
{
    MatchList = 1, -- 匹配的点集
    MatchSwap = 2,  -- 交换的点集
    MatchListAndSwap = 3, -- 匹配点集 + 交换点集
}
this.ComboEvaNeedCount =
{
    First   = 3,
    Second  = 6,
    Third   = 10,
    Fourth  = 15,
    --test
    --First   = 1,
    --Second  = 3,
    --Third   = 5,
    --Fourth  = 7,
}

this.ComboEvaLevel =
{
    First   = 0,
    Second  = 1,
    Third   = 2,
    Fourth  = 3,
    Fifth   = 4,
}
this.OperateMode =
{
    Auto = 1, -- 自动匹配, 即填充匹配
    Hand = 2, -- 手动匹配, 即交换匹配
}
this.GameMode =
{
    TIMER = 1,
    STEP = 2
}

-- 激活特殊块方式
this.ActiveSpeMode =
{
    Normal = 1,
    Account = 2
}
-- 四方向
this.FourDirect =
{
    None        = 0,
    Up          = 1,
    Right       = 2,
    Down        = 3,
    Left        = 4,
}

this.Row = 7            -- 地图的行数
this.Column = 7         -- 地图的列数

-- 常量
this.SWAP_TIME = 0.12  -- 两个piece互换所用时间, 单位秒
this.TIMER_COUNT = 30          -- 限制时间
this.STEP_COUNT = 20          -- 限制步数, 已废弃
this.POOL_POS = Vector3(10000, 10000, 0)  --Image组件的SetParent会有GC, 因此把其设置到看不见的位置即可
this.TIP_TIMESPAN = 5               -- 提示间隔, 单位秒
this.SHUFFLE_TIME = 0.5            -- 洗牌地图时间
this.COMBO_COLD_TIME = 5               -- 默认的COMBO冷却时间
this.EACH_COLUMN_FILL_TIME = 0.2        -- 每列的填充总共时间
this.READY_TIME = 2                     -- 正式开始前的准备时间
this.SPE_MIN = 4                        -- 特殊化要求的最低消除个数
this.SPE_CROSS_MIN = 5                  -- 特殊块十字消要求的最低直线匹配个数
this.SPE_BOMB_MIN = 4                  -- 特殊块炸弹要求的最低匹配个数
this.SPE_SAME_HAND_MIN = 30             -- 特殊块单色消要求的最低数
this.SPE_SAME_HAND_MAX = 50              -- 特殊块单色消要求的最高数
--this.SPE_SAME_HAND_MIN = 10             -- 特殊块单色消要求的最低数
--this.SPE_SAME_HAND_MAX = 15              -- 特殊块单色消要求的最高数

this.SPE_WAIT_TIME = 0.1
this.SWAP_ROLE_TIME = 0.3               -- 交换角色时间
this.ROLE_COUNT = 4               -- 跳舞角色数量
this.START_TIMER_COUNT = 4      -- 开始倒计时
this.OVER_TIMER_COUNT = 5       -- 结束倒计时
this.ACCOUNT_FILL_WAIT = 0.3       -- 结算消除时填充前的等待时间
this.SCREEN_UP_Y = -7              -- 屏幕顶端对应的GridY值
this.SCORE_UNIT = 100              -- 每个块的基础分
-- 每个块的大小
this.pxPerUnit = 134
-- 每个块的大小(正方形)
this.tileUnit = 134
-- 初始地图配置, 用于调试
this.initMapConfig =
{
    -- 普通测试
    --{4}, {1}, {3}, {1}, {2}, {1}, {2},
    --{2}, {2}, {5}, {4}, {3}, {2}, {1},
    --{2}, {5}, {5}, {2}, {5}, {2}, {3},
    --{1}, {2}, {1}, {5}, {2,4}, {3}, {1},
    --{6}, {4}, {2}, {2}, {6}, {2}, {4},
    --{1}, {1}, {3}, {1}, {5}, {4}, {2},
    --{3}, {2}, {1}, {4}, {2}, {1}, {3},

    -- 有第一类特殊块的链式反应测试
    --{4}, {2}, {4}, {1}, {2}, {1}, {4},
    --{2}, {5}, {3}, {2}, {3}, {4}, {1},
    --{3}, {6}, {3}, {4}, {5}, {2}, {3},
    --{4}, {2}, {2}, {3}, {2}, {3}, {1},
    --{1}, {3}, {5}, {0, 4}, {6}, {2}, {4},
    --{2}, {1}, {0, 5}, {2}, {0,4}, {4}, {2},
    --{3}, {2}, {3}, {4}, {2}, {1}, {3},

    --{1}, {3}, {4}, {1}, {2}, {1}, {4},
    --{2}, {6}, {1}, {4}, {3}, {4}, {1},
    --{1}, {3}, {3}, {1}, {5}, {2}, {3,3},
    --{3}, {4}, {2}, {2}, {3}, {4}, {1},
    --{5}, {2}, {3}, {2}, {6,3}, {5}, {4},
    --{6}, {1}, {2,3}, {1}, {5}, {4}, {2},
    --{3}, {2}, {5}, {2}, {3}, {2}, {2},

    -- 结算测试
    {4}, {1}, {3}, {1}, {2,3}, {1}, {2},
    {2}, {2,5}, {5}, {4}, {3}, {2,5}, {1},
    {2}, {5}, {5,5}, {2,3}, {5}, {2}, {3},
    {1}, {2,5}, {1,3}, {5,3}, {2,5}, {3}, {1},
    {6,3}, {4}, {2}, {2}, {6}, {2}, {4,5},
    {1}, {1}, {3,3}, {1}, {5,4}, {4}, {2},
    {3}, {2}, {1}, {4}, {2}, {1}, {3},

-- 结算测试
--    {4}, {1}, {3}, {1}, {2}, {1}, {2},
--    {2}, {2}, {5}, {4}, {3}, {2}, {1},
--    {2}, {5}, {5}, {2}, {5}, {2}, {3},
--    {1}, {2}, {1}, {5}, {2}, {3}, {1},
--    {6}, {4}, {2,5}, {2}, {6}, {2}, {4},
--    {1}, {1}, {3,3}, {1}, {5}, {4}, {2},
--    {3}, {2}, {1}, {4}, {2}, {1}, {3},
}
-- 道具叠加类型
this.ItemCoverRule =
{
    Time            = 1,    -- 只叠加时间
    Effect          = 2,    -- 只叠加效果
    Independent     = 3,    -- 独立计算
    Refresh         = 4,    -- 刷新时间
}
-- 道具类型
this.ItemType =
{
    Craze           = 1,
    RandomEli       = 2,
    PointEli        = 3,
    ClearObstacle   = 4,
    Freeze          = 5,
    Fog             = 6,
    Shake           = 7,
    Grayed          = 8,
    Shuffle         = 9,
}

local m_colorList = nil
function this.GetColorList()
    if table.empty(m_colorList) then
        m_colorList = {}
        for _, v in pairs(this.ColorType) do
            if v ~= this.ColorType.NONE and v ~= this.ColorType.COUNT then
                table.insert(m_colorList, v)
            end
        end
    end
    return m_colorList
end

-- 颜色类型
this.ColorType =
{
    
    NONE        = 0,
    RED         = 1,
    BLACK       = 2,
    --BLUE        = 3,
    GREEN       = 4,
    YELLOW      = 5,
    PURPLE      = 6,
    WHITE       = 7,
    COUNT       = 8,
}
-- piece类型
this.PieceType =
{
    EMPTY          = 1,
    NORMAL         = 2,
    -- 第一类特殊块
    BOMB           = 3,
    -- 第二类特殊块
    CROSS_CLEAR    = 4,
    SAME_CLEAR     = 5,
    COUNT          = 6,
}

this.standardWidth = 1080
this.standardHeight = 1920
