FriendChatDataProxy = {}

FriendChatDataProxy.currDelFriendId = 0

FriendChatDataProxy.currChatFriendId = 0

FriendChatDataProxy.currReceiveMsgFriendId = 0

--这三个变量是用作自己聊天内容显示
FriendChatDataProxy.currChatMsgId = 0
FriendChatDataProxy.currChatMsg = nil
FriendChatDataProxy.currChatTime = 0
FriendChatDataProxy.currChatType = nil --ProtoEnumCommon.ChatMsgType.ChatMsgType_Text/...

FriendChatDataProxy.isGetRedBagFromChat = false
--是否是卡券红包
FriendChatDataProxy.isCoupon = false

-- FriendChatDataProxy.currFriendListIndex = -1