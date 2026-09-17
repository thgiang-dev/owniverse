# **IMPLEMENT-02 — Module Implementation & Dependency Plan**

**Document ID:** IMPLEMENT-02  
**Document Type:** Module Implementation & Dependency Plan  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1 — Updated  
**Primary Roles:** Technical Lead / Backend Developer / Frontend Developer / AI Engineer / AI Coding Assistant  
**Parent Documents:** IMPLEMENT-01, DEV-01  
**Related Documents:** PROD-01, PROD-02, ARCH-01, ARCH-02, DOMAIN-01, DOMAIN-02, AI-01, AI-02, AI-03, DATA-01, DATA-02, API-01, API-02, QA-01, QA-03, DEPLOY-01, DEPLOY-02, SPRINT-01

# **1. Purpose**

IMPLEMENT-02 phân rã roadmap của OWNIVERSE xuống mức module và dependency triển khai.

Tài liệu này trả lời:

- module nào phải xây trước;

- module nào phụ thuộc module nào;

- mỗi module cần Domain/Persistence/Application/API/Frontend/Test gì;

- cross-cutting component nào phải xuất hiện trước AI generation;

- Research Adapter tích hợp vào đâu;

- Definition of Ready/Done cho từng module.

IMPLEMENT-02 không thay thế Domain/Data/API/AI specifications.

# **2. Module Implementation Pattern**

Mỗi module được mô tả theo cấu trúc:

Responsibility

↓

Dependencies

↓

Domain

↓

Persistence

↓

Application

↓

API

↓

Frontend

↓

Tests

↓

Definition of Done

Không phải mọi module đều có UI riêng.

# **3. Locked Stack Assumptions**

Implementation plan dựa trên:

Vue 3 + Vite + TypeScript

ASP.NET Core Web API

PostgreSQL

RabbitMQ

Python AI Worker

IAssetStorage

├── LocalFileAssetStorage

└── S3AssetStorage

SignalR — optional

AI integration:

IImageGenerationService

ITextGenerationService

IConsistencyService

# **4. Core Dependency Order**

Identity / Access

↓

Project

↓

Story

↓

Character

↓

Asset

↓

Scene

↓

Versioning

↓

GenerationContextV1

↓

Context Snapshot

↓

Generation

↓

AI Integration

↓

Validation

↓

Candidate Selection / History

Research path:

Baseline Reproduction

↓

Failure Analysis

↓

SAMIM

↓

Adapter Integration

↓

Evaluation

# **5. Domain Relationship Overview**

User

↓

Project

↓

Story

├── Characters

├── Scenes

└── Assets

Scene ↔ Character:

Scene

↔

SceneCharacter

↔

Character

Generation:

Request

↓

Job

↓

Attempt

↓

Candidate

↓

Asset

Traceability:

Job

↓

Context Snapshot

↓

Story / Character / Scene Versions

# **6. M01 — Identity & Access**

## **Responsibility**

Cung cấp current authenticated user context và ownership foundation.

Không nhằm xây full enterprise IAM.

# **7. M01 Dependencies**

Không phụ thuộc business module khác.

# **8. M01 Domain / Application**

Minimum concepts:

CurrentUser

UserId

Ownership Check

Authorization Policy

# **9. M01 Persistence**

Nếu authentication provider đã quản lý identity ngoài DB, OWNIVERSE chỉ cần mapping phù hợp.

Nếu cần local user table, giữ tối giản.

# **10. M01 API**

API phải biết:

- request thuộc user nào;

- user có quyền Project/resource hay không.

# **11. M01 Frontend**

Frontend chỉ dùng auth/session UX.

Không coi hidden UI là authorization.

# **12. M01 Tests**

\[ \] authenticated user can access own Project

\[ \] user cannot access another user's Project

\[ \] unauthenticated request denied where required

# **13. M01 Definition of Done**

M01 Done khi mọi module sau có thể gọi trusted current-user abstraction và ownership check.

# **14. M02 — Project Module**

## **Responsibility**

Quản lý top-level creative workspace.

# **15. M02 Dependencies**

M01 Identity/Access

