# **DEV-01 — Repository, Coding & Engineering Convention**

**Document ID:** DEV-01  
**Document Type:** Repository, Coding & Engineering Convention  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1 — Updated  
**Primary Roles:** Technical Lead / Backend Developer / Frontend Developer / AI Engineer / AI Coding Assistant  
**Parent Documents:** ARCH-01, ARCH-02, DOMAIN-01, DOMAIN-02, API-01, API-02, AI-01, AI-02, AI-03  
**Related Documents:** DATA-01, DATA-02, QA-01, DEPLOY-01, DEPLOY-02, DEPLOY-03, IMPLEMENT-01, IMPLEMENT-02, SPRINT-01

# **1. Purpose**

DEV-01 định nghĩa các quy ước kỹ thuật dùng xuyên suốt quá trình triển khai OWNIVERSE.

Tài liệu này đóng vai trò như một:

> **Engineering Contract**

giữa:

- developer;

- AI coding assistant;

- backend;

- frontend;

- AI worker;

- persistence;

- runtime infrastructure.

Mục tiêu là bảo đảm code được tạo ra theo cùng một kiến trúc, cùng vocabulary và không âm thầm phá các quyết định đã khóa trong Product, Domain, Architecture, Data, API và AI specifications.

DEV-01 không thay thế các tài liệu đặc tả nghiệp vụ hoặc kiến trúc cấp cao hơn.

# **2. Locked Technology Stack**

Stack implementation chính thức của OWNIVERSE:

Frontend

Vue 3 + Vite + TypeScript

Backend

ASP.NET Core Web API

Database

PostgreSQL

Queue

RabbitMQ

AI Worker

Python

Asset Storage

IAssetStorage

├── LocalFileAssetStorage — development

└── S3AssetStorage — thesis demo / production-like

Preferred S3-Compatible Provider

Cloudflare R2

Realtime

SignalR — optional

Image AI Abstraction

IImageGenerationService

Text AI Abstraction

ITextGenerationService

Consistency Abstraction

IConsistencyService

Không thay đổi stack này trong implementation nếu chưa có quyết định kiến trúc rõ ràng.

# **3. Core Engineering Guardrails**

Các invariant sau là bắt buộc:

Frontend ≠ Canon business owner

Controller ≠ Business Logic

Domain ≠ Infrastructure

Application ≠ Provider Implementation

AI Worker ≠ Canon Owner

Candidate ≠ Canon

Queue ≠ Job Source of Truth

Realtime ≠ Job Source of Truth

Cache ≠ Canon Truth

Rendered Prompt ≠ Application Contract

# **4. Source-of-Truth Hierarchy**

Khi có mâu thuẫn trong implementation, ưu tiên:

Product Requirements

↓

Architecture / Domain Specifications

↓

Data / API / AI Specifications

↓

IMPLEMENT-01 / IMPLEMENT-02

↓

DEV-01

↓

SPRINT-01

↓

Task-level Prompt

AI coding assistant không được dùng task prompt để override business rule từ tài liệu cấp cao hơn.

# **5. Repository Structure**

Repository đề xuất:

/

├── frontend/

├── backend/

├── ai-worker/

├── tests/

├── deployment/

├── docs/

├── scripts/

├── .github/

├── README.md

└── .gitignore

# **6. Frontend Structure**

Recommended:

frontend/

├── src/

│ ├── app/

│ ├── features/

│ ├── components/

│ ├── layouts/

│ ├── router/

│ ├── stores/

│ ├── services/

│ ├── api/

│ ├── types/

│ ├── utils/

│ └── assets/

├── public/

├── tests/

├── package.json

├── tsconfig.json

└── vite.config.ts

Frontend stack:

Vue 3

Vite

TypeScript

Feature-oriented organization được ưu tiên hơn việc gom toàn bộ code theo technical type.

# **7. Backend Structure**

Recommended logical structure:

backend/

├── src/

│ ├── Api/

│ ├── Modules/

│ │ ├── Projects/

│ │ ├── Stories/

│ │ ├── Characters/

