# **DEPLOY-03 — Deployment, Operations, Monitoring & Backup**

**Document ID:** DEPLOY-03  
**Document Type:** Deployment, Operations, Monitoring & Backup Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Status:** Draft v1 — Updated  
**Primary Roles:** Solution Architect / DevOps Engineer / Backend Engineer / AI Engineer  
**Parent Documents:** ARCH-01, ARCH-02, DEPLOY-01, DEPLOY-02  
**Related Documents:** DATA-01, DATA-02, API-01, API-02, AI-01, AI-02, AI-03, QA-01, QA-04, IMPLEMENT-01, DEV-01, SPRINT-01

# **1. Purpose**

Tài liệu này định nghĩa cách OWNIVERSE được triển khai, vận hành, giám sát, sao lưu và phục hồi khi hệ thống chạy ngoài môi trường development cục bộ.

Mục tiêu chính là bảo đảm hệ thống:

- có thể triển khai lặp lại;

- có thể xác định chính xác version đang chạy;

- có khả năng quan sát runtime;

- có thể phục hồi sau lỗi dịch vụ hoặc worker;

- không làm mất Canonical Data;

- không làm mất Generation History;

- không phụ thuộc vào một AI provider hoặc GPU environment duy nhất;

- có quy trình rollback và backup rõ ràng;

- đủ ổn định để sử dụng trong Thesis Demo Environment.

DEPLOY-03 không biến MVP thành một hệ thống cloud-scale phức tạp.

# **2. Scope**

DEPLOY-03 bao gồm:

- deployment topology;

- environment separation;

- release versioning;

- deployment procedure;

- migration procedure;

- rollback;

- health/readiness;

- runtime monitoring;

- logging;

- Job/Worker observability;

- AI execution observability;

- asset storage operations;

- database backup;

- object storage backup/protection;

- recovery procedures;

- security configuration;

- thesis demo preparation;

- incident runbook;

- release readiness.

# **3. Out of Scope**

Tài liệu này không yêu cầu:

- Kubernetes;

- service mesh;

- multi-region active-active architecture;

- enterprise SRE platform;

- autoscaling cluster phức tạp;

- multi-cloud failover;

- zero-downtime database migration ở mức enterprise;

- global CDN architecture;

- full production SOC/SIEM.

Các nội dung đó chỉ xem xét nếu sau MVP có nhu cầu thực tế.

# **4. Locked Implementation Stack**

Deployment hiện tại sử dụng stack đã khóa:

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
> AI Runtime
>
> Python AI Worker
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
> SignalR — optional UX enhancement
>
> AI Implementations
>
> IImageGenerationService
>
> ├── Mock Adapter
>
> ├── StoryDiffusionAdapter
>
> ├── ProposedMethodAdapter / SAMIM
>
> └── External API Adapter

Provider cụ thể không được phép trở thành business dependency.

# **5. Deployment Environments**

OWNIVERSE định nghĩa tối thiểu ba environment logic.

## **5.1 Development**

Mục đích:

- coding;

- debugging;

- automated tests;

- local integration.

Có thể sử dụng:

- local PostgreSQL/container;

- local RabbitMQ/container;

- LocalFileAssetStorage;

- Mock AI;

- local hoặc remote AI worker.

## **5.2 Thesis Demo**

Đây là environment chính dùng để trình bày hệ thống.

Yêu cầu:

- configuration được freeze;

- database riêng;

- asset storage riêng;

- model/runtime đã warm up;

- demo dataset đã chuẩn bị;

- logs truy cập được;

- backup được tạo trước buổi demo;

- rollback path đã kiểm tra.

Thesis Demo Environment không được phụ thuộc vào developer machine duy nhất.

## **5.3 Production-Like**

Production-like không đồng nghĩa với commercial production.

Mục đích:

- kiểm chứng kiến trúc deploy thực tế;

- chạy UAT;

- chạy demo dài hơn;

- kiểm chứng backup/recovery;

- đánh giá integration giữa API, RabbitMQ, Worker và storage.

# **6. Target Deployment Topology**

Topology logic:

> Internet / Client
>
> \|
>
> v
>
> Reverse Proxy / HTTPS
>
> \|
>
> +---------+---------+
>
> \| \|
>
> v v
>
> Frontend API
>
> \|
>
> +---------------------+----------------------+
>
> \| \| \|
>
> v v v
>
> PostgreSQL RabbitMQ S3 Storage
>
> \|
>
> v
>
> Python Worker
>
> \|
>
> v
>
> AI Adapter / Model Runtime
>
> \|
>
> +----------------------+----------------------+
>
> \| \| \|
>
> v v v
>
> StoryDiffusion SAMIM External API