# **16. M02 Domain**

Primary entity:

Project

Likely properties:

- projectId;

- ownerId;

- title;

- description/metadata as specified;

- status;

- revisionNumber;

- timestamps.

Exact domain shape theo DOMAIN-01.

# **17. M02 Persistence**

projects

Migration phải version controlled.

# **18. M02 Application Use Cases**

CreateProject

GetProject

ListProjects

UpdateProject

ArchiveProject

nếu Archive nằm trong current scope.

# **19. M02 API**

Expose business-oriented endpoints under:

/api/v1

JSend + camelCase + stable errorKey.

# **20. M02 Frontend**

Minimum:

Project List

Create Project

Open Project

Project Settings

# **21. M02 Tests**

Create Project

Get Project

Update Project

Revision Conflict

Ownership

Reload Persistence

# **22. M02 Definition of Done**

Create

→ Persist

→ Open

→ Update

→ Reload

hoạt động với authorization.

# **23. M03 — Story Module**

## **Responsibility**

Quản lý Story và Story Core canonical content.

# **24. M03 Dependencies**

M01

M02

# **25. M03 Domain**

Primary concepts:

Story

Story Core

Premise

Synopsis

World

Facts

Relationships

Style

Exact modeling theo DOMAIN-01.

# **26. M03 Persistence**

Minimum current tables/models:

stories

story_core

Hoặc equivalent normalized model theo DATA-01.

# **27. M03 Application**

InitializeStory

GetStory

UpdateStoryCore

AI proposal không update Canon trực tiếp.

# **28. M03 API**

Story API phải thể hiện business intent, không expose raw persistence structure.

# **29. M03 Frontend**

Minimum:

Story Overview

Story Core Editor

# **30. M03 Tests**

Initialize Story

Update Story Core

Revision Conflict

Ownership

Reload

# **31. M03 Definition of Done**

Project có một Story Core canonical có thể sửa và reload ổn định.

# **32. M04 — Character Module**

## **Responsibility**

Quản lý recurring Character identity.

# **33. M04 Dependencies**

M01

M02

M03

# **34. M04 Domain**

Primary:

Character

Phân biệt:

Stable Identity

Canonical Appearance

Locked Attributes

với Scene-specific state.

# **35. M04 Persistence**

characters

Later:

character_versions

# **36. M04 Application**

CreateCharacter

GetCharacter

ListCharacters

UpdateCharacter

# **37. M04 API**

Không cho AI/provider-specific field leak vào Character API.

# **38. M04 Frontend**

Character List

Character Detail

Character Editor

# **39. M04 Tests**

Create

Update

Revision conflict

Stable identity persistence

Ownership

# **40. M04 Definition of Done**

Recurring Characters có canonical identity rõ và không phụ thuộc Scene.

# **41. M05 — Asset Module**

## **Responsibility**

Quản lý metadata và binary asset access.

# **42. M05 Dependencies**

M01

M02

Character references và generated images phụ thuộc Asset.

# **43. M05 Domain**

Primary:

Asset

Potential metadata:

assetId

projectId

storageKey

mediaType

size

checksum

sourceType

createdAt

# **44. M05 Persistence**

Database lưu metadata.

Binary storage qua:

IAssetStorage

# **45. M05 Storage Implementations**

Development:

LocalFileAssetStorage

Demo/production-like:

S3AssetStorage

Preferred provider hiện tại:

Cloudflare R2

# **46. M05 Application**

UploadAsset

RegisterGeneratedAsset

GetAssetMetadata

GetAuthorizedAssetAccess

# **47. M05 API**

Upload/access API phải enforce ownership.

# **48. M05 Frontend**

Common needs:

Upload

Preview

Asset display

# **49. M05 Tests**

Upload binary

Persist metadata

Read asset

Ownership denied

Storage failure

Missing object handling

# **50. M05 Definition of Done**

Binary và metadata được tách đúng, access controlled, provider replaceable.

# **51. M04A — Character Reference Capability**

Character Reference có thể nằm trong Character module nhưng phụ thuộc Asset.

Flow:

Character

↓