│ │ ├── Scenes/

│ │ ├── Assets/

│ │ ├── Generations/

│ │ └── Validation/

│ ├── Shared/

│ ├── Infrastructure/

│ └── Persistence/

└── tests/

Không bắt buộc mọi module có cùng số folder nếu không cần.

# **8. Backend Layering**

Logical layers:

Presentation

↓

Application

↓

Domain

↓

Infrastructure / Persistence

AI Application capability có thể đứng cạnh Application layer nhưng không xâm nhập Domain.

# **9. Domain Layer Rule**

Domain Layer SHOULD chứa:

- entities;

- value objects;

- domain rules;

- domain state transitions;

- domain errors;

- domain services khi thực sự cần.

Domain Layer SHALL NOT phụ thuộc trực tiếp vào:

- EF Core;

- RabbitMQ;

- S3/R2 SDK;

- HTTP client;

- StoryDiffusion;

- SAMIM implementation;

- external AI provider;

- Vue/frontend.

# **10. Application Layer Rule**

Application Layer chịu trách nhiệm:

- use cases;

- orchestration business flow;

- authorization/ownership integration;

- canonical update commands;

- snapshot creation workflow;

- job creation workflow;

- candidate selection;

- revision/version checks.

Application Layer không chứa provider-specific inference logic.

# **11. Infrastructure Layer Rule**

Infrastructure chứa implementation cụ thể cho:

- PostgreSQL/ORM;

- RabbitMQ;

- S3-compatible storage;

- local file storage;

- external APIs;

- AI adapter transport;

- logging;

- authentication infrastructure.

Infrastructure không được trở thành nơi định nghĩa business rule.

# **12. Feature Modules**

Ưu tiên feature modules:

Projects

Stories

Characters

Scenes

Assets

Generations

Validation

Không tạo một architecture kiểu:

Controllers/

Services/

Repositories/

Models/

cho toàn bộ project nếu điều đó làm mất module boundary.

# **13. Module Ownership**

Mỗi module chịu trách nhiệm cho domain của chính nó.

Một module không được tùy tiện đọc/ghi trực tiếp table thuộc module khác nếu có Application/API/module contract phù hợp.

# **14. Internal Module Communication**

Communication giữa module nên đi qua:

- application interface;

- domain event;

- internal message;

- explicit query service.

Không phụ thuộc chéo bằng cách inject DbContext rồi truy cập mọi table.

# **15. Transaction Boundary**

Transaction chỉ bao phủ operation cần atomicity.

Ví dụ:

Update Canon

\+

Create Version

có thể cùng transaction.

Không giữ database transaction xuyên qua:

AI inference

network call

remote model generation

long-running worker execution

# **16. Async Generation Transaction Pattern**

Recommended:

Transaction A

Create Context Snapshot

Create Job

Persist

COMMIT

↓

Queue / Worker / AI

↓

Transaction B

Persist Asset Metadata

Persist Candidate

Persist Validation

Update Job

COMMIT

Không mở transaction từ trước inference rồi chờ model.

# **17. Canonical State Rule**

Canonical creative state bao gồm các entity/domain state được Application công nhận là chính thức.

AI output không tự động trở thành Canon.

# **18. Candidate Rule**

Candidate là:

> AI-generated output available for review.

Candidate chỉ trở thành selected output thông qua explicit Application use case.

# **19. AI Canon Boundary**

Worker và AI adapters:

- không update Character Canon trực tiếp;

- không update Scene Canon trực tiếp;

- không update Story Canon trực tiếp;

- không tự select Candidate.

AI có thể tạo:

- proposal;

- candidate;

- validation result;

- diagnostics.

Application quyết định Canon.

# **20. Stable Domain Vocabulary**

Dùng nhất quán các thuật ngữ:

Project

Story

Story Core

Character

Character Reference

Scene

SceneCharacter

Asset

Version

Context Snapshot

Generation Job

Generation Attempt

Candidate

Validation Result

Canon

Revision

Regeneration

Retry

Không tự đổi tên cùng một khái niệm giữa các module.

