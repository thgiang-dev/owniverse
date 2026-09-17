# **SPRINT-01 — MVP Sprint Execution Plan**

**Document ID: SPRINT-01  
Document Type: MVP Sprint Execution Plan  
Product: OWNIVERSE  
Project: Character-Consistent Long-Range Story Generation  
Status: Draft v1 — Updated  
Primary Roles: Technical Lead / Full-Stack Developer / AI Engineer / Thesis Author / AI Coding Assistant  
Parent Documents: IMPLEMENT-01, IMPLEMENT-02, DEV-01  
Related Documents: PROD-01, PROD-02, ARCH-01, ARCH-02, DOMAIN-01, DOMAIN-02, AI-01, AI-02, AI-03, DATA-01, DATA-02, API-01, API-02, QA-01, QA-02, QA-03, QA-04, DEPLOY-01, DEPLOY-02, DEPLOY-03, RESEARCH-01, RESEARCH-02, RESEARCH-03, RESEARCH-04**

# **1. Purpose**

**SPRINT-01 chuyển IMPLEMENT-01 và IMPLEMENT-02 thành kế hoạch thực thi theo sprint cho MVP OWNIVERSE.**

**Tài liệu này được dùng để:**

- **chia phạm vi coding thành các sprint có dependency rõ;**

- **xác định mục tiêu và outcome của từng sprint;**

- **giữ Product Track và Research Track chạy song song nhưng không trộn trách nhiệm;**

- **cung cấp task boundary rõ cho developer và AI coding assistant;**

- **xác định Definition of Done trước khi chuyển sprint;**

- **giảm tình trạng “vibe coding” hoặc mở rộng scope không kiểm soát.**

**SPRINT-01 không thay thế các specification chi tiết.**

# **2. Sprint Philosophy**

**Mỗi sprint phải kết thúc bằng một working increment.**

**Không xem các task kiểu:**

> **Create table**
>
> **Create interface**
>
> **Create component**

**là outcome cuối cùng nếu chưa tạo được behavior usable.**

**Ưu tiên:**

> **Vertical Slice**
>
> **+**
>
> **Tests**
>
> **+**
>
> **Persistence**
>
> **+**
>
> **Reload**

# **3. Locked Implementation Stack**

**Toàn bộ sprint sử dụng stack đã khóa:**

> **Frontend**
>
> **Vue 3 + Vite + TypeScript**
>
> **Backend**
>
> **ASP.NET Core Web API**
>
> **Database**
>
> **PostgreSQL**
>
> **Queue**
>
> **RabbitMQ**
>
> **AI Worker**
>
> **Python**
>
> **Asset Storage**
>
> **IAssetStorage**
>
> **├── LocalFileAssetStorage — development**
>
> **└── S3AssetStorage — thesis demo / production-like**
>
> **Preferred S3-Compatible Provider**
>
> **Cloudflare R2**
>
> **Realtime**
>
> **SignalR — optional**
>
> **AI Contracts**
>
> **IImageGenerationService**
>
> **ITextGenerationService**
>
> **IConsistencyService**

**Không tự thay framework/provider giữa sprint.**

# **4. Locked AI Context Architecture**

**AI generation phải đi qua:**

> **Canonical Domain Data**
>
> **↓**
>
> **GenerationContextV1**
>
> **↓**
>
> **Immutable Context Snapshot**
>
> **↓**
>
> **Adapter**
>
> **↓**
>
> **Prompt Builder / Condition Builder**
>
> **↓**
>
> **Model Input**

**Raw prompt string không phải application contract chính.**

# **5. Context Priority**

**Conflict resolution:**

> **Canonical / Locked Identity**
>
> **↓**
>
> **Timeline / Scene-Specific State**
>
> **↓**
>
> **User Instruction**
>
> **↓**
>
> **Soft Context**
>
> **↓**
>
> **Model Assumption**

# **6. AI Implementation Order**

> **Mock**
>
> **↓**
>
> **StoryDiffusion**
>
> **↓**
>
> **SAMIM**

**Không tích hợp SAMIM trước khi baseline đủ ổn định và research gap được xác nhận.**

# **7. Sprint Overview**

> **Sprint 0 — Engineering Foundation**
>
> **Sprint 1 — Project & Story Core**
>
> **Sprint 2 — Character & Reference**
>
> **Sprint 3 — Scene & SceneCharacter**
>
> **Sprint 4 — Versioning & Generation Context Snapshot**
>
> **Sprint 5 — Async Generation + Mock AI**
>
> **Sprint 6 — Candidate, Regeneration & History**
>
> **Sprint 7 — Real Baseline + Validation**
>
> **Sprint 8 — Stale Context + AI-Assisted Content**
>
> **Sprint 9 — SAMIM Research Integration**
>
> **Sprint 10 — QA, Deployment & Thesis Demo**

# **8. Sprint Dependency Rule**

**Một sprint chỉ bắt đầu đầy đủ khi dependency critical của sprint trước đạt Definition of Done.**

**Có thể làm research song song nếu không thay đổi Product contracts chưa ổn định.**

# **9. Sprint 0 — Engineering Foundation**

## **Goal**

**Tạo nền tảng repository, runtime và engineering conventions để toàn bộ development về sau dùng chung.**

# **10. Sprint 0 — Frontend Tasks**

> **\[ \] Initialize Vue 3**
>
> **\[ \] Configure Vite**
>
> **\[ \] Enable TypeScript**
>
> **\[ \] Add router**
>
> **\[ \] Create app shell**
>
> **\[ \] Create shared API client**
>
> **\[ \] Add JSend response handling**
>
> **\[ \] Add standard errorKey handling**
>
> **\[ \] Add environment configuration**

# **11. Sprint 0 — Backend Tasks**

