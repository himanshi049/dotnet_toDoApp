# Script to create test data for Task API
# Make sure your API is running at http://localhost:5000

Write-Host "Creating test users..." -ForegroundColor Green

# Create 3 users
$users = @()
for ($i = 1; $i -le 3; $i++) {
    $userBody = @{
        name = "User $i"
    } | ConvertTo-Json
    
    $user = Invoke-RestMethod -Uri "http://localhost:5000/api/user" -Method Post -Body $userBody -ContentType "application/json"
    $users += $user
    Write-Host "Created User $($user.id): $($user.name)"
}

Write-Host "`nCreating test tasks..." -ForegroundColor Green

# Create 20 tasks
$taskTitles = @(
    "Buy groceries", "Complete project", "Call dentist", "Read book", "Exercise",
    "Write report", "Fix bug", "Review code", "Send email", "Clean house",
    "Pay bills", "Schedule meeting", "Update resume", "Learn new skill", "Plan vacation",
    "Water plants", "Do laundry", "Cook dinner", "Study for exam", "Organize files"
)

for ($i = 0; $i -lt 20; $i++) {
    $userId = $users[$i % 3].id  # Distribute tasks among users
    $taskBody = @{
        title = $taskTitles[$i]
        description = "Description for task $($i+1)"
        userId = $userId
    } | ConvertTo-Json
    
    $task = Invoke-RestMethod -Uri "http://localhost:5000/api/tasks" -Method Post -Body $taskBody -ContentType "application/json"
    Write-Host "Created Task $($task.id): $($task.title) (User $userId)"
}

Write-Host "`nTest data created successfully!" -ForegroundColor Green
Write-Host "Total Users: 3" -ForegroundColor Cyan
Write-Host "Total Tasks: 20" -ForegroundColor Cyan
Write-Host "`nYou can now test pagination:" -ForegroundColor Yellow
Write-Host "  http://localhost:5000/api/tasks?page=1&pageSize=5"
Write-Host "  http://localhost:5000/api/tasks?page=2&pageSize=5"
Write-Host "  http://localhost:5000/api/tasks?userId=1"