# **21. Avoid Vague Names**

Hạn chế:

Manager

Helper

UtilService

CommonService

DataService

AIService

nếu tên không nói rõ responsibility.

Ưu tiên:

GenerationContextBuilder

CandidateSelectionService

StoryDiffusionAdapter

RabbitMqJobPublisher

S3AssetStorage

# **22. C# Naming Convention**

Use standard C# conventions:

- PascalCase: class, record, method, public property;

- camelCase: local variable, parameter;

- interfaces: prefix I;

- async methods: suffix Async;

- cancellation token: CancellationToken cancellationToken.

Ví dụ:

public interface IImageGenerationService

{

Task\<ImageGenerationResult\> GenerateAsync(

GenerationContext context,

CancellationToken cancellationToken);

}

Exact signature có thể thay đổi theo implementation, nhưng naming rule giữ nguyên.

# **23. Python Naming Convention**

Python worker:

- snake_case: modules, functions, variables;

- PascalCase: classes;

- UPPER_SNAKE_CASE: constants.

Ví dụ:

class StoryDiffusionAdapter:

...

def build_generation_input(...):

...

# **24. TypeScript Naming Convention**

Vue/TypeScript:

- PascalCase: Vue component / type / interface;

- camelCase: function / variable;

- composables: useXxx;

- API modules: domain-oriented naming;

- avoid any khi có thể định nghĩa type rõ.

# **25. Database Naming**

Database naming SHOULD dùng:

snake_case

Ví dụ:

project_id

revision_number

context_snapshot_id

created_at

Naming phải nhất quán toàn schema.

# **26. Primary Keys**

UUID được ưu tiên cho externally referenced domain entities và API resources.

Không expose database auto-increment detail như business identity nếu không cần.

# **27. Time Handling**

API/domain persistence:

> ISO 8601 UTC.

Database timestamps SHOULD được lưu theo UTC convention.

Frontend chịu trách nhiệm presentation/localization.

# **28. API Base Path**

Public API:

/api/v1

Breaking public contract yêu cầu version strategy rõ ràng.

# **29. JSON Convention**

API JSON:

camelCase

Ví dụ:

{

"projectId": "...",

"revisionNumber": 5,

"createdAt": "..."

}

# **30. JSend Response Convention**

OWNIVERSE sử dụng JSend-style envelope:

2xx → success

4xx → fail

5xx → error

Frontend không parse random response shape cho từng endpoint.

# **31. Error Key Convention**

Machine-readable error:

errorKey

Ví dụ:

REVISION_CONFLICT

PROJECT_NOT_FOUND

FORBIDDEN

GENERATION_FAILED

CANDIDATE_BLOCKED

UI message có thể thay đổi mà không phá client logic.

# **32. HTTP Status Mapping**

Recommended:

200 OK

201 Created

202 Accepted

204 No Content

400 Bad Request

401 Unauthorized

403 Forbidden

404 Not Found

409 Conflict

422 Unprocessable Entity

500 Internal Server Error

503 Service Unavailable

Không ép mọi lỗi thành HTTP 200.

# **33. Controller Rule**

Controller SHOULD:

- parse request;

- call application use case;

- map result;

- return HTTP response.

Controller SHALL NOT chứa:

- business workflow phức tạp;

- direct AI inference;

- raw SQL;

- cross-module state mutation.

# **34. DTO Rule**

Public DTO phải tách khỏi persistence entity.

Không serialize EF entity trực tiếp làm API contract.

# **35. Command / Use-Case Naming**

Dùng business intent:

CreateProject

UpdateCharacter

AssignCharacterToScene

CreateGeneration

SelectCandidate

RegenerateSceneImage

Tránh:

SaveData

UpdateRecord

ProcessAI

# **36. Revision vs Version**

Phân biệt:

### **Revision**

Concurrency/edit state hiện tại.

Ví dụ:

revision_number

### **Version**

Historical canonical state đáng lưu để truy vết generation.

Không dùng hai thuật ngữ này thay thế nhau.

# **37. Optimistic Concurrency**