> **\[ \] Initialize ASP.NET Core Web API**
>
> **\[ \] Create solution/module structure**
>
> **\[ \] Configure DI**
>
> **\[ \] Configure exception handling**
>
> **\[ \] Configure JSend envelopes**
>
> **\[ \] Configure structured logging**
>
> **\[ \] Add health endpoint**
>
> **\[ \] Add readiness foundation**

# **12. Sprint 0 — PostgreSQL Tasks**

> **\[ \] Configure PostgreSQL**
>
> **\[ \] Configure persistence/ORM**
>
> **\[ \] Create migration framework**
>
> **\[ \] Create initial migration**
>
> **\[ \] Verify clean database setup**

# **13. Sprint 0 — RabbitMQ Tasks**

> **\[ \] Configure RabbitMQ connection**
>
> **\[ \] Add queue infrastructure abstraction**
>
> **\[ \] Add publisher skeleton**
>
> **\[ \] Add consumer skeleton**
>
> **\[ \] Add health/readiness check**

**RabbitMQ không phải Job source of truth.**

# **14. Sprint 0 — Asset Storage Tasks**

> **\[ \] Define IAssetStorage**
>
> **\[ \] Implement LocalFileAssetStorage**
>
> **\[ \] Create local asset directory/volume config**
>
> **\[ \] Add upload/read/delete primitives as needed**
>
> **\[ \] Add storage health check**

**Không cần S3 hoàn chỉnh trong Sprint 0.**

# **15. Sprint 0 — Python Worker Tasks**

> **\[ \] Create Python worker project**
>
> **\[ \] Configure RabbitMQ client**
>
> **\[ \] Add job consumer foundation**
>
> **\[ \] Add adapter registry foundation**
>
> **\[ \] Add structured logging**
>
> **\[ \] Add worker health/readiness**

# **16. Sprint 0 — AI Contract Foundation**

**Create:**

> **IImageGenerationService**

**và worker-side equivalent adapter contract.**

**Chưa tích hợp model thật.**

# **17. Sprint 0 — Repository & CI**

