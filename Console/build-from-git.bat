@echo off
setlocal EnableExtensions EnableDelayedExpansion
title Git Auto Builder

REM ============================================================
REM  build-from-git.bat  <url-git> [branch] [pasta-de-saida]
REM  Clona (ou atualiza), detecta o tipo de projeto em ate 5
REM  niveis de pastas e compila em Release.
REM  Suporta: CMake, Meson, .sln/.vcxproj (MSBuild), .NET (csproj),
REM           Rust, Go, Gradle, Maven, Node, Python
REM  Pasta de saida (opcional), ex.: .\BIN ou .\APP. Vazia = padrao.
REM ============================================================

REM ---------- Opcoes (edite aqui) ----------
set "NATIVE=0"
set "FASTMATH=0"
set "WORKDIR=%CD%"
REM NATIVE=1 adiciona /arch:AVX2 | FASTMATH=1 adiciona /fp:fast
REM -----------------------------------------

set "URL=%~1"
set "BRANCH=%~2"
set "OUT=%~3"
if "%URL%"=="" set /p "URL=URL do repositorio Git: "
if "%URL%"=="" exit /b 1

where git >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Git nao encontrado no PATH.
    exit /b 1
)

REM ---------- Nome do repositorio ----------
set "T=%URL:/= %"
set "T=!T::= !"
set "NAME="
for %%A in (!T!) do set "NAME=%%A"
if /i "!NAME:~-4!"==".git" set "NAME=!NAME:~0,-4!"
echo [INFO] Repositorio: !NAME!

REM ---------- Clone / atualizacao (pode rodar varias vezes) ----------
set "BR="
if defined BRANCH set "BR=-b %BRANCH%"
if exist ".git" goto :here
if exist "%WORKDIR%\%NAME%\.git" goto :update
if exist "%WORKDIR%\%NAME%\" goto :cloned
git clone --recursive %BR% "%URL%" "%WORKDIR%\%NAME%"
if errorlevel 1 goto :fail
goto :cloned
:update
echo [INFO] Pasta ja existe, atualizando...
git -C "%WORKDIR%\%NAME%" pull --ff-only
:cloned
cd /d "%WORKDIR%\%NAME%"
goto :ready
:here
echo [INFO] Usando o repositorio da pasta atual...
git pull --ff-only
:ready
git submodule update --init --recursive

REM ---------- Pasta de saida (opcional) ----------
set "OUTROOT="
if defined OUT for %%I in ("%OUT%") do set "OUTROOT=%%~fI"
if defined OUTROOT if "%OUTROOT:~-1%"=="\" set "OUTROOT=%OUTROOT:~0,-1%"

REM ---------- Flags MSVC ----------
set "MSVC_C=/O2 /Ob2 /Oi /Ot /Gy /Qpar /DNDEBUG"
set "MSVC_CXX=%MSVC_C% /EHsc"
set "MSVC_LD=/LTCG /OPT:REF /OPT:ICF"
if "%NATIVE%"=="1" set "MSVC_C=%MSVC_C% /arch:AVX2"
if "%NATIVE%"=="1" set "MSVC_CXX=%MSVC_CXX% /arch:AVX2"
if "%FASTMATH%"=="1" set "MSVC_C=%MSVC_C% /fp:fast"
if "%FASTMATH%"=="1" set "MSVC_CXX=%MSVC_CXX% /fp:fast"

REM ---------- Deteccao (procura em ate N niveis de pastas) ----------
echo [INFO] Detectando tipo de projeto...
call :find "CMakeLists.txt" F_CMAKE 4
call :find "meson.build" F_MESON 3
call :find "*.sln" F_SLN 5
if not defined F_SLN call :find "*.vcxproj" F_SLN 5
call :find "*.csproj" F_CS 4
call :find "Cargo.toml" F_RUST 3
call :find "go.mod" F_GO 3
call :find "build.gradle" F_GRADLE 3
if not defined F_GRADLE call :find "build.gradle.kts" F_GRADLE 3
call :find "pom.xml" F_MVN 3
call :find "package.json" F_NODE 2
call :find "pyproject.toml" F_PY 2
if not defined F_PY call :find "setup.py" F_PY 2
if not defined F_PY call :find "requirements.txt" F_PY 2

if defined F_CMAKE  goto :t_cmake
if defined F_MESON  goto :t_meson
if defined F_SLN    goto :t_sln
if defined F_CS     goto :t_csproj
if defined F_RUST   goto :t_rust
if defined F_GO     goto :t_go
if defined F_GRADLE goto :t_gradle
if defined F_MVN    goto :t_maven
if defined F_NODE   goto :t_node
if defined F_PY     goto :t_python
echo [ERRO] Tipo de projeto nao reconhecido (busca em ate 5 niveis).
goto :fail