Update quan trọng SHOULD gửi expected revision.

Ví dụ:

expectedRevision = 5

Nếu current revision khác:

409 Conflict

errorKey = REVISION_CONFLICT

Không silently overwrite concurrent update.

# **38. Version Creation Rule**

Khi canonical update cần historical traceability:

Update Canon

\+

Create immutable Version

nên diễn ra atomically.

Historical version đã được generation sử dụng không được mutate.

# **39. Context Contract Principle**

OWNIVERSE không lấy raw prompt string làm contract giữa Application và AI.

Contract chính:

> GenerationContextV1

# **40. GenerationContextV1**

GenerationContextV1 SHOULD chứa structured data cần cho generation, ví dụ:

schemaVersion

project/story context

scene

active characters

canonical references

scene-specific appearance/state

continuity

style

user instruction

generation options

Exact fields được AI-02 quản lý.

# **41. Context Priority Rule**

Khi resolve conflict:

Canonical / Locked Identity

↓

Timeline / Scene-Specific State

↓

User Instruction

↓

Soft Context

↓

Model Assumption

User instruction không được âm thầm phá locked canonical identity.

# **42. Context Snapshot Rule**

Trước async generation:

GenerationContextV1

↓

Immutable Context Snapshot

Snapshot phải giữ nguyên xuyên suốt Job.

Canonical edit sau đó không mutate Snapshot cũ.

# **43. Prompt Builder Rule**

Prompt/condition rendering thuộc Adapter-specific layer.

Flow:

Context Snapshot

↓

Adapter

↓

Prompt Builder / Condition Builder

↓

Model-specific Input

Không đặt một global raw prompt builder trong frontend.

# **44. Frontend Prompt Rule**

Frontend chỉ gửi:

- user instruction;

- creative intent;

- exposed generation options.

Frontend SHALL NOT chịu trách nhiệm dựng:

- StoryDiffusion technical prompt;

- SAMIM conditioning payload;

- hidden system instruction;

- provider-specific request.

# **45. Prompt Provenance**

Nếu lưu rendered prompt/model input, nó chỉ là provenance/debug artifact.

Không xem nó là Canon hoặc public business contract.

# **46. AI Abstractions**

Core interfaces:

IImageGenerationService

ITextGenerationService

IConsistencyService

Implementations nằm ở adapter/infrastructure boundary.

# **47. Image Generation Adapters**

Expected evolution:

MockImageGenerationAdapter

↓

StoryDiffusionAdapter

↓

ProposedMethodAdapter / SAMIM

Có thể thêm:

ExternalImageApiAdapter

mà không đổi Scene workflow.

# **48. Text Generation Adapters**

Text AI phải nằm sau:

ITextGenerationService

Business logic không được import provider SDK trực tiếp.

# **49. AI Worker Structure**

Recommended:

ai-worker/

├── app/

│ ├── jobs/

│ ├── adapters/

│ ├── pipelines/

│ ├── validation/

│ ├── models/

│ ├── context/

│ └── infrastructure/

├── tests/

├── requirements/

└── entrypoint

Exact folder names có thể điều chỉnh, nhưng responsibility separation phải giữ.

# **50. Worker Execution Rule**

Worker flow:

Receive / Claim Job

↓

Load Context Snapshot

↓

Resolve Adapter

↓

Build Model Input

↓

Run Inference

↓

Normalize Result

↓

Store Binary Asset

↓

Run Validation

↓

Persist Candidate + Provenance

↓

Update Job

# **51. Worker Idempotency**

Worker phải được thiết kế để retry/requeue không tạo duplicate business result ngoài ý muốn.

Trước khi persist final Candidate, kiểm tra Job/Attempt state phù hợp.

# **52. Retry vs Regeneration**

### **Retry**

Technical recovery của cùng một creative operation.

### **Regeneration**

Một creative operation mới do user/system yêu cầu.

Không dùng cùng command/event semantics.

# **53. RabbitMQ Convention**

RabbitMQ là queue implementation chính.

Code queue phải nằm sau infrastructure abstraction phù hợp.