SignalR nếu được bật đi qua API/reverse proxy.

# **7. Deployment Independence**

Application architecture không phụ thuộc bắt buộc vào:

- AWS;

- Azure;

- GCP;

- một GPU provider cụ thể;

- một image-generation API cụ thể.

Các dependency bên ngoài phải nằm sau abstraction.

# **8. Reverse Proxy and HTTPS**

Demo/production-like deployment SHOULD sử dụng reverse proxy.

Responsibilities:

- HTTPS termination;

- frontend routing;

- API routing;

- request size limit;

- optional rate limit;

- forwarding headers;

- SignalR/WebSocket forwarding nếu bật realtime.

API không được expose database, RabbitMQ hoặc worker management endpoints trực tiếp ra Internet.

# **9. Frontend Deployment**

Frontend được build thành static production bundle từ:

> Vue 3
>
> \+
>
> Vite
>
> \+
>
> TypeScript

Deployment artifact phải xác định được:

- build version;

- commit SHA nếu khả dụng;

- environment;

- API base URL.

Không hard-code secrets vào frontend bundle.

# **10. Backend Deployment**

ASP.NET Core API chịu trách nhiệm:

- authentication/authorization;

- Canonical Domain operations;

- REST API;

- Context Snapshot creation;

- Job creation;

- Candidate selection;

- version/revision handling;

- generation orchestration;

- storage metadata;

- ownership enforcement.

Backend không chạy heavy AI inference trong HTTP request.

# **11. Database Deployment**

Database chính:

> PostgreSQL

Deployment phải bảo đảm:

- connection string từ environment/secret;

- migration version có thể xác định;

- migration chạy trước khi application version phụ thuộc schema mới nhận traffic;

- backup trước migration rủi ro;

- database không public trực tiếp.

# **12. RabbitMQ Deployment**

RabbitMQ là queue implementation chính cho AI execution.

Responsibilities:

- transport Job execution messages;

- worker consumption;

- acknowledgement;

- requeue/recovery khi phù hợp;

- worker decoupling.

RabbitMQ không phải source of truth của Job.

Source of truth vẫn là:

> PostgreSQL Job state exposed through API.

Nếu message queue mất tạm thời, Job record không được mất.

# **13. Python AI Worker Deployment**

Python Worker có thể chạy:

- cùng server với application khi workload nhỏ;

- trên GPU server riêng;

- trên remote GPU environment;

- trên machine khác miễn kết nối được API/DB/storage/queue theo contract cho phép.

Worker responsibilities:

> Receive/claim Job
>
> ↓
>
> Load immutable Context Snapshot
>
> ↓
>
> Resolve Adapter
>
> ↓
>
> Build adapter-specific prompt/conditioning
>
> ↓
>
> Run AI inference
>
> ↓
>
> Normalize output
>
> ↓
>
> Store Asset
>
> ↓
>
> Run Validation
>
> ↓
>
> Persist Candidate + provenance
>
> ↓
>
> Update Job

Worker không được tự sửa Canon.

# **14. AI Runtime Modes**

Supported deployment modes:

## **MOCK_AI**

Không cần GPU.

Dùng để:

- development;

- CI;

- workflow testing;

- failure simulation.

## **FULL_LOCAL**

AI Worker và model chạy trong local/runtime environment có GPU phù hợp.

## **REMOTE_AI**

Application và Worker/model có thể nằm ở các machine khác nhau.

## **EXTERNAL_API**

Adapter gọi AI provider bên ngoài.

Product workflow vẫn phải giữ nguyên.

# **15. Colab Pro Boundary**

Google Colab Pro có thể được sử dụng cho:

- research experiments;

- baseline reproduction;

- temporary GPU worker;

- thesis demo fallback trong trường hợp kiểm soát được session.

Không xem Colab Pro là long-term production infrastructure.

Rủi ro:

- session expiration;

- network instability;

- runtime reset;

- storage volatility;

- environment drift.

Mọi kết quả quan trọng phải được persist ra hệ thống OWNIVERSE thay vì chỉ nằm trong Colab runtime.

# **16. Asset Storage Strategy**

Application sử dụng:

> IAssetStorage

Implementations:

> LocalFileAssetStorage
>
> S3AssetStorage

Development ưu tiên LocalFileAssetStorage.

Thesis Demo / Production-like ưu tiên S3-compatible storage.

Provider hiện tại được ưu tiên:

> Cloudflare R2

Tuy nhiên domain/application không được chứa logic riêng của R2.

