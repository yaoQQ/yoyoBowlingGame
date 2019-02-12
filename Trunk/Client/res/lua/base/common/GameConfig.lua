GameConfig = {}

--开发服
GameConfig.loginIP1 = "login_dev.51e-sport.com"
GameConfig.loginPort1 = 2002

--测试服
GameConfig.loginIPTest = "login_test.51e-sport.com"
GameConfig.loginPortTest = 2002

--预发布服
GameConfig.loginIP2 = "login_beta.51e-sport.com"
GameConfig.loginPort2 = 2002

--正式服
GameConfig.loginIP3 = "login_ga.51e-sport.com"
GameConfig.loginPort3 = 2002

--正式服充值
GameConfig.billingIP = "billing_ga.51e-sport.com"

if IS_UNITY_EDITOR or IS_TEST_SERVER then
	GameConfig.loginIP = GameConfig.loginIP1
	GameConfig.loginPort = GameConfig.loginPort1
elseif IS_SUPER_VERSION then
	--安卓预发布暂时用测试服
	if IS_IOS then
		GameConfig.loginIP = GameConfig.loginIP2
		GameConfig.loginPort = GameConfig.loginPort2
	else
		GameConfig.loginIP = GameConfig.loginIPTest
		GameConfig.loginPort = GameConfig.loginPortTest
	end
else
	GameConfig.loginIP = GameConfig.loginIP3
	GameConfig.loginPort = GameConfig.loginPort3
end

--屈波
--ip:192.168.1.49
--port:2001
--port:2008

--刘炳学
--ip:192.168.1.211
--port:2001
--port:2008

GameConfig.ServerName =
{
	MainLogin = "MainLogin",
	MainGateway = "MainGateway",
	BusinessLogin = "BusinessLogin",
	BusinessGateway = "BusinessGateway",
}