Upload Asset

↓

CharacterReference

↓

Select Canonical Reference

# **52. Character Reference Persistence**

character_references

Reference record map Character → Asset.

# **53. Character Reference Application**

AddCharacterReference

ListCharacterReferences

SelectCanonicalReference

# **54. Character Reference Tests**

Upload ref

Link ref

Select canonical

Reload

Unauthorized asset rejected

# **55. M06 — Scene Module**

## **Responsibility**

Quản lý scene sequence và scene-specific state.

# **56. M06 Dependencies**

M02

M03

M04

Asset dependency optional nếu Scene trực tiếp tham chiếu visual assets.

# **57. M06 Domain**

Primary:

Scene

SceneCharacter

# **58. M06 Scene**

Scene có:

- position/order;

- description;

- intent;

- environment/context;

- canonical story state as specified.

# **59. M06 SceneCharacter**

SceneCharacter giữ:

characterId

sceneId

outfit

emotion

action

temporary appearance

pose/action intent

other scene state

Không rewrite Character stable identity.

# **60. M06 Persistence**

scenes

scene_characters

Later:

scene_versions

# **61. M06 Application**

CreateScene

UpdateScene

ReorderScenes

AssignCharacterToScene

RemoveCharacterFromScene

UpdateSceneCharacterState

# **62. M06 API**

API phải thể hiện assignment/state intent.

Không bắt frontend sửa raw join-table row tùy ý.

# **63. M06 Frontend**

Minimum:

Scene List

Scene Editor

Scene Reorder

Active Character Assignment

Scene Character State Editor

# **64. M06 Tests**

Create Scene

Reorder Scene

Assign Character

Multiple Characters

Character in multiple Scenes

SceneCharacter state persistence

Remove assignment without deleting Character

# **65. M06 Definition of Done**

Story sequence có recurring Characters và scene-specific state rõ.

# **66. C01 — Versioning Cross-Cutting Component**

## **Responsibility**

Lưu historical canonical states cần cho generation traceability.

# **67. C01 Dependencies**

Story

Character

Scene

# **68. C01 Initial Version Types**

MVP:

StoryCoreVersion

CharacterVersion

SceneVersion

# **69. C01 Persistence**

story_core_versions

character_versions

scene_versions

# **70. C01 Rule**

Historical version đã được generation sử dụng:

> immutable.

# **71. C01 Application Rule**

Canonical update cần version:

Update Canon

\+

Create Version

atomic khi appropriate.

# **72. C01 Tests**

Update creates new version

Old version unchanged

Latest Canon correct

Version ownership traceable

# **73. C01 Definition of Done**

Hệ thống có thể chỉ ra generation dùng exact Character/Scene/Story version nào.

# **74. C02 — Generation Context Component**

## **Responsibility**

Xây structured input cho AI từ Canon.

# **75. C02 Dependencies**

Story

Character

Character Reference

Scene

SceneCharacter

Versioning

Asset

# **76. C02 Main Contract**

GenerationContextV1

Raw prompt không phải application contract.

# **77. C02 GenerationContextV1 Content**

Minimum logical shape:

schemaVersion

story

scene

characters\[\]

continuity

style

userInstruction

generationOptions

Character context gồm:

characterId

stableIdentity

canonicalReferences

sceneState

Exact schema theo AI-02.

# **78. C02 Priority Rule**

Resolve conflict:

Canonical / Locked Identity

↓

Timeline / Scene State

↓

User Instruction

↓

Soft Context

↓

Model Assumption

# **79. C02 Context Builder**

Implement:

GenerationContextBuilder

Responsibilities:

- resolve Story context;

- resolve current Scene;

- resolve active Characters;

- resolve references;

- resolve state;

- include relevant continuity;

- exclude irrelevant project data;

- validate required context.

# **80. C02 Tests**

Only active characters selected

Correct canonical reference selected

Scene state overrides soft context

User instruction cannot silently override locked identity

Irrelevant Character excluded

# **81. C03 — Context Snapshot Component**

## **Responsibility**

Freeze GenerationContext for async execution.

# **82. C03 Dependencies**

