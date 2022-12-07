param (
    [Parameter(Mandatory = $false)]
    [Alias('y')]
    [int] $year,

    [Parameter(Mandatory = $false)]
    [Alias('d')]
    [int] $day,
    
    [Parameter(Mandatory = $false)]
    [Alias('e')]
    [int] $numExamples
)

$errorMessage = ""
$minYear = 2015
$currYear = (Get-Date).year


############################################################
# Validate parameters
############################################################
if (!$year) {
    $year = $currYear
}

if ($year -lt 2015 || $year -gt $currYear)
{
    $errorMessage += "Invalid year ($year)! The year must be between $minYear and $currYear.`r`n"
}

if ($day) {
    if ($day -lt 1 -or $day -gt 25) {
        $errorMessage += "Invalid day ($day)! Must be a day between 1 and 25.`r`n"
    }
}

if (!$numExamples) {
    $numExamples = 1
}


if ($errorMessage.Length -gt 0)
{
    Write-Host "ERRORS:"
    Write-Host $errorMessage
    Exit 0
}


############################################################
# Build Filename list
############################################################
$filenameParts = @()
$filenameParts += "input"

for ($i = 1; $i -le $numExamples; $i++) {
    $filenameParts += "example$i"
}


############################################################
# Build Day range
############################################################
$dayStart = 1
$dayEnd = 25

if ($day) {
    $dayStart = $day
    $dayEnd = $day
}


############################################################
# Create input files
############################################################
function CreateInputFile {
    param (
        [string] $dayPath,
        [string] $filename
    )

    $filePath = "$dayPath\$filename.txt"
    
    $fileExists = Test-Path $filePath
    if (-not $fileExists)
    {
        New-Item -ItemType File -Path $filePath | Out-Null
    }
}


for ($i = $dayStart; $i -le $dayEnd; $i++) {
    $dayPath = "$year\Input\day{0:00}" -f $i

    New-Item -ItemType Directory -Force -Path $dayPath | Out-Null

    foreach ($name in $filenameParts) {
        CreateInputFile $dayPath "$name"
        CreateInputFile $dayPath "$name-answer1"
        CreateInputFile $dayPath "$name-answer2"
    }
}