# **17. Asset Storage Responsibilities**

Asset Storage lưu binary object như:

- character reference images;

- generated images;

- thumbnails;

- optional exported files.

PostgreSQL lưu metadata như:

- assetId;

- ownership;

- storage key;

- media type;

- size;

- checksum nếu dùng;

- provenance relation;

- createdAt;

- lifecycle status.

Database không phải binary image store chính.

# **18. GenerationContextV1 in Deployment**

Mỗi generation phải dựa trên structured context:

> GenerationContextV1

Application tạo:

> Canonical Domain Data
>
> ↓
>
> GenerationContextV1
>
> ↓
>
> Immutable Context Snapshot

Worker chỉ xử lý Snapshot tương ứng Job.

Raw prompt không phải application contract chính.

# **19. Adapter-Specific Model Input**

Sau khi Worker load Snapshot:

> Context Snapshot
>
> ↓
>
> Adapter
>
> ↓
>
> Prompt Builder / Condition Builder
>
> ↓
>
> Model-specific Input

Ví dụ:

> StoryDiffusionAdapter
>
> SAMIMAdapter
>
> ExternalImageApiAdapter

Các Adapter có thể render input khác nhau mà không thay đổi Product workflow.

# **20. AI Provenance**

Generation result SHOULD lưu hoặc truy vết được tối thiểu:

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
> promptTemplateVersion
>
> or conditionBuilderVersion
>
> seed
>
> workerVersion
>
> generationTimestamp

Nếu model không sử dụng một field nào đó, field có thể null/not applicable.

# **21. Rendered Prompt / Model Payload**

Rendered prompt hoặc normalized model input có thể được lưu phục vụ:

- debugging;

- reproducibility;

- research;

- failure analysis.

Nhưng nó:

- không phải Canon;

- không phải public business contract;

- không nên mặc định expose toàn bộ cho frontend;

- có thể chứa internal technical instructions.

Sensitive provider credential tuyệt đối không được persist trong prompt provenance.

# **22. Release Unit**

Một release OWNIVERSE phải nhận diện được tối thiểu:

> Frontend Version
>
> API Version
>
> Worker Version
>
> Database Migration Version
>
> AI Adapter Version
>
> Model / Method Version
>
> Validation Profile Version

Đối với thesis demo nên thêm:

> Demo Dataset Version
>
> Context Schema Version
>
> Prompt/Condition Builder Version

# **23. Recommended Release Identifier**

Ví dụ:

> OWNIVERSE-RC-2026-01

hoặc thesis freeze:

> THESIS_DEMO_RC_1

Tên release phải map được tới source revision.

# **24. Deployment Sequence**

Recommended sequence:

> 1\. Freeze release candidate
>
> 2\. Run automated tests
>
> 3\. Create DB backup if needed
>
> 4\. Verify asset storage access
>
> 5\. Deploy infrastructure/config
>
> 6\. Apply database migration
>
> 7\. Deploy API
>
> 8\. Verify API health/readiness
>
> 9\. Deploy/enable Worker
>
> 10\. Verify RabbitMQ consumption
>
> 11\. Deploy Frontend
>
> 12\. Run smoke test
>
> 13\. Run generation test
>
> 14\. Verify logs/metrics
>
> 15\. Mark release active

# **25. Database Migration Rule**

Migration phải:

- version controlled;

- reviewed;

- reproducible;

- tested trên clean database;

- tested trên current schema khi có upgrade path.

Không chỉnh production/demo database thủ công mà không migration record trừ incident recovery có ghi nhận rõ.

# **26. Migration Safety**

Trước migration có nguy cơ cao:

> Backup Database
>
> ↓
>
> Apply Migration
>
> ↓
>
> Run Health Check
>
> ↓
>
> Run Smoke Test

Nếu migration fail:

- không tiếp tục deploy application phụ thuộc schema mới;

- rollback hoặc restore theo migration strategy;

- ghi lại incident.

# **27. Application Rollback**

Rollback có thể gồm:

> Frontend rollback
>
> API rollback
>
> Worker rollback
>
> Config rollback
>
> Adapter rollback

Database rollback cần thận trọng hơn.

Không tự động downgrade schema nếu migration destructive và chưa có rollback path.

# **28. Canonical Data Protection Rule**

Rollback tuyệt đối không được làm mất:

- Project;

- Story;

- Character;

- Scene;

- Version;

- Context Snapshot;

- Job history;

- Candidate history;

- selected canonical references.

Nếu phải lựa chọn giữa rollback code và giữ dữ liệu:

> dữ liệu canonical và traceability có ưu tiên cao hơn.