C01 Versioning

C02 GenerationContext

# **83. C03 Persistence**

context_snapshots

context_snapshot_dependencies

# **84. C03 Rule**

Snapshot immutable after generation starts.

# **85. C03 Dependency Records**

Record dependencies on:

StoryCoreVersion

CharacterVersion

SceneVersion

CharacterReference / Asset

as applicable.

# **86. C03 Tests**

Snapshot persists

Canon edit after snapshot does not mutate snapshot

Dependencies correct

Snapshot reload exact input

# **87. C03 Definition of Done**

Generation can execute later without reading mutable live Canon.

# **88. M07 — Generation Module**

## **Responsibility**

Manage Job / Attempt / Candidate lifecycle.

# **89. M07 Dependencies**

Scene

Asset

Versioning

GenerationContext

ContextSnapshot

# **90. M07 Domain**

Primary:

GenerationJob

GenerationAttempt

Candidate

# **91. M07 Persistence**

Expected:

generation_jobs

generation_attempts

candidates

Exact schema from DATA/API specs.

# **92. M07 Application**

CreateGeneration

GetJob

CancelJob

SelectCandidate

Regenerate

GetGenerationHistory

Retry path can be internal/runtime application path.

# **93. M07 Job Creation**

Transaction A:

Create Context Snapshot

Create Job

Commit

sau đó publish RabbitMQ.

# **94. M07 Queue Publish**

Infrastructure:

RabbitMqGenerationJobPublisher

hoặc equivalent naming.

Application không depend RabbitMQ SDK trực tiếp.

# **95. M07 Queue Message**

Should contain IDs:

jobId

attemptId

operationType

Không serialize toàn GenerationContext vào queue nếu Snapshot đã persisted.

# **96. M07 Candidate**

Candidate references:

Job

Attempt

Scene

Asset

Validation

Provenance

Candidate không mặc định selected.

# **97. M07 Frontend**

Minimum:

Generate Action

Job Progress

Candidate Gallery

Candidate Select

Regenerate

History

# **98. M07 Tests**

202 Accepted

Job persisted

Queue publish

Candidate persisted

Selection

History

Cancel

Failure

Reload

# **99. M07 Definition of Done**

Mock AI generation end-to-end hoạt động trước real model.

# **100. M08 — AI Integration Module**

## **Responsibility**

Kết nối Product generation workflow với AI implementations.

# **101. M08 Dependencies**

M07 Generation

C03 Context Snapshot

M05 Asset

# **102. M08 Core Interfaces**

IImageGenerationService

ITextGenerationService

IConsistencyService

# **103. M08 Worker**

Python Worker thực thi heavy AI operations.

Suggested logical structure:

jobs/

adapters/

context/

pipelines/

validation/

models/

infrastructure/

# **104. M08 Adapter Sequence**

MockImageGenerationAdapter

↓

StoryDiffusionAdapter

↓

ProposedMethodAdapter / SAMIM

Optional:

ExternalImageApiAdapter

# **105. M08 Adapter Input Rule**

Adapter nhận structured Snapshot.

Context Snapshot

↓

Adapter

↓

Prompt Builder / Condition Builder

↓

Model Input

# **106. M08 Prompt Ownership**

Prompt Builder thuộc Adapter.

Application và frontend không sở hữu technical model prompt.

# **107. M08 StoryDiffusion Adapter**

Responsibilities:

- map context to baseline input;

- load canonical references;

- render baseline prompt/condition;

- run model;

- normalize output;

- produce provenance.

# **108. M08 SAMIM Adapter**

Chỉ tích hợp sau confirmed research gap.

Responsibilities có thể gồm:

- retrieve per-character identity memory;

- active-character conditioning;

- scene-aware retrieval;

- identity isolation;

- identity/appearance separation.

Exact method theo RESEARCH-03.

# **109. M08 External API Adapter**

Nếu dùng external provider:

- provider SDK/API nằm trong adapter;

- normalize provider errors;

- never expose secret to frontend;

- preserve same Candidate flow.

# **110. M08 Provenance**

Persist when applicable:

