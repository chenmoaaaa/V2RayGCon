local Utils = require('lua.libs.utils')
local cev = require('lua.modules.coreEvent').new()

function GetName(uid)
    local coreServ = Utils.GetFirstServerWithUid(uid)
    if coreServ == nil then
        return
    end
    local coreState = coreServ:GetCoreStates()
    return "[" .. coreState:GetName() .. "]"
end

function OnGlobalStart(uid) print("服务器启动：", GetName(uid), " ", uid) end
function OnGlobalStop(uid) print("服务器停止：", GetName(uid), " ", uid) end
    
cev:RegGlobalEvStart(OnGlobalStart)
cev:RegGlobalEvStop(OnGlobalStop)

print("等待服务器启动、停止事件...")
while not Signal:Stop() do
   cev:Wait(1000)
end