REM ============================================================
:t_cmake
echo [TIPO] CMake: %F_CMAKE%
call :msvc
for %%I in ("%F_CMAKE%") do set "SRC=%%~dpI."
set "VCPKG_ARGS="
if exist "vcpkg.json" call :vcpkg
set "OUTARG="
if defined OUTROOT set OUTARG=-DCMAKE_RUNTIME_OUTPUT_DIRECTORY="%OUTROOT%" -DCMAKE_LIBRARY_OUTPUT_DIRECTORY="%OUTROOT%"
set "GEN=-A x64"
where ninja >nul 2>&1 && set "GEN=-G Ninja"
cmake -S "%SRC%" -B build %GEN% -DCMAKE_BUILD_TYPE=Release -DCMAKE_INTERPROCEDURAL_OPTIMIZATION=ON -DCMAKE_C_FLAGS_RELEASE="%MSVC_C%" -DCMAKE_CXX_FLAGS_RELEASE="%MSVC_CXX%" -DCMAKE_EXE_LINKER_FLAGS_RELEASE="%MSVC_LD%" -DCMAKE_SHARED_LINKER_FLAGS_RELEASE="%MSVC_LD%" %VCPKG_ARGS% %OUTARG%
if errorlevel 1 goto :fail
cmake --build build --config Release --parallel
if errorlevel 1 goto :fail
set "SHOW=%CD%\build"
if defined OUTROOT set "SHOW=%OUTROOT%"
goto :done

REM ============================================================
:t_meson
echo [TIPO] Meson: %F_MESON%
call :msvc
for %%I in ("%F_MESON%") do set "SRC=%%~dpI."
set "MBD=build"
if defined OUTROOT set "MBD=%OUTROOT%"
meson setup "%MBD%" "%SRC%" --buildtype=release -Db_lto=true -Db_ndebug=true
if errorlevel 1 goto :fail
meson compile -C "%MBD%"
if errorlevel 1 goto :fail
set "SHOW=%CD%\%MBD%"
if defined OUTROOT set "SHOW=%OUTROOT%"
goto :done

REM ============================================================
:t_sln
echo [TIPO] Visual Studio: %F_SLN%
call :msvc
REM Redireciona SDK do Windows (10.0 = mais recente instalado) e toolset (mais novo do VS instalado)
set "TS="
for /d %%D in ("%VSPATH%\MSBuild\Microsoft\VC\v*") do for /f %%T in ('dir /b /ad /o-n "%%D\Platforms\x64\PlatformToolsets\v*" 2^>nul') do if not defined TS set "TS=%%T"
if not defined TS set "TS=v143"
echo [INFO] Toolset: %TS%
set "OUTARG="
if defined OUTROOT set OUTARG=-p:OutDir="%OUTROOT%\\"
set "_CL_=%MSVC_C% /GL"
set "_LINK_=%MSVC_LD%"
msbuild "%F_SLN%" -restore -m -p:Configuration=Release -p:Platform=x64 -p:WindowsTargetPlatformVersion=10.0 -p:PlatformToolset=%TS% %OUTARG%
if errorlevel 1 goto :fail
set "SHOW=%CD%"
if defined OUTROOT set "SHOW=%OUTROOT%"
goto :done

REM ============================================================
:t_csproj
echo [TIPO] .NET: %F_CS%
set "DOUT=%CD%\publish"
if defined OUTROOT set "DOUT=%OUTROOT%"
dotnet publish "%F_CS%" -c Release -r win-x64 --self-contained false -p:PublishReadyToRun=true -p:Optimize=true -p:DebugType=none -o "%DOUT%"
if errorlevel 1 goto :fail
set "SHOW=%DOUT%"
goto :done

REM ============================================================
:t_rust
echo [TIPO] Rust: %F_RUST%
set "CARGO_PROFILE_RELEASE_LTO=fat"
set "CARGO_PROFILE_RELEASE_CODEGEN_UNITS=1"
set "CARGO_PROFILE_RELEASE_OPT_LEVEL=3"
set "CARGO_PROFILE_RELEASE_PANIC=abort"
set "CARGO_PROFILE_RELEASE_STRIP=symbols"
if "%NATIVE%"=="1" set "RUSTFLAGS=-C target-cpu=native"
set "OUTARG="
if defined OUTROOT set OUTARG=--target-dir "%OUTROOT%"
cargo build --release --manifest-path "%F_RUST%" %OUTARG%
if errorlevel 1 goto :fail
set "SHOW=%CD%\target\release"
if defined OUTROOT set "SHOW=%OUTROOT%\release"
goto :done