contextSchemaVersion

adapterVersion

modelId

methodVersion

promptTemplateVersion

conditionBuilderVersion

seed

workerVersion

# **111. M08 Tests**

Mock adapter:

success

failure

delay

warning

Baseline adapter:

small controlled real run

metadata/provenance

failure normalization

# **112. M08 Definition of Done**

Switch Adapter mà Scene/Generation API flow không đổi.

# **113. C04 — Validation Component**

## **Responsibility**

Đánh giá AI output trước Candidate decision.

# **114. C04 Dependencies**

AI Integration

Candidate

Asset

Context Snapshot

# **115. C04 Pipeline**

Logical sequence:

Technical Validation

↓

Contract Validation

↓

Semantic Validation

↓

Consistency Validation

↓

Safety Validation

↓

Candidate Decision

MVP có thể implement subset theo AI-03.

# **116. C04 Output**

Validation record SHOULD include:

type

severity

result

score/label if applicable

methodVersion

details

# **117. C04 Severity**

INFO

WARNING

ERROR

BLOCKING

# **118. C04 Candidate Decisions**

May include:

ACCEPT

REVIEW_RECOMMENDED

REGENERATE_RECOMMENDED

REJECT

# **119. C04 Runtime vs Research**

Runtime validation:

> product safety/quality gate.

RESEARCH-04:

> scientific evaluation.

Không merge metrics blindly.

# **120. C04 Tests**

Technical invalid output rejected

Warning surfaced

Blocking prevents selection

Validation persisted

Version metadata recorded

# **121. C05 — Dependency Tracking & Stale Context**

## **Responsibility**

Biết output nào trở nên outdated khi Canon thay đổi.

# **122. C05 Dependencies**

ContextSnapshotDependency

Versioning

Candidate

# **123. C05 Rule**

Canonical edit:

does not delete Candidate

does not mutate old Snapshot

does not auto-regenerate whole Story

# **124. C05 Behavior**

Canon edited

↓

Create new Version

↓

Find affected Snapshot dependencies

↓

Mark relevant Candidate/Generation OUTDATED_CONTEXT

Exact state name theo Domain/API contract.

# **125. C05 Tests**

Relevant Candidate becomes stale

Unrelated Candidate unaffected

History preserved

New generation uses new version

# **126. M09 — AI-Assisted Content Module**

## **Responsibility**

P1 text-generation proposal features.

# **127. M09 Dependencies**

Story

Scene

ITextGenerationService

# **128. M09 Flow**

User requests proposal

↓

Build relevant structured context

↓

Text AI

↓

Proposal

↓

Review/Edit

↓

Application Command

↓

Canon

# **129. M09 Rule**

Proposal:

≠ Canon

# **130. M09 Tests**

Proposal generated

Proposal not auto-canonical

User accepts

Application command updates Canon

# **131. Research Adapter — SAMIM**

SAMIM không phải Product business module.

Nó là:

> Research implementation behind AI Adapter boundary.

# **132. Research Adapter Dependencies**

RESEARCH-02 confirmed gap

RESEARCH-03 method

Context Snapshot

Character References

Scene State

AI Worker

# **133. Research Adapter Build Order**

V0 Baseline

V1 Per-Character Memory

V2 Scene-Aware Retrieval

V3 Isolation

V4 Identity/Appearance Separation

Variants thực tế phải follow evidence.

# **134. Research Adapter Tests**

Product integration tests:

same API workflow

correct provenance

failure safe

Candidate coexistence

Scientific tests thuộc RESEARCH-04.

# **135. Dependency Matrix**

| **Component**       | **Depends On**                               |
|---------------------|----------------------------------------------|
| Identity            | —                                            |
| Project             | Identity                                     |
| Story               | Project                                      |
| Character           | Project/Story                                |
| Asset               | Identity/Project                             |
| Character Reference | Character + Asset                            |
| Scene               | Story + Character                            |
| Versioning          | Story + Character + Scene                    |
| GenerationContext   | Versions + Scene + Character + References    |
| Context Snapshot    | GenerationContext + Versioning               |
| Generation          | Snapshot + Asset + Scene                     |
| AI Integration      | Generation + Snapshot + Asset                |
| Validation          | AI Integration + Candidate                   |
| Stale Tracking      | Snapshot Dependencies + Versions + Candidate |
| AI-Assisted Content | Story/Scene + Text AI                        |
| SAMIM               | AI Integration + Research Method             |

