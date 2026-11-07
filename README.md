# GIT Auto Builder v1.0 - Release to Public

Desenvolvi esse aplicativo simples para automatizar a tarefa recorrente de compilar um projeto pela linha de comando sempre que possível. Gosto de utilizar aplicações recém compiladas para testar novos recursos, mas ter que abrir toda vez o Visual Studio para configurar e compilar, acabava tomando tempo demais.

Para isso, essa aplicação em 3 formatos, permite que aqueles que tem pressa gerem o script que:
1) Baixa o fonte, submódulos, e compila conforme opções escolhidas
2) Gera o script de build apenas, considerando que já exista a pasta ou você já fez o pull do fonte
3) Gera o script de publish, se for um projeto .NET e você deseja ter um binário com tudo nele.

O funcionamento é o mais simples possível. Você fornece a URL de um repositório GIT, a ferramenta detecta o tipo de projeto (procurando em até 5 níveis de pastas) e gera .BAT para baixar e compilar, compilar apenas ou publicar apenas.

<u><b>/!\ Atenção: Utilize por sua conta e risco!</b></u>

## Conteúdo
| Item | Para que serve |
|---|---|
| `.\console\build-from-git.bat` | **Aplicação de Console.** `build-from-git.bat <url> [branch] [pasta-de-saida]`.<br> Clona/atualiza, detecta o tipo e compila Release x64. Versão simples (sem as caixas de opção). |
| `.\HTML\GIT Script Auto Builder.html` | **Aplicação em HTML+JS.** <br>Abre no navegador. Analisa a URL pela API (GitHub/GitLab/Gitea) ou uma pasta local. |
| `GitAutoBuilder` | **Código fonte em C#** (.NET 10, C# 14, WinForms, auto-contido). <BR>Após compilada, a aplicação clona e analisa qualquer servidor Git. |

### Aplicação HTML
<img width="1215" height="874" alt="image" src="https://github.com/user-attachments/assets/7b678ea3-7868-47f6-ad3b-fb27b812b801" />

### Aplicação C Sharp
<img width="1368" height="827" alt="image" src="https://github.com/user-attachments/assets/29732331-d12d-40c4-ba48-1f06500f906e" />


## Os 3 botões (HTML e C#)
- **Salvar Script BAT**: clona (ou atualiza) o repositório, baixa submódulos e dependências (vcpkg quando há `vcpkg.json`) e compila pela linha de comando.
- **Salvar Build** (`build.bat`): só cria o script de compilação. Salve na pasta do projeto e rode quando quiser.
- **Salvar Publish** (`publish.bat`): só gera os script de publicação. Mesmo uso do Build.<BR>
Os três respeitam arquitetura, configuração, otimizações e pasta de saída. O preview mostra o script do último botão usado.

## Opções
- **Arquitetura**: x86 e/ou x64. 
- **Configuração**: Release e/ou Debug. Mais de uma marcada = compila todas as combinações em lote, com resumo no fim.
- **Otimizações**: LTCG/LTO, PGO (2 fases, com treino), AVX2, AVX-512, /fp:fast, /Qpar e flags extras.
- **Definições do projeto**: presets do `CMakePresets.json` (ex.: OBS) e configurações do `.sln` (ex.: `Release AVX2`).
- **Pasta de saída (opcional)**, ex.: `.\BIN` ou `.\APP` (relativa à pasta do projeto). Vazia = padrão. Com várias combinações, cada uma ganha uma subpasta.
Aplica-se a MSBuild/sln, CMake, Meson, .NET, Rust e Go. Gradle, Maven. Node e Python usam a saída própria.
- **Opções de publish (.NET)**: Self-contained, Arquivo único, Trimmed, Compressão e ReadyToRun. Valem só no botão **Salvar Publish**. Trimmed e compressão exigem self-contained, então marcá-los liga o que for preciso (equivale a `dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true -p:EnableCompressionInSingleFile=true`).
- O que "publish" significa: .NET `dotnet publish`; CMake build + `cmake --install`; MSBuild `OutDir`; Meson `meson install`; Rust copia exe/dll; Go grava na pasta de publish. Gradle/Maven/Node/Python não distinguem.

## Busca de projetos
Procura em até 5 níveis (sln/vcxproj), 4 (CMake, csproj), 3 (Meson, Rust, Go, Gradle, Maven) e 2 (Node, Python). Ignora `.git`, `node_modules`, `bin`, `obj`, `vendor`, `third_party`, `external`, `extern`, `deps`, `packages`. Se houver mais de um arquivo, a lista "Sistema de build" permite escolher qual usar (ex.: CMake na raiz ou a .sln em `win32\`).

## Como Compilar o app C#
Após compilar o app você pode utilizar ele para compilar as novas versões da aplicação. Na primeira vez execute:
```
dotnet publish -c Release
```
Saída: `bin\Release\net10.0-windows\win-x64\publish\GitAutoBuilder.exe` (arquivo único, sem exigir .NET instalado).
O .csproj já liga trimming para reduzir o tamanho. WinForms não tem suporte oficial a trimming: teste o .exe (clonar, pasta local, salvar). Se algo falhar, ponha `PublishTrimmed` como `false`.

## Editar a interface no Visual Studio
Abra `GitAutoBuilder.csproj` e dê duplo clique em `MainForm.cs` (ou botão direito > View Designer). O visual está em `MainForm.Designer.cs` (só controles padrão do WinForms); a lógica, em `MainForm.cs`.
O ícone da janela vem do ícone do .exe (`ApplicationIcon` no .csproj).
Utilizei o ícone criado por IA, nomeado GAB.ico

## Requisitos
Para que o script funcione, você já deve ter seu ambiente e algumas ferramentas configuradas.
- Git no PATH. 
- Para C/C++ e .NET: Visual Studio / Build Tools, CMake e Windows SDK. 
- Rust, Go, Node, Python, JDK conforme o projeto.
