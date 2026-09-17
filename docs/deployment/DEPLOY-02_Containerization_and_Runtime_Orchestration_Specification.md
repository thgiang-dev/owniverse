# **DEPLOY-02 — Containerization & Runtime Orchestration Specification**

**Document ID:** DEPLOY-02  
**Document Type:** Containerization & Runtime Orchestration Specification  
**Product:** OWNIVERSE  
**Project:** Character-Consistent Long-Range Story Generation  
**Primary Roles:** Solution Architect / DevOps Engineer / Backend Engineer / AI Engineer  
**Status:** Draft v1 — Updated Implementation Decision Sync  
**Parent Documents:** ARCH-01, ARCH-02, DEPLOY-01  
**Related Documents:** DEV-01, IMPLEMENT-01, IMPLEMENT-02, AI-01, AI-02, AI-03, DATA-01, DATA-02, API-01, API-02, QA-01, QA-03, DEPLOY-03

# **1. Purpose**

Tài liệu này định nghĩa cách các thành phần runtime của OWNIVERSE được đóng gói, kết nối và vận hành bằng container trong môi trường local development, thesis demo và production-like deployment.

Mục tiêu chính là đảm bảo:

- cùng một kiến trúc logic được sử dụng giữa local và deployment;

- frontend, backend, AI Worker và infrastructure có thể khởi động độc lập;

- AI model/provider có thể thay thế mà không làm thay đổi product workflow;

- background generation tiếp tục hoạt động đúng khi service restart hoặc worker fail;

- configuration, secret và environment-specific dependency không bị hard-code;

- local development vẫn nhẹ đủ để phát triển khi không có GPU.

DEPLOY-02 không biến OWNIVERSE thành microservice architecture. Hệ thống vẫn giữ nguyên định hướng:

> **Modular Monolith Application + Separate AI Worker + External Infrastructure Services.**

# **2. Scope**

DEPLOY-02 bao gồm:

- container boundaries;

- Docker image responsibility;

- Docker Compose runtime topology;

- PostgreSQL container/runtime;

- RabbitMQ container/runtime;

- frontend container;

- ASP.NET Core API container;

- Python AI Worker container;

- Asset Storage runtime modes;

- service discovery;

- environment configuration;

- health/readiness;

- startup/shutdown ordering;

- database migration execution;

- worker concurrency;

- GPU/runtime mode;

- remote AI integration;

- restart and recovery behavior;

- logs and diagnostics;

- Compose profiles;

- runtime compatibility rules.

DEPLOY-02 không định nghĩa:

- cloud vendor-specific production networking;

- Kubernetes;

- multi-region deployment;

- autoscaling policy quy mô lớn;

- CDN strategy chi tiết;

- production IAM platform;

- CI/CD implementation chi tiết.

Các nội dung trên thuộc future deployment evolution hoặc DEPLOY-03 khi applicable.

# **3. Locked Implementation Stack**

Runtime implementation hiện tại được khóa theo:

Frontend

Vue 3 + Vite + TypeScript

Backend

ASP.NET Core Web API

Database

PostgreSQL

Queue

RabbitMQ

AI Runtime

Python AI Worker

Asset Storage

IAssetStorage

├── LocalFileAssetStorage Development

└── S3AssetStorage Demo / Production-like

Realtime UX

SignalR — Optional

Container/runtime configuration SHALL phản ánh đúng stack này.

# **4. Runtime Topology**

Logical runtime:

Browser

↓

Frontend

↓

ASP.NET Core API

├── PostgreSQL

├── RabbitMQ

├── Asset Storage

└── Optional SignalR

↓

Python Worker

↓

AI Adapter / Model

Backend Application là owner của canonical business state.

AI Worker chỉ xử lý job/inference và không được bypass Application business rules để trực tiếp thay đổi Canon.

# **5. Container Boundaries**

OWNIVERSE sử dụng các application container chính:

owniverse-frontend

owniverse-api

owniverse-ai-worker

Infrastructure container trong local environment:

postgres

rabbitmq

Optional local services có thể bổ sung nếu implementation cần, nhưng không được làm thay đổi contract kiến trúc chính.

# **6. Frontend Container**

Frontend container chứa Vue 3 application.

Development có thể chạy Vite dev server trực tiếp ngoài Docker nếu thuận tiện.

Containerized build SHOULD sử dụng multi-stage pattern:

Node build stage

↓