Không để Domain/Application phụ thuộc RabbitMQ SDK trực tiếp.

# **54. Queue Message Rule**

Queue message SHOULD chứa identifier cần thiết, không nhét toàn bộ project state.

Ví dụ:

jobId

attemptId

operationType

Worker load durable Context Snapshot bằng identifier.

# **55. Queue Is Not Job Database**

Nếu queue message mất hoặc requeue:

Job vẫn phải tồn tại trong PostgreSQL.

API Job endpoint đọc persisted state, không đọc RabbitMQ như source of truth.

# **56. Realtime Convention**

SignalR là optional.

Frontend phải hoạt động được bằng:

REST Job API + polling/reconciliation

Realtime event chỉ cải thiện UX.

# **57. Realtime Reconciliation**

Khi nhận SignalR event:

frontend SHOULD reconcile với REST state khi cần.

Không dùng transient realtime event làm durable state.

# **58. Asset Storage Abstraction**

Binary asset access đi qua:

> IAssetStorage

Current implementations:

LocalFileAssetStorage

S3AssetStorage

# **59. Asset Storage Environment Mapping**

Development:

LocalFileAssetStorage

Thesis Demo / Production-like:

S3AssetStorage

Cloudflare R2 là provider ưu tiên hiện tại nhưng không hard-code vào domain.

# **60. Asset Metadata Rule**

Database lưu metadata.

Storage lưu binary.

Không lưu generated image blob lớn trực tiếp trong relational table trừ trường hợp đặc biệt có quyết định riêng.

# **61. Asset Ownership**

Mọi asset access phải enforce ownership/project authorization phía server.

Không tin projectId/userId do frontend gửi mà bỏ authorization.

# **62. Storage Key Rule**

Storage key được server generate.

Không dùng raw user filename làm canonical storage path.

# **63. Configuration**

Mọi environment-dependent value phải externalized.

Ví dụ:

DATABASE_URL

RABBITMQ_URL

ASSET_STORAGE_PROVIDER

ASSET_STORAGE_ENDPOINT

ASSET_STORAGE_BUCKET

AI_PROVIDER

MODEL_ID

WORKER_CONCURRENCY

MAX_RETRIES

Exact variable names có thể theo DEPLOY-01 implementation.

# **64. Secrets**

Không commit:

- database password;

- RabbitMQ password;

- S3 secret;

- external AI API key;

- auth signing secret.

Dùng environment/secret storage.

# **65. Logging**

Structured logging được ưu tiên.

Generation-related log SHOULD hỗ trợ correlation qua:

requestId

jobId

attemptId

contextSnapshotId

candidateId

workerId

khi applicable.

# **66. Log Content Safety**

Không log mặc định:

- tokens;

- credentials;

- passwords;

- private signed URL;

- raw auth header;

- entire story content.

Rendered prompt chỉ log khi debug/research mode explicitly bật.

# **67. Provenance Metadata**

Generation SHOULD lưu:

contextSchemaVersion

promptTemplateVersion

or conditionBuilderVersion

adapterVersion

modelId

methodVersion

seed

workerVersion

khi applicable.

# **68. Persistence Convention**

Mọi schema change phải đi qua version-controlled migration.

Không chỉnh database thủ công như một phần normal implementation flow.

# **69. Repository Pattern**

Repository pattern không bắt buộc cho mọi entity.

Chỉ dùng khi giúp:

- enforce aggregate boundary;

- isolate persistence;

- simplify testing;

- prevent query leakage.

Không tạo repository abstraction máy móc chỉ vì convention.

# **70. Query Strategy**

Read-heavy query có thể dùng application query service/projection phù hợp.

Không bắt buộc mọi read đi qua aggregate repository nếu gây over-engineering.

# **71. Cache**

Cache là optimization.

Cache:

≠ Canon

Nếu cache mất, hệ thống phải rebuild/reload được từ durable state.

# **72. Frontend Feature Structure**

Recommended:

features/

├── projects/

├── stories/

├── characters/

├── scenes/