# **136. Critical Path**

Identity

↓

Project

↓

Story

↓

Character

↓

Asset

↓

Scene

↓

Versioning

↓

GenerationContextV1

↓

Context Snapshot

↓

Generation

↓

Mock AI

↓

Candidate

↓

Selection / History

Real AI:

Mock AI

↓

StoryDiffusion

↓

Basic Validation

↓

Real Candidate

Research:

Baseline Reproduction

↓

Failure Analysis

↓

SAMIM

↓

Adapter

↓

Experiment

# **137. Integration Checkpoint IC-01**

### **Project + Story**

Expected:

Create Project

Initialize Story

Edit

Reload

# **138. IC-02**

### **Character + Reference**

Expected:

Create Character

Upload Asset

Add Reference

Select Canonical Reference

Reload

# **139. IC-03**

### **Scene**

Expected:

Create Scene

Assign Character

Set state

Reorder

Reload

# **140. IC-04**

### **Snapshot**

Expected:

Build GenerationContextV1

Create Snapshot

Edit Canon

Old Snapshot unchanged

# **141. IC-05**

### **Mock Generation**

Expected:

Scene

→ Snapshot

→ Job

→ RabbitMQ

→ Worker

→ Mock

→ Candidate

→ Select

# **142. IC-06**

### **Baseline**

Expected:

Same workflow

→ StoryDiffusionAdapter

→ Real Candidate

# **143. IC-07**

### **Canon Change / Stale**

Expected:

Old Candidate retained

Relevant output stale

New generation uses latest Canon

# **144. IC-08**

### **SAMIM**

Expected:

Baseline and SAMIM use same Product workflow

# **145. Definition of Ready — Module**

Module ready khi:

\[ \] Responsibility clear

\[ \] Dependencies implemented

\[ \] Domain contract clear

\[ \] Relevant API/Data spec exists

\[ \] Acceptance criteria defined

# **146. Definition of Done — Domain Module**

\[ \] Domain model

\[ \] Business rules

\[ \] Persistence

\[ \] Migration

\[ \] Application use cases

\[ \] API

\[ \] Frontend if applicable

\[ \] Tests

\[ \] Authorization

\[ \] Docs sync

# **147. Definition of Done — Cross-Cutting Component**

\[ \] Explicit contract

\[ \] Integration point

\[ \] Persistence if required

\[ \] Failure behavior

\[ \] Tests

\[ \] No hidden cross-module coupling

# **148. Definition of Done — AI Adapter**

\[ \] Implements agreed abstraction

\[ \] Receives structured Snapshot

\[ \] Owns prompt/condition rendering

\[ \] Does not mutate Canon

\[ \] Normalizes output

\[ \] Persists provenance

\[ \] Handles provider/model failure

\[ \] Integration test exists

# **149. Implementation Package for AI Coding — Project**

Provide:

DEV-01

IMPLEMENT-02 M02

DOMAIN Project section

DATA Project section

API Project section

Current code

# **150. AI Coding Package — Character**

DEV-01

IMPLEMENT-02 M04/M04A

DOMAIN Character

DATA Character/Asset

API Character

UX Character

Current code

# **151. AI Coding Package — Scene**

DEV-01

IMPLEMENT-02 M06

DOMAIN Scene/SceneCharacter

DATA Scene

API Scene

UX Scene

Current code

# **152. AI Coding Package — Context Snapshot**

DEV-01

IMPLEMENT-02 C01/C02/C03

DOMAIN-02

AI-02

DATA-02

API-02

Current code

# **153. AI Coding Package — Generation**

DEV-01

IMPLEMENT-02 M07/M08

ARCH-02

AI-01

AI-03

DATA-02

API-02

QA-03

Current code

# **154. AI Coding Package — SAMIM**

