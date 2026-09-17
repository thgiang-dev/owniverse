# **IMPLEMENT-01 — MVP Implementation Roadmap**

**Document ID:** IMPLEMENT-01  
**Document Type:** MVP Implementation Roadmap  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1 — Updated  
**Primary Roles:** Technical Lead / Full-Stack Developer / AI Engineer / Thesis Author / AI Coding Assistant  
**Parent Documents:** PROD-01, PROD-02, ARCH-01, ARCH-02, DOMAIN-01, DOMAIN-02, AI-01, AI-02, AI-03, DATA-01, DATA-02, API-01, API-02  
**Related Documents:** DEV-01, IMPLEMENT-02, QA-01, QA-02, QA-03, QA-04, DEPLOY-01, DEPLOY-02, DEPLOY-03, SPRINT-01, RESEARCH-01, RESEARCH-02, RESEARCH-03, RESEARCH-04

# **1. Purpose**

IMPLEMENT-01 chuyển toàn bộ specification của OWNIVERSE thành một **roadmap triển khai MVP có thứ tự phụ thuộc rõ ràng**.

Tài liệu trả lời:

- nên xây phần nào trước;

- phần nào phụ thuộc phần nào;

- khi nào có thể bắt đầu AI integration;

- khi nào Product Track gặp Research Track;

- milestone nào chứng minh hệ thống đã tiến thêm một bước;

- khi nào có thể chuyển từ Mock AI sang baseline;

- khi nào SAMIM được phép tích hợp;

- khi nào hệ thống đủ điều kiện cho QA và Thesis Demo.

IMPLEMENT-01 không thay thế IMPLEMENT-02 hoặc SPRINT-01.

Có thể hiểu:

> IMPLEMENT-01
>
> = Build Order + Milestones
>
> IMPLEMENT-02
>
> = Module-by-Module Implementation Detail
>
> SPRINT-01
>
> = Concrete Sprint Execution

# **2. Roadmap Philosophy**

OWNIVERSE được triển khai theo nguyên tắc:

> **Vertical Slice Before Horizontal Completion**

Thay vì:

> build all database
>
> ↓
>
> build all backend
>
> ↓
>
> build all frontend
>
> ↓
>
> build AI last

roadmap ưu tiên:

> Domain
>
> ↓
>
> Persistence
>
> ↓
>
> Application
>
> ↓
>
> API
>
> ↓
>
> Frontend
>
> ↓
>
> Test

cho từng capability.

# **3. Core Implementation Principle**

Mỗi milestone phải tạo ra **working behavior**, không chỉ tạo thêm code.

Ví dụ:

Không xem:

> Project table exists

là milestone hoàn chỉnh.

Mà xem:

> Create Project
>
> → Persist
>
> → Reload
>
> → Authorization
>
> → Test

là milestone.

# **4. Locked Technology Stack**

Implementation roadmap sử dụng stack chính thức:

> Frontend
>
> Vue 3 + Vite + TypeScript
>
> Backend
>
> ASP.NET Core Web API
>
> Database
>
> PostgreSQL
>
> Queue
>
> RabbitMQ
>
> AI Worker
>
> Python
>
> Asset Storage
>
> IAssetStorage
>
> ├── LocalFileAssetStorage — development
>
> └── S3AssetStorage — thesis demo / production-like
>
> Preferred S3-Compatible Provider
>
> Cloudflare R2
>
> Realtime
>
> SignalR — optional
>
> AI Interfaces
>
> IImageGenerationService
>
> ITextGenerationService
>
> IConsistencyService

Không thay đổi stack giữa roadmap nếu chưa có architecture decision mới.

# **5. Architecture Guardrails**

Roadmap phải giữ:

> Modular Monolith Backend
>
> \+
>
> Separate Python AI Worker

Không triển khai premature microservices.

Heavy AI inference không chạy trong ASP.NET HTTP request.

Binary assets không lưu trực tiếp như business relational data.

# **6. AI Contract Guardrail**

Application không dùng raw prompt string làm contract chính.

Luồng bắt buộc:

> Canonical Domain Data
>
> ↓
>
> GenerationContextV1
>
> ↓
>
> Immutable Context Snapshot
>
> ↓
>
> AI Adapter
>
> ↓
>
> Prompt Builder / Condition Builder
>
> ↓
>
> Model-Specific Input

# **7. Context Priority**

Mọi generation phải giữ precedence:

> Canonical / Locked Identity
>
> ↓
>
> Timeline / Scene-Specific State
>
> ↓
>
> User Instruction
>
> ↓
>
> Soft Context
>
> ↓
>
> Model Assumption

# **8. AI Replaceability**

Product workflow phải hoạt động với:

> Mock AI
>
> StoryDiffusion
>
> SAMIM
>
> External API

thông qua cùng abstraction.

Product code không được phụ thuộc trực tiếp vào một model implementation.

# **9. Main Application Track**

Application MVP Track bao gồm:

- identity/access basics;

- Project;

- Story;

- Character;

- Character Reference;

- Scene;

- SceneCharacter;

- Asset;

- Version;

- Context Snapshot;

- Job;

- Attempt;

- Candidate;

- Candidate Selection;

- Regeneration;

- History;

- Validation;

- Stale Context;

- AI-assisted content;

- deployment;

- QA.

# **10. Research Track**