Static production bundle

↓

Lightweight web server / reverse proxy runtime

Frontend không chứa:

- database credential;

- RabbitMQ credential;

- AI provider secret;

- S3 secret key;

- model secret.

Frontend chỉ giao tiếp qua public API/realtime endpoint được backend expose.

# **7. ASP.NET Core API Container**

API container chịu trách nhiệm:

- REST API /api/v1;

- authentication/authorization integration;

- business use cases;

- canonical data changes;

- Context Snapshot creation;

- Job creation;

- queue publishing;

- Candidate selection;

- history/query;

- Asset metadata;

- SignalR hub nếu realtime được bật.

API container SHALL NOT chứa heavy model inference runtime.

# **8. Python AI Worker Container**

Worker container chịu trách nhiệm:

RabbitMQ Job

↓

Load Immutable Context Snapshot

↓

Resolve AI Adapter

↓

Build Model-specific Input

↓

Run AI Inference / External API

↓

Normalize Output

↓

Validation

↓

Persist Asset / Candidate / Execution Metadata

↓

Update Job State

Worker không được tự quyết định thay đổi canonical Story/Character/Scene state.

# **9. PostgreSQL Runtime**

PostgreSQL là relational data store chính.

Nó lưu:

- canonical domain state;

- versions;

- Context Snapshots;

- generation Jobs;

- Attempts;

- Candidates;

- Asset metadata;

- validation metadata;

- provenance;

- history.

Binary image/file content không lưu trực tiếp trong PostgreSQL trừ trường hợp technical metadata nhỏ.

# **10. RabbitMQ Runtime**

RabbitMQ là queue implementation chính cho asynchronous job dispatch.

Nó được chọn cho communication giữa:

ASP.NET Core Producer

↓

RabbitMQ

↓

Python Worker Consumer

RabbitMQ không phải source of truth của Job.

> **Database Job state mới là durable source of truth.**

Queue chỉ là execution transport.

# **11. Asset Storage Runtime**

Storage luôn được truy cập qua:

IAssetStorage

Application không phụ thuộc trực tiếp vào vendor.

Runtime mode:

Development

→ LocalFileAssetStorage

Thesis Demo / Production-like

→ S3AssetStorage

S3-compatible provider hiện tại có thể là Cloudflare R2.

Provider có thể thay đổi mà không thay business/domain code.

# **12. Local File Storage Container Behavior**

Trong development:

- file có thể lưu vào mounted Docker volume;

- path không được hard-code trong domain logic;

- metadata vẫn nằm trong PostgreSQL;

- API/Worker sử dụng cùng IAssetStorage contract.

Ví dụ conceptual volume:

owniverse-assets:/app/data/assets

Local filesystem mode chỉ là implementation environment, không phải domain rule.

# **13. S3-compatible Storage Behavior**

Trong demo/production-like:

S3AssetStorage sử dụng:

- endpoint;

- bucket;

- access credential;

- region/config nếu provider yêu cầu.

Các giá trị này MUST lấy từ configuration/secret.

Không hard-code Cloudflare R2-specific behavior vào Application Layer.

# **14. Docker Compose Role**

Docker Compose được sử dụng để dựng reproducible runtime cho development/demo nhỏ.

Nó chịu trách nhiệm:

- network;

- environment variables;

- volumes;

- health checks;

- dependency wiring;

- profiles;

- optional resource settings.

Docker Compose không biến container dependency thành business dependency.

# **15. Recommended Compose Services**

Conceptual:

services:

frontend:

api:

ai-worker:

postgres:

rabbitmq:

S3-compatible external provider không bắt buộc chạy thành local container.

Development mặc định sử dụng LocalFileAssetStorage.

# **16. Compose Profiles**

Recommended profiles:

infrastructure

mock-ai

remote-ai

full-local

# **17. infrastructure Profile**

Mục đích:

> chạy infrastructure trong Docker, app có thể chạy từ IDE.

Bao gồm tối thiểu:

PostgreSQL

RabbitMQ

Backend/frontend/worker có thể chạy ngoài container.

Đây là mode thuận tiện cho coding/debugging hằng ngày.

# **18. mock-ai Profile**

Mục đích:

> chạy full product workflow mà không cần GPU/model thật.

Worker sử dụng:

MockImageGenerationAdapter

MockTextGenerationAdapter nếu cần

Mock mode vẫn phải đi qua:

Job

