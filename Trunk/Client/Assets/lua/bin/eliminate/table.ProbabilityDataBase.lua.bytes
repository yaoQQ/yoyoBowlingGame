-- FILE: 道具表.xlsx SHEET: Sheet2
ProbabilityDataBase = {
[1]={["ID"]=1,["score"]=2000,["probability"]=1.0},
[2]={["ID"]=2,["score"]=4000,["probability"]=1.0},
[3]={["ID"]=3,["score"]=8000,["probability"]=0.9},
[4]={["ID"]=4,["score"]=12000,["probability"]=0.8},
[5]={["ID"]=5,["score"]=18000,["probability"]=0.8},
[6]={["ID"]=6,["score"]=25000,["probability"]=0.7},
[7]={["ID"]=7,["score"]=35000,["probability"]=0.7},
[8]={["ID"]=8,["score"]=45000,["probability"]=0.6},
[9]={["ID"]=9,["score"]=55000,["probability"]=0.5},
[10]={["ID"]=10,["score"]=65000,["probability"]=0.4},
[11]={["ID"]=11,["score"]=75000,["probability"]=0.2},
}
setmetatable(ProbabilityDataBase, {__index = function(__t, __k) if __k == "query" then return function(index) return __t[index] end end end})
