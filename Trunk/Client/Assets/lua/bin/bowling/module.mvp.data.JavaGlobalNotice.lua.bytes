

JavaDataProtocol =
{
	--登录请求
	sendInitScreen = "1",--描述：客户端通知服务器开始发坐标的命令，主要字段有pid（协议号），screen（屏幕尺寸）
	SendStopPos = "2",--描述：客户端通知服务器结束发坐标的命令，主要字段有pid（协议号）
	
	sendPositionContent = "3",--描述：服务器发送给客户端的坐标信息，主要字段有pid（协议号），content（内容），sequence（序列号，每发送一次就加1,表示顺序），其中content里面是人的信息列表，person_id表示人的id号，info里面是左手腕和右手腕的坐标值（x,y,z）
	sendChoosePerson = "4",--描述：客户端发送给服务器，确定选中的人的id，服务器收到后，后续发送协议3到客户端的时候只会发该人的信息
	sendHear = "5",--描述：客户端发送给服务器的心跳包，8s一次，超过8s没收到心跳包，服务器会主动断开连接，主要字段有：pid(协议号)，sequence（序列号，每发送一次加1,表示顺序）

	none="none"
}

