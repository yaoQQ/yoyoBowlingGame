

Queue = {}
local this=Queue
this.count=0
function Queue.new ()
    return {first = 0, last = -1,count = 0, list = {}}
end
 
function Queue.enqueue (queue, value)
    local last = queue.last + 1
    queue.last = last
    --queue[last] = value

    queue.list[last] = value
    queue.count = queue.count+1
end
 
function Queue.dequeue (queue)
    local first = queue.first
    if first > queue.last then 
        --error("queue is empty")
        return nil
    end
    --local value = queue[first]
    local value = queue.list[first]
    --queue[first] = nil    -- to allow garbage collection
    queue.list[first] = nil    -- to allow garbage collection
    queue.first = first + 1
    queue.count = queue.count-1
    return value
end

-- peek 方法
function Queue.peek(queue)
    if queue == nil or queue.count == 0 then
        return nil
    end
    return queue.list[queue.first]
end


 
