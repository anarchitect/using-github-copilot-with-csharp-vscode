# Live demo walkthrough: Copilot in VS Code with C#

> **Current as of 18 November 2026** — VS Code 1.146, .NET 10, **Copilot harness**.
> A presenter-led walkthrough (budget 30–45 minutes; not a measured class duration) using this repository's C# sample. It makes the same points as the
> [OctoCAT Supply general demo](https://github.com/Azure-Samples/octocat-supply/blob/main/demo/walkthroughs/general/demo-steps.md)
> but is rewritten for this sample, the Copilot harness, and current terminology. No OctoCAT Supply code is used.
> What changed and why: [VALIDATION.md](../VALIDATION.md#demo-walkthrough-demo1--demo2).

**Legend:** **[Do]** an action you take · **[PROMPT]** text to paste into chat · `[Ask]` / `[Agent]` the agent to select in the chat input · **✅** what the audience should see.

> [!NOTE]
> Older demo scripts say *Ask Mode* and *Agent Mode*. In the Copilot harness these are **agents** that you choose from the agent picker: **Ask** (answers only) and **Agent** (edits files and runs commands). Edit mode is deprecated.

## Before you present (5 minutes, once)

1. Complete [Part 0 and Part 1 of the README](../README.md#part-0--prerequisites-do-this-before-the-session) so the sample builds and runs.
2. Use a **dedicated disposable clone**, not a working company repository. Confirm it is clean:

   ```powershell
   git status
   ```

   If you have earlier exercise changes or instruction files, keep them and use a fresh clone for this demo. Do not delete existing work to reset.
3. In the Chat view (`Ctrl+Alt+I`), start a new chat and check the pickers:
   1. **Session Target:** **Copilot** (not *Local*).
   2. **Language model:** **Copilot Fusion** (or **Auto**, if shown). This chooses the chat model, not the ghost-text completions model.
   3. **Permissions:** **Manual permissions**. Explain that previous approvals and read-only actions may not prompt again.
4. Use synthetic data only. Local file operations still involve hosted model inference; follow company Copilot policy. See the README's prerequisite and C# Dev Kit licensing checks.

---

## DEMO 1: Copilot foundations

### 1.1 Initial setup: VS Code workspace

1. **[Do]** Inspect the public sample, open it in VS Code (`code .`), and select **Yes, I trust the authors** only if you trust it.
2. **[Do]** Point out the Copilot menu in the Status Bar (signed in). Choose **Copilot** in **Session Target** before the first prompt; an existing session shows a **Copilot** prefix in its chat title instead.

✅ The Explorer shows `src/BackEnd`, `src/FrontEnd`, and `tests/BackEnd.Tests`.

### 1.2 Project overview

**[PROMPT]** `[Ask]`

```text
Review my codebase and explain the project including any relevant programming languages and test cases that are used.
```

✅ Copilot describes a .NET 10 solution: a C# minimal API (`WeatherRepository`, `/weatherforecast` endpoints, OpenAPI + Scalar), a Blazor front end (Razor + C#), and the xUnit tests in `tests/BackEnd.Tests`. No files change in **Ask**.

### 1.3 Install and run

1. **[Do]** Open the terminal (`` Ctrl+` ``) and restore and build:

   ```powershell
   dotnet build
   ```

   > This replaces `make install` / `npm install` from Node.js demos. `dotnet build` restores NuGet packages automatically.

2. **[Do]** Start the API:

   ```powershell
   dotnet run --project src/BackEnd
   ```

3. **[Optional]** In a second terminal, start the web app with `dotnet run --project src/FrontEnd` and open <http://localhost:5200/weather>.

✅ The terminal shows `Now listening on: http://localhost:5100`.

4. **[Do]** Stop both apps with `Ctrl+C` before the next step. This avoids locked build files and stale results on Windows.

### 1.4 Ghost text suggestions, then let the agent finish the job

**Inline suggestions** includes both **ghost text suggestions** at the cursor and **next edit suggestions** (NES) for another edit/location. This step demonstrates ghost text, not the NES gutter arrow.

1. **[Do]** Open these files and keep them open as tabs (Copilot uses open files as context):
   1. `src/BackEnd/WeatherForecast.cs`
   2. `src/BackEnd/WeatherRepository.cs`
   3. `src/BackEnd/Program.cs`
2. **[Do]** In `WeatherRepository.cs`, put the cursor on the blank line **between** `GetByCityAsync` and `GetCitiesAsync` (not at the start or end of the class), and type:

   ```csharp
   // Count forecasts by city
   ```

3. **[Do]** Press `Enter`. If no dimmed suggestion appears, run **Trigger Inline Suggestion** from the Command Palette. Review the ghost text, then press `Tab` to accept it (repeat on new lines until the method is complete).

   ✅ A new method appears, similar to:

   ```csharp
   public Task<int> CountByCityAsync(string city) => ...
   ```

   > Your method name and body may differ — that's normal. If ghost text remains unavailable, record **"ghost text not observed"** and ask Agent to add `CountByCityAsync(string city)` using the existing case-insensitive lookup, returning `0` for an unknown city. Label this as agent recovery, not a ghost-text demonstration. For a method-stub variation, use [README Exercise 1b](../README.md#exercise-1b--ghost-text-suggestions-from-a-method-stub-5-min).

   Save the completed method (`Ctrl+S`) before asking the agent to use it.

4. **[PROMPT]** `[Agent]`

   ```text
   Add GET /weatherforecast/{city}/count for the count-by-city method I just added in WeatherRepository. Return the number of forecasts, or 0 for an unknown city. Update the API documentation (OpenAPI name, summary, and description). Do not build, run or test the app yet.
   ```

✅ The agent edits `Program.cs` to add `GET /weatherforecast/{city}/count` in the **Weather** group with `.WithName(...)`, `.WithSummary(...)`, and `.WithDescription(...)`. It may read files or inspect the repository, but should not build or start the app. Tracked edits are already saved; review the diff before selecting **Keep** to mark them reviewed.

### 1.5 Verification

1. **[Do]** With both apps stopped, run `dotnet build` and `dotnet test`. Fix failures before continuing, then run `dotnet run --project src/BackEnd`.
2. **[Do]** Open <http://localhost:5100/scalar>.
   1. Find the **Weather** section.
   2. Find the new endpoint, for example `GET /weatherforecast/{city}/count`.
   3. Select it, enter `Sydney` for `city`, and send the request.

✅ The response is `5`, and the summary and description you asked for appear in the docs.

Also try `Atlantis`: the documented count contract returns `0`. Check <http://localhost:5100/weatherforecast/Sydney/count> directly if Scalar cannot send the request. Stop the API before Demo 2's build step.

> Point to make: when observed, ghost text got the method started; the agent then wired it through the API *and* its documentation across files. Do not claim ghost-text success if you used the agent recovery.

---

## DEMO 2: Copilot agents

### 2.1 Deep codebase analysis and custom instructions

**[PROMPT]** `[Agent]`

```text
Inspect this sample's .NET 10 API, Blazor InteractiveServer app and xUnit tests. Create a concise .github/copilot-instructions.md with verified repository facts, build/test commands and relevant conventions. Use https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions as a C# reference. Apply async only to actual I/O. Keep new feature endpoints in separate files, but do not move existing endpoints or modify application code in this step.
```

1. **[Do]** If Copilot asks to fetch the Microsoft Learn page, show the approval and select **Allow Once** where offered. Existing approvals may allow it without a new prompt.
2. **[Do]** Review the new file and select **Keep**.

✅ `.github/copilot-instructions.md` exists and has sections for C#, ASP.NET Core minimal APIs, Blazor/Razor, and xUnit tests, with guidance taken from the Microsoft Learn page.

> [!NOTE]
> Older scripts use `#codebase` and `#fetch <url>`. Those are **Local harness** tool references and aren't guaranteed to exist in the Copilot harness. In a Copilot harness session, ask in plain language: the agent searches the workspace itself, and including a URL lets it fetch the page (with your approval). To attach specific files or folders, use **Add Context**.
>
> The original script fetched a TypeScript best-practices page. This sample is C#, so the prompt uses Microsoft's official C# coding conventions instead.

> Tip: you can also create the file without a prompt. Run **Chat: Open Customizations**, select **Instructions**, then select **+** next to **Workspace**.

### 2.2 (Optional, for fun) Give Copilot a persona

This explores repository instructions in a **new** chat. Discovery, inclusion in a request, and compliance are separate checks; the model may not follow the requested tone.

1. **[Do]** Pick a persona, for example Neil Armstrong or James Bond.
2. **[Do]** Add this line at the top of `.github/copilot-instructions.md` (replace the name with your choice) and save:

   ```text
   You are the Neil Armstrong of code. Talk to me like Neil Armstrong and use space characters in your explanations!
   ```

3. **[Do]** Start a **new** chat (`Ctrl+N` in the Chat view) and ask `[Ask]`:

   ```text
   Explain what WeatherRepository does.
   ```

✅ The explanation arrives in your persona's voice. Instruction following is not guaranteed, so treat the tone as a fun signal rather than proof.

> Remove the persona line after the demo so it doesn't leak into the next exercise.

**(Optional) A team-friendly alternative:** instead of a persona, add this line to show a convention that's useful for a real team:

```text
Explain changes in plain language for developers new to Copilot. State what changed, why, and how to verify it. Do not claim a command passed unless you ran it.
```

### 2.3 Code review against your instructions

**[PROMPT]** `[Agent]`

```text
Review this sample against the copilot-instructions file. List concrete correctness or clarity findings first, with file references. Fix only supported findings; do not make a change just to demonstrate activity or refactor the whole sample. Run dotnet build and dotnet test and report their actual results. Do not commit or push.
```

1. **[Do]** Confirm both apps are stopped. Review requests for `dotnet build` and `dotnet test`, and approve only the intended actions.
2. **[Do]** Review the diff and select **Keep** or **Undo** for each file.

✅ Copilot explains supported findings (or reports none), makes only justified changes, and finishes with passing build and tests. Existing tests alone do not prove the new count endpoint: recheck Sydney → `5` and Atlantis → `0`, or ask for endpoint tests.

> Point to make: the instruction file supplies team conventions. Judge the actual review and changes; discovering an instruction file is not proof of compliance.

---

## Reset after the demo

Stop both apps with `Ctrl+C`, then inspect and keep the work you want:

```powershell
git status --short
git diff
```

For another clean run, clone into a **new** directory. Leave the earlier clone intact until you have reviewed its changes. Do not use blanket discard or `git clean` commands on a learner's working directory.

## Troubleshooting during a live demo

| Problem | Quick fix |
| --- | --- |
| No ghost text after the comment | Run **Trigger Inline Suggestion**. Check the Copilot Status Bar menu shows you're signed in and suggestions are enabled for C#. |
| Agent proposes a build/test before verification | Deny the request if prompted and restate the boundary. Previously approved commands may not prompt; approval settings, not prose alone, control execution. |
| New endpoint missing from Scalar | Restart the API (`Ctrl+C`, then `dotnet run --project src/BackEnd`). Scalar only runs in Development, which is the default with `dotnet run`. |
| Fetching the URL is blocked | Your organization may restrict web access. Paste a few key conventions into the prompt instead. |
| Persona not applied | Save the file, start a **new** chat, and confirm the file appears under **Chat: Open Customizations** > **Instructions**. Remove the persona line when you're done. |
| Instructions seem ignored | Save the file, start a **new** chat, and confirm it appears under **Chat: Open Customizations** > **Instructions**. Review the actual response; discovery is not guaranteed compliance. |