├── generations/

└── assets/

Feature chứa:

- views/components;

- API service;

- local types;

- state/composable khi cần.

# **73. Frontend API Client**

Shared API client chịu trách nhiệm:

- base URL;

- auth header;

- JSend unwrap;

- standard error mapping;

- errorKey;

- request cancellation khi cần.

Không duplicate Axios/fetch error handling ở mọi component.

# **74. Frontend State Rule**

Backend là source of truth cho durable state.

Frontend local store không được coi là Canon.

Browser refresh phải reload được current Project/Job/Candidate state.

# **75. Warning vs Blocking UI**

Frontend phải phân biệt:

WARNING

BLOCKING

Warning có thể cho phép user tiếp tục theo policy.

Blocking validation không được cho phép invalid canonical action.

# **76. Candidate UI Rule**

Candidate gallery phải phản ánh:

- selected state;

- validation state;

- stale/outdated context khi applicable;

- generation provenance khi product UX cần.

Không xóa Candidate cũ khi regeneration.

# **77. Testing Layers**

OWNIVERSE sử dụng:

Unit Test

Integration Test

API Test

E2E Test

AI/Validation Test

Không phải feature nào cũng cần mọi layer, nhưng critical workflow phải có integration coverage.

# **78. Unit Tests**

Ưu tiên domain/business rule:

- state transitions;

- validation;

- revision logic;

- context priority;

- candidate selection rules.

Không viết test vô nghĩa chỉ để tăng coverage.

# **79. Integration Tests**

Kiểm tra:

- persistence;

- migrations;

- module integration;

- API/database behavior;

- queue publisher/consumer abstraction;

- storage adapter;

- Context Snapshot.

# **80. Mock AI Tests**

CI và normal backend/frontend development SHOULD dùng Mock AI.

Mock phải hỗ trợ:

- success;

- delay;

- failure;

- warning;

- deterministic fake output.

# **81. Generated Image Testing**

Không assert:

expected.png == generated.png

cho stochastic model.

Test:

- output exists;

- metadata correct;

- candidate persisted;

- validation invoked;

- provenance exists.

Quality evaluation thuộc QA-02/RESEARCH-04.

# **82. Real Model Tests**

Real AI tests chỉ chạy ở:

- selected integration runs;

- research pipeline;

- pre-demo validation;

- manual/controlled CI environment khi available.

Không bắt GPU test cho mọi commit.

# **83. CI Baseline**

Minimum CI:

Frontend build

Frontend tests

Backend build

Backend tests

Python worker tests

Static/lint checks where configured

GPU model execution không phải minimum CI gate.

# **84. Git Main Rule**

main SHOULD luôn:

- build được;

- migration coherent;

- critical tests pass.

Không merge known broken architecture vào main chỉ để “lưu tạm”.

# **85. Branch Convention**

Suggested:

feature/...

fix/...

refactor/...

research/...

Sprint branch có thể dùng khi phù hợp.

# **86. Commit Convention**

Recommended Conventional Commit style:

feat:

fix:

refactor:

test:

docs:

chore:

research:

Commit nên nhỏ theo logical change.

# **87. Pull Request / Review Rule**

Trước merge:

\[ \] Scope đúng task

\[ \] No unrelated refactor

\[ \] Architecture boundary giữ nguyên

\[ \] Migration included if schema changed

\[ \] Tests added/updated

\[ \] API contract updated if needed

\[ \] Docs updated if behavior changed

# **88. AI Coding Assistant Rules**

AI coding agent phải:

1.  đọc relevant code hiện tại trước;

2.  đọc relevant spec;

3.  tôn trọng module boundary;

4.  không tạo duplicate abstraction;

5.  không tự đổi DB/API/business rule;

6.  không mở rộng scope;

7.  không refactor unrelated code;

8.  chạy/đề xuất test liên quan;

9.  báo rõ assumption khi source không quy định.

# **89. AI Coding Context Package**

Không đưa toàn bộ documentation cho mọi task.

Ví dụ Project task:

DEV-01

SPRINT current section

