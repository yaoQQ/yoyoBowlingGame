--协议工具生成的代码，不要手动修改
ProtoEnumAnimalChess = {}
ProtoEnumAnimalChess.MsgIdx =
{
	-- 请求移动棋子
	MsgIdxReqMoveChess = 40200,
	-- 请求翻转棋子
	MsgIdxReqReverseChess = 40201,
	-- 请求翻转棋子返回
	MsgIdxRspReverseChess = 40202,
	-- 请求投降
	MsgIdxReqSurrender = 40203,
	-- 请求投降返回
	MsgIdxRspSurrender = 40204,
	-- 游戏结束通知
	MsgIdxNotifyGameOver = 40205,
	-- 游戏吃棋子通知
	MsgIdxNotifyMoveChess = 40206,
	-- 游戏回合轮转通知
	MsgIdxNotifyRoundTurn = 40207,
	-- 游戏得分改变通知
	MsgIdxNotifyScoreChange = 40208,
}
ProtoEnumAnimalChess.MoveChess_Result =
{
	-- 移动成功
	MoveChess_Success = "MoveChess_Success",
	-- 移动失败
	MoveChess_Fail = "MoveChess_Fail",
	-- 吃棋子
	MoveChess_Eat = "MoveChess_Eat",
	-- 被吃
	MoveChess_BeEaten = "MoveChess_BeEaten",
	-- 同归于尽
	MoveChess_PerishTogether = "MoveChess_PerishTogether",
}
