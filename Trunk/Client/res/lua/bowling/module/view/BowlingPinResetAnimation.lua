BowlingPinResetAnimation = {}
local this = BowlingPinResetAnimation

local clearPinHandAnimation=nil
local restPinAnimation =nil 

function BowlingPinResetAnimation.init(clearPinHand,restPin)
	printDebug("<color='blue'> BowlingPinResetAnimation.init() clearPinHand="..tostring(clearPinHand).."  restPin="..tostring(restPin))
	printDebug("<color='blue'> BowlingPinResetAnimation.init() typeof(Animation)="..tostring(typeof(Animation)).."  clearPinHand:GetComponent="..tostring(clearPinHand))
	clearPinHandAnimation = clearPinHand:GetComponent(typeof(Animation))
	restPinAnimation = restPin:GetComponent(typeof(Animation))
	clearPinHandAnimation:Stop()
	restPinAnimation:Stop()
	
	printDebug("<color='blue'> BowlingPinResetAnimation.init() clearPinHandAnimation="..tostring(clearPinHandAnimation).."  restPinAnimation="..tostring(restPinAnimation))
end

function BowlingPinResetAnimation.HideCleanHand()
	clearPinHandAnimation.gameObject:SetActive(false)
end
function BowlingPinResetAnimation.HideRestHand()
	restPinAnimation.gameObject:SetActive(false)
end
--拿瓶或放瓶动画
function BowlingPinResetAnimation.onSetPins(isPinUp)
	printDebug(" BowlingPinResetAnimation.onSetPins(isPinUp) isPinUp="..tostring(isPinUp))
	if restPinAnimation then
		restPinAnimation.gameObject:SetActive(true)
		restPinAnimation:Play()
	end
	return 0.5
end

--清瓶动画
function BowlingPinResetAnimation.onSweepPins()
	printDebug("BowlingPinResetAnimation.onSweepPins()")
	if clearPinHandAnimation then
		clearPinHandAnimation.gameObject:SetActive(true)
		clearPinHandAnimation:Play()
	end
	return 0.5
end


