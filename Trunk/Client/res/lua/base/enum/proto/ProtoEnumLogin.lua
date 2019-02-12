--协议工具生成的代码，不要手动修改
ProtoEnumLogin = {}
ProtoEnumLogin.MsgIdx =
{
	-- 请求登录
	MsgIdxReqLogin = 10001,
	-- 请求登录返回
	MsgIdxRspLogin = 10002,
	-- 请求短信验证码
	MsgIdxReqSMSAuth = 10003,
	-- 请求短信验证码返回
	MsgIdxRspSMSAuth = 10004,
	-- 请求注册
	MsgIdxReqRegister = 10005,
	-- 请求注册返回
	MsgIdxRspRegister = 10006,
	-- 通过手机号查找用户
	MsgIdxReqFindUserByTelNo = 10007,
	-- 通过手机号查找用户返回
	MsgIdxRspFindUserByTelNo = 10008,
	-- 请求重置密码
	MsgIdxReqResetPwd = 10009,
	-- 请求重置密码返回
	MsgIdxRspResetPwd = 10010,
	-- 通知登录信息
	MsgIdxNotifyLoginInfo = 10012,
	-- 请求支付宝签名
	MsgIdxReqAliPaySign = 10015,
	-- 请求支付宝签名返回
	MsgIdxRspAliPaySign = 10016,
	-- 请求记录设备信息
	MsgIdxReqDeviceLogInfo = 10017,
}
ProtoEnumLogin.ConstDef =
{
	-- 设备id最大长度
	MaxDeviceIDLength = "MaxDeviceIDLength",
	-- 微信授权登陆码最大长度
	MaxWeChatAuthCodeLength = "MaxWeChatAuthCodeLength",
	-- 账号最大长度
	MaxAccountLength = "MaxAccountLength",
	-- 手机号码长度
	PhoneNumberLength = "PhoneNumberLength",
}
ProtoEnumLogin.LoginResult =
{
	-- 连接成功
	LoginResultSuccess = "LoginResultSuccess",
	-- 内部错误
	LoginResultErrInternal = "LoginResultErrInternal",
	-- 该帐号已经登录
	LoginResultErrAlready = "LoginResultErrAlready",
	-- 无效的授权码
	LoginResultErrInvalidAuthCode = "LoginResultErrInvalidAuthCode",
	-- 第三方授权错误
	LoginResultErrAuthFailed = "LoginResultErrAuthFailed",
	-- 没有保存RefreshToken或已失效(此时前端要重新登录)
	LoginResultErrTokenExpired = "LoginResultErrTokenExpired",
	-- 无效的账号
	LoginResultErrInvalidAccount = "LoginResultErrInvalidAccount",
	-- 无效的手机号码
	LoginResultErrInvalidPhoneNumber = "LoginResultErrInvalidPhoneNumber",
	-- 无效的密码
	LoginResultErrInvalidPwd = "LoginResultErrInvalidPwd",
	-- 无效的Token
	LoginResultInvalidToken = "LoginResultInvalidToken",
}
ProtoEnumLogin.SMSAuthResult =
{
	-- 发送成功
	SMSAuthResultSuccess = "SMSAuthResultSuccess",
	-- 发送失败
	SMSAuthResultErr = "SMSAuthResultErr",
	-- 无效的手机号码
	SMSAuthResultErrInvalidNumber = "SMSAuthResultErrInvalidNumber",
	-- 手机号码已经存在
	SMSAuthResultErrExist = "SMSAuthResultErrExist",
	-- 此手机还未注册
	SMSAuthResultErrNotExist = "SMSAuthResultErrNotExist",
	-- 稍后再发送验证码
	SMSAuthResultErrNotTime = "SMSAuthResultErrNotTime",
}
ProtoEnumLogin.ReqRegisterResult =
{
	-- 成功
	ReqRegisterResultSuccess = "ReqRegisterResultSuccess",
	-- 失败
	ReqRegisterResultFail = "ReqRegisterResultFail",
	-- 电话号码已经注册
	ReqRegisterResultExisted = "ReqRegisterResultExisted",
	-- 无效的验证码
	ReqRegisterResultInvalidVerfyCode = "ReqRegisterResultInvalidVerfyCode",
}
ProtoEnumLogin.ReqResetPwdResult =
{
	-- 成功
	ReqResetPwdResultSuccess = "ReqResetPwdResultSuccess",
	-- 失败
	ReqResetPwdResultFail = "ReqResetPwdResultFail",
	-- 无效的验证码
	ReqResetPwdResultInvalidVerfyCode = "ReqResetPwdResultInvalidVerfyCode",
	-- 无效的账号
	ReqResetPwdResultInvalidAccount = "ReqResetPwdResultInvalidAccount",
	-- 密码不能小于4位
	ReqResetPwdResultPwdTooShort = "ReqResetPwdResultPwdTooShort",
	-- 密码不能大于32位
	ReqResetPwdResultPwdTooLong = "ReqResetPwdResultPwdTooLong",
}
