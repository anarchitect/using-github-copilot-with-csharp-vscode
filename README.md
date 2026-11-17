# Using GitHub Copilot with C# in VS Code (Copilot harness)

> **Current as of 18 November 2026** — VS Code 1.146, .NET 10 (LTS), GitHub Copilot **Copilot harness**.
> A beginner-friendly, hands-on refresh of Microsoft's
> [Using GitHub Copilot with C#](https://github.com/microsoft/Mastering-GitHub-Copilot-for-Paired-Programming/tree/main/Using-GitHub-Copilot-with-CSharp)
> module, rewritten for **local VS Code only** (no Codespaces). See [VALIDATION.md](VALIDATION.md) for what changed and why.

> [!IMPORTANT]
> The **Local harness is deprecated** (VS Code 1.146) and will be **removed in February 2027**
> ([changelog](https://github.blog/changelog/2026-11-16-vs-code-retires-the-local-harness-in-february-2027/)).
> Every exercise here uses the **Copilot harness**, which has been the default for new sessions since VS Code 1.143.
> It still runs **on your machine** — your files do not move to the cloud.

## What you will learn

1. Set up VS Code, .NET 10, and GitHub Copilot on your own computer.
2. Start a chat session that uses the **Copilot harness**, and pick an agent, model, and permission level.
3. Write C# faster with **inline suggestions** and **next edit suggestions (NES)**.
4. Explain and document code with **inline chat**.
5. Ask questions about a codebase, **plan** a change, then let an **agent** build and test it.
6. Teach Copilot your team's conventions with **custom instructions**.
7. (Optional, preview, if MCP is allowed) Bring in Microsoft Learn docs with a **Copilot connector** or an **MCP server**.

**Time:** about 90 minutes. **Level:** beginner. No prior Copilot experience needed.

## The sample app

This repository contains a small, runnable .NET 10 solution that every exercise uses:

| Project | What it is | URL when running |
| --- | --- | --- |
| `src/BackEnd` | ASP.NET Core minimal API with a 5-day forecast for Brisbane, Sydney, and Melbourne, plus interactive API docs (OpenAPI + Scalar) | http://localhost:5100/weatherforecast and http://localhost:5100/scalar |
| `src/FrontEnd` | Blazor Web App (interactive server) that shows the forecasts in a table | http://localhost:5200 |
| `tests/BackEnd.Tests` | xUnit tests for the API | `dotnet test` |

Want a shorter, presenter-led version? See the [live demo walkthrough](demo/walkthrough.md) (Demo 1: Copilot foundations, Demo 2: agents and custom instructions).

---

## Part 0 — Prerequisites (do this before the session)

1. **A GitHub account with Copilot.** Any plan works, including **Copilot Free** (signing in without a plan enrolls an eligible account in Copilot Free). If your company provides Copilot, use that account.
2. **VS Code 1.146 or later** — <https://code.visualstudio.com/download>. Check with **Help > About**. GitHub Copilot is built into VS Code; you don't need to install a separate Copilot extension.
3. **.NET 10 SDK** — <https://dotnet.microsoft.com/download/dotnet/10.0>. Check in a terminal:

   ```bash
   dotnet --version
   ```

   You should see `10.0.x` or later.

   > .NET 8 (used by the original module) reached end of support on 10 November 2026. Use .NET 10 (LTS, supported to November 2028).

4. **C# Dev Kit** extension for VS Code — install from the Extensions view (`Ctrl+Shift+X`) by searching for `C# Dev Kit`. This repo also recommends it automatically when you open the folder.
5. **Git** — <https://git-scm.com/downloads>.

## Part 1 — Get the sample running locally

1. Clone and open the repository:

   ```bash
   git clone https://github.com/anarchitect/using-github-copilot-with-csharp-vscode.git
   cd using-github-copilot-with-csharp-vscode
   code .
   ```

2. When VS Code asks **Do you trust the authors of the files in this folder?**, select **Yes, I trust the authors**. (Agents can't edit or run anything in Restricted Mode.)
3. Open the integrated terminal (`` Ctrl+` ``) and build and test:

   ```bash
   dotnet build
   dotnet test
   ```

4. Start the **API** in that terminal:

   ```bash
   dotnet run --project src/BackEnd
   ```

5. Open a **second** terminal (select **+** in the terminal panel) and start the **web app**:

   ```bash
   dotnet run --project src/FrontEnd
   ```

6. Open <http://localhost:5200> and select **Weather**. Then open <http://localhost:5100/scalar> to see the API docs.

> **✅ Checkpoint 1:** `dotnet test` shows **5 passed**, the Weather page shows a 15-row table (5 days × 3 cities), and the Scalar page lists the **Weather** endpoints.
> Leave both apps running, or stop them with `Ctrl+C` and restart when needed.

## Part 2 — Sign in and start a Copilot harness session

1. **Sign in:** select **Sign in to use Copilot** in the Copilot menu in the Status Bar, or use the **Accounts** menu in the Activity Bar > **Sign in with GitHub to use GitHub Copilot**.
2. **Open the Chat view:** `Ctrl+Alt+I`. Start a new chat with `Ctrl+N` while the Chat view has focus.
3. In the chat input you'll find four pickers. Set them like this for the whole module:

   | Picker | Choose | Why |
   | --- | --- | --- |
   | **Session Target** | **Copilot** | The Copilot harness. It's the default, but check it — do **not** pick *Local*. |
   | **Agent** | **Ask** or **Agent** (each exercise tells you which) | *Ask* answers questions; *Agent* edits files and runs commands. Type `/plan` for planning. |
   | **Model** | **Copilot Fusion** (or **Auto**, if shown) | Copilot chooses and coordinates models for you; you only pick an effort level. |
   | **Permissions** | **Manual permissions** (default) | You approve each command or file action — the best way to learn what the agent does. |

> **✅ Checkpoint 2:** Session Target says **Copilot**. Ask: `What can you help me with in this workspace?` and get a reply that mentions the BackEnd and FrontEnd projects.

> [!TIP]
> Type `/` in the chat input to see the slash commands available in Copilot harness sessions, such as `/plan`, `/compact`, `/fork`, and any installed skills.

---

## Exercise 1 — Inline suggestions (≈10 min)

Inline suggestions appear as grey "ghost text" while you type.

1. In the Explorer, right-click `src/BackEnd` > **New File**, and name it `Customer.cs`.
2. Type this comment on the first line and press `Enter`:

   ```csharp
   // Create a class for a Customer with Id, Name, and Email, and a method to validate the email format
   ```

3. Wait for the grey suggestion. Press `Tab` to accept it, or `Esc` to dismiss it. Keep pressing `Enter`/`Tab` until the class is complete.
4. Hover over a suggestion to see the toolbar, where you can cycle alternatives or accept one word at a time (`Ctrl+Right Arrow`).

> **✅ Checkpoint 3:** `Customer.cs` contains a `Customer` class with `Id`, `Name`, `Email`, and an `IsValidEmail()`-style method, and `dotnet build` still succeeds.
>
> Your code will differ from your neighbour's — Copilot's output isn't deterministic. That's expected.

## Exercise 2 — Next edit suggestions (≈5 min)

Next edit suggestions (NES) predict the *next* change you'll need, based on the edit you just made.

1. In `Customer.cs`, rename the `Name` property to `FullName`.
2. Look for an **arrow in the editor gutter** pointing at another line that still uses `Name`.
3. Press `Tab` to jump to the suggestion, and `Tab` again to accept it.

> **✅ Checkpoint 4:** The rename is applied everywhere in the file.
> No gutter arrow? Check that the setting `github.copilot.nextEditSuggestions.enabled` is on (it's on by default).

## Exercise 3 — Inline chat (≈10 min)

Inline chat lets you ask for a change right where your cursor is.

1. Select the email-validation method in `Customer.cs`.
2. Press `Ctrl+I` and type:

   ```text
   Explain what this method does and list any email formats it would get wrong.
   ```

3. Select the method again, press `Ctrl+I`, and type:

   ```text
   Add XML documentation comments to this method.
   ```

4. Review the inline diff and select **Keep** (or **Undo**).

> **✅ Checkpoint 5:** The method has a `/// <summary>` comment.

> [!NOTE]
> Older material asks you to type `/explain`, `/doc`, `/fix`, or `/generate`. Plain-English prompts like the ones above work everywhere and are the recommended approach in the Copilot harness.

## Exercise 4 — Ask about the codebase (≈10 min)

1. In the Chat view, set the agent picker to **Ask**.
2. Select **Add Context** (or type `#`) and attach `src/FrontEnd/Components/Pages/Weather.razor` and `src/BackEnd/Program.cs`.
3. Ask:

   ```text
   How does the Weather page get its data from the API? Walk me through it step by step for a beginner.
   ```

4. Follow up:

   ```text
   Where would I add a new API endpoint, and where would I add a new page to show its data?
   ```

> **✅ Checkpoint 6:** The answer mentions `WeatherForecastClient`, the `BackEndUrl` setting (`http://localhost:5100`), `WeatherRepository`, and the `/weatherforecast` endpoints. Ask doesn't change any files.

## Exercise 5 — Plan before you build (≈10 min)

1. Set the agent picker to **Agent**, then type:

   ```text
   /plan Add a Customer API to the BackEnd project: GET all, GET by id, POST, PUT and DELETE, stored in memory, with email validation and xUnit tests.
   ```

2. Answer any clarifying questions. When the plan card appears, select **Open Full Plan** to read it.
3. Add feedback if you want changes (for example, *"Keep endpoints in a separate file, not in Program.cs"*), then select **Submit Feedback**.

> **✅ Checkpoint 7:** You have a plan that lists the files to change and how the result will be verified. No code has changed yet.

## Exercise 6 — Let the agent build and test it (≈20 min)

1. Implement the plan from Exercise 5, or start a new chat with **Agent** selected and paste:

   ```text
   In the BackEnd project:
   1. Use the existing Customer class. Add a CustomerService registered with dependency injection that stores customers in memory.
   2. Add CRUD endpoints under /customers in a separate CustomerEndpoints.cs file using a route group. Return 400 for an invalid email and 404 when a customer isn't found.
   3. Add xUnit tests in tests/BackEnd.Tests for each endpoint, including the 400 and 404 cases.
   4. Run dotnet build and dotnet test, and fix any failures.
   5. Finish with a short summary of what you changed.
   ```

2. Watch the agent work. With **Manual permissions**, it asks before running commands such as `dotnet test`. Read each command, then select **Allow** (use the dropdown to allow similar commands for this session), or **Skip** and tell it what to do instead.
3. Review the changed files in the chat's changes list. **Keep** what you like and **Undo** what you don't.

> **✅ Checkpoint 8:** `dotnet test` passes with more than 5 tests, and with the API running, <http://localhost:5100/customers> returns `[]` or a JSON list.

> [!TIP]
> Once you're comfortable, try **Assisted permissions**, where an AI judge auto-approves low-risk tool calls and asks you about the rest. Avoid *Bypass approvals* and *Autopilot* while learning.

## Exercise 7 — Add a page to the front end (≈10 min)

With **Agent** selected:

```text
In the FrontEnd project, add a Customers page at /customers that lists customers from the BackEnd /customers endpoint in a table, with a form to add a customer. Follow the same pattern as WeatherForecastClient and Weather.razor, add a nav link in MainLayout.razor, and run dotnet build.
```

Restart the FrontEnd app (`Ctrl+C`, then `dotnet run --project src/FrontEnd`) and open <http://localhost:5200/customers>.

> **✅ Checkpoint 9:** You can add a customer in the browser and see it in the table.

> [!NOTE]
> The FrontEnd is a Blazor **server** app, so the browser never calls the API directly — no CORS setup is needed. If the agent suggests adding CORS, ask it why.

## Exercise 8 — Custom instructions (≈10 min)

Custom instructions are Markdown files that Copilot reads automatically, so you don't have to repeat your conventions in every prompt.

1. Create `.github/copilot-instructions.md` (applies to the whole repository):

   ```markdown
   # Project guidelines
   - This is a .NET 10 solution: an ASP.NET Core minimal API (src/BackEnd) and a Blazor Web App (src/FrontEnd).
   - Use file-scoped namespaces, nullable reference types, and records for DTOs.
   - Keep endpoints out of Program.cs; group them in *Endpoints.cs files using MapGroup.
   - Every new endpoint needs xUnit tests in tests/BackEnd.Tests.
   - Explain changes in simple language; the team is new to C#.
   ```

2. Create `.github/instructions/csharp.instructions.md` (applies only to matching files):

   ```markdown
   ---
   applyTo: "**/*.cs"
   ---
   - Use async/await for I/O and pass CancellationToken where available.
   - Prefer guard clauses and return early.
   - Add XML documentation comments to public members.
   ```

3. Start a **new** chat (Agent) and ask:

   ```text
   Add a GET /customers/search?name= endpoint that searches customers by name.
   ```

4. Check that the code follows them (a separate endpoints file, async code, XML docs, and new tests). To confirm the files are discovered, run **Chat: Open Customizations** and select **Instructions**.

> **✅ Checkpoint 10:** The new endpoint follows your conventions without you mentioning them in the prompt.

> [!NOTE]
> The Copilot harness also reads `AGENTS.md` files. **Prompt files (`*.prompt.md`) aren't loaded in Copilot harness sessions** — convert reusable prompts to [agent skills](https://code.visualstudio.com/docs/agent-customization/migrate-customizations) instead.

## Exercise 9 (optional, preview, if MCP is allowed) — Bring in Microsoft Learn docs

Give Copilot up-to-date .NET documentation. Choose **one** option.

**Option A: Copilot connector (preview, no configuration)**

1. Make sure Session Target is **Copilot**.
2. Run **Chat: Open Customizations** from the Command Palette (`Ctrl+Shift+P`) and select **Browse**.
3. Choose **Copilot Connectors**, find **Microsoft Learn**, select **Connect**, and complete authorization.

**Option B: MCP server for this workspace**

Create `.mcp.json` in the repository root (this is the Copilot harness format, using `mcpServers`):

```json
{
  "mcpServers": {
    "microsoft-learn": {
      "type": "http",
      "url": "https://learn.microsoft.com/api/mcp"
    }
  }
}
```

Or add it through **Chat: Open Customizations** > **MCP Servers** > **Add Server**.

Then ask (Agent):

```text
Using Microsoft Learn, check that my Customer endpoints follow current ASP.NET Core minimal API guidance for .NET 10 and suggest improvements.
```

> **✅ Checkpoint 11:** The chat shows a Microsoft Learn tool call, and the answer links to learn.microsoft.com.

---

## Moving from the Local harness

If you, or your learners, used Copilot chat before VS Code 1.143, you may have Local harness habits or files. Here's how they map:

| If you used… (Local harness) | Do this now (Copilot harness) |
| --- | --- |
| **Session Target: Local** | Select **Copilot**. To continue an old conversation, open it, open **Session Target**, and select **Copilot**. History and context carry over; tools, permissions, and models may differ. |
| **Edit mode** | Use the **Agent** agent. Edit mode is deprecated. |
| `/explain`, `/doc`, `/fix`, `/generate` in the Chat view | Describe what you want in plain English, or use inline chat (`Ctrl+I`) on a selection. Type `/` to see the commands available now. |
| Prompt files (`*.prompt.md`) | Convert them to **skills**; the Copilot harness doesn't load prompt files. |
| `.vscode/mcp.json` (`servers` format) | Use workspace-root `.mcp.json` or `~/.copilot/mcp-config.json` (`mcpServers` format). **Chat: Open Customizations** > **Migrations** can move them for you. |
| `.github/copilot-instructions.md`, `.github/instructions/*.instructions.md`, `AGENTS.md` | Keep them — they work in the Copilot harness. |
| Personal agents/instructions stored in your VS Code profile | Move them to `~/.copilot/agents` (or `.github/agents` for the team). |
| Hooks | Review them manually; tool names and formats differ. |

**Quick migration check:** run **Chat: Open Customizations**, select **Migrations**, and review each detected item. Full guides:
[Move from the Local to the Copilot harness](https://code.visualstudio.com/docs/agents/run/move-to-copilot) ·
[Migrate customizations](https://code.visualstudio.com/docs/agent-customization/migrate-customizations).

> Administrators: if a team still needs the Local harness temporarily, the `vscode.temporaryDefaultToLocalHarness` policy keeps it available until the February 2027 removal.

## Troubleshooting

| Problem | Fix |
| --- | --- |
| No Copilot features or chat | Sign in (Status Bar Copilot menu). Check that `chat.disableAIFeatures` isn't enabled. |
| **Copilot** missing from Session Target | Update VS Code to 1.146+. Your organization may restrict harnesses — ask your admin. |
| Agent can't edit files or run commands | Trust the workspace (**Workspaces: Manage Workspace Trust**). |
| `dotnet` not found or wrong version | Install the .NET 10 SDK, then restart VS Code so the terminal picks up `PATH`. |
| Weather page says it can't reach the API | Start `src/BackEnd` first; it must run on `http://localhost:5100`. |
| Port already in use | Stop the other `dotnet run` (`Ctrl+C`) or change `applicationUrl` in `Properties/launchSettings.json`. |
| No ghost text | Check `github.copilot.enable` for C#, and your Copilot Free monthly allowance. |
| Agent asks for approval too often | Use the **Allow** dropdown to allow a command for the session, or try **Assisted permissions**. |
| Chat gets slow or loses track in a long session | Type `/compact`, or start a new chat. |
| Connectors don't appear in **Browse** | Session Target must be **Copilot**; your plan or admin policy may block them. |

## Quick reference

| Action | Windows |
| --- | --- |
| Open the Chat view | `Ctrl+Alt+I` |
| New chat (in Chat view) | `Ctrl+N` |
| Inline chat (editor or terminal) | `Ctrl+I` |
| Accept / dismiss suggestion | `Tab` / `Esc` |
| Command Palette | `Ctrl+Shift+P` |

## Learn more

1. [Choose an agent harness](https://code.visualstudio.com/docs/agents/run/agent-harnesses)
2. [AI features cheat sheet](https://code.visualstudio.com/docs/agents/reference/ai-features-cheat-sheet)
3. [Manage approvals and permissions](https://code.visualstudio.com/docs/agents/run/approvals)
4. [Plan work with agents](https://code.visualstudio.com/docs/agents/run/planning)
5. [Inline chat](https://code.visualstudio.com/docs/chat/inline-chat)
6. [Custom instructions reference](https://code.visualstudio.com/docs/agents/reference/custom-instructions-reference)
7. [Copilot connectors (preview)](https://code.visualstudio.com/docs/agent-customization/copilot-connectors)
8. [MCP configuration reference](https://code.visualstudio.com/docs/agents/reference/mcp-configuration)
9. [Canvases (preview)](https://code.visualstudio.com/docs/agents/run/canvases)

## Attribution and license

This module is adapted from
[Using GitHub Copilot with C#](https://github.com/microsoft/Mastering-GitHub-Copilot-for-Paired-Programming/tree/main/Using-GitHub-Copilot-with-CSharp)
in [microsoft/Mastering-GitHub-Copilot-for-Paired-Programming](https://github.com/microsoft/Mastering-GitHub-Copilot-for-Paired-Programming),
© Microsoft Corporation, licensed under [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/).
Changes: rewritten for local VS Code and the Copilot harness, updated to .NET 10, restructured into beginner exercises with checkpoints, and outdated steps removed (see [VALIDATION.md](VALIDATION.md)).
The sample app is an original minimal implementation of the Weather API + Blazor scenario used by
[github/dotnet-codespaces](https://github.com/github/dotnet-codespaces).
The [demo walkthrough](demo/walkthrough.md) is adapted from the DEMO1/DEMO2 steps in
[Azure-Samples/octocat-supply](https://github.com/Azure-Samples/octocat-supply/blob/main/demo/walkthroughs/general/demo-steps.md); no code from that repository is included.

1. Documentation: [CC BY 4.0](LICENSE).
2. Sample code: [MIT](LICENSE-CODE).

This is not an official Microsoft or GitHub publication.
