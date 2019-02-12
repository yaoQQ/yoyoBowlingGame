---
--- Created by Lichongzhi.
--- DateTime: 2018\11\1 0001 10:17
---

AnimalConfig = {}
local this = AnimalConfig


this.Row = 4            -- 地图的行数x(0~3)
this.Column = 4         -- 地图的列数y(0~3)

this.MOVE_TIME = 0.12  -- 两个piece互换所用时间, 单位秒

-- 回合状态
this.RoundState =
{
    Me      = 1,    -- 我
    Other   = 2,   -- 他
    Wait    = 3,  -- 等待中
}

-- 棋子状态
this.PieceState =
{
    NotOpen     = 0,
    Opened      = 1,
    Selected    = 2,
    Dead        = 4,
}
this.PieceCnName =
{
    [0] = "鼠",
    [1] = "猫",
    [2] = "狗",
    [3] = "狼",
    [4] = "豹",
    [5] = "虎",
    [6] = "狮",
    [7] = "象",
}
this.PieceEnName =
{
    [0] = "rat",
    [1] = "cat",
    [2] = "dog",
    [3] = "wolf",
    [4] = "pard",
    [5] = "tiger",
    [6] = "lion",
    [7] = "elephant",
}
this.GridDeltaX = 264
this.GridDeltaY = 282
this.FirstPos = {x = -388, y = 570}
this.GameWidth = 1080
this.GameHeight = 1920