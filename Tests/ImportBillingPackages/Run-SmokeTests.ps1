[CmdletBinding()]
param(
    [string]$WebBin = (Join-Path $PSScriptRoot '..\..\LexEnaTrs.Web\bin')
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..\..')).Path
$webBinPath = (Resolve-Path -LiteralPath $WebBin).Path
$webAssembly = Join-Path $webBinPath 'LexEnaTrs.Web.dll'
if (-not (Test-Path -LiteralPath $webAssembly)) {
    throw 'Najpierw skompiluj LexEnaTrs.sln w Visual Studio 2015.'
}

$frameworkPath = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319'
$compiler = Join-Path $frameworkPath 'csc.exe'
$testTempRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
$testOutputDirectory = Join-Path $testTempRoot ('LexEnaBatchSmoke-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testOutputDirectory | Out-Null

try {
    $executable = Join-Path $testOutputDirectory 'BillingImportPackages.SmokeTests.exe'
    $compilerArgs = @(
        '/nologo', '/target:exe', '/platform:anycpu', '/utf8output',
        ('/out:' + $executable),
        ('/reference:' + $webAssembly),
        ('/reference:' + (Join-Path $webBinPath 'WienaDB.dll')),
        '/reference:System.dll', '/reference:System.Core.dll',
        '/reference:System.Data.dll', '/reference:System.Data.Entity.dll',
        '/reference:System.Xml.dll', '/reference:System.Runtime.Serialization.dll',
        (Join-Path $PSScriptRoot 'SmokeTests.cs')
    )
    & $compiler @compilerArgs
    if ($LASTEXITCODE -ne 0) { throw 'Nie udalo sie skompilowac smoke testow.' }

    # Static helpers and offline EDMX-to-SQL translation only; no service instance or SQL connection is opened.
    & $executable $webBinPath $repositoryRoot
    if ($LASTEXITCODE -ne 0) { throw 'Smoke testy importu paczek nie powiodly sie.' }

    # The allocator is shared with Silverlight; compile that exact source without a database stack.
    $distributionExecutable = Join-Path $testOutputDirectory 'BillingDistribution.SmokeTests.exe'
    $distributionArgs = @(
        '/nologo', '/target:exe', '/platform:anycpu', '/utf8output',
        ('/out:' + $distributionExecutable),
        '/reference:System.dll', '/reference:System.Core.dll', '/reference:System.Xml.dll',
        (Join-Path $repositoryRoot 'LexEnaTrs.Web\Models\BillingDistribution.cs'),
        (Join-Path $PSScriptRoot 'DistributionSmokeTests.cs')
    )
    & $compiler @distributionArgs
    if ($LASTEXITCODE -ne 0) { throw 'Nie udalo sie skompilowac testow podzialu.' }
    & $distributionExecutable
    if ($LASTEXITCODE -ne 0) { throw 'Testy podzialu paczki nie powiodly sie.' }
}
finally {
    $resolvedOutput = [System.IO.Path]::GetFullPath($testOutputDirectory)
    if (-not $resolvedOutput.StartsWith($testTempRoot, [StringComparison]::OrdinalIgnoreCase) -or
        [System.IO.Path]::GetFileName($resolvedOutput) -notlike 'LexEnaBatchSmoke-*') {
        throw 'Nieoczekiwana sciezka usuwania plikow testowych.'
    }
    Remove-Item -LiteralPath $resolvedOutput -Recurse -Force
}