# **29. Worker Rollback**

Worker có thể rollback độc lập nếu Adapter contract tương thích.

Ví dụ:

> Worker v1.4
>
> ↓ issue
>
> Worker v1.3

Job đang chạy cần xử lý theo state/recovery contract.

Không được tạo Candidate duplicate chỉ vì worker restart.

# **30. Health Checks**

Mỗi deployable service SHOULD có health status phù hợp.

API:

- process alive;

- DB connectivity;

- optional RabbitMQ connectivity;

- optional storage connectivity.

Worker:

- process alive;

- queue connectivity;

- ability to load configured adapter/model;

- GPU readiness khi applicable.

# **31. Health vs Readiness**

Phân biệt:

### **Health**

Service process còn sống hay không.

### **Readiness**

Service đã sẵn sàng nhận workload hay chưa.

Ví dụ model đang loading:

> Health = OK
>
> Readiness = NOT_READY

# **32. Monitoring Objectives**

Monitoring cần trả lời được:

- API có hoạt động không?

- Job có đang bị backlog không?

- Worker có xử lý Job không?

- Generation fail ở bước nào?

- Storage có lỗi không?

- Database có reachable không?

- Candidate có được persist không?

- model/adapter nào đang gây lỗi?

# **33. API Metrics**

Recommended metrics:

> request count
>
> request latency
>
> 5xx count
>
> 4xx count
>
> active requests
>
> generation request count
>
> database error count

Không cần enterprise observability platform để đạt MVP.

# **34. Queue Metrics**

Theo dõi tối thiểu:

> queued messages
>
> unacked messages
>
> consumer count
>
> queue age if measurable
>
> requeue count

Queue tăng liên tục có thể chỉ ra:

- worker down;

- GPU quá chậm;

- worker concurrency thấp;

- poison message;

- downstream AI failure.

# **35. Worker Metrics**

Recommended:

> jobs claimed
>
> jobs completed
>
> jobs failed
>
> average execution time
>
> model load time
>
> validation time
>
> GPU OOM count
>
> provider error count
>
> retry count

# **36. AI Runtime Metrics**

Có thể theo dõi:

> modelId
>
> adapterVersion
>
> methodVersion
>
> generation duration
>
> seed
>
> validation outcome
>
> candidate outcome

Không dùng monitoring runtime thay thế RESEARCH-04 scientific evaluation.

# **37. Storage Metrics**

Theo dõi:

- upload failure;

- download failure;

- object size;

- storage latency khi cần;

- missing object;

- permission error.

Asset metadata tồn tại nhưng binary object mất là incident nghiêm trọng.

# **38. Logging Standard**

Logs SHOULD sử dụng structured logging.

Recommended fields:

> timestamp
>
> level
>
> service
>
> requestId
>
> userId where safe/needed
>
> projectId
>
> jobId
>
> attemptId
>
> candidateId
>
> workerId
>
> operationType
>
> errorKey

Không cần mọi field trên mọi log.

# **39. Logging Privacy**

Không log mặc định:

- password;

- access token;

- API secret;

- raw authentication header;

- full private story content nếu không cần;

- provider credential;

- signed storage URL dài hạn.

Rendered prompt có thể chứa story content nên chỉ log khi explicitly enabled cho debugging/research.

# **40. Job Traceability**

Một Job phải có thể trace:

> API Request
>
> ↓
>
> Job
>
> ↓
>
> Context Snapshot
>
> ↓
>
> Queue Message
>
> ↓
>
> Worker
>
> ↓
>
> Attempt
>
> ↓
>
> Adapter
>
> ↓
>
> Model
>
> ↓
>
> Asset
>
> ↓
>
> Validation Result
>
> ↓
>
> Candidate

Đây là observability chain quan trọng nhất của AI runtime.

# **41. Retry Monitoring**

Retry phải được quan sát riêng với regeneration.

### **Retry**

Technical recovery.

### **Regeneration**

New creative operation.

Metrics và logs không được trộn hai khái niệm.

# **42. Failure Categories**

Operational failure có thể phân nhóm:

> API_FAILURE
>
> DATABASE_FAILURE
>
> QUEUE_FAILURE
>
> WORKER_FAILURE
>
> MODEL_FAILURE
>
> PROVIDER_FAILURE
>
> STORAGE_FAILURE
>
> VALIDATION_FAILURE
>
> TIMEOUT
>
> CONFIGURATION_FAILURE

Business validation failure không luôn là infrastructure incident.

# **43. API Failure Recovery**

Nếu API crash:

- canonical DB giữ state;