Research Track bao gồm:

> Baseline Reproduction
>
> ↓
>
> Failure Analysis
>
> ↓
>
> Confirmed Research Gap
>
> ↓
>
> SAMIM
>
> ↓
>
> Controlled Experiment
>
> ↓
>
> Ablation
>
> ↓
>
> Evaluation

Research Track chạy song song, nhưng không được block Application MVP.

# **11. Product–Research Boundary**

Research model tích hợp qua:

> IImageGenerationService

Expected sequence:

> MockImageGenerationAdapter
>
> ↓
>
> StoryDiffusionAdapter
>
> ↓
>
> ProposedMethodAdapter / SAMIM

Application Scene/Generation workflow không thay đổi khi đổi Adapter.

# **12. MVP Target Capabilities**

MVP cần đạt tối thiểu:

1.  Create Project.

2.  Initialize Story.

3.  Create Character.

4.  Manage Character.

5.  Manage Character References.

6.  Create and reorder Scenes.

7.  Assign Character to Scene.

8.  Maintain scene-specific Character state.

9.  Create Generation Context.

10. Generate Scene Image.

11. Track async Job.

12. Review Candidate.

13. Select Candidate.

14. Regenerate.

15. View Generation History.

16. Run basic Validation.

17. Persist and reload all critical state.

18. Use updated Canon for later generation.

# **13. P0 Workflows**

P0 gồm các workflow cốt lõi:

> WF-01
>
> WF-02
>
> WF-04
>
> WF-05
>
> WF-06
>
> WF-07
>
> WF-08
>
> WF-10
>
> WF-11
>
> WF-12
>
> WF-13
>
> WF-15

P1 không được làm chậm P0 critical path.

# **14. P1 Workflows**

P1 chủ yếu gồm:

- convenience AI-assisted content;

- optional UX enhancement;

- additional proposal flow;

- non-critical automation.

P1 có thể triển khai sau khi P0 stable.

# **15. Deferred Scope**

Không thuộc MVP critical path:

- full page editor;

- panel layout editor;

- speech-bubble system nâng cao;

- marketplace;

- plugin ecosystem;

- social features;

- collaboration;

- mobile app;

- advanced branching/merge UI;

- microservices;

- Kubernetes;

- enterprise autoscaling.

# **16. Build Strategy**

Roadmap chia thành 11 phase:

> Phase 0 — Engineering Foundation
>
> Phase 1 — Project & Story Core
>
> Phase 2 — Character & References
>
> Phase 3 — Scene & SceneCharacter
>
> Phase 4 — Asset, Version & Context Snapshot
>
> Phase 5 — Async Generation + Mock AI
>
> Phase 6 — Real Baseline Image Generation
>
> Phase 7 — Validation, Stale Context & Canonical Change
>
> Phase 8 — AI-Assisted Story/Scene
>
> Phase 9 — Research Integration & SAMIM
>
> Phase 10 — QA, Deployment & Thesis Demo Hardening

# **17. Phase 0 — Engineering Foundation**

## **Goal**

Tạo skeleton đủ để toàn bộ development tiếp theo đi theo cùng architecture.

# **18. Phase 0 — Repository**

Create:

> /
>
> ├── frontend/
>
> ├── backend/
>
> ├── ai-worker/
>
> ├── tests/
>
> ├── deployment/
>
> ├── docs/
>
> ├── scripts/
>
> └── .github/

# **19. Phase 0 — Frontend Bootstrap**

Initialize:

> Vue 3
>
> Vite
>
> TypeScript

Minimum:

> \[ \] routing
>
> \[ \] application shell
>
> \[ \] shared API client
>
> \[ \] JSend handling
>
> \[ \] errorKey mapping foundation
>
> \[ \] environment config

# **20. Phase 0 — Backend Bootstrap**

Initialize ASP.NET Core Web API.

Minimum:

> \[ \] module registration
>
> \[ \] DI
>
> \[ \] exception handling
>
> \[ \] JSend envelope
>
> \[ \] structured logging
>
> \[ \] health endpoint
>
> \[ \] configuration binding

# **21. Phase 0 — PostgreSQL**

Implement:

> \[ \] PostgreSQL connection
>
> \[ \] ORM/persistence setup
>
> \[ \] migration framework
>
> \[ \] initial migration
>
> \[ \] migration verification on clean DB

# **22. Phase 0 — RabbitMQ**

Implement infrastructure foundation:

> \[ \] connection config
>
> \[ \] publisher abstraction
>
> \[ \] consumer foundation
>
> \[ \] health/readiness check

RabbitMQ không phải Job source of truth.

# **23. Phase 0 — Asset Storage**

Implement:

> IAssetStorage

First concrete implementation:

> LocalFileAssetStorage

S3-compatible implementation có thể được hoàn thiện trước demo.

# **24. Phase 0 — Python Worker**

Create worker skeleton:

> ai-worker/
>
> ├── jobs/
>
> ├── adapters/
>
> ├── pipelines/
>
> ├── context/
>
> ├── validation/
>
> ├── infrastructure/
>
> └── tests/

Worker phải connect được RabbitMQ.

# **25. Phase 0 — Mock Foundation**

Tạo placeholder interface:

> IImageGenerationService

Không cần real model.

# **26. Phase 0 — Milestone**

### **M0 — Engineering Skeleton**