IMPLEMENT-02 Project section

DOMAIN Project section

API Project section

Relevant existing code

# **90. AI Coding Output Rule**

AI không được tạo:

- table mới ngoài spec;

- endpoint mới ngoài task;

- provider dependency trực tiếp trong Domain;

- business rule “hợp lý theo suy đoán” nhưng chưa được chốt.

Nếu thiếu contract:

> dừng ở TODO/explicit assumption hoặc báo gap.

# **91. AI-Generated Code Review**

Developer phải review:

- business correctness;

- architecture;

- security;

- concurrency;

- migration;

- error handling;

- tests.

“AI đã generate thành công” không phải Definition of Done.

# **92. Research Code Isolation**

Research implementation phải nằm sau Adapter hoặc research module boundary.

Không chèn SAMIM logic trực tiếp vào:

- Scene entity;

- Character entity;

- Controller;

- generic Application use case.

# **93. Baseline Before Proposed Method**

Implementation order:

Mock

↓

StoryDiffusion

↓

SAMIM

Research adapter chỉ tích hợp sau khi baseline reproducible và gap đủ rõ.

# **94. Identity Memory Boundary**

Research Identity Memory:

> visual identity conditioning.

Application Story Memory:

> narrative facts, events, relationships, continuity.

Không dùng cùng type/table/service name gây nhầm lẫn.

# **95. Security Rule**

Authorization được enforce server-side.

Frontend visibility không phải security boundary.

# **96. Ownership Rule**

Mọi Project child resource phải verify ownership thông qua trusted server-side relation.

Không chỉ check user-provided projectId.

# **97. Upload Security**

Validate:

- file type;

- size;

- ownership;

- storage key;

- unsafe filename/path input.

# **98. External Provider Security**

Provider keys chỉ tồn tại ở server/worker.

Frontend không được nhận:

- AI provider key;

- S3 secret key;

- RabbitMQ credential;

- DB credential.

# **99. Async Completion Rule**

Job chỉ được COMPLETED khi required durable artifacts đã persist.

Ví dụ image generation:

Asset metadata persisted

\+

Binary asset stored

\+

Candidate persisted

\+

Required validation persisted

\+

Job updated

# **100. Notification After Commit**

Realtime/event notification về completed result chỉ phát sau durable commit.

Không notify frontend rằng Candidate hoàn thành trước khi database state an toàn.

# **101. Job State Machine**

State transition phải explicit.

Không set status tự do bằng arbitrary string.

Possible lifecycle theo API/ARCH contract.

# **102. Cancellation**

Cancellation là explicit request/state transition.

Worker phải check cancellation ở safe boundaries khi feasible.

Cancelled Job không được silently chuyển completed nếu policy không cho phép.

# **103. Stale Context Rule**

Khi Canon thay đổi:

- old Snapshot giữ nguyên;

- old Candidate giữ nguyên;

- dependency tracking xác định stale/outdated;

- generation sau dùng context mới.

Không auto-delete history.

# **104. No Whole-Story Auto-Regeneration**

Canonical edit không tự động regenerate toàn bộ story.

System chỉ đánh dấu affected outputs/context khi cần.

User/application workflow quyết định regeneration.

# **105. Migration Review**

Migration phải review:

- destructive operation;

- index;

- foreign key;

- nullability;

- default;

- large data impact;

- rollback/recovery implication.

# **106. Performance Rule**

Không optimize sớm bằng:

- cache phức tạp;

- microservices;

- distributed state;

- event sourcing toàn hệ thống.

Đo bottleneck trước.

# **107. Microservice Rule**

MVP backend là:

> modular monolith.

Python AI Worker là separate process/service vì workload khác biệt.

Không chia từng business module thành microservice.

# **108. Containerization Rule**

Containerization là deployment concern.

Domain/Application code không được phụ thuộc “đang chạy Docker hay không”.

# **109. Environment Parity**

Dev/demo có thể khác infrastructure implementation, nhưng contract giữ giống nhau.

Ví dụ:

LocalFileAssetStorage

↔

S3AssetStorage