- RabbitMQ giữ message theo durability/config;

- running Job state được reconcile;

- frontend có thể reconnect/reload Job state.

API restart không được làm mất Generation Job.

# **44. Worker Failure Recovery**

Nếu Worker crash:

> Job remains durable
>
> ↓
>
> Message may be requeued
>
> ↓
>
> Recovery logic checks Job/Attempt state
>
> ↓
>
> New worker continues safely

System phải tránh duplicate Candidate do retry/requeue.

# **45. Queue Failure Recovery**

Nếu RabbitMQ unavailable:

- API không giả vờ Job đang chạy;

- Job có thể được tạo ở trạng thái phù hợp và publish retry theo implementation contract;

- error phải observable;

- không mất canonical request intent.

Recovery cần reconcile persisted Job với queue state.

# **46. Database Failure**

Database failure là blocking đối với canonical operations.

System không được:

- tiếp tục AI generation mà không thể persist Job/result;

- xem Candidate là completed nếu persistence chưa thành công;

- lưu Canon chỉ trong worker memory.

# **47. Asset Storage Failure**

Nếu generated image đã tạo nhưng upload storage fail:

> Generation result not complete

Worker phải:

- report failure;

- giữ metadata/diagnostics nếu có thể;

- retry storage theo policy;

- không tạo completed Candidate trỏ tới missing object.

# **48. External AI Provider Failure**

External API Adapter phải normalize:

- rate limit;

- timeout;

- unavailable;

- invalid response;

- authentication failure;

- provider rejection.

Provider-specific error không được leak thành business contract không ổn định.

# **49. GPU OOM Recovery**

GPU Out-Of-Memory có thể xử lý bằng:

- fail attempt;

- retry với safe policy nếu applicable;

- lower concurrency;

- unload/reload model;

- route sang worker khác nếu architecture hỗ trợ.

Không tự ý đổi scientific experiment configuration trong research runs.

# **50. Backup Strategy Overview**

Backup tập trung vào hai lớp chính:

> PostgreSQL
>
> \+
>
> Asset Storage

RabbitMQ không thay thế backup.

Worker filesystem không được xem là durable data source.

# **51. PostgreSQL Backup**

Backup database phải bảo vệ:

- Canon;

- Version history;

- Snapshot;

- Job;

- Attempt;

- Candidate;

- Asset metadata;

- selection state;

- provenance.

Recommended moments:

- trước schema migration quan trọng;

- trước thesis demo;

- định kỳ trong giai đoạn final thesis;

- trước risky data migration.

# **52. Asset Storage Backup / Protection**

Đối với local development:

- backup chỉ cần khi chứa dữ liệu quan trọng.

Đối với thesis demo:

- bảo vệ Character References;

- generated Candidates dùng cho demo;

- prepared fallback assets;

- research outputs quan trọng.

Với S3-compatible provider có thể sử dụng provider durability/versioning nếu available, nhưng không giả định tự động nếu chưa cấu hình.

# **53. Backup Consistency**

Database backup và storage protection nên cùng một release/demo checkpoint khi cần.

Ví dụ:

> THESIS_DEMO_RC_1
>
> ├── DB Backup
>
> ├── Important Asset Snapshot/List
>
> ├── Release Versions
>
> └── Demo Dataset Version

# **54. Restore Procedure**

Restore test phải được thực hiện ít nhất một lần trước final thesis demo.

Basic procedure:

> 1\. Prepare clean recovery environment
>
> 2\. Restore PostgreSQL
>
> 3\. Restore/connect asset storage
>
> 4\. Apply compatible application release
>
> 5\. Verify migrations
>
> 6\. Verify Projects/Characters/Scenes
>
> 7\. Verify selected Candidates
>
> 8\. Verify historical Candidates
>
> 9\. Run smoke generation

# **55. Restore Acceptance Criteria**

Restore thành công khi:

- canonical project mở được;

- character references load được;

- scene sequence đúng;

- selected outputs đúng;

- generation history đọc được;

- application có thể tạo Job mới.

# **56. Recovery Point Priorities**

Ưu tiên dữ liệu:

### **Tier 1**

- Project/Story Canon;

- Characters;

- Scenes;

- Versions;

- selected state.

### **Tier 2**

- Context Snapshots;

- Jobs;

- Attempts;

- Candidates;

- provenance.

### **Tier 3**

- derived cache;

- temporary worker files;

- rebuildable thumbnails.

# **57. Cache Recovery**

Cache nếu được sử dụng:

> không phải Canon.

Cache có thể bị xóa/rebuild.