M0 đạt khi:

> Frontend starts
>
> API starts
>
> PostgreSQL connects
>
> RabbitMQ connects
>
> Worker connects
>
> Local Asset Storage works
>
> Health checks pass

# **27. Phase 1 — Project & Story Core**

## **Goal**

Tạo canonical creative workspace.

# **28. Phase 1 — Identity/Access Minimum**

Implement minimum authenticated identity context.

Không cần enterprise IAM.

Required:

> \[ \] current user abstraction
>
> \[ \] project ownership
>
> \[ \] server-side authorization

# **29. Phase 1 — Project**

Implement vertical slice:

> Project Domain
>
> ↓
>
> Persistence
>
> ↓
>
> Application Use Cases
>
> ↓
>
> API
>
> ↓
>
> Frontend
>
> ↓
>
> Tests

Use cases:

> CreateProject
>
> GetProject
>
> ListProjects
>
> UpdateProject
>
> ArchiveProject if included

# **30. Phase 1 — Story**

Implement:

> Story
>
> Story Core

Minimum use cases:

> InitializeStory
>
> GetStory
>
> UpdateStoryCore

# **31. Phase 1 — Canonical Rule**

Story edit phải đi qua Application.

Frontend không trực tiếp mutate persistence model.

# **32. Phase 1 — Concurrency**

Introduce:

> revision_number

cho critical update.

Conflict:

> 409 REVISION_CONFLICT

# **33. Phase 1 — Milestone**

### **M1 — Project + Story**

M1 đạt khi:

> Create Project
>
> → Initialize Story
>
> → Edit
>
> → Reload
>
> → Same Canon persists

# **34. Phase 2 — Character & References**

## **Goal**

Xây visual identity foundation.

# **35. Phase 2 — Character**

Implement:

> Character

Core information:

- stable identity;

- canonical appearance;

- locked attributes;

- descriptive profile.

# **36. Phase 2 — Stable vs Mutable**

Không trộn:

> Stable Character Identity

với:

> Scene-Specific Appearance State

Scene-specific state sẽ thuộc SceneCharacter.

# **37. Phase 2 — Asset Foundation**

Asset metadata được lưu DB.

Binary được lưu qua:

> IAssetStorage

Development:

> LocalFileAssetStorage

# **38. Phase 2 — Character Reference**

Implement:

> CharacterReference

Capabilities:

> Upload Reference
>
> List References
>
> Select Canonical Reference

# **39. Phase 2 — Ownership**

Asset/Reference access phải enforce Project ownership.

# **40. Phase 2 — Milestone**

### **M2 — Character + Reference**

M2 đạt khi:

> Create Character
>
> → Upload Reference
>
> → Select Canonical Reference
>
> → Reload
>
> → Data remains correct

# **41. Phase 2 — Parallel Research**

Research Track có thể bắt đầu:

> Environment Setup
>
> Baseline Repository Setup
>
> ViStoryBench Preparation
>
> StoryDiffusion Smoke Run

# **42. Phase 3 — Scene & SceneCharacter**

## **Goal**

Tạo story sequence và scene-specific Character state.

# **43. Phase 3 — Scene**

Implement:

> CreateScene
>
> UpdateScene
>
> ReorderScenes
>
> GetScene
>
> ListScenes

# **44. Phase 3 — SceneCharacter**

Implement many-to-many relation:

> Scene
>
> ↔
>
> Character

through:

> SceneCharacter

# **45. Phase 3 — Scene State**

SceneCharacter có thể giữ:

> outfit
>
> emotion
>
> action
>
> pose intent
>
> temporary appearance
>
> injury state
>
> scene-specific visual note

Không rewrite Character stable identity.

# **46. Phase 3 — Active Character Set**

Scene phải xác định được:

> Active Characters

để AI context không load toàn bộ project character list.

# **47. Phase 3 — Milestone**

### **M3 — Scene**

M3 đạt khi:

> Create Scenes
>
> → Reorder
>
> → Assign Characters
>
> → Set Scene State
>
> → Reload

# **48. Phase 3 — Parallel Research**

Research:

> StoryDiffusion faithful reproduction
>
> Initial benchmark subset
>
> Baseline outputs
>
> Failure logging begins

# **49. Phase 4 — Asset, Version & Context Snapshot**

## **Goal**

Tạo traceability foundation trước real generation.

# **50. Phase 4 — Versioning**

MVP ưu tiên:

> CharacterVersion
>
> SceneVersion
>
> StoryCoreVersion

Historical generation-used versions immutable.

# **51. Phase 4 — Atomic Version Creation**

Canonical update cần historical tracking:

> Update Canon
>
> \+
>
> Create Version

trong transaction phù hợp.

# **52. Phase 4 — GenerationContextV1**

Implement structured context contract:

> GenerationContextV1

Minimum content:

> schemaVersion
>
> story context
>
> scene
>
> active characters
>
> canonical references
>
> scene-specific states
>
> continuity
>
> style
>
> userInstruction
>
> generationOptions

# **53. Phase 4 — Context Builder**

Implement:

> GenerationContextBuilder

Context Builder:

- không load toàn Project tùy tiện;

- lấy relevant story context;

- resolve active Character;

- resolve canonical references;

- resolve SceneCharacter state;

- apply context precedence.

# **54. Phase 4 — Context Priority**