→ RabbitMQ

→ Worker

→ Adapter

→ Candidate

Không bypass production pipeline chỉ vì đang mock.

# **19. remote-ai Profile**

Mục đích:

> Application/Worker chạy local hoặc container nhưng inference được gửi đến remote GPU/API.

Ví dụ:

Python Worker

↓

Remote StoryDiffusion / SAMIM Runtime

hoặc:

Python Worker

↓

External Image API

Product workflow không thay đổi.

# **20. full-local Profile**

Mục đích:

> chạy toàn bộ system locally, bao gồm local model nếu machine đủ tài nguyên.

Bao gồm:

Frontend

API

PostgreSQL

RabbitMQ

AI Worker

Local AI Model

LocalFileAssetStorage

Mode này không phải requirement để mọi developer sử dụng.

# **21. Service Network**

Các runtime services nên sử dụng private Compose network.

Ví dụ logical DNS:

postgres

rabbitmq

api

ai-worker

Frontend/browser chỉ truy cập public/reverse-proxied endpoint cần thiết.

Database và RabbitMQ không nên expose public trong deployment environment.

# **22. Service Discovery**

Container sử dụng service name thay vì hard-coded localhost.

Ví dụ:

DATABASE_HOST=postgres

RABBITMQ_HOST=rabbitmq

Khi application chạy ngoài Docker, environment config có thể đổi thành localhost hoặc target host tương ứng.

# **23. Environment Configuration**

Mọi environment-specific value phải externalized.

Các nhóm config chính:

Database

RabbitMQ

Asset Storage

AI Provider

Model

Worker

Logging

Realtime

# **24. Suggested Environment Variables**

Ví dụ:

ASPNETCORE_ENVIRONMENT

DATABASE_URL

RABBITMQ_URL

ASSET_STORAGE_PROVIDER

ASSET_STORAGE_LOCAL_PATH

S3_ENDPOINT

S3_BUCKET

S3_ACCESS_KEY

S3_SECRET_KEY

AI_PROVIDER

MODEL_ID

AI_ENDPOINT

WORKER_CONCURRENCY

MAX_RETRIES

SIGNALR_ENABLED

Tên biến cuối cùng có thể thay đổi theo implementation nhưng ý nghĩa contract phải giữ ổn định.

# **25. Secret Management**

Secret không được commit vào source control.

Bao gồm:

- DB password;

- RabbitMQ password;

- S3 credential;

- external AI API key;

- authentication signing key.

Local development có thể sử dụng .env bị ignore hoặc secret manager phù hợp.

# **26. Image Build Strategy**

Application images SHOULD sử dụng multi-stage build khi phù hợp.

Mục tiêu:

- giảm image size;

- tách build dependency khỏi runtime;

- tăng reproducibility;

- giảm attack surface.

# **27. Image Tagging**

Không chỉ sử dụng latest cho demo/release quan trọng.

Recommended metadata:

frontend:\<version-or-commit\>

api:\<version-or-commit\>

ai-worker:\<version-or-commit\>

Thesis demo nên freeze exact tags.

# **28. Runtime Version Compatibility**

Một deployment release cần theo dõi compatibility giữa:

Frontend

API

Worker

Database Migration

GenerationContext Schema

AI Adapter

Model/Method

Validation Profile

Không giả định mọi version luôn tương thích.

# **29. GenerationContext Compatibility**

Worker phải biết contextSchemaVersion của Snapshot.

Ví dụ:

GenerationContextV1

Nếu worker không hỗ trợ schema version:

- không tự đoán;

- không silently drop field;

- Job phải fail bằng error rõ ràng hoặc route tới compatible worker.

# **30. Prompt/Conditioning Runtime Rule**

Raw prompt string không phải runtime contract chính.

Runtime contract:

Immutable GenerationContext Snapshot

↓

Adapter-specific Prompt / Condition Builder

↓

Model Input

Điều này cho phép StoryDiffusion, SAMIM và external API có cách rendering/conditioning khác nhau.

# **31. Database Migration Strategy**

Migration phải được chạy có kiểm soát trước khi API mới phụ thuộc schema mới.

Recommended deployment order:

Backup when required

↓

Run migration

↓

Verify migration

↓

Start/update API

↓

Start/update Worker

↓

Start/update Frontend

Không để nhiều API container tự chạy destructive migration đồng thời.

# **32. Migration Execution Mode**