Không có workflow nào được phụ thuộc cache như nguồn dữ liệu duy nhất.

# **58. Security — Network**

Production-like deployment SHOULD:

- expose chỉ frontend/API cần thiết;

- giữ PostgreSQL private;

- giữ RabbitMQ private;

- giữ worker management private;

- dùng HTTPS;

- firewall/allowlist khi practical.

# **59. Security — Secrets**

Secrets bao gồm:

- DB credentials;

- RabbitMQ credentials;

- S3 credentials;

- external AI API keys;

- auth signing keys.

Secrets phải đến từ:

- environment;

- secret store;

- secured deployment configuration.

Không commit vào repository.

# **60. Security — Asset Access**

Asset access phải enforce:

- project ownership;

- authorization;

- signed/controlled access khi cần.

Storage public bucket không được là default cho private project data.

# **61. Security — AI Provider**

Khi dùng external API:

- credential chỉ ở backend/worker;

- frontend không nhận secret;

- provider request được tạo server-side;

- user không điều khiển provider endpoint tùy ý.

# **62. Security — Upload**

Character Reference upload cần kiểm tra:

- file type;

- size;

- filename/path handling;

- ownership;

- storage key generation.

Không tin client-provided storage path.

# **63. Thesis Demo Strategy**

Thesis demo nên ưu tiên:

> reliability over architectural spectacle.

Không thay đổi architecture lớn ngay trước demo.

# **64. Thesis Demo Dataset**

Chuẩn bị trước:

- một Project ổn định;

- Story Core;

- ít nhất hai recurring Characters;

- canonical references;

- nhiều Scenes;

- multi-character scene;

- long-range reappearance;

- appearance change;

- baseline Candidate;

- SAMIM Candidate khi research integration hoàn tất.

# **65. Demo Generation Modes**

Nên có:

### **Live Generation**

Chứng minh pipeline thật.

### **Prepared Real Candidate**

Fallback khi external GPU/provider gặp sự cố.

Prepared Candidate phải là kết quả thật đã tạo trước đó và được ghi rõ là prepared output.

Không giả vờ đó là live generation.

# **66. Model Warm-Up**

Trước demo:

> \[ \] Worker online
>
> \[ \] Queue healthy
>
> \[ \] Model loaded/warmed
>
> \[ \] Storage reachable
>
> \[ \] DB reachable
>
> \[ \] API ready
>
> \[ \] One test generation completed

# **67. Demo Freeze**

Trước buổi demo freeze:

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
> Validation Profile
>
> Demo Dataset

Recommended label:

> THESIS_DEMO_RC_1

# **68. Demo Backup**

Ngay trước demo:

> \[ \] Database backup created
>
> \[ \] Important assets verified
>
> \[ \] Demo Project opens
>
> \[ \] Prepared Candidates accessible
>
> \[ \] Restore path known

# **69. Demo Change Policy**

Sau freeze chỉ sửa:

- P0 blocker;

- crash;

- data corruption;

- broken critical workflow;

- serious demo presentation defect.

Không thêm:

- new feature;

- architecture experiment;

- model rewrite;

- untested dependency upgrade.

# **70. Release Smoke Test**

Sau deployment chạy:

> Create/Open Project
>
> ↓
>
> Load Story
>
> ↓
>
> Open Character
>
> ↓
>
> Open Scene
>
> ↓
>
> Generate
>
> ↓
>
> Job queued
>
> ↓
>
> Worker executes
>
> ↓
>
> Candidate appears
>
> ↓
>
> Select Candidate
>
> ↓
>
> Reload

# **71. Operational Smoke Test**

Kiểm tra thêm:

> \[ \] API health
>
> \[ \] API readiness
>
> \[ \] RabbitMQ consumer
>
> \[ \] Worker health
>
> \[ \] Storage upload/download
>
> \[ \] DB migration version
>
> \[ \] Job status reload
>
> \[ \] Error logging

# **72. Research Runtime Protection**

Research experiment không được âm thầm dùng configuration khác production/demo adapter nếu đang so sánh.

Experiment run phải freeze:

- baseline/model version;

- method version;

- seed;

- generation config;

- Context Snapshot;

- validation/evaluation config.

# **73. Product QA vs Research Evaluation**

Operational monitoring trả lời:

> hệ thống có chạy đúng không?

QA-02 trả lời:

> output có đạt runtime quality/consistency requirement không?

RESEARCH-04 trả lời:

> proposed method có cải thiện theo scientific evaluation không?

Ba lớp không thay thế nhau.

# **74. Incident Severity**

Có thể sử dụng:

### **SEV-0**

