PreloadManager={}
local this=PreloadManager

function this:startLoad( preloadOrder )
	CSPreloadManager.Instance:ExecuteOrder(preloadOrder)
end

function this:cleanPreload( preloadOrder )
	
end