Application code không đổi.

# **110. Health/Readiness Rule**

Service có health/readiness endpoint hoặc mechanism phù hợp.

Ready nghĩa là đủ điều kiện nhận workload, không chỉ process đang chạy.

# **111. Failure Handling**

Failure phải normalize thành:

- known application error;

- Job failure;

- validation result;

- retriable infrastructure failure;

- non-retriable business failure.

Không swallow exception.

# **112. Error Logging vs User Message**

Internal log có technical detail.

Frontend response có:

- stable errorKey;

- safe message;

- actionable context khi phù hợp.

Không expose stack trace.

# **113. Documentation Sync**

Nếu code thay đổi contract đã chốt:

- update relevant spec;

- update API doc;

- update migration notes;

- update sprint task.

Không để docs và code drift lâu dài.

# **114. Technical Debt Convention**

Ghi technical debt rõ:

TD-ID

Description

Reason

Risk

Target Sprint

Không để anonymous TODO tồn tại vô thời hạn.

# **115. Definition of Ready for Coding Task**

Task ready khi:

\[ \] Goal rõ

\[ \] Module owner rõ

\[ \] Relevant spec có

\[ \] Input/output contract đủ

\[ \] Dependency đã có

\[ \] Acceptance criteria rõ

# **116. Definition of Done for Coding Task**

Task done khi:

\[ \] Code implemented

\[ \] Architecture respected

\[ \] Build pass

\[ \] Relevant tests pass

\[ \] Migration included if needed

\[ \] Error handling included

\[ \] Documentation updated if contract changed

\[ \] No unrelated changes

# **117. Vertical Slice Principle**

Ưu tiên:

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

Test

thay vì build toàn bộ database trước rồi toàn bộ API sau.

# **118. Critical Implementation Sequence**

Recommended dependency order:

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

Context Snapshot

↓

Generation

↓

Mock AI

↓

Candidate

↓

Baseline AI

↓

Validation

↓

SAMIM

# **119. Canon Before AI**

Không bắt đầu full AI integration khi:

- Character/Scene Canon còn không ổn định;

- Snapshot chưa có;

- Candidate model chưa có.

# **120. Snapshot Before Generation**

Mọi real generation phải dựa trên immutable context snapshot.

Không để worker đọc live canonical tables ngẫu nhiên trong quá trình inference.

# **121. Mock Before Real AI**

Mock Adapter phải hoàn thành end-to-end workflow trước StoryDiffusion integration.

# **122. Baseline Before SAMIM**

StoryDiffusion integration là bước xác nhận Application/Research adapter boundary trước Proposed Method.

# **123. Candidate Before Canon**

AI output luôn đi qua Candidate/review logic trước khi có tác động canonical phù hợp.

# **124. Testing Throughout**

Tests được viết cùng Sprint.

Không dồn testing sang cuối.

# **125. Final Engineering Invariants**

OWNIVERSE implementation phải giữ:

1.  Canon explicit.

2.  Domain độc lập provider.

3.  AI replaceable.

4.  Context structured và versioned.

5.  Snapshot immutable.

6.  Prompt adapter-specific.

7.  Worker không sở hữu Canon.

8.  Job durable.

9.  Queue replaceable/runtime-only.

10. Candidate traceable.

11. Asset binary tách relational data.

12. Retry khác Regeneration.

13. Revision khác Version.

14. Frontend không phải source of truth.

15. Realtime optional.

16. Critical workflow có test.

17. Research method nằm sau Adapter.

18. Provenance đủ cho reproducibility.

# **126. Final Principle**

DEV-01 được tóm tắt bằng:

> **Business rules ở đúng layer.**
>
> **Canon chỉ thay đổi qua Application.**
>
> **AI nhận structured context, không nhận business state ngẫu nhiên.**
>
> **Prompt là implementation detail của Adapter.**
>
> **Worker có thể thay, model có thể thay, provider có thể thay nhưng Product workflow phải ổn định.**
>
> **Mỗi thay đổi quan trọng phải build được, test được và truy vết được.**