> **\[ \] frontend/**
>
> **\[ \] backend/**
>
> **\[ \] ai-worker/**
>
> **\[ \] tests/**
>
> **\[ \] deployment/**
>
> **\[ \] docs/**
>
> **\[ \] scripts/**
>
> **\[ \] .github/**
>
> **\[ \] base CI**

**CI minimum:**

> **Frontend build**
>
> **Backend build**
>
> **Backend tests**
>
> **Python tests**

# **18. Sprint 0 — Exit Criteria**

**Sprint 0 Done khi:**

> **Frontend starts**
>
> **API starts**
>
> **PostgreSQL connects**
>
> **RabbitMQ connects**
>
> **Worker connects**
>
> **Local storage works**
>
> **Health checks pass**
>
> **Basic CI passes**

# **19. Sprint 0 — Research Parallel Track**

**Research có thể:**

> **\[ \] prepare Python environment**
>
> **\[ \] prepare baseline repository**
>
> **\[ \] prepare ViStoryBench subset**
>
> **\[ \] verify GPU/Colab environment**

**Chưa cần tích hợp với Product.**

# **20. Sprint 1 — Project & Story Core**

## **Goal**

**Tạo workspace canonical đầu tiên có thể persist và reload.**

# **21. Sprint 1 — Identity / Access**

> **\[ \] current user abstraction**
>
> **\[ \] project ownership rule**
>
> **\[ \] authorization integration**

**Không cần enterprise IAM.**

# **22. Sprint 1 — Project Domain**

> **\[ \] Project entity**
>
> **\[ \] revision number**
>
> **\[ \] domain validation**
>
> **\[ \] project status if in scope**

# **23. Sprint 1 — Project Persistence**

> **\[ \] projects table**
>
> **\[ \] migration**
>
> **\[ \] repository/query path**
>
> **\[ \] ownership persistence**

# **24. Sprint 1 — Project Application/API**

> **\[ \] CreateProject**
>
> **\[ \] GetProject**
>
> **\[ \] ListProjects**
>
> **\[ \] UpdateProject**
>
> **\[ \] revision conflict handling**

# **25. Sprint 1 — Project Frontend**

> **\[ \] Project List**
>
> **\[ \] Create Project**
>
> **\[ \] Open Project**
>
> **\[ \] Project basic settings**

# **26. Sprint 1 — Story**

> **\[ \] Story entity/model**
>
> **\[ \] Story Core**
>
> **\[ \] InitializeStory**
>
> **\[ \] GetStory**
>
> **\[ \] UpdateStoryCore**
>
> **\[ \] migration**
>
> **\[ \] API**
>
> **\[ \] UI**

# **27. Sprint 1 — Concurrency**

**Implement optimistic concurrency for critical updates.**

**Expected failure:**

> **409**
>
> **REVISION_CONFLICT**

# **28. Sprint 1 — Tests**

> **\[ \] project create/read/update**
>
> **\[ \] story initialize/update**
>
> **\[ \] ownership**
>
> **\[ \] revision conflict**
>
> **\[ \] reload persistence**

# **29. Sprint 1 — Exit Criteria**

> **Create Project**
>
> **→ Initialize Story**
>
> **→ Edit**
>
> **→ Reload**
>
> **→ Same canonical state**

# **30. Sprint 1 — Research Parallel Track**

> **\[ \] run first baseline smoke test**
>
> **\[ \] verify dataset loading**
>
> **\[ \] record environment versions**

# **31. Sprint 2 — Character & Reference**

## **Goal**

**Xây Character identity foundation và asset reference workflow.**

# **32. Sprint 2 — Character Domain**

> **\[ \] Character entity**
>
> **\[ \] stable identity fields**
>
> **\[ \] canonical appearance**
>
> **\[ \] locked attributes**
>
> **\[ \] revision handling**

# **33. Sprint 2 — Character Persistence/API**

> **\[ \] characters table**
>
> **\[ \] migration**
>
> **\[ \] CreateCharacter**
>
> **\[ \] GetCharacter**
>
> **\[ \] ListCharacters**
>
> **\[ \] UpdateCharacter**

# **34. Sprint 2 — Character Frontend**

> **\[ \] Character List**
>
> **\[ \] Character Detail**
>
> **\[ \] Character Editor**

# **35. Sprint 2 — Asset Metadata**

> **\[ \] assets table**
>
> **\[ \] Asset metadata model**
>
> **\[ \] storage key generation**
>
> **\[ \] media type/size metadata**
>
> **\[ \] ownership**

**Binary tiếp tục dùng IAssetStorage.**

# **36. Sprint 2 — Character Reference**

> **\[ \] character_references**
>
> **\[ \] upload reference**
>
> **\[ \] list references**
>
> **\[ \] select canonical reference**
>
> **\[ \] preview in frontend**

# **37. Sprint 2 — Storage Failure Handling**

**Không tạo reference hợp lệ nếu binary persist thất bại.**

**Metadata và binary phải nhất quán ở mức application contract.**

# **38. Sprint 2 — Tests**

> **\[ \] create/update Character**
>
> **\[ \] upload reference**
>
> **\[ \] select canonical reference**
>
> **\[ \] ownership**
>
> **\[ \] invalid upload**
>
> **\[ \] storage failure**
>
> **\[ \] reload**

# **39. Sprint 2 — Exit Criteria**

> **Create Character**
>
> **→ Upload Reference**
>
> **→ Select Canonical Reference**
>
> **→ Reload**

# **40. Sprint 2 — Research Parallel Track**

> **\[ \] reproduce StoryDiffusion baseline**
>
> **\[ \] generate initial recurring-character sequences**
>
> **\[ \] begin failure logs**

# **41. Sprint 3 — Scene & SceneCharacter**

## **Goal**

**Xây story sequence và scene-specific character state.**

# **42. Sprint 3 — Scene Domain**

> **\[ \] Scene entity**
>
> **\[ \] order/position**
>
> **\[ \] description**
>
> **\[ \] intent**
>
> **\[ \] environment/context fields**

# **43. Sprint 3 — SceneCharacter**

> **\[ \] SceneCharacter relation**
>
> **\[ \] outfit**
>
> **\[ \] emotion**
>
> **\[ \] action**
>
> **\[ \] pose/action intent**
>
> **\[ \] temporary appearance**
>
> **\[ \] other scene-local state**

**Không mutate Character stable identity.**

# **44. Sprint 3 — Scene Application/API**

> **\[ \] CreateScene**
>
> **\[ \] UpdateScene**
>
> **\[ \] ReorderScenes**
>
> **\[ \] AssignCharacterToScene**
>
> **\[ \] RemoveCharacterFromScene**
>
> **\[ \] UpdateSceneCharacterState**

# **45. Sprint 3 — Scene Frontend**

> **\[ \] Scene List**
>
> **\[ \] Scene Editor**
>
> **\[ \] Reorder UI**
>
> **\[ \] Character assignment**
>
> **\[ \] SceneCharacter state editor**

# **46. Sprint 3 — Active Characters**

**Scene phải trả về active-character set rõ ràng.**

**Generation về sau chỉ lấy Character relevant cho Scene.**

# **47. Sprint 3 — Tests**

> **\[ \] create Scene**
>
> **\[ \] reorder**
>
> **\[ \] assign recurring Character**
>
> **\[ \] multi-character Scene**
>
> **\[ \] update scene state**
>
> **\[ \] remove assignment**
>
> **\[ \] reload**

# **48. Sprint 3 — Exit Criteria**

**Story có nhiều Scene và recurring Characters với state riêng từng Scene.**

# **49. Sprint 3 — Research Parallel Track**

> **\[ \] baseline recurring-character tests**
>
> **\[ \] multi-character tests**
>
> **\[ \] long-range reappearance tests**
>
> **\[ \] categorize initial failure patterns**

# **50. Sprint 4 — Versioning & Generation Context Snapshot**

## **Goal**

**Tạo reproducibility foundation trước khi async generation bắt đầu.**

# **51. Sprint 4 — Versioning**

**Implement MVP versions:**

> **StoryCoreVersion**
>
> **CharacterVersion**
>
> **SceneVersion**

# **52. Sprint 4 — Versioning Rules**

> **\[ \] historical version immutable**
>
> **\[ \] canonical edit creates new version where required**
>
> **\[ \] version creation transaction-safe**
>
> **\[ \] generation can reference exact version**

# **53. Sprint 4 — GenerationContextV1**

**Implement:**

> **GenerationContextV1**

**Minimum structure:**

> **schemaVersion**
>
> **story**
>
> **scene**
>
> **characters\[\]**
>
> **continuity**
>
> **style**
>
> **userInstruction**
>
> **generationOptions**

# **54. Sprint 4 — Character Context**

**Each active Character context includes conceptually:**

> **characterId**
>
> **stableIdentity**
>
> **canonicalReferences**
>
> **sceneState**

# **55. Sprint 4 — GenerationContextBuilder**

**Implement backend/Application builder.**

**Responsibilities:**

> **Resolve Story**
>
> **Resolve Scene**
>
> **Resolve Active Characters**
>
> **Resolve Canonical References**
>
> **Resolve Scene State**
>
> **Resolve Relevant Continuity**
>
> **Apply Priority**
>
> **Build GenerationContextV1**

# **56. Sprint 4 — Priority Tests**

**Must verify:**

> **Locked identity \> scene state**
>
> **Scene state \> user instruction where conflict concerns canonical constraints**
>
> **User instruction \> soft context**
>
> **Soft context \> model assumption**

**More precisely:**

> **Canonical / Locked Identity**
>
> **↓**
>
> **Timeline / Scene State**
>
> **↓**
>
> **User Instruction**
>
> **↓**
>
> **Soft Context**
>
> **↓**
>
> **Model Assumption**

# **57. Sprint 4 — Context Snapshot**

**Implement:**

> **context_snapshots**
>
> **context_snapshot_dependencies**

**Snapshot immutable after creation for generation.**

# **58. Sprint 4 — Snapshot Provenance Foundation**

**Store:**

> **contextSchemaVersion**
>
> **createdAt**
>
> **dependency references**

**Adapter/model fields come later.**

# **59. Sprint 4 — Tests**

**Required:**

> **Character v1**
>
> **↓**
>
> **Snapshot S1**
>
> **↓**
>
> **Character edited to v2**
>
> **↓**
>
> **S1 remains unchanged**

**Also:**

> **\[ \] only active characters included**
>
> **\[ \] correct canonical refs included**
>
> **\[ \] scene state included**
>
> **\[ \] unrelated project data excluded**

# **60. Sprint 4 — Exit Criteria**

**Backend có thể build và persist GenerationContextV1 Snapshot mà chưa cần AI.**

# **61. Sprint 4 — Research Parallel Track**

> **\[ \] continue failure analysis**
>
> **\[ \] align product context fields with baseline input needs**
>
> **\[ \] do not rewrite product schema around one model**

# **62. Sprint 5 — Async Generation + Mock AI**

## **Goal**

**Chứng minh toàn bộ generation architecture không phụ thuộc real AI model.**

# **63. Sprint 5 — Generation Domain**

**Implement:**

> **GenerationJob**
>
> **GenerationAttempt**
>
> **Candidate**

# **64. Sprint 5 — Job Persistence**

**Before queue:**

> **Create Context Snapshot**
>
> **Create Job**
>
> **COMMIT**

**Then:**

> **Publish RabbitMQ message**

# **65. Sprint 5 — Queue Message**

**Recommended:**

> **jobId**
>
> **attemptId**
>
> **operationType**

**Worker load Snapshot từ durable storage.**

# **66. Sprint 5 — Worker Execution**

> **Receive Job**
>
> **↓**
>
> **Load Snapshot**
>
> **↓**
>
> **Resolve Adapter**
>
> **↓**
>
> **Build Adapter Input**
>
> **↓**
>
> **Generate**
>
> **↓**
>
> **Store Asset**
>
> **↓**
>
> **Persist Candidate**
>
> **↓**
>
> **Update Job**

# **67. Sprint 5 — Mock Adapter**

**Implement:**

> **MockImageGenerationAdapter**

**Supports:**

> **success**
>
> **delay**
>
> **failure**
>
> **warning**
>
> **deterministic fake output**

# **68. Sprint 5 — Prompt Boundary**

**Mock flow vẫn phải giữ architecture:**

> **GenerationContextV1**
>
> **↓**
>
> **Snapshot**
>
> **↓**
>
> **Adapter**
>
> **↓**
>
> **Simulated/Adapter Input**

**Không tạo technical prompt ở frontend.**

# **69. Sprint 5 — Frontend Generation**

> **Generate**
>
> **↓**
>
> **202 Accepted**
>
> **↓**
>
> **jobId**
>
> **↓**
>
> **REST polling**
>
> **↓**
>
> **Candidate visible**

**SignalR chưa bắt buộc.**

# **70. Sprint 5 — Job State**

**Implement explicit lifecycle theo API-02.**

**Không dùng arbitrary status strings.**

# **71. Sprint 5 — Failure Handling**

**Must cover:**

> **queue publish failure**
>
> **worker failure**
>
> **mock model failure**
>
> **asset storage failure**
>
> **database persist failure**

# **72. Sprint 5 — Tests**

> **\[ \] 202 + jobId**
>
> **\[ \] Job persists**
>
> **\[ \] RabbitMQ consume**
>
> **\[ \] Worker success**
>
> **\[ \] Worker failure**
>
> **\[ \] Asset persist**
>
> **\[ \] Candidate persist**
>
> **\[ \] API restart does not erase Job**
>
> **\[ \] duplicate Candidate avoided**

# **73. Sprint 5 — Exit Criteria**

> **Scene**
>
> **→ Snapshot**
>
> **→ Job**
>
> **→ RabbitMQ**
>
> **→ Worker**
>
> **→ Mock Adapter**
>
> **→ Asset**
>
> **→ Candidate**

**end-to-end.**

# **74. Sprint 5 — Research Parallel Track**

> **\[ \] freeze baseline environment candidate**
>
> **\[ \] continue failure evidence collection**

# **75. Sprint 6 — Candidate, Regeneration & History**

## **Goal**

**Hoàn thiện user-control layer của generation workflow.**

# **76. Sprint 6 — Candidate Gallery**

**Frontend must show:**

- **generated Candidates;**

- **selected state;**

- **generation status;**

- **validation state when available;**

- **timestamp/history.**

# **77. Sprint 6 — Candidate Selection**

**Implement:**

> **SelectCandidate**

**Rules:**

> **Candidate ≠ Canon until selected workflow**
>
> **Old Candidates remain**
>
> **Blocked Candidate cannot be selected**

# **78. Sprint 6 — Regeneration**

**Implement:**

> **RegenerateSceneImage**

**Regeneration là creative operation mới.**

# **79. Sprint 6 — Retry**

**Retry là technical recovery.**

**Do not map:**

> **Retry == Regeneration**

# **80. Sprint 6 — History**

**Persist/show:**

> **Job**
>
> **Attempt**
>
> **Candidate**
>
> **Asset**
>
> **Instruction**
>
> **Timestamp**
>
> **Selection**

# **81. Sprint 6 — Optional SignalR**

**Có thể thêm SignalR tại đây để cải thiện UX.**

**Rule:**

> **REST Job state remains source of truth**

# **82. Sprint 6 — Reload Recovery**

**Browser refresh phải reload:**

> **current Job**
>
> **Candidates**
>
> **selected Candidate**
>
> **history**

**từ backend.**

# **83. Sprint 6 — Tests**

> **\[ \] select Candidate**
>
> **\[ \] regenerate creates new creative operation**
>
> **\[ \] retry preserves operation identity**
>
> **\[ \] history retained**
>
> **\[ \] refresh reloads state**
>
> **\[ \] SignalR disconnect does not lose Job**

# **84. Sprint 6 — Exit Criteria**

**User có thể:**

> **Generate**
>
> **→ Review**
>
> **→ Select**
>
> **→ Regenerate**
>
> **→ Review history**

# **85. Sprint 6 — Research Parallel Track**

**Research should now have enough baseline data to:**

> **\[ \] consolidate failure categories**
>
> **\[ \] determine repeated high-value failures**
>
> **\[ \] confirm or reject proposed research gap**

# **86. Sprint 7 — Real Baseline + Validation**

## **Goal**

**Đưa StoryDiffusion vào Product workflow và thêm validation foundation.**

# **87. Sprint 7 — Entry Criteria**

**Required:**

> **Sprint 5 stable**
>
> **Sprint 6 stable**
>
> **StoryDiffusion baseline reproducible**

# **88. Sprint 7 — StoryDiffusion Adapter**

**Implement:**

> **StoryDiffusionAdapter**

**behind image generation abstraction.**

# **89. Sprint 7 — StoryDiffusion Prompt Builder**

**Adapter owns:**

> **StoryDiffusionPromptRenderer**

**hoặc equivalent baseline input builder.**

**Input:**

> **GenerationContextV1**

**Output:**

> **StoryDiffusion-specific model input**

# **90. Sprint 7 — Provenance**

**Persist:**

> **contextSchemaVersion**
>
> **contextSnapshotId**
>
> **adapterVersion**
>
> **modelId**
>
> **methodVersion**
>
> **promptTemplateVersion**
>
> **seed**
>
> **workerVersion**

**when applicable.**

# **91. Sprint 7 — Rendered Prompt**

**Rendered prompt có thể được lưu cho debug/research.**

**It is:**

> **not Canon**
>
> **not frontend business contract**

# **92. Sprint 7 — Real Asset Output**

**Generated image stored via:**

> **IAssetStorage**

**Development may remain local.**

# **93. Sprint 7 — Validation Foundation**

**Implement staged validation:**

> **Technical**
>
> **↓**
>
> **Contract**
>
> **↓**
>
> **Semantic**
>
> **↓**
>
> **Consistency**
>
> **↓**
>
> **Safety**

**MVP có thể bắt đầu với subset practical.**

# **94. Sprint 7 — Validation Severity**

> **INFO**
>
> **WARNING**
>
> **ERROR**
>
> **BLOCKING**

**Frontend phải distinguish Warning vs Blocking.**

# **95. Sprint 7 — Candidate Decision**

**Possible:**

> **ACCEPT**
>
> **REVIEW_RECOMMENDED**
>
> **REGENERATE_RECOMMENDED**
>
> **REJECT**

**theo AI-03.**

# **96. Sprint 7 — Failure Normalization**

**Cover:**

> **model load failure**
>
> **timeout**
>
> **GPU OOM**
>
> **invalid output**
>
> **storage failure**
>
> **validation failure**

# **97. Sprint 7 — Tests**

> **\[ \] same generation API works with StoryDiffusion**
>
> **\[ \] real image stored**
>
> **\[ \] provenance saved**
>
> **\[ \] validation invoked**
>
> **\[ \] failure normalized**
>
> **\[ \] Candidate selection rules preserved**

# **98. Sprint 7 — Exit Criteria**

> **Scene**
>
> **→ GenerationContextV1**
>
> **→ Snapshot**
>
> **→ Job**
>
> **→ StoryDiffusion**
>
> **→ Real Asset**
>
> **→ Validation**
>
> **→ Candidate**
>
> **→ Review**

# **99. Sprint 7 — Research Parallel Track**

> **\[ \] finalize RESEARCH-02 evidence**
>
> **\[ \] confirm research gap**
>
> **\[ \] freeze baseline configuration for experiment**

# **100. Sprint 8 — Stale Context + AI-Assisted Content**

## **Goal**

**Bảo vệ generation history trước Canon edits và thêm P1 text-AI proposal flow.**

# **101. Sprint 8 — Dependency Tracking**

**Use:**

> **ContextSnapshotDependency**

**to trace:**

> **StoryCoreVersion**
>
> **CharacterVersion**
>
> **SceneVersion**
>
> **CharacterReference**

**as applicable.**

# **102. Sprint 8 — Stale Detection**

**When Canon changes:**

> **Create New Version**
>
> **↓**
>
> **Find affected historical dependencies**
>
> **↓**
>
> **Mark relevant output as OUTDATED_CONTEXT / stale equivalent**

# **103. Sprint 8 — Stale Rules**

**Do not:**

> **delete old Candidate**
>
> **mutate old Snapshot**
>
> **auto-regenerate entire Story**

# **104. Sprint 8 — New Generation Rule**

**New generation uses latest applicable Canon.**

**Old Candidate remains traceable to old Snapshot.**

# **105. Sprint 8 — Text AI Interface**

**Implement:**

> **ITextGenerationService**

# **106. Sprint 8 — AI Proposal Flow**

> **Request suggestion**
>
> **↓**
>
> **Build relevant context**
>
> **↓**
>
> **Text AI**
>
> **↓**
>
> **Proposal**
>
> **↓**
>
> **Review/Edit**
>
> **↓**
>
> **Application Command**
>
> **↓**
>
> **Canon**

**Proposal never becomes Canon automatically.**

# **107. Sprint 8 — Possible P1 Features**

**Depending on time:**

> **Story Core suggestion**
>
> **Character description suggestion**
>
> **Scene suggestion**

**Do not expand beyond Product scope.**

# **108. Sprint 8 — Tests**

> **\[ \] relevant Candidate becomes stale**
>
> **\[ \] unrelated Candidate remains valid**
>
> **\[ \] old Snapshot unchanged**
>
> **\[ \] new generation uses new Canon**
>
> **\[ \] proposal does not auto-canonicalize**
>
> **\[ \] accepted proposal goes through Application command**

# **109. Sprint 8 — Exit Criteria**

**System remains historically traceable after canonical edits and supports safe AI text proposals.**

# **110. Sprint 8 — Research Parallel Track**

**If gap confirmed:**

> **\[ \] begin SAMIM implementation**
>
> **\[ \] implement V1/V2 only as evidence supports**
>
> **\[ \] prepare ablation structure**

# **111. Sprint 9 — SAMIM Research Integration**

## **Goal**

**Tích hợp proposed research method vào cùng Product generation workflow.**

# **112. Sprint 9 — Entry Criteria**

**Required:**

> **\[ \] StoryDiffusion baseline stable**
>
> **\[ \] research gap confirmed**
>
> **\[ \] RESEARCH-03 method sufficiently frozen**
>
> **\[ \] experiment protocol sufficiently frozen**

# **113. Sprint 9 — Adapter**

**Implement:**

> **SAMIMAdapter**

**or:**

> **ProposedMethodAdapter**

**behind same IImageGenerationService boundary.**

# **114. Sprint 9 — SAMIM Context Input**

**SAMIM consumes structured context including:**

> **active Character IDs**
>
> **stable identity**
>
> **canonical references**
>
> **scene-specific state**
>
> **appearance state**
>
> **continuity context**
>
> **generation options**

# **115. Sprint 9 — SAMIM Condition Builder**

**SAMIM owns:**

> **SAMIMConditionBuilder**

**Không ép SAMIM phải dùng cùng prompt string với StoryDiffusion.**

# **116. Sprint 9 — Identity Memory**

**Research Identity Memory:**

> **Visual Identity Conditioning**

**Application Story Memory:**

> **Narrative Facts / State / Relationship / Continuity**

**Hai khái niệm phải tách rõ.**

# **117. Sprint 9 — Method Variants**

**Potential staged variants:**

> **V0 Baseline**
>
> **V1 Per-Character Identity Memory**
>
> **V2 Scene-Aware Retrieval**
>
> **V3 Identity Isolation**
>
> **V4 Identity / Appearance Separation**

**Chỉ implement variant có justification từ RESEARCH-02/03.**

# **118. Sprint 9 — Provenance**

**Store:**

> **methodVersion**
>
> **variant**
>
> **adapterVersion**
>
> **modelId**
>
> **seed**
>
> **conditionBuilderVersion**
>
> **contextSnapshotId**
>
> **experimentConfig**

# **119. Sprint 9 — Product Comparison**

**Same Scene can produce:**

> **Baseline Candidate**
>
> **SAMIM Candidate**

**through same Job/Candidate workflow.**

# **120. Sprint 9 — Tests**

**Product integration:**

> **\[ \] same API flow**
>
> **\[ \] same snapshot contract**
>
> **\[ \] provenance differs correctly**
>
> **\[ \] Candidate coexist**
>
> **\[ \] failure safe**

**Scientific evaluation belongs to RESEARCH-04.**

# **121. Sprint 9 — Exit Criteria**

> **StoryDiffusion**
>
> **and**
>
> **SAMIM**

**can both generate Candidates inside the same Product architecture.**

# **122. Sprint 9 — Research Track**

**Primary focus:**

> **\[ \] controlled experiments**
>
> **\[ \] ablations**
>
> **\[ \] long-range consistency metrics**
>
> **\[ \] multi-character analysis**
>
> **\[ \] qualitative failure analysis**

# **123. Sprint 10 — QA, Deployment & Thesis Demo**

## **Goal**

**Freeze a stable, defendable thesis system.**

# **124. Sprint 10 — Full Functional QA**

**Run:**

> **QA-01**
>
> **QA-03**
>
> **Critical API tests**
>
> **Integration tests**
>
> **E2E tests**
>
> **Persistence/reload**
>
> **Authorization**
>
> **Concurrency**
>
> **Job recovery**

# **125. Sprint 10 — AI QA**

**Run:**

> **QA-02**

**Focus:**

> **identity**
>
> **multi-character**
>
> **long-range**
>
> **appearance changes**
>
> **warnings**
>
> **blocking validation**
>
> **regression**

# **126. Sprint 10 — UAT**

**Run:**

> **QA-04**

**using thesis-demo journeys.**

# **127. Sprint 10 — S3AssetStorage**

**Complete:**

> **S3AssetStorage**

**for demo/production-like environment.**

**Preferred provider currently:**

> **Cloudflare R2**

**Provider must remain replaceable.**

# **128. Sprint 10 — Deployment Topology**

**Deploy:**

> **Vue Frontend**
>
> **ASP.NET Core API**
>
> **PostgreSQL**
>
> **RabbitMQ**
>
> **Python Worker**
>
> **S3-Compatible Asset Storage**
>
> **AI Model Runtime**

# **129. Sprint 10 — Monitoring**

**Verify:**

> **API health**
>
> **API readiness**
>
> **RabbitMQ**
>
> **Worker health/readiness**
>
> **Job trace**
>
> **Storage**
>
> **Database**
>
> **Logs**

# **130. Sprint 10 — Backup**

**Required:**

> **\[ \] database backup**
>
> **\[ \] important asset verification**
>
> **\[ \] restore drill**

# **131. Sprint 10 — Demo Dataset**

**Prepare:**

> **Project**
>
> **Story Core**
>
> **Recurring Characters**
>
> **Canonical References**
>
> **Multi-Scene Story**
>
> **Multi-Character Scene**
>
> **Long-Range Reappearance**
>
> **Appearance Change**
>
> **Baseline Candidate**
>
> **SAMIM Candidate**

# **132. Sprint 10 — Live + Fallback Demo**

**Prepare:**

### **Live Generation**

**Real pipeline.**

### **Prepared Real Candidate**

**Fallback when GPU/provider unavailable.**

**Prepared output must be described honestly as pre-generated.**

# **133. Sprint 10 — Release Freeze**

**Freeze:**

> **Frontend Version**
>
> **API Version**
>
> **Worker Version**
>
> **Database Migration Version**
>
> **Context Schema Version**
>
> **Adapter Version**
>
> **Model / Method Version**
>
> **Prompt Template / Condition Builder Version**
>
> **Validation Profile**
>
> **Demo Dataset Version**

**Recommended release:**

> **THESIS_DEMO_RC_1**

# **134. Sprint 10 — Exit Criteria**

> **\[ \] all P0 workflows pass**
>
> **\[ \] no P0 blocker**
>
> **\[ \] deployment reproducible**
>
> **\[ \] backup exists**
>
> **\[ \] restore tested**
>
> **\[ \] live generation verified**
>
> **\[ \] fallback Candidate available**
>
> **\[ \] research results traceable**
>
> **\[ \] final demo dataset stable**

# **135. Sprint Deliverable Summary**

<table>
<colgroup>
<col style="width: 16%" />
<col style="width: 83%" />
</colgroup>
<thead>
<tr class="header">
<th><blockquote>
<p><strong>Sprint</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Main Deliverable</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>0</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Engineering foundation</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>1</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Project + Story</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>2</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Character + Reference</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>3</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Scene + SceneCharacter</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>4</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Version + GenerationContextV1 + Snapshot</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>5</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Async Job + RabbitMQ + Mock AI</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>6</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Candidate + Regeneration + History</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>7</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>StoryDiffusion + Validation</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>8</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>Stale Context + Text AI Proposal</strong></p>
</blockquote></th>
</tr>
<tr class="header">
<th><blockquote>
<p><strong>9</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>SAMIM Integration</strong></p>
</blockquote></th>
</tr>
<tr class="odd">
<th><blockquote>
<p><strong>10</strong></p>
</blockquote></th>
<th><blockquote>
<p><strong>QA + Deployment + Thesis Demo</strong></p>
</blockquote></th>
</tr>
</thead>
<tbody>
</tbody>
</table>

# **136. Product Track Summary**

> **Sprint 0**
>
> **Foundation**
>
> **Sprint 1**
>
> **Project / Story**
>
> **Sprint 2**
>
> **Character / Reference**
>
> **Sprint 3**
>
> **Scene**
>
> **Sprint 4**
>
> **Version / Context / Snapshot**
>
> **Sprint 5**
>
> **Job / Worker / Mock**
>
> **Sprint 6**
>
> **Candidate / History**
>
> **Sprint 7**
>
> **Baseline / Validation**
>
> **Sprint 8**
>
> **Stale / Text AI**
>
> **Sprint 9**
>
> **SAMIM Integration**
>
> **Sprint 10**
>
> **QA / Demo**

# **137. Research Track Summary**

> **Sprint 0–1**
>
> **Environment / Dataset**
>
> **Sprint 2–3**
>
> **Baseline Reproduction**
>
> **Sprint 4–5**
>
> **Failure Collection**
>
> **Sprint 6–7**
>
> **Gap Confirmation**
>
> **Sprint 8**
>
> **Method Implementation**
>
> **Sprint 9**
>
> **Experiments / Ablations**
>
> **Sprint 10**
>
> **Final Evaluation / Thesis Packaging**

# **138. Sprint Scope Rule**

**Nếu một task không phục vụ current sprint goal:**

> **Move to backlog**

**Không kéo vào sprint vì “làm luôn cho tiện”.**

# **139. No Premature P1 Rule**

**Không làm P1 nếu P0 critical path chưa stable.**

**Ví dụ:**

**Không ưu tiên AI Story suggestion trong khi:**

> **Job persistence**
>
> **Candidate selection**
>
> **Context Snapshot**

**chưa ổn định.**

# **140. No Premature SAMIM Rule**

**Không tích hợp SAMIM nếu:**

> **StoryDiffusion chưa reproducible**
>
> **Failure analysis chưa đủ**
>
> **Gap chưa confirmed**

# **141. AI Coding Task Size**

**Task giao cho AI coding assistant nên nhỏ đủ để review.**

**Ví dụ tốt:**

> **Implement CreateProject vertical slice**

**thay vì:**

> **Build whole backend**

# **142. AI Coding Context Rule**

**Mỗi task chỉ cung cấp relevant package:**

> **DEV-01**
>
> **Current Sprint section**
>
> **Relevant IMPLEMENT-02 module**
>
> **Relevant Domain/Data/API/AI specs**
>
> **Current code**

# **143. AI Coding Change Rule**

**AI assistant không được tự:**

- **đổi framework;**

- **đổi queue;**

- **đổi storage architecture;**

- **đổi API contract;**

- **đổi business rules;**

- **thêm schema ngoài task;**

- **refactor unrelated modules.**

# **144. Pull Request Rule**

**Mỗi PR nên map tới:**

> **Sprint**
>
> **+**
>
> **Task**
>
> **+**
>
> **Acceptance Criteria**

# **145. Definition of Done — Sprint Task**

> **\[ \] implementation complete**
>
> **\[ \] build pass**
>
> **\[ \] relevant tests pass**
>
> **\[ \] migration included if needed**
>
> **\[ \] error handling included**
>
> **\[ \] authorization included**
>
> **\[ \] docs updated if contract changed**
>
> **\[ \] no unrelated change**

# **146. Definition of Done — Sprint**

**Sprint Done khi:**

> **\[ \] Sprint goal demonstrated**
>
> **\[ \] Critical tasks done**
>
> **\[ \] Tests pass**
>
> **\[ \] No unresolved blocker in dependency chain**
>
> **\[ \] Working increment can be shown**

**Không cần mọi optional task hoàn thành để đóng sprint.**

# **147. Technical Debt Handling**

**Deferred shortcut phải ghi:**

> **TD-ID**
>
> **Description**
>
> **Risk**
>
> **Reason**
>
> **Target Sprint**

**Không để TODO mơ hồ.**

# **148. Bug Priority**

### **P0**

**Blocks core workflow / data integrity.**

### **P1**

**Major feature defect with workaround.**

### **P2**

**Minor/non-critical defect.**

**Không chuyển sprint nếu P0 dependency blocker còn tồn tại.**

# **149. Database Migration Rule**

**Mọi schema change phải có migration.**

**Migration phải:**

> **build**
>
> **apply**
>
> **test clean DB**
>
> **test current upgrade path when relevant**

# **150. Asset Storage Rule**

**Development:**

> **LocalFileAssetStorage**

**Final demo:**

> **S3AssetStorage**

**Application layer không đổi.**

# **151. Queue Rule**

**RabbitMQ là implementation hiện tại.**

**Business contract là:**

> **asynchronous generation execution.**

**Không leak RabbitMQ semantics vào Domain.**

# **152. SignalR Rule**

**SignalR optional.**

**Nếu chưa đủ thời gian:**

> **REST polling**

**là đủ cho MVP.**

# **153. Context Snapshot Rule**

**Không cho worker đọc live mutable Canon như primary generation input.**

**Worker sử dụng Snapshot của Job.**

# **154. Prompt Rule**

**Không có global raw prompt contract.**

**Each Adapter owns:**

> **Prompt Builder**
>
> **or**
>
> **Condition Builder**

# **155. Provenance Rule**

**Real AI Candidate phải trace được:**

> **Context Snapshot**
>
> **Context Schema Version**
>
> **Adapter**
>
> **Model**
>
> **Method**
>
> **Prompt/Condition Builder Version**
>
> **Seed**
>
> **Attempt**
>
> **Asset**

# **156. Candidate Rule**

**Regeneration không overwrite Candidate cũ.**

**History phải preserve.**

# **157. Retry Rule**

**Technical retry không được xuất hiện như new creative generation trong user history trừ khi contract yêu cầu hiển thị Attempt detail.**

# **158. Stale Rule**

**Canonical edit:**

> **does not mutate old Snapshot**
>
> **does not delete old Candidate**
>
> **does not auto-regenerate whole story**

# **159. Research Integrity Rule**

**Research run không silently switch:**

- **model;**

- **method;**

- **seed policy;**

- **prompt template;**

- **condition builder;**

- **dataset;**

- **evaluation configuration.**

# **160. Demo Stability Rule**

**Sau THESIS_DEMO_RC_1:**

**chỉ sửa:**

> **P0 blockers**
>
> **crash**
>
> **data corruption**
>
> **critical workflow defect**

**Không thêm architecture experiment.**

# **161. First Tasks to Execute**

**Coding start sequence:**

> **1. Create repository skeleton**
>
> **2. Bootstrap Vue 3 + Vite + TypeScript**
>
> **3. Bootstrap ASP.NET Core Web API**
>
> **4. Configure PostgreSQL**
>
> **5. Configure RabbitMQ**
>
> **6. Define IAssetStorage**
>
> **7. Implement LocalFileAssetStorage**
>
> **8. Create Python Worker skeleton**
>
> **9. Add health checks**
>
> **10. Add initial migrations**
>
> **11. Implement Project vertical slice**

# **162. First AI-Related Tasks**

**Do not begin with real model.**

**Order:**

> **1. Define IImageGenerationService**
>
> **2. Implement GenerationContextV1**
>
> **3. Implement GenerationContextBuilder**
>
> **4. Implement Context Snapshot**
>
> **5. Implement Job**
>
> **6. Implement RabbitMQ message flow**
>
> **7. Implement Mock Adapter**
>
> **8. Implement Candidate persistence**
>
> **9. Implement Candidate selection/history**
>
> **10. Integrate StoryDiffusion**
>
> **11. Add Validation**
>
> **12. Integrate SAMIM**

# **163. Sprint Success Model**

**OWNIVERSE should evolve as:**

> **Working Canonical App**
>
> **↓**
>
> **Traceable Context**
>
> **↓**
>
> **Reliable Async Generation**
>
> **↓**
>
> **Real Baseline AI**
>
> **↓**
>
> **Validation**
>
> **↓**
>
> **Research Method**
>
> **↓**
>
> **Stable Thesis Demo**

# **164. Final Sprint Principle**

**SPRINT-01 follows:**

> **One working increment per sprint.**
>
> **Canon before AI.**
>
> **Version before Snapshot.**
>
> **Structured Context before Prompt.**
>
> **Snapshot before Async Execution.**
>
> **Mock before Baseline.**
>
> **Baseline before SAMIM.**
>
> **Candidate before Canonical Acceptance.**
>
> **Testing happens inside each sprint.**
>
> **Deployment and thesis-demo readiness are part of the product, not an afterthought.**