Implement conflict resolution:

> Canonical / Locked Identity
>
> ↓
>
> Timeline / Scene State
>
> ↓
>
> User Instruction
>
> ↓
>
> Soft Context
>
> ↓
>
> Model Assumption

# **55. Phase 4 — Context Snapshot**

Persist:

> ContextSnapshot
>
> ContextSnapshotDependency

Snapshot immutable sau khi Job bắt đầu.

# **56. Phase 4 — Data Scope Protection**

Không implement toàn bộ target schema.

MVP ưu tiên:

> assets
>
> character_versions
>
> scene_versions
>
> story_core_versions
>
> context_snapshots
>
> context_snapshot_dependencies

# **57. Phase 4 — Snapshot Test**

Required test:

> Character v1
>
> ↓
>
> Snapshot S1
>
> ↓
>
> Character edited to v2
>
> ↓
>
> S1 still references v1

# **58. Phase 4 — Milestone**

### **M4 — Version + Snapshot**

M4 đạt khi generation context có thể được build và freeze mà không cần AI.

# **59. Phase 5 — Async Generation + Mock AI**

## **Goal**

Chạy full generation workflow mà không cần GPU/model thật.

Đây là critical architectural milestone.

# **60. Phase 5 — Generation Domain**

Implement:

> GenerationJob
>
> GenerationAttempt
>
> Candidate

# **61. Phase 5 — Job Lifecycle**

Implement explicit state machine theo API-02/ARCH-02.

Ví dụ lifecycle logical:

> CREATED
>
> QUEUED
>
> CLAIMED
>
> RUNNING
>
> VALIDATING
>
> COMPLETED
>
> FAILED
>
> CANCELLED

Exact public mapping phải theo API contract.

# **62. Phase 5 — Job Persistence**

Job phải persist trước queue execution.

Recommended:

> Create Snapshot
>
> Create Job
>
> COMMIT
>
> ↓
>
> Publish RabbitMQ Message

# **63. Phase 5 — RabbitMQ Message**

Queue message chỉ chứa identifier cần thiết.

Ví dụ:

> jobId
>
> attemptId
>
> operationType

Không gửi toàn Project qua queue.

# **64. Phase 5 — Mock Adapter**

Implement:

> MockImageGenerationAdapter

Capabilities:

> success
>
> delay
>
> failure
>
> warning
>
> deterministic fake output

# **65. Phase 5 — Worker Flow**

Worker:

> Receive
>
> ↓
>
> Load Snapshot
>
> ↓
>
> Resolve Mock Adapter
>
> ↓
>
> Generate fake output
>
> ↓
>
> Store Asset
>
> ↓
>
> Create Candidate
>
> ↓
>
> Persist result
>
> ↓
>
> Complete Job

# **66. Phase 5 — Prompt Boundary**

Ngay cả Mock pipeline cũng phải respect architecture:

> Context Snapshot
>
> ↓
>
> Adapter
>
> ↓
>
> Model Input / Simulated Input

Không để frontend build technical prompt.

# **67. Phase 5 — Candidate**

Candidate phải:

- link Scene;

- link Job/Attempt;

- link Asset;

- giữ provenance;

- không tự become Canon.

# **68. Phase 5 — Frontend Job Tracking**

Frontend:

> Generate
>
> ↓
>
> 202 + jobId
>
> ↓
>
> REST Job Polling
>
> ↓
>
> Candidate appears

SignalR optional.

# **69. Phase 5 — Realtime Rule**

Nếu implement SignalR:

- chỉ UX enhancement;

- REST Job API vẫn hoạt động độc lập;

- frontend reload vẫn phục hồi state.

# **70. Phase 5 — Candidate Selection**

Implement:

> SelectCandidate

Rules:

- selected result persisted;

- previous Candidate retained;

- blocked Candidate không selectable.

# **71. Phase 5 — Regeneration**

Implement creative regeneration.

Regeneration:

> ≠ Retry

Regeneration tạo creative operation mới theo contract hiện hành.

# **72. Phase 5 — History**

Implement generation history.

Must preserve:

> Job
>
> Attempt
>
> Candidate
>
> Asset
>
> Instruction
>
> Timestamp
>
> Selection
>
> Validation

# **73. Phase 5 — Milestone**

### **M5 — Mock End-to-End Generation**

M5 đạt khi:

> Scene
>
> → Generate
>
> → Context Snapshot
>
> → Job
>
> → RabbitMQ
>
> → Python Worker
>
> → Mock Adapter
>
> → Asset
>
> → Candidate
>
> → Select
>
> → History

chạy end-to-end.

# **74. Phase 6 — Real Baseline Image Generation**

## **Goal**

Thay Mock bằng baseline AI thực.

Primary baseline:

> StoryDiffusion

# **75. Phase 6 — Entry Criteria**

Không bắt đầu nếu M5 chưa stable.

Required:

> \[ \] Snapshot stable
>
> \[ \] Job stable
>
> \[ \] Worker stable
>
> \[ \] Candidate stable
>
> \[ \] Storage stable

# **76. Phase 6 — StoryDiffusionAdapter**

Implement:

> StoryDiffusionAdapter : IImageGenerationService

hoặc equivalent adapter contract giữa backend/worker.

# **77. Phase 6 — Adapter Input**

