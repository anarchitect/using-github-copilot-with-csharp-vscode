# Using GitHub Copilot with C# in VS Code (Copilot harness)

> **Current as of 18 November 2026** — VS Code 1.146, .NET 10 (LTS), GitHub Copilot **Copilot harness**.
> A beginner-friendly, hands-on refresh of Microsoft's
> [Using GitHub Copilot with C#](https://github.com/microsoft/Mastering-GitHub-Copilot-for-Paired-Programming/tree/main/Using-GitHub-Copilot-with-CSharp)
> module, rewritten for **local VS Code only** (no Codespaces). See [VALIDATION.md](VALIDATION.md) for what changed and why.

> [!IMPORTANT]
> The **Local harness is deprecated** (VS Code 1.146) and will be **removed in February 2027**
> ([changelog](https://github.blog/changelog/2026-11-16-vs-code-retires-the-local-harness-in-february-2027/)).
> Every exercise here uses the **Copilot harness**, which has been the default for new sessions since VS Code 1.143.
> Its file edits and terminal commands run **on your machine**. With the hosted models used in this lab, prompts and relevant code context are sent to the model service. "Local" does not mean offline or that code context never leaves the device. Follow your company's approved Copilot policies.

## What you will learn

1. Set up VS Code, .NET 10, and GitHub Copilot on your own computer.
2. Start a chat session that uses the **Copilot harness**, and pick an agent, model, and permission level.
3. Write C# faster with **inline suggestions** and **next edit suggestions (NES)**.
4. Explain and document code with **inline chat**.
5. Ask questions about a codebase, **plan** a change, then let an **agent** build and test it.
6. Teach Copilot your team's conventions with **custom instructions**.
7. (Optional, preview, if MCP is allowed) Bring in Microsoft Learn docs with a **Copilot connector** or an **MCP server**.

**Time budget:** 90–120 minutes plus prerequisite setup; this is a planning allowance, not a measured beginner completion time. **Level:** new to Copilot, with basic C# and terminal knowledge. Optional preview features are not required.

## The sample app

This repository contains a small, runnable .NET 10 solution that every exercise uses:

The forecasts are randomly generated **synthetic data**, not a live weather service. The Customer exercises must also use fictional people and addresses.

| Project | What it is | URL when running |
| --- | --- | --- |
| `src/BackEnd` | ASP.NET Core minimal API with a 5-day forecast for Brisbane, Sydney, and Melbourne, plus interactive API docs (OpenAPI + Scalar) | http://localhost:5100/weatherforecast and http://localhost:5100/scalar |
| `src/FrontEnd` | Blazor Web App (interactive server) that shows the forecasts in a table | http://localhost:5200 |
| `tests/BackEnd.Tests` | xUnit tests for the API | `dotnet test` |

Want a shorter, presenter-led version? See the [live demo walkthrough](demo/walkthrough.md) (Demo 1: Copilot foundations, Demo 2: agents and custom instructions).

---

## Part 0 — Prerequisites (do this before the session)

1. **An approved GitHub account with Copilot access.** For company training, ask your administrator to assign access and confirm agents, models, and network access are allowed. Copilot Free is an option for eligible personal learning, subject to allowances; do not use it to bypass company policy. Model and preview-feature availability can differ by plan and policy.
2. **VS Code 1.146 or later** — <https://code.visualstudio.com/download>. Check with **Help > About**. GitHub Copilot is built into VS Code; you don't need to install a separate Copilot extension.
3. **.NET SDK 10.0.404** — <https://dotnet.microsoft.com/download/dotnet/10.0>. Install this version side by side with other SDKs. Check in a terminal:

   ```powershell
   dotnet --list-sdks
   ```

   The list must include `10.0.404`. `global.json` selects that exact stable SDK because Blazor's SDK-supplied assets participate in the package lock. Install the **SDK**, not only the runtime; .NET 11 alone does not satisfy this baseline. After opening the repo, `dotnet --version` should report `10.0.404`.

   > .NET 8 (used by the original module) reached end of support on 10 November 2026. Use .NET 10 (LTS, supported to November 2028).

4. **C# tooling** — this repo recommends **C# Dev Kit**, installed from the Extensions view (`Ctrl+Shift+X`). Company use has [licensing conditions](https://marketplace.visualstudio.com/items/ms-dotnettools.csdevkit/license), including classroom-training and qualifying open-source exceptions; a Copilot seat does not automatically grant a Dev Kit licence for ordinary enterprise development. Confirm eligibility with your administrator. Alternatively, use the **C#** extension (`ms-dotnettools.csharp`) and the `dotnet` commands below; Dev Kit's solution/test UI is optional.
5. **Git** — <https://git-scm.com/downloads>.

> [!IMPORTANT]
> Start with this public sample, not a company production repository. Never paste credentials, real customer details, or restricted code into prompts. Review generated code and terminal commands; instructions and passing tests are not security or production-readiness guarantees. Trust only a repository you have inspected, and approve MCP servers separately before enabling them.

## Part 1 — Get the sample running locally

1. Clone and open the repository:

   ```powershell
   git clone https://github.com/anarchitect/using-github-copilot-with-csharp-vscode.git
   cd using-github-copilot-with-csharp-vscode
   code .
   ```

2. Inspect the repository, then, if you trust it, select **Yes, I trust the authors** when VS Code asks. Agents are restricted in Restricted Mode. Open a dedicated training branch so your work is easy to review:

   ```powershell
   git switch -c training/copilot-basics
   ```
3. Open the integrated terminal (`` Ctrl+` ``) and build and test:

   ```powershell
   dotnet build
   dotnet test
   ```

4. Start the **API** in that terminal:

   ```powershell
   dotnet run --project src/BackEnd
   ```

5. Open a **second** terminal (select **+** in the terminal panel) and start the **web app**:

   ```powershell
   dotnet run --project src/FrontEnd
   ```

6. Open <http://localhost:5200> and select **Weather**. Then open <http://localhost:5100/scalar> to see the API docs.

> **✅ Checkpoint 1:** The unchanged starter reports **15 passed**, the Weather page shows a 15-row table (5 days × 3 cities), and Scalar lists the **Weather** endpoints.
> Stop **both** apps with `Ctrl+C` before exercises that edit or rebuild them. On Windows, running apps can lock build output; after an edit, an old process also serves old code.

For a repeatable HTTP smoke check, stop both apps and run `pwsh -File .\demo\Test-Demo.ps1` (PowerShell 7). It starts its own processes, checks the API, Scalar, stylesheet, and server-rendered Weather table, then stops only those processes. Logs are saved under `TestResults\smoke`. It does **not** test browser interactivity or Copilot UI. Ports busy? Stop your own apps or pass `-BackEndPort 15100 -FrontEndPort 15200`.

## Part 2 — Sign in and start a Copilot harness session

1. **Sign in:** select **Sign in to use Copilot** in the Copilot menu in the Status Bar, or use the **Accounts** menu in the Activity Bar > **Sign in with GitHub to use GitHub Copilot**.
2. **Open the Chat view:** `Ctrl+Alt+I`. Start a new chat with `Ctrl+N` while the Chat view has focus.
3. In the chat input you'll find four pickers. Set them like this for the whole module:

   | Picker | Choose | Why |
   | --- | --- | --- |
   | **Session Target** | **Copilot** | The Copilot harness. It's the default, but check it — do **not** pick *Local*. |
   | **Agent** | **Ask** or **Agent** (each exercise tells you which) | *Ask* answers questions; *Agent* edits files and runs commands. Type `/plan` for planning. |
   | **Model** | **Copilot Fusion** or an available **Auto** option | Availability depends on your account and policy; the exercises do not require a particular model. |
   | **Permissions** | **Manual permissions** | Review requested actions and any approval scope. Existing approvals and read-only actions can run without a new prompt. |

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

1. Ensure `Customer.cs` has a use of `Name` to update. For example, add `public string GetDisplayNameForDemo() => Name;` inside the class if it has no such reference.
2. Change the **property declaration** from `Name` to `FullName` by typing, leaving the method's reference unchanged. Do not use the symbol-rename command yet.
3. Look for an **arrow in the editor gutter** pointing at the method's reference to `Name`.
4. Press `Tab` to jump to the suggestion, and `Tab` again to accept it.

> **✅ Checkpoint 4:** The rename is applied everywhere in the file.
> No gutter arrow? Check `github.copilot.nextEditSuggestions.enabled`. Suggestions are not guaranteed. Complete the reference edit manually, or undo the rename and use **Rename Symbol** (`F2`), then run `dotnet build`. A missing suggestion is not a failed build or a reason to block the class.

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

1. Stop both running apps with `Ctrl+C`. Implement the plan from Exercise 5, or start a new chat with **Agent** selected and paste:

   ```text
   In the BackEnd project:
   1. Use the existing Customer class and its FullName and Email properties. Add a singleton CustomerService registered with dependency injection that stores customers in memory, generates unique IDs, and handles concurrent requests safely.
   2. Add CRUD endpoints under /customers in a separate CustomerEndpoints.cs file using a route group. Return 400 for an invalid email and 404 when a customer isn't found.
   3. Add xUnit tests in tests/BackEnd.Tests for each endpoint, including the 400 and 404 cases. Isolate each test's in-memory state.
   4. Run dotnet build and dotnet test, and fix any failures.
   5. Finish with a short summary of what you changed.
   ```

2. Watch the agent work. With **Manual permissions**, review approval requests for commands such as `dotnet test`. An action already covered by an approval might not prompt again. Prefer a one-time approval while learning; select **Skip** if a command is not appropriate.
3. Review the changed files in the chat's changes list. **Keep** what you like and **Undo** what you don't.

> **✅ Checkpoint 8:** `dotnet test` passes with more than the starter's 15 tests. Restart the API with `dotnet run --project src/BackEnd`; <http://localhost:5100/customers> returns `[]` or a JSON list. In-memory customers disappear whenever the API restarts; this is a learning app, not durable storage.

> [!TIP]
> Stay with **Manual permissions** for the workshop. **Assisted permissions** uses a probabilistic AI judge, not a security boundary; consider it only after your company's policy allows it. Avoid the **Allow all** permission level and **Autopilot** mode while learning.

## Exercise 7 — Add a page to the front end (≈10 min)

Stop both apps again before the agent builds. With **Agent** selected:

```text
In the FrontEnd project, add a Customers page at /customers that lists customers from the BackEnd /customers endpoint in a table, with a form to add a customer. Follow the same pattern as WeatherForecastClient and Weather.razor, add a nav link in MainLayout.razor, and run dotnet build.
```

Restart the API (`dotnet run --project src/BackEnd`) and, in a second terminal, the FrontEnd (`dotnet run --project src/FrontEnd`). Open <http://localhost:5200/customers>.

> **✅ Checkpoint 9:** Add a fictional customer (for example, `Alex Example`, `alex@example.com`), refresh and confirm it is still listed while the API stays running. An invalid email must be rejected, not silently saved.

> [!NOTE]
> The FrontEnd is a Blazor **server** app, so the browser never calls the API directly — no CORS setup is needed. If the agent suggests adding CORS, ask it why.

## Exercise 8 — Custom instructions (≈10 min)

Custom instructions are Markdown files that Copilot reads automatically, so you don't have to repeat your conventions in every prompt.

1. Create `.github/copilot-instructions.md` (applies to the whole repository):

   ```markdown
   # Project guidelines
   - This is a .NET 10 solution: an ASP.NET Core minimal API (src/BackEnd) and a Blazor Web App (src/FrontEnd).
   - Use file-scoped namespaces, nullable reference types, and records for DTOs.
   - Add new feature endpoints in *Endpoints.cs files using MapGroup; do not relocate the starter Weather endpoints as an unrelated change.
   - Every new endpoint needs xUnit tests in tests/BackEnd.Tests.
   - Explain changes in simple language; the team is new to C#.
   ```

2. Create `.github/instructions/csharp.instructions.md` (applies only to matching files):

   ```markdown
   ---
   applyTo: "**/*.cs"
   ---
   - Use async/await for actual I/O and pass CancellationToken where available. Do not add fake async wrappers to in-memory work.
   - Prefer guard clauses and return early.
   - Add XML documentation comments to public members.
   ```

3. Stop both apps, start a **new** chat (Agent), and ask:

   ```text
   Add a GET /customers/search?name= endpoint that searches customers by FullName, with xUnit tests. Do not change the Weather endpoints. Run dotnet build and dotnet test.
   ```

4. Review the diff for a separate endpoints file, appropriate async usage, XML docs, and new tests. To confirm discovery, run **Chat: Open Customizations** and select **Instructions**. Instructions guide the model; they are not enforced rules. After testing, restart both apps before trying the new route.

> **✅ Checkpoint 10:** The new endpoint follows your conventions without you mentioning them in the prompt.

> [!NOTE]
> The Copilot harness also reads `AGENTS.md` files. **Prompt files (`*.prompt.md`) aren't loaded in Copilot harness sessions** — convert reusable prompts to [agent skills](https://code.visualstudio.com/docs/agent-customization/migrate-customizations) instead.

## Exercise 9 (optional, preview, if MCP is allowed) — Bring in Microsoft Learn docs

Give Copilot up-to-date .NET documentation. Choose **one** option.

**Option A: Copilot connector (preview, no configuration)**

Use a GitHub.com account. For Business/Enterprise accounts, your administrator must allow **Editor preview features** and the connector/MCP access. Skip this optional exercise if company policy blocks it; a different paid plan is not the automatic remedy.

1. Make sure Session Target is **Copilot**.
2. Run **Chat: Open Customizations** from the Command Palette (`Ctrl+Shift+P`) and select **Browse**.
3. Choose **Copilot Connectors**, find **Microsoft Learn Docs**, select **Connect**, and complete authorization.

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
| Personal agents stored in your VS Code profile | Move them to `~/.copilot/agents` (or `.github/agents` for the team). |
| Personal instruction files stored in your VS Code profile | Move modular files to `~/.copilot/instructions`; general personal instructions can use `~/.copilot/copilot-instructions.md`. Shared instructions belong in `.github/instructions` or `.github/copilot-instructions.md`, not the agents directory. |
| Hooks | Review them manually; tool names and formats differ. |

**Quick migration check:** run **Chat: Open Customizations**, select **Migrations**, and review each detected item. Full guides:
[Move from the Local to the Copilot harness](https://code.visualstudio.com/docs/agents/run/move-to-copilot) ·
[Migrate customizations](https://code.visualstudio.com/docs/agent-customization/migrate-customizations).

> Administrators: `vscode.temporaryDefaultToLocalHarness` can keep Local as the temporary default only while Local remains allowed. It does not override `vscode.temporaryAllowLocalHarness: false` or extend availability beyond removal.

## Troubleshooting

| Problem | Fix |
| --- | --- |
| No Copilot features or chat | Sign in (Status Bar Copilot menu). Check that `chat.disableAIFeatures` isn't enabled. |
| **Copilot** missing from Session Target | Update VS Code to 1.146+. Your organization may restrict harnesses — ask your admin. |
| Agent can't edit files or run commands | Trust the workspace (**Workspaces: Manage Workspace Trust**). |
| `dotnet` not found or required SDK not found | Install SDK **10.0.404**, then restart VS Code so the terminal picks up `PATH`. Use `dotnet --list-sdks` to confirm it is installed. |
| A newer SDK is installed but the lab refuses it | Install 10.0.404 side by side. Maintainers must update `global.json` and package locks together when adopting a newer SDK. |
| Build reports a file in use (`MSB3021` / `MSB3027`) | Stop both `dotnet run` terminals with `Ctrl+C`, then rebuild and restart. |
| NuGet restore fails on a company network | Ask your administrator about approved NuGet feeds, proxy and certificates; do not disable certificate validation. |
| Weather page says it can't reach the API | Start `src/BackEnd` first; it must run on `http://localhost:5100`. |
| Port already in use | Stop only your own app (`Ctrl+C`). If changing the API port in `launchSettings.json`, also update `BackEndUrl` in `src/FrontEnd/appsettings.json`; if changing the web port, use the new browser URL. |
| No ghost text | Check `github.copilot.enable` for C#, and your Copilot Free monthly allowance. |
| Agent asks for approval too often | Use the **Allow** dropdown to allow a command for the session, or try **Assisted permissions**. |
| Chat gets slow or loses track in a long session | Type `/compact`, or start a new chat. |
| Connectors don't appear in **Browse** | Check Session Target **Copilot**, GitHub.com sign-in, discovery/connector settings, and company **Editor preview features** / MCP policy. Skip the optional step if blocked. |

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
