# Live demo walkthrough: Copilot in VS Code with C#

> **Current as of 18 November 2026** — VS Code 1.146, .NET 10, **Copilot harness**.
> A presenter-led walkthrough (about 30 minutes) using this repository's C# sample. It makes the same points as the
> [OctoCAT Supply general demo](https://github.com/Azure-Samples/octocat-supply/blob/main/demo/walkthroughs/general/demo-steps.md)
> but is rewritten for this sample, the Copilot harness, and current terminology. No OctoCAT Supply code is used.
> What changed and why: [VALIDATION.md](../VALIDATION.md#demo-walkthrough-demo1--demo2).

**Legend:** **[Do]** an action you take · **[PROMPT]** text to paste into chat · `[Ask]` / `[Agent]` the agent to select in the chat input · **✅** what the audience should see.

> [!NOTE]
> Older demo scripts say *Ask Mode* and *Agent Mode*. In the Copilot harness these are **agents** that you choose from the agent picker: **Ask** (answers only) and **Agent** (edits files and runs commands). Edit mode is deprecated.

## Before you present (5 minutes, once)

1. Complete [Part 0 and Part 1 of the README](../README.md#part-0--prerequisites-do-this-before-the-session) so the sample builds and runs.
2. Make sure the repo is clean so the demo starts from a known state:

   ```bash
   git status
   ```

3. In the Chat view (`Ctrl+Alt+I`), start a new chat and check the pickers:
   1. **Session Target:** **Copilot** (not *Local*).
   2. **Model:** **Copilot Fusion** (or **Auto**, if shown).
   3. **Permissions:** **Manual permissions**, so the audience sees each approval.
4. If `.github/copilot-instructions.md` exists from an earlier run, delete it so Demo 2 starts from scratch.

---

## DEMO 1: Copilot foundations

### 1.1 Initial setup: VS Code workspace

1. **[Do]** Open the folder in VS Code (`code .`) and select **Yes, I trust the authors**.
2. **[Do]** Point out the Copilot menu in the Status Bar (signed in) and the Chat view's **Session Target: Copilot**.

✅ The Explorer shows `src/BackEnd`, `src/FrontEnd`, and `tests/BackEnd.Tests`.

### 1.2 Project overview

**[PROMPT]** `[Ask]`

```text
Review my codebase and explain the project including any relevant programming languages and test cases that are used.
```

✅ Copilot describes a .NET 10 solution: a C# minimal API (`WeatherRepository`, `/weatherforecast` endpoints, OpenAPI + Scalar), a Blazor front end (Razor + C#), and the xUnit tests in `tests/BackEnd.Tests`. No files change in **Ask**.

### 1.3 Install and run

1. **[Do]** Open the terminal (`` Ctrl+` ``) and restore and build:

   ```bash
   dotnet build
   ```

   > This replaces `make install` / `npm install` from Node.js demos. `dotnet build` restores NuGet packages automatically, on Windows, macOS, and Linux.

2. **[Do]** Start the API:

   ```bash
   dotnet run --project src/BackEnd
   ```

3. **[Optional]** In a second terminal, start the web app with `dotnet run --project src/FrontEnd` and open <http://localhost:5200/weather>.

✅ The terminal shows `Now listening on: http://localhost:5100`.

### 1.4 Inline completion, then let the agent finish the job

1. **[Do]** Open these files and keep them open as tabs (Copilot uses open files as context):
   1. `src/BackEnd/WeatherForecast.cs`
   2. `src/BackEnd/WeatherRepository.cs`
   3. `src/BackEnd/Program.cs`
2. **[Do]** In `WeatherRepository.cs`, put the cursor on the blank line **between** `GetByCityAsync` and `GetCitiesAsync` (not at the start or end of the class), and type:

   ```csharp
   // Count forecasts by city
   ```

3. **[Do]** Press `Enter`. When the grey suggestion appears, press `Tab` to accept it (repeat until the method is complete).

   ✅ A new method appears, similar to:

   ```csharp
   public Task<int> CountByCityAsync(string city) => ...
   ```

   > Your method name and body may differ — that's normal. If no suggestion appears, type `public` to nudge Copilot.

4. **[PROMPT]** `[Agent]`

   ```text
   Add a retrieval endpoint for the count-by-city method I just added in WeatherRepository. Also update the API documentation for it (OpenAPI name, summary, and description). Do not run or test it yet.
   ```

✅ The agent edits `Program.cs` to add an endpoint in the **Weather** group, such as `GET /weatherforecast/{city}/count`, with `.WithName(...)`, `.WithSummary(...)`, and `.WithDescription(...)`. It doesn't run any commands. Select **Keep** to accept the changes.

### 1.5 Verification

1. **[Do]** In the API terminal, press `Ctrl+C`, then run `dotnet run --project src/BackEnd` again.
2. **[Do]** Open <http://localhost:5100/scalar>.
   1. Find the **Weather** section.
   2. Find the new endpoint, for example `GET /weatherforecast/{city}/count`.
   3. Select it, enter `Sydney` for `city`, and send the request.

✅ The response is `5`, and the summary and description you asked for appear in the docs.

> Point to make: inline suggestions got the method started in seconds; the agent then wired it through the API *and* its documentation across files.

---

## DEMO 2: Copilot agents

### 2.1 Deep codebase analysis and custom instructions

**[PROMPT]** `[Agent]`

```text
Do a super deep analysis of this codebase. Then create or update .github/copilot-instructions.md to include best practices for every programming language used in the project. Use https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions as a reference for C#.
```

1. **[Do]** When Copilot asks to fetch the Microsoft Learn page, show the approval and select **Allow**.
2. **[Do]** Review the new file and select **Keep**.

✅ `.github/copilot-instructions.md` exists and has sections for C#, ASP.NET Core minimal APIs, Blazor/Razor, and xUnit tests, with guidance taken from the Microsoft Learn page.

> [!NOTE]
> Older scripts use `#codebase` and `#fetch <url>`. Those are **Local harness** tool references and aren't guaranteed to exist in the Copilot harness. In a Copilot harness session, ask in plain language: the agent searches the workspace itself, and including a URL lets it fetch the page (with your approval). To attach specific files or folders, use **Add Context**.
>
> The original script fetched a TypeScript best-practices page. This sample is C#, so the prompt uses Microsoft's official C# coding conventions instead.

> Tip: you can also create the file without a prompt. Run **Chat: Open Customizations**, select **Instructions**, then select **+** next to **Workspace**.

### 2.2 (Optional, for fun) Give Copilot a persona

This shows how quickly instructions apply to **every new** chat in the repo.

1. **[Do]** Pick a persona — for example Neil Armstrong, James Bond, or Ada Lovelace.
2. **[Do]** Add this line at the top of `.github/copilot-instructions.md` (replace the name with your choice) and save:

   ```text
   You are the Neil Armstrong of code. Talk to me like Neil Armstrong and use space characters in your explanations!
   ```

3. **[Do]** Start a **new** chat (`Ctrl+N` in the Chat view) and ask `[Ask]`:

   ```text
   Explain what WeatherRepository does.
   ```

✅ The explanation arrives in your persona's voice (with plenty of 🚀🌕🛰️).

> Remove the persona line after the demo so it doesn't leak into the next exercise.

### 2.3 Code review against your instructions

**[PROMPT]** `[Agent]`

```text
Can you review my code and ensure it follows the best practices outlined in the updated copilot-instructions file? List your findings first, then fix them, and run dotnet build and dotnet test at the end.
```

1. **[Do]** Approve `dotnet build` and `dotnet test` when asked.
2. **[Do]** Review the diff and select **Keep** or **Undo** for each file.

✅ Copilot lists concrete findings that cite the instructions (for example, XML docs, naming, or async usage), applies fixes, and finishes with a passing build and tests.

> Point to make: the instruction file turned a generic AI assistant into one that reviews code against *your team's* standards.

---

## Reset after the demo

Discard everything the demo changed (this deletes uncommitted work):

```bash
git checkout -- .
git clean -fd
```

## Troubleshooting during a live demo

| Problem | Quick fix |
| --- | --- |
| No grey suggestion after the comment | Press `Enter` again or type `public`. Check the Copilot Status Bar menu shows you're signed in. |
| Agent runs `dotnet test` even though you said not to | Select **Skip** on the approval; it's a good moment to explain Manual permissions. |
| New endpoint missing from Scalar | Restart the API (`Ctrl+C`, then `dotnet run --project src/BackEnd`). Scalar only runs in Development, which is the default with `dotnet run`. |
| Fetching the URL is blocked | Your organization may restrict web access. Paste a few key conventions into the prompt instead. |
| Persona not applied | Save the file, start a **new** chat, and confirm the file appears under **Chat: Open Customizations** > **Instructions**. |