StoryDiffusionAdapter nhận structured Context Snapshot.

Adapter chịu trách nhiệm:

> GenerationContextV1
>
> ↓
>
> StoryDiffusion Prompt/Condition Builder
>
> ↓
>
> StoryDiffusion-Specific Input

# **78. Phase 6 — Provenance**

Persist:

> contextSchemaVersion
>
> contextSnapshotId
>
> adapterVersion
>
> modelId
>
> methodVersion
>
> promptTemplateVersion / conditionBuilderVersion
>
> seed
>
> workerVersion

khi applicable.

# **79. Phase 6 — Rendered Prompt**

Rendered prompt/model payload có thể persist cho debugging/research.

Nó:

> ≠ Canon
>
> ≠ public API contract

# **80. Phase 6 — Storage**

Real generated binary đi qua:

> IAssetStorage

Development có thể LocalFileAssetStorage.

Demo/production-like sẽ dùng S3AssetStorage.

# **81. Phase 6 — Baseline Validation**

Run basic technical/contract validation.

Không cần full research evaluation ở Application runtime.

# **82. Phase 6 — Provider Failure**

Normalize:

> timeout
>
> OOM
>
> invalid output
>
> model load failure
>
> storage failure

thành stable Job failure/error behavior.

# **83. Phase 6 — Milestone**

### **M6 — Real Baseline**

M6 đạt khi:

> Scene
>
> → StoryDiffusion
>
> → Real Image
>
> → Asset
>
> → Candidate
>
> → Review
>
> → Select

trong cùng Product workflow của Mock.

# **84. Phase 6 — Research Dependency**

Song song Research Track hoàn thiện:

> Baseline Reproduction
>
> Failure Collection
>
> Failure Classification
>
> Gap Confirmation

# **85. Phase 7 — Validation, Stale Context & Canonical Change**

## **Goal**

Làm generation workflow an toàn khi Canon tiếp tục thay đổi.

# **86. Phase 7 — Validation Pipeline**

Implement staged validation theo AI-03:

> Technical
>
> ↓
>
> Contract
>
> ↓
>
> Semantic
>
> ↓
>
> Consistency
>
> ↓
>
> Safety
>
> ↓
>
> Candidate Decision

MVP có thể triển khai subset practical.

# **87. Phase 7 — Warning vs Blocking**

Validation phải phân biệt:

> WARNING
>
> BLOCKING

UI không treat mọi warning như hard failure.

# **88. Phase 7 — Candidate Decision**

Runtime decision có thể gồm:

> ACCEPT
>
> REVIEW_RECOMMENDED
>
> REGENERATE_RECOMMENDED
>
> REJECT

Exact mapping theo AI-03/QA-02.

# **89. Phase 7 — Stale Context**

Khi Canon thay đổi:

> Old Snapshot remains
>
> Old Candidate remains
>
> Affected dependency becomes stale/outdated

# **90. Phase 7 — Dependency Tracking**

Implement:

> ContextSnapshotDependency

để biết Candidate nào phụ thuộc:

- Character Version;

- Scene Version;

- Story Core Version;

- Reference.

# **91. Phase 7 — No Auto Delete**

Canonical edit không delete old generation.

# **92. Phase 7 — No Whole-Story Auto-Regeneration**

Không regenerate toàn bộ Story khi một Character thay đổi.

Chỉ mark affected state.

# **93. Phase 7 — Future Generation**

Generation mới phải dùng latest applicable canonical versions.

# **94. Phase 7 — Milestone**

### **M7 — Validation + Stale Context**

M7 đạt khi:

> Generate from v1
>
> ↓
>
> Edit Canon → v2
>
> ↓
>
> Old Candidate remains
>
> ↓
>
> Marked stale where relevant
>
> ↓
>
> New generation uses v2

# **95. Phase 8 — AI-Assisted Story / Scene**

## **Goal**

Thêm text AI convenience mà không phá Canon boundary.

P1 phase.

# **96. Phase 8 — Text AI Interface**

Implement:

> ITextGenerationService

Provider implementation không đi trực tiếp vào domain.

# **97. Phase 8 — Proposal Model**

Text AI tạo:

> Proposal

Proposal:

> ≠ Canon

# **98. Phase 8 — Story Proposal**

Possible capabilities:

> Story Core suggestion
>
> Character description suggestion
>
> Scene suggestion

theo Product scope.

# **99. Phase 8 — Accept/Edit**

User phải:

> Review
>
> ↓
>
> Accept/Edit
>
> ↓
>
> Application Command
>
> ↓
>
> Canon Update

# **100. Phase 8 — Milestone**

### **M8 — AI-Assisted Content**

M8 đạt khi proposal flow hoạt động mà AI không bypass canonical command.

# **101. Phase 9 — Research Integration & SAMIM**

## **Goal**

Tích hợp scientific contribution sau khi research gap được xác nhận.

# **102. Phase 9 — Entry Criteria**

Required:

> \[ \] StoryDiffusion reproducible
>
> \[ \] Failure pattern repeated
>
> \[ \] Research gap confirmed
>
> \[ \] RESEARCH-03 method sufficiently frozen
>
> \[ \] RESEARCH-04 experiment plan frozen enough

# **103. Phase 9 — Proposed Adapter**

Implement:

> ProposedMethodAdapter

hoặc:

> SAMIMAdapter