DEV-01

IMPLEMENT-02 Research Adapter

RESEARCH-02

RESEARCH-03

RESEARCH-04

AI-01

AI-02

AI-03

Existing StoryDiffusionAdapter

# **155. MVP Practical Build Order**

Recommended:

01 Identity

02 Project

03 Story

04 Character

05 Asset

06 Character Reference

07 Scene

08 Versioning

09 GenerationContextV1

10 Context Snapshot

11 Generation Job

12 RabbitMQ Worker

13 Mock Adapter

14 Candidate Selection/History

15 StoryDiffusion Adapter

16 Validation/Stale

17 P1 Text AI

18 SAMIM

19 QA/Deployment

# **156. Why Asset Precedes Generation**

Generation output cần durable binary target.

Không build real generation trước storage abstraction.

# **157. Why Versioning Precedes Snapshot**

Snapshot phải reference immutable historical state.

Nếu chỉ reference live Character row, reproducibility yếu.

# **158. Why GenerationContext Precedes Snapshot**

Snapshot là frozen representation của structured generation context.

Không freeze raw prompt làm primary truth.

# **159. Why Prompt Rendering Belongs to Adapter**

Model khác nhau cần input khác nhau.

Ví dụ:

StoryDiffusion

SAMIM

External API

không nhất thiết dùng cùng prompt string.

# **160. Why RabbitMQ Is Infrastructure**

Business domain chỉ cần:

> asynchronous generation execution.

RabbitMQ là current transport implementation.

Không leak broker type vào domain entities.

# **161. Why SignalR Is Optional**

Job source of truth là persisted backend state.

SignalR chỉ cải thiện progress UX.

Frontend reload vẫn phải recover Job state qua API.

# **162. Research/Product Separation**

Product defines:

GenerationContext

Job

Candidate

Selection

History

Research defines:

How visual identity conditioning works inside SAMIM

Không đảo responsibility.

# **163. Scope Protection**

Module không được tự kéo thêm:

- branching UI;

- comic page editor;

- collaboration;

- plugin marketplace;

- microservice split;

- Kubernetes;

- full event sourcing.

# **164. Module Ownership Rule**

Module chỉ thay state mình sở hữu hoặc gọi explicit application contract.

Không arbitrary DB cross-write.

# **165. Database Rule**

Schema changes:

> migration required.

Không để AI agent chỉnh database thủ công.

# **166. Security Rule**

Authorization đi cùng module implementation.

Không chờ cuối dự án mới thêm ownership.

# **167. Test Rule**

Mỗi integration checkpoint phải có automated test ở mức phù hợp.

Không coi manual click-through là test duy nhất.

# **168. Failure Rule**

Module phải define failure behavior trước khi Done.

Ví dụ Generation:

- queue unavailable;

- worker fail;

- storage fail;

- model fail;

- validation fail.

# **169. Provenance Rule**

AI Candidate không đủ nếu chỉ có image URL.

Phải truy vết được generation configuration cần thiết.

# **170. Final Dependency Model**

OWNIVERSE có thể được hình dung:

Canonical Creative Core

Project

Story

Character

Scene

↓

Traceability Core

Asset

Version

GenerationContext

Context Snapshot

↓

AI Execution Core

Generation Job

RabbitMQ

Python Worker

Adapter

Validation

Candidate

↓

User Control

Review

Select

Regenerate

History

SAMIM nằm trong Adapter/AI Execution Core.

# **171. Final Principle**

IMPLEMENT-02 áp dụng các nguyên tắc:

> **Module boundaries trước convenience.**
>
> **Canon trước AI.**
>
> **Asset + Version + Structured Context trước Generation.**
>
> **Snapshot trước Queue execution.**
>
> **Queue trước Worker, nhưng Queue không phải Job truth.**
>
> **Mock trước Baseline.**
>
> **Baseline trước SAMIM.**
>
> **Prompt/Condition rendering thuộc Adapter.**
>
> **Candidate trước Canonical Acceptance.**
>
> **Mỗi module chỉ được coi là Done khi chạy được trong dependency chain thật.**