Data corruption / canonical data loss.

### **SEV-1**

Critical demo/core workflow unavailable.

### **SEV-2**

Major component degraded, workaround exists.

### **SEV-3**

Minor operational issue.

# **75. Incident Record**

Incident quan trọng nên ghi:

> Incident ID
>
> Time
>
> Environment
>
> Release
>
> Affected Component
>
> Symptoms
>
> Root Cause
>
> Recovery Action
>
> Data Impact
>
> Follow-Up

# **76. Runbook — API Down**

> 1\. Check process/container
>
> 2\. Check configuration
>
> 3\. Check PostgreSQL
>
> 4\. Check logs
>
> 5\. Restart API if safe
>
> 6\. Verify readiness
>
> 7\. Run smoke request
>
> 8\. Reconcile Jobs

# **77. Runbook — Worker Down**

> 1\. Check worker process
>
> 2\. Check RabbitMQ
>
> 3\. Check GPU/model readiness
>
> 4\. Check Job currently claimed
>
> 5\. Restart worker
>
> 6\. Verify requeue/recovery
>
> 7\. Confirm no duplicate Candidate

# **78. Runbook — Queue Backlog**

> 1\. Check consumer count
>
> 2\. Check worker health
>
> 3\. Check average generation duration
>
> 4\. Check poison/failing jobs
>
> 5\. Adjust concurrency only if safe
>
> 6\. Requeue/retry according to policy

Không tăng concurrency mù quáng khi GPU memory không đủ.

# **79. Runbook — Storage Failure**

> 1\. Check credentials
>
> 2\. Check endpoint
>
> 3\. Check bucket/container
>
> 4\. Test upload
>
> 5\. Test read
>
> 6\. Review failed Job
>
> 7\. Retry persistence if safe

# **80. Runbook — Database Failure**

> 1\. Stop risky writes if needed
>
> 2\. Check PostgreSQL
>
> 3\. Check disk/storage
>
> 4\. Check connection
>
> 5\. Restore service
>
> 6\. Verify schema
>
> 7\. Verify canonical records
>
> 8\. Reconcile Jobs

# **81. Runbook — Bad Release**

> 1\. Stop rollout
>
> 2\. Capture logs
>
> 3\. Identify affected version
>
> 4\. Protect database
>
> 5\. Roll back application/worker
>
> 6\. Verify schema compatibility
>
> 7\. Run smoke test
>
> 8\. Record incident

# **82. Runbook — Model Failure**

> 1\. Identify adapter/model/version
>
> 2\. Check worker environment
>
> 3\. Check model files/provider
>
> 4\. Check input/provenance
>
> 5\. Fail/retry attempt according to policy
>
> 6\. Switch adapter only through configured deployment decision

Không silently chuyển model trong scientific experiment.

# **83. Observability Correlation**

Recommended correlation chain:

> requestId
>
> → jobId
>
> → attemptId
>
> → contextSnapshotId
>
> → workerId
>
> → candidateId
>
> → assetId

Điều này giúp debug toàn bộ generation pipeline.

# **84. Configuration Versioning**

Critical configuration SHOULD được version hoặc freeze trong release:

- queue settings;

- worker concurrency;

- AI provider;

- model id;

- context schema version;

- adapter version;

- validation profile;

- storage implementation.

# **85. Configuration Drift**

Nếu Thesis Demo và Research Environment dùng config khác nhau, sự khác biệt phải được ghi rõ.

Không được giả định hai environment tương đương chỉ vì cùng code version.

# **86. Deployment Checklist**

## **Infrastructure**

> \[ \] PostgreSQL ready
>
> \[ \] RabbitMQ ready
>
> \[ \] S3-compatible storage ready
>
> \[ \] Reverse proxy ready
>
> \[ \] HTTPS ready

## **Application**

> \[ \] Frontend deployed
>
> \[ \] API deployed
>
> \[ \] Migration applied
>
> \[ \] API readiness OK

## **AI**

> \[ \] Worker deployed
>
> \[ \] Adapter configured
>
> \[ \] Model/provider reachable
>
> \[ \] GenerationContext schema compatible
>
> \[ \] Test Job completed

## **Operations**

> \[ \] Logs available
>
> \[ \] Backup available
>
> \[ \] Rollback path known
>
> \[ \] Demo assets verified

# **87. Release Readiness Gate**

Release không được xem là ready nếu:

- migration fail;

- API cannot access DB;

- Worker không consume Job;

- Candidate không persist được;

- asset storage broken;

- P0 workflow fail;

- canonical data integrity issue tồn tại.