behind same image-generation abstraction.

# **104. Phase 9 — SAMIM Input**

SAMIM nhận:

> GenerationContextV1

và đặc biệt sử dụng:

- active Character IDs;

- canonical visual references;

- scene state;

- appearance state;

- visual identity memory;

- method-specific conditioning.

# **105. Phase 9 — Identity Memory Boundary**

SAMIM Identity Memory:

> visual identity conditioning.

Application Story Memory:

> narrative facts, state, relationship, continuity.

Không merge hai khái niệm.

# **106. Phase 9 — Staged Variants**

Expected method variants:

> V0 — Baseline
>
> V1 — Per-Character Identity Memory
>
> V2 — Scene-Aware Retrieval
>
> V3 — Identity Isolation
>
> V4 — Identity / Appearance Separation

Final actual variants phụ thuộc failure analysis.

# **107. Phase 9 — No Feature Without Evidence**

Nếu RESEARCH-02 không xác nhận một failure pattern:

không bắt buộc thêm module chỉ vì nó có trong ý tưởng ban đầu.

# **108. Phase 9 — Same Product Workflow**

User phải có thể chọn/run:

> StoryDiffusion

hoặc:

> SAMIM

trong cùng Generation workflow.

# **109. Phase 9 — Research Provenance**

Persist:

> methodVersion
>
> variant
>
> modelId
>
> seed
>
> experimentConfig
>
> Context Snapshot
>
> adapterVersion

đủ để export kết quả.

# **110. Phase 9 — Experiment Support**

System nên hỗ trợ:

> Baseline Candidate
>
> SAMIM Candidate
>
> Ablation Candidate

coexist và trace riêng.

# **111. Phase 9 — Research Evaluation**

Research evaluation theo RESEARCH-04, không nhúng toàn bộ vào runtime QA.

# **112. Phase 9 — Milestone**

### **M9 — SAMIM Integrated**

M9 đạt khi:

> Same Scene
>
> → Baseline
>
> → Candidate A
>
> Same Scene / Controlled Context
>
> → SAMIM
>
> → Candidate B

và cả hai có provenance rõ.

# **113. Phase 10 — QA, Deployment & Thesis Demo Hardening**

## **Goal**

Chuyển từ feature-complete sang defendable system.

# **114. Phase 10 — Functional QA**

Run:

> QA-03 critical cases
>
> Integration tests
>
> E2E workflow
>
> Persistence/reload
>
> Authorization
>
> Concurrency
>
> Job recovery

# **115. Phase 10 — AI QA**

Run:

> QA-02

bao gồm:

- identity;

- multi-character;

- long-range;

- appearance-state;

- warning/blocking;

- validation regression.

# **116. Phase 10 — UAT**

Run QA-04 journeys.

# **117. Phase 10 — Deployment**

Deploy:

> Vue Frontend
>
> ASP.NET API
>
> PostgreSQL
>
> RabbitMQ
>
> Python Worker
>
> S3-Compatible Storage

SignalR optional.

# **118. Phase 10 — S3AssetStorage**

Thesis Demo / Production-like:

> IAssetStorage
>
> ↓
>
> S3AssetStorage

Preferred current provider:

> Cloudflare R2

Provider implementation không leak vào domain/API contract.

# **119. Phase 10 — Monitoring**

Verify:

> API health
>
> RabbitMQ
>
> Worker readiness
>
> Job trace
>
> Storage
>
> Logs

# **120. Phase 10 — Backup**

Required:

> Database Backup
>
> Important Asset Protection
>
> Restore Drill

# **121. Phase 10 — Demo Freeze**

Freeze:

> Frontend Version
>
> API Version
>
> Worker Version
>
> Migration Version
>
> Context Schema Version
>
> Adapter Version
>
> Model / Method Version
>
> Prompt/Condition Builder Version
>
> Validation Profile
>
> Demo Dataset

Recommended:

> THESIS_DEMO_RC_1

# **122. Phase 10 — Milestone**

### **M10 — Thesis Demo Ready**

M10 đạt khi:

- P0 workflow pass;

- deployment stable;

- backup exists;

- demo dataset ready;

- live generation verified;

- fallback real Candidate available;

- no P0 blocker.

# **123. Milestone Summary**

> M0 — Engineering Skeleton
>
> M1 — Project + Story
>
> M2 — Character + Reference
>
> M3 — Scene + SceneCharacter
>
> M4 — Version + Context Snapshot
>
> M5 — Mock End-to-End Generation
>
> M6 — Real Baseline
>
> M7 — Validation + Stale Context
>
> M8 — AI-Assisted Content
>
> M9 — SAMIM Integrated
>
> M10 — Thesis Demo Ready

# **124. Critical Dependency Chain**

> Identity / Access
>
> ↓
>
> Project
>
> ↓
>
> Story
>
> ↓
>
> Character
>
> ↓
>
> Asset
>
> ↓
>
> Scene
>
> ↓
>
> Versioning
>
> ↓
>
> GenerationContextV1
>
> ↓
>
> Context Snapshot
>
> ↓
>
> Generation Job
>
> ↓
>
> RabbitMQ
>
> ↓
>
> Python Worker
>
> ↓
>
> Mock Adapter
>
> ↓
>
> Candidate
>
> ↓
>
> Selection / History
>
> ↓
>
> StoryDiffusion
>
> ↓
>
> Validation / Stale
>
> ↓
>
> SAMIM