Migration có thể chạy bằng:

- dedicated migration command;

- one-shot migration container;

- controlled deployment step.

Cách cuối cùng tùy implementation, nhưng migration execution phải rõ và repeatable.

# **33. Startup Dependencies**

Container startup order không đủ để đảm bảo service ready.

Phải phân biệt:

Started

≠

Ready

API chỉ nên xử lý request phụ thuộc DB/queue khi dependency ready.

Worker chỉ claim job khi DB/queue/config/adapter runtime ready.

# **34. Health Checks**

Mỗi service cần health signal phù hợp.

Frontend:

HTTP response available

API:

process health

Worker:

worker process health

Infrastructure:

PostgreSQL accepts connection

RabbitMQ ready

# **35. Readiness Checks**

Readiness có thể nghiêm ngặt hơn health.

Ví dụ API readiness:

Database reachable

Required migration compatible

RabbitMQ reachable when generation enabled

Worker readiness:

RabbitMQ reachable

Database reachable if needed

Adapter configuration valid

Required model/provider available

# **36. Liveness vs Dependency Failure**

Một dependency fail tạm thời không nhất thiết có nghĩa process phải bị restart liên tục.

Ví dụ external AI provider down:

- worker process vẫn healthy;

- adapter/provider readiness có thể degraded;

- Job fail/retry theo policy.

Tránh restart storm.

# **37. Worker Concurrency**

Worker concurrency phải externalized bằng configuration.

Ví dụ:

WORKER_CONCURRENCY=1

cho GPU nhỏ.

Không hard-code parallel inference.

# **38. GPU Runtime**

GPU Worker có thể chạy:

- local GPU;

- remote GPU machine;

- temporary notebook/runtime;

- external provider.

Application architecture không phụ thuộc GPU location.

# **39. GPU Resource Safety**

Đối với local/full model runtime:

- concurrency nên thấp;

- không chạy nhiều heavy model process vô kiểm soát;

- OOM phải được classify thành technical execution failure;

- Job state phải được cập nhật rõ;

- previous Candidate/Canon không bị ảnh hưởng.

# **40. Remote GPU Boundary**

Remote GPU mode không được cho remote runtime quyền truy cập trực tiếp canonical database nếu không cần thiết.

Preferred flow:

Worker

↓

Prepared Model Input / Authorized Asset Access

↓

Remote Inference

↓

Normalized Result

↓

Application Persistence

# **41. External API Boundary**

External provider được đặt sau adapter.

Ví dụ:

IImageGenerationService

↓

ExternalImageApiAdapter

Provider-specific request/response không leak vào application domain.

# **42. RabbitMQ Acknowledgement Rule**

Worker chỉ acknowledge message khi đã nhận trách nhiệm xử lý theo runtime contract.

Nếu worker crash trước completion:

- queue có thể redeliver/requeue;

- database Job state dùng để xác định việc tiếp tục/recover;

- duplicate execution phải được phòng tránh bằng idempotency/state checks.

# **43. Job Source of Truth**

Không suy ra trạng thái Job chỉ từ RabbitMQ queue state.

Canonical execution state nằm trong persistence:

GenerationJob

GenerationAttempt

Candidate

Validation Result

Queue message có thể mất/replay mà business history vẫn phải giải thích được.

# **44. Idempotent Worker Claim**

Khi worker nhận một Job:

- kiểm tra Job tồn tại;

- kiểm tra state hợp lệ;

- claim bằng concurrency-safe mechanism;

- không chạy lại completed Job như một generation mới;

- retry technical failure theo Attempt semantics.

# **45. Retry vs Regeneration**

Container/runtime recovery không được biến technical retry thành creative regeneration.

Retry

= same creative Job, new technical Attempt when allowed

Regeneration

= new creative request/Job initiated by application/user

# **46. Graceful Shutdown — API**

Khi API container shutdown:

- ngừng nhận request mới;

- hoàn tất request đang xử lý khi có thể;

- không publish message nửa chừng ngoài transaction design;

- flush logs;

- release resources.

# **47. Graceful Shutdown — Worker**

Khi Worker shutdown:

- ngừng claim Job mới;

- nếu đang inference, xử lý theo configured grace period;

- nếu không thể hoàn tất, Job/Attempt phải có trạng thái recoverable;

- không silently đánh dấu completed.

# **48. Restart Safety**

Restart bất kỳ container nào không được gây mất:

- canonical Story data;

- Context Snapshot;

- persisted Job;

- Candidate đã hoàn tất;

- Asset metadata;

- generation history.

Persistent state không được dựa vào container filesystem ephemeral trừ LocalFileAssetStorage volume development đã cấu hình rõ.

# **49. Persistent Volumes**

Local Compose cần persistent volume tối thiểu cho:

PostgreSQL data

Development asset files

RabbitMQ persistence có thể được bật phù hợp runtime, nhưng database Job state vẫn là system of record.

# **50. Container Filesystem Rule**

Không lưu permanent output vào ephemeral container layer.

Generated asset phải đi qua IAssetStorage.

Temporary inference files phải:

- nằm trong temp path;

- có cleanup policy;

- không được xem là Asset canonical.

# **51. Logging**

Container log nên ghi ra stdout/stderr theo structured format khi có thể.

Generation-related log nên có correlation fields:

requestId

jobId

attemptId

candidateId

workerId

adapter

modelId

Không log story text/reference image payload toàn phần nếu không cần thiết.

# **52. Sensitive Logging Rule**

Không log:

- API keys;

- storage secrets;

- auth tokens;

- private signed URLs;

- raw credentials.

Prompt/model input provenance nếu lưu phải có access control phù hợp vì có thể chứa story content.

# **53. SignalR Runtime**

SignalR là optional realtime enhancement.

Nếu bật:

Worker/Backend durable state update

↓

Backend notification

↓

SignalR event

↓

Frontend refresh/reconcile

SignalR không phải source of truth.

Frontend vẫn phải có khả năng query Job qua REST.

# **54. Notification After Commit**

Realtime event chỉ nên được phát sau khi durable state cần thiết đã commit.

Không gửi JobCompleted trước khi Candidate/Asset metadata thực sự persisted.

# **55. Local Development Startup Sequence**

Recommended:

1\. PostgreSQL

2\. RabbitMQ

3\. Database Migration

4\. ASP.NET Core API

5\. Python Worker

6\. Frontend

Nếu dùng remote AI/provider, Worker readiness còn phụ thuộc provider config.

# **56. Full Local Startup Sequence**

Nếu model chạy local:

Infrastructure

↓

Migration

↓

API

↓

Model Runtime / Worker

↓

Frontend

↓

Generation Smoke Test

Không đánh dấu environment ready chỉ vì container state là running.

# **57. Mock Runtime Startup**

Mock mode ưu tiên cho early Sprint:

PostgreSQL

RabbitMQ

API

Mock Worker

Frontend

Mục tiêu là test complete generation infrastructure trước khi model thật được tích hợp.

# **58. Smoke Test**

Sau container startup, chạy ít nhất:

Create/Open Project

↓

Create Character

↓

Create Scene

↓

Generate

↓

Job enters Queue

↓

Worker handles Job

↓

Candidate created

↓

Select Candidate

↓

Reload

Mock mode có thể dùng cho smoke test thường xuyên.

# **59. Infrastructure Smoke Checks**

Ngoài product flow:

API → PostgreSQL

API → RabbitMQ

Worker → RabbitMQ

Worker → Snapshot

Worker → Asset Storage

Frontend → API

mỗi path cần được verify.

# **60. Failure Scenario — API Restart**

Expected:

- existing Job không mất;

- Worker có thể tiếp tục Job đã claim nếu contract cho phép;

- frontend reload state qua REST;

- queued Job tiếp tục sau API recovery.

# **61. Failure Scenario — Worker Restart**

Expected:

- uncompleted Job không bị đánh dấu success;

- Attempt có trạng thái rõ;

- requeue/reclaim theo retry policy;

- duplicate Candidate không được tạo silently.

# **62. Failure Scenario — RabbitMQ Restart**

Expected:

- API/Worker detect queue unavailable;

- durable Job state vẫn tồn tại trong DB;

- recovery/re-dispatch strategy có thể chạy sau queue return;

- user nhận trạng thái rõ thay vì mất request.

# **63. Failure Scenario — Storage Failure**

Nếu Asset Storage không ghi được generated output:

- Candidate không được xem completed;

- Job không được đánh dấu completed;

- failure được log/classify;

- Canon không bị ảnh hưởng.

# **64. Failure Scenario — Database Failure**

Nếu database unavailable:

- API không tiếp tục canonical write;