# **88. Thesis Demo Readiness Gate**

Trước demo:

> \[ \] QA-03 critical cases pass
>
> \[ \] QA-04 critical journey pass
>
> \[ \] Demo Project verified
>
> \[ \] Live generation verified
>
> \[ \] Prepared real fallback available
>
> \[ \] Backup created
>
> \[ \] Monitoring/logging accessible
>
> \[ \] No P0 blocker

# **89. Recovery Principle**

OWNIVERSE ưu tiên:

> **Recover durable state, not process memory.**

Process/container có thể restart.

Canon, Version, Snapshot, Job, Candidate và Asset metadata phải tồn tại độc lập với process.

# **90. Operational Principle**

Queue, cache, realtime event và worker process đều là runtime mechanisms.

Không thành phần nào trong số đó được thay thế:

> Canonical Database State.

# **91. AI Deployment Principle**

AI model là replaceable infrastructure capability.

Product workflow không được phụ thuộc cứng vào:

- StoryDiffusion;

- SAMIM;

- external API;

- một GPU provider.

Abstraction chính:

> IImageGenerationService
>
> ITextGenerationService
>
> IConsistencyService

# **92. Prompt Deployment Principle**

OWNIVERSE không deploy một “global raw prompt” như application contract.

Luồng chính:

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
> Adapter-specific Prompt/Condition Builder
>
> ↓
>
> Model Input

Prompt template có thể thay đổi theo Adapter/version mà không thay application API contract.

# **93. Canon Priority at Runtime**

Generation phải giữ priority đã khóa:

> Canonical / Locked Identity
>
> ↓
>
> Timeline / Scene-specific State
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

User instruction không được silently override locked canonical identity.

# **94. Research Identity Memory Boundary**

SAMIM Identity Memory:

- thuộc research image-generation method;

- dùng cho visual identity conditioning.

Application Story Memory:

- thuộc AI-02;

- lưu narrative facts/state/relationships/continuity.

Deployment/monitoring phải giữ hai lớp này tách biệt trong terminology và metadata.

# **95. Final Deployment Architecture**

> Vue Frontend
>
> ↓
>
> ASP.NET Core API
>
> ↓
>
> PostgreSQL ────────────── Canon / Job / Snapshot / Candidate
>
> \|
>
> +── RabbitMQ
>
> \| ↓
>
> \| Python Worker
>
> \| ↓
>
> \| AI Adapter
>
> \| ↓
>
> \| StoryDiffusion / SAMIM / External API
>
> \|
>
> +── IAssetStorage
>
> ↓
>
> Local File / S3-Compatible Storage

SignalR có thể bổ sung progress UX nhưng không thay Job REST state.

# **96. Final Operational Invariants**

Hệ thống phải luôn giữ các invariant sau:

1.  Canon chỉ thay đổi qua Application business workflow.

2.  AI Worker không sửa Canon trực tiếp.

3.  Job state có durable source of truth.

4.  Queue không phải database.

5.  Cache không phải Canon.

6.  Candidate không mặc định là selected output.

7.  Context Snapshot bất biến sau khi generation bắt đầu.

8.  Rendered Prompt không phải application contract.

9.  Retry khác Regeneration.

10. Binary Asset tách khỏi relational structured data.

11. Release phải xác định được code/model/config version.

12. Backup phải có khả năng phục hồi canonical creative work.

# **97. Completion Criteria**

DEPLOY-03 được xem là implement đủ cho MVP khi:

> \[ \] Thesis Demo Environment deploy được lặp lại
>
> \[ \] PostgreSQL persistence ổn định
>
> \[ \] RabbitMQ + Worker pipeline hoạt động
>
> \[ \] S3-compatible asset storage hoạt động
>
> \[ \] API/Worker health visible
>
> \[ \] Generation Job traceable
>
> \[ \] Logs đủ debug critical workflow
>
> \[ \] Database backup thực hiện được
>
> \[ \] Restore drill đã chạy ít nhất một lần
>
> \[ \] Rollback procedure được kiểm tra
>
> \[ \] Demo freeze/version được ghi nhận
>
> \[ \] Core end-to-end workflow pass

# **98. Final Principle**

DEPLOY-03 theo nguyên tắc:

> **Deployment phải có thể tái hiện.**
>
> **Runtime phải có thể quan sát.**
>
> **Failure phải có thể phục hồi.**
>
> **Canon phải được bảo vệ.**
>
> **AI phải có thể thay thế.**
>
> **Thesis Demo phải dựa trên một release đã freeze, không dựa trên trạng thái ngẫu nhiên của development machine.**