# **125. Why Context Snapshot Comes Before AI**

Không có Snapshot:

- AI input khó tái hiện;

- canonical edits gây ambiguity;

- research comparison thiếu traceability;

- Job restart không biết exact input;

- provenance yếu.

Vì vậy Snapshot không phải feature phụ.

# **126. Why Mock Comes Before StoryDiffusion**

Mock chứng minh:

> Application workflow
>
> \+
>
> Async runtime
>
> \+
>
> Storage
>
> \+
>
> Candidate

độc lập với model.

Nếu pipeline chưa chạy với Mock, tích hợp real AI sẽ làm debugging khó hơn.

# **127. Why StoryDiffusion Comes Before SAMIM**

StoryDiffusion cần cho:

- baseline reproduction;

- adapter contract validation;

- failure analysis;

- research gap evidence;

- controlled comparison.

Không bỏ qua baseline.

# **128. Why SAMIM Comes Late**

SAMIM là scientific contribution.

Không nên dùng research implementation để che các lỗi còn tồn tại ở:

- Project;

- Context;

- Job;

- Storage;

- Candidate workflow.

# **129. Global Definition of Done**

Một feature/vertical slice Done khi:

> \[ \] Domain behavior implemented
>
> \[ \] Persistence implemented
>
> \[ \] Migration included if required
>
> \[ \] Application use case implemented
>
> \[ \] API implemented
>
> \[ \] Frontend implemented if user-facing
>
> \[ \] Validation/error handling implemented
>
> \[ \] Relevant tests pass
>
> \[ \] Docs synced if contract changed

# **130. AI Feature Definition of Done**

AI-related feature Done khi:

> \[ \] Goes through Adapter
>
> \[ \] Uses Context Snapshot
>
> \[ \] Does not mutate Canon directly
>
> \[ \] Job state durable
>
> \[ \] Asset persisted safely
>
> \[ \] Candidate persisted
>
> \[ \] Provenance recorded
>
> \[ \] Failure handled
>
> \[ \] Tests use Mock when possible

# **131. GenerationContext DoD**

GenerationContextV1 implementation Done khi:

> \[ \] Schema versioned
>
> \[ \] Relevant Canon selected
>
> \[ \] Active Characters resolved
>
> \[ \] Canonical refs resolved
>
> \[ \] Scene state resolved
>
> \[ \] Priority rules enforced
>
> \[ \] Snapshot immutable
>
> \[ \] Dependencies recorded
>
> \[ \] Tests verify old Snapshot unaffected by Canon edit

# **132. Asset Storage DoD**

Storage Done khi:

> \[ \] IAssetStorage abstraction exists
>
> \[ \] LocalFileAssetStorage works
>
> \[ \] Metadata persisted separately
>
> \[ \] Ownership enforced
>
> \[ \] Failure does not create broken completed Candidate

S3 implementation required before final demo.

# **133. RabbitMQ DoD**

Queue integration Done khi:

> \[ \] Job persisted before publish
>
> \[ \] Worker consumes
>
> \[ \] Ack/recovery behavior tested
>
> \[ \] Job survives API restart
>
> \[ \] Queue is not Job source of truth
>
> \[ \] Duplicate Candidate prevented

# **134. Frontend DoD**

User-facing feature Done khi:

> \[ \] Vue screen/component implemented
>
> \[ \] API integration stable
>
> \[ \] JSend handled
>
> \[ \] errorKey handled
>
> \[ \] reload restores durable state
>
> \[ \] loading/error state visible

# **135. Security DoD**

Critical feature Done khi:

> \[ \] Authentication context enforced
>
> \[ \] Ownership enforced
>
> \[ \] Secrets server-side only
>
> \[ \] Asset access controlled
>
> \[ \] Unsafe input validated

# **136. Testing Strategy During Roadmap**

Không chờ Phase 10 mới test.

Mỗi phase thêm:

- unit;

- integration;

- API;

- E2E;

- Mock AI tests.

Phase 10 là hardening, không phải điểm bắt đầu QA.

# **137. CI Strategy**

Normal CI không cần GPU.

Minimum:

> Frontend build/tests
>
> Backend build/tests
>
> Python worker tests
>
> Integration tests feasible without GPU

Real model run ở controlled environment.

# **138. Research Parallelization**

Suggested mapping:

> Application M0–M1
>
> → Research environment setup
>
> Application M2–M3
>
> → Baseline reproduction
>
> Application M4–M5
>
> → Failure collection
>
> Application M6
>
> → Failure classification / gap confirmation
>
> Application M7–M8
>
> → Proposed method implementation
>
> Application M9
>
> → Experiment / ablation
>
> Application M10
>
> → Final evaluation / thesis result packaging

Không xem đây là calendar deadline.

# **139. GPU Availability Rule**

Nếu GPU unavailable:

Application tiếp tục bằng:

> Mock Adapter

hoặc remote provider.

Không block Project/Character/Scene development.

# **140. External API Rule**

External image API có thể được thêm sau:

> ExternalImageApiAdapter

Không sửa product workflow.

# **141. Prompt System Rule**

Không tạo “one giant universal prompt” trong Application.

Mỗi Adapter sở hữu:

> Prompt Builder
>
> or
>
> Condition Builder