- Worker không tự lưu trạng thái chỉ trong memory rồi báo success;

- readiness phải phản ánh dependency failure;

- retry/recovery phải an toàn.

# **65. Failure Scenario — AI Provider Failure**

Nếu model/API fail:

Job

→ Attempt Failed

→ Retry if transient and allowed

→ Job Failed when policy exhausted

Không tự động tạo creative regeneration.

# **66. Model Change Runtime Rule**

Đổi model/provider không được yêu cầu thay:

- Scene API;

- Candidate workflow;

- Job query contract;

- Canonical domain structure.

Thay đổi phải nằm sau adapter/configuration boundary.

# **67. Research Adapter Runtime**

Research integration sử dụng cùng Worker pipeline.

IImageGenerationService

├── MockImageGenerationAdapter

├── StoryDiffusionAdapter

├── ProposedMethodAdapter / SAMIM

└── ExternalImageApiAdapter

SAMIM không được tạo deployment path đặc biệt phá vỡ product architecture.

# **68. Provenance Runtime Metadata**

Generation execution SHOULD persist:

contextSchemaVersion

promptTemplateVersion or conditionBuilderVersion

adapterVersion

modelId

methodVersion

seed when applicable

workerVersion

Mục tiêu:

- reproducibility;

- debugging;

- research comparison;

- thesis evidence.

# **69. Runtime Compatibility Failure**

Nếu Adapter/Worker không hỗ trợ Context hoặc method version:

system phải fail explicitly.

Không:

Ignore unknown fields

↓

Generate anyway

nếu điều đó có thể phá reproducibility/consistency.

# **70. Container Resource Limits**

Development/demo SHOULD cấu hình resource limit khi hữu ích.

Đặc biệt AI Worker cần quan tâm:

- memory;

- shared memory;

- GPU VRAM;

- worker concurrency;

- temp disk.

Không áp dụng limit cứng giống nhau cho mọi machine.

# **71. CPU-only Mode**

Worker có thể có CPU/mock mode cho development.

Heavy baseline/SAMIM không bắt buộc chạy CPU nếu không thực tế.

Architecture không được ép developer phải có GPU để phát triển Project/Story/Character/Scene/Job workflow.

# **72. Thesis Demo Runtime**

Demo deployment ưu tiên:

- reproducibility;

- stability;

- known model version;

- known Worker image;

- known migration;

- known Validation Profile;

- preflight smoke test.

Không thay model/container tag ngay trước demo nếu chưa regression test.

# **73. Demo Storage**

Thesis Demo nên sử dụng S3-compatible storage thay vì phụ thuộc local developer filesystem.

Lợi ích:

- Worker/API có thể ở machine khác nhau;

- asset path ổn định;

- gần production topology hơn;

- giảm phụ thuộc vào một container volume đơn lẻ.

# **74. Demo Queue**

RabbitMQ phải được coi là infrastructure dependency rõ ràng.

Trước demo cần kiểm tra:

Queue reachable

No unexpected backlog

Worker consuming

Retry/dead messages understood

# **75. Demo Worker**

Trước demo:

- warm up model nếu applicable;

- verify adapter version;

- verify storage write;

- verify one real generation;

- verify prepared fallback Candidate theo QA-04 nếu live inference có sự cố.

Prepared fallback không được dùng để giả rằng live generation đã thành công.

# **76. Deployment Independence**

Container architecture không khóa OWNIVERSE vào:

- AWS;

- Azure;

- GCP;

- Cloudflare.

Cloudflare R2 chỉ là S3-compatible provider ưu tiên hiện tại cho demo/production-like storage.

# **77. Kubernetes Decision**

Kubernetes không cần thiết cho MVP/thesis.

Không triển khai chỉ để tăng độ phức tạp kỹ thuật.

Chỉ xem xét nếu system scale requirement thực tế xuất hiện sau MVP.

# **78. Microservice Decision**

Containerization không đồng nghĩa với microservices.

ASP.NET Core backend vẫn là modular monolith.

Không split Project/Story/Character/Scene thành nhiều network service trong MVP.

# **79. CI Consideration**

CI có thể build:

- frontend image;

- API image;

- Worker image;

và chạy non-GPU test.

Full GPU inference không bắt buộc trên mỗi commit.

# **80. Container Security Baseline**

Application container SHOULD:

- dùng minimal runtime image hợp lý;

- tránh root khi practical;

- không bake secret vào image;

