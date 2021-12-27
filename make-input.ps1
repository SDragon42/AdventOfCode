param
(
    # [Parameter(Mandatory=$true)]
    [int] $year,
    # [Parameter(Mandatory=$true)]
    [int] $day
)
$errorMessage = ""
$minYear = 2015
$currYear = (Get-Date).year

# Validate parameters
if ($year -lt 2015 || $year -gt $currYear)
{
    $errorMessage += "Invalid year ($year)! Must be a year between $minYear and $currYear.`r`n"
}

if ($day -lt 1 || $year -gt 25)
{
    $errorMessage += "Invalid day ($day)! Must be a day between 0 and 25.`r`n"
}

if ($errorMessage.Length -gt 0)
{
    Write-Host "ERRORS:"
    Write-Host $errorMessage
    Exit 0
}


if ($args.Length -gt 0)
{
    $filenameParts = $args
}
else
{
    $filenameParts = @("input", "example1", "example2")
}

# Create the input file path
$path = "$year\Input\day{0:00}" -f $day
New-Item -ItemType Directory -Force -Path $path | Out-Null

# Create the input files
function CreateInputFile
{
    param ([string] $filename)

    $filename = "$path\$filename.txt"
    
    $fileExists = Test-Path $filename
    if (-not $fileExists)
    {
        New-Item -ItemType File -Path $filename | Out-Null
    }
}

foreach ($name in $filenameParts)
{
    CreateInputFile("$name")
    CreateInputFile("$name-answer1")
    CreateInputFile("$name-answer2")
}

# Show folder path contents
Get-ChildItem $path 