phù hợp model.

# **142. Prompt Versioning**

Nếu Adapter render prompt:

persist/version:

> promptTemplateVersion

Nếu method dùng structured conditioning:

persist/version:

> conditionBuilderVersion

# **143. No Frontend Technical Prompt**

Frontend chỉ gửi:

> userInstruction
>
> creative options

Backend/Worker chịu trách nhiệm technical model input.

# **144. Research Reproducibility**

Research run phải trace:

> Context Snapshot
>
> Model
>
> Method
>
> Adapter
>
> Config
>
> Seed
>
> Output
>
> Validation/Evaluation

# **145. Scope Protection Rule**

Nếu implementation phát hiện feature ngoài MVP:

> Document as backlog
>
> ↓
>
> Do not implement immediately

trừ khi critical dependency.

# **146. Architecture Change Rule**

Nếu cần thay:

- Queue;

- storage provider;

- frontend framework;

- context contract;

- core job semantics;

phải có explicit decision trước.

Không để AI coding agent tự đổi.

# **147. Technical Debt Rule**

Technical debt được ghi:

> TD-ID
>
> Description
>
> Risk
>
> Reason
>
> Target Milestone/Sprint

# **148. First Coding Sequence**

Ngay sau IMPLEMENT-01, coding bắt đầu bằng:

> 1\. Repository skeleton
>
> 2\. Vue 3 + Vite + TypeScript bootstrap
>
> 3\. ASP.NET Core solution
>
> 4\. PostgreSQL configuration
>
> 5\. RabbitMQ configuration
>
> 6\. LocalFileAssetStorage
>
> 7\. Python Worker skeleton
>
> 8\. Health endpoints
>
> 9\. Initial migrations
>
> 10\. Project entity
>
> 11\. Project migration
>
> 12\. Create Project API
>
> 13\. Project tests
>
> 14\. Project UI
>
> 15\. Story entity
>
> 16\. Story vertical slice

# **149. First AI Sequence**

AI implementation order:

> IImageGenerationService contract
>
> ↓
>
> MockImageGenerationAdapter
>
> ↓
>
> GenerationContextV1
>
> ↓
>
> Context Snapshot
>
> ↓
>
> Job + RabbitMQ
>
> ↓
>
> Python Worker
>
> ↓
>
> Mock Candidate
>
> ↓
>
> StoryDiffusionAdapter
>
> ↓
>
> Validation
>
> ↓
>
> SAMIM

# **150. Implementation Context for AI Coding Agents**

Mỗi coding task chỉ đưa:

> DEV-01
>
> Relevant IMPLEMENT-02 section
>
> Current SPRINT section
>
> Relevant Domain/API/Data/AI specification
>
> Existing code

Không gửi toàn bộ docs nếu không cần.

# **151. Code Review Priorities**

Review theo:

> Business correctness
>
> Architecture boundary
>
> Data integrity
>
> Concurrency
>
> Security
>
> Failure handling
>
> Tests
>
> Readability

UI polish không được ưu tiên hơn data integrity.

# **152. MVP Completion Definition**

Application MVP được xem là hoàn thành khi user có thể:

> Create Project
>
> ↓
>
> Initialize Story
>
> ↓
>
> Create Characters
>
> ↓
>
> Upload References
>
> ↓
>
> Create Scenes
>
> ↓
>
> Assign Characters
>
> ↓
>
> Set Scene State
>
> ↓
>
> Generate
>
> ↓
>
> Track Job
>
> ↓
>
> Review Candidates
>
> ↓
>
> Select
>
> ↓
>
> Regenerate
>
> ↓
>
> View History
>
> ↓
>
> Edit Canon
>
> ↓
>
> Generate Later Scene with Updated Context

# **153. Research Completion Definition**

Research integration hoàn thành khi:

> Baseline reproducible
>
> \+
>
> Gap evidenced
>
> \+
>
> SAMIM implemented
>
> \+
>
> Controlled comparison
>
> \+
>
> Ablation
>
> \+
>
> Evaluation
>
> \+
>
> Traceable provenance

# **154. Thesis System Completion**

Thesis system hoàn thành khi cả hai tuyến gặp nhau:

> Application MVP
>
> \+
>
> Research Contribution
>
> \+
>
> QA
>
> \+
>
> Deployment
>
> \+
>
> Demo Readiness

# **155. Final Implementation Model**

Có thể hiểu OWNIVERSE qua ba lõi:

> Canonical Creative Core
>
> Project / Story / Character / Scene
>
> ↓
>
> Traceability Core
>
> Asset / Version / GenerationContext / Snapshot
>
> ↓
>
> AI Execution Core
>
> Job / RabbitMQ / Worker / Adapter / Validation / Candidate

Research method nằm trong AI Execution Core nhưng không sở hữu Canonical Creative Core.

# **156. Final Principle**

IMPLEMENT-01 tuân theo:

> **Canon first.**
>
> **Structured Context before Prompt.**
>
> **Snapshot before Generation.**
>
> **Mock before Real AI.**
>
> **Baseline before Proposed Method.**
>
> **Candidate before Canonical Acceptance.**
>
> **Application and Research evolve separately, integrate through Adapter.**
>
> **Testing happens throughout implementation, not at the end.**
>
> **Every milestone must leave behind a working, traceable system.**