- không expose unnecessary port;

- pin/version dependency quan trọng;

- scan dependency/image nếu tooling cho phép.

# **81. RabbitMQ Security**

Deployment environment phải:

- dùng credential riêng;

- không dùng guest/public defaults;

- giới hạn network access;

- không expose management UI công khai nếu không cần.

# **82. PostgreSQL Security**

Database deployment phải:

- không expose public tùy tiện;

- dùng strong credential;

- backup theo DEPLOY-03;

- hạn chế app credential đúng quyền cần thiết.

# **83. Asset Storage Security**

S3 bucket/object access không mặc định public.

Access nên đi qua:

- backend-authorized access;

- signed URL nếu implementation sử dụng;

- provider policy phù hợp.

Character references và generated assets có thể chứa private creative content.

# **84. Debugging Strategy**

Khi runtime lỗi, kiểm tra theo path:

Frontend

↓

API

↓

Database / RabbitMQ

↓

Worker

↓

Adapter / Model

↓

Asset Storage

Không bắt đầu bằng việc thay model nếu lỗi thực tế nằm ở queue hoặc storage.

# **85. Common Diagnostic Questions**

Khi Job stuck:

Job có tồn tại trong DB?

Job state là gì?

Message đã publish chưa?

RabbitMQ có message không?

Worker có connected không?

Worker có claim không?

Attempt state là gì?

Model/provider có ready không?

Asset write có thành công không?

Candidate đã persisted chưa?

# **86. Runtime State Principle**

Container state không được dùng thay business state.

Ví dụ:

Worker container = running

không đồng nghĩa:

Generation Job = healthy

Job state phải được query từ Application persistence.

# **87. Rebuild Rule**

Infrastructure/config change nên có reproducible command/documented path.

Không dựa vào manual one-off changes bên trong running container.

Nếu container bị recreate, environment phải có thể dựng lại từ source/config/volume/secret.

# **88. Development Convenience Rule**

Không bắt buộc containerize mọi process trong lúc coding.

Developer có thể:

PostgreSQL + RabbitMQ → Docker

API → IDE

Frontend → Vite dev server

Worker → Python environment

miễn vẫn tuân theo cùng contract/configuration.

# **89. Production-like Validation**

Trước Sprint 10/Demo Freeze, phải chạy ít nhất một lần full containerized or equivalent deployment path.

Mục tiêu phát hiện:

- localhost assumptions;

- missing env variables;

- volume/path bug;

- service discovery bug;

- migration issue;

- queue connectivity issue;

- storage configuration issue.

# **90. Definition of Done — DEPLOY-02**

DEPLOY-02 implementation được xem là đủ cho MVP khi:

\[ \] Frontend image/container can run

\[ \] API image/container can run

\[ \] Worker image/container can run

\[ \] PostgreSQL persists data

\[ \] RabbitMQ connects API ↔ Worker

\[ \] LocalFileAssetStorage works in development

\[ \] S3AssetStorage can be configured for demo environment

\[ \] Migration path is reproducible

\[ \] Health/readiness exists

\[ \] Mock generation works through queue

\[ \] Real adapter can use the same pipeline

\[ \] Container restart does not corrupt canonical state

\[ \] Job recovery behavior is testable

# **91. Final Runtime Model**

OWNIVERSE runtime có thể nhớ ngắn gọn như sau:

Vue Frontend

↓

ASP.NET Core Modular Monolith

↓

PostgreSQL + RabbitMQ + IAssetStorage

↓

Python AI Worker

↓

Model / Research Method / API

Trong đó:

> **Database giữ durable application state.**
>
> **RabbitMQ vận chuyển asynchronous work.**
>
> **Context Snapshot giữ immutable AI input.**
>
> **Adapter cô lập model/provider-specific behavior.**
>
> **Asset Storage giữ binary output bên ngoài relational database.**
>
> **Container chỉ là runtime packaging, không thay đổi domain ownership.**

# **92. Final Principle**

Nguyên tắc cốt lõi của DEPLOY-02:

> **Containerize for reproducibility, not for unnecessary distribution.**
>
> **Queue for asynchronous execution, database for truth.**
>
> **Worker executes AI; Application owns Canon.**
>
> **Storage, model and GPU location remain replaceable infrastructure choices.**
>
> **The same Job → Snapshot → Worker → Adapter → Candidate workflow must survive local, remote and thesis-demo environments.**
