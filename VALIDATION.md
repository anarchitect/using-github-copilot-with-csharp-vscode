# Validation report

**Validated on:** 18 November 2026
**Source page:** [microsoft/Mastering-GitHub-Copilot-for-Paired-Programming › Using-GitHub-Copilot-with-CSharp](https://github.com/microsoft/Mastering-GitHub-Copilot-for-Paired-Programming/tree/main/Using-GitHub-Copilot-with-CSharp)
**Target:** local VS Code (1.146) with the **Copilot harness**. Codespaces and the Local harness are out of scope.

**Status key:** ✅ Current · ⚠️ Changed (still possible, but the steps, names, or defaults are different) · ❌ Outdated or removed · ➖ Out of scope for this repo

> **Method.** Each claim was checked against official VS Code docs (code.visualstudio.com), GitHub Docs and the GitHub Changelog, and Microsoft Learn / .NET docs, as published on or before the date above. Direct API access to the `microsoft` org was blocked by SAML SSO, so the source page was reviewed through its public GitHub page and indexed copies. Copilot output isn't deterministic, so "expected results" in the exercises describe the shape of the result, not exact code.

## Headline: the Local harness is retiring

| Item | Status | Evidence |
| --- | --- | --- |
| The Local harness (VS Code's original built-in agent harness) | ❌ Deprecated in VS Code 1.146; **removed in February 2027** | [Changelog, 16 Nov 2026](https://github.blog/changelog/2026-11-16-vs-code-retires-the-local-harness-in-february-2027/) · [Choose an agent harness](https://code.visualstudio.com/docs/agents/run/agent-harnesses) |
| The Copilot harness (built on the Copilot SDK, shared with Copilot CLI and the GitHub Copilot app; default for new sessions since 1.143; runs locally) | ✅ Used throughout this repo | [VS Code 1.146 release notes](https://code.visualstudio.com/updates/v1_146) · [Move to the Copilot harness](https://code.visualstudio.com/docs/agents/run/move-to-copilot) |

> Note: the changelog says the Local harness is removed in **February 2027**. It doesn't name a specific day.

## Source module, item by item

| # | Source claim or step | Status | What's current (Nov 2026) | Evidence |
| --- | --- | --- | --- | --- |
| 1 | Create a GitHub Codespace from the `github/dotnet-codespaces` template | ➖ | Replaced with a local clone + local `dotnet` CLI and a self-contained sample in this repo. Codespaces itself still exists but isn't part of a local-VS-Code training. | [Set up Copilot in VS Code](https://code.visualstudio.com/docs/setup/copilot) |
| 2 | Sample targets .NET 8 | ❌ | .NET 8 (and 9) reached end of support on **10 Nov 2026**. Use **.NET 10 (LTS, to Nov 2028)**. The upstream `dotnet-codespaces` sample also moved to .NET 10. | [.NET blog: .NET 8 and 9 end of support](https://devblogs.microsoft.com/dotnet/dotnet-8-9-end-of-support/) · [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) |
| 3 | Install/enable the Copilot extensions and sign in | ⚠️ | Copilot is built into VS Code. Sign in from the Status Bar Copilot menu or the Accounts menu. Without a plan, sign-in enrolls eligible accounts in **Copilot Free**. | [Set up Copilot in VS Code](https://code.visualstudio.com/docs/setup/copilot) |
| 4 | Comment-driven code completion (ghost text, `Tab` to accept) — e.g. a `Customer` class with an email validator | ✅ | Unchanged. Called *inline suggestions*. | [Inline suggestions](https://code.visualstudio.com/docs/editing/ai-powered-suggestions) |
| 5 | Next Edit Suggestions (gutter arrow; enable if missing) | ✅ | Unchanged. `github.copilot.nextEditSuggestions.enabled` defaults to `true`. | [Inline suggestions](https://code.visualstudio.com/docs/editing/ai-powered-suggestions) · [AI settings reference](https://code.visualstudio.com/docs/agents/reference/ai-settings) |
| 6 | Inline chat with `Ctrl+I` | ✅ | Unchanged. Review with **Keep** / **Undo**. | [Inline chat](https://code.visualstudio.com/docs/chat/inline-chat) |
| 7 | "`Ctrl+Shift+I` opens chat" | ❌ | **Open the Chat view: `Ctrl+Alt+I`** (macOS `Ctrl+Cmd+I`). On Windows, `Ctrl+Shift+I` now switches to agents in the Chat view. | [AI features cheat sheet](https://code.visualstudio.com/docs/agents/reference/ai-features-cheat-sheet) |
| 8 | Chat slash commands `/explain`, `/doc`, `/fix`, `/generate`, `/help` | ⚠️ / ❌ | Copilot harness slash commands are `/plan`, `/compact`, `/fork`, and `/<skill-name>` (type `/` to list them). `/generate` is gone. Inline chat still recognizes `/doc`, `/explain`, `/fix`, `/tests` if typed, but they no longer appear in autocomplete. **This repo uses plain-English prompts instead.** | [AI features cheat sheet](https://code.visualstudio.com/docs/agents/reference/ai-features-cheat-sheet) · [microsoft/vscode#330866](https://github.com/microsoft/vscode/issues/330866) |
| 9 | Ask / Edit / Agent **modes** | ⚠️ | Now three separate choices: **Session Target** (harness: Copilot), **agent** (Ask, Agent; plus Plan via `/plan`), and **permission level**. **Edit mode is deprecated** — use Agent. | [Choose an agent harness](https://code.visualstudio.com/docs/agents/run/agent-harnesses) · [Plan work with agents](https://code.visualstudio.com/docs/agents/run/planning) |
| 10 | "Agent mode is selected by default; you can choose your model" | ⚠️ | The **Copilot harness** is the default target. The model picker now offers **Copilot Fusion** (routes/orchestrates models; you pick an effort level) and Auto. | [Copilot Fusion changelog](https://github.blog/changelog/2026-10-28-route-and-orchestrate-models-with-github-copilot-fusion/) · [Choose a model](https://code.visualstudio.com/docs/agents/guides/choose-a-model) |
| 11 | Add context (files, folders), open editor tab used as context | ✅ | **Add Context** and `#`-mentions still work. | [AI features cheat sheet](https://code.visualstudio.com/docs/agents/reference/ai-features-cheat-sheet) |
| 12 | Agent multi-task prompt (Customer endpoint, CORS, DI, controllers, docs, xUnit tests) | ⚠️ | Still a good agent task. Simplified for beginners: minimal-API route group instead of controllers. **CORS removed** — the Blazor front end calls the API from the server, so CORS isn't needed. A **plan first** step (`/plan`) was added. | [Plan work with agents](https://code.visualstudio.com/docs/agents/run/planning) |
| 13 | Approving agent actions with *Continue* | ⚠️ | Permission levels: **Manual permissions** (default), **Assisted permissions** (LLM judge), Bypass approvals, Autopilot. In Manual, **Allow Once** is the default, with a dropdown to allow a command/site/path for longer. | [Manage approvals and permissions](https://code.visualstudio.com/docs/agents/run/approvals) |
| 14 | Repository instructions in `.github/copilot-instructions.md` | ✅ | Supported by the Copilot harness. `AGENTS.md` is also supported. | [Custom instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions) · [GitHub Docs](https://docs.github.com/copilot/how-tos/configure-custom-instructions-in-your-ide/add-repository-instructions-in-your-ide) |
| 15 | Path-specific `.github/instructions/*.instructions.md` with `applyTo` | ✅ | Supported. In Copilot sessions, `applyTo` targets the files the agent works with. | [Custom instructions reference](https://code.visualstudio.com/docs/agents/reference/custom-instructions-reference) |
| 16 | Reusable prompt files (`*.prompt.md`) | ❌ (Copilot harness) | Not loaded by the Copilot harness. Convert to **agent skills**. | [Migrate customizations](https://code.visualstudio.com/docs/agent-customization/migrate-customizations) |
| 17 | "Agent mode supports MCP" | ⚠️ | Still true, but the Copilot harness uses the portable **`mcpServers`** format in workspace-root `.mcp.json` or `~/.copilot/mcp-config.json`. `.vscode/mcp.json` (`servers` format) is the editor/Local format; **Chat: Open Customizations > Migrations** converts it. **Copilot connectors** (GitHub-managed MCP, e.g. Microsoft Learn) are new and in **preview**. | [MCP configuration reference](https://code.visualstudio.com/docs/agents/reference/mcp-configuration) · [Copilot connectors (preview)](https://code.visualstudio.com/docs/agent-customization/copilot-connectors) · [Microsoft Learn MCP Server](https://learn.microsoft.com/training/support/mcp) |

## New since the source module (included where relevant)

| Feature | Status | Used in | Evidence |
| --- | --- | --- | --- |
| Copilot harness + Session Target picker | GA, default | Part 2 | [Choose an agent harness](https://code.visualstudio.com/docs/agents/run/agent-harnesses) |
| Plan agent (`/plan`) with plan review card | GA | Exercise 5 | [Plan work with agents](https://code.visualstudio.com/docs/agents/run/planning) |
| Copilot Fusion | GA, all plans | Part 2 | [Changelog](https://github.blog/changelog/2026-10-28-route-and-orchestrate-models-with-github-copilot-fusion/) |
| Assisted permissions | Copilot harness | Exercise 6 tip | [Approvals](https://code.visualstudio.com/docs/agents/run/approvals#_permission-levels) |
| Copilot connectors | **Preview** | Exercise 9 | [Copilot connectors](https://code.visualstudio.com/docs/agent-customization/copilot-connectors) |
| Canvases | **Preview** | Learn more only | [Canvases](https://code.visualstudio.com/docs/agents/run/canvases) |

## Demo walkthrough (DEMO1 / DEMO2)

Source: [Azure-Samples/octocat-supply › demo-steps.md](https://github.com/Azure-Samples/octocat-supply/blob/main/demo/walkthroughs/general/demo-steps.md) (DEMO1 and DEMO2 only). Translated to this repo's C# sample in [demo/walkthrough.md](demo/walkthrough.md). No OctoCAT Supply code was copied.

| # | Original demo step | Status | Translation in this repo |
| --- | --- | --- | --- |
| D1 | `[Ask Mode]` project overview prompt | ⚠️ | Same prompt, using the **Ask** agent in a Copilot harness session. ("Ask Mode" → Ask agent.) |
| D2 | `make install` / `npm install`, `make dev` | ➖ | `dotnet build` (restores NuGet automatically) and `dotnet run --project src/BackEnd` / `src/FrontEnd`. |
| D3 | Open `order.ts`, `ordersRepo.ts`, `routes/order.ts`; add `// Count orders by branch` between async methods | ✅ (concept) | Open `WeatherForecast.cs`, `WeatherRepository.cs`, `Program.cs`; add `// Count forecasts by city` between `GetByCityAsync` and `GetCitiesAsync`; accept the inline suggestion. |
| D4 | `[Agent Mode]` "Add a retrieval endpoint … update the API documentation … Do not deploy or test yet" | ⚠️ | Same intent with the **Agent** agent; documentation = OpenAPI `WithName`/`WithSummary`/`WithDescription`. "Deploy" dropped (nothing to deploy locally). |
| D5 | Verify at `http://localhost:3000/api-docs` (Swagger) | ⚠️ | Verify at `http://localhost:5100/scalar` (ASP.NET Core OpenAPI + Scalar). Sample added `Microsoft.AspNetCore.OpenApi` and `Scalar.AspNetCore` for this. |
| D6 | `#codebase` and `#fetch <url>` in the deep-analysis prompt | ❌ (as written) | These are **Local harness** tool reference names; the docs say not to assume they map to Copilot harness tools. Rewritten as plain language ("analysis of this codebase", "use <URL> as a reference"); the agent searches the workspace and fetches the URL with approval. [Tools and context reference](https://code.visualstudio.com/docs/agents/reference/tools-reference) |
| D7 | Fetch W3Schools *TypeScript* best practices | ⚠️ | This sample is C#, so it uses Microsoft Learn's [.NET coding conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions). |
| D8 | Update `.github/copilot-instructions.md` | ✅ | Still the Copilot harness's repository instructions file. Also shown: **Chat: Open Customizations > Instructions > +**. [Custom instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions) |
| D9 | Optional persona added to the instructions file | ✅ | Kept as an optional, fun step, with a reminder to remove it afterwards. |
| D10 | `[Agent Mode]` code review against the instructions file | ✅ | Same prompt, plus "list findings first … run dotnet build and dotnet test" so beginners see verification. |

## Sample app verification

| Check | Result |
| --- | --- |
| `dotnet build WeatherApp.slnx` | Run by the [build-and-test workflow](.github/workflows/build.yml) on every push to `main` (.NET 10 SDK on `ubuntu-latest`). |
| `dotnet test WeatherApp.slnx` | Same workflow: 5 xUnit tests for the forecast endpoints, the 404 case, OpenAPI, and the °F conversion. |