REM ============================================================
:t_go
echo [TIPO] Go: %F_GO%
set "CGO_ENABLED=0"
set "GOUT=%CD%\bin"
if defined OUTROOT set "GOUT=%OUTROOT%"
mkdir "%GOUT%" 2>nul
for %%I in ("%F_GO%") do pushd "%%~dpI"
go build -trimpath -ldflags="-s -w" -o "%GOUT%\\" ./...
if errorlevel 1 (popd & goto :fail)
popd
set "SHOW=%GOUT%"
goto :done

REM ============================================================
:t_gradle
echo [TIPO] Gradle: %F_GRADLE%
for %%I in ("%F_GRADLE%") do pushd "%%~dpI"
if exist "gradlew.bat" (call gradlew.bat build -x test) else (call gradle build -x test)
if errorlevel 1 (popd & goto :fail)
popd
set "SHOW=%CD%"
goto :done

REM ============================================================
:t_maven
echo [TIPO] Maven: %F_MVN%
for %%I in ("%F_MVN%") do pushd "%%~dpI"
if exist "mvnw.cmd" (call mvnw.cmd -B clean package -DskipTests) else (call mvn -B clean package -DskipTests)
if errorlevel 1 (popd & goto :fail)
popd
set "SHOW=%CD%"
goto :done

REM ============================================================
:t_node
echo [TIPO] Node.js: %F_NODE%
for %%I in ("%F_NODE%") do pushd "%%~dpI"
if exist "pnpm-lock.yaml" (call pnpm install --frozen-lockfile) else if exist "yarn.lock" (call yarn install --frozen-lockfile) else if exist "package-lock.json" (call npm ci) else (call npm install)
if errorlevel 1 (popd & goto :fail)
set "NODE_ENV=production"
findstr /c:"\"build\"" package.json >nul 2>&1
if errorlevel 1 (
    echo [AVISO] package.json sem script "build"; apenas dependencias instaladas.
) else (
    call npm run build
    if errorlevel 1 (popd & goto :fail)
)
popd
set "SHOW=%CD%"
goto :done

REM ============================================================
:t_python
echo [TIPO] Python: %F_PY%
for %%I in ("%F_PY%") do pushd "%%~dpI"
python -m venv .venv
if errorlevel 1 (popd & goto :fail)
call .venv\Scripts\activate.bat
python -m pip install -U pip wheel build
if exist "requirements.txt" python -m pip install -r requirements.txt
if exist "pyproject.toml" (python -m build --wheel) else if exist "setup.py" (python -m build --wheel)
popd
set "SHOW=%CD%"
goto :done

REM ============================================================
REM  Sub-rotinas
REM ============================================================
:msvc
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" goto :eof
set "VSPATH="
for /f "usebackq delims=" %%I in (`"%VSWHERE%" -latest -products * -property installationPath`) do set "VSPATH=%%I"
if not defined VSPATH goto :eof
if exist "%VSPATH%\VC\Auxiliary\Build\vcvars64.bat" call "%VSPATH%\VC\Auxiliary\Build\vcvars64.bat" >nul
goto :eof

:vcpkg
if not exist "%CD%\.vcpkg\vcpkg.exe" git clone https://github.com/microsoft/vcpkg "%CD%\.vcpkg"
if not exist "%CD%\.vcpkg\vcpkg.exe" call "%CD%\.vcpkg\bootstrap-vcpkg.bat" -disableMetrics
set VCPKG_ARGS=-DCMAKE_TOOLCHAIN_FILE="%CD%\.vcpkg\scripts\buildsystems\vcpkg.cmake"
goto :eof

REM :find <mascara> <variavel> <niveis>  -> caminho relativo do arquivo mais raso encontrado
:find
set "%~2="
set "BESTN=99"
for /f "delims=" %%F in ('dir /s /b /a-d "%~1" 2^>nul') do call :cand "%%F" "%~2" %~3
goto :eof

:cand
set "P=%~1"
set "R=!P:%CD%\=!"
if "!R!"=="!P!" goto :eof
set "RR=\!R!"
for %%S in (.git node_modules bin obj vendor third_party external extern deps packages) do if not "!RR:\%%S\=!"=="!RR!" goto :eof
set "N=1"
set "TT=!R!"
:cnt
if "!TT:*\=!"=="!TT!" goto :cntdone
set "TT=!TT:*\=!"
set /a N+=1
goto :cnt
:cntdone
if !N! GTR %~3 goto :eof
if !N! LSS !BESTN! (set "BESTN=!N!" & set "%~2=!R!")
goto :eof

:fail
echo.
echo [FALHA] A compilacao falhou. Veja as mensagens acima.
pause
exit /b 1

:done
echo.
echo [OK] Compilacao concluida.
echo [SAIDA] %SHOW%
start "" "%SHOW%"
pause
exit /b 0
